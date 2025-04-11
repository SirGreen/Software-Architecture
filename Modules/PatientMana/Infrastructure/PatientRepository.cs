using BTL_SA.Modules.PatientMana.Domain.Models;
using BTL_SA.Infrastructure;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_SA.Modules.PatientMana.Infrastructure
{
    public class PatientRepository(DatabaseService dbService) : IPatientRepository
    {
        private readonly DatabaseService _dbService = dbService;
        public List<Patient> FindAll() {
            var parameters = new Dictionary<string, object>();
            if (_dbService.DataBaseInquiry("SoftwareArchitecture.GetAllPatients", parameters) is not SqlDataReader result)
                return [];
            using var reader = result;
            var patients = new List<Patient>();
            while (reader.Read())
            {
                var patient = new Patient
                {
                    Id = reader.GetInt32(reader.GetOrdinal("PatientId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Gender = reader.GetBoolean(reader.GetOrdinal("Gender")),
                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    HealthInsuranceId = reader.GetString(reader.GetOrdinal("HealthInsuranceId")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                };
                patients.Add(patient);
            }
            return patients;
        }
        public Patient FindById(int id) {
            var parameters = new Dictionary<string, object>
            {
                { "@PatientId", id }
            };
            if (_dbService.DataBaseInquiry("SoftwareArchitecture.GetPatientById", parameters) is not SqlDataReader result)
                return null;
            using var reader = result;
            if (reader.Read())
            {
                var patient = new Patient
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Gender = reader.GetBoolean(reader.GetOrdinal("Gender")),
                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    HealthInsuranceId = reader.GetString(reader.GetOrdinal("HealthInsuranceId")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                };
                return patient;
            }
            else
            {
                Console.WriteLine("No data found for Patient with ID: " + id);
                return null;
            }
        }
        public Patient FindByEmail(int id) {
            var parameters = new Dictionary<string, object>
            {
                { "@Email", id }
            };
            if (_dbService.DataBaseInquiry("SoftwareArchitecture.GetPatientByEmail", parameters) is not SqlDataReader result)
                return null;
            using var reader = result;
            if (reader.Read())
            {
                var patient = new Patient
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Gender = reader.GetBoolean(reader.GetOrdinal("Gender")),
                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    HealthInsuranceId = reader.GetString(reader.GetOrdinal("HealthInsuranceId")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                };
                return patient;
            }
            else
            {
                Console.WriteLine("No data found for Patient with ID: " + id);
                return null;
            }
        }
        public int Create(PatientForm patient) {
            var parameters = new Dictionary<string, object>
            {
                { "@Name", patient.Name ?? string.Empty },
                { "@Gender", patient.Gender },
                { "@PhoneNumber", patient.PhoneNumber ?? string.Empty },
                { "@Address", patient.Address ?? string.Empty },
                { "@DateOfBirth", patient.DateOfBirth },
                { "@Email", patient.Email ?? string.Empty },
                { "@HealthInsuranceId", patient.HealthInsuranceId ?? string.Empty }
            };

            // Define output parameter
            var outputParameters = new Dictionary<string, SqlParameter>
            {
                ["@NewPatientId"] = new SqlParameter("@NewPatientId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                }
            };

            var result = _dbService.DataBaseInquiry("SoftwareArchitecture.AddPatient", parameters, outputParameters);

            if (result is Dictionary<string, object> outputs &&
                outputs.TryGetValue("@NewPatientId", out var newId) &&
                int.TryParse(newId?.ToString(), out var id))
            {
                Console.WriteLine("Created Patient ID: " + id);
                return id;
            }

            return 0;
        }
        public int Update(int id, PatientForm patient) {
            var parameters = new Dictionary<string, object>
            {
                { "@PatientId", id },
                { "@Name", patient.Name ?? string.Empty },
                { "@Gender", patient.Gender },
                { "@PhoneNumber", patient.PhoneNumber ?? string.Empty },
                { "@Address", patient.Address ?? string.Empty },
                { "@DateOfBirth", patient.DateOfBirth },
                { "@Email", patient.Email ?? string.Empty },
                { "@HealthInsuranceId", patient.HealthInsuranceId ?? string.Empty }
            };
            try
            {
                _dbService.DataBaseInquiry("SoftwareArchitecture.EditPatient", parameters);
                return 1;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error occurred while editing patient: " + ex.Message);
                return 0; // or handle the error as needed
            }
        }
        public int Delete(int id) {
            var parameter = new Dictionary<string, object>
            {
                { "@PatientId", id}
            }
            try { _dbService.DataBaseInquiry("SoftwareArchitecture.DeletePatient", parameters); }
            catch (SqlException ex)
            {
                Console.WriteLine("Error occurred while deleting patient: " + ex.Message);
                return 0; // or handle the error as needed
            }
            return 1;
        }
    }
}