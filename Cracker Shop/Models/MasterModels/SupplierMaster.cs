namespace Cracker_Shop.Models.MasterModels
{
    public class SupplierMaster
    {
        public long SupplierID { get; set; }
        public string SupplierCode { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? AlternatePhone { get; set; }
        public string? Email { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? GSTNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public long? CreatedByUserID { get; set; }
        public string? CreatedSystemName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long? UpdatedByUserID { get; set; }
        public string? UpdatedSystemName { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
