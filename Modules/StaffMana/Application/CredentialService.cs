using BTL_SA.Modules.StaffMana.Domain.Model;
using BTL_SA.Modules.StaffMana.Infrastructure;

namespace BTL_SA.Modules.StaffMana.Application;
public class CredentialService(ICredentialRepository credentialRepository) : ICredentialService
{
    private readonly ICredentialRepository _credentialRepository = credentialRepository;

    public int UploadCredential(Employee employee, CredentialBase credential)
    {
        if (employee == null || credential == null)
        {
            return 0;
        }
        var id = _credentialRepository.Create(credential, employee);
        return id;
    }

    public int RenewCredential(int credentialId, DateTime newExprDate)
    {
        if (credentialId <= 0)
        {
            return 0;
        }
        var credential = _credentialRepository.FindById(credentialId);
        if (credential == null || credential.ExpirationDate >= newExprDate)
        {
            return -1;
        }
        credential.ExpirationDate = newExprDate;
        var id = _credentialRepository.Update(credential);
        return id;
    }

}