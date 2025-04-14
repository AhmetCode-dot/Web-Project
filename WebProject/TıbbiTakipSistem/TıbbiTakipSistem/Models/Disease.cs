using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TıbbiTakipSistem.Models
{
    public class Disease
    {
        public Disease()
        {
            Name = string.Empty;
            Description = string.Empty;
        }

        [Key]
        public int DiseaseId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime DiagnosisDate { get; set; }

        // Foreign keys
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public int? DoctorId { get; set; }
        
        [ForeignKey("DoctorId")]
        public virtual User? Doctor { get; set; }
    }
} 