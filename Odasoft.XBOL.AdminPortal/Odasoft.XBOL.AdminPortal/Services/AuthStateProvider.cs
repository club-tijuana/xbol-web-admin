using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Odasoft.XBOL.AdminPortal.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());
        private ClaimsPrincipal _currentUser = Anonymous;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_currentUser));
        }
        public Task SignInAsync(string email)
        {
            List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Email, email)
        };

            ClaimsIdentity identity =
                new(claims, authenticationType: "Whitelist");

            _currentUser = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(_currentUser)));

            return Task.CompletedTask;
        }

        public Task SignOutAsync()
        {
            _currentUser = Anonymous;

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(_currentUser)));

            return Task.CompletedTask;
        }
    }
}
