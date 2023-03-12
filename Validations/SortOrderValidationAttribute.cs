using System.ComponentModel.DataAnnotations;

namespace AddressAPI.Validations
{
    public class SortOrderValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var sortColumn = validationContext.ObjectType.GetProperty("SortColumn")!.GetValue(validationContext.ObjectInstance, null) as string;
            if (string.IsNullOrEmpty(sortColumn))
            {
                return new ValidationResult("Sort order cannot be applied without a SortColumn value");
            }

            return ValidationResult.Success;
        }
    }
}
