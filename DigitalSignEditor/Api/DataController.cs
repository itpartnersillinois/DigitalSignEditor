using System.Linq;
using DigitalSignEditor.Calendar;
using DigitalSignEditor.Data;
using DigitalSignEditor.Weather;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignEditor.Api {

    [Route("api/[controller]")]
    [ApiController]
    public class DataController : Controller {
        private readonly CalendarHelper _calendarHelper;
        private readonly CalendarIcsHelper _calendarIcsHelper;
        private readonly ISignRepository _signRepository;
        private readonly WeatherHelper _weatherHelper;

        public DataController(WeatherHelper weatherHelper, CalendarHelper calendarHelper, CalendarIcsHelper calendarIcsHelper, ISignRepository signRepository) {
            _weatherHelper = weatherHelper;
            _calendarHelper = calendarHelper;
            _calendarIcsHelper = calendarIcsHelper;
            _signRepository = signRepository;
        }

        [HttpGet("Calendar/{id}")]
        [AllowAnonymous]
        [DisableCors]
        public IActionResult Calendar(int id) {
            var calendar = _signRepository.Read(sr => sr.CalendarItems.FirstOrDefault(ci => ci.Id == id));
            if (calendar == null) {
                return new JsonResult("");
            }
            return calendar.IsIcs ? new JsonResult(_calendarIcsHelper.Get(calendar.Url)) : new JsonResult(_calendarHelper.Get(calendar.Url));
        }

        [HttpGet("Weather")]
        [AllowAnonymous]
        [DisableCors]
        public IActionResult Weather() {
            _weatherHelper.Update();
            return new JsonResult(_weatherHelper);
        }
    }
}