using AspNetCoreRateLimit;
using LocationSystem.Api.BackgroudServices;
using LocationSystem.Api.Middlewares;
using LocationSystem.Application;
using LocationSystem.Application.Utilities.Jwt;
using LocationSystem.Application.Utilities.RabbitMQs;
using LocationSystem.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text;

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

//// 加载限流配置
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
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
#region 使用Redis存储
builder.Services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
#endregion
#region open api
builder.Services.AddOpenApi();
#endregion
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = long.MaxValue; // 如果不限制，设置为long.MaxValue
    options.MemoryBufferThreshold = int.MaxValue;
});

// 配置JWT设置
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// 添加JWT认证
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Sys_";
});

// 1️⃣ 注册 RabbitMQ 服务（单例）
builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();

// 2️⃣ 注册消费者后台服务
builder.Services.AddHostedService<RabbitMQTestService>();
//处理数据库sqlite迁移值系统持久化
//builder.Services.AddHostedService<DatabaseInitializerServices>();
//更新数据库省份字段，数据大约有10000条
//builder.Services.AddHostedService<CompanyUpdateBackgroundService>();
//builder.Services.AddHostedService<HostLoadCachBackgroupService>();


var app = builder.Build();
app.UseIpRateLimiting();
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
app.UseCors("AllowFrontend");
app.UseAuthentication();
// // 1. 添加自定义中间件来拦截静态文件请求
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower() ?? "";

    // 定义需要保护的目录和文件类型
    var protectedPatterns = new[]
    {
        "/uploads/",    // 上传文件目录
        "/private/",    // 私有文件目录
        ".config",      // 配置文件      // JSON文件
        ".xml",         // XML文件
        ".db"           // 数据库文件
    };

    // 检查当前请求是否匹配受保护的模式
    var isProtected = protectedPatterns.Any(pattern =>
        pattern.StartsWith("/") ? path.Contains(pattern) : path.EndsWith(pattern));

    if (isProtected)
    {
        // 验证访问权限
        context.Response.StatusCode = 403;
        context.Response.ContentType = "text/plain; charset=utf-8";
        await context.Response.WriteAsync("拒绝访问",Encoding.UTF8);
        return;
        // 可选：记录访问日志
    }

    await next();
});


//app.UseStaticFiles();
if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")))
    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads"));
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")),
    RequestPath = "/uploads"

})
;
app.UseAuthorization();

app.MapControllers();

app.Run();
