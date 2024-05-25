using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace VetCare_Animal_Clinic.Models
{
    public class AnimalTypes
    {
        [Key, Column(Order = 0)]
        [MaxLength(50)]
        [Display(Name = "Animal Type")]
        [DataType(DataType.Text)]
        public string AnimalType { get; set; }

        [Key, Column(Order = 1)]
        [MaxLength(50)]
        [Display(Name = "Breed")]
        [DataType(DataType.Text)]
        public string Breed { get; set; }

        [MaxLength(100)]
        [Display(Name = "Species")]
        [DataType(DataType.Text)]
        public string Species { get; set; }

        public List<Pet>? Pets { get; set; } // Navigation property to a collection of Pets
    }
}
