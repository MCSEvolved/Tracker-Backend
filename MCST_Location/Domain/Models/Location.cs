using System;
using System.ComponentModel.DataAnnotations;

namespace MCST_Location.Domain.Models
{
	public class Coordinates
	{
        public Coordinates(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }


	public class Location
	{
        public int ComputerId { get; set; } = -1;
        public Coordinates Coordinates { get; set; } = default!;
        public string Dimension { get; set; } = "Unknown";
        public long CreatedOn { get; private set; } = DateTimeOffset.Now.ToUnixTimeSeconds();

        public bool IsValid()
        {
            return ComputerId > -1 && Coordinates != null && CreatedOn > 0;
        }

        public void OverrideCreatedOn(long createdOn)
        {
            CreatedOn = createdOn;
        }
    }
}

