using Cracker_Shop.Code_Generator;
using Cracker_Shop.Models.CommonMasterModels;
using Dapper;
using System.Data;

namespace Cracker_Shop.Repository
{


    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDbConnection _db;

        public CompanyRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<long> SaveCompanyAsync(CompanyMaster company)
        {
            if (string.IsNullOrWhiteSpace(company.CompanyName))
                throw new ArgumentException("CompanyName is required.");

            if (company.CompanyID == 0)
            {
                // Generate code
                company.CompanyCode = await CodeGenerator.GenerateNextCodeAsync(
                    _db, "CompanyMaster", "CompanyCode", "COM", 5
                );

                var sqlInsert = @"INSERT INTO CompanyMaster
            (CompanyCode, CompanyName, Phone, AlternatePhone, Email, Website,
             AddressLine1, AddressLine2, AddressLine3, AddressLine4, City, State, Country, Pincode,
             GSTNumber, PANNumber, CINNumber, BankName, BankAccountNumber, IFSCCode,
             CompanyLogo, CompanyImage, IsActive, CreatedByUserID, CreatedSystemName)
            VALUES
             (@CompanyCode, @CompanyName, @Phone, @AlternatePhone, @Email, @Website,
              @AddressLine1, @AddressLine2, @AddressLine3, @AddressLine4, @City, @State, @Country, @Pincode,
              @GSTNumber, @PANNumber, @CINNumber, @BankName, @BankAccountNumber, @IFSCCode,
              @CompanyLogo, @CompanyImage, @IsActive, @CreatedByUserID, @CreatedSystemName);
            SELECT CAST(SCOPE_IDENTITY() as bigint);";

                return await _db.ExecuteScalarAsync<long>(sqlInsert, company);
            }
            else if (!company.IsActive)
            {
                var sqlDelete = "UPDATE CompanyMaster SET IsActive=0, UpdatedAt=SYSDATETIME() WHERE CompanyID=@CompanyID";
                await _db.ExecuteAsync(sqlDelete, new { company.CompanyID });
                return company.CompanyID;
            }
            else
            {
                var sqlUpdate = @"UPDATE CompanyMaster SET
                            CompanyName=@CompanyName,
                            Phone=@Phone,
                            AlternatePhone=@AlternatePhone,
                            Email=@Email,
                            Website=@Website,
                            AddressLine1=@AddressLine1,
                            AddressLine2=@AddressLine2,
                            AddressLine3=@AddressLine3,
                            AddressLine4=@AddressLine4,
                            City=@City,
                            State=@State,
                            Country=@Country,
                            Pincode=@Pincode,
                            GSTNumber=@GSTNumber,
                            PANNumber=@PANNumber,
                            CINNumber=@CINNumber,
                            BankName=@BankName,
                            BankAccountNumber=@BankAccountNumber,
                            IFSCCode=@IFSCCode,
                            CompanyLogo=@CompanyLogo,
                            CompanyImage=@CompanyImage,
                            UpdatedByUserID=@UpdatedByUserID,
                            UpdatedSystemName=@UpdatedSystemName,
                            UpdatedAt=SYSDATETIME()
                         WHERE CompanyID=@CompanyID";

                await _db.ExecuteAsync(sqlUpdate, company);
                return company.CompanyID;
            }
        }

        public async Task<IEnumerable<CompanyMaster>> GetAllCompaniesAsync()
        {
            var sql = "SELECT * FROM CompanyMaster WHERE IsActive=1 ORDER BY CompanyName";
            return await _db.QueryAsync<CompanyMaster>(sql);
        }

