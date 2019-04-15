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
    /// <summary>
    /// Provider of OAuth user tokens.
    /// </summary>
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            _publicClientId = publicClientId ?? throw new ArgumentNullException(nameof(publicClientId));
        }

        /// <summary>
        /// Method for generating token for user.
        /// </summary>
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

        /// <summary>
        /// Called at the final stage of a successful Token endpoint request. 
        /// </summary>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Called to validate that the origin of the request is a registered "client_id", and that the correct credentials for that client are present on the request.
        /// </summary>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Called to validate that the context.ClientId is a registered "client_id", and that the context.RedirectUri a "redirect_uri" registered for that client.
        /// </summary>
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

        /// <summary>
        /// Adds additional information about user to response with token.
        /// </summary>
        /// <param name="user">The user DTO.</param>
        /// <returns>Authentication properties.</returns>
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