using System.Threading.Tasks;
using RedisTest.Shared;

namespace RedisTest.Subscriber3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new Subscriber().SubscribeAsync(3);
        }
    }
}
