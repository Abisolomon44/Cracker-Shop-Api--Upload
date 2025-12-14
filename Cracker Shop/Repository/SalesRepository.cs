using Cracker_Shop.Code_Generator;
using Cracker_Shop.Models.Purchase;
using Cracker_Shop.Models.Sales;
using Cracker_Shop.Repository.IRepository;
using Dapper;
using System.Data;

namespace Cracker_Shop.Repository
{
    public class SalesRepository : ISalesRepository
    {

        private readonly IDbConnection _db;

        public SalesRepository(IDbConnection db)
        {
            _db = db;
        }



        public async Task<InvoiceSaveResult> AddOrUpdateSalesEntryWithStockAsync(List<SalesEntryMaster> entries)

        {
            if (entries == null || entries.Count == 0)
                throw new ArgumentException("No sales entries provided.");

            var header = entries[0];
            if (header == null)
                throw new ArgumentNullException(nameof(header));

            header.InvoiceNumber = header.InvoiceNumber?.Trim();

            // ⭐ AUTO GENERATE INVOICE NUMBER
            if ((header.InvoiceID == 0 || header.InvoiceID == null) && string.IsNullOrWhiteSpace(header.InvoiceNumber))
            {
                header.InvoiceNumber = await CodeGenerator.GenerateNextCodeAsync(
                    _db, "SalesEntryMaster", "InvoiceNumber", "INV", 5);
            }

            if (_db.State == ConnectionState.Closed)
                _db.Open();

            int lastInvoiceID = 0;
            int lastInvoiceNumber = 0;


            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    // 1️⃣ INSERT SALES ENTRY MASTER
                    const string insertSalesSql = @"
INSERT INTO SalesEntryMaster
(
    InvoiceNumber, InvoiceDate, CompanyID, CompanyName, BranchID, BranchName,
    CustomerID, CustomerName, CustomerContact, CustomerGSTIN, CustomerState, CompanyState,
    AccountingYear, BillingType, IsGSTApplicable, GSTType,
    ProductID, Barcode, ProductCode, ProductName, BrandID, CategoryID, SubCategoryID,
    HSNID, UnitID, SecondaryUnitID, Color, Size, Weight, Volume, Material,
    FinishType, ShadeCode, Capacity, ModelNumber, ExpiryDate, ManufacturingDate,
    Quantity, ProductRate, SaleRate, RetailPrice, WholesalePrice, MRP,
    DiscountPercentage, DiscountAmount, InclusiveAmount, ExclusiveAmount,
    GstPercentage, GstAmount, CGSTRate, CGSTAmount, SGSTRate, SGSTAmount,
    IGSTRate, IGSTAmount, CESSRate, CESSAmount, GrossAmount,
    CustomerDiscount, NetAmount, TaxableAmount, GrandTotal,
    BillingMode, CashAmount, CardAmount, UPIAmount, AdvanceAmount, PaidAmount, BalanceAmount,
    Status, Remarks, IsActive, CreatedBy, CreatedDate,
    TotalGrossAmount, TotalDiscAmount, TotalTaxableAmount, TotalGstAmount,
    TotalCESSAmount, TotalNetAmount, TotalInvoiceAmount, TotalPaidAmount,
    TotalBalanceAmount, TotalRoundOff, TotalQuantity
)
VALUES
(
    @InvoiceNumber, @InvoiceDate, @CompanyID, @CompanyName, @BranchID, @BranchName,
    @CustomerID, @CustomerName, @CustomerContact, @CustomerGSTIN, @CustomerState, @CompanyState,
    @AccountingYear, @BillingType, @IsGSTApplicable, @GSTType,
    @ProductID, @Barcode, @ProductCode, @ProductName, @BrandID, @CategoryID, @SubCategoryID,
    @HSNID, @UnitID, @SecondaryUnitID, @Color, @Size, @Weight, @Volume, @Material,
    @FinishType, @ShadeCode, @Capacity, @ModelNumber, @ExpiryDate, @ManufacturingDate,
    @Quantity, @ProductRate, @SaleRate, @RetailPrice, @WholesalePrice, @MRP,
    @DiscountPercentage, @DiscountAmount, @InclusiveAmount, @ExclusiveAmount,
    @GstPercentage, @GstAmount, @CGSTRate, @CGSTAmount, @SGSTRate, @SGSTAmount,
    @IGSTRate, @IGSTAmount, @CESSRate, @CESSAmount, @GrossAmount,
    @CustomerDiscount, @NetAmount, @TaxableAmount, @GrandTotal,
    @BillingMode, @CashAmount, @CardAmount, @UPIAmount, @AdvanceAmount, @PaidAmount, @BalanceAmount,
    @Status, @Remarks, 1, @CreatedBy, SYSDATETIME(),
    @TotalGrossAmount, @TotalDiscAmount, @TotalTaxableAmount, @TotalGstAmount,
    @TotalCESSAmount, @TotalNetAmount, @TotalInvoiceAmount, @TotalPaidAmount,
    @TotalBalanceAmount, @TotalRoundOff, @TotalQuantity
);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    foreach (var entry in entries)
                    {
                        ValidateSalesEntry(entry);

                        lastInvoiceID = await _db.ExecuteScalarAsync<int>(insertSalesSql, entry, tran);
                        entry.InvoiceID = lastInvoiceID;

                        // 2️⃣ STOCK UPDATE — FIXED & OPTIMIZED
                        const string stockCheckSql = @"
SELECT TOP 1 StockID, CurrentStock
FROM PurchaseEntryStock
WHERE ProductCode = @ProductCode
AND CompanyID = @CompanyID
AND BranchID = @BranchID
AND IsActive = 1";

                        var stockRow = await _db.QueryFirstOrDefaultAsync<dynamic>(stockCheckSql, new
                        {
                            entry.ProductCode,
                            entry.CompanyID,
                            entry.BranchID
                        }, tran);

                        if (stockRow != null)
                        {
                            decimal currentStock = stockRow.CurrentStock ?? 0m;
                            decimal soldQty = entry.Quantity == null
       ? 0m
       : Convert.ToDecimal(entry.Quantity);

                            decimal newStock = currentStock - soldQty;
                            if (newStock < 0) newStock = 0;





                            const string updateStockSql = @"
UPDATE PurchaseEntryStock SET
    CurrentStock = @CurrentStock,
    QuantitySold = ISNULL(QuantitySold, 0) + @SoldQuantity,
    UpdatedByUserID = NULL,
    UpdatedSystemName = @UpdatedBy,
    UpdatedAt = SYSDATETIME()
WHERE StockID = @StockID";

                            await _db.ExecuteAsync(updateStockSql, new
                            {
                                StockID = (long)stockRow.StockID,
                                CurrentStock = newStock,
                                SoldQuantity = soldQty,      // 🔥 THIS UPDATES QuantitySold
                                UpdatedBy = entry.CreatedBy
                            }, tran);







                        }
                        else
                        {
                            // DEBUG: No stock found
                            Console.WriteLine($"⚠ NO STOCK FOUND FOR PRODUCT {entry.ProductCode} (Company {entry.CompanyID}, Branch {entry.BranchID})");
                        }
                    }

                    // 3️⃣ INSERT/UPDATE SALES INVOICE TOTAL (unchanged)
                    const string checkTotalSql = @"
SELECT InvoiceID
FROM SalesInvoiceTotal
WHERE InvoiceNumber = @InvoiceNumber
AND CompanyID = @CompanyID
AND BranchID = @BranchID";

                    var existingTotalID = await _db.ExecuteScalarAsync<int?>(checkTotalSql, header, tran);

                    if (existingTotalID != null)
                    {
                        const string updateTotalSql = @"
UPDATE SalesInvoiceTotal SET
    TotalQuantity = @TotalQuantity,
    TotalSaleRate = @TotalSaleRate,
    TotalDiscountAmount = @TotalDiscountAmount,
    TotalCGSTAmount = @TotalCGSTAmount,
    TotalSGSTAmount = @TotalSGSTAmount,
    TotalIGSTAmount = @TotalIGSTAmount,
    TotalCESSAmount = @TotalCESSAmount,
    TotalGrossAmount = @TotalGrossAmount,
    TotalTaxableAmount = @TotalTaxableAmount,
    GrandTotal = @GrandTotal,
    BillingMode = @BillingMode,
    CashAmount = @CashAmount,
    CardAmount = @CardAmount,
    UPIAmount = @UPIAmount,
    AdvanceAmount = @AdvanceAmount,
    PaidAmount = @PaidAmount,
    BalanceAmount = @BalanceAmount,
    Status = @Status,
    UpdatedBy = @UpdatedBy,
    UpdatedDate = SYSDATETIME()
WHERE InvoiceID = @InvoiceID";

                        await _db.ExecuteAsync(updateTotalSql, new
                        {
                            InvoiceID = existingTotalID,
                            header.TotalQuantity,
                            header.TotalSaleRate,
                            header.TotalDiscountAmount,
                            header.TotalCGSTAmount,
                            header.TotalSGSTAmount,
                            header.TotalIGSTAmount,
                            header.TotalCESSAmount,
                            header.TotalGrossAmount,
                            header.TotalTaxableAmount,
                            header.GrandTotal,
                            header.BillingMode,
                            header.CashAmount,
                            header.CardAmount,
                            header.UPIAmount,
                            header.AdvanceAmount,
                            header.PaidAmount,
                            header.BalanceAmount,
                            header.Status,
                            header.UpdatedBy
                        }, tran);
                    }
                    else
                    {
                        const string insertTotalSql = @"
INSERT INTO SalesInvoiceTotal
(
    InvoiceNumber, InvoiceDate, CompanyID, CompanyName, BranchID, BranchName,
    CustomerID, CustomerName, CustomerContact, CustomerGSTIN, CustomerState,
    AccountingYear, BillingType, GSTType,
    TotalQuantity, TotalSaleRate, TotalDiscountAmount,
    TotalCGSTAmount, TotalSGSTAmount, TotalIGSTAmount, TotalCESSAmount,
    TotalGrossAmount, TotalTaxableAmount, GrandTotal,
    BillingMode, CashAmount, CardAmount, UPIAmount, AdvanceAmount,
    PaidAmount, BalanceAmount, Status, CreatedBy, CreatedDate
)
VALUES
(
    @InvoiceNumber, @InvoiceDate, @CompanyID, @CompanyName, @BranchID, @BranchName,
    @CustomerID, @CustomerName, @CustomerContact, @CustomerGSTIN, @CustomerState,
    @AccountingYear, @BillingType, @GSTType,
    @TotalQuantity, @TotalSaleRate, @TotalDiscountAmount,
    @TotalCGSTAmount, @TotalSGSTAmount, @TotalIGSTAmount, @TotalCESSAmount,
    @TotalGrossAmount, @TotalTaxableAmount, @GrandTotal,
    @BillingMode, @CashAmount, @CardAmount, @UPIAmount, @AdvanceAmount,
    @PaidAmount, @BalanceAmount, @Status, @CreatedBy, SYSDATETIME()
)";

                        await _db.ExecuteAsync(insertTotalSql, header, tran);
                    }

                    // 4️⃣ CUSTOMER OUTSTANDING (unchanged)
                    decimal invoiceAmount = header.GrandTotal ?? 0m;
                    decimal paidAmount = header.PaidAmount ?? 0m;
                    decimal balanceAmount = invoiceAmount - paidAmount;

                    string status =
                            balanceAmount <= 0 ? "PAID"
                          : paidAmount == 0 ? "UNPAID"
                          : "PARTIAL";

                    const string checkOutSql = @"
SELECT OutstandingID
FROM CustomerOutstanding
WHERE InvoiceID = @InvoiceID
AND CustomerID = @CustomerID
AND CompanyID = @CompanyID
AND BranchID = @BranchID
AND IsActive = 1";

                    var existingOutID = await _db.ExecuteScalarAsync<int?>(checkOutSql, new
                    {
                        InvoiceID = lastInvoiceID,
                        header.CustomerID,
                        header.CompanyID,
                        header.BranchID
                    }, tran);

                    if (existingOutID != null)
                    {
                        const string updateOutSql = @"
UPDATE CustomerOutstanding SET
    InvoiceAmount = @InvoiceAmount,
    PaidAmount = @PaidAmount,
    BalanceAmount = @BalanceAmount,
    BillingMode = @BillingMode,
    Status = @Status,
    UpdatedBy = @UpdatedBy,
    UpdatedDate = SYSDATETIME()
WHERE OutstandingID = @OutstandingID";

                        await _db.ExecuteAsync(updateOutSql, new
                        {
                            OutstandingID = existingOutID,
                            InvoiceAmount = invoiceAmount,
                            PaidAmount = paidAmount,
                            BalanceAmount = balanceAmount,
                            header.BillingMode,
                            Status = status,
                            header.UpdatedBy
                        }, tran);
                    }
                    else
                    {
                        const string insertOutSql = @"
INSERT INTO CustomerOutstanding
(
    CompanyID, CompanyName, BranchID, BranchName,
    CustomerID, CustomerName, CustomerContact, CustomerGSTIN, CustomerState,
    InvoiceID, InvoiceNumber, InvoiceDate, BillingMode,
    InvoiceAmount, PaidAmount, BalanceAmount, Status,
    Remarks, IsActive, CreatedBy, CreatedDate
)
VALUES
(
    @CompanyID, @CompanyName, @BranchID, @BranchName,
    @CustomerID, @CustomerName, @CustomerContact, @CustomerGSTIN, @CustomerState,
    @InvoiceID, @InvoiceNumber, @InvoiceDate, @BillingMode,
    @InvoiceAmount, @PaidAmount, @BalanceAmount, @Status,
    @Remarks, 1, @CreatedBy, SYSDATETIME()
)";

                        await _db.ExecuteAsync(insertOutSql, new
                        {
                            header.CompanyID,
                            header.CompanyName,
                            header.BranchID,
                            header.BranchName,
                            header.CustomerID,
                            header.CustomerName,
                            header.CustomerContact,
                            header.CustomerGSTIN,
                            header.CustomerState,
                            InvoiceID = lastInvoiceID,
                            header.InvoiceNumber,
                            header.InvoiceDate,
                            header.BillingMode,
                            InvoiceAmount = invoiceAmount,
                            PaidAmount = paidAmount,
                            BalanceAmount = balanceAmount,
                            Status = status,
                            header.Remarks,
                            header.CreatedBy
                        }, tran);
                    }

                    tran.Commit();
                    return new InvoiceSaveResult
                    {
                        InvoiceID = lastInvoiceID,
                        InvoiceNumber = header.InvoiceNumber
                    };

                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }



        }

