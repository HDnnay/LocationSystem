using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Utilities
{
    public interface IRequestHandler<TRequset,TResponse>
    {
        Task<TResponse> Handle(TRequset request);
    }
}
