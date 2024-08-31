using JobCandidateHub.Controllers;
using JobCandidateHub.Managers;
using JobCandidateHub.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobCandidateHub.Tests.Controllers
{
    public class CandidateDetailsControllerTests
    {
        private readonly Mock<ICandidateDetailsManager> _mockCandidateDetailsManager = new();

        #region Create

        [Fact]
        public async Task Create_CandidateDetails_With_ValidModel_Ok_200()
        {
            //Arrange Data
            var model = GetCandidateDetailsModel("temp.candidate@gmail.com");

            _mockCandidateDetailsManager.Setup(x => x.CreateCandidateDetailsAsync(model))
                .ReturnsAsync(new CandidateDetails());

            var controller = new CandidateDetailsController(_mockCandidateDetailsManager.Object);

            //Act
            var response = await controller.CreateCandidateDetails(model);

            //Assert
            var actionResult = Assert.IsType<ActionResult<CandidateDetails>>(response);
            var createdAtActionResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.Equal(200, createdAtActionResult.StatusCode);

            _mockCandidateDetailsManager.Verify(x => x.CreateCandidateDetailsAsync(model), Times.Exactly(1));
        }

        [Fact]
        public async Task Create_CandidateDetails_With_InValidEmail_BadRequest_400()
        {
            //Arrange Data
            var model = GetCandidateDetailsModel("temp.candidate@gmail");

            _mockCandidateDetailsManager.Setup(x => x.CreateCandidateDetailsAsync(model))
                .ThrowsAsync(new Exception("Invalid email"));

            var controller = new CandidateDetailsController(_mockCandidateDetailsManager.Object);

            //Act
            var response = await controller.CreateCandidateDetails(model);

            //Assert
            var actionResult = Assert.IsType<ActionResult<CandidateDetails>>(response);
            var createdAtActionResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);

            Assert.Equal(400, createdAtActionResult.StatusCode);

            _mockCandidateDetailsManager.Verify(x => x.CreateCandidateDetailsAsync(model), Times.Exactly(1));
        }

        #endregion

        #region Data Helper

        public CandidateDetails GetCandidateDetailsModel(string email)
        {
            return new CandidateDetails
            {
                Email = email,
                FirstName = "Random",
                LastName = "Candidate",
                PhoneNumber = "+999888777666",
                AvailableForCallFrom = "02:00 PM",
                AvailableFroCallTo = "06:00 PM",
                LinkedInProfileUrl = "https://www.linkedin.com/in/random-candidate/",
                GitHubProfileUrl = "https://github.com/randomcandidate",
                FreeTextComment = "Random Comment"
            };
        }

        #endregion

    }
}
