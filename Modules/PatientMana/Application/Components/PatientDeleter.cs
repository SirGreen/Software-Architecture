using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Modules.PatientMana.Infrastructure;
using BTL_SA.Modules.PatientMana.Application.Interface;

namespace BTL_SA.Modules.PatientMana.Application.Components
{
    public class PatientDeleter(IPatientRepository patientRepo) : IPatientDeleter
    {
        private readonly IPatientRepository _patientRepo = patientRepo;
        public int DeletePatient(int id) {
            return _patientRepo.Delete(id);
        }
    }
}