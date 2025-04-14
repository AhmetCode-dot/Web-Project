using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TıbbiTakipSistem.Models
{
    public class User
    {
        public User()
        {
            Name = string.Empty;
            Surname = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Specialty = string.Empty;
            HospitalInformation = string.Empty;
            Diseases = new HashSet<Disease>();
            Drugs = new HashSet<Drug>();
            Tests = new HashSet<Test>();
            Appointments = new HashSet<Appointment>();
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public bool IsDoctor { get; set; }

        // Doctor specific fields
        public string? Specialty { get; set; }
        public string? HospitalInformation { get; set; }

        // Navigation properties
        public virtual ICollection<Disease> Diseases { get; set; }
        public virtual ICollection<Drug> Drugs { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
} 