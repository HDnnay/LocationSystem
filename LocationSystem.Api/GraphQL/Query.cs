using HotChocolate;
using HotChocolate.Types;
using LocationSystem.Application.Contrats.Repositories;
using Dtos = LocationSystem.Application.Dtos;
using MenuModels = LocationSystem.Application.Features.Menus.Models;
using PermissionModels = LocationSystem.Application.Features.Permissions.Models;
using UserModels = LocationSystem.Application.Features.Users.Models;
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
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly MenuDataLoader _menuDataLoader;
        private readonly IMapper _mapper;

        public Query(IMenuRepository menuRepository, IUserRepository userRepository, IRoleRepository roleRepository, IPermissionRepository permissionRepository, MenuDataLoader menuDataLoader, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
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
        public async Task<List<MenuModels.MenuDto>> GetMenuTree()
        {
            var menus = await _menuRepository.GetMenuTreeAsync();
            return _mapper.Map<List<MenuModels.MenuDto>>(menus);
        }

        [GraphQLDescription("获取用户列表")]
        [GraphQLType(typeof(ListType<UserType>))]
        public async Task<List<Dtos.UserDto>> GetUsers()
        {
            var users = await _userRepository.GetAll();
            return _mapper.Map<List<Dtos.UserDto>>(users);
        }

        [GraphQLDescription("获取用户详情")]
        [GraphQLType(typeof(UserType))]
        public async Task<Dtos.UserDto> GetUser(
            [GraphQLDescription("用户ID")] Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception($"用户不存在，ID: {id}");
            }
            return _mapper.Map<Dtos.UserDto>(user);
        }

        [GraphQLDescription("获取角色列表")]
        [GraphQLType(typeof(ListType<RoleType>))]
        public async Task<List<Dtos.RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetRolesWithPermissionsAsync();
            return _mapper.Map<List<Dtos.RoleDto>>(roles);
        }

        [GraphQLDescription("获取角色详情")]
        [GraphQLType(typeof(RoleType))]
        public async Task<Dtos.RoleDto> GetRole(
            [GraphQLDescription("角色ID")] Guid id)
        {
            var role = await _roleRepository.GetRoleWithPermissionsAsync(id);
            if (role == null)
            {
                throw new Exception($"角色不存在，ID: {id}");
            }
            return _mapper.Map<Dtos.RoleDto>(role);
        }

        [GraphQLDescription("获取权限列表")]
        [GraphQLType(typeof(ListType<PermissionType>))]
        public async Task<List<Dtos.PermissionDto>> GetPermissions()
        {
            var permissions = await _permissionRepository.GetAll();
            return _mapper.Map<List<Dtos.PermissionDto>>(permissions);
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
        private readonly Dictionary<Guid, Task<List<Dtos.PermissionDto>>> _cache = new Dictionary<Guid, Task<List<Dtos.PermissionDto>>>();
        private readonly object _cacheLock = new object();

        public PermissionDataLoader(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<List<Dtos.PermissionDto>> LoadAsync(Guid menuId, CancellationToken cancellationToken)
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

        private async Task<List<Dtos.PermissionDto>> LoadPermissionsAsync(Guid menuId, CancellationToken cancellationToken)
        {
            // 创建一个新的作用域，获取新的 IMenuRepository 实例
            using (var scope = _serviceProvider.CreateScope())
            {
                var menuRepository = scope.ServiceProvider.GetRequiredService<IMenuRepository>();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                var menu = await menuRepository.GetByIdWithPermissionsAsync(menuId);
                if (menu == null)
                {
                    return new List<Dtos.PermissionDto>();
                }
                var permissions = menu.PermissionMenus.Select(pm => pm.Permission).ToList();
                return mapper.Map<List<Dtos.PermissionDto>>(permissions);
            }
        }
    }

    public class UserRolesDataLoader
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Guid, Task<List<Role>>> _cache = new Dictionary<Guid, Task<List<Role>>>();
        private readonly object _cacheLock = new object();

        public UserRolesDataLoader(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<List<Role>> LoadAsync(Guid userId, CancellationToken cancellationToken)
        {
            lock (_cacheLock)
            {
                if (_cache.TryGetValue(userId, out var existingTask))
                {
                    return existingTask;
                }

                var task = LoadRolesAsync(userId, cancellationToken);
                _cache[userId] = task;
                return task;
            }
        }

        private async Task<List<Role>> LoadRolesAsync(Guid userId, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var user = await userRepository.GetByIdWithRolesAsync(userId);
                return user?.Roles.ToList() ?? new List<Role>();
            }
        }
    }

    public class RolePermissionsDataLoader
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Guid, Task<List<Permission>>> _cache = new Dictionary<Guid, Task<List<Permission>>>();
        private readonly object _cacheLock = new object();

        public RolePermissionsDataLoader(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<List<Permission>> LoadAsync(Guid roleId, CancellationToken cancellationToken)
        {
            lock (_cacheLock)
            {
                if (_cache.TryGetValue(roleId, out var existingTask))
                {
                    return existingTask;
                }

                var task = LoadPermissionsAsync(roleId, cancellationToken);
                _cache[roleId] = task;
                return task;
            }
        }

        private async Task<List<Permission>> LoadPermissionsAsync(Guid roleId, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var roleRepository = scope.ServiceProvider.GetRequiredService<IRoleRepository>();
                var role = await roleRepository.GetRoleWithPermissionsAsync(roleId);
                return role?.Permissions.ToList() ?? new List<Permission>();
            }
        }
    }
}