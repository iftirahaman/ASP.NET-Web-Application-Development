namespace GBCSporting2021_TEC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Incident
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        public int Customer { get; set; }

        [Required]
        public int Product { get; set; }

        public string Technician { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Display(Name ="Date Opened")]
        public DateTime? DateOpened { get; set; }

        [Required]
        [Display(Name = "Date Closed")]
        public DateTime DateClosed { get; set; }

        public virtual Customer aCustomer { get; set; }
        public virtual Product aProduct { get; set; }
        public virtual Technician aTechnician { get; set; }
    }
}
