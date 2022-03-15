namespace DigitalSignEditor.Weather {

    public class WeatherDay {

        public WeatherDay(dynamic weather, string title) {
            Humidity = weather.humidity;
            PrecipitationProbability = weather.precipProbability;
            PrecipitationType = weather.precipType;
            Summary = weather.summary;
            TemperatureHigh = weather.temperatureHigh;
            TemperatureHighFeelsLike = weather.apparentTemperatureHigh;
            TemperatureLow = weather.temperatureLow;
            TemperatureLowFeelsLike = weather.apparentTemperatureLow;
            Title = title;
            WindSpeed = weather.windSpeed;
            WindGustSpeed = weather.windGust;
        }

        public decimal Humidity { get; set; }
        public string Precipitation => string.IsNullOrWhiteSpace(PrecipitationType) || PrecipitationProbability == 0 ? "" : $"Precipitation: {(PrecipitationProbability * 100).ToString("0")}% chance of {PrecipitationType}";
        public decimal PrecipitationProbability { get; set; }
        public string PrecipitationType { get; set; }
        public string Summary { get; set; }
        public int TemperatureHigh { get; set; }
        public int TemperatureHighFeelsLike { get; set; }
        public int TemperatureLow { get; set; }
        public int TemperatureLowFeelsLike { get; set; }
        public string TempHighString => $"High of {TemperatureHigh}F (feels like {TemperatureHighFeelsLike}F)";
        public string TempLowString => $"Low of {TemperatureLow}F (feels like {TemperatureLowFeelsLike}F)";
        public string Title { get; set; }
        public int WindGustSpeed { get; set; }
        public int WindSpeed { get; set; }
    }
}