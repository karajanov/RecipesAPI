using CookbookProject.DataTransferObjects;
using CookbookProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CookbookProject.Services.Repository.Interfaces
{
    public interface IMeasurementRepository : IRepository<Measurement>
    {
        Task<IEnumerable<MeasurementViewModel>> GetRecipeMeasurementsByIdAsync(int recipeId);
    }
}
