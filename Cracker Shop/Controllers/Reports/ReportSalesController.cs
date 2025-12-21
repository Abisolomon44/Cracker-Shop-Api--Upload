using Cracker_Shop.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Cracker_Shop.Controllers.Reports
{
    [ApiController]
    [Route("api/reports/sales")]
    public class ReportSalesController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;

        public ReportSalesController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        // ======================================
        // GST Filing API
        // ======================================
        [HttpGet("GstFiling")]
        public async Task<IActionResult> GetGSTFiling(
            [FromQuery] int companyId,
            [FromQuery] int? branchId,
            [FromQuery] string gstFileType,   // B2B / B2C / HSN
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate
        )
        {
            // ✅ Company mandatory
            if (companyId <= 0)
                return BadRequest("Company ID is required");

            // ✅ GST type mandatory
            if (string.IsNullOrWhiteSpace(gstFileType))
                return BadRequest("GST File Type is required");

            gstFileType = gstFileType.ToUpper();

            if (gstFileType != "B2B" && gstFileType != "B2C" && gstFileType != "HSN")
                return BadRequest("GST File Type must be B2B, B2C or HSN");

            if (fromDate.HasValue && toDate.HasValue && fromDate > toDate)
                return BadRequest("FromDate cannot be greater than ToDate");

            var result = await _reportRepository.GetGSTFilingAsync(
                companyId,
                branchId,
                gstFileType,
                fromDate,
                toDate
            );

            return Ok(result);
        }

        // ======================================
        [HttpGet("SalesReports")]
        public async Task<IActionResult> GetSalesReport(
            [FromQuery] int companyId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string reportType,    // SUMMARY | DETAILED | PAYMODE | CUSTOMER | AREA
            [FromQuery] int? branchId
        )
        {
            if (companyId <= 0)
                return BadRequest("Company ID is required");

            if (fromDate > toDate)
                return BadRequest("FromDate cannot be greater than ToDate");

            if (string.IsNullOrWhiteSpace(reportType))
                return BadRequest("ReportType is required");

            reportType = reportType.ToUpper();

            var allowedTypes = new[] { "SUMMARY", "DETAILED", "PAYMODE", "CUSTOMER", "AREA" };

            if (!allowedTypes.Contains(reportType))
                return BadRequest("Invalid ReportType");

            var result = await _reportRepository.GetSalesReportAsync(
                companyId,
                fromDate,
                toDate,
                branchId,
                reportType
            );

            return Ok(result);
        }


        // ======================================
        // PROFIT REPORT API
        // ======================================
        [HttpGet("Profit")]
        public async Task<IActionResult> GetProfitReport(
            [FromQuery] int companyId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string reportType,   // ITEM | INVOICE | DAY
            [FromQuery] int? branchId
        )
        {
            // ✅ Company mandatory
            if (companyId <= 0)
                return BadRequest("Company ID is required");

            // ✅ Date validation
            if (fromDate > toDate)
                return BadRequest("FromDate cannot be greater than ToDate");

            // ✅ ReportType validation
            if (string.IsNullOrWhiteSpace(reportType))
                return BadRequest("ReportType is required");

            reportType = reportType.ToUpper();

            var allowedTypes = new[] { "ITEM", "INVOICE", "DAY" };

            if (!allowedTypes.Contains(reportType))
                return BadRequest("ReportType must be ITEM, INVOICE or DAY");

            var result = await _reportRepository.GetProfitReportAsync(
                companyId,
                fromDate,
                toDate,
                branchId,
                reportType
            );

            return Ok(result);

        }

        [HttpGet("CustomerOutstanding")]
        public async Task<IActionResult> GetCustomerOutstanding(
            [FromQuery] int companyId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int? branchId,
            [FromQuery] string reportType   // CUSTOMER | AREA | DATE
        )
        {
            // ✅ ONLY mandatory check
            if (companyId <= 0)
                return BadRequest("Company ID is required");

            if (string.IsNullOrWhiteSpace(reportType))
                return BadRequest("ReportType is required");

            reportType = reportType.ToUpper();

            var allowedTypes = new[] { "CUSTOMER", "AREA", "DATE" };

            if (!allowedTypes.Contains(reportType))
                return BadRequest("ReportType must be CUSTOMER, AREA or DATE");

            var result = await _reportRepository.GetCustomerOutstandingAsync(
                companyId,
                fromDate,
                toDate,
                branchId,
                reportType
            );

            return Ok(result);
        }

        // ======================================================
        // SUPPLIER OUTSTANDING REPORT
        // ======================================================
        [HttpGet("SupplierOutstanding")]
        public async Task<IActionResult> GetSupplierOutstanding(
       [FromQuery] int companyId,
       [FromQuery] DateTime? fromDate,
       [FromQuery] DateTime? toDate,
       [FromQuery] int? branchId,
       [FromQuery] string reportType   // SUPPLIER | AREA | DATE
   )
        {
            // ✅ ONLY mandatory check
            if (companyId <= 0)
                return BadRequest("Company ID is required");

            if (string.IsNullOrWhiteSpace(reportType))
                return BadRequest("ReportType is required");

            reportType = reportType.ToUpper();

            var allowedTypes = new[] { "SUPPLIER", "AREA", "DATE" };

            if (!allowedTypes.Contains(reportType))
                return BadRequest("ReportType must be SUPPLIER, AREA or DATE");

            var result = await _reportRepository.GetSupplierOutstandingAsync(
                companyId,
                fromDate,
                toDate,
                branchId,
                reportType
            );

            return Ok(result);
        }


        [HttpGet("Stock")]
        public async Task<IActionResult> GetStockReport(
              [FromQuery] int companyId,
              [FromQuery] DateTime? fromDate,
              [FromQuery] DateTime? toDate,
              [FromQuery] int? branchId,
              [FromQuery] string reportType,
              [FromQuery] int days = 30
          )
        {
            if (companyId <= 0)
                return BadRequest("Company ID is required");

            if (string.IsNullOrWhiteSpace(reportType))
                return BadRequest("ReportType is required");

            reportType = reportType.ToUpper();

            var allowedTypes = new[]
            {
                "TOTAL", "LOW", "FAST", "SLOW", "LEDGER", "VALUATION"
            };

            if (!allowedTypes.Contains(reportType))
                return BadRequest("Invalid ReportType");

            if (fromDate.HasValue && toDate.HasValue && fromDate > toDate)
                return BadRequest("FromDate cannot be greater than ToDate");

            if (reportType == "LEDGER" && (!fromDate.HasValue || !toDate.HasValue))
                return BadRequest("FromDate and ToDate are required for LEDGER report");

            var result = await _reportRepository.GetStockReportAsync(
                companyId,
                fromDate,
                toDate,
                branchId,
                reportType,
                days
            );

            return Ok(result);
        }





    }


    }
