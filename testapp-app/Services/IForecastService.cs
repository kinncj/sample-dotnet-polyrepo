using Shared;

namespace App.Services;


public interface IForecastService
{
    public IEnumerable<WeatherForecast> GetForecast();
}