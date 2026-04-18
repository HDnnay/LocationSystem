using LocationSystem.Domain.Entities.Interfacies;
using LocationSystem.Domain.Exceptions;
using LocationSystem.Domain.ValueObjects;

namespace LocationSystem.Domain.Entities.UserRolePermissions
{
    public abstract class User : ISoftDeleteEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public string PasswordHash { get; set; }
        //- 实体鉴别器 ：Entity Framework Core 使用鉴别器列来区分继承层次结构中的不同实体类型
        // 只读特性 ：一旦实体被保存到数据库，鉴别器属性就变成只读的，因为它决定了实体在数据库中的类型
        public UserType UserType { get; protected set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        // 导航属性
        public ICollection<Role> Roles { get; private set; } = new List<Role>();
        public bool IsDelete { get; set; }
        public Guid DeleteUserId { get; set; }
        public DateTime DeleteTime { get; set; }
        public bool IsDisabled { get; set; }

        public DateTime CreateTime { get; set; }

        protected User() { }
        public User(string name, Email email, bool isSuperAdmin = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BussinessRuleException($"{nameof(name)}的为空");
            }

            Name = name;
            Email = email;
            Id = Guid.NewGuid();
        }
        public virtual void UpdateName(string name)
        {
            Name = name;
        }
        public virtual void UpdateEmail(Email email)
        {
            Email = email;
        }
        public virtual void SetPasswordHash(string passwordHash)
        {
            if (passwordHash.Length < 6)
                throw new BussinessRuleException("密码长度不能于6位");
            passwordHash = BCrypt.Net.BCrypt.HashPassword(passwordHash);
            PasswordHash = passwordHash;
        }
        public virtual void SetRefreshToken(string refreshToken, DateTime expiryTime)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = expiryTime;
        }
        public virtual void ClearRefreshToken()
        {
            RefreshToken = null;
            RefreshTokenExpiryTime = null;
        }
        public void AddRole(Role role)
        {
            if (!Roles.Contains(role))
            {
                Roles.Add(role);
            }
        }

        public void RemoveRole(Role role)
        {
            Roles.Remove(role);
        }

        public void ClearRoles()
        {
            Roles.Clear();
        }

        public void Disable()
        {
            IsDisabled = true;
        }

        public void Enable()
        {
            IsDisabled = false;
        }
    }
}
