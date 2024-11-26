using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Common.Logging
{
    public static class Logging
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
            (context, loggerConfigs) =>
            {
                var env = context.HostingEnvironment;
                loggerConfigs.MinimumLevel.Information()
                   .Enrich.FromLogContext()
                   .Enrich.WithProperty("ApplicationName", env.ApplicationName)
                   .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
                   .Enrich.WithExceptionDetails()
                   .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
                   .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Warning)
                   .WriteTo.Console();

                if(context.HostingEnvironment.IsDevelopment())
                {
                    ConfigureDevelopmentLogging(loggerConfigs);
                }
                else
                {
                    //ConfigureProductionLogging(loggerConfigs);
                }

                // Elastic Search
                var elasticUrl = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
                if (!string.IsNullOrEmpty(elasticUrl))
                {
                    loggerConfigs.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUrl))
                    {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                        IndexFormat = "eshopping_logs-{0:yyyy.MM.dd}",  // note: elastic index must be lower case
                        MinimumLogEventLevel = Serilog.Events.LogEventLevel.Debug,
                    });
                }
            };

        /// <summary>
        /// Configures logging for the Development environment.
        /// </summary>
        private static void ConfigureDevelopmentLogging(LoggerConfiguration loggerConfigs)
        {
            loggerConfigs.MinimumLevel.Override("Catalog", Serilog.Events.LogEventLevel.Debug);
            loggerConfigs.MinimumLevel.Override("Basket", Serilog.Events.LogEventLevel.Debug);
            loggerConfigs.MinimumLevel.Override("Discount", Serilog.Events.LogEventLevel.Debug);
            loggerConfigs.MinimumLevel.Override("Ordering", Serilog.Events.LogEventLevel.Debug);
                //.WriteTo.Debug(); // Log to the Debug window in the IDE
                //.WriteTo.File(
                //    path: "logs/development-log-.txt",
                //    rollingInterval: RollingInterval.Day,
                //    retainedFileCountLimit: 7); // Optional: File-based logging for development
        }

        /// <summary>
        /// Configures logging for Production or non-development environments.
        /// </summary>
        private static void ConfigureProductionLogging(LoggerConfiguration loggerConfigs)
        {
            loggerConfigs
                .WriteTo.File(
                    path: "logs/production-log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30, // Retain logs for 30 days
                    fileSizeLimitBytes: 10_000_000, // 10 MB per file
                    rollOnFileSizeLimit: true)
                .WriteTo.Console(); // Maintain console output for real-time monitoring
        }
    }
}
