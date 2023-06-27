using MCST_Inventory.Domain;
using MCST_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MCST_Controller.Controllers
{
    [ApiController]
    [Route("inventory")]
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
        public async Task<IActionResult> RequestInventory(int computerId)
        {
            Inventory? inventory = await service.RequestInventory(computerId);
            return inventory == null ? NotFound() : Ok(inventory);
        }

        [HttpPost("new")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Inventory))]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Policy = "IsService")]
        public async Task<IActionResult> NewInventory(Inventory inventory)
        {
            bool success = await service.NewInventory(inventory);
            return !success ? Accepted() : Accepted(inventory);
        }

        
    }
}

