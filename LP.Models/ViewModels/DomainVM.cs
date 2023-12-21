using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Models.ViewModels
{
    public class DomainVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Guid DomainId { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public string CriticalPort{ get; set; }
        public string OpenPort { get; set; }
        public string WebServer { get; set; }
        public int Round { get; set;}
        public int Max_Rounds { get; set;}
        public int Current_Round { get; set;}
    }
}

