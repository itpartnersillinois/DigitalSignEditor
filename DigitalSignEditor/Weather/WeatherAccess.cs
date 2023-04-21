using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace DigitalSignEditor.Weather {

    public static class WeatherAccess {
        private const string DarkSkyUrl = "https://api.darksky.net/forecast/b756fbe60e926c034122775956f56ffd/40.1020,-88.2272?exclude=minutely,hourly,flags";
        private const string GovUrl = "https://api.weather.gov/gridpoints/ILX/95,71/forecast/hourly";

        public static dynamic GetWeather() => GetWeatherByUrl(GovUrl);

        private static dynamic GetWeatherByUrl(string url) {
            var client = new HttpClient(new HttpClientHandler());
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "University of Illinois C-U Digital Sign Application; jonker@illinois.edu");
            var weather = client.GetStringAsync(url).Result;
            return JObject.Parse(weather);
        }
    }
}