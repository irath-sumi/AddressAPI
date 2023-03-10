using AddressAPI.Data;
using AddressAPI.Models;

namespace AddressAPI.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AddressAPIContext _context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public AddressRepository(AddressAPIContext context)
        {
            _context = context;
        }

        /// <summary>        
        /// No, async/await is not required for the IQueryable<Address> 
        ///object returned by the GetAllAddresses method. async/await is typically used for asynchronous operations that involve I/O or other long-running tasks that can benefit from non-blocking executio
        /// </summary>
        /// <returns></returns>
        public IQueryable<Address> GetAllAddresses()
        {
            return _context.Addresses.AsQueryable();
        }
        /// <summary>
        /// This method retrieves a single address from the database based on the given id. 
        /// It uses the FindAsync method to retrieve the address asynchronously. 
        /// Async/await is used here since the query involves an I/O operation.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<Address> GetAddressById(int id)
        {
            return await _context.Addresses.FindAsync(id);

        }
        /// <summary>
        /// This method adds one or more addresses to the database. 
        /// It uses AddRangeAsync to add the addresses asynchronously, followed by SaveChangesAsync to commit the changes to the database. 
        /// Async/await is used here since the database operations involve I/O
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>

        public async Task AddAddress(List<Address> address)
        {
            await _context.Addresses.AddRangeAsync(address);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// This method updates an existing address in the database based on the given id. 
        /// It first retrieves the existing address using FindAsync, updates its properties,
        /// and then saves the changes to the database using SaveChangesAsync.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <returns></returns>

        public async Task UpdateAddress(int id, Address address)
        {
            // Since Include function not needed here, FindAsync is faster and better.
            // if related data is needed(include), FirstOrDefaultAsync is the better choice.
            var existingAddress = await _context.Addresses.FindAsync(id);
            if (existingAddress != null)
            {
                existingAddress.Id = address.Id;
                existingAddress.Street = address.Street;
                existingAddress.HouseNumber = address.HouseNumber;
                existingAddress.ZipCode = address.ZipCode;
                existingAddress.City = address.City;
                existingAddress.Country = address.Country;
                await _context.SaveChangesAsync();
            }
        }
        /// <summary>
        /// This method deletes an existing address from the database based on the given id. 
        /// It first retrieves the existing address using FindAsync, removes it using _context.Addresses.
        /// Remove, and then saves the changes to the database using SaveChangesAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task DeleteAddress(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
        }
    }

}