        private void ValidateSalesEntry(SalesEntryMaster entry)
        {
            // --- REQUIRED NUMERIC IDs ---
            if (entry.CompanyID <= 0)
                throw new Exception("CompanyID is required and must be greater than 0.");

            /*

             if (entry.BranchID <= 0)
                 throw new Exception("BranchID is required and must be greater than 0.");

             if (entry.CustomerID == null || entry.CustomerID <= 0)
                 throw new Exception("CustomerID is required and must be greater than 0.");

             if (entry.ProductID == null || entry.ProductID <= 0)
                 throw new Exception("ProductID is required and must be greater than 0.");

             // --- QUANTITY VALIDATION ---
             if (entry.Quantity <= 0)
                 throw new Exception("Quantity must be greater than 0.");

             // --- RATE VALIDATION ---
             if (entry.SaleRate == null || entry.SaleRate < 0)
                 throw new Exception("SaleRate cannot be empty or negative.");

             if (entry.ProductRate == null || entry.ProductRate < 0)
                 throw new Exception("ProductRate cannot be empty or negative.");

             // --- TOTAL VALIDATION ---
             if (entry.GrandTotal == null || entry.GrandTotal < 0)
                 throw new Exception("GrandTotal must not be null or negative.");

             // --- STRING FIELDS ---
             if (string.IsNullOrWhiteSpace(entry.BillingMode))
                 throw new Exception("BillingMode is required.");
            */
        }

