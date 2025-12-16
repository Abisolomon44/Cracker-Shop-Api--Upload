using Cracker_Shop.Models.Reports;
using Cracker_Shop.Repository.IRepository;
using Dapper;
using System.Data;

namespace Cracker_Shop.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly IDbConnection _db;

        public ReportRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<TerminalReportModel> GetTerminalReportAsync(
       DateTime fromDate,
       DateTime toDate,
       int companyId,
       int? branchId,
       string? createdBy     // ✅ FIXED
   )
        {
            var param = new DynamicParameters();
            param.Add("@FromDate", fromDate.Date);
            param.Add("@ToDate", toDate.Date);
            param.Add("@CompanyID", companyId);
            param.Add("@BranchID", branchId);
            param.Add("@CreatedBy", createdBy);   // string → NVARCHAR

            var report = await _db.QueryFirstOrDefaultAsync<TerminalReportModel>(
                "sp_TerminalReport_Common",
                param,
                commandType: CommandType.StoredProcedure
            );

            return report;
        }

        public async Task<IEnumerable<GSTFilingModel>> GetGSTFilingAsync(
           int companyId,
           int? branchId,
           string gstFileType,
           DateTime? fromDate,
           DateTime? toDate
       )
        {
            var param = new DynamicParameters();
            param.Add("@CompanyID", companyId);
            param.Add("@BranchID", branchId);
            param.Add("@GSTFileType", gstFileType);
            param.Add("@FromDate", fromDate); // NULL safe
            param.Add("@ToDate", toDate);     // NULL safe

            return await _db.QueryAsync<GSTFilingModel>(
                "RP_GST_Filing_Format",
                param,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<SalesReportCommonModel>> GetSalesReportAsync(
         int companyId,
         DateTime? fromDate,
         DateTime? toDate,
         int? branchId,
         string reportType
     )
        {
            var param = new DynamicParameters();
            param.Add("@CompanyID", companyId);
            param.Add("@FromDate", fromDate);
            param.Add("@ToDate", toDate);
            param.Add("@BranchID", branchId);
            param.Add("@ReportType", reportType);

            return await _db.QueryAsync<SalesReportCommonModel>(
                "sp_SalesReports",
                param,
                commandType: CommandType.StoredProcedure
            );
        }


    }
}
