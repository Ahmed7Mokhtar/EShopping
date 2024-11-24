using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly string _connectionString;

        public DiscountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString")
                                ?? throw new ArgumentNullException(nameof(configuration), "Connection string is not configured.");
        }

        private NpgsqlConnection GetConnection() => new(_connectionString);

        public async Task<Coupon?> Get(string productId)
        {
            const string query = "SELECT * FROM Coupons WHERE ProductId = @ProductId";

            await using var connection = GetConnection();
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(query, new { ProductId = productId });

            return coupon; // ?? new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Available" };
        }

        public async Task<bool> Create(Coupon coupon)
        {
            const string query = @"
            INSERT INTO Coupons (ProductId, Description, Amount)
            VALUES (@ProductId, @Description, @Amount)";

            await using var connection = GetConnection();
            var affectedRows = await connection.ExecuteAsync(query, coupon);

            return affectedRows > 0;
        }

        public async Task<bool> Update(Coupon coupon)
        {
            const string query = @"
            UPDATE Coupons 
            SET ProductId = @ProductId, Description = @Description, Amount = @Amount 
            WHERE Id = @Id";

            await using var connection = GetConnection();
            var affectedRows = await connection.ExecuteAsync(query, coupon);

            return affectedRows > 0;
        }

        public async Task<bool> Delete(string productId)
        {
            const string query = "DELETE FROM Coupons WHERE ProductId = @ProductId";

            await using var connection = GetConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { ProductName = productId });

            return affectedRows > 0;
        }
    }
}
