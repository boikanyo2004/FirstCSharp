using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WeatherApp
{
    public class WeatherService
    {
        private readonly string _apiKey = "e183ef3e8293d93d15cb95982b76d7cb"; // Get from https://openweathermap.org/
        private readonly HttpClient _httpClient;
        
        public WeatherService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<WeatherData> GetWeatherAsync(string city)
        {
            try
            {
                string url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric";
                var response = await _httpClient.GetStringAsync(url);
                var json = JObject.Parse(response);
                
                var weatherData = new WeatherData
                {
                    City = json["name"]?.ToString() + ", " + json["sys"]?["country"]?.ToString(),
                    Temperature = Convert.ToDouble(json["main"]?["temp"]),
                    FeelsLike = Convert.ToDouble(json["main"]?["feels_like"]),
                    Humidity = Convert.ToInt32(json["main"]?["humidity"]),
                    Pressure = Convert.ToInt32(json["main"]?["pressure"]),
                    WindSpeed = Convert.ToDouble(json["wind"]?["speed"]),
                    WeatherCondition = json["weather"]?[0]?["main"]?.ToString(),
                    WeatherDescription = json["weather"]?[0]?["description"]?.ToString(),
                    WeatherIcon = json["weather"]?[0]?["icon"]?.ToString(),
                    Cloudiness = Convert.ToInt32(json["clouds"]?["all"]),
                    Visibility = Convert.ToInt32(json["visibility"])
                };

                // Get sunrise/sunset if available
                if (json["sys"]?["sunrise"] != null)
                    weatherData.Sunrise = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(json["sys"]?["sunrise"])).LocalDateTime;
                
                if (json["sys"]?["sunset"] != null)
                    weatherData.Sunset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(json["sys"]?["sunset"])).LocalDateTime;

                return weatherData;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching weather data: {ex.Message}");
            }
        }
    }

    public class WeatherData
    {
        public string City { get; set; } = "";
        public double Temperature { get; set; }
        public double FeelsLike { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }
        public double WindSpeed { get; set; }
        public string WeatherCondition { get; set; } = "";
        public string WeatherDescription { get; set; } = "";
        public string WeatherIcon { get; set; } = "";
        public int Cloudiness { get; set; }
        public int Visibility { get; set; }
        public DateTime? Sunrise { get; set; }
        public DateTime? Sunset { get; set; }
    }
}