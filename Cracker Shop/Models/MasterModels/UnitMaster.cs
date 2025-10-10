namespace Cracker_Shop.Models.MasterModels
{
    public class UnitMaster
    {
        public long UnitID { get; set; }
        public string UnitName { get; set; } = string.Empty;
        public string? UnitCode { get; set; }
        public bool IsActive { get; set; } = true;
        public long? CreatedByUserID { get; set; }
        public string? CreatedSystemName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long? UpdatedByUserID { get; set; }
        public string? UpdatedSystemName { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
