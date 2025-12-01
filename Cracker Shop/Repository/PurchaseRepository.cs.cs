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
            if (grn == null)
                throw new ArgumentNullException(nameof(grn));

            if (grn.CompanyID == null || grn.CompanyID <= 0)
                throw new ArgumentException("CompanyID is required.");

            // ⭐ TRIM + NORMALIZE
            grn.GRNNumber = grn.GRNNumber?.Trim();
            grn.GRNNumber = Regex.Replace(grn.GRNNumber ?? "", @"\s+", " ");

            // ⭐ GENERATE GRN NUMBER IF EMPTY
            if ((grn.GRNEntryID == 0 || grn.GRNEntryID == null) &&
                string.IsNullOrWhiteSpace(grn.GRNNumber))
            {
                grn.GRNNumber = await CodeGenerator.GenerateNextCodeAsync(
                    _db, "GRNEntry", "GRNNumber", prefix: "GRN",  5
                );
            }

            if (_db.State == ConnectionState.Closed)
                _db.Open();

            using (var transaction = _db.BeginTransaction())
            {
                try
                {
                    // 🔍 Duplicate Check
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

                    // ─────────────────────────────────────────────
                    // INSERT
                    // ─────────────────────────────────────────────
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
                        OrderedQty,
                        StatusId, StatusName,
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
                        @OrderedQty,
                        @StatusId, @StatusName,
                        @Remarks, @IsApproved, @ApprovedBy, @ApprovedAt,
                        @IsActive, @CreatedBy, @CreatedAt
                    );

                    SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                        resultId = await _db.ExecuteScalarAsync<long>(insertSql, grn, transaction);
                    }
                    // ─────────────────────────────────────────────
                    // DELETE (SOFT DELETE)
                    // ─────────────────────────────────────────────
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
                    // ─────────────────────────────────────────────
                    // UPDATE
                    // ─────────────────────────────────────────────
                    else
                    {
                        grn.UpdatedAt = DateTime.Now;

                        const string updateSql = @"
                    UPDATE GRNEntry SET
                        GRNNumber     = @GRNNumber,
                        GRNDate       = @GRNDate,
                        POID          = @POID,
                        PODetailID    = @PODetailID,
                        PurchaseID    = @PurchaseID,
                        SupplierID    = @SupplierID,
                        SupplierName  = @SupplierName,
                        CompanyID     = @CompanyID,
                        BranchID      = @BranchID,
                        InvoiceNumber = @InvoiceNumber,
                        InvoiceDate   = @InvoiceDate,
                        TransportName = @TransportName,
                        VehicleNumber = @VehicleNumber,
                        ReceivedBy    = @ReceivedBy,
                        ProductID     = @ProductID,
                        ProductCode   = @ProductCode,
                        ProductName   = @ProductName,
                        UnitID        = @UnitID,
                        ReceivedQty   = @ReceivedQty,
                        AcceptedQty   = @AcceptedQty,
                        RejectedQty   = @RejectedQty,
                        PurchaseRate  = @PurchaseRate,
                        TaxPercentage = @TaxPercentage,
                        TaxAmount     = @TaxAmount,
                        OrderedQty    = @OrderedQty,
                        StatusId      = @StatusId,
                        StatusName    = @StatusName,
                        TotalAmount   = @TotalAmount,
                        Remarks       = @Remarks,
                        IsApproved    = @IsApproved,
                        ApprovedBy    = @ApprovedBy,
                        ApprovedAt    = @ApprovedAt,
                        UpdatedBy     = @UpdatedBy,
                        UpdatedAt     = @UpdatedAt
                    WHERE GRNEntryID = @GRNEntryID";

                        await _db.ExecuteAsync(updateSql, grn, transaction);
                        resultId = grn.GRNEntryID ?? 0;
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



        public async Task<int> AddOrUpdatePurchaseEntryWithStockAsync(List<PurchaseEntry> entries)
        {
            if (entries == null || entries.Count == 0)
                throw new ArgumentException("No purchase entries provided.");

            var header = entries[0];
            if (header == null)
                throw new ArgumentNullException(nameof(header));

            header.PONumber = header.PONumber?.Trim();

            // ⭐ Auto-generate PO Number
            if ((header.POID == 0 || header.POID == null) && string.IsNullOrWhiteSpace(header.PONumber))
            {
                header.PONumber = await CodeGenerator.GenerateNextCodeAsync(
                    _db, "PurchaseEntry", "PONumber", "PO", 5
                );
            }

            if (_db.State == ConnectionState.Closed)
                _db.Open();

            int lastPurchaseId = 0;

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    foreach (var entry in entries)
                    {
                        if (entry.CompanyID == null)
                            throw new ArgumentException("CompanyID is required.");

                        if (string.IsNullOrWhiteSpace(entry.ProductName))
                            throw new ArgumentException("ProductName is required.");

                        entry.ProductName = entry.ProductName.Trim();

                        // ============================================
                        // 1️⃣ INSERT PURCHASE ENTRY ALWAYS
                        // ============================================
                        const string insertEntrySql = @"
INSERT INTO PurchaseEntry
(
    PONumber, PurchaseDate, CompanyID, CompanyName, BranchID, BranchName,
    SupplierID, SupplierName, ProductName, ProductCode, PurchaseRate, Quantity, TotalAmount,
    StatusID, InvoiceNumber, InvoiceDate, SupplierInvoiceNumber, SupplierInvoiceDate,
    BrandID, UnitID, HSNID, CategoryID, SubCategoryID, Barcode,
    RetailPrice, WholesalePrice, SaleRate, MRP, DiscountAmount, DiscountPercentage,
    InclusiveAmount, ExclusiveAmount, GstPercentage, GstAmount, CGSTRate, CGSTAmount,
    SGSTRate, SGSTAmount, IGSTRate, IGSTAmount, CESSRate, CESSAmount,
    TaxableValue, IsGSTInclusive, OrderedQuantity, ReceivedQuantity, ReturnedQuantity,
    RemainingQuantity, OpeningStock, ReorderLevel, CurrentStock, Color, Size, Weight,
    Volume, Material, FinishType, ShadeCode, Capacity, ModelNumber, ExpiryDate,
    IsService, StatusName, TaxAmount, GrandTotal, Remarks, IsActive,
    CreatedByUserID, CreatedSystemName, CreatedAt,
    TotalGrossAmount, TotalDiscAmount, TotalTaxableAmount,
    TotalGstAmount, TotalCessAmount, TotalNetAmount,
    TotalInvoiceAmount, TotalPaidAmount, TotalBalanceAmount, TotalRoundOff
)
VALUES
(
    @PONumber, @PurchaseDate, @CompanyID, @CompanyName, @BranchID, @BranchName,
    @SupplierID, @SupplierName, @ProductName, @ProductCode, @PurchaseRate, @Quantity, @TotalAmount,
    @StatusID, @InvoiceNumber, @InvoiceDate, @SupplierInvoiceNumber, @SupplierInvoiceDate,
    @BrandID, @UnitID, @HSNID, @CategoryID, @SubCategoryID, @Barcode,
    @RetailPrice, @WholesalePrice, @SaleRate, @MRP, @DiscountAmount, @DiscountPercentage,
    @InclusiveAmount, @ExclusiveAmount, @GstPercentage, @GstAmount, @CGSTRate, @CGSTAmount,
    @SGSTRate, @SGSTAmount, @IGSTRate, @IGSTAmount, @CESSRate, @CESSAmount,
    @TaxableValue, @IsGSTInclusive, @OrderedQuantity, @ReceivedQuantity, @ReturnedQuantity,
    @RemainingQuantity, @OpeningStock, @ReorderLevel, @CurrentStock, @Color, @Size, @Weight,
    @Volume, @Material, @FinishType, @ShadeCode, @Capacity, @ModelNumber, @ExpiryDate,
    @IsService, @StatusName, @TaxAmount, @GrandTotal, @Remarks, 1,
    @CreatedByUserID, @CreatedSystemName, SYSDATETIME(),
    @TotalGrossAmount, @TotalDiscAmount, @TotalTaxableAmount,
    @TotalGstAmount, @TotalCessAmount, @TotalNetAmount,
    @TotalInvoiceAmount, @TotalPaidAmount, @TotalBalanceAmount, @TotalRoundOff
);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        lastPurchaseId = await _db.ExecuteScalarAsync<int>(insertEntrySql, entry, tran);
                        entry.PurchaseID = lastPurchaseId;

                        // ============================================
                        // 2️⃣ CHECK STOCK WITH PRICE MATCH (OPTION B)
                        // ============================================
                        const string checkStockSql = @"
SELECT TOP 1 StockID
FROM PurchaseEntryStock
WHERE ProductCode = @ProductCode
  AND CompanyID = @CompanyID
  AND BranchID = @BranchID
  AND IsActive = 1
  AND PurchaseRate = @PurchaseRate
  AND RetailPrice = @RetailPrice
  AND WholesalePrice = @WholesalePrice
  AND SaleRate = @SaleRate
  AND MRP = @MRP
  AND GstPercentage = @GstPercentage";

                        var existingStockID = await _db.ExecuteScalarAsync<int?>(checkStockSql, entry, tran);

                        // ============================================
                        // 3️⃣ UPDATE STOCK WHEN EXACT PRICE MATCH
                        // ============================================
                        if (existingStockID != null && existingStockID > 0)
                        {
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
    QuantityPurchased = ISNULL(QuantityPurchased,0) + @Quantity,
    CurrentStock = ISNULL(CurrentStock,0) + @Quantity,
    TotalAmount = @TotalAmount,
    TaxAmount = @TaxAmount,
    GrandTotal = @GrandTotal,
    TotalGrossAmount = @TotalGrossAmount,
    TotalDiscAmount = @TotalDiscAmount,
    TotalTaxableAmount = @TotalTaxableAmount,
    TotalGstAmount = @TotalGstAmount,
    TotalCessAmount = @TotalCessAmount,
    TotalNetAmount = @TotalNetAmount,
    TotalInvoiceAmount = @TotalInvoiceAmount,
    TotalPaidAmount = @TotalPaidAmount,
    TotalBalanceAmount = @TotalBalanceAmount,
    TotalRoundOff = @TotalRoundOff,
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
                                entry.Quantity,
                                entry.TotalAmount,
                                entry.TaxAmount,
                                entry.GrandTotal,
                                entry.TotalGrossAmount,
                                entry.TotalDiscAmount,
                                entry.TotalTaxableAmount,
                                entry.TotalGstAmount,
                                entry.TotalCessAmount,
                                entry.TotalNetAmount,
                                entry.TotalInvoiceAmount,
                                entry.TotalPaidAmount,
                                entry.TotalBalanceAmount,
                                entry.TotalRoundOff,
                                entry.UpdatedByUserID,
                                entry.UpdatedSystemName
                            }, tran);
                        }
                        else
                        {
                            // ============================================
                            // 4️⃣ INSERT NEW STOCK ROW (PRICE MISMATCH)
                            // ============================================
                            const string insertStockSql = @"
INSERT INTO PurchaseEntryStock
(
    PurchaseID, PONumber, PurchaseDate, CompanyID, CompanyName, BranchID, BranchName,
    SupplierID, SupplierName, ProductCode, ProductName, PurchaseRate, QuantityPurchased, CurrentStock,
    RetailPrice, WholesalePrice, SaleRate, MRP, GstPercentage, GstAmount,
    InclusiveAmount, ExclusiveAmount, DiscountAmount, DiscountPercentage,
    Color, Size, Weight, Volume, Material, FinishType, ShadeCode, Capacity, ModelNumber,
    ExpiryDate, TotalAmount, TaxAmount, GrandTotal, Remarks,
    CreatedByUserID, CreatedSystemName, CreatedAt, IsActive,
    TotalGrossAmount, TotalDiscAmount, TotalTaxableAmount,
    TotalGstAmount, TotalCessAmount, TotalNetAmount,
    TotalInvoiceAmount, TotalPaidAmount, TotalBalanceAmount, TotalRoundOff
)
VALUES
(
    @PurchaseID, @PONumber, @PurchaseDate, @CompanyID, @CompanyName, @BranchID, @BranchName,
    @SupplierID, @SupplierName, @ProductCode, @ProductName, @PurchaseRate, @Quantity, @Quantity,
    @RetailPrice, @WholesalePrice, @SaleRate, @MRP, @GstPercentage, @GstAmount,
    @InclusiveAmount, @ExclusiveAmount, @DiscountAmount, @DiscountPercentage,
    @Color, @Size, @Weight, @Volume, @Material, @FinishType, @ShadeCode, @Capacity, @ModelNumber,
    @ExpiryDate, @TotalAmount, @TaxAmount, @GrandTotal, @Remarks,
    @CreatedByUserID, @CreatedSystemName, SYSDATETIME(), 1,
    @TotalGrossAmount, @TotalDiscAmount, @TotalTaxableAmount,
    @TotalGstAmount, @TotalCessAmount, @TotalNetAmount,
    @TotalInvoiceAmount, @TotalPaidAmount, @TotalBalanceAmount, @TotalRoundOff
);";

                            await _db.ExecuteAsync(insertStockSql, entry, tran);
                        }
                    }

                    // -----------------------------
                    // 5️⃣ SUPPLIER OUTSTANDING
                    // -----------------------------
                    decimal totalBill = header.TotalInvoiceAmount ?? 0m;
                    decimal totalPaid = header.TotalPaidAmount ?? 0m;
                    decimal totalBalance = totalBill - totalPaid;

                    string paymentStatus =
                          totalBalance <= 0 ? "PAID"
                        : totalPaid == 0 ? "UNPAID"
                        : "PARTIAL";

                    const string checkOutstanding = @"
