using System.ComponentModel.DataAnnotations;

namespace CookbookProject.DataTransferObjects
{
    public class VerificationResult
    {
        [Required]
        public UserViewModel UserViewModel { get; set; }

        [Required]
        public string VerificationCode { get; set; }
    }
}
