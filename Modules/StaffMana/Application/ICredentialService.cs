using BTL_SA.Modules.StaffMana.Domain.Model;

namespace BTL_SA.Modules.StaffMana.Application;

public interface ICredentialService
{
    int UploadCredential(Employee employee, CredentialBase credential);
    int RenewCredential(int credentialId, DateTime newExprDate);
}