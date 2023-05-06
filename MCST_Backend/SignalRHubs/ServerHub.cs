using System;
using MCST_Computer.Domain;
using MCST_Computer.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
namespace MCST_Backend.SignalRHubs
{
    [Authorize]
    public class ServerHub : Hub
	{
		private readonly ComputerService computerService;

        public ServerHub(ComputerService computerService)
        {
            this.computerService = computerService;
        }

        public async Task NewComputer(Computer computer)
		{
            computerService.NewComputer(computer);
            await Clients.Caller.SendAsync("NewComputer", 200, "OK");
		}
	}
}

