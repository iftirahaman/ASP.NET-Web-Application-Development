namespace GBCSporting2021_TEC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Diagnostics.CodeAnalysis;

    public partial class Customer
    {
        public Customer()
        {
            Incidents = new HashSet<Incident>();
            Registrations = new HashSet<Registration>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Firstname { get; set; }

        [Required]
        [StringLength(30)]
        public string Lastname { get; set; }

        [Required]
        [StringLength(115)]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(150)]
        public string State { get; set; }

        [Required]
        [StringLength(15)]
        public string PostalCode { get; set; }

        [Required]
        public int Country { get; set; }

        [StringLength(60)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }
        public string Fullname => $"{Firstname} {Lastname}";
        public virtual Country aCountry { get; set; }

        public virtual ICollection<Incident> Incidents { get; set; }

        public virtual ICollection<Registration> Registrations { get; set; }
    }
}
