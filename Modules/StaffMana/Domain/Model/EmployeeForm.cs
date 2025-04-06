namespace BTL_SA.Modules.StaffMana.Domain.Model;

public class EmployeeForm : Person {
    public required int Id { get; set; }
    public required string Department { get; set; }
    public required string Role { get; set; }
}