using BTL_SA.Infrastructure;
using BTL_SA.Modules.PatientMana.Domain.Models;

namespace BTL_SA.Modules.PatientMana.Infrastructure
{
    public class PatientVisitRepository(DatabaseService dbService) : IPatientVisitRepository
    {
        private readonly DatabaseService _dbService = dbService;
        public List<PatientVisitView> findAll() {
            var parameters = new Dictionary<string, object>();
            if (_dbService.DataBaseInquiry("SoftwareArchitecture.GetAllPatientVisits", parameters) is not SqlDataReader result)
                return [];
            using var reader = result;
            var patientVisits = new List<PatientVisitView>();
            while (reader.Read())
            {
                var patientVisit = new PatientVisitView
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                    VisitDate = reader.GetDateTime(reader.GetOrdinal("VisitDate")),
                    Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                    
                    Name = reader.GetString(reader.GetOrdinal("PatientName")),
                    Gender = reader.GetBoolean(reader.GetOrdinal("Gender")),
                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    
                    HealthInsuranceId = reader.GetString(reader.GetOrdinal("HealthInsuranceId"))
                };
                patientVisits.Add(patientVisit);
            }
            return patientVisits;
        }
        public PatientVisit findById(int id) {
            var parameters = new Dictionary<string, object>
            {
                { "@VisitId", id }
            };
            if (_dbService.DataBaseInquiry("SoftwareArchitecture.GetPatientVisitById", parameters) is not SqlDataReader result)
                return null;
            using var reader = result;
            if (reader.Read())
            {
                var patientVisit = new PatientVisitView
                {
                    Id = reader.GetInt32(reader.GetOrdinal("VisitId")),
                    PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                    VisitDate = reader.GetDateTime(reader.GetOrdinal("VisitDate")),
                    Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                    
                    Name = reader.GetString(reader.GetOrdinal("PatientName")),
                    Gender = reader.GetBoolean(reader.GetOrdinal("Gender")),
                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    
                    HealthInsuranceId = reader.GetString(reader.GetOrdinal("HealthInsuranceId"))
                };
                return patientVisit;
            }
            else
            {
                Console.WriteLine("No data found for Patient Visit with ID: " + id);
                return null;
            }
        }
        public List<PatientVisit> findByPatientId(int id) {
            var parameters = new Dictionary<string, object> {
                { "@PatientId", id}
            };
            if (_dbService.DataBaseInquiry("SoftwareArchitecture.GetPatientVisitsByPatientId", parameters) is not SqlDataReader result)
                return null;
            using var reader = result;
            var patientVisits = new List<PatientVisitView>();
            while (reader.Read())
            {
                var patientVisit = new PatientVisitView
                {
                    Id = reader.GetInt32(reader.GetOrdinal("VisitId")),
                    PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                    VisitDate = reader.GetDateTime(reader.GetOrdinal("VisitDate")),
                    Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                    
                    Name = reader.GetString(reader.GetOrdinal("PatientName")),
                    Gender = reader.GetBoolean(reader.GetOrdinal("Gender")),
                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    
                    HealthInsuranceId = reader.GetString(reader.GetOrdinal("HealthInsuranceId"))
                };
                patientVisits.Add(patientVisit);
            }
            return patientVisits;
        }
        public int Create(PatientVisitForm patient) {
            var parameters = new Dictionary<string, object>
            {
                { "@PatientId", patient.PatientId },
                { "@VisitDate", patient.VisitDate },
                { "@Notes", patient.Notes ?? string.Empty }
            };

            // Define output parameter
            var outputParameters = new Dictionary<string, SqlParameter>
            {
                ["@NewVisitId"] = new SqlParameter("@NewVisitId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                }
            };

            var result = _dbService.DataBaseInquiry("SoftwareArchitecture.AddPatientVisit", parameters, outputParameters);

            if (result is Dictionary<string, object> outputs &&
                outputs.TryGetValue("@NewVisitId", out var newId) &&
                int.TryParse(newId?.ToString(), out var id))
            {
                Console.WriteLine("Created Patient Visit ID: " + id);
                return id;
            }

            return 0;
        }
        public int Update(int id, PatientVisitForm patient) {
            var parameters = new Dictionary<string, object>
            {
                { "@VisitId", id},
                { "@PatientId", patient.PatientId },
                { "@VisitDate", patient.VisitDate },
                { "@Notes", patient.Notes ?? string.Empty }
            };
            try
            {
                _dbService.DataBaseInquiry("SoftwareArchitecture.EditPatientVisit", parameters);
                return 1;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error occurred while editing patient visit: " + ex.Message);
                return 0; // or handle the error as needed
            }
        }
        public int Delete(int id) {
            var parameter = new Dictionary<string, object>
            {
                { "@VisitId", id}
            };
            try { _dbService.DataBaseInquiry("SoftwareArchitecture.DeletePatientVisit", parameters); }
            catch (SqlException ex)
            {
                Console.WriteLine("Error occurred while deleting patient visit: " + ex.Message);
                return 0; // or handle the error as needed
            }
            return 1;
        }
    }
}