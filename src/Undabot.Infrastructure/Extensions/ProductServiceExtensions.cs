using System;
using Microsoft.Extensions.DependencyInjection;
using Undabot.Domain.Entities;
using Undabot.Domain.Services;
using Undabot.Infrastructure.Extensions.Policies;

namespace Undabot.Infrastructure.Extensions
{
    public static class ProductServiceExtensions
    {
        public static IServiceCollection AddProductService(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();

            services.AddHttpClient<IProductRepository, ProductRepository>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(2))
                .AddPolicyHandler(ProductServicePolicies.RetryPolicy())
                .AddPolicyHandler(ProductServicePolicies.CircuitBreakerPolicy());

            return services;
        }
    }
}