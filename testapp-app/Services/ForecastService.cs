using App.Configuration;
using Microsoft.Extensions.Options;
using Shared;

namespace App.Services;


public class ForecastService : IForecastService
{
    private readonly HttpClient _httpClient;
    private readonly Configuration.Configuration _configuration;


    public ForecastService(HttpClient httpClient, IOptions<Configuration.Configuration> configuration)
    { 
        httpClient.BaseAddress = configuration.Value.ForecastUri;
        _httpClient = httpClient;
    }

    public IEnumerable<WeatherForecast> GetForecast()
    {
        var response = _httpClient
            .GetFromJsonAsync<IEnumerable<WeatherForecast>>("/weather/forecast");

        return response.Result;
    }
}