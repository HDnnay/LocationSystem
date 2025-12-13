using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Exceptions
{
    public class CustomVallidatorException:Exception
    {
        public List<string> ValidationError { get; set; } = new();
        public CustomVallidatorException(string errMessage)
        {
            ValidationError.Add(errMessage);
        }
        public CustomVallidatorException(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                ValidationError.Add(error.ErrorMessage);
            }
        }
    }
}
