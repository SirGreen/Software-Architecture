using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Modules.PatientMana.Infrastructure;
using BTL_SA.Modules.PatientMana.Application.Interface;
using BTL_SA.Modules.PatientMana.Domain.Models;

namespace BTL_SA.Modules.PatientMana.Application.Components
{
    public class PatientRetriever(IPatientRepository patientRepo) : IPatientRetriever 
    {
        private readonly IPatientRepository _patientRepo = patientRepo;
        public Patient? GetPatientById(int id) {
            if (id < 0) {
                return null;
            }
            var patient = _patientRepo.FindById(id);
            return patient;
        }
        public Patient? GetPatientByEmail(string email) {
            var patient = _patientRepo.FindByEmail(email);
            return patient;
        }
        public List<Patient> GetAllPatient() {
            return _patientRepo.FindAll();
        }
    }
}