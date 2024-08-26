using System.Collections.Concurrent;
using TestApi.Model;

namespace TestApi.Classes
{
    public class Queues
    {
        public static ConcurrentQueue<RequestInfo> RequestInfoQueue = new();
    }
}
