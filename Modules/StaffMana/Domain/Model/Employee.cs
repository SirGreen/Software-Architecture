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

public class EmployeeViewModel : Person {
    public int Id { get; set; }
    public string? Department { get; set; }
    public string? Role { get; set; }
    public List<License>? License { get; set; }
    public List<Certificate>? Certificates { get; set; }
    public new string Gender { get; set; }

    public EmployeeViewModel(Employee employee) {
        Id = employee.Id;
        Department = employee.Department;
        Role = employee.Role;
        License = employee.Credentials?.OfType<License>().ToList();
        Certificates = employee.Credentials?.OfType<Certificate>().ToList();
        Gender = employee.Gender ? "Male" : "Female";
        Name = employee.Name;
        DateOfBirth = employee.DateOfBirth;
        Address = employee.Address;
        Email = employee.Email;
        PhoneNumber = employee.PhoneNumber;
    }
}