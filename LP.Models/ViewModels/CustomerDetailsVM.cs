using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LP.Models.ViewModels
{
    public class CustomerDetailsVM
    {
        public int Id { get; set; }



        [Display(Name = "First Name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Enter Valid Name.")]
        [Required(ErrorMessage = "First Name is required.")]
        public string First_Name { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Enter Valid Last Name.")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string Last_Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [RegularExpression(@"^[a-zA-Z]+(?:[\\s-][a-zA-Z]+)*$", ErrorMessage = "Invalid City Name.")]
        public string City { get; set; }

        public string Address { get; set; }
        public string Organization { get; set; }

        [Display(Name = "Plan Name")]
        public PlanType Plan_Name { get; set; }

        [Display(Name = "Current Round")]
        public int Current_Round { get; set; }

        [Display(Name = "Plan Start")]
        public DateTime Plan_Start { get; set; }

        [Display(Name = "Plan End")]
        public DateTime Plan_End { get; set; }

        [DisplayName("Plan Id")]
        public int PlanId { get; set; }

    }

     public enum PlanType
    {
        Basic = 1,
        Standard = 2,
        Premium = 3
    }


}