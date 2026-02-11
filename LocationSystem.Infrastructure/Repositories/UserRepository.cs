using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Auth.Login;
using LocationSystem.Domain.Entities;
using LocationSystem.Domain.ValueObjects;
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

        public async Task<Dentist?> GetDentistByEmail(string email)
        {
            return await _context.Dentists.FirstOrDefaultAsync(d => d.Email.Value == email);
        }

        public async Task<Patient?> GetPatientByEmail(string email)
        {
            return await _context.Patients.FirstOrDefaultAsync(p => p.Email.Value == email);
        }

        public async Task SaveRefreshToken(Guid userId, string refreshToken)
        {
            // 查找用户
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                // 设置refresh token和过期时间（默认7天）
                user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string?> GetRefreshToken(Guid userId)
        {
            // 查找用户
            var user = await _context.Users.FindAsync(userId);
            if (user != null && user.RefreshTokenExpiryTime > DateTime.UtcNow)
            {
                return user.RefreshToken;
            }
            return null;
        }

        public async Task AddAsync(User user)
        {
            // 保持现有的通用方法
            await base.AddAsync(user);
        }

        public async Task AddDentistAsync(Dentist dentist)
        {
            // 专门添加牙医的方法
            await _context.Dentists.AddAsync(dentist);
            await _context.SaveChangesAsync();
        }

        public async Task AddPatientAsync(Patient patient)
        {
            // 专门添加患者的方法
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.Value == email);
        }

        public async Task<bool> IsEmailExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.Value == email);
        }

        public async Task<User?> GetUserWithRolesAndPermissionsAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Roles)
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AssignRolesToUserAsync(Guid userId, IEnumerable<Guid> roleIds)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user != null)
            {
                // 清除现有角色
                user.ClearRoles();
                
                // 添加新角色
                var roles = await _context.Roles
                    .Where(r => roleIds.Contains(r.Id))
                    .ToListAsync();
                
                foreach (var role in roles)
                {
                    user.AddRole(role);
                }
                
                await _context.SaveChangesAsync();
            }
        }
    }
}