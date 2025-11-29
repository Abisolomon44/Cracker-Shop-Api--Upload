using Cracker_Shop.Models.Purchase;
using Cracker_Shop.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Cracker_Shop.Controllers.Purchase
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseRepository _purchaseRepo;

        public PurchaseController(IPurchaseRepository purchaseRepo)
        {
            _purchaseRepo = purchaseRepo;
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

        // -------------------------------------------------------
        // PURCHASE ORDER: Add / Update / Delete
        // -------------------------------------------------------
        [HttpPost("purchaseorder")]
        public async Task<IActionResult> AddOrUpdatePurchaseOrder([FromBody] PurchaseOrderEntry po)
        {
            if (po == null)
                return BadRequest("Invalid purchase order data.");

            try
            {
                var id = await _purchaseRepo.AddUpDateDeletePurchaseOrderAsync(po);
                var message = GetActionMessage(po.IsActive ?? true, id, po.POID ?? 0, "Purchase Order");
                return ResponseMessage(true, message, new { POID = id });
            }
            catch (Exception ex)
            {
                return ResponseMessage(false, ex.Message);
            }
        }

        // -------------------------------------------------------
        // GRN ENTRY: Add / Update / Delete
        // -------------------------------------------------------
        [HttpPost("grn")]
        public async Task<IActionResult> AddOrUpdateGRN([FromBody] GRNEntry grn)
        {
            if (grn == null)
                return BadRequest("Invalid GRN data.");

            try
            {
                var id = await _purchaseRepo.AddUpdateDeleteGRNAsync(grn);
                var message = GetActionMessage(grn.IsActive ?? true, id, grn.GRNEntryID ?? 0, "GRN Entry");
                return ResponseMessage(true, message, new { GRNEntryID = id });
            }
            catch (Exception ex)
            {
                return ResponseMessage(false, ex.Message);
            }
        }


        [HttpPost("purchaseentry")]
        public async Task<IActionResult> AddOrUpdatePurchaseEntryWithStock(
            [FromBody] List<PurchaseEntry> entries)
        {
            if (entries == null || entries.Count == 0)
                return ResponseMessage(false, "No purchase entries provided.");

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
                var id = await _purchaseRepo.AddOrUpdatePurchaseEntryWithStockAsync(entries);

                return ResponseMessage(
                    true,
                    $"Purchase entry saved successfully. Last PurchaseID: {id}",
                    new { LastPurchaseID = id }
                );
            }
            catch (Exception ex)
            {
                return ResponseMessage(false, ex.Message);
            }
        }


        [HttpGet("GetPurchaseOrder")]
        public async Task<IActionResult> GetPurchaseOrders(
            int? poid = null,
            int? companyId = null,
            int? branchId = null,
            int? supplierId = null,
            DateTime? poDate = null,
            string? poNumber = null   
        )
        {
            var result = await _purchaseRepo.GetPurchaseOrdersAsync(
                poid, companyId, branchId, supplierId, poDate, poNumber  
            );

            return Ok(result);
        }



        [HttpGet("GetPurchaseStock")]
        public async Task<IActionResult> GetPurchaseEntry(
          int? companyId = null,
          int? branchId = null,
          int? supplierId = null,
          DateTime? fromDate = null,
          DateTime? toDate = null,
          string? poNumber = null
      )
        {
            var result = await _purchaseRepo.GetPurchaseStockAsync(
                companyId,
                branchId,
                supplierId,
                fromDate,
                toDate,
                poNumber
            );

            return Ok(result);
        }
        [HttpGet("GetNextPONumber")]
        public async Task<IActionResult> GetNextPONumber(int companyId, string? branchId = null)
        {
            var nextPo = await _purchaseRepo.GetNextPONumberAsync(companyId, branchId);
            return Ok(nextPo);
        }


    }
}
