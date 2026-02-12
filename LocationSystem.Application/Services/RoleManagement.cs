using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationSystem.Application.Services
{
    public class RoleManagement
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public RoleManagement(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new List<Role>();
            }

            return user.Roles;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAll();
        }

        public async Task<Role?> GetRoleByIdAsync(Guid roleId)
        {
            return await _roleRepository.GetByIdAsync(roleId);
        }

        public async Task<Role?> GetRoleWithPermissionsAsync(Guid roleId)
        {
            return await _roleRepository.GetRoleWithPermissionsAsync(roleId);
        }

        public async Task<IEnumerable<Role>> GetRolesWithPermissionsAsync()
        {
            return await _roleRepository.GetRolesWithPermissionsAsync();
        }
    }
}