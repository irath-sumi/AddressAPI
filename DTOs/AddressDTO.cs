using System.ComponentModel.DataAnnotations;

namespace AddressAPI.DTOs
{
    public class AddressDTO
    {
        [Required]
        public string Street { get; set; } = null!;

        [Required]
        public string HouseNumber { get; set; } = null!;

        [Required]
        public string ZipCode { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;
    }
}
