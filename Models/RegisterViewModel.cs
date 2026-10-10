using System.ComponentModel.DataAnnotations;

namespace MedicaidEmploymentVerificationApplication.Models
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Please enter your First Name")]
        [StringLength(255, ErrorMessage = "We're sorry, your first name is too long. Please enter a shortened version.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your Last Name")]
        [StringLength(255, ErrorMessage = "We're sorry, your last name is too long. Please enter a shortened version.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a Username.")]
        [StringLength(255, ErrorMessage = "Please select a shorter username.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter an email address.")]
        [StringLength(255, ErrorMessage = "Please use a shorter email address.")]
        [EmailAddress(ErrorMessage = "Email is in an incorrect format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage ="Please choose a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
