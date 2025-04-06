using BTL_SA.Modules.StaffMana.Domain.Model;
using BTL_SA.Modules.StaffMana.Infrastructure;

namespace BTL_SA.Modules.StaffMana.Application;
public class QueryService(IStaffRepository staffRepository) : IQuerySevice
{
    private readonly IStaffRepository _staffRepository = staffRepository;

    public Employee? FindById(int id)
    {
        if (id <= 0)
        {
            return null;
        }
        var employee = _staffRepository.FindById(id);
        return employee;
    }

    public List<Employee> GetAll()
    {
        var employees = _staffRepository.GetAll();
        return employees;
    }
}