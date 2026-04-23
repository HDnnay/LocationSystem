using LocationSystem.Application.Dtos.Users;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Presentation.DataLoaders
{
    public static class UserDataLoader
    {
        public static async Task<Dictionary<Guid, UserDto>> GetUserByIdAsync(IReadOnlyList<int> ids, [Service] IMediator mediator, CancellationToken cancellationToken)
        {
            //// 构造查询并发送
            //var query = new GetBooksByIdsQuery(ids);
            //var result = await mediator.Send(query, cancellationToken);

            //// 直接返回由Handler计算出的字典
            //return result;
        }
    }
}
