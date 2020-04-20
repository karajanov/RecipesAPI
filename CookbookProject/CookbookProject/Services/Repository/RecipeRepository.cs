using CookbookProject.Models;
using CookbookProject.Models.Query;
using CookbookProject.Services.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookProject.Services.Repository
{
    public class RecipeRepository : Repository<Recipe>, IRecipeRepository
    {
        private readonly DbSet<User> users;
        private readonly DbSet<Category> categories;
        private readonly DbSet<Cuisine> cuisine;

        public RecipeRepository(CookbookProjectContext context)
            : base(context)
        {
            users = context.Set<User>();
            categories = context.Set<Category>();
            cuisine = context.Set<Cuisine>();
        }

        public async Task<IEnumerable<QRecipePreview>> GetRecipePreviewByCategoryAsync(string categoryTitle)
        {
            var items = await (from r in GetEntity()
                               join u in users on r.UserId equals u.Id
                               join ct in categories on r.CategoryId equals ct.Id
                               where ct.Title == categoryTitle
                               select new QRecipePreview()
                               {
                                   Id = r.Id,
                                   Title = r.Title,
                                   ImagePath = r.ImagePath,
                                   Author = u.Username
                               })
                               .ToListAsync()
                               .ConfigureAwait(false);

            return items;
        }

        public async Task<IEnumerable<QRecipePreview>> GetRecipePreviewByCuisineAsync(string cuisineTitle)
        {
            var items = await (from r in GetEntity()
                               join u in users on r.UserId equals u.Id
                               join cu in cuisine on r.CuisineId equals cu.Id
                               where cu.Title == cuisineTitle
                               select new QRecipePreview()
                               {
                                   Id = r.Id,
                                   Title = r.Title,
                                   ImagePath = r.ImagePath,
                                   Author = u.Username
                               })
                               .ToListAsync()
                               .ConfigureAwait(false);

            return items;
        }
    }
}
