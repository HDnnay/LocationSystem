using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos.Users;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Extentions;
using LocationSystem.Application.Features.Users.Queries;
using LocationSystem.Domain.Entities.UserRolePermissions;
using LocationSystem.Infrastructure.Utilities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace LocationSystem.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public new Task<User?> GetByIdAsync(Guid userId)
        {
            return _context.Users.Include(t => t.Roles).FirstOrDefaultAsync(t => t.Id ==userId);
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Value == email);
        }

        public async Task<(int, IEnumerable<UserDto>)> GetUserPage(GetAllUsersQuery query)
        {
            var querable = _context.Users.AsQueryable().AsNoTracking();
            if (query.FilterDelete.HasValue)
            {
                querable = querable.Where(t => t.IsDelete == true);
            }
            else
            {
                querable = querable.Include(u => u.Roles).WhereNotDeleted();
            }

            var total = await querable.CountAsync();
            var users = await querable
                .OrderBy(t => t.CreateTime)
                .Paginate(query.Page, query.PageSize)
                .ToListAsync();

            var userDtos = users.Select(u => u.Adapt<UserDto>()).ToList();

            return (total, userDtos);
        }

        public async Task SaveRefreshToken(Guid id, string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(t => t.Id==id);
            if (user==null)
                throw new ArgumentException("用户不存在");
            user.SetRefreshToken(refreshToken, DateTime.Now.AddDays(7));
            _context.Users.Update(user);
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users
                .Include(u => u.Roles) // 加载角色信息，用于生成 token
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public new async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetByIdWithRolesAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<User>> GetByIdsWithRolesAsync(List<Guid> userIds)
        {
            return await _context.Users
                .Include(u => u.Roles)
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();
        }

        public async Task<User?> DeleteUserAsync(Guid userId)
        {
            var user = await GetByIdAsync(userId);
            if (user==null)
                throw new NotFoundException("删除的用户不存在！");
            user.IsDelete = true;
            _context.Update(user);
            return user;
        }


    }
}
