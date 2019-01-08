using System;
using System.Linq;
using System.Threading.Tasks;
using RefitExample.Interfaces;
using RefitExample.Models;

namespace RefitExample.Programs
{
    public static class Normal
    {
        private const int DELAY = 1; // 0 gives issues with json-server

        public static async Task Run(IRemoteApiService remoteApiService)
        {
            try
            {
                Console.WriteLine(Program.SEPARATOR);
                Console.WriteLine($"Getting all posts");
                var allPosts = await remoteApiService.GetAllPostsAsync(DELAY).ConfigureAwait(false);
                Console.WriteLine($"AllPosts result count: {allPosts.Count()}");

                Console.WriteLine(Program.SEPARATOR);
                Console.WriteLine($"Getting single post");
                var singlePost = await remoteApiService.GetPostByIdAsync(DELAY).ConfigureAwait(false);
                Console.WriteLine($"Single post title: {singlePost.Title}");

                Console.WriteLine(Program.SEPARATOR);
                Console.WriteLine($"Adding post");
                var newPost = await remoteApiService.AddPostAsync(new Post(1, "Test", "This is a test"), DELAY)
                    .ConfigureAwait(false);
                Console.WriteLine($"New post ID: {newPost.Id}");

                Console.WriteLine(Program.SEPARATOR);
                Console.WriteLine($"Updating post {singlePost.Id}, old title: {singlePost.Title}");
                var updatedPost = await remoteApiService.UpdateCompletePostAsync(singlePost.UpdateTitle($"Test {DateTime.Now}"), DELAY)
                    .ConfigureAwait(false);
                Console.WriteLine($"Updated post: {updatedPost.Id}, new title: {updatedPost.Title}");
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
