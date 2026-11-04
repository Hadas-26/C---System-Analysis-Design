using System.ComponentModel.DataAnnotations;

namespace MedicaidEmploymentVerificationApplication.CustomAttributes
{
    public class RequiredString : ValidationAttribute
    {

        private string[] acceptedStrings {  get; set; }
        public RequiredString(string[] accepted) => this.acceptedStrings = accepted;
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is string s)
            {
                if(acceptedStrings.Contains(s))
                    return ValidationResult.Success;
                return new ValidationResult(base.ErrorMessage ?? $"The provided text does not meet the requirement");
            }
            string message = base.ErrorMessage ?? $"Must be a string type";
            return new ValidationResult(message);

        }
    }
}
