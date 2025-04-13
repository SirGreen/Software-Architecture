using BTL_SA.Modules.StaffMana.Domain.Model;

namespace BTL_SA.Modules.StaffMana.Application;

public interface IAssignmentService
{
    int AssignRole(Employee employee, string role);
    int AssignDepartment(Employee employee, string department);
}