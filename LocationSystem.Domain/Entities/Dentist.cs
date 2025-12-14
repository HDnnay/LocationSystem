using LocationSystem.Domain.Exceptions;
using LocationSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Domain.Entities
{
    /// <summary>
    /// 牙医
    /// </summary>
    public class Dentist:User
    {
        public Dentist():base()
        {
        }
        public string DenetistCode { get; private set; } = null!;
        public Dentist(string name, Email email) : base(name, email)
        {
           
        }
        public Dentist(string name, Email email,string code):base(name,email)
        {
            DenetistCode = code;
        }
        public void ChangeName(string name)
        {
            UpdateName(name);
        }
       
    }
}
