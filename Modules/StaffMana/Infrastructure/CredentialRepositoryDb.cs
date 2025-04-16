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
            if (credential is License license)
            {
                parameters.Add("@CredentialType", "License");
                parameters.Add("@Number", license.Number ?? string.Empty);
                parameters.Add("@Restriction", license.Restriction ?? string.Empty);
            }
            else if (credential is Certificate certificate)
            {
                parameters.Add("@CredentialType", "Certificate");
                parameters.Add("@Level", certificate.Level ?? string.Empty);
                parameters.Add("@Version", certificate.Version ?? string.Empty);
            }
            var result = _dbService.DataBaseInquiry("SoftwareArchitecture.AddCredential", parameters, outputParameters);
            var gId = 0;
            if (result is Dictionary<string, object> outputs &&
                outputs.TryGetValue("@NewCredentialId", out var newId) &&
                int.TryParse(newId?.ToString(), out var id))
            {
                Console.WriteLine("Created credential ID: " + id);
                gId = id;
            }

            _dbService.DataBaseInquiry("SoftwareArchitecture.AssignEmployeeToCredential", new Dictionary<string, object>
            {
                { "@EmployeeId", employee.Id },
                { "@CredentialId", gId }
            });

            return gId;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error creating credential: " + ex.Message);
            return 0;
        }
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
        try
        {
            if (reader.Read())
            {
                // Check if columns exist before attempting to access them
                bool hasRestrictionColumn = false;
                bool hasLevelColumn = false;
                    
                try { hasRestrictionColumn = reader.GetOrdinal("Restriction") >= 0; } 
                catch (IndexOutOfRangeException) { /* Column doesn't exist */ }
                    
                try { hasLevelColumn = reader.GetOrdinal("Level") >= 0; } 
                catch (IndexOutOfRangeException) { /* Column doesn't exist */ }
                if (hasRestrictionColumn)
                {
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
                }
                else
                {
                    var certificate = new Certificate(
                        id,
                        reader["Name"]?.ToString(),
                        reader["IssuingBody"]?.ToString(),
                        DateTime.TryParse(reader["IssueDate"]?.ToString(), out var issueDate) ? issueDate : default,
                        DateTime.TryParse(reader["ExpirationDate"]?.ToString(), out var expirateDate) ? expirateDate : default,
                        reader["Level"]?.ToString(),
                        reader["Version"]?.ToString()
                    );
                    Console.WriteLine("Created certificate ID: " + id);
                    return certificate;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading credential: " + ex.Message);
        }
        finally
        {
            reader.Close();
        }
        return null;
    }
    public int Update(CredentialBase credential)
    {
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
                parameters.Add("@CredentialType", "License");
                parameters.Add("@Number", license.Number ?? string.Empty);
                parameters.Add("@Restriction", license.Restriction ?? string.Empty);
            }
            else if (credential is Certificate certificate)
            {
                parameters.Add("@CredentialType", "Certificate");
                parameters.Add("@Level", certificate.Level ?? string.Empty);
                parameters.Add("@Version", certificate.Version ?? string.Empty);
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

    public int Delete(int id)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "@Id", id }
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

    public int AssignEmployeeToCredential(int credentialId, int employeeId)
    {
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