using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Infrastructure;
using BTL_SA.Modules.PatientMana.Domain.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_SA.Modules.PatientMana.Infrastructure
{
    public class MedicalHistoryRepository(DatabaseService dbService) : IMedicalHistoryRepository
    {
        private readonly DatabaseService _dbService = dbService;
        public List<MedicalHistoryView> FindAll() {
            var parameters = new Dictionary<string, object>();
            if (_dbService.DataBaseInquiry("SoftwareArchitecture.GetAllMedicalHistory", parameters) is not SqlDataReader result)
                return [];
            using var reader = result;
            var medicalHistories = new List<MedicalHistoryView>();
            while (reader.Read())
            {
                var medicalHistory = new MedicalHistoryView
                {
                    Id = reader.IsDBNull(reader.GetOrdinal("MedicalHistoryId")) ? 0 : reader.GetInt32(reader.GetOrdinal("MedicalHistoryId")),
                    PatientVisitId = reader.IsDBNull(reader.GetOrdinal("PatientVisitId")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientVisitId")),
                    DoctorId = reader.IsDBNull(reader.GetOrdinal("DoctorId")) ? 0 : reader.GetInt32(reader.GetOrdinal("DoctorId")),
                    Department = reader.IsDBNull(reader.GetOrdinal("Department")) ? null : reader.GetString(reader.GetOrdinal("Department")),
                    ReasonForVisit = reader.IsDBNull(reader.GetOrdinal("ReasonForVisit")) ? null : reader.GetString(reader.GetOrdinal("ReasonForVisit")),
                    Diagnosis = reader.IsDBNull(reader.GetOrdinal("Diagnosis")) ? null : reader.GetString(reader.GetOrdinal("Diagnosis")),
                    Treatment = reader.IsDBNull(reader.GetOrdinal("Treatment")) ? null : reader.GetString(reader.GetOrdinal("Treatment")),
                    PrescribedMedication = reader.IsDBNull(reader.GetOrdinal("PrescribedMedication")) ? null : reader.GetString(reader.GetOrdinal("PrescribedMedication")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),
                };
                medicalHistories.Add(medicalHistory);
            }
            return medicalHistories;
        }
        public MedicalHistoryView? FindById(int id) {
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
                    PatientVisitId = reader.IsDBNull(reader.GetOrdinal("PatientVisitId")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientVisitId")),
                    DoctorId = reader.IsDBNull(reader.GetOrdinal("DoctorId")) ? 0 : reader.GetInt32(reader.GetOrdinal("DoctorId")),
                    Department = reader.GetString(reader.GetOrdinal("Department")),
                    ReasonForVisit = reader.GetString(reader.GetOrdinal("ReasonForVisit")),
                    Diagnosis = reader.GetString(reader.GetOrdinal("Diagnosis")),
                    Treatment = reader.GetString(reader.GetOrdinal("Treatment")),
                    PrescribedMedication = reader.GetString(reader.GetOrdinal("PrescribedMedication")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),
                };
                return medicalHistory;
            }
            else
            {
                Console.WriteLine("No data found for medical history with ID: " + id);
                return null;
            }
        }
        public List<MedicalHistoryView>? FindByPatientId(int id) {
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
                    PatientVisitId = reader.IsDBNull(reader.GetOrdinal("PatientVisitId")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientVisitId")),
                    DoctorId = reader.IsDBNull(reader.GetOrdinal("DoctorId")) ? 0 : reader.GetInt32(reader.GetOrdinal("DoctorId")),
                    Department = reader.GetString(reader.GetOrdinal("Department")),
                    ReasonForVisit = reader.GetString(reader.GetOrdinal("ReasonForVisit")),
                    Diagnosis = reader.GetString(reader.GetOrdinal("Diagnosis")),
                    Treatment = reader.GetString(reader.GetOrdinal("Treatment")),
                    PrescribedMedication = reader.GetString(reader.GetOrdinal("PrescribedMedication")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),
                };
                medicalHistories.Add(medicalHistory);
            }
            return medicalHistories;
        }
        public int Create(MedicalHistoryForm medicalHistory) {
            var parameters = new Dictionary<string, object>
            {
                { "@PatientVisitId", medicalHistory.PatientVisitId ?? 0 },
                { "@DoctorId", medicalHistory.DoctorId ?? 0 },
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
                { "@PatientVisitId", medicalHistory.PatientVisitId ?? 0 },
                { "@DoctorId", medicalHistory.DoctorId ?? 0 },
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
            var parameters = new Dictionary<string, object>
            {
                { "@MedicalHistoryId", id}
            };
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