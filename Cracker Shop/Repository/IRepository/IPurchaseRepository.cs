using Cracker_Shop.Models.Purchase;

namespace Cracker_Shop.Repository.IRepository
{
    public interface IPurchaseRepository
    {
        Task<long> AddUpDateDeletePurchaseOrderAsync(PurchaseOrderEntry po);
        Task<long> AddUpdateDeleteGRNAsync(GRNEntry grn);
        Task<int> AddOrUpdatePurchaseEntryWithStockAsync(List<PurchaseEntry> entries);
    }
}
