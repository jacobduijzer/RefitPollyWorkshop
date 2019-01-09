using Refit;
using RefitExample.Factories;
using RefitExample.Interfaces;
using RefitExample.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefitExample.Services
{
    public class RemoteApiService : IRemoteApiService
    {
        private readonly IRemoteApiAuthorized _remoteApi;

        public RemoteApiService(LogLevel logLevel) =>
            _remoteApi = RestService.For<IRemoteApiAuthorized>(HttpClientFactory.GetHttpClient(logLevel));

        public RemoteApiService(LogLevel logLevel, Login credentials) =>
            _remoteApi = RestService.For<IRemoteApiAuthorized>(
                HttpClientFactory.GetHttpClient(logLevel, () => DoLoginAsync(credentials))
            );

        public async Task<LoginResult> DoLoginAsync(Login credentials) =>
            await _remoteApi.DoLoginAsync(credentials).ConfigureAwait(false);

        public async Task<IEnumerable<Post>> GetAllPostsAsync(int delay = 0) =>
            await _remoteApi.GetAllPostsAsync(delay).ConfigureAwait(false);

        public async Task<Post> GetPostByIdAsync(int id, int delay = 0) =>
            await _remoteApi.GetPostByIdAsync(id, delay).ConfigureAwait(false);

        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId, int delay = 0) =>
            await _remoteApi.GetPostsByUserIdAsync(userId, delay).ConfigureAwait(false);

        public async Task<Post> AddPostAsync(Post post, int delay = 0) =>
            await _remoteApi.AddPostAsync(post, delay).ConfigureAwait(false);

        public async Task<Post> UpdateCompletePostAsync(Post post, int delay = 0) =>
            await _remoteApi.UpdateCompletePostAsync(post.Id, post, delay).ConfigureAwait(false);

        public async Task DeletePostByIdAsync(int id, int delay = 0) =>
            await _remoteApi.DeletePostByIdAsync(id, delay).ConfigureAwait(false);
    }
}