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
        
        // 内存中的refresh token存储
        private static readonly Dictionary<Guid, string> _refreshTokens = new Dictionary<Guid, string>();

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
            _refreshTokens[userId] = refreshToken;
        }

        public async Task<string?> GetRefreshToken(Guid userId)
        {
            _refreshTokens.TryGetValue(userId, out var refreshToken);
            return refreshToken;
        }
    }
}