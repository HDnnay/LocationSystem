using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Exceptions
{
    public class CustomVallidatorException:Exception
    {
        public List<string> ValidationError { get; set; } = new();
        public CustomVallidatorException(string errMessage) : base(errMessage)
        {
            ValidationError.Add(errMessage);
        }
        public CustomVallidatorException(ValidationResult validationResult) : base(validationResult.Errors.Count > 0 ? string.Join("；", validationResult.Errors.Select(e => e.ErrorMessage)) : "验证失败")
        {
            foreach (var error in validationResult.Errors)
            {
                ValidationError.Add(error.ErrorMessage);
            }
        }
    }
}