        public async Task<CompanyMaster?> GetCompanyByIdAsync(long companyId)
        {
            var sql = "SELECT * FROM CompanyMaster WHERE CompanyID=@CompanyID AND IsActive=1";
            return await _db.QueryFirstOrDefaultAsync<CompanyMaster>(sql, new { CompanyID = companyId });
        }
        public async Task<long> SaveBranchAsync(BranchMaster branch)
        {
            if (string.IsNullOrWhiteSpace(branch.BranchName))
                throw new ArgumentException("BranchName is required.");

            if (branch.BranchID == 0)
            {
                // Generate BranchCode if needed (similar to Company)
                branch.BranchCode = await CodeGenerator.GenerateNextCodeAsync(
                    _db, "BranchMaster", "BranchCode", "BRN", 5
                );

                var sqlInsert = @"
INSERT INTO BranchMaster
(CompanyID, BranchCode, BranchName, Address, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt)
VALUES
(@CompanyID, @BranchCode, @BranchName, @Address, @IsActive, @CreatedByUserID, @CreatedSystemName, SYSDATETIME());
SELECT CAST(SCOPE_IDENTITY() AS bigint);
";
                return await _db.ExecuteScalarAsync<long>(sqlInsert, branch);
            }
            else if (!branch.IsActive)
            {
                // Soft delete
                var sqlDelete = @"
UPDATE BranchMaster
SET IsActive=0, UpdatedAt=SYSDATETIME()
WHERE BranchID=@BranchID
";
                await _db.ExecuteAsync(sqlDelete, new { branch.BranchID });
                return branch.BranchID;
            }
            else
            {
                // Update branch
                var sqlUpdate = @"
UPDATE BranchMaster
SET 
    BranchName=@BranchName,
    Address=@Address,
    UpdatedByUserID=@UpdatedByUserID,
    UpdatedSystemName=@UpdatedSystemName,
    UpdatedAt=SYSDATETIME()
WHERE BranchID=@BranchID
";
                await _db.ExecuteAsync(sqlUpdate, branch);
                return branch.BranchID;
            }
        }
        public async Task<BranchMaster?> GetBranchByIdAsync(long branchId)
        {
            var sql = "SELECT * FROM BranchMaster WHERE BranchID=@BranchID";
            return await _db.QueryFirstOrDefaultAsync<BranchMaster>(sql, new { BranchID = branchId });
        }
        public async Task<IEnumerable<BranchMaster>> GetBranchesByCompanyAsync(long companyId)
        {
            var sql = "SELECT * FROM BranchMaster WHERE CompanyID=@CompanyID AND IsActive=1";
            return await _db.QueryAsync<BranchMaster>(sql, new { CompanyID = companyId });
        }

        public async Task<IEnumerable<BranchMaster>> GetAllBranchesAsync()
        {
            var sql = "SELECT * FROM BranchMaster WHERE IsActive=1";
            return await _db.QueryAsync<BranchMaster>(sql);
        }
        public async Task<long> SaveDepartmentAsync(DepartmentMaster department)
        {
            if (string.IsNullOrWhiteSpace(department.DepartmentName))
                throw new ArgumentException("DepartmentName is required.");

            if (department.DepartmentID == 0)
            {
                // Generate DepartmentCode
                department.DepartmentCode = await CodeGenerator.GenerateNextCodeAsync(
                    _db, "DepartmentMaster", "DepartmentCode", "DEP", 5
                );

                var sqlInsert = @"
INSERT INTO DepartmentMaster
(BranchID, DepartmentCode, DepartmentName, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt)
VALUES
(@BranchID, @DepartmentCode, @DepartmentName, @IsActive, @CreatedByUserID, @CreatedSystemName, SYSDATETIME());
SELECT CAST(SCOPE_IDENTITY() AS bigint);
";
                return await _db.ExecuteScalarAsync<long>(sqlInsert, department);
            }
            else if (!department.IsActive)
            {
                // Soft delete
                var sqlDelete = "UPDATE DepartmentMaster SET IsActive=0, UpdatedAt=SYSDATETIME() WHERE DepartmentID=@DepartmentID";
                await _db.ExecuteAsync(sqlDelete, new { department.DepartmentID });
                return department.DepartmentID;
            }
            else
            {
                // Update
                var sqlUpdate = @"
UPDATE DepartmentMaster
SET 
    DepartmentName=@DepartmentName,
    UpdatedByUserID=@UpdatedByUserID,
    UpdatedSystemName=@UpdatedSystemName,
    UpdatedAt=SYSDATETIME()
WHERE DepartmentID=@DepartmentID
";
                await _db.ExecuteAsync(sqlUpdate, department);
                return department.DepartmentID;
            }
        }
        public async Task<IEnumerable<DepartmentMaster>> GetAllDepartmentsAsync()
        {
            var sql = "SELECT * FROM DepartmentMaster WHERE IsActive=1";
            return await _db.QueryAsync<DepartmentMaster>(sql);
        }


