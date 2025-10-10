namespace Cracker_Shop.Models.MasterModels
{
    public class SubCategoryMaster
    {
        public long SubCategoryID { get; set; }
        public long? CategoryID { get; set; }
        public string SubCategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public long? CreatedByUserID { get; set; }
        public string? CreatedSystemName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long? UpdatedByUserID { get; set; }
        public string? UpdatedSystemName { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
