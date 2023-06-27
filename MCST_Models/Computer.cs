using System;
using System.ComponentModel.DataAnnotations;
using MCST_Enums;

namespace MCST_Models
{
	
	public class Computer
	{
        public Computer()
        {
        }

        public int Id { get; set; } = -1;

		public string Label { get; set; } = "No Label";

		public int SystemId { get; set; } = -1;

		public ComputerDevice Device { get; set; } = ComputerDevice.INVALID;

		public int FuelLimit { get; set; } = -1;

        public string Status { get; set; } = "Working";

        public int FuelLevel { get; set; } = -1;

		public long LastUpdate { get; private set; } = DateTimeOffset.Now.ToUnixTimeSeconds();

		public bool HasModem { get; set; } = false;

        public bool IsValid()
        {
            if (Device == ComputerDevice.INVALID)
            {
                return false;
            }
            else if (Device == ComputerDevice.Turtle || Device == ComputerDevice.Advanced_Turtle)
            {
                return Id > -1 && LastUpdate > 0 && FuelLimit > -1 && FuelLevel > -1;
            }
            else
            {
                return Id > -1 && LastUpdate > 0;
            }
        }

        public void OverrideLastUpdate(long lastUpdate)
        {
            LastUpdate = lastUpdate;
        }


    }

    
}

