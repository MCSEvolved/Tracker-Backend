using System;
using MCST_Inventory.Data;
using MCST_Inventory.Data.DTOs;
using MCST_Inventory.Domain.Models;

namespace MCST_Inventory.Domain
{
	public class InventoryService
	{
        private readonly InventoryRepository repo;
        private readonly IInventoryController inventoryController
			;

        public InventoryService(InventoryRepository _inventoryRepository, IInventoryController _InventoryController)
		{
            repo = _inventoryRepository;
            inventoryController = _InventoryController;
        }

		public Inventory? RequestInventory(int computerId)
		{
			inventoryController.SendRequestInventoryCommand(computerId);

			InventoryDTO inventoryDTO = repo.GetInventory(computerId);
			if (inventoryDTO == null)
			{
				return null;
			}

			List<Item> items = new List<Item>();
			foreach (ItemDTO itemDTO in inventoryDTO.Items)
			{
				items.Add(new Item { Name = itemDTO.Name, Size = itemDTO.Size, Slot = itemDTO.Slot, StackSize = itemDTO.StackSize });
			}

			Inventory inventory = new Inventory
			{
				ComputerId = inventoryDTO.ComputerId,
				Items = items
			};
			inventory.OverrideCreatedOn(inventoryDTO.CreatedOn);

			return inventory;
		}


		public bool NewInventory(Inventory inventory)
		{
			if (inventory.IsValid())
			{
				inventoryController.NewInventoryOverWS(inventory);
				List<ItemDTO> itemDTOs = new List<ItemDTO>();
				foreach (Item item in inventory.Items)
				{
					itemDTOs.Add(new ItemDTO(item.Name, item.Size, item.StackSize, item.Slot));
				}
				repo.InsertInventory(new InventoryDTO(inventory.ComputerId, itemDTOs, inventory.CreatedOn));
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}

