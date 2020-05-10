using CookbookProject.DataTransferObjects;
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

        public async Task<bool> IsUserValidAsync(Credentials c)
        {
            var hashedPassword = hashConverter.GetHashedPassword(c.PlainPassword);

            var user = await GetEntity()
                .Where(u => u.Username == c.Username && u.Password == hashedPassword)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return user != null;
        }

        public async Task<int> GetIdByUsernameAsync(string username)
        {
            var id = await GetEntity()
                .Where(u => u.Username == username)
                .Select(u => u.Id)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return id;
        }
    }
}
