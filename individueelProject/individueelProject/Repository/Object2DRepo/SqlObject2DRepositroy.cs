using Dapper;
using individueelProject.Data;
using individueelProject.Repository.Models;

namespace individueelProject.Repository.Object2DRepo
{
    public class SqlObject2DRepository : IObjectRepository
    {
        private readonly DapperDbContext _context;

        public SqlObject2DRepository(DapperDbContext context)
        {
            _context = context;
        }

      
        public async Task<IEnumerable<Object2D>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();

            string query = "SELECT * FROM Object2D";

            return await connection.QueryAsync<Object2D>(query);
        }

        public async Task<IEnumerable<Object2D>> GetByEnvironmentIdAsync(Guid environmentId)
        {
            using var connection = _context.CreateConnection();

            string query = "SELECT * FROM Object2D WHERE EnvironmentId = @EnvironmentId";

            return await connection.QueryAsync<Object2D>(query, new { EnvironmentId = environmentId });
        }

       
        public async Task<Object2D> GetByIdAsync(Guid id , Guid environmentId )
        {

            using var connection = _context.CreateConnection();

            string query = "SELECT * FROM Object2D WHERE Id = @Id AND EnvironmentId = @EnvironmentId";

            return await connection.QueryFirstOrDefaultAsync<Object2D>(query, new { Id = id , EnvironmentId  = environmentId });
        }

      
        public async Task<int> AddAsync(Object2D object2D)
        {
            using var connection = _context.CreateConnection();
            string query = @"
            INSERT INTO Object2D (Id, EnvironmentId, PrefabId, PostionX, PostionY, ScaleX, ScaleY, RotationZ, SortingLayer)
            VALUES (@Id, @EnvironmentId, @PrefabId, @PostionX, @PostionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer)";

            return await connection.ExecuteAsync(query, object2D);
        }

   
     
        public async Task<int> DeleteAsync(Guid id, Guid environmentId)
        {
            using var connection = _context.CreateConnection();

            string query = "DELETE FROM Object2D WHERE Id = @Id AND EnvironmentId = @EnvironmentId";

            return await connection.ExecuteAsync(query, new { Id = id , EnvironmentId = environmentId });
        }
    }
}
