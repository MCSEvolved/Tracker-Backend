using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCST_Computer.Domain.Models;
using MCST_Controller.SignalRHubs;
using MCST_Inventory;
using MCST_Inventory.Domain;
using MCST_Inventory.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MCST_Controller.Controllers
{
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController : ControllerBase, IInventoryController
    {
        private readonly InventoryService service;
        private readonly IHubContext<ServerHub> hub;

        public InventoryController(InventoryService service, IHubContext<ServerHub> hub)
        {
            this.service = service;
            this.hub = hub;
        }

        [HttpGet("get/by-id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Inventory))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RequestInventory(int computerId)
        {
            Inventory? inventory = service.RequestInventory(computerId);
            return inventory == null ? NotFound() : Ok(inventory);
        }

        [HttpPost("new")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Inventory))]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public IActionResult NewInventory(Inventory inventory)
        {
            return !service.NewInventory(inventory) ? Accepted() : Accepted(inventory);
        }

        public async void SendRequestInventoryCommand(int computerId)
        {
            await hub.Clients.All.SendAsync("RequestInventory", computerId);
        }

        public async void NewInventoryOverWS(Inventory inventory)
        {
            await hub.Clients.All.SendAsync("NewInventory", inventory);
        }
    }
}

