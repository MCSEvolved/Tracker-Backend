using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCST_Computer.Domain;
using MCST_Computer.Domain.Models;
using MCST_Message.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MCST_Backend.Controllers
{
    [ApiController]
    [Route("api/computer")]
    public class ComputerController : ControllerBase
    {
        private readonly ComputerService service;

        public ComputerController(ComputerService service)
        {
            this.service = service;
        }

        [HttpPost("new")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Computer))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult NewComputer([FromBody] Computer computer)
        {
            service.NewComputer(computer);
            return Ok(computer);
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


    }
}