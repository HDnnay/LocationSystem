using LocationSystem.Application.Features.Users.Queries.GetAllUsers;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUserPage(GetAllUsersQuery query);
        Task SaveRefreshToken(Guid id, string refreshToken);
    }
}
