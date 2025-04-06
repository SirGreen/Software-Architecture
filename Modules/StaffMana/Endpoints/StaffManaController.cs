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
                return Ok(employee);
            }
            return NotFound(new { message = "Employee not found." });
        }

        [HttpGet("getall")]
        public IActionResult GetAllEmployees()
        {
            var employees = _staffInfoMana.GetAll();
            if (employees != null && employees.Count > 0)
            {
                return Ok(employees);
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
        public IActionResult UploadCredential([FromBody] CredentialBase credential, [FromQuery] int employeeId)
        {
            var employee = _staffInfoMana.FindById(employeeId);
            if (employee == null)
            {
                return NotFound(new { message = "Employee not found." });
            }

            var result = _staffInfoMana.UploadCredential(employee, credential);
            if (result != 0)
            {
                return Ok(new { message = "Credential uploaded successfully.", credentialId = result });
            }
            return BadRequest(new { message = "Failed to upload credential." });
        }

        [HttpPost("renewcredential")]
        public IActionResult RenewCredential([FromBody] CredentialBase credential)
        {
            var result = _staffInfoMana.RenewCredential(credential);
            if (result != 0)
            {
                return Ok(new { message = "Credential renewed successfully.", credentialId = result });
            }
            return BadRequest(new { message = "Failed to renew credential." });
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