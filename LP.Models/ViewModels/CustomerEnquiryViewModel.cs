using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Models.ViewModels
{
    public class CustomerEnquiryViewModel
    {

            public string CustomerName { get; set; }
            public string CustomerEmail { get; set; }
            public string CurrentPlanName { get; set; }
            public int PlanValidity { get; set; }
            public List<Enquiry> Enquiries { get; set; }

    }
}
