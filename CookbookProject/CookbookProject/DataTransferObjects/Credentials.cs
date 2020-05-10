using System.ComponentModel.DataAnnotations;

namespace CookbookProject.DataTransferObjects
{
    public class Credentials
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string PlainPassword { get; set; }
    }
}
