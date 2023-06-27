using System;
using MCST_Models;

namespace MCST_ControllerInterfaceLayer.Interfaces
{
	public interface IWsService
	{
        Task SendCommandToComputer(int computerId, string command);
        Task NewComputerOverWS(Computer computer);
        Task SendRequestInventoryCommand(int computerId);
        Task NewInventoryOverWS(Inventory inventory);
        Task NewLocationOverWS(Location location);
        Task NewMessageOverWS(Message message);
    }
}

