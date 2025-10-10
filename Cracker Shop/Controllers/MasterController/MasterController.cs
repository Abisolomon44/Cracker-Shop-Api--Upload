using Cracker_Shop.Models.MasterModels;
using Cracker_Shop.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Cracker_Shop.Controllers.MasterController
{
    [ApiController]
    [Route("api/[controller]")]
    public class MasterController : ControllerBase
    {
        private readonly IMasterRepository _repo;

        public MasterController(IMasterRepository repo)
        {
            _repo = repo;
        }

        // ---------- Helper to generate Add/Update/Delete message ----------
        private string GetActionMessage(bool isActive, long id, long requestId, string entity)
        {
            if (!isActive)
                return $"{entity} deleted successfully.";
            else if (requestId == 0)
                return $"{entity} added successfully.";
            else
                return $"{entity} updated successfully.";
        }

        private IActionResult ResponseMessage(bool success, string message, object? data = null)
        {
            return Ok(new
            {
                Success = success,
                Message = message,
                Data = data
            });
        }

        // ================= Brand =================
        [HttpPost("Brand")]
        public async Task<IActionResult> SaveBrand([FromBody] BrandMaster brand)
        {
            if (brand == null) return BadRequest("Brand data is required.");

            try
            {
                var id = await _repo.AddUpdateDelete(brand);
                string msg = GetActionMessage(brand.IsActive, id, brand.BrandID, "Brand");
                return ResponseMessage(true, msg, new { BrandID = id });
            }
            catch (Exception ex)
            {
                return ResponseMessage(false, ex.Message);
            }
        }

        [HttpGet("Brands")]
        public async Task<IActionResult> GetBrands() =>
            Ok(await _repo.GetActiveBrands());

        // ================= Category =================
        [HttpPost("Category")]
        public async Task<IActionResult> SaveCategory([FromBody] CategoryMaster category)
        {
            if (category == null) return BadRequest("Category data is required.");

            try
            {
                var id = await _repo.SaveCategoryAsync(category);
                string msg = GetActionMessage(category.IsActive, id, category.CategoryID, "Category");
                return ResponseMessage(true, msg, new { CategoryID = id });
            }
            catch (Exception ex) { return ResponseMessage(false, ex.Message); }
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategories() =>
            Ok(await _repo.GetActiveCategoriesAsync());

        // ================= Customer =================
        [HttpPost("Customer")]
        public async Task<IActionResult> SaveCustomer([FromBody] CustomerMaster customer)
        {
            if (customer == null) return BadRequest("Customer data is required.");

            try
            {
                var id = await _repo.SaveCustomerAsync(customer);
                string msg = GetActionMessage(customer.IsActive, id, customer.CustomerID, "Customer");
                return ResponseMessage(true, msg, new { CustomerID = id });
            }
            catch (Exception ex)
            {
                return ResponseMessage(false, ex.Message);
            }
        }

        [HttpGet("Customers")]
        public async Task<IActionResult> GetCustomers() =>
            Ok(await _repo.GetActiveCustomersAsync());

        // ================= Product =================
        [HttpPost("Product")]
        public async Task<IActionResult> SaveProduct([FromBody] ProductMaster product)
        {
            if (product == null) return BadRequest("Product data is required.");

            try
            {
                var id = await _repo.SaveProductAsync(product);
                string msg = GetActionMessage(product.IsActive, id, product.ProductID, "Product");
                return ResponseMessage(true, msg, new { ProductID = id });
            }
            catch (Exception ex) { return ResponseMessage(false, ex.Message); }
        }

        [HttpGet("Products/{companyId}")]
        public async Task<IActionResult> GetProducts(long companyId) =>
            Ok(await _repo.GetActiveProductsAsync(companyId));

        // ================= Supplier =================
        [HttpPost("Supplier")]
        public async Task<IActionResult> SaveSupplier([FromBody] SupplierMaster supplier)
        {
            if (supplier == null) return BadRequest("Supplier data is required.");

            try
            {
                var id = await _repo.SaveSupplierAsync(supplier);
                string msg = GetActionMessage(supplier.IsActive, id, supplier.SupplierID, "Supplier");
                return ResponseMessage(true, msg, new { SupplierID = id });
            }
            catch (Exception ex) { return ResponseMessage(false, ex.Message); }
        }

        [HttpGet("Suppliers")]
        public async Task<IActionResult> GetSuppliers() =>
            Ok(await _repo.GetActiveSuppliersAsync());

        // ================= Tax =================
        [HttpPost("Tax")]
        public async Task<IActionResult> SaveTax([FromBody] TaxMaster tax)
        {
            if (tax == null) return BadRequest("Tax data is required.");

            try
            {
                var id = await _repo.SaveTaxAsync(tax);
                string msg = GetActionMessage(tax.IsActive, id, tax.TaxID, "Tax");
                return ResponseMessage(true, msg, new { TaxID = id });
            }
            catch (Exception ex) { return ResponseMessage(false, ex.Message); }
        }

        [HttpGet("Taxes")]
        public async Task<IActionResult> GetTaxes() =>
            Ok(await _repo.GetActiveTaxesAsync());

        // ================= Unit =================
        [HttpPost("Unit")]
        public async Task<IActionResult> SaveUnit([FromBody] UnitMaster unit)
        {
            if (unit == null) return BadRequest("Unit data is required.");

            try
            {
                var id = await _repo.SaveUnitAsync(unit);
                string msg = GetActionMessage(unit.IsActive, id, unit.UnitID, "Unit");
                return ResponseMessage(true, msg, new { UnitID = id });
            }
            catch (Exception ex) { return ResponseMessage(false, ex.Message); }
        }

        [HttpGet("Units")]
        public async Task<IActionResult> GetUnits() =>
            Ok(await _repo.GetActiveUnitsAsync());

        // ================= HSN =================
        [HttpPost("HSNCode")]
        public async Task<IActionResult> SaveHSNCode([FromBody] HSNCodeMaster hsn)
        {
            if (hsn == null) return BadRequest("HSN code data is required.");

            try
            {
                var id = await _repo.SaveHSNCodeAsync(hsn);
                string msg = GetActionMessage(hsn.IsActive, id, hsn.HSNID, "HSN code");
                return ResponseMessage(true, msg, new { HSNID = id });
            }
            catch (Exception ex) { return ResponseMessage(false, ex.Message); }
        }

        [HttpGet("HSNCodes")]
        public async Task<IActionResult> GetHSNCodes() =>
            Ok(await _repo.GetActiveHSNCodesAsync());

        // ================= Service =================
        [HttpPost("Service")]
        public async Task<IActionResult> SaveService([FromBody] ServiceMaster service)
        {
            if (service == null) return BadRequest("Service data is required.");

            try
            {
                var id = await _repo.SaveServiceAsync(service);
                string msg = GetActionMessage(service.IsActive, id, service.ServiceID, "Service");
                return ResponseMessage(true, msg, new { ServiceID = id });
            }
            catch (Exception ex) { return ResponseMessage(false, ex.Message); }
        }

        [HttpGet("Services")]
        public async Task<IActionResult> GetServices() =>
            Ok(await _repo.GetActiveServicesAsync());

        // ================= SubCategory =================
        [HttpPost("SubCategory")]
        public async Task<IActionResult> SaveSubCategory([FromBody] SubCategoryMaster subCategory)
        {
            if (subCategory == null) return BadRequest("SubCategory data is required.");

            try
            {
                var id = await _repo.SaveSubCategoryAsync(subCategory);
                string msg = GetActionMessage(subCategory.IsActive, id, subCategory.SubCategoryID, "SubCategory");
                return ResponseMessage(true, msg, new { SubCategoryID = id });
            }
            catch (Exception ex) { return ResponseMessage(false, ex.Message); }
        }

        [HttpGet("SubCategories")]
        public async Task<IActionResult> GetSubCategories(long? categoryId = null) =>
            Ok(await _repo.GetActiveSubCategoriesAsync(categoryId));
    }
}
