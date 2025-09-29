namespace Cracker_Shop.Models.CommonMasterModels
{
    public class DepartmentMaster
    {
        public long DepartmentID { get; set; }
        public long BranchID { get; set; }
        public string DepartmentCode { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public long? CreatedByUserID { get; set; }
        public string? CreatedSystemName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public long? UpdatedByUserID { get; set; }
        public string? UpdatedSystemName { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
