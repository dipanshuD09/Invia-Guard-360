using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Models.ViewModels
{
    public class VulneribilitiesVM
    {
        public Guid VulnerabilityId { get; set; }
        public string VulnerabilityName { get; set; }
        public string VulnerabilityDescription { get; set; }
        public string VulnerabilityPath { get; set; }
        public int SeverityRanking { get; set; }
        public string RemediationInfo { get; set; }
        public string DomainName { get; set; }
        public string DomainIpAddress { get; set; }
    }
}
