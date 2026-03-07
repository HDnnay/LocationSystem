using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Permissions.Queries.GetPermissionTreeWithCheckStatus;
using LocationSystem.Application.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Permissions.Queries.GetMenuPermissionTreeWithCheck
{
    public class GetMenuPermissionTreeWithCheckQueryHandler : IRequestHandler<GetMenuPermissionTreeWithCheckQuery, List<PermissionTreeDto>>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ICacheService _cacheService;
        ILogger<GetMenuPermissionTreeWithCheckQueryHandler> _logger;
        public GetMenuPermissionTreeWithCheckQueryHandler(IPermissionRepository permissionRepository, ICacheService cacheService, ILogger<GetMenuPermissionTreeWithCheckQueryHandler> logger)
        {
            _permissionRepository = permissionRepository;
            _cacheService = cacheService;
            _logger = logger;
        }
        public async Task<List<PermissionTreeDto>> Handle(GetMenuPermissionTreeWithCheckQuery request)
        {
            var mdoel =await _permissionRepository.GetMenuPermissionTreeWithCheck(request.MenuId);
            return mdoel;
        }
    }
}
