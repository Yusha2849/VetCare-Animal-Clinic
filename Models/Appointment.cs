using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetCare_Animal_Clinic.Models
{
    public class Appointment
    {
        [Key]
        [Display(Name = "Appointment ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentID { get; set; }

        [Required]
        [Display(Name = "Appointment Date")]
        [DataType(DataType.Date)]
        public DateTime ADate { get; set; }

        [Required]
        [Display(Name = "Appointment Time")]
        [DataType(DataType.Time)]
        public DateTime ATime { get; set; }


        // Use PetID as the foreign key for the relationship with the Pet entity
        [Display(Name = "Pet ID")]
        public int PetID { get; set; }

        // Define the foreign key relationship
        [ForeignKey("PetID")]
        public virtual Pet? Pet { get; set; }
        

        [Required]
        [MaxLength(100)]
        [Display(Name = "Appointment Reason")]
        [DataType(DataType.Text)]
        public string Appointment_Reason { get; set; }

        [Required]
        [Display(Name = "Appointment Status")]
        [EnumDataType(typeof(AppointmentStatus))]
        public AppointmentStatus AppointmentStatus { get; set; }

        public Visit? Visit { get; set; } // Navigation property for Pet
    }

    public enum AppointmentStatus
    {
        Seen,
        Unseen
    }
}
