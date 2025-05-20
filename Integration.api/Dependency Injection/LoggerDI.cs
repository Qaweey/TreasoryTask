using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

namespace Integration.api.Dependency_Injection
{
   
        public static class LoggerDI
        {
            public static Logger AddLogger(this WebApplicationBuilder builder)
            {
                var configuration = new ConfigurationBuilder()
               .SetBasePath(builder.Environment.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
               .Build();

                var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
                builder.Logging.ClearProviders();

                return logger;
            }
        }

    
}
