using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class ValidCivilId : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Civil ID is required.");
        }

        var civilId = value.ToString();
        if (Regex.IsMatch(civilId, @"^\d{12}$"))
        {
            return ValidationResult.Success;

        }

        return new ValidationResult("Civil ID must be exactly 12 digits long.");

    }
}
