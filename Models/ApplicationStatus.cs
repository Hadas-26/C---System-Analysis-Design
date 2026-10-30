/*
 * Written by Josh Green
 */

using System.ComponentModel.DataAnnotations;

namespace MedicaidEmploymentVerificationApplication.Models
{
    public class ApplicationStatus
    {
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        public Application Application { get; set; } = null!;

        public int? EmployeeId { get; set; }

        public Employee? Employee { get; set; } //Should be changed into a User

        public bool? IsApproved { get; set; }

        public DateTime? DateApproved { get; set; }
        public DateTime? DateSubmitted { get; set; }

        /// <summary>
        /// Determines if six months have passed since approval.
        /// </summary>
        public bool? RefreshNeeded => DateApproved != null ? DateApproved.Value.AddMonths(6) > DateTime.Now : null; //Change name to compliance check

        public string Notes { get; set; } = string.Empty;


    }
}
