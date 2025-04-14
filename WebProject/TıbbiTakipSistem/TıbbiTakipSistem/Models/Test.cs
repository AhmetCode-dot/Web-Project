using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TıbbiTakipSistem.Models
{
    public class Test
    {
        public Test()
        {
            Name = string.Empty;
            Results = string.Empty;
        }

        [Key]
        public int TestId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Results { get; set; }

        public DateTime TestDate { get; set; }

        public string? Notes { get; set; }

        // Foreign keys
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public int? DoctorId { get; set; }
        
        [ForeignKey("DoctorId")]
        public virtual User? Doctor { get; set; }
    }
} 