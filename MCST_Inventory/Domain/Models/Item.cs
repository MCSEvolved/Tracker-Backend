using System;
namespace MCST_Inventory.Domain.Models
{
	public class Item
	{
		public string Name { get; set; } = default!;
		public int Size { get; set; } = -1;
		public int StackSize { get; set; } = -1;
		public int Slot { get; set; } = -1;

		public bool IsValid()
		{
			return Name != null && Size > -1 && StackSize > -1 && Slot > -1;
		}
	}
}

