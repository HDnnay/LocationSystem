using HotChocolate;
using HotChocolate.Types;
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Menus.Models;
using LocationSystem.Application.Features.Permissions.Models;
using LocationSystem.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LocationSystem.Api.GraphQL
{
    public class Query
    {
        private readonly IMenuRepository _menuRepository;
        private readonly MenuDataLoader _menuDataLoader;
        private readonly IMapper _mapper;

        public Query(IMenuRepository menuRepository, MenuDataLoader menuDataLoader, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _menuDataLoader = menuDataLoader;
            _mapper = mapper;
        }

        [GraphQLDescription("获取菜单列表")]
        public async Task<object> GetMenus(
            [GraphQLDescription("页码")] int page = 1,
            [GraphQLDescription("每页数量")] int pageSize = 10)
        {
            var query = new LocationSystem.Application.Features.Menus.Queries.GetAllMenus.GetAllMenusQuery { Page = page, PageSize = pageSize };
            return await _menuRepository.GetMenuPage(query);
        }

        [GraphQLDescription("获取菜单详情")]
        [GraphQLType(typeof(MenuType))]
        public async Task<Menu> GetMenu(
            [GraphQLDescription("菜单ID")] Guid id,
            CancellationToken cancellationToken)
        {
            return await _menuDataLoader.LoadAsync(id, cancellationToken);
        }

        [GraphQLDescription("获取菜单树形结构")]
        [GraphQLType(typeof(ListType<MenuType>))]
        public async Task<List<MenuDto>> GetMenuTree()
        {
            var menus = await _menuRepository.GetMenuTreeAsync();
            return _mapper.Map<List<MenuDto>>(menus);
        }
    }

    public class MenuDataLoader
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Guid, Task<Menu>> _cache = new Dictionary<Guid, Task<Menu>>();
        private readonly object _cacheLock = new object();

        public MenuDataLoader(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<Menu> LoadAsync(Guid id, CancellationToken cancellationToken)
        {
            // 检查缓存中是否已有结果
            lock (_cacheLock)
            {
                if (_cache.TryGetValue(id, out var existingTask))
                {
                    return existingTask;
                }

                // 创建一个新的任务并添加到缓存
                var task = LoadMenuAsync(id, cancellationToken);
                _cache[id] = task;
                return task;
            }
        }

        private async Task<Menu> LoadMenuAsync(Guid id, CancellationToken cancellationToken)
        {
            // 创建一个新的作用域，获取新的 IMenuRepository 实例
            using (var scope = _serviceProvider.CreateScope())
            {
                var menuRepository = scope.ServiceProvider.GetRequiredService<IMenuRepository>();
                var menus = await menuRepository.GetByIdsAsync(new[] { id });
                var menu = menus.FirstOrDefault();
                if (menu == null)
                {
                    throw new Exception($"Menu with id {id} not found");
                }
                return menu;
            }
        }
    }

    public class PermissionDataLoader
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Guid, Task<List<PermissionDto>>> _cache = new Dictionary<Guid, Task<List<PermissionDto>>>();
        private readonly object _cacheLock = new object();

        public PermissionDataLoader(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<List<PermissionDto>> LoadAsync(Guid menuId, CancellationToken cancellationToken)
        {
            // 检查缓存中是否已有结果
            lock (_cacheLock)
            {
                if (_cache.TryGetValue(menuId, out var existingTask))
                {
                    return existingTask;
                }

                // 创建一个新的任务并添加到缓存
                var task = LoadPermissionsAsync(menuId, cancellationToken);
                _cache[menuId] = task;
                return task;
            }
        }

        private async Task<List<PermissionDto>> LoadPermissionsAsync(Guid menuId, CancellationToken cancellationToken)
        {
            // 创建一个新的作用域，获取新的 IMenuRepository 实例
            using (var scope = _serviceProvider.CreateScope())
            {
                var menuRepository = scope.ServiceProvider.GetRequiredService<IMenuRepository>();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                var menu = await menuRepository.GetByIdWithPermissionsAsync(menuId);
                if (menu == null)
                {
                    return new List<PermissionDto>();
                }
                var permissions = menu.PermissionMenus.Select(pm => pm.Permission).ToList();
                return mapper.Map<List<PermissionDto>>(permissions);
            }
        }
    }
}