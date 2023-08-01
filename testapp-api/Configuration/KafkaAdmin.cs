using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Options;

namespace Api.Configuration;

public class KafkaAdmin
{
    private Configuration _configuration;
    public KafkaAdmin(IOptions<Configuration> configuration)
    {
        _configuration = configuration.Value;
        configure();
    }

    public void configure()
    {
        configureTopic(_configuration.replyTopic, _configuration.bootstrapServers);
        configureTopic(_configuration.consumeTopic, _configuration.bootstrapServers);
    }

    private void configureTopic(String topic, String servers)
    {
        using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers =  servers}).Build())
        {
            try
            {
                adminClient.CreateTopicsAsync(new TopicSpecification[] { 
                    new TopicSpecification { Name = topic, ReplicationFactor = 1, NumPartitions = 1 } });
            }
            catch (CreateTopicsException e)
            {
                Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
            }
        }
    }
}