using BTL_SA.Modules.StaffMana.Application;
using BTL_SA.Modules.StaffMana.Domain.Model;

namespace BTL_SA.Modules.StaffMana.Facade;

public class FacadeStaffInformationManagement(
    IEmployeeService employeeService,
    ICredentialService credentialService,
    IQuerySevice querySevice,
    IAssignmentService assignmentService)
{
    private readonly IAssignmentService _assignmentService = assignmentService;
    private readonly IEmployeeService _employeeService = employeeService;
    private readonly ICredentialService _credentialService = credentialService;
    private readonly IQuerySevice _querySevice = querySevice;

    public int Create(EmployeeForm employee)
    {
        return _employeeService.Create(employee);
    }

    public int Edit(EmployeeForm employee)
    {
        return _employeeService.Edit(employee);
    }

    public int Delete(int id)
    {
        return _employeeService.Delete(id);
    }

    public Employee? FindById(int id)
    {
        return _querySevice.FindById(id);
    }

    public List<Employee> GetAll()
    {
        return _querySevice.GetAll();
    }

    public int UploadCredential(Employee employee, CredentialBase credential)
    {
        return _credentialService.UploadCredential(employee, credential);
    }

    public int RenewCredential(CredentialBase credential)
    {
        return _credentialService.RenewCredential(credential);
    }

    public int AssignRole(Employee employee, string role)
    {
        return _assignmentService.AssignRole(employee, role);
    }

    public int AssignDepartment(Employee employee, string department)
    {
        return _assignmentService.AssignDepartment(employee, department);
    }
}