using System;
using System.Threading;
using Confluent.Kafka;

public class Kafka
{
    private string _topicName;
    private ConsumerConfig _conf;

    public Kafka(string topicName, ConsumerConfig conf)
    {
        _topicName = topicName;
        _conf = conf;

    }

    public void StartConsuming(Action<string> function)
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
                        function(value);
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

    public async Task Produce(string value)
    {
        using (var p = new ProducerBuilder<Null, string>(_conf).Build())
        {
            try
            {
                var dr = await p.ProduceAsync(_topicName, new Message<Null, string> { Value = value});
                Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
        }
    }
}