            public async Task<long> SaveRoleAsync(RoleMaster role)
            {
                if (string.IsNullOrWhiteSpace(role.RoleName))
                    throw new ArgumentException("RoleName is required.");

                if (role.RoleID == 0)
                {
                    // Generate RoleCode
                    role.RoleCode = await CodeGenerator.GenerateNextCodeAsync(_db, "RoleMaster", "RoleCode", "ROL", 5);

                    var sqlInsert = @"
INSERT INTO RoleMaster
(RoleCode, RoleName, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt)
VALUES
(@RoleCode, @RoleName, @IsActive, @CreatedByUserID, @CreatedSystemName, SYSDATETIME());
SELECT CAST(SCOPE_IDENTITY() AS bigint);";
                    return await _db.ExecuteScalarAsync<long>(sqlInsert, role);
                }
                else if (!role.IsActive)
                {
                    var sqlDelete = "UPDATE RoleMaster SET IsActive=0, UpdatedAt=SYSDATETIME() WHERE RoleID=@RoleID";
                    await _db.ExecuteAsync(sqlDelete, new { role.RoleID });
                    return role.RoleID;
                }
                else
                {
                    var sqlUpdate = @"
UPDATE RoleMaster
SET RoleName=@RoleName,
    UpdatedByUserID=@UpdatedByUserID,
    UpdatedSystemName=@UpdatedSystemName,
    UpdatedAt=SYSDATETIME()
WHERE RoleID=@RoleID";
                    await _db.ExecuteAsync(sqlUpdate, role);
                    return role.RoleID;
                }
            }

            public async Task<IEnumerable<RoleMaster>> GetAllRolesAsync()
            {
                var sql = "SELECT * FROM RoleMaster WHERE IsActive=1";
                return await _db.QueryAsync<RoleMaster>(sql);
            }
        

            public async Task<long> SaveUserAsync(UserMaster user)
            {
                if (string.IsNullOrWhiteSpace(user.UserName))
                    throw new ArgumentException("UserName is required.");

                if (user.UserID == 0)
                {
                    var sqlInsert = @"
INSERT INTO UserMaster
(CompanyID, BranchID, DepartmentID, RoleID, UserName, Email, PasswordHash, IsActive, CreatedByUserID, CreatedSystemName, CreatedAt)
VALUES
(@CompanyID, @BranchID, @DepartmentID, @RoleID, @UserName, @Email, @PasswordHash, @IsActive, @CreatedByUserID, @CreatedSystemName, SYSDATETIME());
SELECT CAST(SCOPE_IDENTITY() AS bigint);";
                    return await _db.ExecuteScalarAsync<long>(sqlInsert, user);
                }
                else if (!user.IsActive)
                {
                    var sqlDelete = "UPDATE UserMaster SET IsActive=0, UpdatedAt=SYSDATETIME() WHERE UserID=@UserID";
                    await _db.ExecuteAsync(sqlDelete, new { user.UserID });
                    return user.UserID;
                }
                else
                {
                    var sqlUpdate = @"
UPDATE UserMaster
SET CompanyID=@CompanyID,
    BranchID=@BranchID,
    DepartmentID=@DepartmentID,
    RoleID=@RoleID,
    UserName=@UserName,
    Email=@Email,
    PasswordHash=@PasswordHash,
    UpdatedByUserID=@UpdatedByUserID,
    UpdatedSystemName=@UpdatedSystemName,
    UpdatedAt=SYSDATETIME()
WHERE UserID=@UserID";
                    await _db.ExecuteAsync(sqlUpdate, user);
                    return user.UserID;
                }
            }

            public async Task<IEnumerable<UserMaster>> GetAllUsersAsync()
            {
                var sql = "SELECT * FROM UserMaster WHERE IsActive=1";
                return await _db.QueryAsync<UserMaster>(sql);
            }
        
    


}
}