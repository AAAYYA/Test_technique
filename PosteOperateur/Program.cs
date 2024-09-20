using System;
using System.Diagnostics;
using System.IO;
using RabbitMQ.Client;

namespace PosteOperateur
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Select mode: 1 - Record from microphone, 2 - Load audio file");
            var choice = Console.ReadLine();
            byte[] audioData;
            if (choice == "1")
                audioData = record_from_microphone();
            else if (choice == "2")
                audioData = load_audio_file();
            else {
                Console.WriteLine("Invalid choice.");
                return;
            }
            send_audio_to_server(audioData);
        }

        static byte[] record_from_microphone()
        {
            string outputFile = "recordedAudio.wav";
            var process = new Process();
            process.StartInfo.FileName = "arecord";
            process.StartInfo.Arguments = $"-f cd -t wav {outputFile}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            Console.WriteLine("Recording... Press Enter to stop.");
            process.Start();
            Console.ReadLine();
            process.Kill();
            process.WaitForExit();
            Console.WriteLine($"Recording saved as {outputFile}");
            return File.ReadAllBytes(outputFile);
        }

        static byte[] load_audio_file()
        {
            Console.WriteLine("Enter the file path to load the audio:");
            var filePath = Console.ReadLine();
            if (File.Exists(filePath)) {
                var audioData = File.ReadAllBytes(filePath);
                Console.WriteLine($"Audio file {filePath} loaded, size: {audioData.Length} bytes.");
                return audioData;
            } else {
                Console.WriteLine("File not found.");
                return null;
            }
        }

        static void send_audio_to_server(byte[] audioData)
        {
            if (audioData == null) {
                Console.WriteLine("No audio data to send.");
                return;
            }
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel()) {
                channel.QueueDeclare(queue: "audioQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.BasicPublish(exchange: "", routingKey: "audioQueue", basicProperties: null, body: audioData);
                Console.WriteLine(" [x] Sent audio data to the server.");
            }
        }
    }
}
