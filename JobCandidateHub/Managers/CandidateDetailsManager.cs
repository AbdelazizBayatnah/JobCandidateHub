using JobCandidateHub.Models;

namespace JobCandidateHub.Managers
{
    public interface ICandidateDetailsManager
    {
        Task<CandidateDetails?> CreateCandidateDetailsAsync(CandidateDetails candidateDetailsModel);

        Task<CandidateDetails?> UpdateCandidateDetailsAsync(CandidateDetails newCandidateDetailsModel);
    }

    public class CandidateDetailsManager : ICandidateDetailsManager
    {
        public CandidateDetailsManager()
        {

        }

        public Task<CandidateDetails?> CreateCandidateDetailsAsync(CandidateDetails candidateDetailsModel)
        {
            throw new NotImplementedException();
        }

        public Task<CandidateDetails?> UpdateCandidateDetailsAsync(CandidateDetails newCandidateDetailsModel)
        {
            throw new NotImplementedException();
        }
    }
}
