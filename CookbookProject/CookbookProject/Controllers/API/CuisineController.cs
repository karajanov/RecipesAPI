using System.Collections.Generic;
using System.Threading.Tasks;
using CookbookProject.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CookbookProject.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuisineController : ControllerBase
    {
        private readonly ICuisineRepository cuisineRepository;

        public CuisineController(ICuisineRepository cuisineRepository)
        {
            this.cuisineRepository = cuisineRepository;
        }
            
        [HttpGet]
        [Route("All")] // api/Cuisine/All
        public async Task<IEnumerable<string>> GetAllCuisineTitlesAsync()
        {
            return await cuisineRepository
                .GetAllTitlesAsync()
                .ConfigureAwait(false);
        }
    }
}
