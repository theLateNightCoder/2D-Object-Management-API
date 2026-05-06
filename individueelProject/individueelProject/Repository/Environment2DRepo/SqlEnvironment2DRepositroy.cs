using Dapper;
using individueelProject.Data;
using individueelProject.Repository.Models;

namespace individueelProject.Repository.Environment2DRepo
{
    public class SqlEnvironment2DRepositroy : IEnivronmentRepository
    {
        private readonly DapperDbContext _context;

        public SqlEnvironment2DRepositroy(DapperDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> GetEnvironmentIdAsync(string name , string userId)
        {
            using var connection = _context.CreateConnection();

            string query = "SELECT Id FROM Environment2D WHERE Name = @Name AND OwnerUserId = @UserId";

            return await connection.QueryFirstOrDefaultAsync<Guid>(query, new { Name = name , UserId = userId });
        }

        public async Task<IEnumerable<Environment2D>> GetByUserIdAsync(string userId)
        {
            using var connection = _context.CreateConnection();

            string query = "SELECT * FROM Environment2D WHERE OwnerUserId = @UserId";   

            return await connection.QueryAsync<Environment2D>(query, new { UserId = userId });
        }

        public async Task<IEnumerable<Environment2D>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();

            string query = "SELECT * FROM Environment2D";

            return await connection.QueryAsync<Environment2D>(query);
        }

       
        public async Task<Environment2D> GetByIdAsync(Guid id , string userId)
        {
            using var connection = _context.CreateConnection();

            string query = "SELECT * FROM Environment2D WHERE Id = @Id AND OwnerUserId = @UserId";

            return await connection.QueryFirstOrDefaultAsync<Environment2D>(query, new { Id = id  , UserId  = userId });
        }

        public async Task<Environment2D?> GetByUserAndNameAsync(string userId, string name)
        {
            using var connection = _context.CreateConnection();

            string query = "SELECT * FROM Environment2D WHERE Name = @Name AND OwnerUserId = @UserId";

            return await connection.QueryFirstOrDefaultAsync<Environment2D>(query, new { Name = name, UserId = userId });
        }

        public async Task<int> CountByUserAsync(string ownerUserId)
        {
            using var connection = _context.CreateConnection();
            string query = "SELECT COUNT(*) FROM Environment2D WHERE OwnerUserId = @OwnerUserId";

            return await connection.ExecuteScalarAsync<int>(query, new { OwnerUserId = ownerUserId });
        }

        public async Task<int> AddAsync(Environment2D environment)
        {
            using var connection = _context.CreateConnection();
            string query = @" INSERT INTO Environment2D (Id, Name, OwnerUserId, MaxLength, MaxHeight)
            VALUES (@Id, @Name, @OwnerUserId, @MaxLength, @MaxHeight)";

            return await connection.ExecuteAsync(query, environment);
        }
 
        public async Task<int> DeleteAsync(Guid id , string userId)
        {
            using var connection = _context.CreateConnection();

            string query = "DELETE FROM Environment2D WHERE Id = @Id AND OwnerUserId = @UserId";

            return await connection.ExecuteAsync(query, new { Id = id  , UserId = userId });
        }
    }
}
