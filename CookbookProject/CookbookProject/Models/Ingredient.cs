using System.Collections.Generic;

namespace CookbookProject.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public virtual IEnumerable<Measurement> Recipes { get; set; }
    }
}
