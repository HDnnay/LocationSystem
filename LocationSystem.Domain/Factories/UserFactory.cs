using LocationSystem.Domain.Entities;
using LocationSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace LocationSystem.Domain.Factories
{
    public class UserFactory
    {
        private readonly Dictionary<UserType, Func<string, Email, string, User>> _userCreators;

        public UserFactory()
        {
            _userCreators = new Dictionary<UserType, Func<string, Email, string, User>>
            {
                { UserType.Dentist, (name, email, password) => {
                    var dentist = new Dentist(name, email);
                    dentist.SetPasswordHash(password);
                    return dentist;
                }},
                { UserType.Patient, (name, email, password) => {
                    var patient = new Patient(name, email);
                    patient.SetPasswordHash(password);
                    return patient;
                }}
            };
        }

        public User CreateUser(UserType userType, string name, string email, string password)
        {
            if (!_userCreators.TryGetValue(userType, out var creator))
            {
                throw new ArgumentException($"不支持的用户类型: {userType}");
            }

            var emailValue = new Email(email);
            return creator(name, emailValue, password);
        }
    }
}
