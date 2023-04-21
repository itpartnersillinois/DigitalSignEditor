namespace DigitalSignEditor.Weather {

    public class WeatherDay {

        public WeatherDay(dynamic weather, string title) {
            var item = weather.probabilityOfPrecipitation;
            PrecipitationProbability = weather.probabilityOfPrecipitation.value;
            Summary = weather.shortForecast;
            Temperature = weather.temperature;
            Title = title;
            Wind = $"Wind: {weather.windSpeed} {weather.windDirection}";
        }

        public string Precipitation => PrecipitationProbability == 0 ? "" : $"Precipitation: {PrecipitationProbability}% chance";
        public int PrecipitationProbability { get; set; }
        public string Summary { get; set; }
        public int Temperature { get; set; }
        public string Title { get; set; }
        public string Wind { get; set; }

        public static string GetIcon(string s) {
            if (s.ToLowerInvariant().Contains("rain")) {
                return "rain";
            }
            if (s.ToLowerInvariant().Contains("snow")) {
                return "snow";
            }
            if (s.ToLowerInvariant().Contains("wind")) {
                return "wind";
            }
            if (s.ToLowerInvariant().Contains("sleet")) {
                return "sleet";
            }
            if (s.ToLowerInvariant().Contains("snow")) {
                return "snow";
            }
            if (s.ToLowerInvariant().Contains("fog")) {
                return "fog";
            }
            if (s.ToLowerInvariant().Contains("clear")) {
                return "clear";
            }
            if (s.ToLowerInvariant().Contains("cloud")) {
                return "cloudy";
            }
            return string.Empty;
        }
    }
}