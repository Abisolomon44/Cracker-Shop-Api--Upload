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

        Task<IEnumerable<ProfitReportModel>> GetProfitReportAsync(
         int companyId,
         DateTime? fromDate,
         DateTime? toDate,
         int? branchId,
         string reportType   // ITEM | INVOICE | DAY
     );

        // ===== CUSTOMER OUTSTANDING =====
        Task<IEnumerable<CustomerOutstandingReportModel>> GetCustomerOutstandingAsync(
            int companyId,
            DateTime? fromDate,
            DateTime? toDate,
            int? branchId,
            string reportType        // CUSTOMER | AREA | DATE
        );

        // ===== SUPPLIER OUTSTANDING =====
        Task<IEnumerable<SupplierOutstandingReportModel>> GetSupplierOutstandingAsync(
            int companyId,
            DateTime? fromDate,
            DateTime? toDate,
            int? branchId,
            string reportType        // SUPPLIER | AREA | DATE
        );
        Task<IEnumerable<StockReportDto>> GetStockReportAsync(
           int companyId,
           DateTime? fromDate,
           DateTime? toDate,
           int? branchId,
           string reportType,
           int days = 30
       );
    }
   

    }
