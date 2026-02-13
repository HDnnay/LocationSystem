# LocationSystem - 通用管理系统

## 项目概述

LocationSystem 是一个现代化的通用管理系统，提供全面的运营管理功能，包括用户管理、角色权限管理、公司管理、租房管理等。系统采用前后端分离架构，后端基于 ASP.NET Core 构建，前端使用 Vue 框架，支持实时通信和事件驱动的缓存更新机制。

系统采用技术架构，包括 CQRS 模式实现命令查询责任分离，以及轻量级的事件总线实现事件驱动架构，提高系统的可维护性和扩展性。

## 功能特性

### 核心功能

- **用户认证与授权**：基于 JWT 的认证系统，支持多种用户类型
- **用户管理**：添加、编辑、删除用户信息，分配角色和权限
- **角色权限管理**：基于角色的访问控制（RBAC），支持自定义角色和权限
- **公司管理**：公司信息管理，包括省份统计等功能
- **租房管理**：租房信息管理，支持创建和查询
- **菜单管理**：动态菜单管理，基于用户权限显示相应菜单

### 技术特性

- **CQRS 模式**：实现了命令查询责任分离，提高系统可维护性和扩展性
- **SimpleMediator**：轻量级的中介者模式实现，用于处理命令和查询
- **EventBus 简单实现事件驱动架构**：轻量级基于内存的事件总线实现，支持事件的发布和订阅
- **缓存一致性**：通过事件驱动机制确保权限变更时缓存同步更新
- **实时通信**：使用 SignalR 实现菜单更新等实时通知
- **容器化部署**：支持 Docker 容器化部署，便于环境一致性
- **消息队列**：集成 RabbitMQ 实现异步消息处理
- **多数据库支持**：支持 SQL Server 和 SQLite 数据库
- **CORS 配置**：灵活的跨域资源共享配置

## 技术栈

### 后端

- **ASP.NET Core 10.0**：Web API 框架
- **Entity Framework Core**：ORM 框架
- **SignalR**：实时通信库
- **RabbitMQ**：消息队列
- **Redis**：缓存服务
- **JWT**：身份认证
- **FluentValidation**：数据验证
- **Docker**：容器化

### 前端

- **Vue 3**：前端框架
- **Vue Router**：路由管理
- **Element Plus**：UI 组件库
- **Axios**：HTTP 客户端
- **@microsoft/signalr**：SignalR 客户端

## 目录结构

```
LocationSystem/
├── LocationSystem.Api/             # API 层
│   ├── Controllers/                # 控制器
│   ├── BackgroundServices/         # 后台服务
│   ├── Data/                       # 数据初始化
│   ├── Filters/                    # 过滤器
│   ├── Hubs/                       # SignalR Hubs
│   ├── Middlewares/                # 中间件
│   ├── RabbitMQs/                  # RabbitMQ 服务
│   ├── Program.cs                  # 应用入口
│   └── appsettings.json            # 配置文件
├── LocationSystem.Application/     # 应用层
│   ├── Contrats/                   # 接口定义
│   ├── Events/                     # 事件系统
│   ├── Features/                   # 功能模块
│   ├── Services/                   # 应用服务
│   └── RegisterApplicationServices.cs # 服务注册
├── LocationSystem.Domain/          # 领域层
│   ├── Entities/                   # 实体
│   ├── ValueObjects/               # 值对象
│   └── Aggregates/                 # 聚合根
├── LocationSystem.Infrastructure/  # 基础设施层
│   ├── Repositories/               # 仓储实现
│   ├── Services/                   # 基础设施服务
│   └── Persistence/                # 数据持久化
├── locationsystem_webapp/          # 前端应用
│   ├── src/                        # 源代码
│   ├── public/                     # 静态资源
│   └── Dockerfile                  # 前端 Dockerfile
├── docker-compose.yml              # Docker Compose 配置
└── Docker部署文档.md               # Docker 部署文档
```

## 快速开始

### 前提条件

