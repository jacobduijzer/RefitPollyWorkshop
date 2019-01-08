using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Polly;
using Refit;
using RefitExample.Helpers;
using RefitExample.Interfaces;
using RefitExample.Models;

namespace RefitExample.Services
{
    public class PollyService
    {
        private readonly ILogger _logger;        

        private readonly IAsyncPolicy _circuitBreaker;
        
        private const int NUMBER_OF_RETRIES = 3;

        public PollyService(ILogger logger)
        {
            _logger = logger;

            _circuitBreaker = Policy.Handle<Exception>()
                .CircuitBreakerAsync(
                    3,
                    TimeSpan.FromSeconds(1),
                    (x, y) => _logger.Write("Breaking circuit"),
                    () => _logger.Write("Resetting circuit"));
        }


        public async Task<T> GetWithPolicy<T>(PolicyType policyType, Func<Task<T>> apiCall, Func<Task<T>> fallbackCall)
        {
            _logger.Write($"GetWithPolicy: {policyType}");
            switch (policyType)
            {
                case PolicyType.Retry:
                    return await CreateRetryPolicy<T>()
                        .ExecuteAsync(apiCall);

                //case PolicyType.CircuitBreaker:
                //    return await _circuitBreaker.ExecuteAsync(apiCall);
                //    break;
                case PolicyType.Fallback:
                    if (fallbackCall == null)
                        throw new ArgumentNullException(nameof(fallbackCall));

                    return await CreateFallbackPolicy<T>(fallbackCall)
                        .ExecuteAsync(apiCall);

                //    break;
                //case PolicyType.CircuitBreakerWithFallBack:
                //    return await fallbackPolicy.WrapAsync(_circuitBreaker).ExecuteAsync(apiCall);
                //    break;
                case PolicyType.RetryWithFallBack:
                    if (fallbackCall == null)
                        throw new ArgumentNullException(nameof(fallbackCall));

                    return await CreateFallbackPolicy<T>(fallbackCall)
                        .WrapAsync(CreateRetryPolicy<T>())
                        .ExecuteAsync(apiCall);
                    //case PolicyType.CircuitBreakerWithRetryAndFallBack:
                    //    return  await retryPolicy.WrapAsync(fallbackPolicy).WrapAsync(_circuitBreaker).ExecuteAsync(apiCall);
                    //    break;
                    //default:
                    //    return await apiCall.Invoke();
                    //    break;
            }

            throw new NotImplementedException();
        }

        private IAsyncPolicy<T> CreateRetryPolicy<T>() =>
            Policy<T>.Handle<Exception>()
            .RetryAsync(NUMBER_OF_RETRIES, (exception, retryCount, context) =>
            {
                _logger.Write("RetryPolicy invoked");
            });

        private IAsyncPolicy<T> CreateFallbackPolicy<T>(Func<Task<T>> fallbackCall) =>
            Policy<T>.Handle<Exception>()
            .FallbackAsync((cancellationToken) =>
            {
                _logger.Write("Fallback invoked");
                return fallbackCall.Invoke();
            });
    }
}
