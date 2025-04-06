using BTL_SA.Modules.StaffMana.Domain.Model;

namespace BTL_SA.Modules.StaffMana.Infrastructure
{
    public interface IStaffRepository
    {
        int Create(EmployeeForm employee);
        int Edit(EmployeeForm employee);
        int Delete(int id);
        Employee? FindById(int id);
        List<Employee> GetAll();
    }
}