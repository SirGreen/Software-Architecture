using BTL_SA.Modules.StaffMana.Domain.Model;
using BTL_SA.Infrastructure;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Http.Headers;

namespace BTL_SA.Modules.StaffMana.Infrastructure;

public class CredentialRepositoryDb(DatabaseService dbService) : ICredentialRepository
{
    private readonly DatabaseService _dbService = dbService;

    public int Create(CredentialBase credential, Employee employee)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "@Name", credential.Name ?? string.Empty },
                { "@IssueDate", credential.IssueDate},
                { "@ExpirationDate", credential.ExpirationDate },
                { "@IssuingBody", credential.IssuingBody ?? string.Empty }
            };
            // Define output parameter
            var outputParameters = new Dictionary<string, SqlParameter>
            {
                ["@NewCredentialId"] = new SqlParameter("@NewCredentialId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                }
            };
            var outString = "";
            if (credential is License license)
            {
                outString = "@NewLicenseId";
                parameters.Add("@Number", license.LicenseIden ?? string.Empty);
                parameters.Add("@Restriction", license.LicenseName ?? string.Empty);
            }
            else if (credential is Certificate certificate)
            {
                outString = "@NewCertificateId";
                parameters.Add("@Level", certificate.CertificateIden ?? string.Empty);
                parameters.Add("@Version", certificate.CertificateName ?? string.Empty);
            }
            var result = _dbService.DataBaseInquiry("SoftwareArchitecture.AddCredential", parameters, outputParameters);
            if (result is Dictionary<string, object> outputs &&
                outputs.TryGetValue(outString, out var newId) &&
                int.TryParse(newId?.ToString(), out var id))
            {
                Console.WriteLine("Created credential ID: " + id);
                return id;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error creating credential: " + ex.Message);
            return 0;
        }

        return 1;
    }

    public CredentialBase? FindById(int id)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@Id", id }
        };
        if (_dbService.DataBaseInquiry("SoftwareArchitecture.GetCredentialById", parameters) is not SqlDataReader result)
            return null;
        using var reader = result; 
        if (reader.Read())
        {
            if (reader["Restriction"] != null) {
                var license = new License(
                    id,
                    reader["Name"]?.ToString(),
                    reader["IssuingBody"]?.ToString(),
                    DateTime.TryParse(reader["IssueDate"]?.ToString(), out var issueDate) ? issueDate : default,
                    DateTime.TryParse(reader["ExpirationDate"]?.ToString(), out var expirateDate) ? expirateDate : default,
                    reader["Number"]?.ToString(),
                    reader["Restriction"]?.ToString()
                );
                return license;
            } else {
                var certificate = new Certificate(
                    id,
                    reader["Name"]?.ToString(),
                    reader["IssuingBody"]?.ToString(),
                    DateTime.TryParse(reader["IssueDate"]?.ToString(), out var issueDate) ? issueDate : default,
                    DateTime.TryParse(reader["ExpirationDate"]?.ToString(), out var expirateDate) ? expirateDate : default,
                    reader["Level"]?.ToString(),
                    reader["Version"]?.ToString()
                );
                return certificate;
            }
        }
        return null;
    }

    public int Update(CredentialBase credential) {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "@Id", credential.Id },
                { "@Name", credential.Name ?? string.Empty },
                { "@IssueDate", credential.IssueDate },
                { "@ExpirationDate", credential.ExpirationDate },
                { "@IssuingBody", credential.IssuingBody ?? string.Empty }
            };
            if (credential is License license)
            {
                parameters.Add("@CredentialType","License");
                parameters.Add("@Number", license.LicenseIden ?? string.Empty);
                parameters.Add("@Restriction", license.LicenseName ?? string.Empty);
            }
            else if (credential is Certificate certificate)
            {
                parameters.Add("@CredentialType","Certificate");
                parameters.Add("@Level", certificate.CertificateIden ?? string.Empty);
                parameters.Add("@Version", certificate.CertificateName ?? string.Empty);
            }
            _dbService.DataBaseInquiry("SoftwareArchitecture.UpdateCredential", parameters);
            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error updating credential: " + ex.Message);
            return 0;
        }
    }

    public int Delete(CredentialBase credential) {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "@Id", credential.Id }
            };
            _dbService.DataBaseInquiry("SoftwareArchitecture.DeleteCredential", parameters);
            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error deleting credential: " + ex.Message);
            return 0;
        }
    }

    public int AssignEmployeeToCredential(int credentialId, int employeeId) {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "@EmployeeId", employeeId },
                { "@CredentialId", credentialId }
            };
            _dbService.DataBaseInquiry("SoftwareArchitecture.AssignEmployeeToCredential", parameters);
            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error assigning employee to credential: " + ex.Message);
            return 0;
        }
    }
}