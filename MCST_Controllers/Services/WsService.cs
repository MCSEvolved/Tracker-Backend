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

        public async void NewComputerOverWS(Computer computer)
        {
            await clientHub.Clients.All.SendAsync("NewComputer", computer);
        }

        public async void SendRequestInventoryCommand(int computerId)
        {
            await serverHub.Clients.All.SendAsync("RequestInventory", computerId);
        }

        public async void NewInventoryOverWS(Inventory inventory)
        {
            await clientHub.Clients.All.SendAsync("NewInventory", inventory);
        }

        public async void SendCommandToComputer(int computerId, string command)
        {
            await serverHub.Clients.All.SendAsync("ComputerCommand", computerId, command);
        }


        public async void NewLocationOverWS(Location location)
        {
            await clientHub.Clients.All.SendAsync("NewLocation", location);
        }

        public async void NewMessageOverWS(Message message)
        {
            await clientHub.Clients.All.SendAsync("NewMessage", message);
        }
    }
}

