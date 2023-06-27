using System;
using System.Text.Json;
using MCST_Computer.Data;
using MCST_Computer.Data.DTOs;
using MCST_ControllerInterfaceLayer.Interfaces;
using MCST_Enums;
using MCST_Models;

namespace MCST_Computer.Domain
{
	public class ComputerService
	{
		private readonly ComputerRepository repo;
        private readonly IWsService clientWsService;
        private readonly INotificationService notificationService;

        public ComputerService(ComputerRepository repo, IWsService _clientWsService, INotificationService _notificationService)
        {
            this.repo = repo;
            this.clientWsService = _clientWsService;
            this.notificationService = _notificationService;
        }

  

        public async Task<bool> NewComputer(Computer c)
        {
            if (c.IsValid())
            {
                await clientWsService.NewComputerOverWS(c);
                await repo.InsertComputer(new ComputerDTO(c.Id, c.Label, c.SystemId, c.Device, c.FuelLimit, c.Status, c.FuelLevel, c.LastUpdate, c.HasModem));
                return true;
            } else
            {
                return false;
            }
        }

        public async Task<List<Computer>> GetAllComputers()
        {
            List<ComputerDTO> computerDTOs = await repo.GetAllComputers();
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

        public async Task<List<Computer>> GetAllComputersBySystem(int systemId)
        {
            List<ComputerDTO> computerDTOs = await repo.GetAllComputersBySystem(systemId);
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

        public async Task<Computer?> GetComputerById(int id)
        {
            ComputerDTO computerDTO = await repo.GetComputerById(id);
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

