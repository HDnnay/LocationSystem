# Docker部署文档

## 项目结构

```
LocationSystem/
├── LocationSystem.Api/           # 后端API项目
│   ├── Dockerfile               # 后端Dockerfile
│   ├── Program.cs               # 应用程序入口
│   └── Sqlite/                  # SQLite数据库文件
├── locationsystem_webapp/        # 前端项目
│   ├── Dockerfile               # 前端Dockerfile
│   ├── nginx.conf               # Nginx配置文件
│   └── src/                     # 前端源代码
└── docker-compose.yml            # Docker Compose配置文件
```

## 后端Dockerfile详解

```dockerfile
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
```

### 命令详解

1. **基础镜像选择**
   - `FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base` - 使用ASP.NET Core 10.0运行时作为基础镜像，用于最终运行应用
   - `FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build` - 使用.NET SDK 10.0镜像作为构建环境，包含完整的编译工具

2. **工作目录设置**
   - `WORKDIR /app` - 在base镜像中设置工作目录为/app
   - `WORKDIR /src` - 在build镜像中设置工作目录为/src

3. **端口暴露**
   - `EXPOSE 80` - 暴露容器的80端口，用于接收HTTP请求

4. **项目结构准备**
   - `RUN mkdir -p LocationSystem.Api LocationSystem.Application LocationSystem.Domain LocationSystem.Infrastructure` - 创建项目所需的目录结构

5. **文件复制**
   - `COPY LocationSystem.Api ./LocationSystem.Api` - 复制后端API项目文件
   - `COPY LocationSystem.Application ./LocationSystem.Application` - 复制应用层项目文件
   - `COPY LocationSystem.Domain ./LocationSystem.Domain` - 复制领域层项目文件
   - `COPY LocationSystem.Infrastructure ./LocationSystem.Infrastructure` - 复制基础设施层项目文件

6. **依赖管理**
   - `RUN dotnet restore LocationSystem.Api/LocationSystem.Api.csproj` - 还原项目依赖包

7. **项目构建**
   - `RUN dotnet build LocationSystem.Api/LocationSystem.Api.csproj -c Release -o /app/build` - 以Release模式构建项目，输出到/app/build目录

8. **应用发布**
   - `FROM build AS publish` - 创建一个新的构建阶段，基于之前的build阶段
   - `RUN dotnet publish LocationSystem.Api/LocationSystem.Api.csproj -c Release -o /app/publish` - 发布应用到/app/publish目录

9. **最终镜像构建**
   - `FROM base AS final` - 创建最终镜像，基于base阶段
   - `COPY --from=publish /app/publish .` - 从publish阶段复制发布文件到最终镜像
   - `COPY LocationSystem.Api/Sqlite ./Sqlite` - 复制SQLite数据库文件

10. **目录创建与权限设置**
    - `RUN mkdir -p wwwroot/uploads` - 创建上传目录
    - `RUN if [ ! -d "/app/Sqlite" ]; then mkdir -p /app/Sqlite; fi` - 确保SQLite目录存在
    - `RUN chmod -R 755 /app/Sqlite` - 设置SQLite目录权限

11. **应用启动**
    - `ENTRYPOINT ["dotnet", "LocationSystem.Api.dll"]` - 设置容器启动命令，运行后端API应用

## 前端Dockerfile详解

```dockerfile
# 使用官方Node.js镜像进行构建
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# 使用Nginx作为运行时
FROM nginx:alpine AS runtime
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

### 命令详解

1. **构建环境设置**
   - `FROM node:20-alpine AS build` - 使用Node.js 20 Alpine镜像作为构建环境，Alpine版本体积更小
   - `WORKDIR /app` - 设置工作目录为/app

2. **依赖安装**
   - `COPY package*.json ./` - 复制package.json和package-lock.json文件
   - `RUN npm install` - 安装前端项目依赖

3. **项目构建**
   - `COPY . .` - 复制所有前端源代码文件
   - `RUN npm run build` - 执行构建命令，生成静态文件到dist目录

4. **运行时环境设置**
   - `FROM nginx:alpine AS runtime` - 使用Nginx Alpine镜像作为运行时环境
   - `COPY --from=build /app/dist /usr/share/nginx/html` - 从build阶段复制构建产物到Nginx的静态文件目录
   - `COPY nginx.conf /etc/nginx/conf.d/default.conf` - 复制自定义Nginx配置文件

5. **端口暴露与启动**
   - `EXPOSE 80` - 暴露容器的80端口
   - `CMD ["nginx", "-g", "daemon off;"]` - 设置Nginx以非守护进程模式运行

## docker-compose.yml详解

```yaml
version: '3.8'

services:
  backend:
    build:
      context: .
      dockerfile: LocationSystem.Api/Dockerfile
    ports:
      - "8000:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__SqlServerConnectionString=Server=db;Database=LocationSystem;User Id=sa;Password=Password123!;TrustServerCertificate=True;
      - ConnectionStrings__Redis=host.docker.internal:6379,password=123456,abortConnect=false
      - RabbitMQ__HostName=host.docker.internal
      - RabbitMQ__Port=5672
      - RabbitMQ__UserName=guest
      - RabbitMQ__Password=guest
      - RabbitMQ__VirtualHost=/
    depends_on:
      - db
    extra_hosts:
      - "host.docker.internal:host-gateway"

  frontend:
    build:
      context: ./locationsystem_webapp
      dockerfile: Dockerfile
    ports:
      - "8080:80"
    depends_on:
      - backend

  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    ports:
      - "1434:1433"
    environment:
      - SA_PASSWORD=Password123!
      - ACCEPT_EULA=Y
    volumes:
      - sqlserver_data:/var/opt/mssql
    restart: unless-stopped

