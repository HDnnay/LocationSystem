var builder = DistributedApplication.CreateBuilder(args);

// 添加 SQL Server 服务
var sqlServer = builder.AddSqlServer("sqlserver", port: 11433);
var database = sqlServer.AddDatabase("LocationSystemDB");

// 添加 Redis 服务
var redis = builder.AddRedis("redis");

// 添加 RabbitMQ 服务
var rabbitMq = builder.AddRabbitMQ("rabbitmq");

// 添加现有 API 项目
var apiService = builder.AddProject<Projects.LocationSystem_Api>("location-api")
    .WithHttpHealthCheck("/health")
    .WithHttpEndpoint(name: "api-http")
    .WithReference(redis)
    .WithReference(rabbitMq)
    .WithReference(database);

// 添加 locationsystem_webapp 前端项目
var webApp = builder.AddNpmApp("location-webapp", "../locationsystem_webapp", scriptName: "dev")
    .WithHttpEndpoint(name: "web-http", env: "PORT")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
