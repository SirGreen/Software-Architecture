using BTL_SA.Modules.PatientMana.Application.Interface;
using BTL_SA.Modules.PatientMana.Domain.Models;
using BTL_SA.Modules.PatientMana.Infrastructure;

namespace BTL_SA.Modules.PatientMana.Application
{
    public class PatientCreator(IPatientRepository patientRepo) : IPatientCreator
    {
        private readonly IPatientRepository _patientRepo = patientRepo;

        public int CreatePatient(PatientForm patient) {
            if (patient == null) {
                return 0;
            }
            var id = _patientRepo.Create(patient);

            return id;
        }
    }
}