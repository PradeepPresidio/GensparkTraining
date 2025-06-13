using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace TeamColabApp.Validations
{
    public class ValidNameAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string username)
            {
                if (username.Length < 3)
                {
                    return new ValidationResult("Name must be at least 5 characters long.");
                }

                if (!Regex.IsMatch(username, @"^[A-Za-z].*"))
                {
                    return new ValidationResult("Name must start with a letter.");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid Name format.");
        }
    }
}
