using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CookbookProject.CustomAttributes
{
    public class MaxFileNameLengthAttribute : ValidationAttribute
    {
        private readonly ushort maxLength;

        public MaxFileNameLengthAttribute(ushort maxLength)
        {
            this.maxLength = maxLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value != null)
            {
                if((value as IFormFile).FileName.Length > maxLength)
                {
                    return new ValidationResult($"Max filename length: { maxLength }");
                }
            }

            return ValidationResult.Success;
        }
    }
}
