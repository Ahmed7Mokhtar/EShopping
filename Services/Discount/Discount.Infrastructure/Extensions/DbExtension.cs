using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Infrastructure.Extensions
{
    public static class DbExtension
    {
        private static readonly AsyncRetryPolicy RetryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    var logger = context["Logger"] as ILogger;
                    logger?.LogWarning($"Retry {retryCount} encountered an error: {exception.Message}. Retrying in {timeSpan}...");
                });

        public static async Task<IHost> MigrateDatabaseAsync<TContext>(this IHost host, CancellationToken cancellationToken = default)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var config = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Starting database migration for Discount DB.");

                await RetryPolicy.ExecuteAsync(
                    async (context, token) => await ApplyMigrations(config),
                    new Context("Migration") { ["Logger"] = logger },
                    cancellationToken);

                logger.LogInformation("Database migration completed successfully.");
            }
            catch (Exception ex) when (ex is OperationCanceledException)
            {
                logger.LogWarning("Database migration was canceled.");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the Discount DB.");
                throw;
            }

            return host;
        }

        private static async Task ApplyMigrations(IConfiguration config)
        {
            var connectionString = config.GetValue<string>("DatabaseSettings:ConnectionString");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("The database connection string is not configured.");
            }

            try
            {
                await using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                await using var cmd = new NpgsqlCommand { Connection = connection };
                await ExecuteMigrationCommands(cmd);
            }
            catch (Exception ex)
            {

            }
        }

        private static async Task ExecuteMigrationCommands(NpgsqlCommand cmd)
        {
            await using var transaction = await cmd.Connection.BeginTransactionAsync();

            try
            {
                cmd.Transaction = transaction;

                cmd.CommandText = "DROP TABLE IF EXISTS Coupons";
                await cmd.ExecuteNonQueryAsync();

                cmd.CommandText = @"
                CREATE TABLE Coupons (
                    Id SERIAL PRIMARY KEY,
                    ProductId VARCHAR(500) NOT NULL,
                    Description TEXT,
                    Amount DECIMAL(10, 2) NOT NULL
                )";
                await cmd.ExecuteNonQueryAsync();

                cmd.CommandText = @"
                INSERT INTO Coupons (ProductId, Description, Amount) 
                VALUES 
                ('5f68b8a1670d4a8b903f02c8', 'Product 1 Discount Desc', 50.00),
                ('5f68b8a1670d4a8b903f02cd', 'Product 2 Discount Desc', 60.00)";
                await cmd.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
    //public static class DbExtension
    //{
    //    private static readonly AsyncRetryPolicy RetryPolicy = Policy
    //    .Handle<Exception>() // Retry on any exception
    //    .WaitAndRetryAsync(
    //        retryCount: 3,
    //        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
    //        onRetry: (exception, timeSpan, retryCount, context) =>
    //        {
    //            Console.WriteLine($"Retry {retryCount} encountered an error: {exception.Message}. Retrying in {timeSpan}...");
    //        });

    //    public static IHost MigrateDatabase<TContext>(this IHost host)
    //    {
    //        using var scope = host.Services.CreateScope();
    //        var services = scope.ServiceProvider;
    //        var config = services.GetRequiredService<IConfiguration>();
    //        var logger = services.GetRequiredService<ILogger<TContext>>();

    //        try
    //        {
    //            logger.LogInformation("Starting database migration for Discount DB.");

    //            RetryPolicy.ExecuteAsync(async () => await ApplyMigrations(config)).Wait();

    //            logger.LogInformation("Database migration completed successfully.");
    //        }
    //        catch (Exception ex)
    //        {
    //            logger.LogError(ex, "An error occurred while migrating the Discount DB.");
    //            throw;
    //        }

    //        return host;
    //    }

    //    private static async Task ApplyMigrations(IConfiguration config)
    //    {
    //        var connectionString = config.GetValue<string>("DatabaseSettings:ConnectionString");
    //        if (string.IsNullOrEmpty(connectionString))
    //        {
    //            throw new InvalidOperationException("The database connection string is not configured.");
    //        }

    //        await using var connection = new NpgsqlConnection(connectionString);
    //        await connection.OpenAsync();

    //        await using var cmd = new NpgsqlCommand
    //        {
    //            Connection = connection,
    //        };

    //        try
    //        {
    //            await ExecuteMigrationCommands(cmd);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception("Error occurred while applying migrations.", ex);
    //        }
    //    }

    //    private static async Task ExecuteMigrationCommands(NpgsqlCommand cmd)
    //    {
    //        // Drop the table if it exists
    //        cmd.CommandText = "DROP TABLE IF EXISTS Coupons";
    //        await cmd.ExecuteNonQueryAsync();

    //        // Create the table
    //        cmd.CommandText = @"
    //            CREATE TABLE Coupons (
    //                Id SERIAL PRIMARY KEY,
    //                ProductId VARCHAR(500) NOT NULL,
    //                Description TEXT,
    //                Amount DECIMAL(10, 2) NOT NULL
    //            )";
    //        await cmd.ExecuteNonQueryAsync();

    //        // Insert seed data
    //        cmd.CommandText = @"
    //            INSERT INTO Coupons (ProductId, Description, Amount) 
    //            VALUES 
    //            ('5f68b8a1670d4a8b903f02c8', 'Product 1 Discount Desc', 50.00),
    //            ('5f68b8a1670d4a8b903f02cd', 'Product 2 Discount Desc', 60.00)";
    //        await cmd.ExecuteNonQueryAsync();
    //    }
    //}
}
