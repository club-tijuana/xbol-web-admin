namespace Odasoft.XBOL.AdminPortal.Services.Contracts
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string email, string password);
        Task LogoutAsync();
    }
}
