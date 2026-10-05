using System.ComponentModel.DataAnnotations;

namespace MedicaidEmploymentVerificationApplication.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public int EmployeeNumber { get; set; }
        public string FirstName { get; set; } = string.Empty;
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
        
        public DateTime HireDate { get; set; }

        public string UserId { get; set; } = string.Empty;

        public User User { get; set; } = null!;
    }
}
