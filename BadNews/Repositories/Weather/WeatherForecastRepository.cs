using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace BadNews.Repositories.Weather
{
    public class WeatherForecastRepository : IWeatherForecastRepository
    {
        private const string defaultWeatherImageUrl = "/images/cloudy.png";

        private readonly Random random = new Random();

        private string apiKey;

        public WeatherForecastRepository(IOptions<OpenWeatherOptions> weatherOptions)
        {
            this.apiKey = weatherOptions?.Value.ApiKey;
        }

        public async Task<WeatherForecast> GetWeatherForecastAsync()
        {
            var client = new OpenWeatherClient(apiKey);
            try
            {
                var weather = await client.GetWeatherFromApiAsync();
                return WeatherForecast.CreateFrom(weather);
            }
            catch (Exception e)
            {
                return BuildRandomForecast();
            }
        }

        private WeatherForecast BuildRandomForecast()
        {
            var temperature = random.Next(-20, 20 + 1);
            return new WeatherForecast
            {
                TemperatureInCelsius = temperature,
                IconUrl = defaultWeatherImageUrl
            };
        }
    }
}
