using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace State_ApiSideOnly.Controllers
{
    [ApiController]
    [Authorize]
    [Route("Controller")]
    public class ValuesController : Controller
    {
        [HttpGet("GetValues")]
        public IActionResult GetValues()
        {
            return Ok(new { value1= "firstvalue" , value2="secondvalue"});
        }
    }
}
