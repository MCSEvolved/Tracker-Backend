using System;
using MCST_Location.Data.DTOs;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;


namespace MCST_Location.Data
{
	public class LocationRepository
	{
        private readonly IMongoCollection<LocationDTO> _locations;

        public LocationRepository(IMongoClient client, IConfiguration config)
        {
            var database = client.GetDatabase(config["MongoDb:DbName"]);
            var collection = database.GetCollection<LocationDTO>("locations");

            _locations = collection;
        }

        public async Task InsertLocation(LocationDTO location)
        {
            var filter = Builders<LocationDTO>.Filter.Eq("_id", location.ComputerId);
            if (_locations.Find(filter).FirstOrDefault() == null)
            {
                await _locations.InsertOneAsync(location);
            }
            else
            {
                await _locations.ReplaceOneAsync(filter, location);
            }
        }

        public async Task<LocationDTO> GetLocationByComputerId(int computerId)
        {
            var sort = Builders<LocationDTO>.Sort.Ascending("_id");
            LocationDTO location = await _locations
                .Find(new BsonDocument("_id", computerId))
                .Sort(sort)
                .FirstOrDefaultAsync();
            return location;
        }
    }
}

