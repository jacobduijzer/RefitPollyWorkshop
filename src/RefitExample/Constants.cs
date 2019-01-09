using System;

namespace RefitExample
{
    public static class Constants
    {
        public const string API_BASE_URL = "http://localhost:3000";
        public const string API_POSTS_PREFIX = "/posts";

        public static readonly TimeSpan TIMEOUT = TimeSpan.FromSeconds(2);
    }
}