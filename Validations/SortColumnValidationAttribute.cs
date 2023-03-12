using AddressAPI.DTOs;
using AddressAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AddressAPI.Validations
{
    public class SortColumnValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            var addressSortField = validationContext.ObjectInstance as SearchParameters;
            if (addressSortField == null)
            {
                return new ValidationResult("ValidationContext does not contain an Address instance.");
            }

            var sortColumn = addressSortField.SortColumn;
            if (sortColumn != null)
            {
                var propertyInfo = typeof(Address).GetProperty(sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                {
                    return new ValidationResult($"Invalid sort column '{sortColumn}'.");
                }
            }


            return ValidationResult.Success;
        }
    }
}
