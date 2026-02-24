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

        public async Task<long> CreateSuiteAgreementAsync(
            long suiteId,
            string ownerName,
            string ownerEmail,
            string ownerPhone,
            DateTimeOffset startDate,
            DateTimeOffset endDate)
        {
            try
            {
                var agreementId = await _adminClient.CreateSuiteAgreementAsync(new CreateSuiteAgreementRequest
                {
                    EndDate = endDate.ToUniversalTime(),
                    OwnerEmail = ownerEmail,
                    OwnerPhone = ownerPhone,
                    OwnerName = ownerName,
                    StartDate = startDate.ToUniversalTime(),
                    SuiteId = suiteId
                });

                return agreementId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating suite agreement: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> UploadSuiteAgreementFileAsync(long suiteAgreementId, FileParameter file)
        {
            try
            {
                var success = await _adminClient.UploadSuiteAgreementFileAsync(suiteAgreementId, file);

                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading suite agreementFile: {ex.Message}");
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

        public async Task<FileResponse?> DownloadSuiteAgreementFileAsync(long suiteAgreementId)
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

        public async Task<FileResponse?> DownloadSuiteAgreementFilesAsync(List<long> suiteAgreementIds)
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
