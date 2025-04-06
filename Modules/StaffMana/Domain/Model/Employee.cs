namespace BTL_SA.Modules.StaffMana.Domain.Model;

public class Employee : Person {
    private int id;
    private string? departmemt;
    private string? role;
    private List<CredentialBase>? credentials;
    
    public int Id {
        get => id;
        set => id = value;
    }

    public string? Department {
        get => departmemt;
        set => departmemt = value;
    }

    public string? Role {
        get => role;
        set => role = value;
    }

    public List<CredentialBase>? Credentials {
        get => credentials;
        set => credentials = value;
    }
}

public class EmployeeViewModel(Employee employee) : Person {
    public int Id { get; set; } = employee.Id;
    public string? Department { get; set; } = employee.Department;
    public string? Role { get; set; } = employee.Role;
    public List<License>? License { get; set; } = employee.Credentials?.OfType<License>().ToList();
    public List<Certificate>? Certificates { get; set; } = employee.Credentials?.OfType<Certificate>().ToList();
    public new string Gender { get; set; } = employee.Gender? "Male" : "Female";
}