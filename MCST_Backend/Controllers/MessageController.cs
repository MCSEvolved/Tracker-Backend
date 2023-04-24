using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MCST_Message.Models;
using MCST_Message;
using Microsoft.AspNetCore.Authorization;
using MCST_Computer.Domain.Models;

namespace MCST_Backend.Controllers
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
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Message))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult NewMessage([FromBody] Message message)
        {
            service.NewMessage(message);
            return Ok(message);
        }

        [HttpGet("get/all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Message>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var messages = service.GetAll(page, pageSize);
            return messages.Count() < 1 ? NotFound() : Ok(messages);
        }

        [HttpGet("get/by-source")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Message>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBySource([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] MessageSource source)
        {
            var messages = service.GetBySource(page, pageSize, source);
            return messages.Count() < 1 ? NotFound() : Ok(messages);
        }

        [HttpGet("get/by-identifiers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Message>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByIdentifier([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string[] identifiers)
        {
            var messages = service.GetByIdentifiers(page, pageSize, identifiers);
            return messages.Count() < 1 ? NotFound() : Ok(messages);
        }
    }
}

