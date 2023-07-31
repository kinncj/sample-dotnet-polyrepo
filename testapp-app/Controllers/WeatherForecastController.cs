using App.Services;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace App.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IForecastService _forecastService;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        IForecastService forecastService) {
        _logger = logger;
        _forecastService = forecastService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return _forecastService.GetForecast();
    }
}