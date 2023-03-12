using Microsoft.EntityFrameworkCore;
using AddressAPI.Models;

namespace AddressAPI.Data
{
    public class AddressAPIContext : DbContext
    {
        public AddressAPIContext(DbContextOptions<AddressAPIContext> options)
             : base(options)
        { }
        /*  tells the compiler that the property Addresses may be null, 
        but the code guarantees that it will not be null at runtime. 
        Written this to eliminate null reference warning */
        public DbSet<Address> Addresses { get; set; } = default!;
    }
}
