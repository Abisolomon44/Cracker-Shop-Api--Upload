using Cracker_Shop.Models.Purchase;
using Cracker_Shop.Models.Sales;

namespace Cracker_Shop.Repository.IRepository
{

     public interface ISalesRepository
      {
        Task<int> AddOrUpdateSalesEntryWithStockAsync(List<SalesEntryMaster> entries);
        Task<IEnumerable<ProductStockPriceDto>> GetProductStockAndPriceAsync(
        int? companyId = null,
        int? branchId = null


    );

        Task<int> SaveBUsinessTypeAsync(BusinessType model);
        Task<IEnumerable<BusinessType>> GetAllAsync();

        Task<int> SaveAsync(GstTransactionType model, string action);
        Task<IEnumerable<GstTransactionType>> GetAllGstAsync();

    }

}
