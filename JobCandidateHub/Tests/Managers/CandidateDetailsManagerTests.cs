using JobCandidateHub.Managers;
using JobCandidateHub.Models;
using JobCandidateHub.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace JobCandidateHub.Tests.Managers
{
    public class CandidateDetailsManagerTests
    {
        private readonly ICandidateDetailsManager _candidateDetailsManager;
        private readonly Mock<ICsvFileService> _csvFileService = new();

        public CandidateDetailsManagerTests()
        {
            _candidateDetailsManager = new CandidateDetailsManager(_csvFileService.Object);
        }

        #region Create

        [Fact]
        public async Task Create_CandidateDetails_With_ValidModel_Ok_200()
        {
            //Arrange
            var model = GetCandidateDetailsModel("temp.candidate@gmail.com");

            _csvFileService.Setup(x => x.GetAllCandidateDetails())
                .ReturnsAsync(new List<CandidateDetails>());

            _csvFileService.Setup(x => x.SaveCandidateDetails(It.IsAny<List<CandidateDetails>>()));

            //Act
            await _candidateDetailsManager.CreateCandidateDetailsAsync(model);


            //Assert
            _csvFileService.Verify(x => x.GetAllCandidateDetails(), Times.Exactly(1));

            _csvFileService.Verify(x => x.SaveCandidateDetails(It.IsAny<List<CandidateDetails>>()), Times.Exactly(1));
        }

        [Fact]
        public async Task Create_CandidateDetails_With_InValidEmail_BadRequest_400()
        {
            //Arrange
            var model = GetCandidateDetailsModel("temp.candidate@gmail");

            //Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _candidateDetailsManager.CreateCandidateDetailsAsync(model));

            // Assert
            Assert.Equal("Invalid email address", exception.Message);
        }

        [Fact]
        public async Task Create_CandidateDetails_With_InValidLinkedInUrl_BadRequest_400()
        {
            //Arrange
            var model = GetCandidateDetailsModel("temp.candidate@gmail.com", "https://www.linkedin/random-candidate/");

            //Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _candidateDetailsManager.CreateCandidateDetailsAsync(model));

            // Assert
            Assert.Equal("Invalid linkedIn profile url", exception.Message);
        }

        [Fact]
        public async Task Create_CandidateDetails_With_InValidGitHubUrl_BadRequest_400()
        {
            //Arrange
            var model = GetCandidateDetailsModel("temp.candidate@gmail.com", gitHubProfileUrl: "https://randomcandidate");

            //Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _candidateDetailsManager.CreateCandidateDetailsAsync(model));

            // Assert
            Assert.Equal("Invalid github profile url", exception.Message);
        }

        #endregion

        #region Update

        [Fact]
        public async Task Create_CandidateDetails_With_ExistingEmailAndValidModel_ShouldUpdate_Ok_200()
        {
            //Arrange
            var model = GetCandidateDetailsModel("temp.candidate@gmail.com");

            _csvFileService.Setup(x => x.GetAllCandidateDetails())
                .ReturnsAsync(new List<CandidateDetails>{ GetCandidateDetailsModel("temp.candidate@gmail.com") });

            _csvFileService.Setup(x => x.SaveCandidateDetails(It.IsAny<List<CandidateDetails>>()));

            //Act
            await _candidateDetailsManager.CreateCandidateDetailsAsync(model);


            //Assert
            _csvFileService.Verify(x => x.GetAllCandidateDetails(), Times.Exactly(1));

            _csvFileService.Verify(x => x.SaveCandidateDetails(It.IsAny<List<CandidateDetails>>()), Times.Exactly(1));
        }

        #endregion

        #region Data Helper

        public CandidateDetails GetCandidateDetailsModel(string email, string? linkedInProfileUrl = null, string? gitHubProfileUrl = null)
        {
            return new CandidateDetails
            {
                Email = email,
                FirstName = "Random",
                LastName = "Candidate",
                PhoneNumber = "+999888777666",
                AvailableForCallFrom = "02:00 PM",
                AvailableFroCallTo = "06:00 PM",
                LinkedInProfileUrl = linkedInProfileUrl,
                GitHubProfileUrl = gitHubProfileUrl,
                FreeTextComment = "Random Comment"
            };
        }

        #endregion


    }
}
