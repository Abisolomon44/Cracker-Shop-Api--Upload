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
            string? createdBy   
        );


        Task<IEnumerable<GSTFilingModel>> GetGSTFilingAsync(
            int companyId,
            int? branchId,
            string gstFileType, 
            DateTime? fromDate,
            DateTime? toDate
        );

        Task<IEnumerable<SalesReportCommonModel>> GetSalesReportAsync(
            int companyId,
            DateTime? fromDate,
            DateTime? toDate,
            int? branchId,
            string reportType
        );




    }


    }
