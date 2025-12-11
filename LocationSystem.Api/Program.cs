using LocationSystem.Application;
using LocationSystem.Infrastructure;
using Microsoft.OpenApi;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
