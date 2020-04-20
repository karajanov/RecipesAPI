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
        private readonly DbSet<Recipe> recipes;

        public CuisineRepository(CookbookProjectContext context)
                : base(context)
        {
            recipes = context.Set<Recipe>();
        }

        public async Task<IEnumerable<string>> GetAllTitlesAsync()
        {
            return await GetEntity()
                .Select(c => c.Title)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<string> GetTitleByRecipeIdAsync(int recipeId)
        {
            return await (from c in GetEntity()
                          join r in recipes on c.Id equals r.CuisineId
                          where r.Id == recipeId
                          select c.Title)
                          .AsNoTracking()
                          .FirstOrDefaultAsync()
                          .ConfigureAwait(false);
        }
    }
}
