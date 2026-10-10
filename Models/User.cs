using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MedicaidEmploymentVerificationApplication.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        
    }
}
