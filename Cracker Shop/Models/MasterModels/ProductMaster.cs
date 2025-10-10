namespace Cracker_Shop.Models.MasterModels
{
    public class ProductMaster
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public long? CategoryID { get; set; }
        public long? SubCategoryID { get; set; }
        public long? BrandID { get; set; }
        public long? UnitID { get; set; }
        public long? HSNID { get; set; }
        public long? TaxID { get; set; }
        public long? CessID { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal? WholesalePrice { get; set; }
        public decimal? SaleRate { get; set; }
        public decimal? MRP { get; set; }
        public bool IsActive { get; set; } = true;
        public long? CreatedByUserID { get; set; }
        public string? CreatedSystemName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long? UpdatedByUserID { get; set; }
        public string? UpdatedSystemName { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? OpeningStock { get; set; }
        public decimal? ReorderLevel { get; set; }
        public decimal? CurrentStock { get; set; }
        public string? Barcode { get; set; }
        public bool IsService { get; set; } = false;
        public string? ProductDescription { get; set; }
        public byte[]? ProductImage { get; set; }

        public long CompanyID { get; set; }
    }
}
