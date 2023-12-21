using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public string PlanName { get; set; }
        public int PlanValidity { get; set; }
        public decimal PlanAmount { get; set; }

    }
}
