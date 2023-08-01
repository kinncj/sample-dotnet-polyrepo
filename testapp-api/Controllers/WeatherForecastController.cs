using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Api.Controllers;

[ApiController]
//[Route("[controller]")]
[Route("weather/forecast", Name = "WeatherForecastController")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastService<WeatherForecast> _forecastService;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        IWeatherForecastService<WeatherForecast> forecastService
    ) {
        _logger = logger;
        _forecastService = forecastService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return _forecastService.execute();
    }
}