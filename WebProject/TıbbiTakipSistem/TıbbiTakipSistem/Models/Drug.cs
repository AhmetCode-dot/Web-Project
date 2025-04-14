using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TıbbiTakipSistem.Models
{
    public class Drug
    {
        public Drug()
        {
            Name = string.Empty;
            Dosage = string.Empty;
            Instructions = string.Empty;
        }

        [Key]
        public int DrugId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int DurationInDays { get; set; }

        public DateTime StartDate { get; set; }
        
        [Required]
        public string Dosage { get; set; }
        
        [Required]
        public string Instructions { get; set; }

        // Foreign keys
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public int? DoctorId { get; set; }
        
        [ForeignKey("DoctorId")]
        public virtual User? Doctor { get; set; }
    }
} 