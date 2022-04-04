using System.Linq;
using DigitalSignEditor.Calendar;
using DigitalSignEditor.Data;
using DigitalSignEditor.Twitter;
using DigitalSignEditor.Weather;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DigitalSignEditor.Api {

    [Route("api/[controller]")]
    [ApiController]
    public class DataController : Controller {
        private readonly CalendarHelper calendarHelper;
        private readonly CalendarIcsHelper calendarIcsHelper;
        private readonly ISignRepository signRepository;
        private readonly ITwitterCache twitterCache;
        private readonly TwitterHelper twitterHelper;
        private readonly WeatherHelper weatherHelper;

        public DataController(ITwitterCache twitterCache, TwitterHelper twitterHelper, WeatherHelper weatherHelper, CalendarHelper calendarHelper, CalendarIcsHelper calendarIcsHelper, ISignRepository signRepository) {
            this.twitterCache = twitterCache;
            this.twitterHelper = twitterHelper;
            this.weatherHelper = weatherHelper;
            this.calendarHelper = calendarHelper;
            this.calendarIcsHelper = calendarIcsHelper;
            this.signRepository = signRepository;
        }

        [HttpGet("Calendar/{id}")]
        [AllowAnonymous]
        [DisableCors]
        public IActionResult Calendar(int id) {
            var calendar = signRepository.Read(sr => sr.CalendarItems.FirstOrDefault(ci => ci.Id == id));
            if (calendar == null) {
                return new JsonResult("");
            }
            return calendar.IsIcs ? new JsonResult(calendarIcsHelper.Get(calendar.Url)) : new JsonResult(calendarHelper.Get(calendar.Url));
        }

        [HttpGet("Twitter/{id}")]
        [AllowAnonymous]
        [DisableCors]
        public IActionResult Twitter(string id) {
            var returnValue = twitterCache.GetCache(id);
            if (!string.IsNullOrEmpty(returnValue)) {
                return Content(returnValue, "application/json");
            }
            twitterHelper.Update(id);
            var jsonResult = new JsonResult(twitterHelper.Tweets);
            twitterCache.AddCache(id, JsonConvert.SerializeObject(jsonResult.Value));
            return jsonResult;
        }

        [HttpGet("Weather")]
        [AllowAnonymous]
        [DisableCors]
        public IActionResult Weather() {
            weatherHelper.Update();
            return new JsonResult(weatherHelper);
        }
    }
}