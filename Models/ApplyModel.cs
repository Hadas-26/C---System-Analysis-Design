using System.ComponentModel.DataAnnotations;

namespace MedicaidEmploymentVerificationApplication.Models
{
    public class ApplyModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(1, MinimumLength = 1, ErrorMessage = "Middle initial should be 1 character in length.")]
        public string? MiddleInitial { get; set; }

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string StreetAddress { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        [Length(5, 5)]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        public string VolunteerStatus { get; set; } = string.Empty;

        public decimal? VolunteerHours { get; set; }

        [Required]
        public string EmploymentStatus { get; set; } = string.Empty;

        public decimal? EmploymentHours { get; set; }

        [Required]
        public string EducationStatus { get; set; } = string.Empty;

        public decimal? EducationHours { get; set; }

        [Required]
        public string MaritalStatus { get; set; } = string.Empty;

        public bool DisabilityStatus { get; set; }

        public bool PregnancyStatus { get; set; }

        public bool PrimaryCaregiverStatus { get; set; }

        public List<IFormFile> Documents { get; set; } = new();
    }
}
