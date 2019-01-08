﻿using FluentAssertions;
using Moq;
using RefitExample.Interfaces;
using RefitExample.Models;
using RefitExample.Services;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace RefitExample.Testss.Services
{
    public class PollyServiceShould
    {
        private readonly ILogger _logger;
        private readonly PollyService _pollyService;

        public PollyServiceShould(ITestOutputHelper output)
        {
            _logger = new TestLogger(output);
            _pollyService = new PollyService(_logger);
        }

        [Fact]
        public void Construct() =>
            _pollyService.Should().BeOfType<PollyService>();

        [Theory]
        [InlineData(PolicyType.CircuitBreakerWithFallBack)]
        [InlineData(PolicyType.CircuitBreakerWithRetryAndFallBack)]
        [InlineData(PolicyType.Fallback)]
        [InlineData(PolicyType.RetryWithFallBack)]
        public void ThrowWhenFallbackIsNull(PolicyType type) =>
            new Func<Task>(async () => await _pollyService.GetWithPolicy<string>(type, () => default(Task<string>), null))
            .Should().Throw<ArgumentNullException>();

        [Fact]
        public void RetryThreeTimesBeforeThrowing()
        {
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.Write(It.Is<string>(y => y.Equals("RetryPolicy invoked")))).Verifiable();
            var pollyService = new PollyService(mockLogger.Object);
            
            var action = new Func<Task>(async () => 
                await pollyService.GetWithPolicy<string>(PolicyType.Retry,
                    () => throw new Exception("BOEM"),
                    null
                ));

            action.Should().Throw<Exception>().WithMessage("BOEM");

            mockLogger.Verify(x => x.Write(It.Is<string>(y => y.Equals("RetryPolicy invoked"))), Times.Exactly(3));
        }

        [Fact]
        public async Task FallbackTest()
        {
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.Write(It.Is<string>(y => y.Equals("FallbackPolicy invoked")))).Verifiable();
            var pollyService = new PollyService(mockLogger.Object);
            
            var result = await pollyService.GetWithPolicy<string>(PolicyType.Fallback,
                    () => throw new Exception("BOEM"),
                    () => Task.FromResult("test") 
                );

            result.Should().Be("test");

            mockLogger.Verify(x => x.Write(It.Is<string>(y => y.Equals("FallbackPolicy invoked"))), Times.Exactly(1));
        }

        [Fact]
        public async Task RetryThreeTimesThenReturnFallback()
        {
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.Write(It.Is<string>(y => y.Equals("FallbackPolicy invoked")))).Verifiable();
            var pollyService = new PollyService(mockLogger.Object);

            var result = await pollyService.GetWithPolicy<string>(PolicyType.RetryWithFallBack,
                    () => throw new Exception("BOEM"),
                    () => Task.FromResult("test")
                );

            result.Should().Be("test");

            mockLogger.Verify(x => x.Write(It.Is<string>(y => y.Equals("RetryPolicy invoked"))), Times.Exactly(3));
            mockLogger.Verify(x => x.Write(It.Is<string>(y => y.Equals("FallbackPolicy invoked"))), Times.Exactly(1));
        }

        [Fact]
        public void BreakCircuitAndThrow()
        {
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.Write(It.IsAny<string>())).Verifiable();
            var pollyService = new PollyService(mockLogger.Object);

            var action = new Func<Task>(async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    await pollyService.GetWithPolicy<string>(PolicyType.CircuitBreaker,
                        () => throw new Exception("BOEM"),
                        null
                    );
                }
            });

            action.Should().Throw<Exception>().WithMessage("BOEM");
            mockLogger.VerifyAll();
                //(x => x.Write(It.Is<string>(y => y.Equals("Breaking circuit"))), Times.Exactly(1));
        }
    }
}