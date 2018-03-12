using System;

using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Linq;

using System.Web.Helpers;
using IdentityModel;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using eCommerce.WebUI.Utils;

using System.Security.Claims;
using Microsoft.Owin.Security;

namespace eCommerce.WebUI
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            // app.CreatePerOwinContext(ApplicationDbContext.Create);
            // app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            //app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
                //AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                //LoginPath = new PathString("/Account/Login"),
                //Provider = new CookieAuthenticationProvider
                //{
                //    // Enables the application to validate the security stamp when the user logs in.
                //    // This is a security feature which is used when you change a password or add an external login to your account.  
                //    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                //        validateInterval: TimeSpan.FromMinutes(30),
                //        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                //}
            });

            AntiForgeryConfig.UniqueClaimTypeIdentifier = JwtClaimTypes.Subject;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = AppConstants.IdSrv,

                //TokenValidationParameters = new TokenValidationParameters
                //{
                //    ValidateIssuer = false,
                //    RoleClaimType = "ebteller.role",
                //    NameClaimType = "name"
                //},

                ClientId = AppConstants.ClientId,
                //ClientSecret = "adetoberu",
                Scope = "openid profile identity-server-api",
                RedirectUri = AppConstants.WebClientUri,
                ResponseType = "code id_token token",
                UseTokenLifetime = true,
                SignInAsAuthenticationType = "Cookies",

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = async n =>
                    {
                        var nid = new ClaimsIdentity(n.AuthenticationTicket.Identity.AuthenticationType,
                                    JwtClaimTypes.GivenName, JwtClaimTypes.Role);

                        //var userInfo = await TokenHelper.CallUserInfoEndpoint(n.ProtocolMessage.AccessToken);
                        JwtSecurityToken securityToken = new JwtSecurityTokenHandler().ReadToken(n.ProtocolMessage.AccessToken) as JwtSecurityToken;
                        nid.AddClaim(new Claim(JwtClaimTypes.Subject, securityToken.Subject));

                        nid.AddClaim(securityToken.Claims.Where(c => c.Type == "preferred_username").FirstOrDefault());
                        nid.AddClaim(securityToken.Claims.Where(c => c.Type == "given_name").FirstOrDefault());
                        nid.AddClaim(securityToken.Claims.Where(c => c.Type == "family_name").FirstOrDefault());
                        nid.AddClaim(securityToken.Claims.Where(c => c.Type == "email").FirstOrDefault());
                        nid.AddClaim(securityToken.Claims.Where(c => c.Type == "phone_number").FirstOrDefault());
                        //nid.AddClaim(securityToken.Claims.Where(c => c.Type == "ebteller.role").FirstOrDefault());
                        var roles = securityToken.Claims.Where(c => c.Type == "ebteller.role").AsEnumerable();

                        foreach (Claim role in roles)
                        {
                            nid.AddClaim(new Claim(JwtClaimTypes.Role, role.Value));
                        }
                       
                        //securityToken.Claims.ToList().ForEach(ui => nid.AddClaim(ui));

                        // keep the id_token for logout
                        nid.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

                        // add access token for sample API
                        nid.AddClaim(new Claim("access_token", n.ProtocolMessage.AccessToken));

                        // keep track of access token expiration
                        nid.AddClaim(new Claim("expires_at", DateTimeOffset.Now.AddSeconds(int.Parse(n.ProtocolMessage.ExpiresIn)).ToString()));

                        n.AuthenticationTicket = new AuthenticationTicket(
                            nid,
                            n.AuthenticationTicket.Properties);
                    }
                    //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

                    //// Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
                    //app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

                    //// Enables the application to remember the second login verification factor such as phone or email.
                    //// Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
                    //// This is similar to the RememberMe option when you log in.
                    //app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

                    // Uncomment the following lines to enable logging in with third party login providers
                    //app.UseMicrosoftAccountAuthentication(
                    //    clientId: "",
                    //    clientSecret: "");

                    //app.UseTwitterAuthentication(
                    //   consumerKey: "",
                    //   consumerSecret: "");

                    //app.UseFacebookAuthentication(
                    //   appId: "",
                    //   appSecret: "");

                    //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
                    //{
                    //    ClientId = "",
                    //    ClientSecret = ""
                    //});
                }
            });

        } 
    }
}