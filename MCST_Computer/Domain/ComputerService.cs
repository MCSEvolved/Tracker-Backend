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
        private readonly IComputerController controller;

        public ComputerService(ComputerRepository repo, IComputerController controller)
        {
            this.repo = repo;
            this.controller = controller;
        }

        public bool NewComputer(Computer c)
        {
            if (c.IsValid())
            {
                controller.NewComputerOverWS(c);
                repo.InsertComputer(new ComputerDTO(c.Id, c.Label, c.SystemId, c.Device, c.FuelLimit, c.Status, c.FuelLevel, c.LastUpdate, c.HasModem));
                return true;
            } else
            {
                return false;
            }
        }

        public List<Computer> GetAllComputers()
        {
            List<ComputerDTO> computerDTOs = repo.GetAllComputers();
            List<Computer> computers = new List<Computer>();
            foreach (var computerDTO in computerDTOs)
            {
                Models.Computer computer = new Computer
                {
                    Id = computerDTO.Id,
                    Label = computerDTO.Label,
                    SystemId = computerDTO.SystemId,
                    Device = computerDTO.Device,
                    FuelLevel = computerDTO.FuelLevel,
                    Status = computerDTO.Status,
                    FuelLimit = computerDTO.FuelLimit,
                    HasModem = computerDTO.HasModem
                };
                computer.OverrideLastUpdate(computerDTO.LastUpdate);
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
                    HasModem = computerDTO.HasModem
                };
                computer.OverrideLastUpdate(computerDTO.LastUpdate);
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
                HasModem = computerDTO.HasModem
            };
            computer.OverrideLastUpdate(computerDTO.LastUpdate);

            return computer;
        }
    }
}

