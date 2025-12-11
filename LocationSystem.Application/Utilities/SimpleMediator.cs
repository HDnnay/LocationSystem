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
        public async Task<TResponse> Send<TResponse>(IRequset<TResponse> request)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var handler = _serviceProvider.GetService(handlerType);
            if (handler is null)
                throw new MediatorExpcetion($"{nameof(handler)}为空");
            var method = handlerType.GetMethod("Handle");
            return await (Task<TResponse>)method.Invoke(handler, new object[] { request })!;

        }
    }
}
