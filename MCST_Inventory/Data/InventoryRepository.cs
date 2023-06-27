using System;
using MCST_Inventory.Data.DTOs;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MCST_Inventory.Data
{
	public class InventoryRepository
	{
        private IMongoCollection<InventoryDTO> _inventories;

        public InventoryRepository(IMongoClient client, IConfiguration config)
		{
            var database = client.GetDatabase(config["MongoDb:DbName"]);
            _inventories = database.GetCollection<InventoryDTO>("inventories");
        }

        public async Task InsertInventory(InventoryDTO inventory)
        {
            var filter = Builders<InventoryDTO>.Filter.Eq("_id", inventory.ComputerId);
            if (await _inventories.FindAsync(filter) == null)
            {
                await _inventories.InsertOneAsync(inventory);
            }
            else
            {
                await _inventories.ReplaceOneAsync(filter, inventory);
            }
        }

        public async Task<InventoryDTO> GetInventory(int computerId)
        {
            var sort = Builders<InventoryDTO>.Sort.Ascending("_id");
            InventoryDTO inventory = await _inventories
                .Find(new BsonDocument("_id", computerId))
                .Sort(sort)
                .FirstOrDefaultAsync();
            return inventory;
        }
	}
}

