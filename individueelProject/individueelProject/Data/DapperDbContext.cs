using Microsoft.Data.SqlClient;
using System.Data;

namespace individueelProject.Data
{
    public class DapperDbContext  
    {
        private readonly string _connectionString;

        public DapperDbContext(string configuration)
        {
            _connectionString = configuration;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
