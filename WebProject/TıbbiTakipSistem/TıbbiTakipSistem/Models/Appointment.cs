using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TıbbiTakipSistem.Models
{
    public class Appointment
    {
        public Appointment()
        {
            Status = "Pending";
            Description = string.Empty;
            Notes = string.Empty;
        }

        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        public string? Description { get; set; }

        [Required]
        public string Status { get; set; } // Pending, Confirmed, Cancelled, Completed

        public string? Notes { get; set; }

        // Foreign keys
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public int DoctorId { get; set; }
        
        [ForeignKey("DoctorId")]
        public virtual User? Doctor { get; set; }
    }
} 