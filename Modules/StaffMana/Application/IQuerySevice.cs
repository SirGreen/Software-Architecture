using BTL_SA.Modules.StaffMana.Domain.Model;

namespace BTL_SA.Modules.StaffMana.Application;

public interface IQuerySevice
{
    Employee? FindById(int id);
    List<Employee> GetAll();
}