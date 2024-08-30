using JobCandidateHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobCandidateHub.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class CandidateDetailsController : ControllerBase
    {
        public CandidateDetailsController() { }


        /// <summary>
        ///  Post or Put Candidate Details
        /// </summary>
        /// <response code="404">Not found</response>
        /// <response code="422">The model is not valid.</response>
        /// <response code="500">Server error.</response>
        /// <returns></returns>
        [HttpPost]

        public async Task<ActionResult<CandidateDetails>> CreateCandidateDetails(
            [FromBody] CandidateDetails candidateDetailsModel)
        {
            return Ok("");
        }
    }
}
