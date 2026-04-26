using LocationSystem.Presentation.GraphQL;
using Microsoft.Extensions.DependencyInjection;

namespace LocationSystem.Presentation
{
    public static class PresentationServiceExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            _=services
            .AddGraphQLServer()
            .AddQueryType<QueryType>()
            .AddMutationType<MutationType>()
            .AddType<Models.UserType>()
            .AddCostAnalyzer()
            .ModifyCostOptions(options =>
            {
                // 提高最大字段成本（默认 1000）
                options.MaxFieldCost = 10000;   // 或更大，如 10000
                // 也可以提高最大类型成本
                options.MaxTypeCost = 10000;
                // 降低成本计算乘数（如果你的查询使用了 filtering/sorting）
                // 这样可以降低带变量的列表查询的预估成本
                options.Filtering.VariableMultiplier = 2;
                options.Sorting.VariableMultiplier = 2;
                // 确保强制启用成本限制（默认 true，保持即可）
                options.EnforceCostLimits = true;

                // 可选：为解析器设置默认成本（默认 10）
                options.DefaultResolverCost = 5;   // 降低默认成本
                options.ApplyCostDefaults = true;
                options.EnforceCostLimits = true;
            })

            .AddSorting()
            .AddFiltering();
            return services;
        }
    }
}
