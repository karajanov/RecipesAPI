using CookbookProject.CustomAttributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CookbookProject.DataTransferObjects
{
    public class RecipeViewModel
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(800, MinimumLength = 15)]
        public string Instructions { get; set; }

        [Required]
        public string Username { get; set; }

        [StringLength(15, MinimumLength = 3)]
        public string PrepTime { get; set; }

        [StringLength(25, MinimumLength = 3)]
        public string CuisineTitle { get; set; }

        [StringLength(25)]
        public string CategoryTitle { get; set; }

        [MaxFileSize(1572864)] // 1.5 MB
        [MaxFileNameLength(30)]
        public IFormFile Image { get; set; }
    }
}