- .NET SDK 10.0 或更高版本
- Node.js 16.0 或更高版本
- Docker 和 Docker Compose（可选，用于容器化部署）
- SQL Server 或 SQLite（用于数据存储）
- Redis（用于缓存）
- RabbitMQ（用于消息队列）

### 本地开发

#### 后端启动

1. 克隆仓库
2. 打开 `LocationSystem.Api` 目录
3. 运行 `dotnet restore` 恢复依赖
4. 运行 `dotnet run` 启动 API 服务

#### 前端启动

1. 打开 `locationsystem_webapp` 目录
2. 运行 `npm install` 安装依赖
3. 运行 `npm run dev` 启动开发服务器

### Docker 部署

1. 克隆仓库
2. 在项目根目录运行 `docker-compose up -d`
3. 访问 `http://localhost:8080` 查看前端应用
4. API 服务运行在 `http://localhost:8000`

## 系统架构

### 分层架构

- **API 层**：处理 HTTP 请求，路由到相应的控制器
- **应用层**：实现业务逻辑，协调领域对象完成业务操作
- **领域层**：定义核心业务实体和规则
- **基础设施层**：提供数据访问、缓存、消息队列等技术服务

### 事件驱动设计

系统采用事件驱动架构，通过内存事件总线实现模块间通信：

- **事件发布**：当权限、角色变更时发布事件
- **事件订阅**：缓存服务订阅事件，自动更新缓存
- **实时通知**：通过 SignalR 实现菜单更新等实时通知

### 缓存策略

- **缓存键管理**：统一的缓存键命名规范，集中维护于 `CacheKeys` 类
- **缓存更新**：基于事件驱动的缓存更新机制，确保缓存一致性
- **缓存过期**：合理的缓存过期策略，平衡性能和数据一致性

### 中间件和事件系统

#### SimpleMediator

- **轻量级实现**：基于反射的简单中介者模式实现，用于处理命令和查询
- **请求处理**：支持 `IRequest<TResponse>` 和 `IRequest` 两种请求类型
- **验证集成**：自动集成 FluentValidation 进行请求验证
- **依赖注入**：通过构造函数注入 `IServiceProvider`，实现处理器的动态解析

#### IEventBus 简单实现

- **内存实现**：基于内存字典的轻量级事件总线实现
- **事件发布**：支持异步发布事件到所有订阅者
- **事件订阅**：支持订阅特定类型的事件
- **取消订阅**：支持取消事件订阅，避免内存泄漏
- **类型安全**：使用泛型确保事件类型的安全传递

## 关键模块

### 用户认证与授权

- **JWT 认证**：无状态认证，支持令牌刷新
- **基于角色的访问控制**：细粒度的权限管理
- **权限验证**：自定义 `PermissionAuthorizeAttribute` 实现权限验证

### 角色权限管理

- **角色管理**：支持自定义角色创建和管理
- **权限分配**：细粒度的权限分配机制
- **菜单权限**：基于权限的动态菜单显示

### 实时通信

- **SignalR 集成**：实现菜单更新等实时通知
- **用户组管理**：基于用户 ID 的消息分组

## 配置管理

### 环境变量

- **后端配置**：通过 `appsettings.json` 和环境变量配置
- **前端配置**：通过 `.env` 文件和环境变量配置
- **Docker 配置**：通过 `docker-compose.yml` 配置容器环境

### 关键配置项

- **数据库连接字符串**：支持 SQL Server 和 SQLite
- **Redis 连接字符串**：用于缓存服务
- **RabbitMQ 配置**：用于消息队列
- **JWT 配置**：用于身份认证
- **CORS 配置**：用于跨域资源共享

## 监控与日志

- **应用日志**：详细的应用运行日志
- **错误处理**：统一的错误处理机制
- **性能监控**：请求限流和性能监控

## 贡献指南

1. Fork 仓库
2. 创建特性分支
3. 提交更改
4. 推送到分支
5. 创建 Pull Request

## 许可证

本项目采用 MIT 许可证 - 详见 [LICENSE](LICENSE) 文件


---

**注意**：本项目仅供学习和参考使用，实际生产环境部署前请进行充分的安全评估和测试。