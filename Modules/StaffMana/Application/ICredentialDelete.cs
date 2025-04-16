using BTL_SA.Modules.StaffMana.Domain.Model;

namespace BTL_SA.Modules.StaffMana.Application;

public interface ICredentialDelete
{
    int DeleteCredential(int cred);
}