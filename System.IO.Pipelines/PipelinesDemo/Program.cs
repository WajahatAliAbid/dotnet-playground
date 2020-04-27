using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KenHaise.Core.Extensions;

namespace PipelinesDemo
{
    class Program
    {
        static byte[] StreamDelimterBytes { get => new[] { EncodingBytes.FILESEPERATOR, EncodingBytes.CARRIAGERETURN }; }
        static async Task Main(string[] args)
        {
            var pipe = new Pipe();
            var producer = Task.Run(async delegate
            {
                var messages = new[]
                {
                    "A quick brown fox jumps over the lazy dog",
                    "Today is a rainy day",
                    "We are not going to school today",
                    "C# is an interesting language"
                };
                var writer = pipe.Writer;
                var random = new Random();
                for (int i = 0; i < 10; i++)
                {
                    var message = messages[random.Next(0, 3)];
                    var bytes = new List<byte>();
                    bytes.AddRange(Encoding.ASCII.GetBytes(message));
                    bytes.AddRange(StreamDelimterBytes);
                    await writer.WriteAsync(bytes.ToArray().AsMemory());
                }
            });
            var consumer = Task.Run(async delegate
            {
                var reader = pipe.Reader;
                var result = await reader.ReadAsync();
                Console.WriteLine(Encoding.ASCII.GetString(result.Buffer.ToArray().TrimEnd(StreamDelimterBytes).ToArray()));
            });
            await Task.WhenAll(producer, consumer);
            Console.WriteLine("Hello World!");
        }

        public bool IsMessageComplete(IEnumerable<byte> streamBytes) =>
            streamBytes.EndsWith(StreamDelimterBytes);
    }
    public static class EncodingBytes
    {
        /* 
        * Reperesents 1C character in Hex
        * Equivalent to Convert.ToByte("1C",16);
        */
        // public const byte FILESEPERATOR =28;

        public static byte FILESEPERATOR { get => Convert.ToByte("1C", 16); }
        /* 
        * Reperesents 0D character in Hex
        * Equivalent to Convert.ToByte("0D",16);
        */
        public static byte CARRIAGERETURN { get => Convert.ToByte("0D", 16); }

        /* 
        * Reperesents 0B character in Hex
        * Equivalent to Convert.ToByte("0B",16);
        */
        public static byte VERTICALTAB { get => Convert.ToByte("0B", 16); }
    }
}
