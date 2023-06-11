using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using MCST_Message.Domain;
using MCST_Enums;
using MCST_Models;

namespace MCST_Controller.Controllers
{
    [ApiController]
    [Route("api/message")]
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
        public IActionResult NewMessage([FromBody] Message message)
        {
            if(service.NewMessage(message))
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
        public IActionResult GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var messages = service.GetAll(page, pageSize);
            return messages.Count() < 1 ? NotFound() : Ok(messages);
        }

        [HttpGet("get/by-source")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Message>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "IsGuest")]
        public IActionResult GetBySource([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] MessageSource source)
        {
            var messages = service.GetBySource(page, pageSize, source);
            return messages.Count() < 1 ? NotFound() : Ok(messages);
        }

        [HttpGet("get/by-identifiers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Message>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "IsGuest")]
        public IActionResult GetByIdentifier([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string[] identifiers)
        {
            var messages = service.GetByIdentifiers(page, pageSize, identifiers);
            return messages.Count() < 1 ? NotFound() : Ok(messages);
        }

        
    }
}

