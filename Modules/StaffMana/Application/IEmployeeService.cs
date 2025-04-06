using BTL_SA.Modules.StaffMana.Domain.Model;

namespace BTL_SA.Modules.StaffMana.Application;

public interface IEmployeeService
{
    int Create(Employee employee);
    int Edit(Employee employee);
    int Delete(Employee employee);
}