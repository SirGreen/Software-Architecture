using BTL_SA.Modules.StaffMana.Domain.Model;
using BTL_SA.Modules.StaffMana.Infrastructure;

namespace BTL_SA.Modules.StaffMana.Application;
public class CredentialDelete(ICredentialRepository credentialRepository) : ICredentialDelete
{
    private readonly ICredentialRepository _credentialRepository = credentialRepository;

    public int DeleteCredential(int cred)
    {
        if (cred <= 0) {
            Console.WriteLine("Invalid credential ID.");
            return 0; // Indicate failure
        }
        try {
            _credentialRepository.Delete(cred);
            return 1;
        } catch (Exception ex) {
            Console.WriteLine($"Error deleting credential: {ex.Message}");
            return -1; // Indicate failure
        }
    }
}