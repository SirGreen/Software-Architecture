using BTL_SA.Modules.StaffMana.Domain.Model;
using BTL_SA.Infrastructure;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_SA.Modules.StaffMana.Infrastructure;

public class StaffRepositoryDb(DatabaseService dbService) : IStaffRepository
{
    private readonly DatabaseService _dbService = dbService;
    public int Create(EmployeeForm employee)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@Name", employee.Name ?? string.Empty },
            { "@Gender", employee.Gender },
            { "@PhoneNumber", employee.PhoneNumber ?? string.Empty },
            { "@Address", employee.Address ?? string.Empty },
            { "@DateOfBirth", employee.DateOfBirth },
            { "@Email", employee.Email ?? string.Empty },
            { "@Department", employee.Department ?? string.Empty },
            { "@Role", employee.Role ?? string.Empty }
        };

        // Define output parameter
        var outputParameters = new Dictionary<string, SqlParameter>
        {
            ["@NewEmployeeId"] = new SqlParameter("@NewEmployeeId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            }
        };

        var result = _dbService.DataBaseInquiry("SoftwareArchitecture.AddEmployee", parameters, outputParameters);

        if (result is Dictionary<string, object> outputs &&
            outputs.TryGetValue("@NewEmployeeId", out var newId) &&
            int.TryParse(newId?.ToString(), out var id))
        {
            Console.WriteLine("Created employee ID: " + id);
            return id;
        }

        return 0;
    }
    public int Edit(EmployeeForm employee)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@Id", employee.Id },
            { "@Name", employee.Name ?? string.Empty },
            { "@Gender", employee.Gender },
            { "@PhoneNumber", employee.PhoneNumber ?? string.Empty },
            { "@Address", employee.Address ?? string.Empty },
            { "@DateOfBirth", employee.DateOfBirth },
            { "@Email", employee.Email ?? string.Empty },
            { "@Department", employee.Department ?? string.Empty },
            { "@Role", employee.Role ?? string.Empty }
        };
        try
        {
            _dbService.DataBaseInquiry("SoftwareArchitecture.EditEmployee", parameters);
            return 1;
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error occurred while editing employee: " + ex.Message);
            return 0; // or handle the error as needed
        }
    }
    public int Delete(int id)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@Id", id }
        };
        try { _dbService.DataBaseInquiry("SoftwareArchitecture.DeleteEmployee", parameters); }
        catch (SqlException ex)
        {
            Console.WriteLine("Error occurred while deleting employee: " + ex.Message);
            return 0; // or handle the error as needed
        }
        return 1; // Assuming deletion is successful
    }
    public List<CredentialBase> GetEmployeeCredentials(int id)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@EmployeeId", id }
        };
        if (_dbService.DataBaseInquiry("SoftwareArchitecture.GetEmployeeCredentials", parameters) is not SqlDataReader result)
            return [];
        // Here, you need to process the SqlDataReader and map the data to an array of CredentialBase objects.
        using var reader = result;
        var credentials = new List<CredentialBase>();
        while (reader.Read())
        {
            var credentialType = reader.GetString(reader.GetOrdinal("CredentialType"));
            if (credentialType == "License") // Assuming 1 is for License
            {
                var license = new License(
                    reader.GetInt32(reader.GetOrdinal("CredentialId")),
                    reader.GetString(reader.GetOrdinal("CredentialName")),
                    reader.GetString(reader.GetOrdinal("IssuingBody")),
                    reader.GetDateTime(reader.GetOrdinal("IssueDate")),
                    reader.GetDateTime(reader.GetOrdinal("ExpirationDate")),
                    reader.GetString(reader.GetOrdinal("LicenseNumber")),
                    reader.GetString(reader.GetOrdinal("LicenseRestriction"))
                );
                credentials.Add(license);
            }
            else if (credentialType == "Certification") // Assuming 2 is for CredentialBase
            {
                var credential = new Certificate(
                    reader.GetInt32(reader.GetOrdinal("CredentialId")),
                    reader.GetString(reader.GetOrdinal("CredentialName")),
                    reader.GetString(reader.GetOrdinal("IssuingBody")),
                    reader.GetDateTime(reader.GetOrdinal("IssueDate")),
                    reader.GetDateTime(reader.GetOrdinal("ExpirationDate")),
                    reader.GetString(reader.GetOrdinal("CertificationLevel")),
                    reader.GetString(reader.GetOrdinal("CertificationVersion"))
                );
                credentials.Add(credential);
            }
        }
        return [.. credentials];
    }
    public Employee? FindById(int id)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@Id", id }
        };
        if (_dbService.DataBaseInquiry("SoftwareArchitecture.FindById", parameters) is not SqlDataReader result)
            return null;
        using var reader = result;
        if (reader.Read())
        {
            var employee = new Employee
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Gender = reader.GetBoolean(reader.GetOrdinal("Gender")),
                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                Address = reader.GetString(reader.GetOrdinal("Address")),
                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Department = reader.GetString(reader.GetOrdinal("Department")),
                Role = reader.GetString(reader.GetOrdinal("Role"))
            };
            return employee;
        }
        else
        {
            Console.WriteLine("No data found for Employee with ID: " + id);
            return null;
        }
    }
    public List<Employee> GetAll()
    {

        var parameters = new Dictionary<string, object>();
        if (_dbService.DataBaseInquiry("SoftwareArchitecture.GetAllEmployees", parameters) is not SqlDataReader result)
            return [];
        using var reader = result;
        var employees = new List<Employee>();
        while (reader.Read())
        {
            var employee = new Employee
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Gender = reader.GetBoolean(reader.GetOrdinal("Gender")),
                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                Address = reader.GetString(reader.GetOrdinal("Address")),
                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Department = reader.GetString(reader.GetOrdinal("Department")),
                Role = reader.GetString(reader.GetOrdinal("Role"))
            };
            employees.Add(employee);
        }
        return employees;
    }
}
