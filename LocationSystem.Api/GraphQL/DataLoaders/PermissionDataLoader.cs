using AutoMapper;
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;

namespace LocationSystem.Api.GraphQL.DataLoaders
{
    public class PermissionDataLoader : BatchDataLoader<Guid, List<PermissionDto>>
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

        protected override async Task<IReadOnlyDictionary<Guid, List<PermissionDto>>> LoadBatchAsync(IReadOnlyList<Guid> menuIds, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var menuRepository = scope.ServiceProvider.GetRequiredService<IMenuRepository>();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                var menus = await menuRepository.GetByIdsWithPermissionsAsync(menuIds.ToList());
                var result = new Dictionary<Guid, List<PermissionDto>>();

                foreach (var menu in menus)
                {
                    var permissions = menu.PermissionMenus.Select(pm => pm.Permission).ToList();
                    result[menu.Id] = mapper.Map<List<PermissionDto>>(permissions);
                }

                // 确保所有请求的菜单都有结果
                foreach (var menuId in menuIds)
                {
                    if (!result.ContainsKey(menuId))
                    {
                        result[menuId] = new List<PermissionDto>();
                    }
                }

                return result;
            }
        }
    }
}