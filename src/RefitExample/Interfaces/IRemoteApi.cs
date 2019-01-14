using Refit;
using RefitExample.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefitExample.Interfaces
{
    public interface IRemoteApi
    {
        [Post("/auth/login")]
        Task<LoginResult> DoLoginAsync([Body]Login credentials);

        [Get("/posts?_delay={delay}")]
        Task<IEnumerable<Post>> GetAllPostsAsync(int delay);

        [Get("/posts/{id}?_delay={delay}")]
        Task<Post> GetPostByIdAsync(int id, int delay);

        [Get("/posts?_delay={delay}&userid={userId}")]
        Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId, int delay);

        [Post("/posts?_delay={delay}")]
        Task<Post> AddPostAsync([Body]Post post, int delay);

        [Put("/posts/{id}?_delay={delay}")]
        Task<Post> UpdateCompletePostAsync(int id, [Body]Post post, int delay);

        [Delete("/posts/{id}?_delay={delay}")]
        Task DeletePostByIdAsync(int id, int delay);
    }
}