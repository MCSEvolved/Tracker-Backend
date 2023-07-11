using Microsoft.AspNetCore.Mvc;

namespace MCST_Controller.Controllers
{
    [ApiController]
    [Route("tracker")]
    public class DefaultController
    {
        [HttpGet("is-alive")]
        public IActionResult IsAlive()
        {
            return new OkObjectResult("I am alive!");
        }
    }
}
