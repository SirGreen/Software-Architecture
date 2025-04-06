using namespace BTL_SA.Modules.StaffMana.Domain.Model;

namespace BTL_SA.Modules.StaffMana.Infrastructure
{
    public interface IStaffRepository
    {
        int Create(Employee employee);
        int Edit(Employee employee);
        int Delete(Employee employee);
        Employee FindById(int id);
    }
}