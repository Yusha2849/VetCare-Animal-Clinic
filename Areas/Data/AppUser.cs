using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using VetCare_Animal_Clinic.Models;

namespace VetCare_Animal_Clinic.Areas.Data
{
    public class AppUser: IdentityUser
    {
        [Required]
        [Display(Name = "ID Number")]
        public string IDNumber { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Home Address")]
        public string HomeAddress { get; set; }

        [Required]
        [Display(Name = "Cell Number")]
        [Phone]
        public string PhoneNumber { get; set; }

        public List<Pet>? Pets { get; set; } // Navigation property to a collection of Pets
       
    }
}
