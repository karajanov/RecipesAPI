using System.ComponentModel.DataAnnotations;

namespace CookbookProject.DataTransferObjects
{
    public class MeasurementViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string IngredientTitle { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string Quantity { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string Consistency { get; set; }
    }
}
