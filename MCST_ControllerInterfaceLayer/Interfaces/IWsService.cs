using System;
using MCST_Models;

namespace MCST_ControllerInterfaceLayer.Interfaces
{
	public interface IWsService
	{
        void SendCommandToComputer(int computerId, string command);
        void NewComputerOverWS(Computer computer);
        void SendRequestInventoryCommand(int computerId);
        void NewInventoryOverWS(Inventory inventory);
        void NewLocationOverWS(Location location);
        void NewMessageOverWS(Message message);
    }
}

