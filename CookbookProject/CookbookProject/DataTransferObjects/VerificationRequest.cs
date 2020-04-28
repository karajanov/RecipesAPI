using System.ComponentModel.DataAnnotations;

namespace CookbookProject.DataTransferObjects
{
    public class VerificationRequest
    {
        [Required]
        public UserViewModel UserViewModel { get; set; }

        [Required]
        public string VerificationCode { get; set; }
    }
}
