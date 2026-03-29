using AutoMapper;
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities.Menus;
using LocationSystem.Domain.Entities.UserRolePermissions;
using LocationSystem.Api.GraphQL.DataLoaders;
using Dtos = LocationSystem.Application.Dtos;
using MenuModels = LocationSystem.Application.Features.Menus.Models;
using LocationSystem.Api.GraphQL.Types;

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


}