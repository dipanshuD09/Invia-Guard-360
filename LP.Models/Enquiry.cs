using LP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Models
{
    public class Enquiry
    {
        public int Id { get; set; }
        public DateTime Enquiry_Date { get; set; }
        public bool isResolved { get; set; }
        public string Message { get; set; }
    }
}


