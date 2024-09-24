using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace Pinger
{
    public class Program
    {
        const ConsoleColor BaseColor = ConsoleColor.Green;
        static async Task Main(string[] args)
        {
            //SetBaseColor();

            var startup = new Startup();

            var pingService = startup.Services.GetRequiredService<IPingService>();

            var root = new PingCommand(pingService);

            await root.InvokeAsync(args);
        }

        static void SetBaseColor() => Console.ForegroundColor = BaseColor;
    }
}