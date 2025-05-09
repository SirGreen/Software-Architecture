namespace BTL_SA.Modules.StaffMana.Domain.Model;

public class CredentialBase {
    private int id;
    private string? name;
    private string? issuingBody;
    private DateTime issueDate;
    private DateTime expirationDate;

    public CredentialBase(int id, string? name, string? issuingBody, DateTime issueDate, DateTime expirationDate) {
        this.id = id;
        this.name = name;
        this.issuingBody = issuingBody;
        this.issueDate = issueDate;
        this.expirationDate = expirationDate;
    }

    public int Id {
        get => id;
        set => id = value;
    }

    public string? Name {
        get => name;
        set => name = value;
    }

    public string? IssuingBody {
        get => issuingBody;
        set => issuingBody = value;
    }

    public DateTime IssueDate {
        get => issueDate;
        set => issueDate = value;
    }

    public DateTime ExpirationDate {
        get => expirationDate;
        set => expirationDate = value;
    }

    public void Renew(DateTime newExpirationDate) {
        if (newExpirationDate > expirationDate) {
            expirationDate = newExpirationDate;
        } else {
            throw new ArgumentException("New expiration date must be later than the current expiration date.");
        }
    }
}

public class License(int id, string? name, string? issuingBody, DateTime issueDate, DateTime expirationDate, string? number, string? restriction) : CredentialBase(id, name, issuingBody, issueDate, expirationDate) {
    private string? number = number;
    private string? restriction = restriction;

    public string? Number {
        get => number;
        set => number = value;
    }

    public string? Restriction {
        get => restriction;
        set => restriction = value;
    }
}

public class Certificate(int id, string? name, string? issuingBody, DateTime issueDate, DateTime expirationDate, string? level, string? version) : CredentialBase(id, name, issuingBody, issueDate, expirationDate) {
    private string? level = level;
    private string? version = version;

    public string? Level {
        get => level;
        set => level = value;
    }

    public string? Version {
        get => version;
        set => version = value;
    }
}