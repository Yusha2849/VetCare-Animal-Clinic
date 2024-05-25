using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VetCare_Animal_Clinic.Areas.Data;

namespace VetCare_Animal_Clinic.Models
{
    public class Pet
    {
        [Key]
        [Display(Name = "Pet ID")]
        public int PetId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Pet Name")]
        [DataType(DataType.Text)]
        public string PetName { get; set; }

        [Display(Name = "User ID")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser? AppUser { get; set; }

        [Required(ErrorMessage = "Please select an Animal Type")]
        [Display(Name = "Animal Type")]
        public string AnimalType { get; set; }

        [Required(ErrorMessage = "Please select a Breed")]
        [Display(Name = "Breed")]
        public string Breed { get; set; }

        [ForeignKey("Breed, AnimalType")]
        public virtual AnimalTypes? AnimalTypes { get; set; }

        [Required]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        
        public DateTime YearOfBirth { get; set; }

        [NotMapped] // Age is not stored in the database
        public int Age
        {
            get
            {
                return DateTime.Now.Year - YearOfBirth.Year;
            }
        }
        // Define the navigation property to represent the relationship with appointments
        public virtual ICollection<Appointment>? Appointments { get; set; }
    }
}
