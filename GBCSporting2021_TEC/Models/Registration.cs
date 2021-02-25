namespace GBCSporting2021_TEC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Registration
    {
        public int Id { get; set; }

        [Required]
        public int Customer { get; set; }

        [Required]
        public int Product { get; set; }

        public virtual Customer aCustomer { get; set; }
        public virtual Product aProduct { get; set; }
    }
}
