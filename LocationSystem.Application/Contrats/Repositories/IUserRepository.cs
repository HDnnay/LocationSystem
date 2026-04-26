using LocationSystem.Application.Dtos.Users;
using LocationSystem.Application.Features.Users.Queries;
using LocationSystem.Application.GrapqLDTOs.Users;
using LocationSystem.Domain.Entities.UserRolePermissions;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<(int, IEnumerable<UserDto>)> GetUserPage(GetAllUsersQuery query);
        Task SaveRefreshToken(Guid id, string refreshToken);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<User?> GetByIdWithRolesAsync(Guid userId);
        Task<List<User>> GetByIdsWithRolesAsync(List<Guid> userIds);
        Task<User?> DeleteUserAsync(Guid UserId);

        Task<Dictionary<Guid, UserGraphqLDto>> GetUserByIds(IReadOnlyList<Guid> ids);
    }
}
