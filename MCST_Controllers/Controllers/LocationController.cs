using Microsoft.AspNetCore.Mvc;
using MCST_Location.Domain;
using Amazon.Auth.AccessControlPolicy;
using Microsoft.AspNetCore.Authorization;
using MCST_Models;

namespace MCST_Controller.Controllers
{
    [ApiController]
    [Route("tracker/location")]
    public class LocationController : ControllerBase
    {
        private readonly LocationService service;

        public LocationController(LocationService service)
        {
            this.service = service;
        }

        [HttpGet("get/by-id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Location>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> GetLocationById([FromQuery]int computerId)
        {
            Location location = await service.GetLocationByComputerId(computerId);
            return location == null ? NotFound() : Ok(location);
        }

        
    }
}

