using System.ComponentModel.DataAnnotations;

namespace CookbookProject.DataTransferObjects
{
    public class UserViewModel
    {
        [Required]
        [StringLength(32)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        public string PlainPassword { get; set; }
    }
}
