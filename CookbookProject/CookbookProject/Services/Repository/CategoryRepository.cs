using CookbookProject.Models;
using CookbookProject.Services.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<int> GetIdByTitleAsync(string title)
        {
            var item = await GetEntity()
                .AsNoTracking()
                .Where(ct => ct.Title == title)
                .Select(ct => ct.Id)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return item;
        }

        public async Task<int?> InsertIfNecessaryAsync(string title)
        {
            if (title == null)
                return null;

            var categoryId = await GetIdByTitleAsync(title)
                .ConfigureAwait(false);

            if (categoryId == 0)
            {
                var category = new Category() { Title = title };
                try
                {
                    await InsertAsync(category)
                        .ConfigureAwait(false);
                }
                catch (Exception)
                {
                    return null;
                }

                return category.Id;
            }
            else
            {
                return categoryId;
            }

        }
    }
}
