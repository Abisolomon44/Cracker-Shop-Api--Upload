namespace Cracker_Shop.Models.MasterModels
{
    public class CessMaster
    {
        public long CessID { get; set; }
        public string CessName { get; set; } = string.Empty;
        public decimal CessRate { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public int? CreatedByUserID { get; set; }
        public string? CreatedSystemName { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? UpdatedByUserID { get; set; }
        public string? UpdatedSystemName { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
