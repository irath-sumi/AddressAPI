using System.ComponentModel.DataAnnotations;

namespace AddressAPI.Models
{
    // All fields are manadatory and it cannot hold null. Written this to remove
    //possible null reference warning. Handled in the code to ensure these values are never null
    public class Address
    {
        [Key]
        public int Id { get; set; }

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

        // overriden ToString to get a custom string representation of the Address object.
        //useful when calling GeoCode as it expect string address.
        public override string ToString()
        {
            return $"{Street},{HouseNumber},{ZipCode},{City},{Country}";
        }
    }

}
