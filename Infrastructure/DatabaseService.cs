using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace BTL_SA.Infrastructure;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        }
    }

    protected int DataBaseInquiry(string query, params object[] parameters)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(query, connection);
        if (parameters != null)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                command.Parameters.AddWithValue($"@param{i}", parameters[i]);
            }
        }

        connection.Open();
        return command.ExecuteNonQuery();
    }
}
