using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CookbookProject.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        // GET: api/Ingredient
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
