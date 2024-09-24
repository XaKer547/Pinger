using System.Net;

namespace Pinger
{
    public interface IPingService
    {
        Task RunIPv4Ping(IPAddress addressStart, IPAddress addressEnd);
        Task RunIPv6Ping(IPAddress addressStart, IPAddress addressEnd);
    }
}
