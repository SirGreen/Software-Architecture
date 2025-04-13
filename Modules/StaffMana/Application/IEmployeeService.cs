using BTL_SA.Modules.StaffMana.Domain.Model;

namespace BTL_SA.Modules.StaffMana.Application;

public interface IEmployeeService
{
    int Create(EmployeeForm employee);
    int Edit(EmployeeForm employee);
    int Delete(int id);
}