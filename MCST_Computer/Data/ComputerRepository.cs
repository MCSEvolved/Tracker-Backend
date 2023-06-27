using System;
using System.Text.Json;
using MCST_Computer.Data.DTOs;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MCST_Computer.Data
{
	public class ComputerRepository
	{
        private readonly IMongoCollection<ComputerDTO> _computers;

        public ComputerRepository(IMongoClient client, IConfiguration config)
        {
            var database = client.GetDatabase(config["MongoDb:DbName"]);
            var collection = database.GetCollection<ComputerDTO>("computers");

            _computers = collection;
        }

        public async Task InsertComputer(ComputerDTO computer)
        {
            var filter = Builders<ComputerDTO>.Filter.Eq("_id", computer.Id);
            if (await _computers.Find(filter).FirstOrDefaultAsync() == null)
            {
                await _computers.InsertOneAsync(computer);
            } else
            {
                await _computers.ReplaceOneAsync(filter, computer);
            }
        }

        public async Task<List<ComputerDTO>> GetAllComputers()
        {
            var sort = Builders<ComputerDTO>.Sort.Ascending("_id");
            List<ComputerDTO> computers = await _computers
                .Find(new BsonDocument())
                .Sort(sort)
                .ToListAsync();
            return computers;
        }

        public async Task<List<ComputerDTO>> GetAllComputersBySystem(int systemId)
        {
            var sort = Builders<ComputerDTO>.Sort.Ascending("_id");
            List<ComputerDTO> computers = await _computers
                .Find(new BsonDocument("SystemId", systemId))
                .Sort(sort)
                .ToListAsync();
            return computers;
        }

        public async Task<ComputerDTO> GetComputerById(int id)
        {
            var sort = Builders<ComputerDTO>.Sort.Ascending("_id");
            ComputerDTO computer = await _computers
                .Find(new BsonDocument("_id", id))
                .Sort(sort)
                .FirstOrDefaultAsync();
            return computer;
        }

    }
}

