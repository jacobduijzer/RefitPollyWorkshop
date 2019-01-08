using System.Collections.Generic;
using System.Threading.Tasks;
using RefitExample.Models;

namespace RefitExample.Interfaces
{
    public interface IRemoteApiService
    {
        Task<LoginResult> DoLoginAsync(Login credentials);

        Task<IEnumerable<Post>> GetAllPostsAsync(int delay = 0);

        Task<Post> GetPostByIdAsync(int id, int delay = 0);

        Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId, int delay = 0);

        Task<Post> AddPostAsync(Post post, int delay = 0);

        Task<Post> UpdateCompletePostAsync(Post post, int delay = 0);

        Task DeletePostByIdAsync(int id, int delay = 0);
    }
}
