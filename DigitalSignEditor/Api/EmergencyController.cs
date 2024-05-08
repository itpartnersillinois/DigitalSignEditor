using DigitalSignEditor.Emergency;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignEditor.Api {

    [Route("emergency")]
    [ApiController]
    public class EmergencyController : Controller {
        private readonly EmergencyContainer _container;

        public EmergencyController(EmergencyContainer container) {
            _container = container;
        }

        [HttpGet("Get")]
        [AllowAnonymous]
        [DisableCors]
        public IActionResult Index() {
            var results = _container.Get();
            return string.IsNullOrWhiteSpace(results.Title) && string.IsNullOrWhiteSpace(results.Description) ?
                new JsonResult("") :
                new JsonResult(results);
        }

        [HttpGet("GetFull")]
        [AllowAnonymous]
        [DisableCors]
        public IActionResult Time() => new JsonResult(_container.Get());
    }
}