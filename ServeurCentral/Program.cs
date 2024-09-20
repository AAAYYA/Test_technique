using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ServeurCentral
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ServeurCentral is running...");
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel()) {
                channel.ExchangeDeclare(exchange: "audioExchange", type: "fanout");
                channel.QueueDeclare(queue: "audioQueue",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) => {
                    var body = ea.Body.ToArray();
                    channel.BasicPublish(exchange: "audioExchange",
                                        routingKey: "",
                                        basicProperties: null,
                                        body: body);
                    Console.WriteLine("Forwarded audio data to all ÉlémentSonore instances.");
                };
                channel.BasicConsume(queue: "audioQueue",
                                    autoAck: true,
                                    consumer: consumer);
                Console.WriteLine("Press [Enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
