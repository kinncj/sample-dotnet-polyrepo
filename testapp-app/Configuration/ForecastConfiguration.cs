namespace App.Configuration;
using Microsoft.Extensions.Configuration;

public class ForecastConfiguration
{
    [ConfigurationKeyName("FORECAST_SERVICE")]
    public Uri BaseUri { get; init; }
}