SELECT TOP 1 SupplierOutstandingID
FROM SupplierOutstanding
WHERE PurchaseID = @PurchaseID
  AND SupplierID = @SupplierID
  AND CompanyID = @CompanyID
  AND BranchID = @BranchID
  AND IsActive = 1";

                    var existingOutID = await _db.ExecuteScalarAsync<int?>(checkOutstanding, new
                    {
                        PurchaseID = lastPurchaseId,
                        header.SupplierID,
                        header.CompanyID,
                        header.BranchID
                    }, tran);

                    if (existingOutID != null && existingOutID > 0)
                    {
                        const string updateOut = @"
UPDATE SupplierOutstanding SET
    TotalBillAmount = @TotalBillAmount,
    TotalPaidAmount = @TotalPaidAmount,
    TotalBalanceAmount = @TotalBalanceAmount,
    PaymentMode = @PaymentMode,
    PaymentStatus = @PaymentStatus,
    UpdatedByUserID = @UpdatedByUserID,
    UpdatedSystemName = @UpdatedSystemName,
    UpdatedAt = SYSDATETIME()
WHERE SupplierOutstandingID = @SupplierOutstandingID";

                        await _db.ExecuteAsync(updateOut, new
                        {
                            SupplierOutstandingID = existingOutID,
                            TotalBillAmount = totalBill,
                            TotalPaidAmount = totalPaid,
                            TotalBalanceAmount = totalBalance,
                            PaymentMode = header.PaymentMode,
                            PaymentStatus = paymentStatus,
                            header.UpdatedByUserID,
                            header.UpdatedSystemName
                        }, tran);
                    }
                    else
                    {
                        const string insertOut = @"
INSERT INTO SupplierOutstanding
(
    CompanyID, CompanyName, BranchID, BranchName,
    SupplierID, SupplierName, PurchaseID, PurchaseNumber,
    PurchaseDate, TotalBillAmount, TotalPaidAmount,
    TotalBalanceAmount, PaymentMode, PaymentStatus,
    Remarks, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt
)
VALUES
(
    @CompanyID, @CompanyName, @BranchID, @BranchName,
    @SupplierID, @SupplierName, @PurchaseID, @PONumber,
    @PurchaseDate, @TotalBillAmount, @TotalPaidAmount,
    @TotalBalanceAmount, @PaymentMode, @PaymentStatus,
    @Remarks, 1, @CreatedByUserID, @CreatedSystemName, SYSDATETIME()
)";

                        await _db.ExecuteAsync(insertOut, new
                        {
                            header.CompanyID,
                            header.CompanyName,
                            header.BranchID,
                            header.BranchName,
                            header.SupplierID,
                            header.SupplierName,
                            PurchaseID = lastPurchaseId,
                            PONumber = header.PONumber,
                            header.PurchaseDate,
                            TotalBillAmount = totalBill,
                            TotalPaidAmount = totalPaid,
                            TotalBalanceAmount = totalBalance,
                            PaymentMode = header.PaymentMode,
                            PaymentStatus = paymentStatus,
                            header.Remarks,
                            header.CreatedByUserID,
                            header.CreatedSystemName
                        }, tran);
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


















        /*
        public async Task<int> SavePurchaseEntryUsingSP_AllColumns(List<PurchaseEntry> entries)
        {
            if (entries == null || entries.Count == 0)
                throw new ArgumentException("No purchase entries provided.");

            int lastPurchaseId = 0;

            if (_db.State == ConnectionState.Closed)
                _db.Open();

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    foreach (var e in entries)
                    {
                        var p = new DynamicParameters();

                        // ---------------------------
                        // BASIC PURCHASE ENTRY FIELDS
                        // ---------------------------
                        p.Add("@PONumber", e.PONumber);
                        p.Add("@PurchaseDate", e.PurchaseDate);
                        p.Add("@CompanyID", e.CompanyID);
                        p.Add("@CompanyName", e.CompanyName);
                        p.Add("@BranchID", e.BranchID);
                        p.Add("@BranchName", e.BranchName);
                        p.Add("@SupplierID", e.SupplierID);
                        p.Add("@SupplierName", e.SupplierName);

                        p.Add("@ProductName", e.ProductName);
                        p.Add("@PurchaseRate", e.PurchaseRate);
                        p.Add("@Quantity", e.Quantity);
                        p.Add("@TotalAmount", e.TotalAmount);

                        // ---------------------------
                        // PRICE FIELDS
                        // ---------------------------
                        p.Add("@RetailPrice", e.RetailPrice);
                        p.Add("@WholesalePrice", e.WholesalePrice);
                        p.Add("@SaleRate", e.SaleRate);
                        p.Add("@MRP", e.MRP);

                        // ---------------------------
                        // MASTER IDs
                        // ---------------------------
                        p.Add("@BrandID", e.BrandID);
                        p.Add("@UnitID", e.UnitID);
                        p.Add("@HSNID", e.HSNID);
                        p.Add("@CategoryID", e.CategoryID);
                        p.Add("@SubCategoryID", e.SubCategoryID);

                        // ---------------------------
                        // PRODUCT DETAILS
                        // ---------------------------
                        p.Add("@Barcode", e.Barcode);
                        p.Add("@ProductCode", e.ProductCode);

                        // ---------------------------
                        // GST DETAILS
                        // ---------------------------
                        p.Add("@GstPercentage", e.GstPercentage);
                        p.Add("@GstAmount", e.GstAmount);
                        p.Add("@CGSTRate", e.CGSTRate);
                        p.Add("@CGSTAmount", e.CGSTAmount);
                        p.Add("@SGSTRate", e.SGSTRate);
                        p.Add("@SGSTAmount", e.SGSTAmount);
                        p.Add("@IGSTRate", e.IGSTRate);
                        p.Add("@IGSTAmount", e.IGSTAmount);
                        p.Add("@CESSRate", e.CESSRate);
                        p.Add("@CESSAmount", e.CESSAmount);

                        // ---------------------------
                        // TAXABLE VALUES
                        // ---------------------------
                        p.Add("@TaxableValue", e.TaxableValue);
                        p.Add("@IsGSTInclusive", e.IsGSTInclusive);

                        // ---------------------------
                        // STOCK QUANTITIES
                        // ---------------------------
                        p.Add("@OrderedQuantity", e.OrderedQuantity);
                        p.Add("@ReceivedQuantity", e.ReceivedQuantity);
                        p.Add("@ReturnedQuantity", e.ReturnedQuantity);
                        p.Add("@RemainingQuantity", e.RemainingQuantity);
                        p.Add("@OpeningStock", e.OpeningStock);
                        p.Add("@ReorderLevel", e.ReorderLevel);
                        p.Add("@CurrentStock", e.CurrentStock);

                        // ---------------------------
                        // PRODUCT VARIANTS
                        // ---------------------------
                        p.Add("@Color", e.Color);
                        p.Add("@Size", e.Size);
                        p.Add("@Weight", e.Weight);
                        p.Add("@Volume", e.Volume);
                        p.Add("@Material", e.Material);
                        p.Add("@FinishType", e.FinishType);
                        p.Add("@ShadeCode", e.ShadeCode);
                        p.Add("@Capacity", e.Capacity);
                        p.Add("@ModelNumber", e.ModelNumber);
                        p.Add("@ExpiryDate", e.ExpiryDate);

                        // ---------------------------
                        // SERVICE & STATUS
                        // ---------------------------
                        p.Add("@IsService", e.IsService);
                        p.Add("@StatusName", e.StatusName);

                        // ---------------------------
                        // TAX AMOUNTS
                        // ---------------------------
                        p.Add("@TaxAmount", e.TaxAmount);
                        p.Add("@GrandTotal", e.GrandTotal);

                        // ---------------------------
                        // TOTALS SECTION (NEW)
                        // ---------------------------
                        p.Add("@TotalGrossAmount", e.TotalGrossAmount);
                        p.Add("@TotalDiscAmount", e.TotalDiscAmount);
                        p.Add("@TotalTaxableAmount", e.TotalTaxableAmount);
                        p.Add("@TotalGstAmount", e.TotalGstAmount);
                        p.Add("@TotalCessAmount", e.TotalCessAmount);
                        p.Add("@TotalNetAmount", e.TotalNetAmount);
                        p.Add("@TotalInvoiceAmount", e.TotalInvoiceAmount);
                        p.Add("@TotalPaidAmount", e.TotalPaidAmount);
                        p.Add("@TotalBalanceAmount", e.TotalBalanceAmount);
                        p.Add("@TotalRoundOff", e.TotalRoundOff);

                        // ---------------------------
                        // PAYMENT DETAILS
                        // ---------------------------
                        p.Add("@PaymentMode", e.PaymentMode);
                        p.Add("@AccountingYear", e.AccountingYear);
                        p.Add("@Remarks", e.Remarks);

                        // ---------------------------
                        // AUDIT COLUMNS
                        // ---------------------------
                        p.Add("@CreatedByUserID", e.CreatedByUserID);
                        p.Add("@CreatedSystemName", e.CreatedSystemName);
                        p.Add("@UpdatedByUserID", e.UpdatedByUserID);
                        p.Add("@UpdatedSystemName", e.UpdatedSystemName);

                        // ---------------------------
                        // OUTPUT PARAM
                        // ---------------------------
                        p.Add("@NewPurchaseID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                        // ---------------------------
                        // EXECUTE SP
                        // ---------------------------
                        await _db.ExecuteAsync(
                            "sp_AddOrUpdate_PurchaseEntry_WithStock_And_Outstanding",
                            p,
                            tran,
                            commandType: CommandType.StoredProcedure
                        );

                        // READ OUTPUT
                        lastPurchaseId = p.Get<int>("@NewPurchaseID");
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
        */











        public async Task<IEnumerable<PurchaseOrderEntry>> GetPurchaseOrdersAsync(
         int? poid = null,
         int? companyId = null,
         int? branchId = null,
         int? supplierId = null,
         DateTime? poDate = null,
         string? poNumber = null  
     )
        {
            var parameters = new DynamicParameters();

            parameters.Add("@POID", poid);
            parameters.Add("@CompanyID", companyId);
            parameters.Add("@BranchID", branchId);
            parameters.Add("@SupplierID", supplierId);
            parameters.Add("@PODate", poDate);
            parameters.Add("@PONumber", poNumber);  

            if (_db.State == ConnectionState.Closed)
                _db.Open();

            var list = await _db.QueryAsync<PurchaseOrderEntry>(
                "sp_GetPurchaseOrder",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return list;
        }
        public async Task<IEnumerable<PurchaseEntry>> GetPurchaseStockAsync(
           int? companyId = null,
           int? branchId = null,
           int? supplierId = null,
           DateTime? fromDate = null,
           DateTime? toDate = null,
           string? poNumber = null
       )
        {
            var parameters = new DynamicParameters();

            parameters.Add("@CompanyID", companyId);
            parameters.Add("@BranchID", branchId);
            parameters.Add("@SupplierID", supplierId);
            parameters.Add("@FromDate", fromDate);
            parameters.Add("@ToDate", toDate);
            parameters.Add("@PONumber", poNumber);

            if (_db.State == ConnectionState.Closed)
                _db.Open();

            var list = await _db.QueryAsync<PurchaseEntry>(
                "sp_Get_PurchaseEntry",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return list;
        }





public async Task<string> GetNextPONumberAsync(int companyId, string? branchId)
{
    var parameters = new DynamicParameters();
    parameters.Add("@CompanyID", companyId);

    // If branchId is null or empty, send empty string (SP handles it)
    parameters.Add("@BranchID", string.IsNullOrWhiteSpace(branchId) ? "" : branchId);

    if (_db.State == ConnectionState.Closed)
        _db.Open();

    var result = await _db.QueryFirstOrDefaultAsync<string?>(
        "sp_GetNextPONumber",
        parameters,
        commandType: CommandType.StoredProcedure
    );

    return result ?? "PO00001";  // Never return null
}








    }

}
