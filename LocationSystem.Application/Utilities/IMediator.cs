using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Utilities
{
    public interface IMediator
    {
        Task<TResponse> Send<TResponse>(IRequset<TResponse> request);
    }
}
