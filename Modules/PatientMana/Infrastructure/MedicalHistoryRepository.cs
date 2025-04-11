using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Modules.PatientMana.Domain.Models;

namespace BTL_SA.Modules.PatientMana.Infrastructure
{
    public class MedicalHistoryRepository
    {
        public List<MedicalHistoryView> findAll() {
            var parameters = new Dictionary<string, object>();
            if (_dbService.DataBaseInquiry("SoftwareArchitecture.GetAllMedicalHistory", parameters) is not SqlDataReader result)
                return [];
            using var reader = result;
            var medicalHistories = new List<MedicalHistoryView>();
            while (reader.Read())
            {
                var medicalHistory = new MedicalHistoryView
                {
                    Id = reader.GetInt32(reader.GetOrdinal("MedicalHistoryId")),
                    PatientVisitId = reader.GetInt32(reader.GetOrdinal("PatientVisitId")),
                    DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                    Department = reader.GetString(reader.GetOrdinal("Department")),
                    ReasonForVisit = reader.GetString(reader.GetOrdinal("ReasonForVisit")),
                    Diagnosis = reader.GetString(reader.GetOrdinal("Diagnosis")),
                    Treatment = reader.GetString(reader.GetOrdinal("Treatment")),
                    PrescribedMedication = reader.GetString(reader.GetOrdinal("PrescribedMedication")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),

                    PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                    HealthInsuranceId = reader.GetString(reader.GetOrdinal("HealthInsuranceId")),

                    PatientName = reader.GetString(reader.GetOrdinal("PatientName")),
                    Gender = reader.GetBoolean(reader.GetOrdinal("Gender")),
                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),

                    DoctorName = reader.GetString(reader.GetOrdinal("DoctorName"))
                };
                medicalHistories.Add(medicalHistory);
            }
            return medicalHistories;
        }
        public MedicalHistoryView findById(int id) {
            var parameters = new Dictionary<string, object>
            {
                { "@MedicalHistoryId", id }
            };
            if (_dbService.DataBaseInquiry("SoftwareArchitecture.GetMedicalHistoryById", parameters) is not SqlDataReader result)
                return null;
            using var reader = result;
            if (reader.Read())
            {
                var medicalHistory = new MedicalHistoryView
                {
                    Id = reader.GetInt32(reader.GetOrdinal("MedicalHistoryId")),
                    PatientVisitId = reader.GetInt32(reader.GetOrdinal("PatientVisitId")),
                    DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                    Department = reader.GetString(reader.GetOrdinal("Department")),
                    ReasonForVisit = reader.GetString(reader.GetOrdinal("ReasonForVisit")),
                    Diagnosis = reader.GetString(reader.GetOrdinal("Diagnosis")),
                    Treatment = reader.GetString(reader.GetOrdinal("Treatment")),
                    PrescribedMedication = reader.GetString(reader.GetOrdinal("PrescribedMedication")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),

                    PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                    HealthInsuranceId = reader.GetString(reader.GetOrdinal("HealthInsuranceId")),

                    PatientName = reader.GetString(reader.GetOrdinal("PatientName")),
                    Gender = reader.GetBoolean(reader.GetOrdinal("Gender")),
                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),

                    DoctorName = reader.GetString(reader.GetOrdinal("DoctorName"))
                };
                return medicalHistory;
            }
            else
            {
                Console.WriteLine("No data found for medical history with ID: " + id);
                return null;
            }
        }
        public List<MedicalHistoryView> findByPatientId(int id) {
            var parameters = new Dictionary<string, object>
            {
                { "@PatientId", id }
            };
            if (_dbService.DataBaseInquiry("SoftwareArchitecture.GetMedicalHistoryByPatientId", parameters) is not SqlDataReader result)
                return null;
            using var reader = result;
            var medicalHistories = new List<MedicalHistoryView>();
            while (reader.Read())
            {
                var medicalHistory = new MedicalHistoryView
                {
                    Id = reader.GetInt32(reader.GetOrdinal("MedicalHistoryId")),
                    PatientVisitId = reader.GetInt32(reader.GetOrdinal("PatientVisitId")),
                    DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                    Department = reader.GetString(reader.GetOrdinal("Department")),
                    ReasonForVisit = reader.GetString(reader.GetOrdinal("ReasonForVisit")),
                    Diagnosis = reader.GetString(reader.GetOrdinal("Diagnosis")),
                    Treatment = reader.GetString(reader.GetOrdinal("Treatment")),
                    PrescribedMedication = reader.GetString(reader.GetOrdinal("PrescribedMedication")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),

                    PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                    HealthInsuranceId = reader.GetString(reader.GetOrdinal("HealthInsuranceId")),

                    PatientName = reader.GetString(reader.GetOrdinal("PatientName")),
                    Gender = reader.GetBoolean(reader.GetOrdinal("Gender")),
                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),

                    DoctorName = reader.GetString(reader.GetOrdinal("DoctorName"))
                };
                medicalHistories.Add(medicalHistory);
            }
            return medicalHistories;
        }
        public int Create(MedicalHistoryForm medicalHistory) {
            var parameters = new Dictionary<string, object>
            {
                { "@PatientVisitId", medicalHistory.PatientVisitId },
                { "@DoctorId", medicalHistory.DoctorId },
                { "@Department", medicalHistory.Department ?? string.Empty },
                { "@ReasonForVisit", medicalHistory.ReasonForVisit ?? string.Empty },
                { "@Diagnosis", medicalHistory.Diagnosis ?? string.Empty },
                { "@Treatment", medicalHistory.Treatment ?? string.Empty },
                { "@PrescribedMedication", medicalHistory.PrescribedMedication ?? string.Empty }
            };

            // Define output parameter
            var outputParameters = new Dictionary<string, SqlParameter>
            {
                ["@NewMedicalHistoryId"] = new SqlParameter("@NewMedicalHistoryId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                }
            };

            var result = _dbService.DataBaseInquiry("SoftwareArchitecture.AddMedicalHistory", parameters, outputParameters);

            if (result is Dictionary<string, object> outputs &&
                outputs.TryGetValue("@NewMedicalHistoryId", out var newId) &&
                int.TryParse(newId?.ToString(), out var id))
            {
                Console.WriteLine("Created medical history ID: " + id);
                return id;
            }

            return 0;
        }
        public int Update(int id, MedicalHistoryForm medicalHistory) {
            var parameters = new Dictionary<string, object>
            {
                { "@MedicalHistoryId", id },
                { "@PatientVisitId", medicalHistory.PatientVisitId },
                { "@DoctorId", medicalHistory.DoctorId },
                { "@Department", medicalHistory.Department ?? string.Empty },
                { "@ReasonForVisit", medicalHistory.ReasonForVisit ?? string.Empty },
                { "@Diagnosis", medicalHistory.Diagnosis ?? string.Empty },
                { "@Treatment", medicalHistory.Treatment ?? string.Empty },
                { "@PrescribedMedication", medicalHistory.PrescribedMedication ?? string.Empty }
            };
            try
            {
                _dbService.DataBaseInquiry("SoftwareArchitecture.EditMedicalHistory", parameters);
                return 1;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error occurred while editing medical history: " + ex.Message);
                return 0; // or handle the error as needed
            }
        }
        public int Delete(int id) {
            var parameter = new Dictionary<string, object>
            {
                { "@MedicalHistoryId", id}
            }
            try { _dbService.DataBaseInquiry("SoftwareArchitecture.DeleteMedicalHistory", parameters); }
            catch (SqlException ex)
            {
                Console.WriteLine("Error occurred while deleting medical history: " + ex.Message);
                return 0; // or handle the error as needed
            }
            return 1;
        }
    }
}