using Microsoft.EntityFrameworkCore;
using AddressAPI.Models;

namespace AddressAPI.Data
{
    public class AddressAPIContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public AddressAPIContext(DbContextOptions<AddressAPIContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = _configuration["ConnectionStrings:AddressAPIContext"];
            // connect to sqlite database
            options.UseSqlite(connectionString);
                //@"Data Source=./SQLiteDatabaseFile/AddressAPI.db;");
        }
        public DbSet<Address> Addresses { get; set; } = default!;
    }
}
