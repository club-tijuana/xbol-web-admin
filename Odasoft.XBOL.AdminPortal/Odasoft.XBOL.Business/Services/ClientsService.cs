using Odasoft.XBOL.Models.Parameters;

namespace Odasoft.XBOL.Business.Services
{
    public class ClientsService(IAdminClient _adminClient)
    {
        public async Task<ClientResultPagedResponse> GetClientsAsync(ClientFilterParameters parameters)
        {
            var clients = await _adminClient.GetClientsAsync(
                parameters.CreditStatus,
                parameters.HasCredit,
                parameters.SearchTerm,
                parameters.SortBy,
                parameters.Descending,
                parameters.Page,
                parameters.PageSize);

            return clients;
        }

        public async Task<ClientDetailResult> GetClientDetailsAsync(long clientId)
        {
            var clientDetails = await _adminClient.GetClientDetailByIdAsync(clientId);
            return clientDetails;
        }

        public async Task<ClientResult> CreateClientAsync(CreateClientRequest clientForm)
        {
            var createdClient = await _adminClient.CreateClientAsync(clientForm);
            return createdClient;
        }

        public async Task UpdateClientAsync(long clientId, UpdateClientRequest clientForm)
        {
            await _adminClient.UpdateClientAsync(clientId, clientForm);
        }

        public async Task DeleteClientAsync(long clientId)
        {
            await _adminClient.DeleteClientAsync(clientId);
        }

        public async Task<List<EnumItemDto>> GetCreditStatusOptionsAsync()
        {
            var result = await _adminClient.GetCreditStatusListAsync();
            return result.ToList();
        }

        public async Task<List<EnumItemDto>> GetClientTypeOptionsAsync()
        {
            var result = await _adminClient.GetClientTypeListAsync();
            return result.ToList();
        }

        public async Task<List<EnumItemDto>> GetPaymentFrequencyOptionsAsync()
        {
            var result = await _adminClient.GetPaymentFrequencyListAsync();
            return result.ToList();
        }

        public async Task<FileResponse?> DownloadClientsCsvAsync(List<long> clientIds)
        {
            try
            {
                return await _adminClient.DownloadClientsCsvAsync(clientIds);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading clients: {ex.Message}");
                return null;
            }
        }

        public async Task<ClientContactResponse?> SearchClientAsync(string phone, string email)
        {
            if (string.IsNullOrWhiteSpace(phone) && string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            var contactInfo = await _adminClient.SearchClientAsync(phone, email);

            return contactInfo;
        }
    }

    // TODO: Move this to a separate file
    public record ClientFilterParameters : BaseFilterParameters
    {
        public bool? HasCredit { get; set; }
        public List<CreditStatus> CreditStatus { get; set; } = [];
    }
}
