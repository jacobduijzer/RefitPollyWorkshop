using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using RefitExample.Models;

namespace RefitExample.Interfaces
{
    public interface IRemoteApiAuthorized
    {
        [Post("/auth/login")]        
        Task<LoginResult> DoLoginAsync([Body]Login credentials);

        [Get("/posts?_delay={delay}")]
        [Headers("Authorization: Bearer")]
        Task<IEnumerable<Post>> GetAllPostsAsync(int delay);

        [Get("/posts/{id}?_delay={delay}")]
        [Headers("Authorization: Bearer")]
        Task<Post> GetPostByIdAsync(int id, int delay);

        [Get("/posts?_delay={delay}&userid={userId}")]
        [Headers("Authorization: Bearer")]
        Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId, int delay);

        [Post("/posts?_delay={delay}")]
        [Headers("Authorization: Bearer")]
        Task<Post> AddPostAsync([Body]Post post, int delay);

        [Put("/posts/{id}?_delay={delay}")]
        [Headers("Authorization: Bearer")]
        Task<Post> UpdateCompletePostAsync(int id, [Body]Post post, int delay);

        [Delete("/posts/{id}?_delay={delay}")]
        [Headers("Authorization: Bearer")]
        Task DeletePostByIdAsync(int id, int delay);
    }
}
