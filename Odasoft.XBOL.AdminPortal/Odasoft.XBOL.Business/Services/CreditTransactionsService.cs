using Odasoft.XBOL.Models.Parameters;

namespace Odasoft.XBOL.Business.Services
{
    public class CreditTransactionsService
    {
        private readonly IAdminClient _adminClient;

        public CreditTransactionsService(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task<CreditTransactionResultPagedResponse> GetCreditTransactionsAsync(CreditTransactionsFilterParameters parameters)
        {
            var response = await _adminClient.GetCreditTransactionsAsync(
                parameters.CreditAccountId,
                parameters.PaymentTypes,
                parameters.StartDate,
                parameters.EndDate,
                parameters.SearchTerm,
                parameters.SortBy,
                parameters.Descending,
                parameters.Page,
                parameters.PageSize);

            return response;
        }

        public async Task<bool> CreateCreditTransactionByCreditAccountIdAsync(long creditAccountId, ClientCreditTransactionRequest request)
        {
            var response = await _adminClient.CreateCreditTransactionByCreditAccountIdAsync(creditAccountId, request);
            return response;
        }

        public async Task<bool> UpdateCreditTransactionByIdAsync(long creditTransactionId, ClientCreditTransactionRequest request)
        {
            var response = await _adminClient.UpdateCreditTransactionByIdAsync(creditTransactionId, request);
            return response;
        }

        public async Task<bool> DeleteCreditTransactionByIdAsync(long creditTransactionId)
        {
            var response = await _adminClient.DeleteCreditTransactionByIdAsync(creditTransactionId);
            return response;
        }
    }

    public record CreditTransactionsFilterParameters : BaseFilterParameters
    {
        public long? CreditAccountId { get; set; }
        public List<PaymentType> PaymentTypes { get; set; } = [];
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}
