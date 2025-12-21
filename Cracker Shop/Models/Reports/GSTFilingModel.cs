namespace Cracker_Shop.Models.Reports
{
    public class GSTFilingModel
    {
        public string? GSTFileType { get; set; }

        public int CompanyID { get; set; }
        public int? BranchID { get; set; }

        // B2B
        public string? RecipientGSTIN { get; set; }
        public string? InvoiceNo { get; set; }
        public string? InvoiceDate { get; set; }
        public string? PlaceOfSupply { get; set; }

        // HSN
        public string? HSNCode { get; set; }
        public string? HSNDescription { get; set; }
        public decimal? TotalQuantity { get; set; }

        // Common Amounts
        public decimal InvoiceValue { get; set; }
        public decimal TaxableValue { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CESSAmount { get; set; }
    }

    public class SalesReportCommonModel
    {
        /* ---------- Invoice Level ---------- */
        public int? InvoiceID { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }

        /* ---------- Customer ---------- */
        public string? CustomerName { get; set; }
        public string? CustomerGSTIN { get; set; }
        public string? CustomerState { get; set; }

        /* ---------- Product (Detailed) ---------- */
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? SaleRate { get; set; }

        /* ---------- Tax ---------- */
        public decimal? TaxableAmount { get; set; }
        public decimal? CGSTAmount { get; set; }
        public decimal? SGSTAmount { get; set; }
        public decimal? IGSTAmount { get; set; }
        public decimal? GSTAmount { get; set; }

        /* ---------- Totals ---------- */
        public decimal? TotalQuantity { get; set; }
        public decimal? GrandTotal { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? BalanceAmount { get; set; }

        /* ---------- Payment ---------- */
        public string? BillingMode { get; set; }
        public decimal? CashAmount { get; set; }
        public decimal? CardAmount { get; set; }
        public decimal? UPIAmount { get; set; }

        /* ---------- Aggregates ---------- */
        public int? InvoiceCount { get; set; }
        public decimal? TotalSalesAmount { get; set; }

        /* ---------- Status ---------- */
        public string? Status { get; set; }

        public string? Area { get; set; } // CustomerState alias
    }

    public class ProfitReportModel
    {
        // ITEM-wise
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }

        // INVOICE-wise
        public string? InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? CustomerName { get; set; }

        // DAY-wise
        public DateTime? SaleDate { get; set; }

        // COMMON
        public decimal SoldQty { get; set; }
        public decimal AvgPurchaseRate { get; set; }   // Only ITEM-wise
        public decimal CostAmount { get; set; }
        public decimal SalesAmount { get; set; }
        public decimal ProfitAmount { get; set; }
    }
}
