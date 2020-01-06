using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCart.Infrastructure
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        //validationContext is used for (db context)services if you need to
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //TODO var context = (ShopCartContext)validationContext.GetService(typeof(ShopCartContext)); used in case of services needed

            //value.ToString() to validate the string??

            var file = value as IFormFile;

            if(file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                string[] extensions = { "jpg", "png" };
                bool result = extension.Any(x => extension.EndsWith(x));

                if (!result)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return "Allowed extensions are .jpg and .png"; 
        }
    }
}
