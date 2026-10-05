using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MedicaidEmploymentVerificationApplication.Models
{
    
    public class Applicant
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        public string LastName { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public User User { get; set; } = null!;

    }
}
