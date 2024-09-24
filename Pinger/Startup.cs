using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Pinger
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IServiceProvider provider;
        public Startup()
        {
            configuration = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddEnvironmentVariables().Build();

            var services = new ServiceCollection();

            services.AddScoped<IPingService, PingService>();

            services.AddScoped(configure =>
            {
                var loggerFactory = LoggerFactory.Create(builder => builder.AddSimpleConsole(options =>
                {
                    options.SingleLine = true;
                }));

                return loggerFactory.CreateLogger<Program>();
            });

            provider = services.BuildServiceProvider();
        }

        public IServiceProvider Services => provider;
        public IConfiguration Configuration => configuration;
    }
}
