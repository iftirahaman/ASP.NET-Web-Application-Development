namespace GBCSporting2021_TEC.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class Technician
    {
        public Technician()
        {
            Incidents = new HashSet<Incident>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(60)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [StringLength(30)]
        public string Password { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        public string AspNetUser { get; set; }

        [Required] 
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Incident> Incidents { get; set; }
    }

}
