using LocationSystem.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Domain.ValueObjects
{
    public record Email
    {
        private Email() { }
        public string Value { get; } = null!;
        public Email(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new BussinessRuleException($"{nameof(email)}的为空");
            }
            if (!email.Contains("@"))
            {
                throw new BussinessRuleException($"{nameof(email)}格式不正确");
            }
            Value = email;
        }
    }
}
