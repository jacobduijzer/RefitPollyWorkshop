namespace RefitExample.Models
{
    public enum PolicyType
    {
        None = 0,
        Retry = 1 << 0,
        Fallback = 1 << 1,
        RetryWithFallBack = 1 << 2,
        CircuitBreaker = 1 << 3,
        CircuitBreakerWithFallBack = 1 << 4,
        CircuitBreakerWithRetryAndFallBack = 1 << 5,
        AnyFallback = Fallback | RetryWithFallBack | CircuitBreakerWithFallBack | CircuitBreakerWithFallBack    }
}
