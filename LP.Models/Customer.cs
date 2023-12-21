using System;
using System.ComponentModel.DataAnnotations;

namespace LP.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Enter Valid Name.")]
        [Required(ErrorMessage = "First Name is required.")]
        [RegularExpression("[a-zA-Z][a-zA-Z ]+[a-zA-Z]$",ErrorMessage ="Numbers and Special character are not allowed")]
        public string F_Name { get; set; }

        [Display(Name = "Last Name")]   
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Enter Valid Last Name.")]
        [Required(ErrorMessage = "Last Name is required.")]
        [RegularExpression("[a-zA-Z][a-zA-Z ]+[a-zA-Z]$", ErrorMessage = "numbers and Special character are not allowed")]
        public string L_Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage ="Please provide a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Current Plan is required.")]
        [Display(Name = "Current Plan")]
        public PlanType CurrentPlan { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public City City { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public Country Country { get; set; }
       
        [Required(ErrorMessage = "Organisation is required.")]
        public string Organisation{ get; set; }
        public DateTime Created_Date { get; set;} 
        public int TransactionId { get; set; }
        public int PlanId { get; set; }
    }

    public enum PlanType
    {
        Basic = 1,
        Standard = 2,
        Premium = 3
    }

    public enum City
    {
        Noida,
        Gurugram,
        Mumbai,
        Delhi,
        Bangalore,
        Kolkata,
        Chennai,
        Hyderabad,
        Pune,
        Ahmedabad,
        Jaipur,
        Chandigarh,
        Other
    }

    public enum Country
    {
        India,
        Australia,
        Singapore,
        UnitedStates,
        Indonesia,
        Brazil,
        Nigeria,
        Russia
    }

}