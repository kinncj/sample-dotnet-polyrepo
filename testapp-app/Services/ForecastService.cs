using App.Configuration;
using Microsoft.Extensions.Options;
using Shared;

namespace App.Services;


public class ForecastService : IForecastService
{
    private readonly HttpClient _httpClient;
    private readonly ForecastConfiguration _forecastConfiguration;


    public ForecastService(HttpClient httpClient, IOptions<ForecastConfiguration> forecastConfiguration)
    { 
        httpClient.BaseAddress = forecastConfiguration.Value.BaseUri;
        _httpClient = httpClient;
    }

    public IEnumerable<WeatherForecast> GetForecast()
    {
        var response = _httpClient
            .GetFromJsonAsync<IEnumerable<WeatherForecast>>("/weather/forecast");

        return response.Result;
    }
}