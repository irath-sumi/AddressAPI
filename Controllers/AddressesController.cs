using Microsoft.AspNetCore.Mvc;
using AddressAPI.Models;
using AddressAPI.Services;
using AddressAPI.DTOs;

namespace AddressAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        /// <summary>
        /// Retrieve a specific address by its ID
        /// </summary>
        /// <param name="id">The ID of the address to retrieve</param>
        /// <returns>The address if found, otherwise a 404 Not Found response</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddress(int id)
        {
            try
            {
                var address = await _addressService.GetAddressById(id);
                if (address == null)
                {
                    return NotFound();
                }

                return Ok(address);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        /// <summary>
        /// Retrieve a list of addresses that match the given search parameters
        /// </summary>
        /// <param name="searchParams">The search parameters to filter the addresses</param>
        /// <returns>A list of filtered addresses, or a 404 Not Found response if none are found</returns>
        [HttpGet("searchParams")]
        public async Task<ActionResult<Address>> GetAddresses([FromQuery] SearchParameters searchParams)
        {
            try
            {
                var filteredAddresses = await _addressService.GetAllAddresses(searchParams);
                if (filteredAddresses == null || filteredAddresses.Any() == false)
                {
                    return NotFound();
                }

                return Ok(filteredAddresses);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        /// <summary>
        /// Add a new address to the database
        /// </summary>
        /// <param name="address">The new address to add</param>
        /// <returns>A 201 Created response with the added address in the response body</returns>
        [HttpPost("AddNewAddress")]
        public async Task<IActionResult> PostAddress(List<Address> address)
        {
            try
            {
                await _addressService.AddAddress(address);

                return CreatedAtAction(nameof(PostAddress), address);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error adding new address");
            }
        }

        /// <summary>
        /// Update an existing address in the database
        /// </summary>
        /// <param name="id">The ID of the address to update</param>
        /// <param name="address">The updated address information</param>
        /// <returns>A 204 No Content response if successful, or a 400 Bad Request response if the ID does not match the address</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(int id, Address address)
        {
            try
            {
                if (id != address.Id)
                {
                    return BadRequest("Address ID mismatch");
                }

                var addressUpdateResult = await _addressService.UpdateAddress(id, address);

                return CreatedAtAction("GetAddress", "Addresses", new { id = address.Id }, addressUpdateResult);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error updating data");
            }
        }

        /// <summary>
        /// Delete an address from the database
        /// </summary>
        /// <param name="id">The ID of the address to delete</param>
        /// <returns>A 204 No Content response if successful</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {
                var addressToDelete = await _addressService.DeleteAddress(id);
                if (addressToDelete == null)
                {
                    return NotFound($"Address with Id = {id} is not found");
                }
                return Content("Address removed");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error deleting data");
            }
        }

        /// <summary>
        /// Calculate the distance between two addresses using their IDs
        /// </summary>
        /// <param name="address1ID">The ID of the first address</param>
        /// <param name="address2ID">The ID of the second address</param>
        /// <returns>The distance between the addresses in kilometers, or a 400 Bad Request or 404 Not Found response if the addresses are invalid or not in the database</returns>
        [HttpGet("distance")]
        public async Task<IActionResult> GetDistance(int address1ID, int address2ID)
        {
            var distance = await _addressService.GetDistance(address1ID, address2ID);
            if (distance.Equals(default(double)))
            {
                return BadRequest("Not a localtion!!Invalid addresses provided");
            }
            else if (distance.Equals("NotFoundInDb"))
            {
                return NotFound("Address not in database");
            }

            return Ok(distance);

        }
    }

}
