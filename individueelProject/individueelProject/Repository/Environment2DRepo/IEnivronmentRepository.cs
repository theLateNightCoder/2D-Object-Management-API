using individueelProject.Repository.Models;

namespace individueelProject.Repository.Environment2DRepo
{
    public interface IEnivronmentRepository
    {

        Task<Guid> GetEnvironmentIdAsync(string name, string userId);
        Task<IEnumerable<Environment2D>> GetAllAsync();
        Task<Environment2D> GetByIdAsync(Guid id , string userId);
        Task<IEnumerable<Environment2D>> GetByUserIdAsync(string userId);
        Task<Environment2D?> GetByUserAndNameAsync(string userId, string name);
        Task<int> CountByUserAsync(string ownerUserId);
        Task<int> AddAsync(Environment2D environment);
    
        Task<int> DeleteAsync(Guid id , string userId);
    }
}
