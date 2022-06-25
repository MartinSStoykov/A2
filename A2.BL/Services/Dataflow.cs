using A2.BL.Interfaces;
using A2.Models;
using MessagePack;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace A2.BL.Services
{
    public class Dataflow : IDataflow
    {
        private IKafkaProducer _producer;


        TransformBlock<byte[], Motor> entryBlock = new TransformBlock<byte[], Motor>(data => MessagePackSerializer.Deserialize<Motor>(data));

        Random rnd = new Random();

        public Dataflow(IKafkaProducer producer)
        {
            _producer = producer;

            var enrichBlock = new TransformBlock<Motor, Motor>(c =>
            {
                Console.WriteLine($"Received value: {c.Year}");
                c.Year = rnd.Next(1980, DateTime.Now.Year);

                return c;
            });

            var publishBlock = new ActionBlock<Motor>(motor =>
            {
                Console.WriteLine($"Updated value: {motor.Year} \n");
                _producer.ProduceMotor(motor);
            });

            var linkOptions = new DataflowLinkOptions()
            {
                PropagateCompletion = true
            };

            entryBlock.LinkTo(enrichBlock, linkOptions);
            enrichBlock.LinkTo(publishBlock, linkOptions);

        }
        public async Task SendMotor(byte[] data)
        {
            await entryBlock.SendAsync(data);
        }
    }
}