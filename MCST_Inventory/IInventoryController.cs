using System;
using MCST_Inventory.Domain.Models;

namespace MCST_Inventory
{
	public interface IInventoryController
	{
        void SendRequestInventoryCommand(int computerId);
        void NewInventoryOverWS(Inventory inventory);
    }
}

