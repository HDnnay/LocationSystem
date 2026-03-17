using AutoMapper;
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities;
using Dtos = LocationSystem.Application.Dtos;
using MenuModels = LocationSystem.Application.Features.Menus.Models;

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
            var permissions = await _permissionRepository.GetPermissionTreeAsync();
            return _mapper.Map<List<Dtos.PermissionDto>>(permissions);
        }
    }

    public class MenuDataLoader : BatchDataLoader<Guid, Menu>
    {
        private readonly IServiceProvider _serviceProvider;

        public MenuDataLoader(IBatchScheduler batchScheduler, IServiceProvider serviceProvider) : base(batchScheduler, GetOptions())
        {
            _serviceProvider = serviceProvider;
        }

        private static DataLoaderOptions GetOptions()
        {
            return new DataLoaderOptions();
        }

        protected override async Task<IReadOnlyDictionary<Guid, Menu>> LoadBatchAsync(IReadOnlyList<Guid> menuIds, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var menuRepository = scope.ServiceProvider.GetRequiredService<IMenuRepository>();
                var menus = await menuRepository.GetByIdsAsync(menuIds);
                var result = new Dictionary<Guid, Menu>();

                foreach (var menu in menus)
                {
                    result[menu.Id] = menu;
                }

                // 确保所有请求的菜单都有结果
                foreach (var menuId in menuIds)
                {
                    if (!result.ContainsKey(menuId))
                    {
                        throw new Exception($"Menu with id {menuId} not found");
                    }
                }

                return result;
            }
        }
    }

    public class PermissionDataLoader : BatchDataLoader<Guid, List<Dtos.PermissionDto>>
    {
        private readonly IServiceProvider _serviceProvider;

        public PermissionDataLoader(IBatchScheduler batchScheduler, IServiceProvider serviceProvider) : base(batchScheduler, GetOptions())
        {
            _serviceProvider = serviceProvider;
        }

        private static DataLoaderOptions GetOptions()
        {
            return new DataLoaderOptions();
        }

        protected override async Task<IReadOnlyDictionary<Guid, List<Dtos.PermissionDto>>> LoadBatchAsync(IReadOnlyList<Guid> menuIds, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var menuRepository = scope.ServiceProvider.GetRequiredService<IMenuRepository>();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                var menus = await menuRepository.GetByIdsWithPermissionsAsync(menuIds.ToList());
                var result = new Dictionary<Guid, List<Dtos.PermissionDto>>();

                foreach (var menu in menus)
                {
                    var permissions = menu.PermissionMenus.Select(pm => pm.Permission).ToList();
                    result[menu.Id] = mapper.Map<List<Dtos.PermissionDto>>(permissions);
                }

                // 确保所有请求的菜单都有结果
                foreach (var menuId in menuIds)
                {
                    if (!result.ContainsKey(menuId))
                    {
                        result[menuId] = new List<Dtos.PermissionDto>();
                    }
                }

                return result;
            }
        }
    }

    public class UserRolesDataLoader : BatchDataLoader<Guid, List<Role>>
    {
        private readonly IServiceProvider _serviceProvider;

        public UserRolesDataLoader(IBatchScheduler batchScheduler, IServiceProvider serviceProvider) : base(batchScheduler, GetOptions())
        {
            _serviceProvider = serviceProvider;
        }

        private static DataLoaderOptions GetOptions()
        {
            return new DataLoaderOptions();
        }

        protected override async Task<IReadOnlyDictionary<Guid, List<Role>>> LoadBatchAsync(IReadOnlyList<Guid> userIds, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var users = await userRepository.GetByIdsWithRolesAsync(userIds.ToList());

                var result = new Dictionary<Guid, List<Role>>();
                foreach (var user in users)
                {
                    result[user.Id] = user.Roles.ToList();
                }

                // 确保所有请求的用户都有结果
                foreach (var userId in userIds)
                {
                    if (!result.ContainsKey(userId))
                    {
                        result[userId] = new List<Role>();
                    }
                }

                return result;
            }
        }
    }

    public class RolePermissionsDataLoader : BatchDataLoader<Guid, List<Permission>>
    {
        private readonly IServiceProvider _serviceProvider;

        public RolePermissionsDataLoader(IBatchScheduler batchScheduler, IServiceProvider serviceProvider) : base(batchScheduler, GetOptions())
        {
            _serviceProvider = serviceProvider;
        }

        private static DataLoaderOptions GetOptions()
        {
            return new DataLoaderOptions();
        }

        protected override async Task<IReadOnlyDictionary<Guid, List<Permission>>> LoadBatchAsync(IReadOnlyList<Guid> roleIds, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var roleRepository = scope.ServiceProvider.GetRequiredService<IRoleRepository>();
                var roles = await roleRepository.GetRolesWithPermissionsByIdsAsync(roleIds.ToList());

                var result = new Dictionary<Guid, List<Permission>>();
                foreach (var role in roles)
                {
                    result[role.Id] = role.Permissions.ToList();
                }

                // 确保所有请求的角色都有结果
                foreach (var roleId in roleIds)
                {
                    if (!result.ContainsKey(roleId))
                    {
                        result[roleId] = new List<Permission>();
                    }
                }

                return result;
            }
        }
    }
}