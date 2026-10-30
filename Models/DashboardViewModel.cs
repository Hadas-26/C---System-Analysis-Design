/*
 * Written by Josh Green
 */

namespace MedicaidEmploymentVerificationApplication.Models
{
    public class DashboardViewModel
    {
        public List<ApplicationStatus> ApplicationStatuses { get; set; } = new();
        public List<ApplicationDocument> ApplicationDocuments { get; set; } = new();
    }
}
