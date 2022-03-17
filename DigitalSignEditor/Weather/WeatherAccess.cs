using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace DigitalSignEditor.Weather {

    public static class WeatherAccess {

        public static dynamic GetWeather() {
            var client = new HttpClient(new HttpClientHandler());
            var weather = client.GetStringAsync("https://api.darksky.net/forecast/b756fbe60e926c034122775956f56ffd/40.1020,-88.2272?exclude=minutely,hourly,flags").Result;
            return JObject.Parse(weather);
        }
    }
}