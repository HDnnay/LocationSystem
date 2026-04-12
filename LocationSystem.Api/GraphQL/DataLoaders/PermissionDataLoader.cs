using HotChocolate;
using Mapster;
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
                var mapper = scope.ServiceProvider.GetRequiredService<MapsterMapper.IMapper>();

                var menus = await menuRepository.GetByIdsWithPermissionsAsync(menuIds.ToList());
                var result = new Dictionary<Guid, List<PermissionDto>>();

                foreach (var menu in menus)
                {
                    var permissions = menu.PermissionMenus.Select(pm => pm.Permission).ToList();
                    result[menu.Id] = permissions.Adapt<List<PermissionDto>>();
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