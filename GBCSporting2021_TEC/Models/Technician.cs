using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBCSporting2021_TEC.Models
{
    public partial class Technician
    {
        public Technician()
        {
            Incidents = new HashSet<Incident>();
        }

        public string Id { get; set; }

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

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Incident> Incidents { get; set; }
    }

}
