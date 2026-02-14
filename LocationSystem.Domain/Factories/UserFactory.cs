using LocationSystem.Domain.Entities;
using LocationSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace LocationSystem.Domain.Factories
{
    public interface IUserCreationStrategy
    {
        User CreateUser(string name, Email email, UserType userType,bool isSuperAdmin=false);
    }

    public class DefaultUserCreationStrategy : IUserCreationStrategy
    {
        public User CreateUser(string name, Email email, UserType userType, bool isSuperAdmin = false)
        {
            return new DefaultUser(name, email, userType);
        }
    }

    public class AdminUserCreationStrategy : IUserCreationStrategy
    {
        public User CreateUser(string name, Email email, UserType userType, bool isSuperAdmin = false)
        {
            return new AdminUser(name, email, userType, isSuperAdmin);
        }
    }

    public class RegularUserCreationStrategy : IUserCreationStrategy
    {
        public User CreateUser(string name, Email email, UserType userType, bool isSuperAdmin = false)
        {
            return new RegularUser(name, email, userType);
        }
    }

    public class UserFactory
    {
        private readonly Dictionary<UserType, IUserCreationStrategy> _strategies;

        public UserFactory()
        {
            _strategies = new Dictionary<UserType, IUserCreationStrategy>
            {
                { UserType.Default, new DefaultUserCreationStrategy() },
                { UserType.Admin, new AdminUserCreationStrategy() },
                { UserType.User, new RegularUserCreationStrategy() }
            };
        }

        public User CreateUser(UserType userType, string name, string email, string password,bool isSuperAdmin)
        {
            var emailValue = new Email(email);
            
            if (!_strategies.TryGetValue(userType, out var strategy))
            {
                strategy = new DefaultUserCreationStrategy(); // 默认策略
            }

            var user = strategy.CreateUser(name, emailValue, userType, isSuperAdmin);
            user.SetPasswordHash(password);
            return user;
        }
    }
}
