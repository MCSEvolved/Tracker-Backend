using System;
using MCST_ControllerInterfaceLayer.Interfaces;
using MCST_Models;

namespace MCST_Command.Domain
{
	public class CommandService
	{
        private readonly IWsService wsService;

        public CommandService(IWsService wsService)
		{
            this.wsService = wsService;
        }
		
		public async Task ExecuteCommand(ComputerCommand command)
		{
			await wsService.SendCommandToComputer(command.ComputerId, command.Command);
		}
	}
}

