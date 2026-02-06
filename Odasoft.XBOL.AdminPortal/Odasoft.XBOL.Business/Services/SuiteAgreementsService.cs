namespace Odasoft.XBOL.Business.Services
{
    public class SuiteAgreementsService
    {
        private IAdminClient _adminClient;

        public SuiteAgreementsService(IAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public async Task<ICollection<SuiteAgreementResult>> GetSuiteAgreementsAsync()
        {
            var agreements = await _adminClient.GetSuiteAgreementsAsync();
            return agreements;
        }

        public async Task<SuiteAgreementResult> GetSuiteAgreementByIdAsync(long agreementId)
        {
            return await _adminClient.GetSuiteAgreementByIdAsync(agreementId);
        }

        public async Task<bool> CreateSuiteAgreementAsync(
            long suiteId,
            string ownerName,
            string ownerEmail,
            string ownerPhone,
            DateTimeOffset? startDate,
            DateTimeOffset? endDate,
            FileParameter agreementFile)
        {
            try
            {
                await _adminClient.CreateSuiteAgreementAsync(suiteId, ownerName, ownerEmail, ownerPhone, startDate, endDate, agreementFile);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating suite agreement: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateSuiteAgreementAsync(UpdateSuiteAgreementRequest request)
        {
            try
            {
                await _adminClient.UpdateSuiteAgreementAsync(request);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating suite agreement: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteSuiteAgreementAsync(long agreementId)
        {
            try
            {
                await _adminClient.DeleteSuiteAgreementAsync(agreementId);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting suite agreement: {ex.Message}");
                return false;
            }
        }

        public async Task<FileResponse> DownloadSuiteAgreementFileAsync(long suiteAgreementId)
        {
            try
            {
                return await _adminClient.DownloadSuiteAgreementFileAsync(suiteAgreementId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading suite agreement file: {ex.Message}");
                return null;
            }
        }

        public async Task<FileResponse> DownloadSuiteAgreementFilesAsync(List<long> suiteAgreementIds)
        {
            try
            {
                return await _adminClient.DownloadMultipleSuiteAgreementsAsync(suiteAgreementIds);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading suite agreements: {ex.Message}");
                return null;
            }
        }
    }
}
