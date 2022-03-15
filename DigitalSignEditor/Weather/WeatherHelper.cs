using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace DigitalSignEditor.Weather {

    public class WeatherHelper {
        private WeatherDay[] days = new WeatherDay[3];
        private DateTime? lastAccessed;
        public WeatherDay Day1 => days[0];
        public WeatherDay Day2 => days[1];
        public WeatherDay Day3 => days[2];
        public string Icon { get; set; }

        //TODO Switch over to another service -- maybe https://api.weather.gov/gridpoints/ILX/95,71/forecast
        public void Update() {
            if (lastAccessed == null || lastAccessed.Value.AddHours(1) < DateTime.Now) {
                var client = new HttpClient(new HttpClientHandler());
                var weather = client.GetStringAsync("https://api.darksky.net/forecast/b756fbe60e926c034122775956f56ffd/40.1020,-88.2272?exclude=minutely,hourly,flags").Result;
                dynamic weatherJson = JObject.Parse(weather);
                lastAccessed = DateTime.Now;
                Icon = weatherJson["daily"].data[0].icon;
                days[0] = new WeatherDay(weatherJson["daily"].data[0], "Today");
                days[1] = new WeatherDay(weatherJson["daily"].data[1], "Tomorrow");
                days[2] = new WeatherDay(weatherJson["daily"].data[2], DateTime.Now.AddDays(2).DayOfWeek.ToString());
            }
        }
    }
}