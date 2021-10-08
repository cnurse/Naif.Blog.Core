using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Naif.Blog.Framework;

namespace Naif.Blog.Authentication
{
    public static class Auth0Extensions
    {
	    private static void CheckSameSite(CookieOptions options)
	    {
		    if (options.SameSite == SameSiteMode.None && options.Secure == false)
		    {
			    options.SameSite = SameSiteMode.Unspecified;
		    }
	    }

	    private static void SetAuth0Options(OpenIdConnectOptions options, TenantOptions tenantOptions, string tenant)
	    {
		    var auth0Options = tenantOptions.Auth0[tenant];
		    
		    // Set the authority to your Auth0 domain
		    options.Authority = $"https://{auth0Options.Domain}";
		
		    // Configure the Auth0 Client ID and Client Secret
		    options.ClientId = auth0Options.ClientId;
		    options.ClientSecret = auth0Options.ClientSecret;

		    // Set the callback path, so Auth0 will call back to http://[MyDomain]/[CallbackPath] 
		    // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard 
		    options.CallbackPath = new PathString(auth0Options.CallbackPath);

		    // Set the correct name claim type
		    var nameClaimType = auth0Options.NameClaimType;
		    if (String.IsNullOrEmpty(nameClaimType))
		    {
			    nameClaimType = "name";
		    }
				
		    var roleClaimType = auth0Options.RoleClaimType;
		    if (String.IsNullOrEmpty(roleClaimType))
		    {
			    roleClaimType = "https://schemas.naifblog.com/roles";
		    }
				
		    options.TokenValidationParameters = new TokenValidationParameters
		    {
			    NameClaimType = nameClaimType,
			    RoleClaimType = roleClaimType
		    };
	    }

        public static void AddAuth0(this IServiceCollection services, TenantOptions tenantOptions)
        {
	        services.Configure<CookiePolicyOptions>(options =>
	        {
		        options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
		        options.OnAppendCookie = cookieContext => CheckSameSite(cookieContext.CookieOptions);
		        options.OnDeleteCookie = cookieContext => CheckSameSite(cookieContext.CookieOptions);
	        });

            // Add authentication services
			services.AddAuthentication(options => {
				options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			})
			.AddCookie()
			.AddOpenIdConnect("Auth0", options =>
			{
				SetAuth0Options(options, tenantOptions, "default");
		
				// Set response type to code
                options.ResponseType = OpenIdConnectResponseType.Code;
		
				// Configure the scope
				options.Scope.Clear();
				options.Scope.Add("openid");
				options.Scope.Add("profile");
				options.Scope.Add("email");
					
				// Configure the Claims Issuer to be Auth0
				options.ClaimsIssuer = "Auth0";

				options.Events = new OpenIdConnectEvents
				{
					OnRedirectToIdentityProvider = (context) =>
					{
						//Tenant may have independent Auth 0 settings
						if (context.Properties.Items.ContainsKey("tenant"))
						{
							var tenant = context.Properties.Items["tenant"];
							SetAuth0Options(options, tenantOptions, tenant);
						}

						return Task.FromResult(0);
					},
						
					// handle the logout redirection 
					OnRedirectToIdentityProviderForSignOut = (context) =>
					{
						//Tenant may have independent Auth 0 settings
						if (context.Properties.Items.ContainsKey("tenant"))
						{
							var tenant = context.Properties.Items["tenant"];
							SetAuth0Options(options, tenantOptions, tenant);
						}
						
						var logoutUri = $"{options.Authority}/v2/logout?client_id={options.ClientId}";
		
						var postLogoutUri = context.Properties.RedirectUri;
						if (!string.IsNullOrEmpty(postLogoutUri))
						{
							if (postLogoutUri.StartsWith("/"))
							{
								// transform to absolute
								var request = context.Request;
								postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
							}
							logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
						}
		
						context.Response.Redirect(logoutUri);
						context.HandleResponse();
		
						return Task.CompletedTask;
					}
				};   
			});
        }
    }
}