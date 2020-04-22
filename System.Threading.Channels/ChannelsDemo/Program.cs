using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ChannelsDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = Channel.CreateBounded<int>(2);
            _ = Task.Run(async delegate {
                for (int i = 0; i < 100; i++)
                {
                    await Task.Delay(500);
                    await channel.Writer.WriteAsync(i);
                }
                channel.Writer.Complete();
            });
            await foreach (var item in channel.Reader.ReadAllAsync())
            {
                Console.WriteLine(item);
                await Task.Delay(1000);
            }
        }
    }
}
