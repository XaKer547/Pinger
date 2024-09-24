using System.CommandLine;
using System.Net;

namespace Pinger
{
    public class PingCommand : RootCommand
    {
        private readonly IPingService _pingService;

        public PingCommand(IPingService pingService)
        {
            Description = "pings ip in given interval";

            _pingService = pingService;

            Argument<IPAddress> beginIpArg = new();
            Argument<IPAddress> endIpArg = new();

            AddArgument(beginIpArg);
            AddArgument(endIpArg);

            AddValidator(result =>
            {
                var beginIp = result.GetValueForArgument(beginIpArg);

                if (beginIp is null)
                {
                    result.ErrorMessage = "The first IP address could not be identified";
                    return;
                }

                var endIp = result.GetValueForArgument(endIpArg);

                if (endIp is null)
                {
                    result.ErrorMessage = "The second IP address could not be identified";
                    return;
                }

                if (beginIp.AddressFamily != endIp.AddressFamily)
                {
                    result.ErrorMessage = "Given addresses are different types";
                    return;
                }
            });

            this.SetHandler(async (beginIp, endIp) =>
            {
                if (beginIp.GetAddressBytes().Length == 4)
                {
                    await _pingService.RunIPv4Ping(beginIp, endIp);
                }
                else
                {
                    throw new NotImplementedException();
                    //await _pingService.RunIPv6Ping(beginIp, endIp);
                }
            }, beginIpArg, endIpArg);
        }
    }
}