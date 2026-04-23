using GreenDonut;
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities.Menus;

namespace LocationSystem.Api.GraphQL.DataLoaders
{
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
}