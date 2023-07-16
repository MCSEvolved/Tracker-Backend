using System;
using MCST_Controller.SignalRHubs;
using MCST_ControllerInterfaceLayer.Interfaces;
using MCST_Models;
using Microsoft.AspNetCore.SignalR;

namespace MCST_Controller.Services
{
	public class WsService: IWsService
	{
        private readonly IHubContext<ServerHub> serverHub;
        private readonly IHubContext<ClientHub> clientHub;

        public WsService(IHubContext<ServerHub> _serverHub, IHubContext<ClientHub> _clientHub)
		{
			this.serverHub = _serverHub;
            this.clientHub = _clientHub;
		}

        public async Task NewComputerOverWS(Computer computer)
        {
            await clientHub.Clients.All.SendAsync("NewComputer", computer);
        }

        public async Task NewInventoryOverWS(Inventory inventory)
        {
            await clientHub.Clients.All.SendAsync("NewInventory", inventory);
        }

        public async Task SendCommandToComputer(int computerId, string command)
        {
            await serverHub.Clients.All.SendAsync("ComputerCommand", computerId, command);
        }


        public async Task NewLocationOverWS(Location location)
        {
            await clientHub.Clients.All.SendAsync("NewLocation", location);
        }

        public async Task NewMessageOverWS(Message message)
        {
            await clientHub.Clients.All.SendAsync("NewMessage", message);
        }
    }
}

