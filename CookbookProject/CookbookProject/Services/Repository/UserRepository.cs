using CookbookProject.Models;
using CookbookProject.Services.Repository.Interfaces;
using CookbookProject.Services.Security;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookProject.Services.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IHashConverter hashConverter;

        public UserRepository(CookbookProjectContext context, IHashConverter hashConverter)
            : base(context)
        {
            this.hashConverter = hashConverter;
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            var isTaken = await GetEntity()
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return isTaken != null;
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            var isTaken = await GetEntity()
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return isTaken != null;
        }

        public async Task<bool> IsUserValidAsync(string username, string password)
        {
            var hashedPassword = hashConverter.GetHashedPassword(password);

            var user = await GetEntity()
                .Where(u => u.Username == username && u.Password == hashedPassword)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return user != null;
        }
    }
}
