using GameStore.Web.Services.Abstract;
using GameStore.Web.Services.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Web.Middlewares.Extensions
{
    public static class CartProviderExtensions
    {
        public static IApplicationBuilder UseCookieCartProvider(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CartProviderMiddleware>();
        }

        public static IServiceCollection AddCookieCartProvider(this IServiceCollection services)
        {
            services.AddScoped<ICartProvider, CookieCartProvider>();
            return services;
        }
    }
}
