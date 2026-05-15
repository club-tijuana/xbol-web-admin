namespace Odasoft.XBOL.AdminPortal.Services.Contracts
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(string email, string password);
        Task<AuthResult> RegisterAsync(string email, string password, string token, string? displayName);
        Task ForgotPasswordAsync(string email);
        Task<string> VerifyPasswordResetAsync(string oobCode);
        Task<AuthResult> ResetPasswordAsync(string email, string oobCode, string newPassword);
        Task LogoutAsync();
    }

    public static class AuthErrorCodes
    {
        public const string MissingCredentials = nameof(MissingCredentials);
        public const string MissingRegistrationFields = nameof(MissingRegistrationFields);
        public const string InvalidCredentials = nameof(InvalidCredentials);
        public const string LoginFailed = nameof(LoginFailed);
        public const string RegistrationFailed = nameof(RegistrationFailed);
        public const string PasswordResetFailed = nameof(PasswordResetFailed);
    }

    public sealed record AuthResult(
        bool Succeeded,
        string? ErrorCode = null,
        string? IdToken = null,
        string? ErrorMessage = null)
    {
        public static AuthResult Success(string idToken) => new(true, IdToken: idToken);
        public static AuthResult Failed(string errorCode, string? errorMessage = null) =>
            new(false, ErrorCode: errorCode, ErrorMessage: errorMessage);
    }
}
