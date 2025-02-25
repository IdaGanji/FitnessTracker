using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace FitnessTracker.Data;

public class DataContextDapper
{
    private IConfiguration _configuration;
    private string _connectionString;
    public DataContextDapper(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
        

    // To be able to set up a method to accept a generic type 
    // The actual type is inferred when the method is called.
    public IEnumerable<T> LoadDate<T>(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_connectionString);
        return dbConnection.Query<T>(sql);
    }
    
    // Returns the number of rows that were affected
    public bool ExecuteSql(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_connectionString);
        return (dbConnection.Execute(sql)>0);
    }
}