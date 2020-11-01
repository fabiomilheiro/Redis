using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RedisTest.Broadcaster
{
    internal class Program
    {
        private static readonly RedisValue MessageContent = File.ReadAllText("MessageContent.json");

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Broadcaster starting...");

            var redis =
                await ConnectionMultiplexer.ConnectAsync("localhost");

            var db = redis.GetDatabase();

            Console.WriteLine("Getting key1 value...");
            var cacheValue = db.StringGet("key1");
            Console.WriteLine($"Old value = {cacheValue.ToString() ?? "Nothing"}");

            Console.WriteLine("Saving key1 value...");
            var newValue = $"ABC-{new Random().Next(int.MaxValue)}";
            db.StringSet("key1", newValue);

            Console.WriteLine("Getting key1 value again...");
            cacheValue = db.StringGet("key1");
            Console.WriteLine($"New value = {cacheValue}");

            var number = 1;
            const int pageSize = 10;

            while (true)
            {

                var tasks = new List<Task>();

                for (var i = number; i < number + pageSize; i++)
                {
                    Console.WriteLine($"Sending message {i}...");
                    tasks.Add(db.PublishAsync("messages", MessageContent));
                }

                await Task.WhenAll(tasks);

                number += pageSize;
            }
        }
    }
}