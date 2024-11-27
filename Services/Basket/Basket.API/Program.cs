using Asp.Versioning;
using Basket.Application.Basket.Queries;
using Basket.Application.GrpcServices;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Common.Logging;
using Discount.Grpc.Protos;
using MassTransit;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApiVersioning(opts =>
{
    opts.ReportApiVersions = true;
    opts.AssumeDefaultVersionWhenUnspecified = true;
    opts.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
})
.AddApiExplorer(opts =>
{
    opts.GroupNameFormat = "'v'VVV";
    opts.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Basket.API",
        Version = "v1"
    });
    opts.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Basket.API",
        Version = "v2"
    });

    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if(File.Exists(xmlPath))
    {
        opts.IncludeXmlComments(xmlPath);
    }

    // Configure swagger to use versioning
    opts.DocInclusionPredicate((version, apiDescription) =>
    {
        if(!apiDescription.TryGetMethodInfo(out var methodInfo))
            return false;

        var versions = methodInfo.DeclaringType?
            .GetCustomAttributes(true)
            .OfType<ApiVersionAttribute>()
            .SelectMany(att => att.Versions)
            .Union(
                methodInfo.DeclaringType?.BaseType?
                .GetCustomAttributes(true)
                .OfType<ApiVersionAttribute>()
                .SelectMany(att => att.Versions) ?? Enumerable.Empty<ApiVersion>());

        return versions?.Any(v => $"v{v.ToString()}" == version) ?? false;
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
    app.UseSwaggerUI(cfg =>
    {
        cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API V1");
        cfg.SwaggerEndpoint("/swagger/v2/swagger.json", "Basket.API V2");
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
