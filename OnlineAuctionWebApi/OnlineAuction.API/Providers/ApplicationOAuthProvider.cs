using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using OnlineAuction.BLL.DTO;
using OnlineAuction.BLL.Exceptions;
using OnlineAuction.BLL.Interfaces;

namespace OnlineAuction.API.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            _publicClientId = publicClientId ?? throw new ArgumentNullException(nameof(publicClientId));
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (var scope = System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver.BeginScope())
            {
                if (string.IsNullOrEmpty(context.UserName) || string.IsNullOrEmpty(context.Password))
                {
                    context.SetError("invalid_grant", "UserName and Password is required.");
                    return;
                }
                var usersService = scope.GetService(typeof(IUsersService)) as IUsersService;
                UserDTO user;
                try
                {
                    user = await usersService.GetUserByNameAsync(context.UserName);
                }
                catch (NotFoundException e)
                {
                    context.SetError("invalid_grant", e.Message);
                    return;
                }
                ClaimsIdentity oAuthIdentity = await usersService.AuthenticateUserAsync(context.UserName, context.Password);
                AuthenticationProperties properties = CreateProperties(user);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(UserDTO user)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", user.Name },
                { "role",  user.Role },
                { "id", user.UserProfileId.ToString() },
                { "email", user.Email }
            };
            return new AuthenticationProperties(data);
        }
    }
}