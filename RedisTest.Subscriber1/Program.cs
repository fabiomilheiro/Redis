using System.Threading.Tasks;
using RedisTest.Shared;

namespace RedisTest.Subscriber1
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await new Subscriber().SubscribeAsync(1);
        }
    }
}