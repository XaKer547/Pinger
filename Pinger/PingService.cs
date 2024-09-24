using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.NetworkInformation;

namespace Pinger
{
    public class PingService(ILogger<Program> logger) : IPingService
    {
        private readonly ILogger logger = logger;

        public async Task RunIPv4Ping(IPAddress addressStart, IPAddress addressEnd)
        {
            using Ping pinger = new();

            var startBytes = addressStart.GetAddressBytes();

            var endBytes = addressEnd.GetAddressBytes();

            while (true)
            {
                var address = string.Join(".", startBytes);

                await PingAsync(pinger, address);

                if (Enumerable.SequenceEqual(startBytes, endBytes))
                    break;

                TryIncrement(3, ref startBytes);
            }
        }
        public async Task RunIPv6Ping(IPAddress addressStart, IPAddress addressEnd)
        {
            using Ping pinger = new();

            for (int i = 235; i < 256; i++)
            {
                var address = $"{12}.{i}";

                await PingAsync(pinger, address);
            }
        }

        private void TryIncrement(int index, ref byte[] array)
        {
            if (index < 0 || index >= array.Length)
                return;

            if (array[index] == byte.MaxValue)
            {
                array[index] = 0;

                TryIncrement(index - 1, ref array);
            }
            else array[index]++;
        }
        private async Task PingAsync(Ping pinger, string address)
        {
            var reply = await pinger.SendPingAsync(address, 3);

            if (reply.Status == IPStatus.Success)
            {
                logger.LogInformation($"{address} accessed");
            }
            else
            {
                logger.LogError($"{address} unavaible");
            }
        }
    }
}