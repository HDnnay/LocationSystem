using LocationSystem.Api.BackgroudServices;
using LocationSystem.Api.Middlewares;
using LocationSystem.Application;
using LocationSystem.Application.Utilities.RabbitMQs;
using LocationSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 添加CORS服务配置
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173/", "http://localhost:5174/").AllowAnyHeader().AllowAnyMethod().AllowCredentials(); // 允许前端应用的来源
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
    options.JsonSerializerOptions.IncludeFields = true;

    options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

    //options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
    //options.JsonSerializerOptions.Converters.Add(new NullableDateTimeJsonConverter());
}); ;
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

builder.Services.AddOpenApi();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Sys_";
});

// 1️⃣ 注册 RabbitMQ 服务（单例）
builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();

// 2️⃣ 注册消费者后台服务
builder.Services.AddHostedService<RabbitMQTestService>();
builder.Services.AddHostedService<DatabaseInitializerServices>();
var app = builder.Build();

// 4️⃣ 应用启动时，确保服务已启动
app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("✅ 应用已启动，所有后台服务正在运行");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.AddDocument("v1", "Production API", "api/v1/openapi.json")
           .AddDocument("v2-beta", "Beta API", "api/v2-beta/openapi.json", isDefault: true)
           .AddDocument("internal", "Internal API", "internal/openapi.json");
    });
}
app.UseCustomExceptionHandler();
app.UseCors("AllowFrontend"); // 使用CORS策略
app.UseAuthorization();

app.MapControllers();

app.Run();
