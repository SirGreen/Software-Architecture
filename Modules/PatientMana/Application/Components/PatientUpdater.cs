using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Modules.PatientMana.Domain.Models;
using BTL_SA.Modules.PatientMana.Infrastructure;
using BTL_SA.Modules.PatientMana.Application.Interface;

namespace BTL_SA.Modules.PatientMana.Application
{
    public class PatientUpdater(IPatientRepository patientRepo) : IPatientUpdater
    {
        private readonly IPatientRepository _patientRepo = patientRepo;
        public int UpdatePatient(int id, PatientForm patient) {
            if (patient == null) {
                return 0;
            }
            return _patientRepo.Update(id, patient);
        }
    }
}