using System.Threading.Tasks;

namespace CookbookProject.Services.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(object id);

        Task InsertAsync(T obj);

        Task UpdateAsync(T obj);

        Task DeleteAsync(object id);

        Task SaveAsync();
    }
}
