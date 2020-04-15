using System.Collections.Generic;

namespace CookbookProject.Models
{
    public class Cuisine
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public virtual IEnumerable<Recipe> Recipes { get; set; }
    }
}
