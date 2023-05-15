using System;
using MCST_Inventory.Data.DTOs;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MCST_Inventory.Data
{
	public class InventoryRepository
	{
        private IMongoCollection<InventoryDTO> _inventories;

        public InventoryRepository(IMongoClient client)
		{
            var database = client.GetDatabase("mcst_dev");
            _inventories = database.GetCollection<InventoryDTO>("inventories");
        }

        public void InsertInventory(InventoryDTO inventory)
        {
            var filter = Builders<InventoryDTO>.Filter.Eq("_id", inventory.ComputerId);
            if (_inventories.Find(filter).FirstOrDefault() == null)
            {
                _inventories.InsertOne(inventory);
            }
            else
            {
                _inventories.ReplaceOne(filter, inventory);
            }
        }

        public InventoryDTO GetInventory(int computerId)
        {
            var sort = Builders<InventoryDTO>.Sort.Ascending("_id");
            InventoryDTO inventory = _inventories
                .Find(new BsonDocument("_id", computerId))
                .Sort(sort)
                .FirstOrDefault();
            return inventory;
        }
	}
}

