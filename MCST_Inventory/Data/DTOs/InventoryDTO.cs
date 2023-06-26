using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MCST_Inventory.Data.DTOs
{
	public class InventoryDTO
	{
        public InventoryDTO(int computerId, List<ItemDTO> items, long createdOn)
        {
            ComputerId = computerId;
            Items = items;
            CreatedOn = createdOn;
        }

        [BsonId]
        public int ComputerId { get; set; } = -1;
        public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();
        public long CreatedOn { get; set; }
    }
}

