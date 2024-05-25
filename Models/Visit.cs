using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VetCare_Animal_Clinic.Areas.Data;

namespace VetCare_Animal_Clinic.Models
{
    public class Visit
    {
        [Key]
        [Display(Name = "Visit ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VisitID { get; set; }

        [Display(Name = "Appointment ID")]
        public int AppointmentID { get; set; }
        [ForeignKey("AppointmentID")]
        public virtual Appointment? Appointment { get; set; }

        [Display(Name = "Visit Comments")]
        [DataType(DataType.MultilineText)]
        public string VisitComments { get; set; }

        public AppUser? AppUser { get; set; } // Navigation property for Pet

    }
}
