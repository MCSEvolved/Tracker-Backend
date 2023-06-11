using MCST_Computer.Domain;
using MCST_Inventory.Domain;
using MCST_Location.Domain;
using MCST_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
namespace MCST_Controller.SignalRHubs
{
    [Authorize(Policy = "IsService")]
    public class ServerHub : Hub
	{
		private readonly ComputerService computerService;
        private readonly LocationService locationService;
        private readonly InventoryService inventoryService;

        public ServerHub(ComputerService computerService, LocationService locationService, InventoryService inventoryService)
        {
            this.computerService = computerService;
            this.locationService = locationService;
            this.inventoryService = inventoryService;
        }

        public async Task NewComputer(Computer computer)
		{
            if (computerService.NewComputer(computer))
            {
                await Clients.Caller.SendAsync("NewComputer", 200, "Ok");
            }
            else
            {
                await Clients.Caller.SendAsync("NewComputer", 400, "Invalid Model");
            }
		}

        public async Task NewLocation(Location location)
        {
            if (locationService.NewLocation(location))
            {
                await Clients.Caller.SendAsync("NewLocation", 200, "Ok");
            } else
            {
                await Clients.Caller.SendAsync("NewLocation", 400, "Invalid Model");
            }
        }

        public async Task NewInventory(Inventory inventory)
        {
            if (inventoryService.NewInventory(inventory))
            {
                await Clients.Caller.SendAsync("NewInventory", 200, "Ok");
            } else
            {
                await Clients.Caller.SendAsync("NewInventory", 400, "Invalid Model");
            }
        }
	}
}

