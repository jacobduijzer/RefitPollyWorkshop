using Newtonsoft.Json;

namespace RefitExample.Models
{
    public class LoginResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
