using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Users.Queries.GetAllUsers;
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

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Value == email);
        }

        public async Task<IEnumerable<User>> GetUserPage(GetAllUsersQuery query)
        {
            var querable = _context.Users.AsQueryable().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(query.keyWord))
            {
                querable = querable.Where(t => t.Name.Contains(query.keyWord) || t.Email.Value.Contains(query.keyWord));
            }
            return await querable.Include(u => u.Roles)
                .OrderBy(t => t.Name)
                .Paginate(query.Page, query.PageSize)
                .ToListAsync();
        }

        public async Task SaveRefreshToken(Guid id, string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(t => t.Id==id);
            if (user==null)
                throw new ArgumentException("���û�������");
            user.SetRefreshToken(refreshToken,DateTime.Now.AddDays(7));
            _context.Users.Update(user);
        }
    }
}
