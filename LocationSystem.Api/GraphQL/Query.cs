using AutoMapper;
using LocationSystem.Api.GraphQL.DataLoaders;
using LocationSystem.Api.GraphQL.Types;
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Articles.Queries.GetArticle;
using LocationSystem.Application.Features.Articles.Queries.GetArticles;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.Menus;
using Dtos = LocationSystem.Application.Dtos;

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
        private readonly IMediator _mediator;

        public Query(IMenuRepository menuRepository, IUserRepository userRepository, IRoleRepository roleRepository, IPermissionRepository permissionRepository, MenuDataLoader menuDataLoader, IMapper mapper, IMediator mediator)
        {
            _menuRepository = menuRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _menuDataLoader = menuDataLoader;
            _mapper = mapper;
            _mediator = mediator;
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
        public async Task<List<Dtos.MenuDto>> GetMenuTree()
        {
            var menus = await _menuRepository.GetMenuTreeAsync();
            return _mapper.Map<List<Dtos.MenuDto>>(menus);
        }

        [GraphQLDescription("获取用户列表")]
        [GraphQLType(typeof(ListType<LocationSystem.Api.GraphQL.Types.UserType>))]
        public async Task<List<Dtos.UserDto>> GetUsers()
        {
            var users = await _userRepository.GetAll();
            return _mapper.Map<List<Dtos.UserDto>>(users);
        }

        [GraphQLDescription("获取用户详情")]
        [GraphQLType(typeof(LocationSystem.Api.GraphQL.Types.UserType))]
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
        [GraphQLType(typeof(ListType<LocationSystem.Api.GraphQL.Types.RoleType>))]
        public async Task<List<Dtos.RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetRolesWithPermissionsAsync();
            return _mapper.Map<List<Dtos.RoleDto>>(roles);
        }

        [GraphQLDescription("获取角色详情")]
        [GraphQLType(typeof(LocationSystem.Api.GraphQL.Types.RoleType))]
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
        [GraphQLType(typeof(ListType<LocationSystem.Api.GraphQL.Types.PermissionType>))]
        public async Task<List<Dtos.PermissionDto>> GetPermissions()
        {
            var permissions = await _permissionRepository.GetPermissionTreeAsync();
            return _mapper.Map<List<Dtos.PermissionDto>>(permissions);
        }

        [UsePaging(IncludeTotalCount = true)]
        [UseSorting]
        [UseFiltering]
        [GraphQLDescription("获取文章列表")]
        public async Task<IQueryable<Dtos.ArticleDto>> GetArticles(
            [GraphQLDescription("排序字段")] string? sortBy = null,
            [GraphQLDescription("是否降序排序")] bool? sortDescending = null)
        {
            var articles = await _mediator.Send(new GetArticlesQuery { SortBy = sortBy, SortDescending = sortDescending });
            return articles.Select(a => _mapper.Map<Dtos.ArticleDto>(a)).AsQueryable();
        }

        [GraphQLDescription("获取文章详情")]
        [GraphQLType(typeof(ArticleType))]
        public async Task<Dtos.ArticleDto> GetArticle(
            [GraphQLDescription("文章ID")] Guid id)
        {
            return await _mediator.Send(new GetArticleQuery { Id = id });
        }
    }


}