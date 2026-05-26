using Microsoft.JSInterop;
using Odasoft.XBOL.AdminPortal.Services.Contracts;
using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.Services;

public class AuthService(
    FirebaseAuthJsInterop firebaseAuth,
    IAdminClient adminClient,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return AuthResult.Failed(AuthErrorCodes.MissingCredentials);
        }

        try
        {
            var user = await firebaseAuth.SignInAsync(email, password);
            return AuthResult.Success(user.IdToken);
        }
        catch (JSException ex)
        {
            await SignOutBestEffortAsync();
            logger.LogWarning(ex, "Firebase sign-in failed for {Email}", email);
            return AuthResult.Failed(AuthErrorCodes.InvalidCredentials);
        }
        catch (Exception ex)
        {
            await SignOutBestEffortAsync();
            logger.LogError(ex, "Unexpected login failure for {Email}", email);
            return AuthResult.Failed(AuthErrorCodes.LoginFailed);
        }
    }

    public async Task<AuthResult> RegisterAsync(
        string email,
        string password,
        string token,
        string? displayName)
    {
        if (string.IsNullOrWhiteSpace(email)
            || string.IsNullOrWhiteSpace(password)
            || string.IsNullOrWhiteSpace(token))
        {
            return AuthResult.Failed(AuthErrorCodes.MissingRegistrationFields);
        }

        try
        {
            var normalizedEmail = email.Trim();

            await adminClient.RegisterAsync(new RegisterRequest
            {
                Email = normalizedEmail,
                Password = password,
                Token = token,
                DisplayName = string.IsNullOrWhiteSpace(displayName) ? null : displayName.Trim()
            });

            var user = await firebaseAuth.SignInAsync(normalizedEmail, password);
            return AuthResult.Success(user.IdToken);
        }
        catch (ApiException<ProblemDetails> ex)
        {
            await SignOutBestEffortAsync();
            logger.LogWarning(
                ex,
                "Admin invitation registration failed for {Email} with status {StatusCode}",
                email,
                ex.StatusCode);
            throw;
        }
        catch (JSException ex)
        {
            await SignOutBestEffortAsync();
            logger.LogWarning(ex, "Firebase sign-in after registration failed for {Email}", email);
            return AuthResult.Failed(AuthErrorCodes.LoginFailed);
        }
        catch (Exception ex)
        {
            await SignOutBestEffortAsync();
            logger.LogError(ex, "Unexpected registration failure for {Email}", email);
            return AuthResult.Failed(AuthErrorCodes.RegistrationFailed);
        }
    }

    public async Task ForgotPasswordAsync(string email)
    {
        await adminClient.ForgotPasswordAsync(new ForgotPasswordRequest
        {
            Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim()
        });
    }

    public async Task<string> VerifyPasswordResetAsync(string oobCode)
    {
        return await firebaseAuth.VerifyPasswordResetAsync(oobCode);
    }

    public async Task<AuthResult> ResetPasswordAsync(string email, string oobCode, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(email)
            || string.IsNullOrWhiteSpace(oobCode)
            || string.IsNullOrWhiteSpace(newPassword))
        {
            return AuthResult.Failed(AuthErrorCodes.PasswordResetFailed);
        }

        try
        {
            await firebaseAuth.ConfirmPasswordResetAsync(oobCode, newPassword);
            var user = await firebaseAuth.SignInAsync(email, newPassword);
            return AuthResult.Success(user.IdToken);
        }
        catch (JSException ex)
        {
            await SignOutBestEffortAsync();
            logger.LogWarning(ex, "Firebase password reset failed.");
            return AuthResult.Failed(AuthErrorCodes.PasswordResetFailed);
        }
        catch (Exception ex)
        {
            await SignOutBestEffortAsync();
            logger.LogError(ex, "Unexpected password reset failure.");
            return AuthResult.Failed(AuthErrorCodes.PasswordResetFailed);
        }
    }

    public async Task LogoutAsync()
    {
        await SignOutBestEffortAsync();
    }

    private async Task SignOutBestEffortAsync()
    {
        try
        {
            await firebaseAuth.SignOutAsync();
        }
        catch (JSException ex)
        {
            logger.LogWarning(ex, "Firebase sign-out cleanup failed.");
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Unexpected Firebase sign-out cleanup failure.");
        }
    }
}
