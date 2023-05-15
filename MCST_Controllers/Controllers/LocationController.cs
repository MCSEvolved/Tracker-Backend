using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MCST_Location.Domain;
using MCST_Location.Domain.Models;
using MCST_Location;
using Microsoft.AspNetCore.SignalR;
using MCST_Controller.SignalRHubs;

namespace MCST_Controller.Controllers
{
    [ApiController]
    [Route("api/location")]
    public class LocationController : ControllerBase, ILocationController
    {
        private readonly LocationService service;
        private readonly IHubContext<ClientHub> hub;

        public LocationController(LocationService service, IHubContext<ClientHub> hub)
        {
            this.service = service;
            this.hub = hub;
        }

        [HttpGet("get/by-id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Location>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetLocationById([FromQuery]int computerId)
        {
            Location location = service.GetLocationByComputerId(computerId);
            return location == null ? NotFound() : Ok(location);
        }

        public async void NewLocationOverWS(Location location)
        {
            await hub.Clients.All.SendAsync("NewLocation", location);
        }
    }
}

