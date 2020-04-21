using CookbookProject.Models;
using CookbookProject.Models.Query;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CookbookProject.Services.Repository.Interfaces
{
    public interface IMeasurementRepository : IRepository<Measurement>
    {
        Task<IEnumerable<QRecipeMeasurement>> GetRecipeMeasurementsByIdAsync(int recipeId);
    }
}
