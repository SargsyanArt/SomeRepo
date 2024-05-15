using Confluent.Kafka;

namespace Some.Web.Services
{
    public class KafkaProducer
    {
        private readonly ProducerConfig _config;

        public KafkaProducer(string bootstrapServers)
        {
            _config = new ProducerConfig { BootstrapServers = bootstrapServers };
        }

        public async Task<Tuple<bool, string>> ProduceAsync(string topic, string message)
        {
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                try
                {
                    var deliveryReport = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
                    string resultMessage = $"Delivered message '{deliveryReport.Value}' to '{deliveryReport.TopicPartitionOffset}'";
                    Console.WriteLine(message);
                    return Tuple.Create(true, message);
                }
                catch (ProduceException<Null, string> e)
                {
                    string resultMessage = $"Delivery failed: {e.Error.Reason}";
                    Console.WriteLine(resultMessage);
                    return Tuple.Create(false, resultMessage);
                }
            }
        }
    }
}
