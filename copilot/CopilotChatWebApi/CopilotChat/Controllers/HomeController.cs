using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SemanticKernel.Service.CopilotChat.Controllers
{
    [ApiController]
    [Authorize]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("index/{Name}")]
        public IActionResult Index(string Name)
        {
            return this.Ok(Name);
            
        }
    }
}
