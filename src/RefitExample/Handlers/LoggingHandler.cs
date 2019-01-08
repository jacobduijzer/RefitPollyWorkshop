using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using RefitExample.Interfaces;
using RefitExample.Loggers;
using RefitExample.Models;

namespace RefitExample.Handlers
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger _logger;
        private readonly LogLevel _logLevel;

        public LoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            if (_logger == null)
                _logger = new ConsoleLogger();
        }

        public LoggingHandler(HttpMessageHandler innerHandler, ILogger logger, LogLevel logLevel)
            : base(innerHandler)
        {
            _logger = logger;
            _logLevel = logLevel;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.Write("Request:");
            _logger.Write(request.ToString());
            if (_logLevel == LogLevel.Debug && request.Content != null)
            {
                _logger.Write(await request.Content.ReadAsStringAsync());
            }
            _logger.Write(Environment.NewLine);

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            _logger.Write("Response:");
            _logger.Write(response.ToString());
            if (_logLevel == LogLevel.Debug && response.Content != null)
            {
                _logger.Write(await response.Content.ReadAsStringAsync());
            }
            _logger.Write(Environment.NewLine);

            return response;
        }
    }
}