        public async Task<IEnumerable<ProductStockPriceDto>> GetProductStockAndPriceAsync(
        int? companyId = null,
        int? branchId = null,
        int? businessTypeId = null)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@CompanyID", companyId);
            parameters.Add("@BranchID", branchId);
            parameters.Add("@BusinessTypeID", businessTypeId);  

            if (_db.State == ConnectionState.Closed)
                _db.Open();

            var list = await _db.QueryAsync<ProductStockPriceDto>(
                "GetProductStockAndPrice",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return list;
        }



        public async Task<int> SaveBusinessTypeAsync(BusinessType model)
        {
            if (model.BusinessTypeID == 0)
            {
                // INSERT
                var query = @"
            INSERT INTO BusinessType
                (CompanyID, BusinessTypeName, Description, IsActive,
                 CreatedBy, CreatedDate, CreatedSystemName)
            VALUES
                (@CompanyID, @BusinessTypeName, @Description, @IsActive,
                 @CreatedBy, GETDATE(), @CreatedSystemName);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
        ";

                return await _db.ExecuteScalarAsync<int>(query, model);
            }
            else
            {
                // UPDATE
                var query = @"
            UPDATE BusinessType
            SET BusinessTypeName = @BusinessTypeName,
                Description = @Description,
                IsActive = @IsActive, 
                UpdatedBy = @UpdatedBy,
                UpdatedDate = GETDATE()
            WHERE BusinessTypeID = @BusinessTypeID;
        ";

                await _db.ExecuteAsync(query, model);
                return model.BusinessTypeID;
            }
        }
        public async Task<IEnumerable<BusinessType>> GetAllAsync()
        {
            var query = @"
        SELECT BusinessTypeID, CompanyID, BusinessTypeName, Description,
               IsActive, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate, CreatedSystemName
        FROM BusinessType
        WHERE IsActive = 1
        ORDER BY BusinessTypeName ASC;
    ";

            return await _db.QueryAsync<BusinessType>(query);
        }


