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
            return await recipeRepository
                .GetRecipePreviewByCategoryAsync(item)
                .ConfigureAwait(false);
        }

        [HttpGet]
        [Route("Cuisine")] // api/Recipe/Cuisine?item=value
        public async Task<IEnumerable<QRecipePreview>> GetRecipePreviewByCuisineAsync([FromQuery] string item)
        {
            return await recipeRepository
                .GetRecipePreviewByCuisineAsync(item)
                .ConfigureAwait(false);
        }

        [HttpGet]
        [Route("Details/{id}")] // api/Recipe/Details/{id}
        public async Task<QRecipeDetails> GetRecipeDetailsByIdAsync([FromRoute] int id)
        {
            return await recipeRepository
                .GetRecipeDetailsByIdAsync(id)
                .ConfigureAwait(false);
        }

        [HttpGet]
        [Route("Exact")] // api/Recipe/Exact?title=value
        public async Task<IEnumerable<QRecipePreview>> GetRecipePreviewByExactTitleAsync([FromQuery] string title)
        {
            return await recipeRepository
                .GetRecipePreviewByExactTitleAsync(title)
                .ConfigureAwait(false);
        }

        [HttpGet]
        [Route("Contains")] // api/Recipe/Contains?key=value
        public async Task<IEnumerable<QRecipePreview>> GetRecipePreviewThatContainsKeyAsync([FromQuery] string key)
        {
            return await recipeRepository
                .GetRecipePreviewThatContainsKeyAsync(key)
                .ConfigureAwait(false);
        }

        [HttpGet]
        [Route("Starts")] // api/Recipe/Starts?key=value
        public async Task<IEnumerable<QRecipePreview>> GetRecipePreviewThatStartsWithKeyAsync([FromQuery] string key)
        {
            return await recipeRepository
                .GetRecipePreviewThatStartsWithKeyAsync(key)
                .ConfigureAwait(false);
        }
    }
}
