using FluentValidation;
using FluentValidation.Results;
using LocationSystem.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Utilities
{
    public class SimpleMediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;
        public SimpleMediator(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }
        public async Task<TResponse> Send<TResponse>(IRequset<TResponse> requset)
        {
            await ValidationMethod(requset);
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requset.GetType(), typeof(TResponse));
            var handler = _serviceProvider.GetService(handlerType);
            if (handler is null)
                throw new MediatorExpcetion($"{nameof(handler)}为空");
            var method = handlerType.GetMethod("Handle");
            return await (Task<TResponse>)method.Invoke(handler, new object[] { requset })!;

        }

        public async Task Send(IRequset requset)
        {
           await ValidationMethod(requset);
             var handlerType = typeof(IRequestHandler<>).MakeGenericType(requset.GetType());
            var handler = _serviceProvider.GetService(handlerType);
            if (handler is null)
                throw new MediatorExpcetion($"{nameof(handler)}为空");
            var method = handlerType.GetMethod("Handle");
            await (Task)method.Invoke(handler,new object[] { requset})!;
        }

        private async Task ValidationMethod(object requset)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(requset.GetType());
            var validator = _serviceProvider.GetService(validatorType);
            if (validator is not null)
            {
                var validaterMethod = validatorType.GetMethod("ValidateAsync");
                var taskValidate = (Task)validaterMethod!.Invoke(validator, new object[] { requset, CancellationToken.None })!;
                await taskValidate;
                var result = taskValidate.GetType().GetProperty("Result");
                var validationResult = (ValidationResult)result!.GetValue(taskValidate)!; // 使用 FluentValidation.Results.ValidationResult
                if (!validationResult.IsValid) // 现在 IsValid 属性可用
                {
                    throw new CustomVallidatorException(validationResult);
                }
            }
        }
    }
}
