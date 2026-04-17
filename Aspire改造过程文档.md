# Aspire 项目改造过程文档

## 1. 改造背景和目标

### 1.1 背景
- 现有项目使用 .NET 10.0 框架
- 需要将项目架构改造为 Aspire，实现服务编排和管理
- 要求只使用 .NET 10.0 以上版本，不允许降级

### 1.2 目标
- 创建基于 .NET 10.0 的 Aspire 项目
- 整合现有的 API 项目到 Aspire 中
- 配置 Redis 和 RabbitMQ 服务
- 整合前端项目到 Aspire 中
- 实现服务的健康检查和监控

## 2. 改造步骤

### 2.1 创建 Aspire 项目

1. **创建 Aspire 解决方案**
   - 在项目根目录下创建 `LocationSystem.Aspire` 系列项目
   - 使用 .NET 10.0 框架

2. **创建 Aspire 主机项目**
   - 项目名称：`LocationSystem.Aspire.AppHost`
   - 负责管理和编排所有服务

3. **创建服务默认配置项目**
   - 项目名称：`LocationSystem.Aspire.ServiceDefaults`
   - 提供共享的服务配置（健康检查、服务发现等）

4. **创建前端项目**
   - 项目名称：`LocationSystem.Aspire.Web`
   - 基于 Blazor 的前端服务

### 2.2 配置 Aspire 主机项目

1. **修改 AppHost.csproj 文件**
   - 添加必要的 NuGet 包引用
   - 添加对现有 API 项目和前端项目的引用

2. **修改 AppHost.cs 文件**
   - 配置 Redis 和 RabbitMQ 服务
   - 添加现有 API 项目到 Aspire 中
   - 添加前端项目到 Aspire 中
   - 配置健康检查端点

### 2.3 配置服务默认项目

1. **修改 ServiceDefaults.csproj 文件**
   - 添加必要的 NuGet 包引用

2. **修改 Extensions.cs 文件**
   - 配置服务发现
   - 配置健康检查
   - 配置 HTTP 客户端默认设置

### 2.4 配置 API 项目

1. **修改 API 项目的 csproj 文件**
   - 添加对 ServiceDefaults 项目的引用

2. **修改 Program.cs 文件**
   - 添加 `builder.AddServiceDefaults()` 调用
   - 添加 `app.MapDefaultEndpoints()` 调用

### 2.5 整合前端项目

1. **保留现有的前端项目配置**
   - 在 AppHost.cs 中添加前端项目的引用
   - 配置健康检查端点

## 3. 依赖项

### 3.1 Aspire 主机项目依赖

```xml
<ItemGroup>
  <PackageReference Include="Aspire.AppHost.Sdk" Version="13.1.2" />
  <PackageReference Include="Aspire.Hosting.Redis" Version="13.1.2" />
  <PackageReference Include="Aspire.Hosting.RabbitMQ" Version="13.1.2" />
  <PackageReference Include="Aspire.Hosting" Version="13.1.2" />
</ItemGroup>

<ItemGroup>
  <ProjectReference Include="..\LocationSystem.Api\LocationSystem.Api.csproj" />
  <ProjectReference Include="..\LocationSystem.Aspire.Web\LocationSystem.Aspire.Web.csproj" />
</ItemGroup>
```

### 3.2 服务默认项目依赖

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.Extensions.Http.Resilience" Version="10.1.0" />
  <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="10.1.0" />
  <PackageReference Include="Microsoft.Extensions.ServiceDiscovery" Version="1.2.0" />
  <PackageReference Include="Microsoft.Extensions.Diagnostics.HealthChecks" Version="10.1.0" />
</ItemGroup>
```

### 3.3 API 项目依赖

```xml
<ItemGroup>
  <ProjectReference Include="..\LocationSystem.Application\LocationSystem.Application.csproj" />
  <ProjectReference Include="..\LocationSystem.Infrastructure\LocationSystem.Infrastructure.csproj" />
  <ProjectReference Include="..\LocationSystem.Aspire.ServiceDefaults\LocationSystem.Aspire.ServiceDefaults.csproj" />
</ItemGroup>
```

## 4. 关键代码

### 4.1 AppHost.cs 配置

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// 添加 Redis 服务
var redis = builder.AddRedis("redis");

// 添加 RabbitMQ 服务
var rabbitMq = builder.AddRabbitMQ("rabbitmq");

// 添加现有 API 项目
var apiService = builder.AddProject<Projects.LocationSystem_Api>("location-api")
    .WithHttpHealthCheck("/health")
    .WithReference(redis)
    .WithReference(rabbitMq);

// 添加现有前端项目
var webService = builder.AddProject<Projects.LocationSystem_Aspire_Web>("location-web")
    .WithHttpHealthCheck("/health");

builder.Build().Run();
```

