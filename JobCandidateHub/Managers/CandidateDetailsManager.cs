using JobCandidateHub.Models;
using JobCandidateHub.Services;

namespace JobCandidateHub.Managers
{
    public interface ICandidateDetailsManager
    {
        Task<CandidateDetails?> CreateCandidateDetailsAsync(CandidateDetails candidateDetailsModel);

        Task<CandidateDetails?> UpdateCandidateDetailsAsync(
            CandidateDetails existingCandidateDetailsModel,
            CandidateDetails updatedCandidateDetailsModel,
            List<CandidateDetails> allCandidatesDetails);
    }

    public class CandidateDetailsManager : ICandidateDetailsManager
    {
        private readonly ICsvFileService _csvFileService;

        public CandidateDetailsManager(ICsvFileService csvFileService)
        {
            _csvFileService = csvFileService;
        }

        public async Task<CandidateDetails?> CreateCandidateDetailsAsync(CandidateDetails candidateDetailsModel)
        {
            ValidateInputs(candidateDetailsModel.Email, candidateDetailsModel.LinkedInProfileUrl, candidateDetailsModel.GitHubProfileUrl);

            var candidatesDetails = await _csvFileService.GetAllCandidateDetails();

            var existingCandidateDetails = candidatesDetails.FirstOrDefault(s => s.Email == candidateDetailsModel.Email);

            if (existingCandidateDetails != null)
            {
                var updateCandidateDetails = await UpdateCandidateDetailsAsync(existingCandidateDetails, candidateDetailsModel, candidatesDetails);

                return updateCandidateDetails;
            }

            candidatesDetails.Add(candidateDetailsModel);

            await _csvFileService.SaveCandidateDetails(candidatesDetails);

            return candidateDetailsModel;
        }

        public async Task<CandidateDetails?> UpdateCandidateDetailsAsync(
            CandidateDetails existingCandidateDetailsModel,
            CandidateDetails updatedCandidateDetailsModel,
            List<CandidateDetails> allCandidatesDetails)
        {
            MapUpdatedModelToExistingModel(existingCandidateDetailsModel, updatedCandidateDetailsModel);

            await _csvFileService.SaveCandidateDetails(allCandidatesDetails);

            //Here, we return the "existingCandidateDetailsModel" after we update its fields with the updatedCandidateDetailsModel
            return existingCandidateDetailsModel;
        }

        private static void ValidateInputs(string email, string? linkedInProfileUrl, string? gitHubProfileUrl)
        {
            //This method has a simple conditions to validate the user inputs (email, and urls), this can be improved by creating a fluent validation service

            if (!(email.Contains("@") && !email.StartsWith("@") && email.EndsWith(".com")))
            {
                throw new ArgumentException("Invalid email address");
            }

            if (linkedInProfileUrl != null && !linkedInProfileUrl.StartsWith("https://www.linkedin.com/in/", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Invalid linkedIn profile url");
            }

            if (gitHubProfileUrl != null && !gitHubProfileUrl.StartsWith("https://github.com/", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Invalid github profile url");
            }
        }

        private static void MapUpdatedModelToExistingModel(CandidateDetails existingCandidateDetailsModel, CandidateDetails updatedCandidateDetailsModel)
        {
            existingCandidateDetailsModel.FirstName = updatedCandidateDetailsModel.FirstName;
            existingCandidateDetailsModel.LastName = updatedCandidateDetailsModel.LastName;
            existingCandidateDetailsModel.PhoneNumber = updatedCandidateDetailsModel.PhoneNumber;
            existingCandidateDetailsModel.AvailableForCallFrom = updatedCandidateDetailsModel.AvailableForCallFrom;
            existingCandidateDetailsModel.AvailableFroCallTo = updatedCandidateDetailsModel.AvailableFroCallTo;
            existingCandidateDetailsModel.LinkedInProfileUrl = updatedCandidateDetailsModel.LinkedInProfileUrl;
            existingCandidateDetailsModel.GitHubProfileUrl = updatedCandidateDetailsModel.GitHubProfileUrl;
            existingCandidateDetailsModel.FreeTextComment = updatedCandidateDetailsModel.FreeTextComment;
        }
    }
}
