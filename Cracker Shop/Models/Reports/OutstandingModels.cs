namespace Cracker_Shop.Models.Reports
{
    public class CustomerOutstandingReportModel
    {
        public int? CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerContact { get; set; }
        public string? CustomerGSTIN { get; set; }
        public string? Area { get; set; }
        public DateTime? OutstandingDate { get; set; }

        public int InvoiceCount { get; set; }

        public decimal InvoiceAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
    }

    public class SupplierOutstandingReportModel
    {
        public int? SupplierID { get; set; }
        public string? SupplierName { get; set; }
        public string? Area { get; set; }
        public DateTime? OutstandingDate { get; set; }

        public int PurchaseCount { get; set; }

        public decimal BillAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
    }
}
