using Dapper;
using LP.Models;
using LP.Repository.Interfaces;
using LP.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;

namespace LP.Repository.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _transaction;


      public CustomerRepository(IDbConnection dbConnection,IDbTransaction transaction)
      {
        _dbConnection = dbConnection;
        _transaction = transaction;
        
      }

         IEnumerable<CustomerPlanViewModel> ICustomerRepository.GetAllCustomers()
        {
            try
            {
               
                var customers = _dbConnection.Query<CustomerPlanViewModel>
                ("sp_GetAllCustomerData", null, commandType: CommandType.StoredProcedure, transaction: _transaction);
                return customers;
            }

            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                throw;
            }
        }

         CustomerDetailsVM ICustomerRepository.GetCustomerById(int id)
        {
            try
            {
                var parameters = new
                {
                    customerId = id
                };

                string sql = "sp_CustomerPlanDetails";
                var result = _dbConnection.QueryFirstOrDefault<CustomerDetailsVM>(
                    sql,
                    parameters,
                    commandType: CommandType.StoredProcedure, transaction: _transaction);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                throw; 
            }
        }

        bool ICustomerRepository.SendEnquiryToAdmin(int PlanId,int customerId )
        {
            try
            {
               
                _dbConnection.Execute("sp_AddEnquiry", new { Id = PlanId, CustomerId = customerId },
                    commandType: CommandType.StoredProcedure, transaction: _transaction);
                return true; 
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return false; 
            }

        }

        public bool UpdateEnquiryStatus(int enquiryId, bool isResolved)
        {
                try
                {
                    string sql = "UPDATE Enquires SET IsResolved = @IsResolved WHERE Id = @EnquiryId";

                    int rowsAffected = _dbConnection.Execute(sql, new { EnquiryId = enquiryId, IsResolved = isResolved },
                        transaction: _transaction);

                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
                }
        }

         IEnumerable<TransactionViewModel> ICustomerRepository.GetTransactions(int Id)
        {
            try
            {

               var Results = _dbConnection.Query<TransactionViewModel>("sp_CustomerTransactions", new { customerId = Id },
                   commandType: CommandType.StoredProcedure, transaction: _transaction);

                return Results;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                throw;
            }
        }
    }
}
