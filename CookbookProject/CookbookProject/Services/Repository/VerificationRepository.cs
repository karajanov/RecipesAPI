using CookbookProject.Models;
using CookbookProject.Services.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookProject.Services.Repository
{
    public class VerificationRepository : Repository<Verification>, IVerificationRepository
    {
        public VerificationRepository(CookbookProjectContext context)
            : base(context)
        {
        }

        public async Task<Verification> GetVerificationByUsernameAsync(string username)
        {
            var item = await GetEntity()
                .Where(v => v.Username == username)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return item;
        }
    }
}
