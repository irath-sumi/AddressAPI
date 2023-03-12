using AddressAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace AddressAPI.DTOs
{
    public class SearchParameters
    {
        public string? SearchText { get; set; }

        [RegularExpression("^(asc|desc)$")]
        [SortOrderValidation]
        public string? SortOrder { get; set; }
        [SortColumnValidation]
        public string? SortColumn { get; set; }
    }
}
