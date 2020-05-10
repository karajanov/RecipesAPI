using System.Collections.Generic;
using System.Threading.Tasks;

namespace CookbookProject.Services.Repository.Interfaces
{
    public interface ITitles
    {
        Task<IEnumerable<string>> GetAllTitlesAsync();

        Task<int> GetIdByTitleAsync(string title);

        Task<int?> InsertIfNecessaryAsync(string title);
    }
}
