namespace Cracker_Shop.Models.MasterModels
{
    public class TaxMaster
    {
        public long TaxID { get; set; }
        public string TaxName { get; set; } = string.Empty;
        public decimal? TaxRate { get; set; }
        public decimal? CessRate { get; set; }
        public bool IsActive { get; set; } = true;
        public long? CreatedByUserID { get; set; }
        public string? CreatedSystemName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long? UpdatedByUserID { get; set; }
        public string? UpdatedSystemName { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
