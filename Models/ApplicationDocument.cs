/*
 * Written by Josh Green
 */

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MedicaidEmploymentVerificationApplication.Models
{
    public class ApplicationDocument
    {
        public int Id { get; set; }

        [Required]
        public byte[] Document { get; set; } = Array.Empty<byte>();
        
        [Required]
        public string Extension { get; set; } = string.Empty;
        public bool Reviewed { get; set; }
        public bool Rejected { get; set; }
        [Required]
        public int ApplicantionId { get; set; }
        public Application Application { get; set; } = null!;
    }
}
