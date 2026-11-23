namespace MedicaidEmploymentVerificationApplication.Models
{
    public class NotesViewModel
    {
        public int StatusId { get; set; }
        public string Notes { get; set; } = string.Empty;
        public bool IsRejected { get; set; }

    }
}
