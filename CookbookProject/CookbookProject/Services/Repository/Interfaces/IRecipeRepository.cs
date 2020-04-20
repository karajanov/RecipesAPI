using CookbookProject.Models;
using CookbookProject.Models.Query;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CookbookProject.Services.Repository.Interfaces
{
    public interface IRecipeRepository : IRepository<Recipe>
    {
        Task<IEnumerable<QRecipePreview>> GetRecipePreviewByCategoryAsync(string categoryTitle);

        Task<IEnumerable<QRecipePreview>> GetRecipePreviewByCuisineAsync(string cuisineTitle);
    }
}
