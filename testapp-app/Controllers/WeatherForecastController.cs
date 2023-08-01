using App.Services;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace App.Controllers;

[ApiController]
[Route("weather/forecast", Name = "WeatherForecastController")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IForecastService _forecastService;
    private readonly ISampleProducer<WeatherForecast, IEnumerable<WeatherForecast>> _sampleProducer;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        IForecastService forecastService,
        ISampleProducer<WeatherForecast, IEnumerable<WeatherForecast>> sampleProducer) {
        _logger = logger;
        _forecastService = forecastService;
        _sampleProducer = sampleProducer;
    }

    [HttpGet("/sync")]
    public IEnumerable<WeatherForecast> GetSync()
    {
        return _forecastService.GetForecast();
    }
    
    [HttpGet("/async")]
    public IEnumerable<WeatherForecast> GetAsync()
    {
        return _sampleProducer.send(new WeatherForecast());
    }
}