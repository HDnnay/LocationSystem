using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Utilities
{
    public interface IRequsetHandler<TRequset,TResponse>
    {
        Task<TResponse> Handle(TRequset request);
    }
    public interface IRequestHandler<TRequest>
    {
        Task Handle(TRequest request);
    }
}
