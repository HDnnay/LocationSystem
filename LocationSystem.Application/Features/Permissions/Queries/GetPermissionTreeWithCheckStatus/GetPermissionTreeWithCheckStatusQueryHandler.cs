using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Permissions.Queries.GetPermissionTreeWithCheckStatus;
using LocationSystem.Application.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Permissions.Queries.GetPermissionTreeWithCheckStatus
{
    public class GetPermissionTreeWithCheckStatusQueryHandler : IRequestHandler<GetPermissionTreeWithCheckStatusQuery, List<PermissionTreeDto>>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<GetPermissionTreeWithCheckStatusQueryHandler> _logger;

        public GetPermissionTreeWithCheckStatusQueryHandler(IPermissionRepository permissionRepository, ICacheService cacheService, ILogger<GetPermissionTreeWithCheckStatusQueryHandler> logger)
        {
            _permissionRepository = permissionRepository;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<List<PermissionTreeDto>> Handle(GetPermissionTreeWithCheckStatusQuery request)
        {
            // 生成缓存键，包含角色ID以区分不同角色的权限状态
            var cacheKey = request.RoleId.HasValue ? 
                $"permissions:tree:check:{request.RoleId.Value}" : 
                "permissions:tree:check:null";

            try
            {
                _logger.LogInformation($"开始处理带选中状态的权限树查询，缓存键: {cacheKey}");
                var startTime = DateTime.Now;

                // 从缓存中获取权限树或创建缓存
                var permissionDtos = await _cacheService.GetOrCreateAsync<List<PermissionTreeDto>>(cacheKey, async (options) => {
                    _logger.LogInformation($"缓存未命中，从数据库获取带选中状态的权限树");
                    var dbStartTime = DateTime.Now;
                    
                    // 直接从数据库获取带选中状态的权限树DTO
                    var dtos = await _permissionRepository.GetPermissionTreeWithCheckStatusAsync(request.RoleId);
                    
                    var dbEndTime = DateTime.Now;
                    _logger.LogInformation($"从数据库获取带选中状态的权限树DTO完成，耗时: {(dbEndTime - dbStartTime).TotalMilliseconds}ms");
                    
                    return dtos;
                }, 1800); // 缓存30分钟（1800秒）

                var endTime = DateTime.Now;
                _logger.LogInformation($"带选中状态的权限树查询处理完成，总耗时: {(endTime - startTime).TotalMilliseconds}ms");
                return permissionDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理带选中状态的权限树查询时发生错误，缓存键: {cacheKey}");
                throw;
            }
        }
    }
}