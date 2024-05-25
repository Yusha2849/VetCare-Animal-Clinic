using System.ComponentModel.DataAnnotations;

namespace VetCare_Animal_Clinic.Models
{
    public class GreaterThanTodayAttribute : ValidationAttribute
    {
        public GreaterThanTodayAttribute()
        {
            
        }
        public string GetErrorMessage() => "Bookings Cannot Be Made in the Past";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;

            if (DateTime.Compare(date, DateTime.Now) < 0) return new ValidationResult(GetErrorMessage());
            else return ValidationResult.Success;
            
        }
    }
    public class LessThanTodayAttribute : ValidationAttribute
    {
        public LessThanTodayAttribute()
        {

        }
        public string GetErrorMessage() => "Pet's Date of Birth Cannot be in the Future";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;

            if (DateTime.Compare(date, DateTime.Now) > 0) return new ValidationResult(GetErrorMessage());
            else return ValidationResult.Success;

        }
    }
}
