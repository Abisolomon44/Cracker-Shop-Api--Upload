using Cracker_Shop.Models.Purchase;
using Cracker_Shop.Models.Sales;

namespace Cracker_Shop.Repository.IRepository
{

     public interface ISalesRepository
      {
        Task<InvoiceSaveResult> AddOrUpdateSalesEntryWithStockAsync(List<SalesEntryMaster> entries);
        Task<IEnumerable<ProductStockPriceDto>> GetProductStockAndPriceAsync(
           int? companyId = null,
           int? branchId = null,
           int? businessTypeId = null);

        Task<int> SaveBusinessTypeAsync(BusinessType model);
        Task<IEnumerable<BusinessType>> GetAllAsync();

        Task<int> SaveGstAsync(GstTransactionType model, string action);
        Task<IEnumerable<GstTransactionType>> GetAllGstAsync();
        Task<string> GetNextInvoiceNumberAsync(int companyId, string? branchId);
        Task<IEnumerable<SalesEntryMaster>> GetSalesEntriesAsync(
        int? companyId,
        int? branchId,
        string invoiceNumber = null);



    }

}
