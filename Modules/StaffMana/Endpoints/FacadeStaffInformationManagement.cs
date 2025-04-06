using Microsoft.AspNetCore.Mvc;
using BTL_SA.Modules.StaffMana.Domain.Model;
using BTL_SA.Modules.StaffMana.Infrastructure;

namespace BTL_SA.Modules.StaffMana.Endpoints
{
    [ApiController]
    [Route("api/StaffMana")]
    public class FacadeStaffInformationManagement : ControllerBase
    {
        private readonly IStaffRepository _staffDbService;

        public FacadeStaffInformationManagement(IStaffRepository employeeService)
        {
            _staffDbService = employeeService;
        }

        [HttpPost("create")]
        public IActionResult CreateEmployee([FromBody] EmployeeForm employee)
        {
            var result = _staffDbService.Create(employee);
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
            var result = _staffDbService.Edit(employee);
            if (result != 0)
            {
                return Ok(new { message = "Employee edited successfully.", employeeId = employee.Id });
            }
            return BadRequest(new { message = "Failed to edit employee." });
        }

        [HttpPost("findbyid")]
        public IActionResult FindById([FromBody] int id)
        {
            var employee = _staffDbService.FindById(id);
            if (employee != null)
            {
                return Ok(employee);
            }
            return NotFound(new { message = "Employee not found." });
        }

        [HttpPost("delete")]
        public IActionResult DeleteEmployee([FromBody] int id)
        {
            var result = _staffDbService.Delete(id);
            if (result == 1)
            {
                return Ok(new { message = "Employee deleted successfully.", employeeId = id });
            }
            return BadRequest(new { message = "Failed to delete employee." });
        }
    }
}