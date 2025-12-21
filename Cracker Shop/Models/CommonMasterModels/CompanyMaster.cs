namespace Cracker_Shop.Models.CommonMasterModels
{
    public class CompanyMaster
    {
        public long CompanyID { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
        public string AlternatePhone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;

        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public string AddressLine3 { get; set; } = string.Empty;
        public string AddressLine4 { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;

        public string GSTNumber { get; set; } = string.Empty;
        public string PANNumber { get; set; } = string.Empty;
        public string CINNumber { get; set; } = string.Empty;

        public string BankName { get; set; } = string.Empty;
        public string BankBranch { get; set; } = string.Empty;
        public string BankAccountNumber { get; set; } = string.Empty;
        public string IFSCCode { get; set; } = string.Empty;
        public string SwiftCode { get; set; } = string.Empty;

        public byte[]? CompanyLogo { get; set; }
        public byte[]? CompanyImage { get; set; }

        public bool IsActive { get; set; }

        public long? CreatedByUserID { get; set; }
        public string CreatedSystemName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }

        public long? UpdatedByUserID { get; set; }
        public string UpdatedSystemName { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }

        public string BusinessType { get; set; } = string.Empty;
        public string BusinessCategory { get; set; } = string.Empty;
        public string BusinessSubCategory { get; set; } = string.Empty;

        public string StateName { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;

        public string TimeZone { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string CurrencySymbol { get; set; } = string.Empty;
        public string LanguageCompany { get; set; } = string.Empty;
        public string DateFormat { get; set; } = string.Empty;

        public string WhatsAppNumber { get; set; } = string.Empty;
        public string Tagline { get; set; } = string.Empty;

        public DateTime? FinancialYearStart { get; set; }
        public DateTime? FinancialYearEnd { get; set; }
        public string AccountingYear { get; set; } = string.Empty;
    }

    public class RegisterRequestDto
    {
        public string CompanyName { get; set; } = "";
        public string CompanyEmail { get; set; } = "";
        public string Phone { get; set; } = "";

        public string UserName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public string PasswordHash { get; set; } = "";
    }

    public class RegisterResultDto
    {
        public bool Success { get; set; }
        public int CompanyID { get; set; }
    }
    public class CompanyDashboardDto
    {
        public decimal TotalSalesAmount { get; set; }
        public decimal CustomerOutstandingAmount { get; set; }
        public decimal TotalPurchaseAmount { get; set; }
        public decimal SupplierOutstandingAmount { get; set; }
        public decimal TotalPurchasedQuantity { get; set; }
        public decimal TotalSoldQuantity { get; set; }
        public decimal CurrentStockQuantity { get; set; }
        public int CustomerCount { get; set; }
        public int SupplierCount { get; set; }
        public int ProductCount { get; set; }
    }
    public class BranchCountDto
    {
        public int ActiveBranchCount { get; set; }
    }
    public class CompanyDashboardResponseDto
    {
        public CompanyDashboardDto Dashboard { get; set; } = new();
        public int ActiveBranchCount { get; set; }
    }

}
