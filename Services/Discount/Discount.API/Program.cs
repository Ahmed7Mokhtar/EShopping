using Common.Logging;
using Discount.API.Services;
using Discount.Application.Queries;
using Discount.Core.Repositories;
using Discount.Infrastructure.Extensions;
using Discount.Infrastructure.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Serilog Config
builder.Host.UseSerilog(Logging.ConfigureLogger);

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR(opt => opt.RegisterServicesFromAssembly(typeof(GetDiscountQueryHandler).Assembly));

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

builder.Services.AddGrpc();

var app = builder.Build();

// Migrate database
await app.MigrateDatabaseAsync<Program>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapGrpcService<DiscountService>();

app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("This service supports gRPC communication. Use a gRPC client to access the endpoints.");
});

app.Run();
