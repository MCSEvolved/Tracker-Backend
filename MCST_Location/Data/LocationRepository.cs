using System;
using MCST_Location.Data.DTOs;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MCST_Location.Data
{
	public class LocationRepository
	{
        private readonly IMongoCollection<LocationDTO> _locations;

        public LocationRepository(IMongoClient client)
        {
            var database = client.GetDatabase("mcst_dev");
            var collection = database.GetCollection<LocationDTO>("locations");

            _locations = collection;
        }

        public void InsertLocation(LocationDTO location)
        {
            var filter = Builders<LocationDTO>.Filter.Eq("_id", location.ComputerId);
            if (_locations.Find(filter).FirstOrDefault() == null)
            {
                _locations.InsertOne(location);
            }
            else
            {
                _locations.ReplaceOne(filter, location);
            }
        }

        public LocationDTO GetLocationByComputerId(int computerId)
        {
            var sort = Builders<LocationDTO>.Sort.Ascending("_id");
            LocationDTO location = _locations
                .Find(new BsonDocument("_id", computerId))
                .Sort(sort)
                .FirstOrDefault();
            return location;
        }
    }
}

