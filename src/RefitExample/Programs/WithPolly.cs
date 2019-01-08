using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RefitExample.Interfaces;
using RefitExample.Loggers;
using RefitExample.Models;
using RefitExample.Services;

namespace RefitExample.Programs
{
    public class WithPolly
    {
        private readonly IRemoteApiService _remoteApiService;
        private readonly PollyService _pollyService;
        private readonly ILogger _logger;

        public WithPolly(IRemoteApiService remoteApiService)
        {
            _logger = new ConsoleLogger();
            _remoteApiService = remoteApiService;
            _pollyService = new PollyService(new ConsoleLogger());
        }

        private const int DELAY = 5000; // 0 gives issues with json-server
        private const int FALLBACK_DELAY = 1; // 0 gives issues with json-server

        public async Task GetAllPostsWithRetry()
        {
            try
            {
                _logger.Write(Program.SEPARATOR);
                _logger.Write($"Getting all posts with retry");
                var posts = await _pollyService.GetWithPolicy<IEnumerable<Post>>(
                    PolicyType.Retry,
                    () => _remoteApiService.GetAllPostsAsync(DELAY), null).ConfigureAwait(false);
                _logger.Write($"result count: {posts.Count()}");
            }
            catch (Exception)
            {
                _logger.Write("Expected exception, timeout");
            }
        }

        public async Task GetAllPostsWithFallback()
        {
            _logger.Write(Program.SEPARATOR);
            _logger.Write($"Getting all posts with fallback");
            var posts = await _pollyService.GetWithPolicy<IEnumerable<Post>>(
                    PolicyType.Fallback,
                    () => _remoteApiService.GetAllPostsAsync(DELAY),
                    () => _remoteApiService.GetAllPostsAsync(FALLBACK_DELAY)).ConfigureAwait(false);
            _logger.Write($"result count: {posts.Count()}");
        }

        public async Task GetAllPostsWithRetryAndFallBack()
        {
            _logger.Write(Program.SEPARATOR);
            _logger.Write($"Getting all posts with retry & fallback");
            var posts = await _pollyService.GetWithPolicy<IEnumerable<Post>>(
                    PolicyType.RetryWithFallBack,
                    () => _remoteApiService.GetAllPostsAsync(DELAY),
                    () => _remoteApiService.GetAllPostsAsync(FALLBACK_DELAY)).ConfigureAwait(false);
            _logger.Write($"result count: {posts.Count()}");
        }

        public async Task GetAllPostsWithCircuitBreaker()
        {
            _logger.Write(Program.SEPARATOR);
            _logger.Write($"Getting all posts with circuit breaker");
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    var posts = await _pollyService.GetWithPolicy<IEnumerable<Post>>(
                        PolicyType.CircuitBreaker,
                        () => _remoteApiService.GetAllPostsAsync(DELAY), 
                        null).ConfigureAwait(false);
                    _logger.Write($"result count: {posts.Count()}");
                }
                catch (Exception ex)
                {
                    _logger.Write(ex.Message);
                }                
            }
        }

        public async Task GetAllPostsWithCircuitBreakerWithFallBack()
        {
            _logger.Write(Program.SEPARATOR);
            _logger.Write($"Getting all posts with circuit breaker");
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    var posts = await _pollyService.GetWithPolicy<IEnumerable<Post>>(
                        PolicyType.CircuitBreakerWithFallBack,
                        () => _remoteApiService.GetAllPostsAsync(DELAY),
                        () => _remoteApiService.GetAllPostsAsync(1)).ConfigureAwait(false);
                    _logger.Write($"result count: {posts.Count()}");
                }
                catch (Exception ex)
                {
                    _logger.Write(ex.Message);
                }
            }
        }

        public async Task GetAllPostsWithCircuitBreakerWithRetryAndFallBack()
        {
            _logger.Write(Program.SEPARATOR);
            _logger.Write($"Getting all posts with circuit breaker");
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    var posts = await _pollyService.GetWithPolicy<IEnumerable<Post>>(
                        PolicyType.CircuitBreakerWithRetryAndFallBack,
                        async () => await _remoteApiService.GetAllPostsAsync(DELAY),
                        async () =>
                        {
                            // remove to show multiple breaking circuits
                            //await Task.Delay(500);
                            return await _remoteApiService.GetAllPostsAsync(1);
                        }).ConfigureAwait(false);
                    _logger.Write($"result count: {posts.Count()}");
                }
                catch (Exception ex)
                {
                    _logger.Write(ex.Message);
                }
            }
        }
    }
}
