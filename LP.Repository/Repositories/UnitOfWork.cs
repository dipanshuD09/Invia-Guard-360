using LP.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Repository.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction _transaction;
        private IAdminRepository _adminRepository;
        private ICustomerRepository _customerRepository;

        public UnitOfWork(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            _transaction = _dbConnection.BeginTransaction();
        }
        ICustomerRepository IUnitOfWork.CustomerRepository {
            get
            {
                if (_customerRepository == null)
                {
                    _customerRepository = new CustomerRepository(_dbConnection, _transaction);
                }
                return _customerRepository;
            }
        }

        IAdminRepository IUnitOfWork.AdminRepository
        {
            get
            {
                if (_adminRepository == null)
                {
                    _adminRepository = new AdminRepository(_dbConnection, _transaction);
                }
                return _adminRepository;
            }
        }

        void IUnitOfWork.SaveChanges()
        {
            _transaction.Commit();
        }

        void IDisposable.Dispose()
        {
            _transaction?.Dispose();
            _dbConnection?.Close();
        }
    }
}
