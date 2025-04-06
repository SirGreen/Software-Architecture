namespace BTL_SA.Modules.StaffMana.Domain.Model;

public class CredentialBase {
    private int id;
    private string? issuingBody;
    private DateTime issueDate;
    private DateTime expirationDate;

    public CredentialBase(int id, string? issuingBody, DateTime issueDate, DateTime expirationDate) {
        this.id = id;
        this.issuingBody = issuingBody;
        this.issueDate = issueDate;
        this.expirationDate = expirationDate;
    }

    public int Id {
        get => id;
        set => id = value;
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

public class License : CredentialBase {
    private string? licenseIden;
    private string? licenseName;

    public License(int id, string? issuingBody, DateTime issueDate, DateTime expirationDate, string? licenseIden, string? licenseName) : base(id, issuingBody, issueDate, expirationDate) {
        this.licenseIden = licenseIden;
        this.licenseName = licenseName;
    }

    public string? LicenseIden {
        get => licenseIden;
        set => licenseIden = value;
    }

    public string? LicenseName {
        get => licenseName;
        set => licenseName = value;
    }
}

public class Certificate : CredentialBase {
    private string? certificateIden;
    private string? certificateName;

    public Certificate(int id, string? issuingBody, DateTime issueDate, DateTime expirationDate, string? certificateIden, string? certificateName) : base(id, issuingBody, issueDate, expirationDate) {
        this.certificateIden = certificateIden;
        this.certificateName = certificateName;
    }

    public string? CertificateIden {
        get => certificateIden;
        set => certificateIden = value;
    }

    public string? CertificateName {
        get => certificateName;
        set => certificateName = value;
    }
}