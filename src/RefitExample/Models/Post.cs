using Newtonsoft.Json;

namespace RefitExample.Models
{
    public class Post
    {
        [JsonProperty("id")]
        public int Id
        {
            get;
            private set;
        }

        [JsonProperty("userId")]
        public int UserId
        {
            get;
            private set;
        }

        [JsonProperty("title")]
        public string Title
        {
            get;
            private set;
        }

        [JsonProperty("body")]
        public string Body
        {
            get;
            private set;
        }

        public Post(int userId, string title, string body)
        {
            UserId = userId;
            Title = title;
            Body = body;
        }

        [JsonConstructor]
        public Post(int id, int userId, string title, string body) : this(userId, title, body) => Id = id;

        public Post UpdateTitle(string title) => 
            new Post(Id, UserId, title, Body);
    }
}
