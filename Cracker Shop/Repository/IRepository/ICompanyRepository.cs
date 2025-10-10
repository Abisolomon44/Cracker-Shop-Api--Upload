using Cracker_Shop.Models.CommonMasterModels;
using System.Reflection;

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

    Task<long> SavePermissionAsync(UserRolePermission permission);

    Task DeletePermissionAsync(long permissionId);

    Task<IEnumerable<UserRolePermission>> GetPermissionsByRoleAsync(int roleId);

    Task<IEnumerable<ModuleDto>> GetModulesByUserAsync(int userId);
    Task<IEnumerable<ModuleDto>> GetAllModulesAsync();
    Task<ModuleDto?> GetModuleByIdAsync(long moduleId);
    Task<long> SaveModuleAsync(ModuleDto module);




        }
