using LocationSystem.Application.IServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocationSystem.Application.Utils
{
    public static class CurrentUser
    {
        private static volatile IServiceProvider? _serviceProvider;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private static ICurrentUserService? GetService()
        {
            var sp = _serviceProvider;
            if (sp == null) return null;
            
            using var scope = sp.CreateScope();
            return scope.ServiceProvider.GetService<ICurrentUserService>();
        }

        public static string? UserId => GetService()?.UserId;
        public static string? UserName => GetService()?.UserName;
        public static string? Email => GetService()?.Email;
        public static bool IsAuthenticated => GetService()?.IsAuthenticated ?? false;
        public static IEnumerable<string> Roles => GetService()?.Roles ?? Enumerable.Empty<string>();
    }
}