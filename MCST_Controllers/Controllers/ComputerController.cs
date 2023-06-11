using MCST_Computer.Domain;
using MCST_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCST_Controller.Controllers
{
    [ApiController]
    [Route("api/computer")]
    public class ComputerController : ControllerBase
    {
        private readonly ComputerService service;

        public ComputerController(ComputerService service) {
            this.service = service;
        }

        [HttpPost("new")]
        [Authorize(Policy = "IsService")]
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
        [Authorize(Policy = "IsGuest")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Computer>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            var computers = service.GetAllComputers();
            return computers.Count() < 1 ? NotFound() : Ok(computers);
        }

        [HttpGet("get/by-id")]
        [Authorize(Policy = "IsGuest")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Computer))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById([FromQuery]int id)
        {
            var computer = service.GetComputerById(id);
            return computer == null ? NotFound() : Ok(computer);
        }

        [HttpGet("get/by-system")]
        [Authorize(Policy = "IsGuest")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Computer>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBySystem([FromQuery] int systemId)
        {
            var computers = service.GetAllComputersBySystem(systemId);
            return computers.Count() < 1 ? NotFound() : Ok(computers);
        }

        
    }
}