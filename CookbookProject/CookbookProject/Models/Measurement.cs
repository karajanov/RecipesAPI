namespace CookbookProject.Models
{
    public class Measurement
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public int IngredientId { get; set; }
        
        public string Quantity { get; set; }

        public string Consistency { get; set; }

        public virtual Recipe Recipe { get; set; }

        public virtual Ingredient Ingredient { get; set; }
    }
}
