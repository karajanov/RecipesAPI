using CookbookProject.DataTransferObjects;
using CookbookProject.Models;
using System.Threading.Tasks;

namespace CookbookProject.Services.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> IsEmailTakenAsync(string email);

        Task<bool> IsUsernameTakenAsync(string username);

        Task<bool> IsUserValidAsync(Credentials c);

        Task<int> GetIdByUsernameAsync(string username);
    }
}
