using Refit;
using RefitExample.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefitExample.Interfaces
{
    public interface IRemoteApiAuthorized : IRemoteApi
    {
        [Post("/auth/login")]
        new Task<LoginResult> DoLoginAsync([Body]Login credentials);

        [Get("/posts?_delay={delay}")]
        [Headers("Authorization: Bearer")]
        new Task<IEnumerable<Post>> GetAllPostsAsync(int delay);

        [Get("/posts/{id}?_delay={delay}")]
        [Headers("Authorization: Bearer")]
        new Task<Post> GetPostByIdAsync(int id, int delay);

        [Get("/posts?_delay={delay}&userid={userId}")]
        [Headers("Authorization: Bearer")]
        new Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId, int delay);

        [Post("/posts?_delay={delay}")]
        [Headers("Authorization: Bearer")]
        new Task<Post> AddPostAsync([Body]Post post, int delay);

        [Put("/posts/{id}?_delay={delay}")]
        [Headers("Authorization: Bearer")]
        new Task<Post> UpdateCompletePostAsync(int id, [Body]Post post, int delay);

        [Delete("/posts/{id}?_delay={delay}")]
        [Headers("Authorization: Bearer")]
        new Task DeletePostByIdAsync(int id, int delay);
    }
}