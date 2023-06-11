using MCST_Inventory.Domain;
using MCST_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MCST_Controller.Controllers
{
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService service;

        public InventoryController(InventoryService service)
        {
            this.service = service;
        }

        [HttpGet("get/by-id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Inventory))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "IsGuest")]
        public IActionResult RequestInventory(int computerId)
        {
            Inventory? inventory = service.RequestInventory(computerId);
            return inventory == null ? NotFound() : Ok(inventory);
        }

        [HttpPost("new")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Inventory))]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Policy = "IsService")]
        public IActionResult NewInventory(Inventory inventory)
        {
            return !service.NewInventory(inventory) ? Accepted() : Accepted(inventory);
        }

        
    }
}

