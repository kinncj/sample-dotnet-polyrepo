using System.Text.Json;
using App.Configuration;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Shared;

namespace App.Services;

public class SampleProducer: ISampleProducer<WeatherForecast, IEnumerable<WeatherForecast>>
{
    private Configuration.Configuration _configuration;
    private readonly ILogger<SampleProducer> _logger;
    private IProducer<Null, String> _producer;
    private IConsumer<Null, String> _consumer;

    public SampleProducer(
        IOptions<Configuration.Configuration> configuration,
        ILogger<SampleProducer> logger
        )
    {
        _logger = logger;
        _configuration = configuration.Value;
        _producer = new ProducerBuilder<Null, String>(
            new ProducerConfig { BootstrapServers = _configuration.bootstrapServers }
        ).Build();
        _consumer = new ConsumerBuilder<Null, String>(
            new ConsumerConfig
            {
                BootstrapServers = _configuration.bootstrapServers,
                GroupId = "testapp.testapp-app-" + System.Guid.NewGuid().ToString()
            }
        ).Build();
    }

    public IEnumerable<WeatherForecast> send(WeatherForecast message)
    {
        byte[] uuid = System.Guid.NewGuid().ToByteArray();
        Headers header = new Headers();
        header.Add("Correlation-ID", uuid) ;
        header.Add("Reply-To", System.Text.Encoding.Unicode.GetBytes(_configuration.consumeTopic));
        Message<Null, String> kafkaMessage = new Message<Null, String>()
        {
            Headers = header,
            Value = JsonSerializer.Serialize(message)
        };
        _producer.Produce(_configuration.sendTopic, kafkaMessage);
        _consumer.Subscribe(_configuration.consumeTopic);
        while (true)
        {
            ConsumeResult<Null, String> consumerResult = _consumer.Consume();
            byte[] correlationId = consumerResult.Message.Headers.GetLastBytes("Correlation-ID");
            if (uuid.SequenceEqual(correlationId))
            {
                return JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(consumerResult.Message.Value);
            }
        }
    }
}