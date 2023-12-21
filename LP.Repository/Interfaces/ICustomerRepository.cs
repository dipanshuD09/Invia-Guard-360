using LP.Models;
using LP.Models.ViewModels;
using System;
using System.Collections.Generic;


namespace LP.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<CustomerPlanViewModel> GetAllCustomers();
        CustomerDetailsVM GetCustomerById(int id);
        bool SendEnquiryToAdmin(int PlanId, int customerId);
        bool UpdateEnquiryStatus(int enquiryId, bool isResolved);
        IEnumerable<TransactionViewModel> GetTransactions(int id);
    }
}
