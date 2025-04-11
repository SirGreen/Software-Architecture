using BTL_SA.Modules.PatientMana.Domain.Models;

namespace BTL_SA.Modules.PatientMana.Infrastructure
{
    public interface IPatientRepository
    {
        List<Patient> FindAll();
        Patient FindById(int id);
        Patient FindByEmail(int id);
        int Create(PatientForm patient);
        int Update(int id, PatientForm patient);
        int Delete(int id);
    }
}