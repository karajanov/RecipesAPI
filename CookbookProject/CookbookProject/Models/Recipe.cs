using System.Collections.Generic;

namespace CookbookProject.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Instructions { get; set; }

        public string PrepTime { get; set; }

        public int? CuisineId { get; set; }

        public int? CategoryId { get; set; }

        public int UserId { get; set; }

        public string ImagePath { get; set; }

        public virtual User User { get; set; }

        public virtual Cuisine Cuisine { get; set; }

        public virtual Category Category { get; set; }

        public virtual IEnumerable<Measurement> Ingredients { get; set; }
    }
}
