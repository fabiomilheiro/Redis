using System;
using System.Diagnostics;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RedisTest.Shared
{
    public class Subscriber
    {
        private static readonly object LockPad = new object();

        public async Task SubscribeAsync(int subscriberId)
        {
            SetDefaultColors();
            WriteLine($"Subscriber {subscriberId} starting...");

            var redis = await ConnectionMultiplexer.ConnectAsync("localhost");

            var db = redis.GetDatabase();
            var numberOfMessagesReceived = 0;
            var stopwatch = Stopwatch.StartNew();

            redis
                .GetSubscriber()
                .Subscribe("messages")
                .OnMessage(channelMessage =>
                {
                    numberOfMessagesReceived += 1;

                    //WriteLine(
                    //    $"Received message: {channelMessage.Message} ({channelMessage.Channel}).");

                    if (numberOfMessagesReceived % 10 == 0)
                    {
                        var messagePerSecond =
                            Math.Round(numberOfMessagesReceived / stopwatch.Elapsed.TotalSeconds,
                                1);
                        WriteLineHighlight($"{messagePerSecond} messages/second.");
                    }
                });

            Console.ReadKey();
        }

        private static void WriteLine(string text)
        {
            lock (LockPad)
            {
                Console.WriteLine(text);
            }
        }

        private static void WriteLineHighlight(string text)
        {
            lock (LockPad)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;

                Console.WriteLine(text);

                SetDefaultColors();
            }
        }

        private static void SetDefaultColors()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}