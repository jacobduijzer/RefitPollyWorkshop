namespace RefitExample.Models
{
    public enum PolicyType
    {
        None,
        Retry,
        Fallback,
        RetryWithFallBack,
        CircuitBreaker,
        CircuitBreakerWithFallBack,
        CircuitBreakerWithRetryAndFallBack
    }
}
