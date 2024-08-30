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
            var candidatesDetails = await _csvFileService.GetAllCandidateDetails();

            var existingCandidateDetails = candidatesDetails.FirstOrDefault(s => s.Email == candidateDetailsModel.Email);

            if (existingCandidateDetails != null)
            {
                var updateCandidateDetails = await UpdateCandidateDetailsAsync(existingCandidateDetails, candidateDetailsModel, candidatesDetails);

                return updateCandidateDetails;
            }

            candidatesDetails.Add(candidateDetailsModel);

            await _csvFileService.SaveCandidateDetails(candidatesDetails);

            return candidatesDetails.Find(x => x.Email == candidateDetailsModel.Email);
        }

        public async Task<CandidateDetails?> UpdateCandidateDetailsAsync(
            CandidateDetails existingCandidateDetailsModel,
            CandidateDetails updatedCandidateDetailsModel,
            List<CandidateDetails> allCandidatesDetails)
        {
            MapUpdatedModelToExistingModel(existingCandidateDetailsModel, updatedCandidateDetailsModel);

            await _csvFileService.SaveCandidateDetails(allCandidatesDetails);

            return allCandidatesDetails.Find(x => x.Email == existingCandidateDetailsModel.Email);
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
