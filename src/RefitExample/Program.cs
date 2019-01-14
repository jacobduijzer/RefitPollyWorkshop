using RefitExample.Interfaces;
using RefitExample.Models;
using RefitExample.Services;
using System;
using System.Threading.Tasks;

namespace RefitExample
{
    internal class Program
    {
        public static string SEPARATOR = $"{Environment.NewLine}=========================";

        private static bool USE_AUTHENTICATION = false;
        private static LogLevel LOG_LEVEL = LogLevel.None;

        private static async Task Main(string[] args)
        {
            IRemoteApiService remoteApiService;
            if (!USE_AUTHENTICATION)
                remoteApiService = new RemoteApiService(LOG_LEVEL,
                                                        TimeSpan.FromSeconds(30));
            else
                remoteApiService = new RemoteApiService(LOG_LEVEL,
                                                        TimeSpan.FromSeconds(2),
                                                        new Login
                                                        {
                                                            UserName = "jduijzer@vaa.com",
                                                            Password = "test123!"
                                                        });

            //await new Programs.WithPolly(remoteApiService)
            //.GetAllPostsWithCircuitBreaker();

            await new Programs.Normal(remoteApiService).GetAllPosts();

            Console.WriteLine(SEPARATOR);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}