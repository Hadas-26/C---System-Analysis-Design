using System.ComponentModel.DataAnnotations;

namespace MedicaidEmploymentVerificationApplication.Models
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Please enter a Username")]
        [StringLength(255, ErrorMessage = "Please select a shorter username")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage ="Please choose a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
