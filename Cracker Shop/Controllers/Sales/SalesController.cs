using Cracker_Shop.Models.Purchase;
using Cracker_Shop.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

using Cracker_Shop.Models.Sales;


namespace Cracker_Shop.Controllers.Sales
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController :ControllerBase
    {


        private readonly ISalesRepository _salesRepo;

        public SalesController(ISalesRepository salesRepo)
        {
            _salesRepo = salesRepo;
        }



        // ---------- Helper: Generate success message ----------
        private string GetActionMessage(bool isActive, long id, long requestId, string entity)
        {
            if (!isActive)
                return $"{entity} deleted successfully.";
            else if (requestId == 0)
                return $"{entity} added successfully.";
            else
                return $"{entity} updated successfully.";
        }

        // ---------- Helper: Unified API response ----------
        private IActionResult ResponseMessage(bool success, string message, object? data = null)
        {
            return Ok(new
            {
                Success = success,
                Message = message,
                Data = data
            });
        }


        [HttpPost("salesentry")]
        public async Task<IActionResult> AddOrUpdateSalesEntryWithStock(
       [FromBody] List<SalesEntryMaster> entries)
        {
            if (entries == null || entries.Count == 0)
                return ResponseMessage(false, "No sales entries provided.");

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .ToDictionary(
                        e => e.Key,
                        e => e.Value.Errors.Select(err => err.ErrorMessage).ToList()
                    );

                return ResponseMessage(false, "Validation Failed", errors);
            }

            try
            {
                var result = await _salesRepo.AddOrUpdateSalesEntryWithStockAsync(entries);

                return ResponseMessage(
                    true,
                    "Sales entry saved successfully.",
                    new
                    {
                        lastInvoiceID = result.InvoiceID,
                        lastInvoiceNumber = result.InvoiceNumber
                    }
                );

            }
            catch (Exception ex)
            {
                return ResponseMessage(false, ex.Message);
            }
        }

        [HttpGet("SalesProducts")]
        public async Task<IActionResult> GetProductStockAndPrice(
      int? companyId = null,
      int? branchId = null,
      int? businessTypeId = null)
        {
            try
            {
                var result = await _salesRepo.GetProductStockAndPriceAsync(
                    companyId,
                    branchId,
                    businessTypeId   
                );

                return ResponseMessage(
                    true,
                    "Product stock & price fetched successfully.",
                    result
                );
            }
            catch (Exception ex)
            {
                return ResponseMessage(false, ex.Message);
            }
        }


        [HttpPost("AddUpdateDeleteBusinessType")]
        public async Task<IActionResult> Save([FromBody] BusinessType model)
        {
            if (!ModelState.IsValid)
                return ResponseMessage(false, "Invalid model");

            try
            {
                var id = await _salesRepo.SaveBusinessTypeAsync(model);

                string msg = model.BusinessTypeID == 0
                    ? "Business Type added successfully."
                    : "Business Type updated successfully.";

                return ResponseMessage(true, msg, new { BusinessTypeID = id });
            }
            catch (Exception ex)
            {
                return ResponseMessage(false, ex.Message);
            }
        }


        [HttpGet("GetAllBusinessTypes")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _salesRepo.GetAllAsync();
                return ResponseMessage(true, "Business Types fetched successfully", list);
            }
            catch (Exception ex)
            {
                return ResponseMessage(false, ex.Message);
            }
        }


        [HttpPost("AddUpdateDelteGstTransType")]
        public async Task<IActionResult> Save([FromBody] GstTransactionType model, string action)
        {
            if (!ModelState.IsValid)
                return ResponseMessage(false, "Invalid model");

            try
            {
                var id = await _salesRepo.SaveGstAsync(model, action.ToLower());

                string msg = action.ToLower() switch
                {
                    "insert" => "GST Transaction Type added successfully.",
                    "update" => "GST Transaction Type updated successfully.",
                    "delete" => "GST Transaction Type deleted successfully.",
                    _ => "Invalid action"
                };

                return ResponseMessage(true, msg, new { GstTransactionTypeID = id });
            }
            catch (Exception ex)
            {
                return ResponseMessage(false, ex.Message);
            }
        }


        [HttpGet("GetAllGstType")]
        public async Task<IActionResult> GetAllGstTypes()
        {
            try
            {
                var list = await _salesRepo.GetAllGstAsync();
                return ResponseMessage(true, "GST Transaction Types fetched successfully", list);
            }
            catch (Exception ex)
            {
                return ResponseMessage(false, ex.Message);
            }
        }

        [HttpGet("GetNextInvoiceNumber")]
        public async Task<IActionResult> GetNextInvoiceNumber(int companyId, string? branchId = null)
        {
            var nextInvoice = await _salesRepo.GetNextInvoiceNumberAsync(companyId, branchId);
            return Ok(nextInvoice);
        }
        [HttpGet("GetSalesEntries")]
        public async Task<IActionResult> GetSalesEntries(
            int? companyId = null,
            int? branchId = null,
            string invoiceNumber = null)
        {
            var result = await _salesRepo.GetSalesEntriesAsync(companyId, branchId, invoiceNumber);
            return Ok(result);
        }

    }
}
