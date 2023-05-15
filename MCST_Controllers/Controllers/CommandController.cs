using System;
using MCST_Command;
using MCST_Controller.SignalRHubs;
using Microsoft.AspNetCore.SignalR;

namespace MCST_Controller.Controllers
{
	public class CommandController: ICommandController
	{
        private readonly IHubContext<ServerHub> hub;

        public CommandController(IHubContext<ServerHub> hub)
		{
            this.hub = hub;
        }

        public async void SendCommandToComputer(int computerId, string command)
        {
            await hub.Clients.All.SendAsync("ComputerCommand", computerId, command);
        }

        
    }
}

