using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CookbookProject.CustomAttributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly long maxSize;

        public MaxFileSizeAttribute(long maxSize)
        {
            this.maxSize = maxSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value != null)
            {
                if((value as IFormFile).Length > maxSize)
                {
                    return new ValidationResult($"Max Size: { maxSize } bytes");
                }
                    
            }

            return ValidationResult.Success;
        }
    }
}
