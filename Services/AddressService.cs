using AddressAPI.DTOs;
using AddressAPI.Models;
using AddressAPI.Repositories;
using GeoCoordinatePortable;
using OpenCage.Geocode;
using System.Reflection;

namespace AddressAPI.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IConfiguration _configuration;
        public AddressService(IAddressRepository addressRepository, IConfiguration configuration)
        {
            _addressRepository = addressRepository;
            _configuration = configuration;

        }
        /// <summary>
        /// This method retrieves all the addresses from the repository and filters them according to the SearchParameters object passed as a parameter. 
        /// It checks if the query parameter is not null or whitespace, 
        /// and if so, it applies a filter that searches for the query string in any property of 
        /// the Address class that is of type string. It then checks if the sort column 
        /// and sort order parameters are not null or whitespace, and if so, 
        /// it applies the sorting based on the specified column and order. 
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        public async Task<List<Address>> GetAllAddresses(SearchParameters searchParams)
        {
            List<Address> addresses = await _addressRepository.GetAllAddresses(searchParams);

            if (searchParams.SortColumn != null)
            {
                // sorting
                var propertyInfo = typeof(Address).GetProperty(searchParams.SortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    addresses = searchParams.SortOrder != null ? (searchParams.SortOrder.ToLower() == "desc"
                        ? addresses.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList()
                        : addresses.OrderBy(x => propertyInfo.GetValue(x, null)).ToList())
                        : addresses.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                }
            }

            return addresses;
        }
        /// <summary>
        /// This method simply calls the GetAddressById method of the repository 
        /// and returns the result.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<Address?> GetAddressById(int id)
        {
            return await _addressRepository.GetAddressById(id);
        }
        /// <summary>
        /// This method simply calls the AddAddress method of the repository and returns the result.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>

        public async Task AddAddress(List<Address> address)
        {
            await _addressRepository.AddAddress(address);
        }
        /// <summary>
        /// This method simply calls the UpdateAddress method of the repository and returns the result.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public async Task<string> UpdateAddress(int id, Address address)
        {
            return await _addressRepository.UpdateAddress(id, address);
        }
        /// <summary>
        ///  
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        public async Task<Address> DeleteAddress(int id)
        {
            return await _addressRepository.DeleteAddress(id);
        }
        /// <summary>
        ///  This method takes two address IDs as parameters and retrieves the addresses from the repository. 
        ///  It then converts the addresses to strings and uses the OpenCage Geocoder API to retrieve the latitude and longitude coordinates for each address. 
        ///  Once it has the coordinates, it calculates the distance between the two addresses using the GetDistanceTo method of the GeoCoordinate class. 
        ///  One potential improvement here could be to implement caching of the geocoding results to reduce the number of API calls and improve performance.
        /// </summary>
        /// <param name="address1"></param>
        /// <param name="address2"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<object> GetDistance(int address1ID, int address2ID)
        {
            var addressFirst = await _addressRepository.GetAddressById(address1ID);
            var addressLast = await _addressRepository.GetAddressById(address2ID);

            if (addressFirst != null && addressLast != null)
            {
                string addressFirstAsString = addressFirst.ToString();
                string addressLastAsString = addressLast.ToString();
                // Read API key from appsettings 
                var apiKey = _configuration["GeoCodingDevAPIKey"];

                Geocoder _geocoder = new Geocoder(apiKey);
                var responseFirstAddress = _geocoder.Geocode(addressFirstAsString);
                var responseSecondAddress = _geocoder.Geocode(addressLastAsString);

                if (responseFirstAddress.Status.Code != 200 || responseSecondAddress.Status.Code != 200)
                {
                    return default(double);
                }

                var coordinatesFirstAddress = new GeoCoordinate(responseFirstAddress.Results[0].Geometry.Latitude, responseFirstAddress.Results[0].Geometry.Longitude);
                var coordinatesSecondAddress = new GeoCoordinate(responseSecondAddress.Results[0].Geometry.Latitude, responseSecondAddress.Results[0].Geometry.Longitude);

                var DistanceDTO = new DistanceDTO
                {
                    Db_LocationA = addressFirstAsString,
                    Db_LocationB = addressLastAsString,
                    Distance = coordinatesFirstAddress.GetDistanceTo(coordinatesSecondAddress) / 1000.0,
                    Unit_Of_Measure = "KM."
                };
                return DistanceDTO;
            }
            else
            {
                return "NotFoundInDb";
            }

        }
    }

}
