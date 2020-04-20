using CookbookProject.Models;
using CookbookProject.Services.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
