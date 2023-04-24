using System;
using System.ComponentModel.DataAnnotations;

namespace MCST_Computer.Domain.Models
{
	public enum ComputerDevice
	{
		Turtle,
		Advanced_Turtle,
		Computer,
		Advanced_Computer,
		Pocket_Computer,
		Advanced_Pocket_Computer
	}

	public class Computer
	{
        [Required]
        public int Id { get; set; }

		public string Label { get; set; } = "No Label";

		public int SystemId { get; set; } = -1;

		[Required]
		public ComputerDevice Device { get; set; } = ComputerDevice.Turtle;

		public int FuelLimit { get; set; } = -1;

        public string Status { get; set; } = "Working";

        public int FuelLevel { get; set; } = 0;

		public long LastUpdate { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds();
    }
}

