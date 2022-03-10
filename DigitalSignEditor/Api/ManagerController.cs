using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignEditor.Api {

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ManagerController : ControllerBase {

        [HttpGet("Clean")]
        public async Task<string> CleanSigns() => "OK";
    }
}