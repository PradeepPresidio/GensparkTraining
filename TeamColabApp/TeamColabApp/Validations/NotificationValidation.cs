using System;
using System.ComponentModel.DataAnnotations;
namespace TeamColabApp.Validations
{
    public class ValidNotificationMessageAttribute : ValidationAttribute
    {
        private const int MinLength = 3;
        private const int MaxLength = 250;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string message || string.IsNullOrWhiteSpace(message))
            {
                return new ValidationResult("Notification message cannot be empty.");
            }

            if (message.Length < MinLength)
            {
                return new ValidationResult($"Notification message must be at least {MinLength} characters long.");
            }

            if (message.Length > MaxLength)
            {
                return new ValidationResult($"Notification message must not exceed {MaxLength} characters.");
            }

            return ValidationResult.Success;
        }
    }
}