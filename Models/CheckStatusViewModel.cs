namespace MedicaidEmploymentVerificationApplication.Models
{
    public class CheckStatusViewModel
    {
        public List<ApplicationDocument> Documents { get; set; } = new();
        public ApplicationStatus ApplicationStatus { get; set; } = null!;
        public Application Application { get; set; } = null!;
    }
}
