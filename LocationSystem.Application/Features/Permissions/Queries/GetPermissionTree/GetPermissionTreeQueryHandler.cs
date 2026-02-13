using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Permissions.Queries.GetPermissionTree;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Permissions.Queries.GetPermissionTree
{
    public class GetPermissionTreeQueryHandler : IRequestHandler<GetPermissionTreeQuery, List<PermissionTreeDto>>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<GetPermissionTreeQueryHandler> _logger;

        public GetPermissionTreeQueryHandler(IPermissionRepository permissionRepository, ICacheService cacheService, ILogger<GetPermissionTreeQueryHandler> logger)
        {
            _permissionRepository = permissionRepository;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<List<PermissionTreeDto>> Handle(GetPermissionTreeQuery request)
        {
            // 生成缓存键
            var cacheKey = CacheKeys.PermissionTree;

            try
            {
                _logger.LogInformation($"开始处理权限树查询，缓存键: {cacheKey}");
                var startTime = DateTime.Now;

                // 从缓存中获取权限树或创建缓存
                var permissionDtos = await _cacheService.GetOrCreateAsync<List<PermissionTreeDto>>(cacheKey, async (options) => {
                    _logger.LogInformation($"缓存未命中，从数据库获取权限树DTO");
                    var dbStartTime = DateTime.Now;
                    
                    // 直接从数据库获取专门的权限树DTO，只包含前端需要的字段
                    var dtos = await _permissionRepository.GetPermissionTreeDtosAsync();
                    
                    var dbEndTime = DateTime.Now;
                    _logger.LogInformation($"从数据库获取权限树DTO完成，耗时: {(dbEndTime - dbStartTime).TotalMilliseconds}ms");
                    
                    return dtos;
                }, 1800); // 缓存30分钟（1800秒）

                var endTime = DateTime.Now;
                _logger.LogInformation($"权限树查询处理完成，总耗时: {(endTime - startTime).TotalMilliseconds}ms");
                return permissionDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理权限树查询时发生错误，缓存键: {cacheKey}");
                throw;
            }
        }
    }
}
