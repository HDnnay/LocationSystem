var builder = DistributedApplication.CreateBuilder(args);

// 添加 SQL Server 服务
var sqlServer = builder.AddSqlServer("sqlserver");
var database = sqlServer.AddDatabase("LocationSystemDB");

// 添加 Redis 服务
var redis = builder.AddRedis("redis");

// 添加 RabbitMQ 服务
var rabbitMq = builder.AddRabbitMQ("rabbitmq");

// 添加现有 API 项目
var apiService = builder.AddProject<Projects.LocationSystem_Api>("location-api")
    .WithHttpHealthCheck("/health")
    .WithReference(redis)
    .WithReference(rabbitMq)
    .WithReference(database);

// 添加现有前端项目
var webService = builder.AddProject<Projects.LocationSystem_Aspire_Web>("location-web")
    .WithHttpHealthCheck("/health");

builder.Build().Run();
