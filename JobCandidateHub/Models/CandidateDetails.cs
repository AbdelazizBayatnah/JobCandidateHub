using System.ComponentModel.DataAnnotations;

namespace JobCandidateHub.Models
{
    public class CandidateDetails
    {
        [Key]
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? AvailableForCallFrom { get; set; }

        public string? AvailableFroCallTo { get; set; }

        public string? LinkedInProfileUrl { get; set; }

        public string? GitHubProfileUrl { get; set; }

        [Required]
        public string FreeTextComment { get; set; } = null!;
    }
}
