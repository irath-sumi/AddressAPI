using AddressAPI.DTOs;
using AddressAPI.Models;

namespace AddressAPI.Services
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAllAddresses(SearchParameters searchParameters);
        Task<Address> GetAddressById(int id);
        Task AddAddress(List<Address> address);
        Task UpdateAddress(int id, Address address);
        Task DeleteAddress(int id);
        Task<object> GetDistance(int address1ID, int address2ID);
       
    }

}
