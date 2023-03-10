using System.ComponentModel.DataAnnotations;

namespace AddressAPI.DTOs
{
    public class SearchParameters
    {
        [Required]
        public string? SearchText { get; set; }
        public string? SortOrder { get; set; } = "ASC";
        public string? SortColumn { get; set; } 
    }
}
