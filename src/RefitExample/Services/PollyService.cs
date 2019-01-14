using Polly;
using RefitExample.Interfaces;
using RefitExample.Models;
using System;
using System.Threading.Tasks;

namespace RefitExample.Services
{
    public class PollyService
    {
        private readonly ILogger _logger;

        private readonly IAsyncPolicy _circuitBreaker;

        private const int NUMBER_OF_RETRIES = 3;

        private const int EXCEPTIONS_ALLOWED_BEFORE_BREAKING_CIRCUIT = 3;

        public PollyService(ILogger logger, int circuitBreakingTimeInMs = 1000)
        {
            _logger = logger;

            _circuitBreaker = Policy.Handle<Exception>()
                .CircuitBreakerAsync(
                    EXCEPTIONS_ALLOWED_BEFORE_BREAKING_CIRCUIT,
                    TimeSpan.FromMilliseconds(circuitBreakingTimeInMs),
                    (x, y) => _logger.Write("Breaking circuit"),
                    () => _logger.Write("Resetting circuit"));
        }

        public async Task<T> GetWithPolicy<T>(PolicyTypes policyType, Func<Task<T>> apiCall, Func<Task<T>> fallbackCall)
        {
            if ((policyType & (PolicyTypes.AnyFallback)) != 0 && fallbackCall == null)
                throw new ArgumentNullException(nameof(fallbackCall));

            return await ExecuteWithPolicy<T>(policyType, apiCall, fallbackCall);
        }

        private async Task<T> ExecuteWithPolicy<T>(PolicyTypes policyType, Func<Task<T>> apiCall, Func<Task<T>> fallbackCall)
        {
            _logger.Write($"GetWithPolicy: {policyType}");
            switch (policyType)
            {
                case PolicyTypes.Retry:
                    return await CreateRetryPolicy()
                        .ExecuteAsync(apiCall);

                case PolicyTypes.CircuitBreaker:
                    return await _circuitBreaker
                        .ExecuteAsync(apiCall);

                case PolicyTypes.Fallback:
                    return await CreateFallbackPolicy<T>(fallbackCall)
                        .ExecuteAsync(apiCall);

                case PolicyTypes.CircuitBreakerWithFallBack:
                    return await CreateFallbackPolicy<T>(fallbackCall)
                        .WrapAsync(_circuitBreaker)
                        .ExecuteAsync(apiCall);

                case PolicyTypes.RetryWithFallBack:
                    return await CreateFallbackPolicy<T>(fallbackCall)
                        .WrapAsync(CreateRetryPolicy())
                        .ExecuteAsync(apiCall);

                case PolicyTypes.CircuitBreakerWithRetryAndFallBack:
                    return await CreateFallbackPolicy<T>(fallbackCall)
                        .WrapAsync(CreateRetryPolicy())
                        .WrapAsync(_circuitBreaker)
                        .ExecuteAsync(apiCall);

                default:
                    return await apiCall.Invoke();
            }
        }

        private IAsyncPolicy CreateRetryPolicy() =>
            Policy.Handle<Exception>()
            .RetryAsync(NUMBER_OF_RETRIES, (exception, retryCount, context) =>
            {
                _logger.Write("RetryPolicy invoked");
            });

        private IAsyncPolicy<T> CreateFallbackPolicy<T>(Func<Task<T>> fallbackCall) =>
            Policy<T>.Handle<Exception>()
            .FallbackAsync((cancellationToken) =>
            {
                _logger.Write("FallbackPolicy invoked");
                return fallbackCall.Invoke();
            });
    }
}