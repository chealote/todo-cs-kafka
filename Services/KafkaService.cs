using System;
using System.Threading;
using Confluent.Kafka;
using TodoApi.Services.Interfaces;

namespace TodoApi.Services;

public class KafkaService : IKafkaService
{
    // this properties should now come from another service that provides the configuration?
    // how to ingest config variables into a service?
    private string _topicName;
    private ConsumerConfig _conf;

    public KafkaService()
    {
        _topicName = "kafka-topic";
        _conf = new ConsumerConfig {
            GroupId = "kafka-consumer-group",
                    BootstrapServers = "localhost:9092",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
        };
    }

    public void Subscribe(Action<string> Process)
    {
        using (var c = new ConsumerBuilder<Ignore, string>(_conf).Build())
        {
            c.Subscribe(_topicName);

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true;
                cts.Cancel();
            };

            try
            {
                while (true)
                {
                    try
                    {
                        var cr = c.Consume(cts.Token);
                        string value = cr.Message.Value;
                        Process(value);
                        Console.WriteLine($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                c.Close();
            }
        }
    }

    public async Task Produce(string message)
    {
        using (var p = new ProducerBuilder<Null, string>(_conf).Build())
        {
            try
            {
                var dr = await p.ProduceAsync(_topicName, new Message<Null, string> { Value = message});
                Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
        }
    }
}
