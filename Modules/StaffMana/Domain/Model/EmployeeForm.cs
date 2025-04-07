namespace BTL_SA.Modules.StaffMana.Domain.Model;

public class EmployeeForm : Person
{
    public int Id { get; set; }
    public string Department { get; set; }
    public string Role { get; set; }

    [System.Text.Json.Serialization.JsonConstructor]
    public EmployeeForm(int id, string name, bool gender, string phoneNumber, string address, DateTime dateOfBirth, string email, string department, string role)
    {
        Id = id;
        Name = name;
        Gender = gender;
        PhoneNumber = phoneNumber;
        Address = address;
        DateOfBirth = dateOfBirth;
        Email = email;
        Department = department;
        Role = role;
    }
    public EmployeeForm(Employee employee)
    {
        Name = employee.Name;
        Gender = employee.Gender;
        PhoneNumber = employee.PhoneNumber;
        Address = employee.Address;
        DateOfBirth = employee.DateOfBirth;
        Email = employee.Email;
        Id = employee.Id;
        Department = employee.Department ?? string.Empty;
        Role = employee.Role ?? string.Empty;

    }
}