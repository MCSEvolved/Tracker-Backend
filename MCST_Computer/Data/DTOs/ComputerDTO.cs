
using System;
using MCST_Computer.Domain.Models;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MCST_Enums;

namespace MCST_Computer.Data.DTOs
{
	public class ComputerDTO
	{
        public ComputerDTO(int id, string label, int systemId, ComputerDevice device, int fuelLimit, string status, int fuelLevel, long lastUpdate, bool hasModem)
        {
            Id = id;
            Label = label;
            SystemId = systemId;
            Device = device;
            FuelLimit = fuelLimit;
            Status = status;
            FuelLevel = fuelLevel;
            LastUpdate = lastUpdate;
            HasModem = hasModem;
        }

        [BsonId]
        public int Id { get; set; }
        public string Label { get; set; }
        public int SystemId { get; set; }
        public ComputerDevice Device { get; set; }
        public int FuelLimit { get; set; }
        public string Status { get; set; }
        public int FuelLevel { get; set; } 
        public long LastUpdate { get; set; }
        public bool HasModem { get; set; }
    }
}

