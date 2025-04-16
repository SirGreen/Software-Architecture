using Microsoft.AspNetCore.Mvc;
using BTL_SA.Modules.StaffMana.Domain.Model;
using BTL_SA.Modules.StaffMana.Facade;
using Swashbuckle.AspNetCore.Annotations;

namespace BTL_SA.Modules.StaffMana.Endpoints
{
    [ApiController]
    [Route("api/employees")]
    public class StaffController(FacadeStaffInformationManagement staffInfoMana) : ControllerBase
    {
        private readonly FacadeStaffInformationManagement _staffInfoMana = staffInfoMana;

        [HttpGet]
        [SwaggerOperation(Summary = "Get all employees information")]
        [ProducesResponseType(typeof(List<EmployeeViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Get employee information by ID")]
        [ProducesResponseType(typeof(EmployeeViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEmployeeById([FromRoute] int id)
        {
            var employee = _staffInfoMana.FindById(id);
            if (employee != null)
            {
                return Ok(new EmployeeViewModel(employee));
            }
            return NotFound(new { message = "Employee not found." });
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new employee")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateEmployee([FromBody] EmployeeForm employee)
        {
            var result = _staffInfoMana.Create(employee);
            if (result != 0)
            {
                return CreatedAtAction(nameof(GetEmployeeById), new { id = result }, new { message = "Employee created successfully.", employeeId = result });
            }
            return BadRequest(new { message = "Failed to create employee." });
        }

        [HttpPut("{id:int}")]
        [SwaggerOperation(Summary = "Update an existing employee's information")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateEmployee([FromRoute] int id, [FromBody] EmployeeForm employee)
        {
            if (id != employee.Id)
            {
                return BadRequest(new { message = "ID in route must match ID in body." });
            }
            
            var result = _staffInfoMana.Edit(employee);
            if (result != 0)
            {
                return Ok(new { message = "Employee updated successfully.", employeeId = employee.Id });
            }
            return BadRequest(new { message = "Failed to update employee." });
        }

        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Delete an employee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteEmployee([FromRoute] int id)
        {
            var result = _staffInfoMana.Delete(id);
            if (result == 1)
            {
                return NoContent();
            }
            return BadRequest(new { message = "Failed to delete employee." });
        }

        [HttpPost("{employeeId:int}/credentials")]
        [SwaggerOperation(Summary = "Upload a credential for an employee")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UploadCredential([FromRoute] int employeeId, [FromBody] CredentialForm credential)
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
                    return CreatedAtAction(nameof(GetEmployeeById), new { id = employeeId }, new { message = "Credential uploaded successfully.", credentialId = result });
                }
                return BadRequest(new { message = "Failed to upload credential." });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error uploading credential: " + ex.Message);
                return StatusCode(500, new { message = "Internal server error." });
            }
        }

        [HttpPut("credentials")]
        [SwaggerOperation(Summary = "Renew a credential")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult RenewCredential([FromBody] RenewCredForm credential)
        {
            try
            {
                if (credential.CredId == 0)
                {
                    return BadRequest(new { message = "Credential ID must be provided." });
                }
                
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

        [HttpDelete("credentials/{credId:int}")]
        [SwaggerOperation(Summary = "Delete a credential")]
        public IActionResult DeleteCredential([FromRoute] int credId)
        {
            var result = _staffInfoMana.DeleteCredential(credId);
            if (result == 1)
            {
                return NoContent();
            }
            return BadRequest(new { message = "Failed to delete credential." });
        }

        [HttpPut("{employeeId:int}/role")]
        [SwaggerOperation(Summary = "Assign a role to an employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult AssignRole([FromRoute] int employeeId, [FromBody] string role)
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

        [HttpPut("{employeeId:int}/department")]
        [SwaggerOperation(Summary = "Assign a department to an employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult AssignDepartment([FromRoute] int employeeId, [FromBody] string department)
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