using individueelProject.Repository.Models;

namespace individueelProject.Repository.Object2DRepo
{
    public interface IObjectRepository
    {

        Task<IEnumerable<Object2D>> GetAllAsync();
        Task<IEnumerable<Object2D>> GetByEnvironmentIdAsync(Guid environmentId);

        Task<Object2D> GetByIdAsync(Guid id , Guid environmentId);
        Task<int> AddAsync(Object2D object2D);
       
        Task<int> DeleteAsync(Guid id , Guid environmentId);
    }
}
