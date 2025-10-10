using Cracker_Shop.Models.CommonMasterModels;
using Cracker_Shop.Models.MasterModels;

namespace Cracker_Shop.Repository.IRepository
{
    public interface IMasterRepository
    {
        Task<long> AddUpdateDelete(BrandMaster brand);
        Task<IEnumerable<BrandMaster>> GetActiveBrands();


        Task<long> SaveCategoryAsync(CategoryMaster category);
        Task<IEnumerable<CategoryMaster>> GetActiveCategoriesAsync();

        Task<long> SaveCessAsync(CessMaster cess);
        Task<IEnumerable<CessMaster>> GetActiveCessAsync();

        Task<long> SaveCustomerAsync(CustomerMaster customer);
        Task<IEnumerable<CustomerMaster>> GetActiveCustomersAsync();

        Task<long> SaveHSNCodeAsync(HSNCodeMaster hsn);
        Task<IEnumerable<HSNCodeMaster>> GetActiveHSNCodesAsync();

        Task<long> SavePaymentModeAsync(PaymentModeMaster paymentMode);
        Task<IEnumerable<PaymentModeMaster>> GetActivePaymentModesAsync();

        Task<long> SaveProductImageAsync(ProductImageMaster image);
        Task<IEnumerable<ProductImageMaster>> GetActiveProductImagesAsync(long productId);

        // Product
        Task<long> SaveProductAsync(ProductMaster product);
        Task<IEnumerable<ProductMaster>> GetActiveProductsAsync(long companyId);

        Task<long> SaveServiceAsync(ServiceMaster service);
        Task<IEnumerable<ServiceMaster>> GetActiveServicesAsync();

        Task<long> SaveSubCategoryAsync(SubCategoryMaster subCategory);
        Task<IEnumerable<SubCategoryMaster>> GetActiveSubCategoriesAsync(long? categoryId = null);

        Task<long> SaveSupplierAsync(SupplierMaster supplier);
        Task<IEnumerable<SupplierMaster>> GetActiveSuppliersAsync();

        Task<long> SaveTaxAsync(TaxMaster tax);
        Task<IEnumerable<TaxMaster>> GetActiveTaxesAsync();

        Task<long> SaveUnitAsync(UnitMaster unit);
        Task<IEnumerable<UnitMaster>> GetActiveUnitsAsync();
    }
}
