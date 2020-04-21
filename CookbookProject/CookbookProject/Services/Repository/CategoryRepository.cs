using CookbookProject.Models;
using CookbookProject.Services.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookProject.Services.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(CookbookProjectContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<string>> GetAllTitlesAsync()
        {
            return await GetEntity()
                .Select(ct => ct.Title)
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}
