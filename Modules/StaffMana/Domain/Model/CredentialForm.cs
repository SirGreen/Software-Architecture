namespace BTL_SA.Modules.StaffMana.Domain.Model;
public class CredentialForm : CredentialBase
{
    public string CertificateType { get; set; }
    public string LicenseNumber { get; set; }
    public string LicenseRestriction { get; set; }
    public string CertificateLevel { get; set; }
    public string CertificateVersion { get; set; }

    public CredentialForm(
        int id,
        string name,
        string issuingBody,
        DateTime issueDate,
        DateTime expirationDate,
        string certificateType,
        string licenseNumber,
        string licenseRestriction,
        string certificateLevel,
        string certificateVersion)
        : base(id, name, issuingBody, issueDate, expirationDate)
    {
        CertificateType = certificateType;
        LicenseNumber = licenseNumber;
        LicenseRestriction = licenseRestriction;
        CertificateLevel = certificateLevel;
        CertificateVersion = certificateVersion;
    }

    public object CreateLicenseOrCertificate()
    {
        if (CertificateType == "License")
        {
            return new License(
                Id, Name, IssuingBody, IssueDate, ExpirationDate, LicenseNumber, LicenseRestriction
            );
        }
        else if (CertificateType == "Credential")
        {
            return new Certificate(
                Id, Name, IssuingBody, IssueDate, ExpirationDate, CertificateLevel, CertificateVersion
            );
        }
        throw new ArgumentException("Invalid certificate type. Must be either 'License' or 'Credential'.");
    }
}
