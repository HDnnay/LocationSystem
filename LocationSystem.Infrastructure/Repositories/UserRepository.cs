using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Users.Queries;
using LocationSystem.Domain.Entities;
using LocationSystem.Infrastructure.Utilities;
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

        public async Task<(int,IEnumerable<User>)> GetUserPage(GetAllUsersQuery query)
        {
            var querable = _context.Users.AsQueryable().AsNoTracking();
           
            return (await querable.CountAsync(),await querable.Include(u => u.Roles)
                .OrderBy(t => t.Name)
                .Paginate(query.Page, query.PageSize)
                .ToListAsync());
        }

        public async Task SaveRefreshToken(Guid id, string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(t => t.Id==id);
            if (user==null)
                throw new ArgumentException("用户不存在");
            user.SetRefreshToken(refreshToken,DateTime.Now.AddDays(7));
            _context.Users.Update(user);
        }
    }
}
