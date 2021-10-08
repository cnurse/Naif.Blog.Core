using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Naif.Blog.Framework;

namespace Naif.Auth0.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private IBlogContext _blogContext;
        private Dictionary<string,TenantOptions> _tenantOptions;
        
        public AccountController(IBlogContext blogContext, Dictionary<string,TenantOptions> tenantOptions)
        {
            _blogContext = blogContext;
            _tenantOptions = tenantOptions;
        }
        
        [Route("Login")]
        public async Task Login(string returnUrl = "/")
        {
            await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties()
            {
                RedirectUri = returnUrl
            });
        }
        
        [Authorize]
        [Route("Logout")]
        public async Task Logout()
        {
            var tenant = _blogContext.Blog.BlogId;
            var options = _tenantOptions[tenant].Auth0;
            
            await HttpContext.SignOutAsync("Auth0", new AuthenticationProperties
            {
                // Indicate here where Auth0 should redirect the user after a logout.
                // Note that the resulting absolute Uri must be whitelisted in the 
                // **Allowed Logout URLs** settings for the app.
                RedirectUri = options.LogoutRedirectUrl
            });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}