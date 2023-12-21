using LP.Models;
using LP.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Repository.Interfaces
{
    public interface IAdminRepository
    {
        bool AddCustomer(Customer obj);
        bool DeleteCustomer(int id);
        bool DisableUser(int id);
        bool UpdateRound(int Id);
        IEnumerable<CustomerEnquiryViewModel> Enquires();
        int ActiveEnquires();
        IEnumerable<DomainVM> GetAllDomains(int Id);
        bool AddDomain(Domain domain);
        bool UpdateCustomer(CustomerDetailsVM obj);
        bool DeleteDomain(int id, string IpAddress);
        IEnumerable<VulneribilitiesVM> GetVulneribilities(int Id);
        int DomainCount(int id);
    }
}
