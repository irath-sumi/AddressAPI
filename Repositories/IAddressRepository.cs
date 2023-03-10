using AddressAPI.Models;

namespace AddressAPI.Repositories
{
    public interface IAddressRepository
    {
        IQueryable<Address> GetAllAddresses();
        Task<Address> GetAddressById(int id);
        Task AddAddress(List<Address> address);
        Task UpdateAddress(int id, Address address);
        Task DeleteAddress(int id);
    }
}
