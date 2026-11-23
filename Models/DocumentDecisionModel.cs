namespace MedicaidEmploymentVerificationApplication.Models
{
    public class DocumentDecisionModel
    {
        public int ApplicationId { get; set; }
        public int DocumentId { get; set; }
        public int StatusId { get; set; }
        public bool Approved { get; set; }
    }
}
