using System.ComponentModel.DataAnnotations;

namespace MedicaidEmploymentVerificationApplication.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter a Username")]
        [StringLength(255, ErrorMessage = "Please select a shorter username")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please choose a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string ReturnURL { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
