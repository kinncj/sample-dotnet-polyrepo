using System.Text.Json;
using Api.Configuration;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Shared;

namespace Api.Services;

public class SampleConsumer: IHostedService
{
    private Configuration.Configuration _configuration;
    private Confluent.Kafka.IProducer<Null, String> _producer;
    private Confluent.Kafka.IConsumer<Null, String> _consumer;
    private readonly IWeatherForecastService<WeatherForecast> _weatherForecastService;
    private readonly ILogger<SampleConsumer> _logger;


    public SampleConsumer(
        IOptions<Configuration.Configuration> configuration,
        IWeatherForecastService<WeatherForecast> weatherForecastService,
        ILogger<SampleConsumer> logger
    )
    {
        new KafkaAdmin(configuration);
        _logger = logger;
        _configuration = configuration.Value;
        _producer = new ProducerBuilder<Null, String>(
            new ProducerConfig { BootstrapServers = _configuration.bootstrapServers }
        ).Build();
        _consumer = new ConsumerBuilder<Null, String>(
            new ConsumerConfig
            {
                BootstrapServers = _configuration.bootstrapServers,
                GroupId = "testapp.testapp-api"
            }
        ).Build();

        _weatherForecastService = weatherForecastService;
    }

    protected async void execute()
    {
        await Task.Yield();
        _consumer.Subscribe(_configuration.consumeTopic);
        while (true)
        {
            ConsumeResult<Null, String> consumerResult = _consumer.Consume();
            byte[] uuid = consumerResult.Message.Headers.GetLastBytes("Correlation-ID");
            byte[] replyToTopic = consumerResult.Message.Headers.GetLastBytes("Reply-To");
            String replyToTopicString = System.Text.Encoding.Unicode.GetString(replyToTopic);
            
            IEnumerable<WeatherForecast> response = _weatherForecastService.execute();
            Headers header = new Headers();
            header.Add("Correlation-ID", uuid) ;
            Message<Null, String> kafkaMessage = new Message<
                Null,
                String>
            { Headers = header, Value = JsonSerializer.Serialize(response)};

            _producer.Produce(replyToTopicString, kafkaMessage);
        }
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        execute();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _consumer.Close();
        return Task.CompletedTask;
    }
}