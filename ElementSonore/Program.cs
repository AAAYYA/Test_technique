using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.IO;

namespace ElementSonore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ÉlémentSonore is running...");
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel()) {
                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName, exchange: "audioExchange", routingKey: "");
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) => {
                    var body = ea.Body.ToArray();
                    Console.WriteLine($"Received audio data, size: {body.Length} bytes.");
                    string tempAudioFile = "receivedAudio.wav";
                    File.WriteAllBytes(tempAudioFile, body);
                    play_audio(tempAudioFile);
                };
                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                Console.WriteLine("Press [Enter] to exit.");
                Console.ReadLine();
            }
        }

        static void play_audio(string filePath)
        {
            try {
                Console.WriteLine($"Attempting to play audio file: {filePath}");
                var process = new Process();
                process.StartInfo.FileName = "aplay";
                process.StartInfo.Arguments = filePath;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();
                Console.WriteLine("Audio playback finished.");
            }
            catch (Exception ex) {
                Console.WriteLine($"Error playing audio: {ex.Message}");
            }
        }
    }
}
