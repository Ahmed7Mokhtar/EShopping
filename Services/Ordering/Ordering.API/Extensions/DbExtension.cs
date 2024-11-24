using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;

namespace Ordering.API.Extensions
{
    public static class DbExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetRequiredService<TContext>();

            try
            {
                logger.LogInformation($"Started DB Migration: {typeof(TContext).Name}");
                
                // retry strategy
                var retry = Policy.Handle<SqlException>()
                    .WaitAndRetry(
                        retryCount: 5,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (exception, span, retryCount) =>
                        {
                            logger?.LogWarning($"Retry {retryCount} encountered an error: {exception.Message}. Retrying in {span}...");
                        }
                    );
                retry.Execute(() => CallSeeder<TContext>(seeder, context, services));

                logger.LogInformation($"Migration completed: {typeof(TContext).Name}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occured while migrating DB: {typeof(TContext).Name}");
            }

            return host;
        }

        private static void CallSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();

            seeder(context, services);
        }
    }
}
