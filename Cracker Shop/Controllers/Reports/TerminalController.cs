using Microsoft.AspNetCore.Mvc;
using Cracker_Shop.Repository.IRepository;

namespace Cracker_Shop.Controllers.Reports
{
    [ApiController]
    [Route("api/reports/terminal")]
    public class TerminalController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;

        public TerminalController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet("terminal-report")]
        public async Task<IActionResult> GetTerminalReport(
            DateTime fromDate,     
            DateTime toDate,      
            int companyId,
            int? branchId,
            string? createdBy
        )
        {
            if (companyId <= 0)
                return BadRequest("CompanyId is mandatory");

            if (fromDate == default || toDate == default)
                return BadRequest("FromDate and ToDate are mandatory");

            var report = await _reportRepository.GetTerminalReportAsync(
                fromDate,
                toDate,
                companyId,
                branchId,
                createdBy
            );

            return Ok(report);
        }
    }
}
