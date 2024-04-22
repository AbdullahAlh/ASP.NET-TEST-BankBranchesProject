using BankBranches.Models;
using System.ComponentModel.DataAnnotations;

public class ValidCivilIdAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string civilId = value as string;

        if (string.IsNullOrEmpty(civilId))
        {
            return new ValidationResult("Civil ID is required.");
        }

       
        var context = (BankContext)validationContext.GetService(typeof(BankContext));
        bool exists = context.Employees.Any(e => e.CivilId == civilId);

        if (exists)
        {
            return new ValidationResult("Duplicate CivilId.");
        }

        return ValidationResult.Success;
    }
}
