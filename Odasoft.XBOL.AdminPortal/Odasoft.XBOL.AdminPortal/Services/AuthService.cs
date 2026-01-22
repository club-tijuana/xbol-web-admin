using Microsoft.Extensions.Options;
using Odasoft.XBOL.AdminPortal.Configs;
using Odasoft.XBOL.AdminPortal.Services.Contracts;

namespace Odasoft.XBOL.AdminPortal.Services
{
    public class AuthService : IAuthService
    {
        private readonly Authentication _authenticationConfig;
        private readonly AuthStateProvider _authStateProvider;

        public AuthService(IOptions<Authentication> authenticationConfig, AuthStateProvider authStateProvider)
        {
            _authenticationConfig = authenticationConfig.Value;
            _authStateProvider = authStateProvider;
        }

        private bool ValidateCredentials(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            return _authenticationConfig.AllowedUsers.Any(user =>
            string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase)
            && user.Password == password);
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            bool isValid = ValidateCredentials(email, password);

            if (!isValid)
            {
                return false;
            }

            await _authStateProvider.SignInAsync(email);
            return true;
        }

        public async Task LogoutAsync()
        {
            await _authStateProvider.SignOutAsync();
        }
    }
}
