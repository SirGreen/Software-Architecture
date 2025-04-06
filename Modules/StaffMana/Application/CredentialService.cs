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

    public int RenewCredential(CredentialBase credential)
    {
        if (credential == null)
        {
            return 0;
        }
        var id = _credentialRepository.Update(credential);
        return id;
    }

}