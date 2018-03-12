using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using IdentityServer3.AccessTokenValidation;

[assembly: OwinStartup(typeof(eCommerce.WebApi.Startup))]

namespace eCommerce.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://sso.test.vggdev.com/identity/",
                ClientId = "ebteller",
                ClientSecret = "adetoberu",
                RequiredScopes = new[] { "openid", "profile", "identity-server-api", "roles", "ebteller" }
            });
            ConfigureAuth(app);
        }
    }
}
