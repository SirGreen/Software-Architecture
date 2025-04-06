using BTL_SA.Modules.StaffMana.Domain.Model;
using BTL_SA.Modules.StaffMana.Infrastructure;

namespace BTL_SA.Modules.StaffMana.Application;
public class AssignmentService(IStaffRepository staffRepository) : IAssignmentService
{
    private readonly IStaffRepository _staffRepository = staffRepository;

    public int AssignRole(Employee employee, string role)
    {
        if (employee == null || string.IsNullOrEmpty(role))
        {
            return 0;
        }

        employee.Role = role;
        var employeeForm = new EmployeeForm
        {
            Id = employee.Id,
            Name = employee.Name,
            Role = employee.Role ?? "Unknown",
            Department = employee.Department ?? "Unknown"
        };
        var result = _staffRepository.Edit(employeeForm);
        return result;
    }

    public int AssignDepartment(Employee employee, string department)
    {
        if (employee == null || string.IsNullOrEmpty(department))
        {
            return 0;
        }

        employee.Department = department;
        var employeeForm = new EmployeeForm
        {
            Id = employee.Id,
            Name = employee.Name,
            Role = employee.Role ?? "Unknown",
            Department = employee.Department ?? "Unknown"
        };
        var result = _staffRepository.Edit(employeeForm);
        return result;
    }
}
