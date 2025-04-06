using BTL_SA.Modules.StaffMana.Domain.Model;

namespace BTL_SA.Modules.StaffMana.Infrastructure
{
    public interface ICredentialRepository
    {
        int Insert(CredentialBase credential, Employee employee);
        int Edit(CredentialBase credential);
        int Delete(CredentialBase credential);
        CredentialBase FindById(int id);
    }
}