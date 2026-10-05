using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MedicaidEmploymentVerificationApplication.Models
{
    public class Application
    {
        
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public char MiddleInitial { get; set; }

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string StreetAddress { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        [Length(6, 6)]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        [Length(10, 11)]
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

        [Required]
        public bool DisabilityStatus { get; set; }

        [Required]
        public bool PregnancyStatus { get; set; }

        [Required]
        public bool PrimaryCaregiverStatus { get; set; }

        public int ApplicantId { get; set; }

        public Applicant Applicant { get; set; } = null!;
    }
}
