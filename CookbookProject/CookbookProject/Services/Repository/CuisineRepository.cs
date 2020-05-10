using CookbookProject.Models;
using CookbookProject.Services.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<int> GetIdByTitleAsync(string title)
        {
            var item = await GetEntity()
                .AsNoTracking()
                .Where(cu => cu.Title == title)
                .Select(cu => cu.Id)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return item;
        }

        public async Task<int?> InsertIfNecessaryAsync(string title)
        {
            if (title == null)
                return null;

            var cuisineId = await GetIdByTitleAsync(title)
                .ConfigureAwait(false);

            if (cuisineId == 0)
            {
                var cuisine = new Cuisine() { Title = title };
                try
                {
                    await InsertAsync(cuisine)
                        .ConfigureAwait(false);
                }
                catch (Exception)
                {
                    return null;
                }

                return cuisine.Id;
            }
            else
            {
                return cuisineId;
            }
        }
    }
}
