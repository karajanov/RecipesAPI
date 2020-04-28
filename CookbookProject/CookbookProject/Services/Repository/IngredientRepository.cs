using CookbookProject.Models;
using CookbookProject.Services.Repository.Interfaces;

namespace CookbookProject.Services.Repository
{
    public class IngredientRepository : Repository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(CookbookProjectContext context)
            : base(context)
        {

        }
    }
}
