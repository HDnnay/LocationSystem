using LocationSystem.Api.Middlewares;
using LocationSystem.Application;
using LocationSystem.Infrastructure;
using Microsoft.OpenApi;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 添加CORS服务配置
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173/").AllowAnyHeader().AllowAnyMethod().AllowCredentials(); // 允许前端应用的来源
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LocationSystem API",
        Version = "v1",
        Description = "LocationSystem",
        Contact = new OpenApiContact { Name = "LocationSystem", Email = "team@example.com" }
    });
});
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Sys_";
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LocationSystem API V1");
    });
}
app.UseCustomExceptionHandler();
app.UseCors("AllowFrontend"); // 使用CORS策略
app.UseAuthorization();

app.MapControllers();

app.Run();
