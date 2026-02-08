# 构建阶段
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

# 发布项目
FROM build AS publish
RUN dotnet publish LocationSystem.Api/LocationSystem.Api.csproj -c Release -o /app/publish

# 运行阶段
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# 复制发布文件
COPY --from=publish /app/publish .

# 创建上传目录
RUN mkdir -p wwwroot/uploads

# 设置环境变量
ENV ASPNETCORE_URLS=http://+:8082
ENV ASPNETCORE_ENVIRONMENT=Production

# 暴露端口
EXPOSE 8082

# 运行应用
ENTRYPOINT ["dotnet", "LocationSystem.Api.dll"]