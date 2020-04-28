using CookbookProject.Models;
using System.Threading.Tasks;

namespace CookbookProject.Services.Repository.Interfaces
{
    public interface IVerificationRepository : IRepository<Verification>
    {
        Task<Verification> GetVerificationByUsernameAsync(string username);
    }
}
