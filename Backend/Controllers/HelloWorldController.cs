using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public string SayHello()
        {
            return "Hello from recruitment test backend";
        }
    }
}
