using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Models
{
    public class Domain
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title Name is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Name Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "IpAddress is required.")]
        public string IpAddress { get; set; }

        [Required(ErrorMessage = "Critical Port is required.")]
        public string CriticalPort { get; set; }

        [Required(ErrorMessage = "Open Port is required.")]
        public string OpenPort { get; set; }

        [Required(ErrorMessage = "Web Server is required.")]
        public string WebServer { get; set; }
    }
}
