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
    public static class WithPolly
    {
        private const int DELAY = 5; // 0 gives issues with json-server

        public static async Task Run(IRemoteApiService remoteApiService)
        {
            try
            {
                var pollyService = new PollyService(new ConsoleLogger());

                //try
                //{
                //    Console.WriteLine(Program.SEPARATOR);
                //    Console.WriteLine($"Getting all posts with retry");
                //    var allPostsWithRetry = await pollyService.GetWithPolicy<IEnumerable<Post>>(
                //        PolicyType.Retry,
                //        () => remoteApiService.GetAllPostsAsync(), null).ConfigureAwait(false);
                //    Console.WriteLine($"AllPosts result count: {allPostsWithRetry.Count()}");
                //}
                //catch (Exception)
                //{
                //    Console.WriteLine("Expected exception, timeout");
                //}

                //Console.WriteLine(Program.SEPARATOR);
                //Console.WriteLine($"Getting all posts with fallback");
                //var allPostsWithFallBack = await pollyService.GetWithPolicy<IEnumerable<Post>>(
                //    PolicyType.Fallback,
                //    () => remoteApiService.GetAllPostsAsync(),
                //    () => remoteApiService.GetAllPostsAsync(1)).ConfigureAwait(false);
                //Console.WriteLine($"AllPosts result count: {allPostsWithFallBack.Count()}");

                Console.WriteLine(Program.SEPARATOR);
                Console.WriteLine($"Getting all posts with retry & fallback");
                var allPostsWithRetryAndFallBack = await pollyService.GetWithPolicy<IEnumerable<Post>>(
                    PolicyType.RetryWithFallBack,
                    () => remoteApiService.GetAllPostsAsync(DELAY),
                    () => remoteApiService.GetAllPostsAsync(1)).ConfigureAwait(false);
                Console.WriteLine($"AllPosts result count: {allPostsWithRetryAndFallBack.Count()}");


            }
            catch (Exception ex)
            {
                Console.WriteLine(Program.SEPARATOR);
                Console.WriteLine($"Exception");
                Console.WriteLine($"{ex.Message}");
            }
        }
    }
}
