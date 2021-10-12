using System;
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
            var tenant = _blogContext.Blog.BlogId;

            var properties = new AuthenticationProperties() {RedirectUri = "/"};

            if (!String.IsNullOrEmpty(tenant))
            {
                properties.Items.Add("tenant", tenant);
            }

            await HttpContext.ChallengeAsync("Auth0", properties);
        }
        
        [Authorize]
        [Route("Logout")]
        public async Task Logout()
        {
            var tenant = _blogContext.Blog.BlogId;
            var options = _tenantOptions[tenant].Auth0;

            var properties = new AuthenticationProperties() {RedirectUri = options.LogoutRedirectUrl};

            if (!String.IsNullOrEmpty(tenant))
            {
                properties.Items.Add("tenant", tenant);
            }

            await HttpContext.SignOutAsync("Auth0", properties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}