using CookbookProject.Models;
using CookbookProject.Services.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookProject.Services.Repository
{
    public class CuisineRepository : Repository<Cuisine>, ICuisineRepository
    {
        public CuisineRepository(CookbookProjectContext context)
                : base(context)
        {
        }

        public async Task<IEnumerable<string>> GetAllTitlesAsync()
        {
            return await GetEntity()
                .Select(c => c.Title)
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}
