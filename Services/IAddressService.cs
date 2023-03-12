using AddressAPI.DTOs;
using AddressAPI.Models;

namespace AddressAPI.Services
{
    public interface IAddressService
    {
        Task<List<Address>> GetAllAddresses(SearchParameters searchParameters);
        Task<Address?> GetAddressById(int id);
        Task AddAddress(List<Address> address);
        Task<string> UpdateAddress(int id, Address address);
        Task<Address> DeleteAddress(int id);
        Task<object> GetDistance(int address1ID, int address2ID);
       
    }

}
