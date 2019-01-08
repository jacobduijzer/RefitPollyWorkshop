using Newtonsoft.Json;

namespace RefitExample.Models
{
    public class Login
    {
        [JsonProperty("email")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
