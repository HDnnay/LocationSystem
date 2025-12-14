using LocationSystem.Domain.Exceptions;
using LocationSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Domain.Entities
{
    /// <summary>
    /// 患者
    /// </summary>
    public class Patient:User
    {
        
        private Patient():base() { }
        public Patient(string name, Email email):base(name,email)
        {
            ValidatorName(name);
            UserType = UserType.Patient;
        }

        private static void ValidatorName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BussinessRuleException($"{nameof(name)}的为空");
            }
        }
        private static void ValidatorEmail(Email email)
        {
            if (email is null)
            {
                throw new BussinessRuleException($"{nameof(email)}的为空");
            }
        }

        public override void UpdateName(string name) 
        {
            ValidatorName(name);
            base.UpdateName(name);
        }
        public override void UpdateEmail(Email email) 
        {
            ValidatorEmail(email);
            base.UpdateEmail(email);
        }
        public override void SetPasswordHash(string passwordHash)
        {
            base.SetPasswordHash(passwordHash);
        }
    }
}
