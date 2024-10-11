
namespace BlazorApp3.Helpers
{
    public class UserToken
    {
        public string AccessToken { get; internal set; }
        public string AccessTokenType { get; internal set; }
        public DateTimeOffset Expiration { get; internal set; }
        public string Scope { get; internal set; }
        public string Error { get; internal set; }
    }
}