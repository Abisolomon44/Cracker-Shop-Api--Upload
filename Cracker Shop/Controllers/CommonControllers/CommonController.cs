using Cracker_Shop.Models.CommonMasterModels;
using Microsoft.AspNetCore.Mvc;

namespace Cracker_Shop.Controllers.CommonControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonController : ControllerBase
    {

        private readonly ICompanyRepository _repo;

        public CommonController(ICompanyRepository repo)
        {
            _repo = repo;
        }
        [HttpPost("PostCompanyMaster")]
        public async Task<IActionResult> SaveCompany([FromBody] CompanyMaster company)
        {
            if (company == null)
                return BadRequest("Company data is required.");

            if (string.IsNullOrWhiteSpace(company.CompanyName))
                return BadRequest("CompanyName is required.");

            try
            {
                var id = await _repo.SaveCompanyAsync(company);

                return Ok(new
                {
                    Message = "Company saved successfully.",
                    CompanyID = id
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("GetCompanylist")]
        public async Task<IActionResult> GetAllCompany()
        {
            var companies = await _repo.GetAllCompaniesAsync();
            return Ok(companies);
        }

        [HttpGet("GetCompanylistByID")]
        public async Task<IActionResult> GeCompanyByIDt(long id)
        {
            var company = await _repo.GetCompanyByIdAsync(id);
            if (company == null) return NotFound();
            return Ok(company);
        }









        [HttpPost("PostBranchMaster")]
        public async Task<IActionResult> SaveBranch([FromBody] BranchMaster branch)
        {
            if (branch == null)
                return BadRequest("Branch data is required.");

            if (string.IsNullOrWhiteSpace(branch.BranchName))
                return BadRequest("BranchName is required.");

            try
            {
                var id = await _repo.SaveBranchAsync(branch);

                return Ok(new
                {
                    Message = "Branch saved successfully.",
                    BranchID = id
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetBranchList")]
        public async Task<IActionResult> GetAllbBranches()
        {
            var branches = await _repo.GetAllBranchesAsync(); // If implemented
            return Ok(branches);
        }

        [HttpGet("GetBranchListByID")]
        public async Task<IActionResult> Get(long id)
        {
            var branch = await _repo.GetBranchByIdAsync(id);
            if (branch == null) return NotFound();
            return Ok(branch);
        }

        [HttpGet("GetBranchesByCompany")]
        public async Task<IActionResult> GetByCompany(long companyId)
        {
            var branches = await _repo.GetBranchesByCompanyAsync(companyId);
            return Ok(branches);
        }



        [HttpPost("PostDepartmentMaster")]
        public async Task<IActionResult> SaveDepartment([FromBody] DepartmentMaster department)
        {
            if (department == null)
                return BadRequest("Department data is required.");

            if (string.IsNullOrWhiteSpace(department.DepartmentName))
                return BadRequest("DepartmentName is required.");

            try
            {
                var id = await _repo.SaveDepartmentAsync(department);

                return Ok(new
                {
                    Message = "Department saved successfully.",
                    DepartmentID = id
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("GetDepartmentList")]
        public async Task<IActionResult> GetAllDeparmants()
        {
            var departments = await _repo.GetAllDepartmentsAsync();
            return Ok(departments);
        }




        [HttpPost("PostRoleMaster")]
        public async Task<IActionResult> SaveRole([FromBody] RoleMaster role)
        {
            if (role == null)
                return BadRequest("Role data is required.");

            if (string.IsNullOrWhiteSpace(role.RoleName))
                return BadRequest("RoleName is required.");

            try
            {
                var id = await _repo.SaveRoleAsync(role);
                return Ok(new
                {
                    Message = "Role saved successfully.",
                    RoleID = id
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetRoleList")]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _repo.GetAllRolesAsync();
            return Ok(roles);
        }


        [HttpPost("PostUserMaster")]
        public async Task<IActionResult> SaveUser([FromBody] UserMaster user)
        {
            if (user == null)
                return BadRequest("User data is required.");

            if (string.IsNullOrWhiteSpace(user.UserName))
                return BadRequest("UserName is required.");

            if (string.IsNullOrWhiteSpace(user.Email))
                return BadRequest("Email is required.");

            try
            {
                var id = await _repo.SaveUserAsync(user);
                return Ok(new
                {
                    Message = "User saved successfully.",
                    UserID = id
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("GetUserList")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _repo.GetAllUsersAsync();
            return Ok(users);
        }











    }






    }
