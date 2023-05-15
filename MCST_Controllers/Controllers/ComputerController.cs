using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCST_Computer;
using MCST_Computer.Domain;
using MCST_Computer.Domain.Models;
using MCST_Controller.SignalRHubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MCST_Controller.Controllers
{
    [ApiController]
    [Route("api/computer")]
    public class ComputerController : ControllerBase, IComputerController
    {
        private readonly ComputerService service;
        private readonly IHubContext<ClientHub> hub;

        public ComputerController(ComputerService service, IHubContext<ClientHub> hub)
        {
            this.service = service;
            this.hub = hub;
        }

        [HttpPost("new")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Computer))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult NewComputer([FromBody] Computer computer)
        {
            if (service.NewComputer(computer))
            {
                return Ok(computer);
            } else
            {
                return BadRequest(computer);
            }
        }

        [HttpGet("get/all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Computer>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            var computers = service.GetAllComputers();
            return computers.Count() < 1 ? NotFound() : Ok(computers);
        }

        [HttpGet("get/by-id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Computer))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById([FromQuery]int id)
        {
            var computer = service.GetComputerById(id);
            return computer == null ? NotFound() : Ok(computer);
        }

        [HttpGet("get/by-system")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Computer>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBySystem([FromQuery] int systemId)
        {
            var computers = service.GetAllComputersBySystem(systemId);
            return computers.Count() < 1 ? NotFound() : Ok(computers);
        }

        public async void NewComputerOverWS(Computer computer)
        {
            await hub.Clients.All.SendAsync("NewComputer", computer);
        }
    }
}