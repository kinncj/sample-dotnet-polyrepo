namespace Api.Configuration;
using Microsoft.Extensions.Configuration;

public class Configuration
{
    [ConfigurationKeyName("KAFKA_BOOTSTRAP_SERVERS")]
    public String bootstrapServers { get; init; }

    [ConfigurationKeyName("CONSUME_TOPIC")]
    public String consumeTopic { get; init; }
    
    [ConfigurationKeyName("REPLY_TOPIC")]
    public String replyTopic { get; init; }
}