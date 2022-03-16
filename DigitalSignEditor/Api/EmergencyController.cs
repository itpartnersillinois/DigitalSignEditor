using DigitalSignEditor.Emergency;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignEditor.Api {

    [Route("emergency")]
    [ApiController]
    public class EmergencyController : Controller {
        private EmergencyContainer container;

        public EmergencyController(EmergencyContainer container) {
            this.container = container;
        }

        [HttpGet("Get")]
        [AllowAnonymous]
        [DisableCors]
        public IActionResult Index() {
            var results = this.container.Get();
            return string.IsNullOrWhiteSpace(results.Title) && string.IsNullOrWhiteSpace(results.Description) ?
                new JsonResult("") :
                new JsonResult(results);
        }
    }
}