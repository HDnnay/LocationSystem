# GraphQL 接入文档

## 1. 项目概述

本项目是一个基于 .NET 10.0 的位置管理系统，为了提供更灵活的 API 查询方式，我们接入了 GraphQL 功能。GraphQL 允许客户端精确指定需要的数据，减少了网络传输和服务器处理开销。

## 2. 接入步骤

### 2.1 安装依赖项

首先，我们需要安装 GraphQL 相关的 NuGet 包：

```bash
dotnet add package HotChocolate.AspNetCore
dotnet add package HotChocolate.Data
```

### 2.2 配置 GraphQL 服务

在 `Program.cs` 文件中添加 GraphQL 服务配置：

```csharp
// 配置 GraphQL 服务
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

// 配置 GraphQL 中间件
app.MapGraphQL();
```

### 2.3 实现 GraphQL 类型

#### 2.3.1 创建 Query 类

创建 `GraphQL/Query.cs` 文件，实现查询操作：

```csharp
using HotChocolate;
using HotChocolate.Types;
using LocationSystem.Application.Features.Menus.Queries.GetAllMenus;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Api.GraphQL
{
    public class Query
    {
        private readonly IMediator _mediator;

        public Query(IMediator mediator)
        {
            _mediator = mediator;
        }

        [GraphQLName("menus")]
        [GraphQLDescription("获取菜单列表")]
        public async Task<object> GetMenus(
            [GraphQLDescription("页码")] int page = 1,
            [GraphQLDescription("每页数量")] int pageSize = 10)
        {
            var query = new GetAllMenusQuery { Page = page, PageSize = pageSize };
            return await _mediator.Send(query);
        }
    }
}
```

#### 2.3.2 创建 Mutation 类

创建 `GraphQL/Mutation.cs` 文件，实现变更操作：

```csharp
using HotChocolate;
using HotChocolate.Types;
using LocationSystem.Application.Features.Menus.Commands.AssignPermissionsToMenu;
using LocationSystem.Application.Features.Menus.Commands.CreateMenu;
using LocationSystem.Application.Features.Menus.Commands.DeleteMenu;
using LocationSystem.Application.Features.Menus.Commands.UpdateMenu;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Api.GraphQL
{
    public class Mutation
    {
        private readonly IMediator _mediator;

        public Mutation(IMediator mediator)
        {
            _mediator = mediator;
        }

        [GraphQLName("createMenu")]
        [GraphQLDescription("创建菜单")]
        public async Task<object> CreateMenu(
            [GraphQLDescription("菜单信息")] CreateMenuCommand command)
        {
            return await _mediator.Send(command);
        }

        [GraphQLName("updateMenu")]
        [GraphQLDescription("更新菜单")]
        public async Task<object> UpdateMenu(
            [GraphQLDescription("菜单ID")] Guid id,
            [GraphQLDescription("菜单信息")] UpdateMenuCommand command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        [GraphQLName("deleteMenu")]
        [GraphQLDescription("删除菜单")]
        public async Task<object> DeleteMenu(
            [GraphQLDescription("菜单ID")] Guid id)
        {
            var command = new DeleteMenuCommand { MenuId = id };
            await _mediator.Send(command);
            return new { success = true };
        }

        [GraphQLName("assignPermissionsToMenu")]
        [GraphQLDescription("为菜单分配权限")]
        public async Task<object> AssignPermissionsToMenu(
            [GraphQLDescription("菜单ID")] Guid id,
            [GraphQLDescription("权限ID列表")] List<Guid> permissionIds)
        {
            var command = new AssignPermissionsToMenuCommand { MenuId = id, PermissionIds = permissionIds };
            await _mediator.Send(command);
            return new { success = true };
        }
    }
}
```

### 2.4 配置类型系统

创建 `GraphQL/MenuType.cs` 文件，定义菜单的 GraphQL 类型：

