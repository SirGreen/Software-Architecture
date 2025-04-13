using Microsoft.Data.SqlClient;
using System.Data;

namespace BTL_SA.Infrastructure;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SqlServerConnection") ?? throw new InvalidOperationException("Connection string 'SqlServerConnection' is not configured.");
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("Connection string 'SqlServerConnection' is not configured.");
        }
    }

    public object? DataBaseInquiry(
        string query,
        Dictionary<string, object>? parameters = null,
        Dictionary<string, SqlParameter>? outputParameters = null)
    {
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand(query, connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Add input parameters
        if (parameters != null)
        {
            foreach (var kvp in parameters)
            {
                var value = kvp.Value ?? DBNull.Value;

                // Handle VARBINARY if needed
                if (value is byte[] bytes)
                {
                    var binaryParam = new SqlParameter(kvp.Key, SqlDbType.VarBinary, -1)
                    {
                        Value = bytes
                    };
                    command.Parameters.Add(binaryParam);
                }
                else
                {
                    command.Parameters.AddWithValue(kvp.Key, value);
                }
            }
        }

        // Add output parameters
        if (outputParameters != null)
        {
            foreach (var kvp in outputParameters)
            {
                command.Parameters.Add(kvp.Value);
            }
        }

        connection.Open();
        var results = command.ExecuteReader();

        // Extract output parameter values
        if (outputParameters != null)
        {
            var outputResults = new Dictionary<string, object>();
            foreach (var kvp in outputParameters)
            {
                outputResults[kvp.Key] = command.Parameters[kvp.Key].Value;
            }
            return outputResults;
        }

        return results;
    }


}
