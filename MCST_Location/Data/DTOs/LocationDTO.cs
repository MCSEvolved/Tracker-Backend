using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MCST_Location.Data.DTOs
{
	public class CoordinatesDTO
	{
        public CoordinatesDTO(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }


	public class LocationDTO
	{
        public LocationDTO(int computerId, CoordinatesDTO coordinates, string dimension, long createdOn)
        {
            ComputerId = computerId;
            Coordinates = coordinates;
            Dimension = dimension;
            CreatedOn = createdOn;
        }

        [BsonId]
        public int ComputerId { get; set; }
        public CoordinatesDTO Coordinates { get; set; }
        public string Dimension { get; set; }
        public long CreatedOn { get; set; }
    }
}

