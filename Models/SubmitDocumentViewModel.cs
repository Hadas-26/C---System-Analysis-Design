namespace MedicaidEmploymentVerificationApplication.Models
{
    public class SubmitDocumentViewModel
    {
        public int ApplicationId { get; set; }
        public List<IFormFile> Documents { get; set; } = new List<IFormFile>();
    }
}
