using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VetCare_Animal_Clinic.Areas.Data;
using VetCare_Animal_Clinic.Models;

namespace VetCare_Animal_Clinic.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define a composite key for the AnimalType model
            modelBuilder.Entity<AnimalTypes>()
                .HasKey(at => new { at.AnimalType, at.Breed });

            // Define relationships
            modelBuilder.Entity<Pet>()
                .HasOne(p => p.AppUser) // Each Pet belongs to one AppUser
                .WithMany(u => u.Pets) // Each AppUser can have multiple Pets
                .HasForeignKey(p => p.UserId); // Foreign key in Pet referencing AppUser

            modelBuilder.Entity<Pet>()
                .HasOne(p => p.AnimalTypes)  // Each Pet is associated with one AnimalType
                .WithMany(at => at.Pets) // Each AnimalType can have multiple Pets
                .HasForeignKey(p => new { p.AnimalType, p.Breed });  // Composite foreign key in Pet

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Pet)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PetID);

            
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Visit) // Each Appointment is associated with one Visit
                .WithOne(v => v.Appointment) // Each Visit is associated with one Appointment
                .HasForeignKey<Visit>(v => v.AppointmentID); // Foreign key in Visit referencing Appointment

            
        }

        public DbSet<VetCare_Animal_Clinic.Models.AnimalTypes>? AnimalTypes { get; set; }

        public DbSet<VetCare_Animal_Clinic.Models.Pet>? Pet { get; set; }

        public DbSet<VetCare_Animal_Clinic.Models.Appointment>? Appointment { get; set; }

        public DbSet<VetCare_Animal_Clinic.Models.Visit>? Visit { get; set; }

        //public DbSet<VetCare_Animal_Clinic.Models.Visit>? Visit { get; set; }
    }
}
