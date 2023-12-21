using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LP.Models
{
    public class CustomerPlanViewModel
    {
        public int Id { get; set; }
       public string Email { get; set; }
       public string First_Name { get; set; }
       public string Last_Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public int Current_Plan { get; set; }
        public string Organization { get; set; }

    }
}