        public async Task<int> SaveGstAsync(GstTransactionType model, string action)
        {
            if (action == "insert" && model.GstTransactionTypeID == 0)
            {
                var q = @"
                INSERT INTO GstTransactionType
                    (TransactionTypeName, Description, IsActive,
                     CreatedBy, CreatedDate, CreatedSystemName)
                VALUES
                    (@TransactionTypeName, @Description, @IsActive,
                     @CreatedBy, GETDATE(), @CreatedSystemName);

                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

                return await _db.ExecuteScalarAsync<int>(q, model);
            }

            if (action == "update" && model.GstTransactionTypeID > 0)
            {
                var q = @"
                UPDATE GstTransactionType
                SET TransactionTypeName = @TransactionTypeName,
                    Description = @Description,
                    IsActive = @IsActive,
                    UpdatedBy = @UpdatedBy,
                    UpdatedDate = GETDATE()
                WHERE GstTransactionTypeID = @GstTransactionTypeID;
            ";

                return await _db.ExecuteAsync(q, model);
            }

            if (action == "delete" && model.GstTransactionTypeID > 0)
            {
                var q = @"
                UPDATE GstTransactionType
                SET IsActive = 0,
                    UpdatedDate = GETDATE()
                WHERE GstTransactionTypeID = @GstTransactionTypeID;
            ";

                return await _db.ExecuteAsync(q, new { model.GstTransactionTypeID });
            }

            return 0;
        }
        public async Task<IEnumerable<GstTransactionType>> GetAllGstAsync()
        {
            var q = @"
        SELECT GstTransactionTypeID, TransactionTypeName, Description,
               IsActive, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate, CreatedSystemName
        FROM GstTransactionType
        WHERE IsActive = 1
        ORDER BY TransactionTypeName ASC;
    ";

            return await _db.QueryAsync<GstTransactionType>(q);
        }


        public async Task<string> GetNextInvoiceNumberAsync(int companyId, string? branchId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyID", companyId);

            parameters.Add("@BranchID", string.IsNullOrWhiteSpace(branchId) ? "" : branchId);

            if (_db.State == ConnectionState.Closed)
                _db.Open();

            var result = await _db.QueryFirstOrDefaultAsync<string?>(
                "sp_GetNextInvoiceNumber",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result ?? "INV00001";  
        }


        public async Task<IEnumerable<SalesEntryMaster>> GetSalesEntriesAsync(
            int? companyId,
            int? branchId,
            string invoiceNumber = null)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyID", companyId);
            parameters.Add("@BranchID", branchId);
            parameters.Add("@InvoiceNumber", invoiceNumber);

            if (_db.State == ConnectionState.Closed)
                _db.Open();

            try
            {
                var result = await _db.QueryAsync<SalesEntryMaster>(
                    "sp_GetSalesEntries",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return result;
            }
            finally
            {
                if (_db.State == ConnectionState.Open)
                    _db.Close();
            }
        }


    }

}
