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
        private static bool USE_POLICIES = true;
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

            
            if(USE_POLICIES)
                await Programs.WithPolly.Run(remoteApiService);
            else
                await Programs.Normal.Run(remoteApiService);

            Console.WriteLine(SEPARATOR);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
