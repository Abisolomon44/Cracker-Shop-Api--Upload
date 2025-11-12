using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cracker_Shop.Models.Purchase
{
    [Table("PurchaseOrderEntry")]
    public class PurchaseOrderEntry
    {
        [Key]
        public int? POID { get; set; }

        public int? CompanyID { get; set; }
        public string? CompanyName { get; set; }
        public int? BranchID { get; set; }
        public string? BranchName { get; set; }

        public string? PONumber { get; set; }
        public DateTime? PODate { get; set; }

        public int? SupplierID { get; set; }
        public string? SupplierName { get; set; }

        public int? StatusID { get; set; }
        public string? StatusName { get; set; }

        public decimal? TotalAmount { get; set; }
        public string? PORemarks { get; set; }

        public int? ProductID { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public int? ProductCategoryId { get; set; }
        public string? ProductCategoryName { get; set; }
        public int? ProductSubCategory { get; set; }
        public string? ProductSubCategoryName { get; set; }

        public decimal? PORate { get; set; }
        public decimal? OrderedQty { get; set; }
        public decimal? ApprovedQty { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string? ProductRemarks { get; set; }

        public bool? IsActive { get; set; }
        public int? CreatedByUserID { get; set; }
        public string? CreatedSystemName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedByUserID { get; set; }
        public string? UpdatedSystemName { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public DateTime? CancelledDate { get; set; }
        public string? CancelledBy { get; set; }
        public string? CancelReason { get; set; }
        public string? AccountingYear { get; set; }
    }
}
