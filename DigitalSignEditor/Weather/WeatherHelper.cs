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

        public void Update() {
            if (lastAccessed == null || lastAccessed.Value.AddMinutes(20) < DateTime.Now) {
                lastAccessed = DateTime.Now;
                var weatherJson = weatherAccess();
                days[0] = new WeatherDay(weatherJson.properties.periods[0], "Now");
                days[1] = new WeatherDay(weatherJson.properties.periods[1], "In an hour");
                days[2] = new WeatherDay(weatherJson.properties.periods[2], "In two hours");
                Icon = WeatherDay.GetIcon(days[0].Summary);
            }
        }
    }
}