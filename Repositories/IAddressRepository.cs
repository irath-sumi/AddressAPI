using AddressAPI.DTOs;
using AddressAPI.Models;

namespace AddressAPI.Repositories
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetAllAddresses(SearchParameters searchParameters);
        Task<Address?> GetAddressById(int id);
        Task AddAddress(List<Address> address);
        Task<string> UpdateAddress(int id, Address address);
        Task<Address> DeleteAddress(int id);
    }
}
