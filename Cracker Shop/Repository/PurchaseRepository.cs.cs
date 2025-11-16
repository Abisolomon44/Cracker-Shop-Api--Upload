using Cracker_Shop.Code_Generator;
using Cracker_Shop.Models.MasterModels;
using Cracker_Shop.Repository.IRepository;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cracker_Shop.Models.Purchase;
namespace Cracker_Shop.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly IDbConnection _db;

        public PurchaseRepository(IDbConnection db)
        {
            _db = db;
        }
        public async Task<long> AddUpDateDeletePurchaseOrderAsync(PurchaseOrderEntry po)
        {
            if (po == null)
                throw new ArgumentNullException(nameof(po));

            if (po.CompanyID == null || po.CompanyID <= 0)
                throw new ArgumentException("CompanyID is required.");

            if (po.SupplierID == null || po.SupplierID <= 0)
                throw new ArgumentException("SupplierID is required.");

            // Trim PO Number
            po.PONumber = po.PONumber?.Trim();

            // ⭐ Generate PO number BEFORE starting transaction
            if ((po.POID == 0 || po.POID == null) && string.IsNullOrWhiteSpace(po.PONumber))
            {
                po.PONumber = await CodeGenerator.GenerateNextCodeAsync(
                    _db, "PurchaseOrderEntry", "PONumber", "PO", 5
                );
            }

            if (_db.State == ConnectionState.Closed)
                _db.Open();

            using (var transaction = _db.BeginTransaction())
            {
                try
                {
                    // ---------------------------------------------
                    // 1) DUPLICATE CHECK
                    // ---------------------------------------------
                    const string duplicateSql = @"
                SELECT COUNT(1)
                FROM PurchaseOrderEntry
                WHERE LTRIM(RTRIM(UPPER(PONumber))) = UPPER(@PONumber)
                  AND (@POID = 0 OR POID <> @POID)
                  AND CompanyID = @CompanyID
                  AND BranchID = @BranchID
                  AND IsActive = 1";

                    var dupCount = await _db.ExecuteScalarAsync<int>(duplicateSql,
                        new
                        {
                            po.PONumber,
                            po.POID,
                            po.CompanyID,
                            po.BranchID
                        }, transaction);

                    if (dupCount > 0)
                        throw new InvalidOperationException("A purchase order with the same number already exists.");

                    // ---------------------------------------------
                    // 2) LOAD LOOKUP NAMES (IMPORTANT)
                    // ---------------------------------------------
                    po.CompanyName = await _db.ExecuteScalarAsync<string>(
                        "SELECT CompanyName FROM CompanyMaster WHERE CompanyID = @CompanyID",
                        new { po.CompanyID }, transaction);

                    po.BranchName = await _db.ExecuteScalarAsync<string>(
                        "SELECT BranchName FROM BranchMaster WHERE BranchID = @BranchID",
                        new { po.BranchID }, transaction);

                    po.SupplierName = await _db.ExecuteScalarAsync<string>(
                        "SELECT SupplierName FROM SupplierMaster WHERE SupplierID = @SupplierID",
                        new { po.SupplierID }, transaction);

                    po.StatusName = await _db.ExecuteScalarAsync<string>(
                        "SELECT StatusName FROM StatusMaster WHERE StatusID = @StatusID",
                        new { po.StatusID }, transaction);

                    po.ProductCategoryName = await _db.ExecuteScalarAsync<string>(
                        "SELECT CategoryName FROM CategoryMaster WHERE CategoryID = @ProductCategoryId",
                        new { po.ProductCategoryId }, transaction);

                    po.ProductSubCategoryName = await _db.ExecuteScalarAsync<string>(
                        "SELECT SubCategoryName FROM SubCategoryMaster WHERE SubCategoryID = @ProductSubCategory",
                        new { po.ProductSubCategory }, transaction);


                    long resultId;

                    // ---------------------------------------------
                    // 3) INSERT
                    // ---------------------------------------------
                    if (po.POID == 0 || po.POID == null)
                    {
                        const string sqlInsert = @"
                    INSERT INTO PurchaseOrderEntry
                    (CompanyID, CompanyName, BranchID, BranchName, PONumber, PODate,
                     SupplierID, SupplierName, StatusID, StatusName, TotalAmount, PORemarks,
                     ProductID, ProductCode, ProductName, ProductCategoryId, ProductCategoryName,
                     ProductSubCategory, ProductSubCategoryName, PORate, OrderedQty, ApprovedQty,
                     ExpectedDeliveryDate, ProductRemarks, IsActive, CreatedByUserID, CreatedSystemName,
                     CreatedAt, AccountingYear)
                    VALUES
                    (@CompanyID, @CompanyName, @BranchID, @BranchName, @PONumber, @PODate,
                     @SupplierID, @SupplierName, @StatusID, @StatusName, @TotalAmount, @PORemarks,
                     @ProductID, @ProductCode, @ProductName, @ProductCategoryId, @ProductCategoryName,
                     @ProductSubCategory, @ProductSubCategoryName, @PORate, @OrderedQty, @ApprovedQty,
                     @ExpectedDeliveryDate, @ProductRemarks, 1, @CreatedByUserID, @CreatedSystemName,
                     SYSDATETIME(), @AccountingYear);

                    SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                        resultId = await _db.ExecuteScalarAsync<long>(sqlInsert, po, transaction);
                    }
                    else if (po.IsActive == false)
                    {
                        // ---------------------------------------------
                        // 4) DELETE  (DEACTIVATE)
                        // ---------------------------------------------
                        const string sqlDeactivate = @"
                    UPDATE PurchaseOrderEntry SET
                        IsActive = 0,
                        CancelledDate = SYSDATETIME(),
                        CancelledBy = @CancelledBy,
                        CancelReason = @CancelReason,
                        UpdatedByUserID = @UpdatedByUserID,
                        UpdatedSystemName = @UpdatedSystemName,
                        UpdatedAt = SYSDATETIME()
                    WHERE POID = @POID";

                        await _db.ExecuteAsync(sqlDeactivate, po, transaction);
                        resultId = po.POID ?? 0;
                    }
                    else
                    {
                        // ---------------------------------------------
                        // 5) UPDATE
                        // ---------------------------------------------
                        const string sqlUpdate = @"
                    UPDATE PurchaseOrderEntry SET
                        CompanyID = @CompanyID,
                        CompanyName = @CompanyName,
                        BranchID = @BranchID,
                        BranchName = @BranchName,
                        SupplierID = @SupplierID,
                        SupplierName = @SupplierName,
                        StatusID = @StatusID,
                        StatusName = @StatusName,
                        TotalAmount = @TotalAmount,
                        PORemarks = @PORemarks,
                        ProductID = @ProductID,
                        ProductCode = @ProductCode,
                        ProductName = @ProductName,
                        ProductCategoryId = @ProductCategoryId,
                        ProductCategoryName = @ProductCategoryName,
                        ProductSubCategory = @ProductSubCategory,
                        ProductSubCategoryName = @ProductSubCategoryName,
                        PORate = @PORate,
                        OrderedQty = @OrderedQty,
                        ApprovedQty = @ApprovedQty,
                        ExpectedDeliveryDate = @ExpectedDeliveryDate,
                        ProductRemarks = @ProductRemarks,
                        UpdatedByUserID = @UpdatedByUserID,
                        UpdatedSystemName = @UpdatedSystemName,
                        UpdatedAt = SYSDATETIME()
                    WHERE POID = @POID";

                        await _db.ExecuteAsync(sqlUpdate, po, transaction);
                        resultId = po.POID ?? 0;
                    }

                    transaction.Commit();
                    return resultId;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }



        public async Task<long> AddUpdateDeleteGRNAsync(GRNEntry grn)
        {
            if (string.IsNullOrWhiteSpace(grn.GRNNumber))
                throw new ArgumentException("GRN number is required.");

            if (grn.CompanyID == null || grn.CompanyID <= 0)
                throw new ArgumentException("CompanyID is required.");


            grn.GRNNumber = Regex.Replace(grn.GRNNumber.Trim(), @"\s+", " ");

            if (_db.State == ConnectionState.Closed)
                _db.Open(); // ✅ Ensure connection is open
            using (var transaction = _db.BeginTransaction())
            {
                try
                {
                    // 🔹 Check for duplicates
                    const string duplicateSql = @"
                SELECT COUNT(1)
                FROM GRNEntry
                WHERE LTRIM(RTRIM(UPPER(GRNNumber))) = UPPER(@GRNNumber)
                  AND (@GRNEntryID = 0 OR GRNEntryID <> @GRNEntryID)
                  AND CompanyID = @CompanyID
                  AND IsActive = 1";

                    var exists = await _db.ExecuteScalarAsync<int>(duplicateSql, new
                    {
                        grn.GRNNumber,
                        grn.GRNEntryID,
                        grn.CompanyID
                    }, transaction);

                    if (exists > 0)
                        throw new InvalidOperationException("A GRN with the same number already exists.");

                    long resultId;

                    // 🔹 INSERT
                    if (grn.GRNEntryID == 0 || grn.GRNEntryID == null)
                    {
                        grn.IsActive = true;
                        grn.CreatedAt = DateTime.Now;

                        const string insertSql = @"
                    INSERT INTO GRNEntry
                    (
                        GRNNumber, GRNDate, POID, PODetailID, PurchaseID,
                        SupplierID, SupplierName, CompanyID, BranchID,
                        InvoiceNumber, InvoiceDate, TransportName, VehicleNumber, ReceivedBy,
                        ProductID, ProductCode, ProductName, UnitID,
                        ReceivedQty, AcceptedQty, RejectedQty, PurchaseRate,
                        TaxPercentage, TaxAmount, TotalAmount,
                        Remarks, IsApproved, ApprovedBy, ApprovedAt,
                        IsActive, CreatedBy, CreatedAt
                    )
                    VALUES
                    (
                        @GRNNumber, @GRNDate, @POID, @PODetailID, @PurchaseID,
                        @SupplierID, @SupplierName, @CompanyID, @BranchID,
                        @InvoiceNumber, @InvoiceDate, @TransportName, @VehicleNumber, @ReceivedBy,
                        @ProductID, @ProductCode, @ProductName, @UnitID,
                        @ReceivedQty, @AcceptedQty, @RejectedQty, @PurchaseRate,
                        @TaxPercentage, @TaxAmount, @TotalAmount,
                        @Remarks, @IsApproved, @ApprovedBy, @ApprovedAt,
                        @IsActive, @CreatedBy, @CreatedAt
                    );

                    SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                        resultId = await _db.ExecuteScalarAsync<long>(insertSql, grn, transaction);
                    }
                    // 🔹 DEACTIVATE
                    else if (grn.IsActive == false)
                    {
                        const string deactivateSql = @"
                    UPDATE GRNEntry
                    SET IsActive = 0,
                        UpdatedBy = @UpdatedBy,
                        UpdatedAt = SYSDATETIME()
                    WHERE GRNEntryID = @GRNEntryID";

                        await _db.ExecuteAsync(deactivateSql, grn, transaction);
                        resultId = grn.GRNEntryID ?? 0;
                    }
                    // 🔹 UPDATE
                    else
                    {
                        grn.UpdatedAt = DateTime.Now;

                        const string updateSql = @"
                    UPDATE GRNEntry
                    SET
                        GRNNumber       = @GRNNumber,
                        GRNDate         = @GRNDate,
                        POID            = @POID,
                        PODetailID      = @PODetailID,
                        PurchaseID      = @PurchaseID,
                        SupplierID      = @SupplierID,
                        SupplierName    = @SupplierName,
                        CompanyID       = @CompanyID,
                        BranchID        = @BranchID,
                        InvoiceNumber   = @InvoiceNumber,
                        InvoiceDate     = @InvoiceDate,
                        TransportName   = @TransportName,
                        VehicleNumber   = @VehicleNumber,
                        ReceivedBy      = @ReceivedBy,
                        ProductID       = @ProductID,
                        ProductCode     = @ProductCode,
                        ProductName     = @ProductName,
                        UnitID          = @UnitID,
                        ReceivedQty     = @ReceivedQty,
                        AcceptedQty     = @AcceptedQty,
                        RejectedQty     = @RejectedQty,
                        PurchaseRate    = @PurchaseRate,
                        TaxPercentage   = @TaxPercentage,
                        TaxAmount       = @TaxAmount,
                        TotalAmount     = @TotalAmount,
                        Remarks         = @Remarks,
                        IsApproved      = @IsApproved,
                        ApprovedBy      = @ApprovedBy,
                        ApprovedAt      = @ApprovedAt,
                        UpdatedBy       = @UpdatedBy,
                        UpdatedAt       = @UpdatedAt
                    WHERE GRNEntryID = @GRNEntryID";

                        await _db.ExecuteAsync(updateSql, grn, transaction);
                        resultId = grn.GRNEntryID ?? 0;
                    }

                    // 🔹 Commit if everything succeeded
                    transaction.Commit();
                    return resultId;
                }
                catch (Exception)
                {
                    // 🔻 Rollback if anything failed
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public async Task<int> AddOrUpdatePurchaseEntryWithStockAsync(List<PurchaseEntry> entries)
        {
            if (entries == null || entries.Count == 0)
                throw new ArgumentException("No purchase entries provided.");

            int lastPurchaseId = 0;

            if (_db.State == ConnectionState.Closed)
                _db.Open(); // ✅ Ensure connection is open
            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    foreach (var entry in entries)
                    {
                        if (entry.CompanyID == null)
                            throw new ArgumentException("CompanyID  are required.");
                        if (string.IsNullOrWhiteSpace(entry.ProductName))
                            throw new ArgumentException("ProductName is required.");

                        entry.ProductName = entry.ProductName.Trim();

                        // 🔍 Check if PurchaseEntry exists
                        const string checkPurchaseSql = @"
                    SELECT TOP 1 PurchaseID
                    FROM PurchaseEntry
                    WHERE LTRIM(RTRIM(UPPER(ProductName))) = UPPER(@ProductName)
                      AND ISNULL(PurchaseRate,0) = ISNULL(@PurchaseRate,0)
                      AND CompanyID = @CompanyID
                      AND BranchID = @BranchID
                      AND IsActive = 1";

                        var existingPurchaseID = await _db.ExecuteScalarAsync<int?>(checkPurchaseSql, entry, tran);

                        if (existingPurchaseID != null && existingPurchaseID > 0)
                        {
                            // ✅ Update Purchase Entry (full columns)
                            const string updateEntrySql = @"
                        UPDATE PurchaseEntry SET
                            Quantity = ISNULL(Quantity,0) + @Quantity,
                            TotalAmount = ISNULL(TotalAmount,0) + @TotalAmount,
                            PurchaseRate = @PurchaseRate,
                            RetailPrice = @RetailPrice,
                            WholesalePrice = @WholesalePrice,
                            SaleRate = @SaleRate,
                            MRP = @MRP,
                            GstPercentage = @GstPercentage,
                            GstAmount = @GstAmount,
                            DiscountAmount = @DiscountAmount,
                            DiscountPercentage = @DiscountPercentage,
                            InclusiveAmount = @InclusiveAmount,
                            ExclusiveAmount = @ExclusiveAmount,
                            UpdatedByUserID = @UpdatedByUserID,
                            UpdatedSystemName = @UpdatedSystemName,
                            UpdatedAt = SYSDATETIME()
                        WHERE PurchaseID = @PurchaseID";

                            entry.PurchaseID = existingPurchaseID;
                            await _db.ExecuteAsync(updateEntrySql, entry, tran);
                            lastPurchaseId = existingPurchaseID.Value;
                        }
                        else
                        {
                            // 🆕 Insert Purchase Entry
                            const string insertEntrySql = @"
                        INSERT INTO PurchaseEntry
                        (PONumber, PurchaseDate, CompanyID, CompanyName, BranchID, BranchName,
                         SupplierID, SupplierName, ProductName, PurchaseRate, Quantity, TotalAmount,
                         StatusID, InvoiceNumber, InvoiceDate, SupplierInvoiceNumber, SupplierInvoiceDate,
                         BrandID, UnitID, HSNID, CategoryID, SubCategoryID, Barcode, ProductCode,
                         RetailPrice, WholesalePrice, SaleRate, MRP, DiscountAmount, DiscountPercentage,
                         InclusiveAmount, ExclusiveAmount, GstPercentage, GstAmount, CGSTRate, CGSTAmount,
                         SGSTRate, SGSTAmount, IGSTRate, IGSTAmount, CESSRate, CESSAmount,
                         TaxableValue, IsGSTInclusive, OrderedQuantity, ReceivedQuantity, ReturnedQuantity,
                         RemainingQuantity, OpeningStock, ReorderLevel, CurrentStock, Color, Size, Weight,
                         Volume, Material, FinishType, ShadeCode, Capacity, ModelNumber, ExpiryDate,
                         IsService, StatusName, TaxAmount, GrandTotal, Remarks, IsActive,
                         CreatedByUserID, CreatedSystemName, CreatedAt)
                        VALUES
                        (@PONumber, @PurchaseDate, @CompanyID, @CompanyName, @BranchID, @BranchName,
                         @SupplierID, @SupplierName, @ProductName, @PurchaseRate, @Quantity, @TotalAmount,
                         @StatusID, @InvoiceNumber, @InvoiceDate, @SupplierInvoiceNumber, @SupplierInvoiceDate,
                         @BrandID, @UnitID, @HSNID, @CategoryID, @SubCategoryID, @Barcode, @ProductCode,
                         @RetailPrice, @WholesalePrice, @SaleRate, @MRP, @DiscountAmount, @DiscountPercentage,
                         @InclusiveAmount, @ExclusiveAmount, @GstPercentage, @GstAmount, @CGSTRate, @CGSTAmount,
                         @SGSTRate, @SGSTAmount, @IGSTRate, @IGSTAmount, @CESSRate, @CESSAmount,
                         @TaxableValue, @IsGSTInclusive, @OrderedQuantity, @ReceivedQuantity, @ReturnedQuantity,
                         @RemainingQuantity, @OpeningStock, @ReorderLevel, @CurrentStock, @Color, @Size, @Weight,
                         @Volume, @Material, @FinishType, @ShadeCode, @Capacity, @ModelNumber, @ExpiryDate,
                         @IsService, @StatusName, @TaxAmount, @GrandTotal, @Remarks, 1,
                         @CreatedByUserID, @CreatedSystemName, SYSDATETIME());
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                            lastPurchaseId = await _db.ExecuteScalarAsync<int>(insertEntrySql, entry, tran);
                            entry.PurchaseID = lastPurchaseId;
                        }

                        // 🔍 Check Stock
                        const string checkStockSql = @"
                    SELECT TOP 1 StockID
                    FROM PurchaseEntryStock
                    WHERE LTRIM(RTRIM(UPPER(ProductName))) = UPPER(@ProductName)
                      AND CompanyID = @CompanyID
                      AND BranchID = @BranchID
                      AND IsActive = 1";

                        var existingStockID = await _db.ExecuteScalarAsync<int?>(checkStockSql, entry, tran);

                        if (existingStockID != null && existingStockID > 0)
                        {
                            // ✅ FULL UPDATE Stock
                            const string updateStockSql = @"
                        UPDATE PurchaseEntryStock SET
                            PurchaseDate = @PurchaseDate,
                            PurchaseRate = @PurchaseRate,
                            RetailPrice = @RetailPrice,
                            WholesalePrice = @WholesalePrice,
                            SaleRate = @SaleRate,
                            MRP = @MRP,
                            GstPercentage = @GstPercentage,
                            GstAmount = @GstAmount,
                            InclusiveAmount = @InclusiveAmount,
                            ExclusiveAmount = @ExclusiveAmount,
                            DiscountAmount = @DiscountAmount,
                            DiscountPercentage = @DiscountPercentage,
                            QuantityPurchased = ISNULL(QuantityPurchased,0) + @Quantity,
                            CurrentStock = ISNULL(CurrentStock,0) + @Quantity,
                            TotalAmount = @TotalAmount,
                            TaxAmount = @TaxAmount,
                            GrandTotal = @GrandTotal,
                            Color = @Color,
                            Size = @Size,
                            Weight = @Weight,
                            Volume = @Volume,
                            Material = @Material,
                            FinishType = @FinishType,
                            ShadeCode = @ShadeCode,
                            Capacity = @Capacity,
                            ModelNumber = @ModelNumber,
                            ExpiryDate = @ExpiryDate,
                            Remarks = @Remarks,
                            UpdatedByUserID = @UpdatedByUserID,
                            UpdatedSystemName = @UpdatedSystemName,
                            UpdatedAt = SYSDATETIME()
                        WHERE StockID = @StockID";

                            await _db.ExecuteAsync(updateStockSql, new
                            {
                                StockID = existingStockID,
                                entry.PurchaseDate,
                                entry.PurchaseRate,
                                entry.RetailPrice,
                                entry.WholesalePrice,
                                entry.SaleRate,
                                entry.MRP,
                                entry.GstPercentage,
                                entry.GstAmount,
                                entry.InclusiveAmount,
                                entry.ExclusiveAmount,
                                entry.DiscountAmount,
                                entry.DiscountPercentage,
                                entry.Quantity,
                                entry.TotalAmount,
                                entry.TaxAmount,
                                entry.GrandTotal,
                                entry.Color,
                                entry.Size,
                                entry.Weight,
                                entry.Volume,
                                entry.Material,
                                entry.FinishType,
                                entry.ShadeCode,
                                entry.Capacity,
                                entry.ModelNumber,
                                entry.ExpiryDate,
                                entry.Remarks,
                                entry.UpdatedByUserID,
                                entry.UpdatedSystemName
                            }, tran);
                        }
                        else
                        {
                            // 🆕 Insert Stock
                            const string insertStockSql = @"
                        INSERT INTO PurchaseEntryStock
                        (PurchaseID, PONumber, PurchaseDate, CompanyID, CompanyName, BranchID, BranchName,
                         SupplierID, SupplierName, ProductName, PurchaseRate, QuantityPurchased, CurrentStock,
                         RetailPrice, WholesalePrice, SaleRate, MRP, GstPercentage, GstAmount,
                         InclusiveAmount, ExclusiveAmount, DiscountAmount, DiscountPercentage,
                         Color, Size, Weight, Volume, Material, FinishType, ShadeCode, Capacity, ModelNumber,
                         ExpiryDate, TotalAmount, TaxAmount, GrandTotal, Remarks,
                         CreatedByUserID, CreatedSystemName, CreatedAt, IsActive)
                        VALUES
                        (@PurchaseID, @PONumber, @PurchaseDate, @CompanyID, @CompanyName, @BranchID, @BranchName,
                         @SupplierID, @SupplierName, @ProductName, @PurchaseRate, @Quantity, @Quantity,
                         @RetailPrice, @WholesalePrice, @SaleRate, @MRP, @GstPercentage, @GstAmount,
                         @InclusiveAmount, @ExclusiveAmount, @DiscountAmount, @DiscountPercentage,
                         @Color, @Size, @Weight, @Volume, @Material, @FinishType, @ShadeCode, @Capacity, @ModelNumber,
                         @ExpiryDate, @TotalAmount, @TaxAmount, @GrandTotal, @Remarks,
                         @CreatedByUserID, @CreatedSystemName, SYSDATETIME(), 1);";

                            await _db.ExecuteAsync(insertStockSql, entry, tran);
                        }
                    }

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }

            return lastPurchaseId;
        }

        public async Task<IEnumerable<PurchaseOrderEntry>> GetPurchaseOrdersAsync(
        int? poid = null,
        int? companyId = null,
        int? branchId = null,
        int? supplierId = null,
        DateTime? poDate = null)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@POID", poid);
            parameters.Add("@CompanyID", companyId);
            parameters.Add("@BranchID", branchId);
            parameters.Add("@SupplierID", supplierId);
            parameters.Add("@PODate", poDate);

            if (_db.State == ConnectionState.Closed)
                _db.Open();

            var list = await _db.QueryAsync<PurchaseOrderEntry>(
                "sp_GetPurchaseOrder",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return list;
        }
    }

    }
