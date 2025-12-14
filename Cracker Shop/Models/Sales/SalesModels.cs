namespace Cracker_Shop.Models.Sales
{
    public class SalesEntryMaster
    {
        public long InvoiceID { get; set; }
        public string InvoiceNumber { get; set; } = "";
        public DateTime? InvoiceDate { get; set; }

        public long CompanyID { get; set; }
        public string CompanyName { get; set; } = "";

        public long BranchID { get; set; }
        public string BranchName { get; set; } = "";

        public long CustomerID { get; set; }
        public string CustomerName { get; set; } = "";
        public string CustomerContact { get; set; } = "";
        public string CustomerGSTIN { get; set; } = "";
        public string CustomerState { get; set; } = "";
        public string CompanyState { get; set; } = "";

        public string AccountingYear { get; set; } = "";
        public string BillingType { get; set; } = "";
        public bool? IsGSTApplicable { get; set; }
        public string GSTType { get; set; } = "";

        public long ProductID { get; set; }
        public string Barcode { get; set; } = "";
        public string ProductCode { get; set; } = "";
        public string ProductName { get; set; } = "";

        public long? BrandID { get; set; }
        public long? CategoryID { get; set; }
        public long? SubCategoryID { get; set; }
        public long? HSNID { get; set; }
        public long? UnitID { get; set; }
        public long? SecondaryUnitID { get; set; }

        public string Color { get; set; } = "";
        public string Size { get; set; } = "";
        public decimal? Weight { get; set; }      // FIXED: decimal
        public decimal? Volume { get; set; }      // FIXED: decimal
        public string Material { get; set; } = "";
        public string FinishType { get; set; } = "";
        public string ShadeCode { get; set; } = "";
        public string Capacity { get; set; } = "";
        public string ModelNumber { get; set; } = "";

        public DateTime? ExpiryDate { get; set; }
        public DateTime? ManufacturingDate { get; set; }

        // Qty + Prices
        public decimal Quantity { get; set; }               // NOT NULL IN SQL
        public decimal? ProductRate { get; set; }
        public decimal? SaleRate { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal? WholesalePrice { get; set; }
        public decimal? MRP { get; set; }

        // Discount
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }

        // Amounts
        public decimal? InclusiveAmount { get; set; }
        public decimal? ExclusiveAmount { get; set; }

        // GST
        public decimal? GstPercentage { get; set; }
        public decimal? GstAmount { get; set; }
        public decimal? CGSTRate { get; set; }
        public decimal? CGSTAmount { get; set; }
        public decimal? SGSTRate { get; set; }
        public decimal? SGSTAmount { get; set; }
        public decimal? IGSTRate { get; set; }
        public decimal? IGSTAmount { get; set; }
        public decimal? CESSRate { get; set; }
        public decimal? CESSAmount { get; set; }

        public decimal? GrossAmount { get; set; }
        public decimal? CustomerDiscount { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? TaxableAmount { get; set; }
        public decimal? GrandTotal { get; set; }

        // Billing
        public string BillingMode { get; set; } = "";
        public decimal? CashAmount { get; set; }
        public decimal? CardAmount { get; set; }
        public decimal? UPIAmount { get; set; }
        public decimal? AdvanceAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? BalanceAmount { get; set; }

        public string Status { get; set; } = "";
        public bool? IsActive { get; set; }
        public bool? IsService { get; set; }
        public string Remarks { get; set; } = "";

        public string CreatedBy { get; set; } = "";
        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; } = "";
        public DateTime? UpdatedDate { get; set; }

        public DateTime? CancelledDate { get; set; }
        public string CancelledBy { get; set; } = "";
        public string CancelReason { get; set; } = "";

        // Totals
        public decimal? TotalSaleRate { get; set; }
        public decimal? TotalDiscountAmount { get; set; }
        public decimal? TotalCGSTAmount { get; set; }
        public decimal? TotalSGSTAmount { get; set; }
        public decimal? TotalIGSTAmount { get; set; }
        public decimal? TotalCESSAmount { get; set; }

        public decimal? TotalGrossAmount { get; set; }
        public decimal? TotalDiscAmount { get; set; }
        public decimal? TotalTaxableAmount { get; set; }
        public decimal? TotalGstAmount { get; set; }
        public decimal? TotalNetAmount { get; set; }
        public decimal? TotalInvoiceAmount { get; set; }
        public decimal? TotalPaidAmount { get; set; }
        public decimal? TotalBalanceAmount { get; set; }
        public decimal? TotalRoundOff { get; set; }
        public decimal? TotalQuantity { get; set; }



    }

    public class InvoiceSaveResult
    {
        public int InvoiceID { get; set; }
        public string InvoiceNumber { get; set; }
    }

    public class ProductStockPriceDto
    {
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }

        public decimal RetailPrice { get; set; }
        public decimal WholesalePrice { get; set; }
        public decimal MRP { get; set; }

        public decimal GstPercentage { get; set; }
        public decimal CurrentStock { get; set; }
    }
    public class BusinessType
    {
        public int BusinessTypeID { get; set; }
        public int? CompanyID { get; set; }
        public string BusinessTypeName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedSystemName { get; set; }
    }

    public class GstTransactionType
    {
        public int GstTransactionTypeID { get; set; }
        public string TransactionTypeName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedSystemName { get; set; }
    }


}
