using BTL_SA.Modules.StaffMana.Domain.Model;
using BTL_SA.Modules.StaffMana.Infrastructure;

namespace BTL_SA.Modules.StaffMana.Application;
public class EmployeeService(IStaffRepository staffRepository) : IEmployeeService
{
    private readonly IStaffRepository _staffRepository = staffRepository;

    public int Create(EmployeeForm employee)
    {
        if (employee == null)
        {
            return 0;
        }
        var id = _staffRepository.Create(employee);
        return id;
    }

    public int Edit(EmployeeForm employee)
    {
        if (employee == null)
        {
            return 0;
        }
        var id = _staffRepository.Edit(employee);
        return id;
    }

    public int Delete(int id)
    {
        return _staffRepository.Delete(id);
    }
}