volumes:
  sqlserver_data:
```

### 配置详解

1. **服务定义**
   - `services:` - 定义所有容器服务

2. **后端服务**
   - `backend:` - 后端API服务
   - `build:` - 构建配置
     - `context: .` - 构建上下文为当前目录
     - `dockerfile: LocationSystem.Api/Dockerfile` - 使用后端Dockerfile
   - `ports:` - 端口映射
     - `"8000:8080"` - 将主机的8000端口映射到容器的8080端口
   - `environment:` - 环境变量配置
     - `ASPNETCORE_ENVIRONMENT=Production` - 设置为生产环境
     - `ConnectionStrings__SqlServerConnectionString` - SQL Server数据库连接字符串
     - `ConnectionStrings__Redis` - Redis连接字符串
     - `RabbitMQ__*` - RabbitMQ相关配置
   - `depends_on:` - 依赖关系
     - `db` - 依赖数据库服务
   - `extra_hosts:` - 额外主机配置
     - `"host.docker.internal:host-gateway"` - 允许容器访问主机上的服务

3. **前端服务**
   - `frontend:` - 前端服务
   - `build:` - 构建配置
     - `context: ./locationsystem_webapp` - 构建上下文为前端目录
     - `dockerfile: Dockerfile` - 使用前端Dockerfile
   - `ports:` - 端口映射
     - `"8080:80"` - 将主机的8080端口映射到容器的80端口
   - `depends_on:` - 依赖关系
     - `backend` - 依赖后端服务

4. **数据库服务**
   - `db:` - SQL Server数据库服务
   - `image: mcr.microsoft.com/mssql/server:2022-latest` - 使用SQL Server 2022镜像
   - `ports:` - 端口映射
     - `"1434:1433"` - 将主机的1434端口映射到容器的1433端口
   - `environment:` - 环境变量配置
     - `SA_PASSWORD=Password123!` - 设置SA用户密码
     - `ACCEPT_EULA=Y` - 接受SQL Server许可协议
   - `volumes:` - 数据卷配置
     - `sqlserver_data:/var/opt/mssql` - 持久化SQL Server数据
   - `restart: unless-stopped` - 除非手动停止，否则自动重启

5. **数据卷定义**
   - `volumes:` - 定义数据卷
   - `sqlserver_data:` - 用于持久化SQL Server数据的数据卷

## 编译部署步骤

### 1. 准备工作

确保已安装以下软件：
- Docker Desktop (Windows) 或 Docker Engine (Linux)
- Docker Compose

### 2. 构建并启动所有服务

在项目根目录执行以下命令：

```bash
# 构建并启动所有服务
docker-compose up -d --build

# 查看服务状态
docker-compose ps

# 查看服务日志
docker-compose logs
```

### 3. 访问应用

- **前端应用**：http://localhost:8080
- **后端API**：http://localhost:8000
- **数据库**：localhost,1434 (SQL Server)

## 单独项目编译部署命令

### 后端API

```bash
# 构建后端镜像
docker-compose build backend

# 启动后端服务
docker-compose up -d backend

# 查看后端日志
docker-compose logs backend

# 停止后端服务
docker-compose stop backend
```

### 前端

```bash
# 构建前端镜像
docker-compose build frontend

# 启动前端服务
docker-compose up -d frontend

# 查看前端日志
docker-compose logs frontend

# 停止前端服务
docker-compose stop frontend
```

### 数据库

```bash
# 启动数据库服务
docker-compose up -d db

# 查看数据库日志
docker-compose logs db

# 停止数据库服务
docker-compose stop db
```

## 常见问题及解决方案

### 1. 数据库连接失败

**问题**：后端服务无法连接到数据库

**解决方案**：
- 检查数据库容器是否运行：`docker-compose ps db`
- 检查数据库连接字符串是否正确
- 确保数据库服务已完全启动（SQL Server启动需要一定时间）

### 2. 端口冲突

**问题**：启动服务时出现端口已被占用的错误

**解决方案**：
- 修改docker-compose.yml中的端口映射，使用未被占用的端口
- 停止占用端口的其他服务

### 3. 构建失败

**问题**：构建镜像时失败

**解决方案**：
- 检查网络连接是否正常
- 检查Dockerfile中的文件路径是否正确
- 检查项目依赖是否完整

### 4. 前端无法访问后端API

**问题**：前端应用无法连接到后端API

**解决方案**：
- 检查Nginx配置文件中的API代理设置
- 确保后端服务正常运行
- 检查前端代码中的API地址配置

### 5. 数据库持久化问题

**问题**：重启容器后数据库数据丢失

**解决方案**：
- 确保使用了数据卷持久化：`volumes: - sqlserver_data:/var/opt/mssql`
- 检查数据卷是否正确创建：`docker volume ls`

## 总结

通过Docker Compose，我们成功将ASP.NET Core后端、Vue.js前端和SQL Server数据库容器化，实现了以下目标：

1. **环境一致性**：确保开发、测试和生产环境的一致性
2. **简化部署**：通过一个命令即可构建和启动所有服务
3. **资源隔离**：各服务运行在独立的容器中，避免相互干扰
4. **可扩展性**：易于添加新服务或修改现有服务配置
5. **数据持久化**：通过数据卷确保数据库数据不会丢失

这种容器化部署方式不仅提高了开发效率，也使得应用的部署和维护变得更加简单可靠。