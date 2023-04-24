using System;
using System.Text.Json;
using MCST_Computer.Data;
using MCST_Computer.Data.DTOs;
using MCST_Computer.Domain.Models;

namespace MCST_Computer.Domain
{
	public class ComputerService
	{
		private readonly ComputerRepository repo;

        public ComputerService(ComputerRepository repo)
        {
            this.repo = repo;
        }

        public void NewComputer(Computer c)
        {
            repo.InsertComputer(new ComputerDTO(c.Id, c.Label, c.SystemId, c.Device, c.FuelLimit, c.Status, c.FuelLevel, c.LastUpdate));
        }

        public List<Computer> GetAllComputers()
        {
            List<ComputerDTO> computerDTOs = repo.GetAllComputers();
            List<Computer> computers = new List<Computer>();
            foreach (var computerDTO in computerDTOs)
            {
                Computer computer = new Computer
                {
                    Id = computerDTO.Id,
                    Label = computerDTO.Label,
                    SystemId = computerDTO.SystemId,
                    Device = computerDTO.Device,
                    FuelLevel = computerDTO.FuelLevel,
                    Status = computerDTO.Status,
                    FuelLimit = computerDTO.FuelLimit,
                    LastUpdate = computerDTO.LastUpdate
                };
                computers.Add(computer);
            }
            return computers;
        }

        public List<Computer> GetAllComputersBySystem(int systemId)
        {
            List<ComputerDTO> computerDTOs = repo.GetAllComputersBySystem(systemId);
            List<Computer> computers = new List<Computer>();
            foreach (var computerDTO in computerDTOs)
            {
                Computer computer = new Computer
                {
                    Id = computerDTO.Id,
                    Label = computerDTO.Label,
                    SystemId = computerDTO.SystemId,
                    Device = computerDTO.Device,
                    FuelLevel = computerDTO.FuelLevel,
                    Status = computerDTO.Status,
                    FuelLimit = computerDTO.FuelLimit,
                    LastUpdate = computerDTO.LastUpdate
                };
                computers.Add(computer);
            }
            return computers;
        }

        public Computer? GetComputerById(int id)
        {
            ComputerDTO computerDTO = repo.GetComputerById(id);
            if (computerDTO == null)
            {
                return null;
            }
            Computer computer = new Computer
            {
                Id = computerDTO.Id,
                Label = computerDTO.Label,
                SystemId = computerDTO.SystemId,
                Device = computerDTO.Device,
                FuelLevel = computerDTO.FuelLevel,
                Status = computerDTO.Status,
                FuelLimit = computerDTO.FuelLimit,
                LastUpdate = computerDTO.LastUpdate
            };

            return computer;
        }
    }
}

