namespace Cracker_Shop.Models.MasterModels
{
    public class HSNCodeMaster
    {
        public long HSNID { get; set; }
        public string HSNCode { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long TaxID { get; set; }
        public bool IsActive { get; set; }

        public int? CreatedByUserID { get; set; }
        public string? CreatedSystemName { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? UpdatedByUserID { get; set; }
        public string? UpdatedSystemName { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
