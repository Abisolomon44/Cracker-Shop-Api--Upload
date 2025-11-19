using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cracker_Shop.Models.Purchase
{
    [Table("GRNEntry")]
    public class GRNEntry
    {
        [Key]
        public long? GRNEntryID { get; set; }

        public string? GRNNumber { get; set; }
        public DateTime? GRNDate { get; set; }

        public long? POID { get; set; }
        public long? PODetailID { get; set; }
        public long? PurchaseID { get; set; }

        public long? SupplierID { get; set; }
        public string? SupplierName { get; set; }

        public long? CompanyID { get; set; }
        public long? BranchID { get; set; }

        public string? InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }

        public string? TransportName { get; set; }
        public string? VehicleNumber { get; set; }
        public string? ReceivedBy { get; set; }

        public long? ProductID { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public long? UnitID { get; set; }

        public decimal? ReceivedQty { get; set; }
        public decimal? AcceptedQty { get; set; }
        public decimal? RejectedQty { get; set; }
        public decimal? OrderedQty { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal? TaxPercentage { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? TotalAmount { get; set; }

        public string? Remarks { get; set; }
        public bool? IsApproved { get; set; }
        public long? StatusID { get; set; }
        public string? StatusName { get; set; }
        public long? ApprovedBy { get; set; } 
        public DateTime? ApprovedAt { get; set; }

        public bool? IsActive { get; set; }

        public long? CreatedBy { get; set; } 
        public DateTime? CreatedAt { get; set; }

        public long? UpdatedBy { get; set; } // ✅ bigint
        public DateTime? UpdatedAt { get; set; }
    }
}
