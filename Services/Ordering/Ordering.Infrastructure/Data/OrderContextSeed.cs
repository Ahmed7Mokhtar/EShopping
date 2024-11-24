using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext context, ILogger<OrderContextSeed> logger)
        {
            if (!context.Orders.Any())
            {
                context.Orders.AddRange(GetOrders());
                await context.SaveChangesAsync();
                logger.LogInformation($"Ordering database: {typeof(OrderContext).Name} seeded!!!");
            }
        }

        private static IEnumerable<Order> GetOrders()
        {
            return new List<Order>
            {
                new()
                {
                    UserName = "Rooney",
                    FirstName = "Ahmed",
                    LastName = "Mokhtar",
                    Email = "rooneya250@gmail.com",
                    AddressLine = "Cairo",
                    Country = "Egypt",
                    TotalPrice = 750,
                    State = "Cairo",
                    Zipcode = "111111",
                    CardName = "Visa",
                    CardNumber = "1234567890123456",
                    Expiration = "12/25",
                    Cvv = "123",
                    PaymentMethod = 1,
                    CreatedBy = "Rooney",
                    CreatedAt = DateTimeOffset.UtcNow,
                    LastModifiedBy = "Rooney",
                    LastModifiedAt = DateTimeOffset.UtcNow,
                }
            };
        }
    }
}
