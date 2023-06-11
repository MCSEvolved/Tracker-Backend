using System;
namespace MCST_Models
{
	public class Inventory
	{
		public int ComputerId { get; set; } = -1;
		public List<Item> Items { get; set; } = new List<Item>();
        public long CreatedOn { get; private set; } = DateTimeOffset.Now.ToUnixTimeSeconds();

        public bool IsValid()
		{
			foreach (var item in Items)
			{
				if (!item.IsValid())
				{
					return false;
				}
			}

			return ComputerId > -1 && CreatedOn > 0;
		}

        public void OverrideCreatedOn(long createdOn)
        {
            CreatedOn = createdOn;
        }
    }
}

