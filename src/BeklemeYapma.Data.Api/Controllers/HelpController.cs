using Microsoft.AspNetCore.Mvc;

namespace BeklemeYapma.Data.Api.Controllers
{
    [Produces("application/json")]
    [Route("healthcheck")]
    public class HelpController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}