namespace Naif.Blog.Authentication
{
    public class Auth0Options
    {
        public string Domain { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CallbackPath { get; set; }
        
        public string LogoutRedirectUrl { get; set; }
        public string NameClaimType { get; set; }
        public string RoleClaimType { get; set; }
    }
}