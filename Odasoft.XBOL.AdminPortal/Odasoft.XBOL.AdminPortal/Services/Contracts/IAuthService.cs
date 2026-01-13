namespace Odasoft.XBOL.AdminPortal.Services.Contracts
{
    public interface IAuthService
    {
        public Task<bool> LoginAsync(string email, string password);
        public Task LogoutAsync();
    }
}
