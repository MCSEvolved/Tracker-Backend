using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MCST_Message.Models;
using MCST_Message;

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

        [HttpPost]
        [Route("new")]
        public IActionResult NewMessage([FromBody] Message message)
        {
            service.NewMessage(message);
            return Ok(message);
        }

        [HttpGet]
        [Route("get/all")]
        public List<Message> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            return service.GetAll(page, pageSize);
        }

        [HttpGet]
        [Route("get/by-source")]
        public List<Message> GetBySource([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] MessageSource source)
        {
            return service.GetBySource(page, pageSize, source);
        }
        [HttpGet]
        [Route("get/by-identifiers")]
        public List<Message> GetByIdentifier([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string[] identifiers)
        {
            return service.GetByIdentifiers(page, pageSize, identifiers);
        }
    }
}

