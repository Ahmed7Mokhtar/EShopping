using Basket.Application.Basket.Queries;
using Basket.Application.GrpcServices;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Common.Logging;
using Discount.Grpc.Protos;
using MassTransit;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Serilog Config
builder.Host.UseSerilog(Logging.ConfigureLogger);

builder.Services.AddControllers();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<DiscountGrpcService>();
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(cfg =>
{
    cfg.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]);
});

builder.Services.AddApiVersioning(opts =>
{
    opts.ReportApiVersions = true;
    opts.AssumeDefaultVersionWhenUnspecified = true;
    opts.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Basket.API",
        Version = "v1"
    });
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR(opt => opt.RegisterServicesFromAssembly(typeof(GetBasketByUserNameQueryHandler).Assembly));

// Redis
builder.Services.AddStackExchangeRedisCache(opts =>
{
    opts.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((brc, brf) =>
    {
        brf.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});

var app = builder.Build();

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
