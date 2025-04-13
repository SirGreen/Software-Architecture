using BTL_SA.Modules.PatientMana.Domain.Models;

namespace BTL_SA.Modules.PatientMana.Application.Interface
{
    public interface IPatientCreator
    {
        int CreatePatient(PatientForm patient);
    }
}