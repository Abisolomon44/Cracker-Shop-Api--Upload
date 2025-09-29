namespace Cracker_Shop.Models.CommonMasterModels
{
    public class BranchMaster
    {
  
            public long BranchID { get; set; }
            public long CompanyID { get; set; }
            public string BranchCode { get; set; } = string.Empty;
            public string BranchName { get; set; } = string.Empty;
            public string? Address { get; set; }
            public bool IsActive { get; set; } = true;

            public long? CreatedByUserID { get; set; }
            public string? CreatedSystemName { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.Now;

            public long? UpdatedByUserID { get; set; }
            public string? UpdatedSystemName { get; set; }
            public DateTime? UpdatedAt { get; set; }
        
    }
}
