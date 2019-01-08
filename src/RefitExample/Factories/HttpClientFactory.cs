using System;
using System.Net.Http;
using System.Threading.Tasks;
using RefitExample.Handlers;
using RefitExample.Loggers;
using RefitExample.Models;

namespace RefitExample.Factories
{
    public static class HttpClientFactory
    {
        public static HttpClient GetHttpClient(LogLevel logLevel, Func<Task<LoginResult>> getToken = null)
        {
            HttpClient httpClient;

            switch (logLevel)
            {
                case LogLevel.Debug:
                case LogLevel.Information:
                    if(getToken != null)
                        httpClient = new HttpClient(new LoggingHandler(new AuthenticatedHttpClientHandler(getToken), new ConsoleLogger(), logLevel));
                    else
                        httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler(), new ConsoleLogger(), logLevel));
                    break;

                default:
                    if (getToken != null)
                        httpClient = new HttpClient(new AuthenticatedHttpClientHandler(getToken));
                    else
                        httpClient = new HttpClient(new HttpClientHandler());
                    break;
            }

            httpClient.BaseAddress = new Uri(Constants.API_BASE_URL);
            httpClient.Timeout = Constants.TIMEOUT;

            return httpClient;
        }
    }
}
