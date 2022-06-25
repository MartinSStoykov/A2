using A2.BL.Interfaces;
using A2.Models;
using Confluent.Kafka;
using System.Threading.Tasks;

namespace A2.BL.Services
{
    public class KafkaProducer : IKafkaProducer
    {
        private IProducer<int, Motor> _producer;

        public KafkaProducer()
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092"
            };

            _producer = new ProducerBuilder<int, Motor>(config)
                            .SetValueSerializer(new MsgPackSerializer<Motor>())
                            .Build();
        }
        public async Task ProduceMotor(Motor motor)
        {
            var result = await _producer.ProduceAsync("Motor", new Message<int, Motor>()
            {
                Key = motor.Id,
                Value = motor
            });
        }
    }
}