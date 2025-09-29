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
            public string BankAccountNumber { get; set; } = string.Empty;
            public string IFSCCode { get; set; } = string.Empty;
        public byte[]? CompanyLogo { get; set; }
        public byte[]? CompanyImage { get; set; }

        public bool IsActive { get; set; } 
            public long? CreatedByUserID { get; set; }
            public string CreatedSystemName { get; set; } = string.Empty;
            public long? UpdatedByUserID { get; set; }
            public string UpdatedSystemName { get; set; } = string.Empty;
        }

    
}
