using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Threading;

namespace Ordering.API.Extensions
{
    public static class DbExtension
    {
        public static async Task<IHost> MigrateDatabaseSync<TContext>(this IHost host, Func<TContext, IServiceProvider, Task> seeder) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetRequiredService<TContext>();

            try
            {
                logger.LogInformation($"Started DB Migration: {typeof(TContext).Name}");

                // retry strategy
                //var retry = Policy.Handle<SqlException>()
                //    .WaitAndRetry(
                //        retryCount: 5,
                //        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                //        onRetry: (exception, span, retryCount) =>
                //        {
                //            logger?.LogWarning($"Retry {retryCount} encountered an error: {exception.Message}. Retrying in {span}...");
                //        }
                //    );
                //retry.Execute(() => CallSeeder<TContext>(seeder, context, services));

                var retry = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        var logger = context["Logger"] as ILogger;
                        logger?.LogWarning($"Retry {retryCount} encountered an error: {exception.Message}. Retrying in {timeSpan}...");
                    });

                await retry.ExecuteAsync(async () =>
                {
                    // Migrate the database
                    await context.Database.MigrateAsync();

                    // Call the seeder
                    await seeder(context, services);
                });

                logger.LogInformation($"Migration completed: {typeof(TContext).Name}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occured while migrating DB: {typeof(TContext).Name}");
            }

            return host;
        }

        private static async Task CallSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            await context.Database.MigrateAsync();

            seeder(context, services);
        }
    }
}
