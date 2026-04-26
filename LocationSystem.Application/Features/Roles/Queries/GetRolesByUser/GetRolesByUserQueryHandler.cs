using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.Utilities;
using Mapster;

namespace LocationSystem.Application.Features.Roles.Queries.GetRolesByUser
{
    public class GetRolesByUserQueryHandler : IRequestHandler<GetRolesByUserQuery, Dictionary<Guid, List<RoleGraphqLDto>>>
    {
        private readonly IRoleRepository _repository;
        public GetRolesByUserQueryHandler(IRoleRepository roleRepository)
        {
            _repository = roleRepository;
        }
        public async Task<Dictionary<Guid, List<RoleGraphqLDto>>> Handle(GetRolesByUserQuery request)
        {
            if (request.Ids == null || !request.Ids.Any())
            {
                return new Dictionary<Guid, List<RoleGraphqLDto>>();
            }

            // 查询用户角色关系
            var result = await _repository.GetRolesByUserIdsAsync(request.Ids);

            // 确保所有请求的用户ID都有对应的条目（即使为空列表）
            foreach (var userId in request.Ids)
            {
                if (!result.ContainsKey(userId))
                {
                    result[userId] = new List<RoleGraphqLDto>();
                }
            }

            return result;
        }
    }
}
