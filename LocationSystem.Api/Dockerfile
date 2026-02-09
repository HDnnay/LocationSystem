# 使用官方ASP.NET Core运行时作为基础镜像
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

# 使用官方SDK镜像进行构建
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 创建项目目录结构
RUN mkdir -p LocationSystem.Api LocationSystem.Application LocationSystem.Domain LocationSystem.Infrastructure

# 复制项目文件
COPY LocationSystem.Api ./LocationSystem.Api
COPY LocationSystem.Application ./LocationSystem.Application
COPY LocationSystem.Domain ./LocationSystem.Domain
COPY LocationSystem.Infrastructure ./LocationSystem.Infrastructure

# 还原依赖
RUN dotnet restore LocationSystem.Api/LocationSystem.Api.csproj

# 构建项目
RUN dotnet build LocationSystem.Api/LocationSystem.Api.csproj -c Release -o /app/build

# 发布应用
FROM build AS publish
WORKDIR /src
RUN dotnet publish LocationSystem.Api/LocationSystem.Api.csproj -c Release -o /app/publish

# 构建最终镜像
FROM base AS final
WORKDIR /app

# 复制发布文件
COPY --from=publish /app/publish .

# 复制SQLite数据库文件
COPY LocationSystem.Api/Sqlite ./Sqlite

# 创建上传目录
RUN mkdir -p wwwroot/uploads

# 确保SQLite目录存在并设置权限
RUN if [ ! -d "/app/Sqlite" ]; then mkdir -p /app/Sqlite; fi
RUN chmod -R 755 /app/Sqlite

# 暴露端口
EXPOSE 80

# 运行应用
ENTRYPOINT ["dotnet", "LocationSystem.Api.dll"]