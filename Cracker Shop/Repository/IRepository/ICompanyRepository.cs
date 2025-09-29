using Cracker_Shop.Models.CommonMasterModels;

public interface ICompanyRepository
{
    Task<long> SaveCompanyAsync(CompanyMaster company);
    Task<IEnumerable<CompanyMaster>> GetAllCompaniesAsync(); // Get all
    Task<CompanyMaster?> GetCompanyByIdAsync(long companyId); // Get single company     

    Task<long> SaveBranchAsync(BranchMaster branch); // Add or update
    Task<BranchMaster?> GetBranchByIdAsync(long branchId);
    Task<IEnumerable<BranchMaster>> GetBranchesByCompanyAsync(long companyId);
    Task<IEnumerable<BranchMaster>> GetAllBranchesAsync();

    Task<long> SaveDepartmentAsync(DepartmentMaster department);
    Task<IEnumerable<DepartmentMaster>> GetAllDepartmentsAsync();

    Task<long> SaveRoleAsync(RoleMaster role);
    Task<IEnumerable<RoleMaster>> GetAllRolesAsync();

    Task<long> SaveUserAsync(UserMaster user);
    Task<IEnumerable<UserMaster>> GetAllUsersAsync();
}
