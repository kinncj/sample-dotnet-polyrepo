namespace App.Configuration;
using Microsoft.Extensions.Configuration;

public class Configuration
{
    [ConfigurationKeyName("FORECAST_SERVICE")]
    public Uri ForecastUri { get; init; }
    
    [ConfigurationKeyName("KAFKA_BOOTSTRAP_SERVERS")]
    public String bootstrapServers { get; init; }

    [ConfigurationKeyName("SEND_TOPIC")]
    public String sendTopic { get; init; }
    
    [ConfigurationKeyName("CONSUME_TOPIC")]
    public String consumeTopic { get; init; }
}