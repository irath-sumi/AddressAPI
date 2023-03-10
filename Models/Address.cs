using System.ComponentModel.DataAnnotations;

namespace AddressAPI.Models
{
    // All fields are manadatory and it cannot hold null 
    public class Address
    {
        [Key]
        public int Id { get; set; }
        [Required]
     //   [MaxLength(50)] // was getting SQLite Error 1: 'near "max": syntax error' when create table script was run, hence maxlength 
        public string Street { get; set; } = null!;
        [Required]
     //   [MaxLength(50)]
        public string HouseNumber { get; set; } = null!;
        [Required]
     //   [MaxLength(50)]
        public string ZipCode { get; set; } = null!;
        [Required]
     //   [MaxLength(50)]
        public string City { get; set; } = null!;
        [Required]
     //   [MaxLength(50)]
        public string Country { get; set; } = null!;

        // overriden ToString to get a custom string representation of the Address object.
        //useful when calling GeoCode as it expect string address.
        public override string ToString()
        {
            return $"{Street},{HouseNumber},{ZipCode},{City},{Country}";
        }
    }

}
