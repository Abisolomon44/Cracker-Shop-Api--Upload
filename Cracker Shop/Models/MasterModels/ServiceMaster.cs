namespace Cracker_Shop.Models.MasterModels
{
    public class ServiceMaster
    {
        public long ServiceID { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceCode { get; set; } = string.Empty;
        public long? CategoryID { get; set; }
        public long? HSNID { get; set; }
        public long? TaxID { get; set; }
        public long? CessID { get; set; }
        public decimal? ServiceCharge { get; set; }
        public bool IsActive { get; set; } = true;
        public long? CreatedByUserID { get; set; }
        public string? CreatedSystemName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long? UpdatedByUserID { get; set; }
        public string? UpdatedSystemName { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
