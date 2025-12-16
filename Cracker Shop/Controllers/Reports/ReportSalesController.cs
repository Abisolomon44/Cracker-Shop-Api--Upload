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
    }
}
