using Common.Logging;
using EventBus.Messages.Common;
using MassTransit;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Serilog Config
builder.Host.UseSerilog(Logging.ConfigureLogger);

builder.Services.AddControllers();

builder.Services.AddApplicationServices();  // App Services
builder.Services.AddInfraServices(builder.Configuration);    // Infrastructure services
//builder.Services.AddScoped<BasketOrderingConsumer>();

builder.Services.AddApiVersioning(opt =>
{
    opt.ReportApiVersions = true;
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Ordering.API",
        Version = "v1"
    });
});

builder.Services.AddMassTransit(cfg =>
{
    // mark as consumer
    cfg.AddConsumer<BasketOrderingConsumer>();
    cfg.UsingRabbitMq((ctx, rbc) =>
    {
        rbc.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        // provide the queue name with consumer settings
        rbc.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketOrderingConsumer>(ctx);
        });
    });
});

var app = builder.Build();

await app.MigrateDatabaseSync<OrderContext>(async (context, services) =>
{
    var logger = services.GetRequiredService<ILogger<OrderContextSeed>>();
    await OrderContextSeed.SeedAsync(context, logger);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
