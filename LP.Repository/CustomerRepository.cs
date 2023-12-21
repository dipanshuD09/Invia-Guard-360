using Dapper;
using LP.Common.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Repository
{
   
    internal class CustomerRepository : IRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public Customer( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        void IRepository.Delete(int id)
        {
            throw new NotImplementedException();
        }

        T IRepository.Get<T>(int id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<T> IRepository.GetAll<T>(string Email)
        {
            var connection = _unitOfWork.GetDbConnection();

            _unitOfWork.BeginTransaction();

            var parametersObject = new { Email };
            DynamicParameters parameters = Helper.HelperMethod(parametersObject);

            string query = Login.GetAllQuery(Email);


           _unitOfWork.BeginTransaction();
        var results = connection.Query<T>(query, parameters);
            _unitOfWork.CommitTransaction();


            _unitOfWork.CommitTransaction();

            return results;
        }

        void IRepository.Insert<T>(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
