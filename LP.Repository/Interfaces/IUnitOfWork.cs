using LP.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Repository
{
    public interface IUnitOfWork:IDisposable
    {
            IAdminRepository AdminRepository { get; }
          ICustomerRepository CustomerRepository { get; }
            void SaveChanges();
    }
}
