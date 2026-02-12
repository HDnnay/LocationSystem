using LocationSystem.Application.Features.Users.Queries;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<(int, IEnumerable<User>)> GetUserPage(GetAllUsersQuery query);
        Task SaveRefreshToken(Guid id, string refreshToken);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    }
}
