using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using MCST_Message.Domain;
using MCST_Enums;
using MCST_Models;

namespace MCST_Controller.Controllers
{
    [ApiController]
    [Route("tracker/message")]
    public class MessageController : ControllerBase
    {
        private readonly MessageService service;

        public MessageController(MessageService _service)
        {
            service = _service;
        }

        [HttpPost("new")]
        [Authorize(Policy = "IsService")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Message))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> NewMessage([FromBody] Message message)
        {
            if(await service.NewMessage(message))
            {
                return Ok(message);
            } else
            {
                return BadRequest(message);
            }
        }

        [HttpGet("get/all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Message>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "IsGuest")]
        public async Task<IActionResult> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var messages = await service.GetAll(page, pageSize);
            return Ok(messages);
        }

        [HttpGet("get/by-source")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Message>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "IsGuest")]
        public async Task<IActionResult> GetBySource([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] MessageSource source)
        {
            var messages = await service.GetBySource(page, pageSize, source);
            return Ok(messages);
        }

        [HttpGet("get/by-source-ids")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Message>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "IsGuest")]
        public async Task<IActionResult> GetBySourceIds([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string[] sourceIds)
        {
            var messages = await service.GetBySourceIds(page, pageSize, sourceIds);
            return Ok(messages);
        }

        
    }
}

