using LocationSystem.Application.Dtos.Users;
using LocationSystem.Application.Features.Users.Queries.GetUserByIds;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Presentation.DataLoaders
{
    public static class UserDataLoader
    {
        [DataLoader]

        public static async Task<Dictionary<Guid, UserDto>> GetUsersAsync(IReadOnlyList<Guid> ids, [Service] IMediator mediator, CancellationToken cancellationToken)
        {
            //// 构造查询并发送
            var query = new GetUserByIdsQuery(ids);
            var result = await mediator.Send(query);

            // 直接返回由Handler计算出的字典
            return result;
        }
    }
}
