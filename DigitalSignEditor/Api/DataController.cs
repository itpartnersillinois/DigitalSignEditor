using DigitalSignEditor.Twitter;
using DigitalSignEditor.Weather;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignEditor.Api {

    [Route("api/[controller]")]
    [ApiController]
    public class DataController : Controller {
        private TwitterHelper twitterHelper;
        private WeatherHelper weatherHelper;

        public DataController(TwitterHelper twitterHelper, WeatherHelper weatherHelper) {
            this.twitterHelper = twitterHelper;
            this.weatherHelper = weatherHelper;
        }

        [HttpGet("Calendar/{id}")]
        [AllowAnonymous]
        public IActionResult Calendar(string id) {
            // TODO Add Calendar information
            return new JsonResult("");
        }

        [HttpGet("Twitter/{id}")]
        [AllowAnonymous]
        public IActionResult Twitter(string id) {
            twitterHelper.Update(id);
            return new JsonResult(twitterHelper);
        }

        [HttpGet("Weather")]
        [AllowAnonymous]
        public IActionResult Weather() {
            weatherHelper.Update();
            return new JsonResult(weatherHelper);
        }
    }
}