### 4.2 Extensions.cs 配置

```csharp
public static class Extensions
{
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        // 添加健康检查服务
        builder.Services.AddHealthChecks();

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default
            http.AddStandardResilienceHandler();

            // Turn on service discovery by default
            http.AddServiceDiscovery();
        });

        return builder;
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        app.MapHealthChecks("/health");

        return app;
    }
}
```

### 4.3 Program.cs 配置

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults(); // 添加 Aspire 服务配置

// 其他配置...

app.MapDefaultEndpoints(); // 映射健康检查端点
app.Run();
```

## 5. 遇到的问题和解决方案

### 5.1 问题：Aspire.RuntimeIdentifier.Tool 运行失败
**解决方案**：创建新的 Aspire 项目并指定 .NET 10.0 框架，确保 SDK 兼容性。

### 5.2 问题：找不到包 Microsoft.AspNetCore.Http.Resilience
**解决方案**：从 ServiceDefaults.csproj 中移除该包引用，仅保留 Microsoft.Extensions.Http.Resilience。

### 5.3 问题："IHostApplicationBuilder" 未包含 "AddDefaultHealthChecks" 的定义
**解决方案**：将 `builder.AddDefaultHealthChecks()` 替换为 `builder.Services.AddHealthChecks()`。

### 5.4 问题："AddOpenTelemetry" 方法没有采用 1 个参数的重载
**解决方案**：简化 OpenTelemetry 配置，移除复杂的配置代码。

### 5.5 问题：文件被锁定，无法复制
**解决方案**：使用 `taskkill` 命令终止占用文件的进程，然后重新启动项目。

### 5.6 问题：Aspire 无法接入 LocationSystem.Api 项目
**解决方案**：将 LocationSystem.Api 及其依赖项目添加到 LocationSystem.Aspire.sln 解决方案中。

```bash
# 添加 API 项目到解决方案
dotnet sln LocationSystem.Aspire.sln add LocationSystem.Api/LocationSystem.Api.csproj

# 重新构建解决方案
dotnet build LocationSystem.Aspire.sln

# 重新启动 Aspire
cd LocationSystem.Aspire.AppHost
dotnet run
```

**原因**：Aspire 在构建时需要扫描解决方案中的所有项目，当 LocationSystem.Api 不在解决方案中时，Aspire 无法生成对应的 Projects.LocationSystem_Api 类型，导致配置无法生效。

## 6. 最终项目结构

```
LocationSystem/
├── LocationSystem.Api/                # 现有 API 项目
├── LocationSystem.Application/         # 应用服务层
├── LocationSystem.Infrastructure/      # 基础设施层
├── LocationSystem.Aspire.AppHost/      # Aspire 主机项目
├── LocationSystem.Aspire.ServiceDefaults/ # 服务默认配置项目
├── LocationSystem.Aspire.Web/          # 前端项目
├── locationsystem_webapp/              # Vue 前端项目
├── LocationSystem.slnx                 # 原始解决方案
└── LocationSystem.Aspire.sln           # Aspire 解决方案
```

## 7. 运行和访问

### 7.1 启动 Aspire 项目

在 `LocationSystem.Aspire.AppHost` 目录下运行：

```bash
dotnet run
```

### 7.2 访问 Aspire 仪表盘

启动后，Aspire 会显示仪表盘 URL，例如：

```
Login to the dashboard at https://localhost:17166/login?t=1b9334b6eab248dc33d6bc1721ed6f23
```

### 7.3 访问服务

- **API 服务**：通过 Aspire 仪表盘点击 `location-api` 服务的链接
- **前端服务**：通过 Aspire 仪表盘点击 `location-web` 服务的链接

## 8. 总结

通过本次改造，我们成功将现有项目架构改造为 Aspire，实现了以下目标：

1. 创建了基于 .NET 10.0 的 Aspire 项目
2. 整合了现有的 API 项目到 Aspire 中
3. 配置了 Redis 和 RabbitMQ 服务
4. 整合了前端项目到 Aspire 中
5. 实现了服务的健康检查和监控

Aspire 提供了强大的服务编排和管理能力，通过统一的仪表盘可以方便地监控和管理所有服务，提高了系统的可维护性和可靠性。