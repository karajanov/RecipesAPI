using CookbookProject.Models;
using CookbookProject.Services.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookProject.Services.Repository
{
    public class IngredientRepository : Repository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(CookbookProjectContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<string>> GetAllTitlesAsync()
        {
            var items = await GetEntity()
                .Select(ing => ing.Title)
                .ToListAsync()
                .ConfigureAwait(false);

            return items;
        }

        public async Task<int> GetIdByTitleAsync(string title)
        {
            var item = await GetEntity()
                .AsNoTracking()
                .Where(ing => ing.Title == title)
                .Select(ing => ing.Id)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return item;
        }

        public async Task<int?> InsertIfNecessaryAsync(string title)
        {
            if (title == null)
                return null;

            var ingredientId = await GetIdByTitleAsync(title)
                .ConfigureAwait(false);

            if (ingredientId == 0)
            {
                var ingredient = new Ingredient() { Title = title };
                try
                {
                    await InsertAsync(ingredient)
                        .ConfigureAwait(false);
                }
                catch (Exception)
                {
                    return null;
                }

                return ingredient.Id;
            }
            else
            {
                return ingredientId;
            }
        }
    }
}
