using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MCST_Controller.SignalRHubs;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using MCST_Message.Domain;
using MCST_Message.Models;
using MCST_Message;

namespace MCST_Controller.Controllers
{
    [ApiController]
    [Route("api/message")]
    public class MessageController : ControllerBase, IMessageController
    {
        private readonly MessageService service;
        private readonly IHubContext<ClientHub> hub;

        public MessageController(MessageService _service, IHubContext<ClientHub> hub)
        {
            service = _service;
            this.hub = hub;
        }

        [HttpPost("new")]
        [Authorize]
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

        public async void NewMessageOverWS(Message message)
        {
            await hub.Clients.All.SendAsync("NewMessage", message);
        }
    }
}

