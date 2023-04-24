using System;
using System.Text.Json;
using MCST_Computer.Data.DTOs;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MCST_Computer.Data
{
	public class ComputerRepository
	{
        private readonly IMongoCollection<ComputerDTO> _computers;

        public ComputerRepository(IMongoClient client)
        {
            var database = client.GetDatabase("mcst_dev");
            var collection = database.GetCollection<ComputerDTO>("computers");

            _computers = collection;
        }

        public void InsertComputer(ComputerDTO computer)
        {
            var filter = Builders<ComputerDTO>.Filter.Eq("_id", computer.Id);
            if (_computers.Find(filter).FirstOrDefault() == null)
            {
                _computers.InsertOne(computer);
            } else
            {
                _computers.ReplaceOne(filter, computer);
            }
        }

        public List<ComputerDTO> GetAllComputers()
        {
            var sort = Builders<ComputerDTO>.Sort.Ascending("_id");
            List<ComputerDTO> computers = _computers
                .Find(new BsonDocument())
                .Sort(sort)
                .ToList();
            return computers;
        }

        public List<ComputerDTO> GetAllComputersBySystem(int systemId)
        {
            var sort = Builders<ComputerDTO>.Sort.Ascending("_id");
            List<ComputerDTO> computers = _computers
                .Find(new BsonDocument("SystemId", systemId))
                .Sort(sort)
                .ToList();
            return computers;
        }

        public ComputerDTO GetComputerById(int id)
        {
            var sort = Builders<ComputerDTO>.Sort.Ascending("_id");
            ComputerDTO computer = _computers
                .Find(new BsonDocument("_id", id))
                .Sort(sort)
                .FirstOrDefault();
            return computer;
        }

    }
}

