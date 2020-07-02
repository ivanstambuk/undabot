using System;
using System.Net;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;

namespace Undabot.Infrastructure.Extensions.Policies
{
    public static class ProductServicePolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> RetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode ==
                    HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt =>
                  TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public static IAsyncPolicy<HttpResponseMessage>
        CircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(3, TimeSpan.FromMinutes(1));
        }
    }

}