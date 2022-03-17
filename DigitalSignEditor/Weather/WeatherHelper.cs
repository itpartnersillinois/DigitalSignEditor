using System;

namespace DigitalSignEditor.Weather {

    public class WeatherHelper {
        private WeatherDay[] days = new WeatherDay[3];
        private DateTime? lastAccessed;
        private Func<dynamic> weatherAccess;

        public WeatherHelper(Func<dynamic> weatherAccess) {
            this.weatherAccess = weatherAccess;
        }

        public WeatherDay Day1 => days[0];
        public WeatherDay Day2 => days[1];
        public WeatherDay Day3 => days[2];
        public string Icon { get; set; }

        //TODO Switch over to another service -- maybe https://api.weather.gov/gridpoints/ILX/95,71/forecast
        public void Update() {
            if (lastAccessed == null || lastAccessed.Value.AddHours(1) < DateTime.Now) {
                lastAccessed = DateTime.Now;
                var weatherJson = weatherAccess();
                Icon = weatherJson["daily"].data[0].icon;
                days[0] = new WeatherDay(weatherJson["daily"].data[0], "Today");
                days[1] = new WeatherDay(weatherJson["daily"].data[1], "Tomorrow");
                days[2] = new WeatherDay(weatherJson["daily"].data[2], DateTime.Now.AddDays(2).DayOfWeek.ToString());
            }
        }
    }
}