using CookbookProject.Models;
using CookbookProject.Models.Query;
using CookbookProject.Services.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookProject.Services.Repository
{
    public class MeasurementRepository : Repository<Measurement>, IMeasurementRepository
    {
        private readonly DbSet<Ingredient> ingredients;

        public MeasurementRepository(CookbookProjectContext context)
            : base(context)
        {
            ingredients = context.Set<Ingredient>();
        }

        public async Task<IEnumerable<QRecipeMeasurement>> GetRecipeMeasurementsByIdAsync(int recipeId)
        {
            var items = await (from m in GetEntity()
                              join ig in ingredients on m.IngredientId equals ig.Id
                              where m.RecipeId == recipeId
                              select new QRecipeMeasurement()
                              {
                                  Ingredient = ig.Title,
                                  Quantity = m.Quantity,
                                  Consistency = m.Consistency
                              })
                              .ToListAsync()
                              .ConfigureAwait(false);

            return items;
        }
    }
}
