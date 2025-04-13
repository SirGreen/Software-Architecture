using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Modules.PatientMana.Application.Interface;
using BTL_SA.Modules.PatientMana.Domain.Models; 

namespace BTL_SA.Modules.PatientMana.Facade
{
    public class FacadePatientManagement(
        IPatientRetriever patientRetriever,
        IPatientCreator patientCreator,
        IPatientUpdater patientUpdater,
        IPatientDeleter patientDeleter,
        IPatientVisitRetriever patientVisitRetriever,
        IPatientVisitCreator patientVisitCreator,
        IPatientVisitUpdater patientVisitUpdater,
        IPatientVisitDeleter patientVisitDeleter,
        IMedicalHistoryRetriever medicalHistoryRetriever,
        IMedicalHistoryCreator medicalHistoryCreator,
        IMedicalHistoryUpdater medicalHistoryUpdater,
        IMedicalHistoryDeleter medicalHistoryDeleter
    )
    {
        private readonly IPatientRetriever _patientRetriever = patientRetriever;
        private readonly IPatientCreator _patientCreator = patientCreator;
        private readonly IPatientUpdater _patientUpdater = patientUpdater;
        private readonly IPatientDeleter _patientDeleter = patientDeleter;
        private readonly IPatientVisitRetriever _patientVisitRetriever = patientVisitRetriever;
        private readonly IPatientVisitCreator _patientVisitCreator = patientVisitCreator;
        private readonly IPatientVisitUpdater _patientVisitUpdater = patientVisitUpdater;
        private readonly IPatientVisitDeleter _patientVisitDeleter = patientVisitDeleter;
        private readonly IMedicalHistoryRetriever _medicalHistoryRetriever = medicalHistoryRetriever;
        private readonly IMedicalHistoryCreator _medicalHistoryCreator = medicalHistoryCreator;
        private readonly IMedicalHistoryUpdater _medicalHistoryUpdater = medicalHistoryUpdater;
        private readonly IMedicalHistoryDeleter _medicalHistoryDeleter = medicalHistoryDeleter;

        public Patient? GetPatientById(int id) {
            return _patientRetriever.GetPatientById(id);
        }
        public Patient? GetPatientByEmail(string email) {
            return _patientRetriever.GetPatientByEmail(email);
        }
        public List<Patient> GetAllPatient() {
            return _patientRetriever.GetAllPatient();
        }
        public int CreatePatient(PatientForm patient) {
            return _patientCreator.CreatePatient(patient);
        }
        public int UpdatePatient(int id, PatientForm patient) {
            return _patientUpdater.UpdatePatient(id, patient);
        }
        public int DeletePatient(int id) {
            return _patientDeleter.DeletePatient(id);
        }
        public List<PatientVisitView>? GetAllVisit() {
            return _patientVisitRetriever.GetAllVisit();
        }
        public List<PatientVisitView>? GetVisitByPatientId(int id) {
            return _patientVisitRetriever.GetVisitByPatientId(id);
        }
        public PatientVisitView? GetVisitById(int id) {
            return _patientVisitRetriever.GetVisitById(id);
        }
        public int CreatePatientVisit(PatientVisitForm patientVisitForm) {
            return _patientVisitCreator.CreatePatientVisit(patientVisitForm);
        }
        public int UpdatePatientVisit(int id, PatientVisitForm patientVisit) {
            return _patientVisitUpdater.UpdatePatientVisit(id, patientVisit);
        }
        public int DeletePatientVisit(int id) {
            return _patientVisitDeleter.DeletePatientVisit(id);
        }

        public List<MedicalHistoryView> GetAllMedicalHistory() {
            return _medicalHistoryRetriever.GetAllMedicalHistory();
        }
        public MedicalHistoryView? GetMedicalHistoryById(int id) {
            return _medicalHistoryRetriever.GetMedicalHistoryById(id);
        }
        public int CreateMedicalHistory(MedicalHistoryForm medicalHistoryForm) {
            return _medicalHistoryCreator.CreateMedicalHistory(medicalHistoryForm);
        }
        public int UpdateMedicalHistory(int id, MedicalHistoryForm medicalHistoryForm) {
            return _medicalHistoryUpdater.UpdateMedicalHistory(id, medicalHistoryForm);
        }
        public int DeleteMedicalHistory(int id) {
            return _medicalHistoryDeleter.DeleteMedicalHistory(id);
        }
    }
}