using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HR_Project.Application.Extensions
{
    public class PictureFileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            IFormFile file = (IFormFile)value;

            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName).ToLower(); //.JPEG => .jpeg

                string[] extensions = { "jpg", "jpeg", "png" };

                bool result = extensions.Any(x => x.EndsWith(extension));

                if (!result)
                {
                    return new ValidationResult("Valid format is 'jpg', 'jpeg', 'png'");
                }

            }

            return ValidationResult.Success;
        }
    }
}
