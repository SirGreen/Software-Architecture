namespace BTL_SA.Modules.StaffMana.Domain.Model;

public class Employee : Person {
    private int id;
    private string? departmemt;
    private string? role;
    private CredentialBase[]? credentials;
    
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

    public CredentialBase[]? Credentials {
        get => credentials;
        set => credentials = value;
    }

    public int AddCredential(CredentialBase credential) {
        try {
            if (credentials == null) {
                credentials = [credential];
            } else {
                Array.Resize(ref credentials, credentials.Length + 1);
                credentials[^1] = credential;
            }
            return credentials.Length;
        } catch {
            return 0;
        }
    }
}