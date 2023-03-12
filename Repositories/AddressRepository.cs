using AddressAPI.Data;
using AddressAPI.DTOs;
using AddressAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace AddressAPI.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AddressAPIContext _context;
        public AddressRepository(AddressAPIContext context)
        {
            _context = context;
        }

        /// <summary>        
        /// This is a method that searches for addresses based on a given search parameter. 
        /// If the search parameter is null, it returns the first 30 addresses. Otherwise, 
        /// it uses reflection to dynamically build an expression tree that represents 
        /// the search condition, and applies it as a filter on the Addresses DbSet of a DbContext instance. 
        /// The method returns a list of addresses that match the search condition.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Address>> GetAllAddresses(SearchParameters searchParameters)
        {
            if (searchParameters.SearchText == null)
            {
                return await _context.Addresses.Take(30).ToListAsync();
            }

            var parameterExpression = Expression.Parameter(typeof(Address), "a");
            var filterExpressions = new List<Expression>();
            foreach (var property in typeof(Address).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.PropertyType != typeof(string))
                {
                    continue;
                }

                var propertyAccess = Expression.Property(parameterExpression, property);
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var searchExpression = Expression.Constant(searchParameters.SearchText, typeof(string));
                var containsExpression = Expression.Call(propertyAccess, containsMethod, searchExpression);
                var nullCheckExpression = Expression.NotEqual(propertyAccess, Expression.Constant(null, typeof(string)));
                var combinedExpression = Expression.AndAlso(nullCheckExpression, containsExpression);
                filterExpressions.Add(combinedExpression);
            }

            if (filterExpressions.Count > 0)
            {
                var orExpression = filterExpressions[0];
                for (int i = 1; i < filterExpressions.Count; i++)
                {
                    orExpression = Expression.OrElse(orExpression, filterExpressions[i]);
                }
                var lambdaExpression = Expression.Lambda<Func<Address, bool>>(orExpression, parameterExpression);

                return await _context.Addresses.Where(lambdaExpression).ToListAsync();
            }
            else
            {
                return await _context.Addresses.Take(30).ToListAsync();
            }         

        }
        /// <summary>
        /// This method retrieves a single address from the database based on the given id. 
        /// It uses the FindAsync method to retrieve the address asynchronously. 
        /// Async/await is used here since the query involves an I/O operation.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<Address?> GetAddressById(int id)
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
            var addresses = address.Select(dto => new Address
            {
                Street = dto.Street,
                HouseNumber = dto.HouseNumber,
                ZipCode = dto.ZipCode,
                City = dto.City,
                Country = dto.Country,

            }).ToList();
            _context.Addresses.AddRange(addresses);
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

        public async Task<string> UpdateAddress(int id, Address address)
        {
            // Since Include function not needed here, FindAsync is faster and better.
            // if related data is needed(include), FirstOrDefaultAsync is the better choice.
            var existingAddress = await _context.Addresses.FindAsync(id);
            if (existingAddress != null)
            {
                existingAddress.Id = id;
                existingAddress.Street = address.Street;
                existingAddress.HouseNumber = address.HouseNumber;
                existingAddress.ZipCode = address.ZipCode;
                existingAddress.City = address.City;
                existingAddress.Country = address.Country;
                await _context.SaveChangesAsync();
                return "Address successfully updated";
            }
            return "Address not found in database";
        }
        /// <summary>
        /// This method deletes an existing address from the database based on the given id.
        /// This method first checks if an address with the specified ID exists in the database by calling the GetAddressById method of the repository. 
        /// If the address exists, removes it using _context.Addresses.
        /// and then saves the changes to the database using SaveChangesAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<Address> DeleteAddress(int id)
        {
            var address = await GetAddressById(id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
            return address!;
        }
    }

}
