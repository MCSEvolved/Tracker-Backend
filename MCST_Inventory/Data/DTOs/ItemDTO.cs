using System;
namespace MCST_Inventory.Data.DTOs
{
	public class ItemDTO
	{
        public ItemDTO(string name, int size, int stackSize, int slot)
        {
            Name = name;
            Size = size;
            StackSize = stackSize;
            Slot = slot;
        }

        public string Name { get; set; } = default!;
        public int Size { get; set; } = -1;
        public int StackSize { get; set; } = -1;
        public int Slot { get; set; } = -1;
	}
}

