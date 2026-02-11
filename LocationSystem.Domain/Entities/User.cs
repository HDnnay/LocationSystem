﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using LocationSystem.Domain.Exceptions;
using LocationSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Domain.Entities
{
    public abstract class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public string PasswordHash { get; set; }
        public UserType UserType { get; protected set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        
        // 导航属性
        public ICollection<Role> Roles { get; private set; } = new List<Role>();
        
        protected User() { }
        public User(string name, Email email)
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
        public virtual void UpdataUserType(UserType userType)
        {
            UserType = userType;
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
    }
}
