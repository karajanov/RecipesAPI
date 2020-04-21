using System.Collections.Generic;
using System.Threading.Tasks;
using CookbookProject.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CookbookProject.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpGet] 
        [Route("All")] // api/Category/All
        public async Task<IEnumerable<string>> GetAllCategoryTitlesAsync()
        {
            return await categoryRepository
                .GetAllTitlesAsync()
                .ConfigureAwait(false);
        }
    }
}
