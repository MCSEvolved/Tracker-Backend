using Amazon.Auth.AccessControlPolicy;
using MCST_Command.Domain;
using MCST_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCST_Controller.Controllers
{

    [ApiController]
    [Route("tracker/command")]
    public class CommandController : ControllerBase
    {
        private readonly CommandService service;

        public CommandController(CommandService service)
		{
            this.service = service;
        }

        [HttpPost("execute")]
        [Authorize(Policy = "IsPlayer")]
        public async Task<IActionResult> ExecuteCommand([FromQuery] int[] computerIds, string command)
        {
            foreach (var computerId in computerIds)
            {
                await service.ExecuteCommand(new ComputerCommand(computerId, command));
            }
            
            return Ok($"Command '{command}' is send to turtle(s)");
        }
        
        
        
    }
}

