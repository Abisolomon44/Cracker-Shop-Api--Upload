namespace Cracker_Shop.Models.Reports
{
    public class TerminalReportModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public string TerminalUser { get; set; } = string.Empty;


        public string? TotalBills { get; set; }
        public decimal TotalQuantity { get; set; }

        public decimal GrossSales { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxableAmount { get; set; }

        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal CESS { get; set; }

        public decimal TotalGST { get; set; }
        public decimal NetSales { get; set; }

        public decimal CashTotal { get; set; }
        public decimal CardTotal { get; set; }
        public decimal UPITotal { get; set; }
        public decimal AdvanceTotal { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal BalanceAmount { get; set; }

        public int CancelledBills { get; set; }
        public decimal CancelledAmount { get; set; }

        public List<TerminalGSTBreakup> GSTBreakup { get; set; } = new();
    }

    public class TerminalGSTBreakup
    {
        public string? GSTType { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal CESS { get; set; }
        public decimal TotalGST { get; set; }
    }


}
