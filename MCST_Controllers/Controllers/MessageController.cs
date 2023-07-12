using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using MCST_Message.Domain;
using MCST_Enums;
using MCST_Models;
using Google.Api.Gax;

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
                return Ok("message saved");
            } else
            {
                return BadRequest("message was not a valid model, why? idk I haven't implemented good exceptions yet");
            }
        }

        [HttpGet("get/all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Message>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "IsGuest")]
        public async Task<IActionResult> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            if (page < 1) {
                return BadRequest("Page has to be 1 or higher");
            }
            if (pageSize < 1) {
                return BadRequest("PageSize has to be 1 or higher");
            }
            var messages = await service.GetAll(page, pageSize);
            return Ok(messages);
        }

        [HttpGet("get/by-source")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Message>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "IsGuest")]
        public async Task<IActionResult> GetBySource([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] MessageSource source)
        {
            if (page < 1)
            {
                return BadRequest("Page has to be 1 or higher");
            }
            if (pageSize < 1)
            {
                return BadRequest("PageSize has to be 1 or higher");
            }
            var messages = await service.GetBySource(page, pageSize, source);
            return Ok(messages);
        }

        [HttpGet("get/by-source-ids")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Message>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "IsGuest")]
        public async Task<IActionResult> GetBySourceIds([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string[] sourceIds)
        {
            if (page < 1)
            {
                return BadRequest("Page has to be 1 or higher");
            }
            if (pageSize < 1)
            {
                return BadRequest("PageSize has to be 1 or higher");
            }
            var messages = await service.GetBySourceIds(page, pageSize, sourceIds);
            return Ok(messages);
        }

        [HttpGet("get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Message>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Policy = "IsGuest")]
        public async Task<IActionResult> GetByFilters([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] MessageFilter filters)
        {
            if (page < 1)
            {
                return BadRequest("Page has to be 1 or higher");
            }
            if (pageSize < 1)
            {
                return BadRequest("PageSize has to be 1 or higher");
            }
            var messages = await service.GetByFilters(page, pageSize, filters);
            return Ok(messages);
        }

        [HttpGet("get/amount")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Message>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Policy = "IsGuest")]
        public async Task<IActionResult> GetAmountByFilters([FromQuery] MessageFilter filters)
        {
            var amount = await service.GetAmountByFilters(filters);
            return Ok(amount);
        }
    }
}

