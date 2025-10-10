namespace Cracker_Shop.Models.MasterModels
{
    public class ProductImageMaster
    {
        public long ProductImageID { get; set; }
        public long ProductID { get; set; }

        public byte[]? ImageData { get; set; }
        public string? ImageName { get; set; }
        public string? ContentType { get; set; }

        public bool IsPrimary { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsActive { get; set; }

        public int? CreatedByUserID { get; set; }
        public string? CreatedSystemName { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? UpdatedByUserID { get; set; }
        public string? UpdatedSystemName { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
