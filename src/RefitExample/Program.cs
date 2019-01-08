using System;
using System.Threading.Tasks;
using RefitExample.Interfaces;
using RefitExample.Models;
using RefitExample.Services;

namespace RefitExample
{
    class Program
    {
        public static string SEPARATOR = $"{Environment.NewLine}=========================";
        
        private static bool USE_AUTHENTICATION = true;
        private static LogLevel LOG_LEVEL = LogLevel.None;

        static async Task Main(string[] args)
        {
            IRemoteApiService remoteApiService;
            if(USE_AUTHENTICATION)
                remoteApiService = new RemoteApiService(LOG_LEVEL);
            else
                remoteApiService = new RemoteApiService(LOG_LEVEL, new Login
                {
                    UserName = "jduijzer@vaa.com",
                    Password = "test123!"
                });

            await new Programs.WithPolly(remoteApiService)
                .GetAllPostsWithCircuitBreakerWithRetryAndFallBack();

            //await new Programs.Normal(remoteApiService).UpdatePost();

            Console.WriteLine(SEPARATOR);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
