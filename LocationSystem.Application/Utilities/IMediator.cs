using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Utilities
{
    public interface IMediator
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request);
        Task Send(IRequset requset);
    }
}
