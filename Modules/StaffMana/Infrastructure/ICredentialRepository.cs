using BTL_SA.Modules.StaffMana.Domain.Model;

namespace BTL_SA.Modules.StaffMana.Infrastructure
{
    public interface ICredentialRepository
    {
        int Create(CredentialBase credential, Employee employee);
        CredentialBase? FindById(int id);
        int Update(CredentialBase credential);
        int Delete(int id);
        int AssignEmployeeToCredential(int credentialId, int employeeId);
    }
}