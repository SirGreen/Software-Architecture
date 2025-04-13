namespace BTL_SA.Modules.StaffMana.Domain.Model;

public class Person {
    private string? name;
    private bool gender;
    private string? phoneNumber;
    private string? address;
    private DateTime dateOfBirth;
    private string? email;

    public string? Name {
        get => name;
        set => name = value;
    }

    public bool Gender {
        get => gender;
        set => gender = value;
    }

    public string? PhoneNumber {
        get => phoneNumber;
        set => phoneNumber = value;
    }

    public string? Address {
        get => address;
        set => address = value;
    }

    public DateTime DateOfBirth {
        get => dateOfBirth;
        set => dateOfBirth = value;
    }

    public string? Email {
        get => email;
        set => email = value;
    }
}