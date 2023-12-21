using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Models.ViewModels
{
        public class TransactionViewModel
        {

        public int Id { get; set; }

        [DisplayName("Transaction Id")]
        public int Transaction_Id { get; set; }
       
        [DisplayName("Plan Start")]
        public DateTime Plan_Start { get; set; }
       
        [DisplayName("Plan End")]
        public DateTime Plan_End { get; set; }
        
        [DisplayName("Plan Name")]
        public string Plan_Name { get; set; }

    }
}