```csharp
using HotChocolate.Types;
using LocationSystem.Application.Features.Menus.Models;

namespace LocationSystem.Api.GraphQL
{
    public class MenuType : ObjectType<MenuDto>
    {
        protected override void Configure(IObjectTypeDescriptor<MenuDto> descriptor)
        {
            descriptor.Field(m => m.Id).Type<IdType>();
            descriptor.Field(m => m.Name).Type<StringType>();
            descriptor.Field(m => m.ChildMenus).Type<ListType<MenuType>>();
        }
    }
}
```

## 3. 解决的问题

### 3.1 命名空间冲突

在实现过程中，我们遇到了 `Path` 命名空间冲突的问题，HotChocolate 和 System.IO 都定义了 Path 类。我们通过使用完全限定名 `System.IO.Path` 来解决这个问题。

### 3.2 服务依赖问题

项目依赖于 Redis 和 RabbitMQ 服务，但这些服务可能不可用。我们通过以下方式解决：

1. 移除了对 RabbitMQ 服务的强制依赖
2. 保留了 Redis 缓存服务，因为 AspNetCoreRateLimit 依赖于它
3. 移除了 AspNetCoreRateLimit 中间件，以减少对 Redis 的依赖

### 3.3 Mutation 字段识别问题

HotChocolate 无法识别 Mutation 类中的字段，我们通过添加 `[GraphQLName]` 特性来明确指定字段名称，解决了这个问题。

## 4. 使用示例

### 4.1 查询菜单列表

```graphql
query {
  menus(page: 1, pageSize: 10) {
    items {
      id
      name
      icon
      url
      parentId
      orderIndex
      childMenus {
        id
        name
        icon
        url
      }
    }
    totalCount
    pageSize
    currentPage
  }
}
```

### 4.2 创建菜单

```graphql
mutation {
  createMenu(
    command: {
      name: "测试菜单"
      icon: "test"
      url: "/test"
      parentId: null
      orderIndex: 1
    }
  ) {
    id
    name
    icon
    url
  }
}
```

### 4.3 更新菜单

```graphql
mutation {
  updateMenu(
    id: "12345678-1234-1234-1234-1234567890ab"
    command: {
      name: "更新的菜单"
      icon: "updated"
      url: "/updated"
      orderIndex: 2
    }
  ) {
    id
    name
    icon
    url
  }
}
```

### 4.4 删除菜单

```graphql
mutation {
  deleteMenu(id: "12345678-1234-1234-1234-1234567890ab") {
    success
  }
}
```

### 4.5 为菜单分配权限

```graphql
mutation {
  assignPermissionsToMenu(
    id: "12345678-1234-1234-1234-1234567890ab"
    permissionIds: ["87654321-4321-4321-4321-098765432109"]
  ) {
    success
  }
}
```

## 5. 部署建议

### 5.1 开发环境

在开发环境中，您可以通过以下命令启动服务：

```bash
cd LocationSystem.Api
dotnet run
```

然后访问 `http://localhost:5231/graphql` 打开 GraphQL  playground 进行测试。

### 5.2 生产环境

在生产环境中，建议：

1. 确保 Redis 服务可用，以支持缓存功能
2. 配置适当的错误处理和日志记录
3. 启用 GraphQL  persisted queries 以提高性能
4. 考虑使用 GraphQL 缓存来减少数据库查询

## 6. 总结

通过接入 GraphQL，我们为系统提供了更灵活、更高效的 API 查询方式。客户端可以精确指定需要的数据，减少了网络传输和服务器处理开销。同时，我们解决了接入过程中遇到的各种问题，确保了系统的稳定性和可靠性。

GraphQL 的接入为系统带来了以下好处：

1. **减少网络传输**：客户端只获取需要的数据，避免了过度获取
2. **提高开发效率**：前端开发人员可以自主查询所需数据，减少了与后端的沟通成本
3. **增强 API 灵活性**：API 可以随着前端需求的变化而灵活调整，而不需要修改后端代码
4. **改善开发体验**：GraphQL playground 提供了交互式的 API 测试环境

未来，我们可以考虑进一步扩展 GraphQL 功能，例如添加更多的查询和变更操作，实现实时数据订阅等。