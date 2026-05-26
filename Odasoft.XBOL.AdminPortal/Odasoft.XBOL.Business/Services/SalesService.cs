namespace Odasoft.XBOL.Business.Services
{
    public class SalesService
    {
        private readonly IAdminClient _adminClient;

        public SalesService(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task<SaleResponsePagedResponse> GetSalesAsync(
            AdminSaleType adminSaleType,
            long referenceId,
            DateTimeOffset? startDate,
            DateTimeOffset? endDate,
            IEnumerable<PaymentType>? paymentTypes,
            IEnumerable<SaleChannel>? saleChannel,
            IEnumerable<OrderStatus>? orderStatus,
            string? searchTerm,
            string? sortBy,
            bool? descending,
            int page,
            int pageSize)
        {
            return await _adminClient.GetSalesAsync(
                adminSaleType,
                referenceId,
                startDate,
                endDate,
                paymentTypes,
                saleChannel,
                orderStatus,
                searchTerm,
                sortBy,
                descending,
                page,
                pageSize);
        }
    }
}
