using System;
using System.Linq;
using System.Threading.Tasks;
using RefitExample.Interfaces;
using RefitExample.Loggers;
using RefitExample.Models;

namespace RefitExample.Programs
{
    public class Normal
    {
        private const int DELAY = 1; // 0 gives issues with json-server

        private readonly ILogger _logger;
        private readonly IRemoteApiService _remoteApiService;

        public Normal(IRemoteApiService apiService)
        {
            _logger = new ConsoleLogger();
            _remoteApiService = apiService;
        }

        public async Task GetAllPosts()
        {
            _logger.Write(Program.SEPARATOR);
            _logger.Write($"Getting all posts");
            var allPosts = await _remoteApiService.GetAllPostsAsync(DELAY).ConfigureAwait(false);
            _logger.Write($"AllPosts result count: {allPosts.Count()}");
        }

        public async Task<Post> GetSinglePost()
        {
            _logger.Write(Program.SEPARATOR);
            _logger.Write($"Getting single post");
            var singlePost = await _remoteApiService.GetPostByIdAsync(DELAY).ConfigureAwait(false);
            _logger.Write($"Single post title: {singlePost.Title}");

            return singlePost;
        }

        public async Task AddPost()
        {
            _logger.Write(Program.SEPARATOR);
            _logger.Write($"Adding post");
            var newPost = await _remoteApiService.AddPostAsync(
                new Post(1, "Test", "This is a test"), 
                DELAY).ConfigureAwait(false);
            _logger.Write($"New post ID: {newPost.Id}");
        }

        public async Task UpdatePost()
        {
            var singlePost = await GetSinglePost();
            _logger.Write(Program.SEPARATOR);
            _logger.Write($"Updating post {singlePost.Id}, old title: {singlePost.Title}");
            var updatedPost = await _remoteApiService.UpdateCompletePostAsync(
                singlePost.UpdateTitle($"Test {DateTime.Now}"),
                DELAY).ConfigureAwait(false);
            _logger.Write($"Updated post: {updatedPost.Id}, new title: {updatedPost.Title}");
        }
    }
}
