using Cracker_Shop.Models.Reports;
using System;
using System.Threading.Tasks;

namespace Cracker_Shop.Repository.IRepository
{
    public interface IReportRepository
    {
        Task<TerminalReportModel> GetTerminalReportAsync(
            DateTime fromDate,
            DateTime toDate,
            int companyId,
            int? branchId,
            string? createdBy    // ✅ FIXED
        );
    }


}
