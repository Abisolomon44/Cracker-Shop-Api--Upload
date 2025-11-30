using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cracker_Shop.Models.Purchase
{
    [Table("PurchaseEntry")]
    public class PurchaseEntry
    {
        [Key]
        public int? PurchaseID { get; set; }

        public string? PONumber { get; set; }
        public DateTime? PurchaseDate { get; set; }

        public int? CompanyID { get; set; }
        public string? CompanyName { get; set; }
        public int? BranchID { get; set; }
        public string? BranchName { get; set; }

        public int? SupplierID { get; set; }
        public string? SupplierName { get; set; }

        public int? StatusID { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? SupplierInvoiceNumber { get; set; }
        public DateTime? SupplierInvoiceDate { get; set; }

        public int? BrandID { get; set; }
        public int? UnitID { get; set; }
        public int? HSNID { get; set; }
        public int? CategoryID { get; set; }
        public int? SubCategoryID { get; set; }

        public string? Barcode { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }

        public decimal? ProductRate { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal? WholesalePrice { get; set; }
        public decimal? SaleRate { get; set; }
        public decimal? MRP { get; set; }

        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercentage { get; set; }

        public decimal? InclusiveAmount { get; set; }
        public decimal? ExclusiveAmount { get; set; }

        public decimal? GstPercentage { get; set; }
        public decimal? GstAmount { get; set; }
        public decimal? CGSTRate { get; set; }
        public decimal? CGSTAmount { get; set; }
        public decimal? SGSTRate { get; set; }
        public decimal? SGSTAmount { get; set; }
        public decimal? IGSTRate { get; set; }
        public decimal? IGSTAmount { get; set; }
        public decimal? CESSRate { get; set; }
        public decimal? CESSAmount { get; set; }

        public decimal? TaxableValue { get; set; }
        public bool? IsGSTInclusive { get; set; }

        public decimal? OrderedQuantity { get; set; }
        public decimal? ReceivedQuantity { get; set; }
        public decimal? ReturnedQuantity { get; set; }
        public decimal? RemainingQuantity { get; set; }
        public decimal? OpeningStock { get; set; }
        public decimal? ReorderLevel { get; set; }
        public decimal? CurrentStock { get; set; }

        public string? Color { get; set; }
        public string? Size { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Volume { get; set; }
        public string? Material { get; set; }
        public string? FinishType { get; set; }
        public string? ShadeCode { get; set; }
        public string? Capacity { get; set; }
        public string? ModelNumber { get; set; }

        public DateTime? ExpiryDate { get; set; }
        public bool? IsService { get; set; }
        public string? StatusName { get; set; }

        public decimal? TotalAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? GrandTotal { get; set; }

        public string? Remarks { get; set; }
        public bool? IsActive { get; set; }

        public int? CreatedByUserID { get; set; }
        public string? CreatedSystemName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedByUserID { get; set; }
        public string? UpdatedSystemName { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string? GRNNumber { get; set; }
        public DateTime? GRNDate { get; set; }

        public int? POID { get; set; }
        public int? PODetailID { get; set; }
        public string? GRNRemarks { get; set; }
        public bool? IsGRNApproved { get; set; }
        public int? ApprovedByUserID { get; set; }
        public DateTime? ApprovedAt { get; set; }

        public string? PaymentMode { get; set; }
        public int? PaidDays { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public string? TaxType { get; set; }
        public int? SecondaryUnitID { get; set; }

        public DateTime? CancelledDate { get; set; }
        public string? CancelledBy { get; set; }
        public string? CancelReason { get; set; }
        public string? AccountingYear { get; set; }
        // ------- TOTALS SECTION -------
        public decimal TotalGrossAmount { get; set; }
        public decimal TotalDiscAmount { get; set; }
        public decimal TotalTaxableAmount { get; set; }
        public decimal TotalGstAmount { get; set; }
        public decimal TotalCessAmount { get; set; }
        public decimal TotalNetAmount { get; set; }
        public decimal? TotalInvoiceAmount { get; set; }
        public decimal? TotalPaidAmount { get; set; }

        public decimal TotalBalanceAmount { get; set; }
        public decimal TotalRoundOff { get; set; }
        public decimal? QuantityPurchased { get; set; }
        public decimal? QuantitySold { get; set; }
        public decimal? QuantityReturned { get; set; }


    }


    [Table("PurchaseEntryStock")]
    public class PurchaseEntryStock
    {
        [Key]
        public int? StockID { get; set; }
        public int? PurchaseID { get; set; }
        public string? PONumber { get; set; }
        public DateTime? PurchaseDate { get; set; }

        public int? CompanyID { get; set; }
        public string? CompanyName { get; set; }
        public int? BranchID { get; set; }
        public string? BranchName { get; set; }

        public int? SupplierID { get; set; }
        public string? SupplierName { get; set; }

        public int? StatusID { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? SupplierInvoiceNumber { get; set; }
        public DateTime? SupplierInvoiceDate { get; set; }

        public int? BrandID { get; set; }
        public int? UnitID { get; set; }
        public int? HSNID { get; set; }
        public int? CategoryID { get; set; }
        public int? SubCategoryID { get; set; }

        public string? Barcode { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }

        public decimal? ProductRate { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal? WholesalePrice { get; set; }
        public decimal? SaleRate { get; set; }
        public decimal? MRP { get; set; }

        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercentage { get; set; }

        public decimal? InclusiveAmount { get; set; }
        public decimal? ExclusiveAmount { get; set; }

        public decimal? GstPercentage { get; set; }
        public decimal? GstAmount { get; set; }
        public decimal? CGSTRate { get; set; }
        public decimal? CGSTAmount { get; set; }
        public decimal? SGSTRate { get; set; }
        public decimal? SGSTAmount { get; set; }
        public decimal? IGSTRate { get; set; }
        public decimal? IGSTAmount { get; set; }
        public decimal? CESSRate { get; set; }
        public decimal? CESSAmount { get; set; }

        public decimal? TaxableValue { get; set; }
        public bool? IsGSTInclusive { get; set; }

        public decimal? QuantityPurchased { get; set; }
        public decimal? QuantitySold { get; set; }
        public decimal? QuantityReturned { get; set; }

        public decimal? OpeningStock { get; set; }
        public decimal? CurrentStock { get; set; }
        public decimal? ReorderLevel { get; set; }

        public string? Color { get; set; }
        public string? Size { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Volume { get; set; }
        public string? Material { get; set; }
        public string? FinishType { get; set; }
        public string? ShadeCode { get; set; }
        public string? Capacity { get; set; }
        public string? ModelNumber { get; set; }

        public DateTime? ExpiryDate { get; set; }
        public bool? IsService { get; set; }
        public string? StatusName { get; set; }

        public decimal? TotalAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? GrandTotal { get; set; }

        public string? Remarks { get; set; }
        public bool? IsActive { get; set; }

        public int? CreatedByUserID { get; set; }
        public string? CreatedSystemName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedByUserID { get; set; }
        public string? UpdatedSystemName { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string? GRNNumber { get; set; }
        public DateTime? GRNDate { get; set; }

        public int? POID { get; set; }
        public int? PODetailID { get; set; }
        public string? GRNRemarks { get; set; }
        public bool? IsGRNApproved { get; set; }
        public int? ApprovedByUserID { get; set; }
        public DateTime? ApprovedAt { get; set; }

        public DateTime? CancelledDate { get; set; }
        public string? CancelledBy { get; set; }
        public string? CancelReason { get; set; }
        public string? AccountingYear { get; set; }


        public decimal TotalGrossAmount { get; set; }
        public decimal TotalDiscAmount { get; set; }
        public decimal TotalTaxableAmount { get; set; }
        public decimal TotalGstAmount { get; set; }
        public decimal TotalCessAmount { get; set; }
        public decimal TotalNetAmount { get; set; }
        public decimal? TotalInvoiceAmount { get; set; }
        public decimal? TotalPaidAmount { get; set; }

        public decimal TotalBalanceAmount { get; set; }
        public decimal TotalRoundOff { get; set; }




    }
}
