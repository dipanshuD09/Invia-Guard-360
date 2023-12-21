using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int Transaction_Id { get; set; }
        public DateTime Plan_Start { get; set; }
        public DateTime Plan_End { get; set; }
        public int CurrentRound { get; set; }
        public bool IsLatest { get; set; }
        public int CustomerId { get; set; }
        public int PlanId { get; set; }
        public Customer Customer { get; set; } 
        public Plan Plan { get; set; } 
    }
}
