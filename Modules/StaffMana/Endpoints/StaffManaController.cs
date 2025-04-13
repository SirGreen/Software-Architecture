using Microsoft.AspNetCore.Mvc;
using BTL_SA.Modules.StaffMana.Domain.Model;
using BTL_SA.Modules.StaffMana.Facade;

namespace BTL_SA.Modules.StaffMana.Endpoints
{
    [ApiController]
    [Route("api/StaffMana")]
    public class StaffManaController(FacadeStaffInformationManagement staffInfoMana) : ControllerBase
    {
        private readonly FacadeStaffInformationManagement _staffInfoMana = staffInfoMana;

        [HttpPost("create")]
        public IActionResult CreateEmployee([FromBody] EmployeeForm employee)
        {
            var result = _staffInfoMana.Create(employee);
            if (result != 0)
            {
                return Ok(new { message = "Employee created successfully.", employeeId = result });
            }
            return BadRequest(new { message = "Failed to create employee." });
        }

        [HttpPost("edit")]
        public IActionResult EditEmployee([FromBody] EmployeeForm employee)
        {
            Console.WriteLine("Editing employee with ID: " + employee.Id);
            var result = _staffInfoMana.Edit(employee);
            if (result != 0)
            {
                return Ok(new { message = "Employee edited successfully.", employeeId = employee.Id });
            }
            return BadRequest(new { message = "Failed to edit employee." });
        }

        [HttpGet("findbyid/{id}")]
        public IActionResult FindById([FromRoute] int id)
        {
            var employee = _staffInfoMana.FindById(id);
            if (employee != null)
            {
                return Ok(new EmployeeViewModel(employee));
            }
            return NotFound(new { message = "Employee not found." });
        }

        [HttpGet("getall")]
        public IActionResult GetAllEmployees()
        {
            var employees = _staffInfoMana.GetAll();
            if (employees != null && employees.Count > 0)
            {
                var employeeViewModels = employees.Select(e => new EmployeeViewModel(e)).ToList();
                return Ok(employeeViewModels);
            }
            return NotFound(new { message = "No employees found." });
        }

        [HttpPost("delete")]
        public IActionResult DeleteEmployee([FromBody] int id)
        {
            var result = _staffInfoMana.Delete(id);
            if (result == 1)
            {
                return Ok(new { message = "Employee deleted successfully.", employeeId = id });
            }
            return BadRequest(new { message = "Failed to delete employee." });
        }

        [HttpPost("uploadcredential")]
        public IActionResult UploadCredential([FromBody] CredentialForm credential, [FromQuery] int employeeId)
        {
            try
            {
                var employee = _staffInfoMana.FindById(employeeId);
                if (employee == null)
                {
                    return NotFound(new { message = "Employee not found." });
                }
                CredentialBase cre = (CredentialBase)credential.CreateLicenseOrCertificate();
                var result = _staffInfoMana.UploadCredential(employee, cre);
                if (result != 0)
                {
                    return Ok(new { message = "Credential uploaded successfully.", credentialId = result });
                }
                return BadRequest(new { message = "Failed to upload credential." });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error uploading credential: " + ex.Message);
                return StatusCode(500, new { message = "Internal server error." });
            }
        }

        [HttpPost("renewcredential")]
        public IActionResult RenewCredential([FromBody] RenewCredForm credential)
        {
            try
            {
                var result = _staffInfoMana.RenewCredential(credential.CredId, credential.NewExprDate);
                if (result == 1)
                {
                    return Ok(new { message = "Credential renewed successfully.", credentialId = result });
                } else if (result == -1)
                {
                    return BadRequest(new { message = "Credential has not expired yet." });
                }
                return BadRequest(new { message = "Failed to renew credential." });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error renewing credential: " + ex.Message);
                return StatusCode(500, new { message = "Internal server error." });
            }
        }

        [HttpPost("assignrole")]
        public IActionResult AssignRole([FromBody] string role, [FromQuery] int employeeId)
        {
            var employee = _staffInfoMana.FindById(employeeId);
            if (employee == null)
            {
                return NotFound(new { message = "Employee not found." });
            }

            var result = _staffInfoMana.AssignRole(employee, role);
            if (result != 0)
            {
                return Ok(new { message = "Role assigned successfully.", employeeId = employeeId });
            }
            return BadRequest(new { message = "Failed to assign role." });
        }

        [HttpPost("assigndepartment")]
        public IActionResult AssignDepartment([FromBody] string department, [FromQuery] int employeeId)
        {
            var employee = _staffInfoMana.FindById(employeeId);
            if (employee == null)
            {
                return NotFound(new { message = "Employee not found." });
            }

            var result = _staffInfoMana.AssignDepartment(employee, department);
            if (result != 0)
            {
                return Ok(new { message = "Department assigned successfully.", employeeId = employeeId });
            }
            return BadRequest(new { message = "Failed to assign department." });
        }
    }
}