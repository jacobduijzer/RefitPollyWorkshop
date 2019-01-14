using System;

namespace RefitExample.Models
{
    [Flags]
    public enum PolicyTypes
    {
        None = 0,
        Retry = 1 << 1,
        Fallback = 1 << 2,
        RetryWithFallBack = 1 << 3,
        CircuitBreaker = 1 << 4,
        CircuitBreakerWithFallBack = 1 << 5,
        CircuitBreakerWithRetryAndFallBack = 1 << 6,
        AnyFallback = Fallback | RetryWithFallBack | CircuitBreakerWithFallBack | CircuitBreakerWithRetryAndFallBack
    }
}