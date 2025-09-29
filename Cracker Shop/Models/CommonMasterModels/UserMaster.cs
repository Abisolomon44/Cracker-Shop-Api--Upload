namespace Cracker_Shop.Models.CommonMasterModels
{
    public class UserMaster
    {
        public long UserID { get; set; }
        public long CompanyID { get; set; }
        public long BranchID { get; set; }
        public long DepartmentID { get; set; }
        public long RoleID { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public long? CreatedByUserID { get; set; }
        public string? CreatedSystemName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public long? UpdatedByUserID { get; set; }
        public string? UpdatedSystemName { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
