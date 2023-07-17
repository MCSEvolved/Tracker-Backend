using MCST_Computer.Domain;
using MCST_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCST_Controller.Controllers
{
    [ApiController]
    [Route("tracker/computer")]
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
        public async Task<IActionResult> NewComputer([FromBody] Computer computer)
        {
            if (await service.NewComputer(computer))
            {
                return Ok(computer);
            } else
            {
                return BadRequest("JSON is Invalid");
            }
        }

        [HttpGet("get/all")]
        [Authorize(Policy = "IsGuest")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Computer>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var computers = await service.GetAllComputers();
            return Ok(computers);
        }

        [HttpGet("get/by-id")]
        [Authorize(Policy = "IsGuest")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Computer))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromQuery]int id)
        {
            var computer = await service.GetComputerById(id);
            return computer == null ? NotFound() : Ok(computer);
        }

        [HttpGet("get/by-system")]
        [Authorize(Policy = "IsGuest")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Computer>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBySystem([FromQuery] int systemId)
        {
            var computers = await service.GetAllComputersBySystem(systemId);
            return Ok(computers);
        }

        
    }
}