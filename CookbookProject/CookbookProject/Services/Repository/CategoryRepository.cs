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
        private readonly DbSet<Recipe> recipes;

        public CategoryRepository(CookbookProjectContext context)
            : base(context)
        {
            recipes = context.Set<Recipe>();
        }

        public async Task<IEnumerable<string>> GetAllTitlesAsync()
        {
            return await GetEntity()
                .Select(ct => ct.Title)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<string> GetTitleByRecipeIdAsync(int recipeId)
        {
            return await (from ct in GetEntity()
                          join r in recipes on ct.Id equals r.CategoryId
                          where r.Id == recipeId
                          select ct.Title)
                          .AsNoTracking()
                          .FirstOrDefaultAsync()
                          .ConfigureAwait(false);
        }
    }
}
