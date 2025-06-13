using System;
using System.ComponentModel.DataAnnotations;

namespace TeamColabApp.Validations
{
    public class DateAfter2000Attribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not DateTime date)
            {
                return new ValidationResult("Invalid date format.");
            }

            var minDate = new DateTime(2000, 1, 1);

            if (date < minDate)
            {
                return new ValidationResult("Date must be on or after January 1st, 2000.");
            }

            return ValidationResult.Success;
        }
    }
}