using System.Collections.Generic;
using System.Threading.Tasks;
using CookbookProject.Models.Query;
using CookbookProject.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CookbookProject.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository recipeRepository;

        public RecipeController(IRecipeRepository recipeRepository)
        {
            this.recipeRepository = recipeRepository;
        }

        [HttpGet]
        [Route("Category")] // api/Recipe/Category?item=value
        public async Task<IEnumerable<QRecipePreview>> GetRecipePreviewByCategoryAsync([FromQuery] string item)
        {
            return await recipeRepository.GetRecipePreviewByCategoryAsync(item).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("Cuisine")] // api/Recipe/Cuisine?item=value
        public async Task<IEnumerable<QRecipePreview>> GetRecipePreviewByCuisineAsync([FromQuery] string item)
        {
            return await recipeRepository.GetRecipePreviewByCuisineAsync(item).ConfigureAwait(false);
        }
    }
}
