using LocationSystem.Api.Middlewares;
using LocationSystem.Application;
using LocationSystem.Infrastructure;
using Microsoft.OpenApi;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    // 配置JSON序列化选项，确保包含默认值
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
    options.JsonSerializerOptions.IncludeFields = true;

    // 配置中文字符不进行Unicode转义，直接输出中文
    options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

    //// 配置DateTime格式为 yyyy-MM-dd HH:mm:ss
    //options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
    //options.JsonSerializerOptions.Converters.Add(new NullableDateTimeJsonConverter());
}); ;
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices(builder.Configuration.GetConnectionString("Redis"));
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "我的 API",
        Version = "v1",
        Description = "这是一个示例 API 文档",
        Contact = new OpenApiContact { Name = "开发团队", Email = "team@example.com" }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "我的 API V1");
        // c.RoutePrefix = string.Empty; // 可将 UI 设置为根路径访问
    });
}
app.UseCustomExceptionHandler();
app.UseAuthorization();

app.MapControllers();

app.Run();
