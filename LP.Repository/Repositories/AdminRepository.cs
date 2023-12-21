using LP.Models;
using LP.Repository.Interfaces;
using System;
using System.Linq;
using Dapper;
using System.Data;
using LP.Models.ViewModels;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using System.Net;

namespace LP.Repository.Repositories
{
    public class AdminRepository:IAdminRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _transaction;

        public AdminRepository(IDbConnection dbConnection, IDbTransaction transaction)
        {
            _dbConnection = dbConnection;
            _transaction = transaction;
        }

        bool IAdminRepository.AddCustomer(Customer obj)
        {
            try
            {
               
                _dbConnection.Execute("sp_Create_Customer", new
                {
                    Email = obj.Email,
                    First_Name = obj.F_Name,
                    Last_Name = obj.L_Name,
                    City = obj.City.ToString(),
                    Country = obj.Country.ToString(),
                    Address = obj.Address,
                    Current_Plan = obj.CurrentPlan,
                    TransactionId = GenerateNewTransactionId(),
                    PlanId = obj.CurrentPlan,
                    Organization = obj.Organisation
                }, commandType: CommandType.StoredProcedure, transaction: _transaction);


                return true;
            }


            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }

        int GenerateNewTransactionId()
        {
            Random rand = new Random();
            int transactionId = rand.Next(100000, 999999);

            bool isUnique = IsTransactionIdUnique(transactionId);
            if (isUnique)
            {
                return transactionId;
            }
            else
            {
                return GenerateNewTransactionId();
            }

        }

        bool IsTransactionIdUnique(int transactionId)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Transactions WHERE Transaction_Id = @transactionId";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@transactionId", transactionId);

                int count = _dbConnection.Query<int>(query, dp,transaction:_transaction).FirstOrDefault();
                return count == 0;
            }

            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }

        bool IAdminRepository.DeleteCustomer(int id)
        {
            try
            {
                _dbConnection.Execute("Sp_Delete_Customer", new { Customer_Id = id }, commandType: CommandType.StoredProcedure, transaction: _transaction);
                return true;
            }

            catch (Exception ex)
            {
                return false;
                throw new Exception("Error deleting customer.", ex);
            }
        }

        bool IAdminRepository.DisableUser(int id)
        {
            try
            {
                string query = "SELECT IsActive FROM Users WHERE CustomerId = @customerId";
                int currentIsActiveValue = _dbConnection.QuerySingleOrDefault<int>(query, new { customerId = id }, transaction: _transaction);

                int newIsActiveValue = currentIsActiveValue == 1 ? 0 : 1;

                string updateQuery = "UPDATE Users SET IsActive = @newIsActiveValue WHERE CustomerId = @customerId";
                _dbConnection.Execute(updateQuery, new { newIsActiveValue, customerId = id }, transaction: _transaction);

                return true;
            }

            catch (Exception ex)
            {
                return false;
                throw new Exception("Error deleting customer.", ex);
            }
        }

        bool IAdminRepository.UpdateRound(int Id)
        {
            try
            {
                var rowsAffected =_dbConnection.Execute("Sp_RoundUpdate", new { Customer_Id = Id },
                    commandType: CommandType.StoredProcedure, transaction: _transaction);
                if(rowsAffected >= 1)
                {
                return true;
                }
                else { 
                    return false; 
                };
            }

            catch (Exception ex)
            {
                return false;
                throw new Exception("Error updating round.", ex);
            }
        }

        IEnumerable<CustomerEnquiryViewModel> IAdminRepository.Enquires()
        {
            try
            {

                string query = @"
                                SELECT
                                    C.First_Name + ' ' + C.Last_Name AS CustomerName,
                                    C.Email AS CustomerEmail,
                                    P.Plan_Name AS CurrentPlanName,
                                    P.Plan_Validity AS PlanValidity,
                                    E.Id,
                                    E.Enquiry_Date,
                                    E.isResolved,
                                    E.Message
                                FROM Enquires E
                                INNER JOIN Customers C ON E.CustomerId = C.Id
                                INNER JOIN Plans P ON C.PlanId = P.Id
                                ORDER BY CustomerName, E.isResolved ASC, E.Enquiry_Date ASC";

                var results = new Dictionary<string, CustomerEnquiryViewModel>();
                _dbConnection.Query<CustomerEnquiryViewModel, Enquiry, CustomerEnquiryViewModel>(
                    query,
                    (viewModel, enquiry) =>
                    {
                        if (!results.TryGetValue(viewModel.CustomerName, out var existingViewModel))
                        {
                            existingViewModel = viewModel;
                            existingViewModel.Enquiries = new List<Enquiry>();
                            results[viewModel.CustomerName] = existingViewModel;
                        }
                        existingViewModel.Enquiries.Add(enquiry);
                        return viewModel;
                    },
                    splitOn: "Id",
                    transaction: _transaction
                );

                var groupedResults = results.Values.ToList();

                return groupedResults;
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
                throw;
            }
        }

        int IAdminRepository.ActiveEnquires()
        {
            try
            {
                string sql = "SELECT COUNT(*) FROM enquires WHERE IsResolved = 0";
                int Count = _dbConnection.QuerySingle<int>(sql,transaction:_transaction);

                return Count;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
                throw;
            }
        }

        IEnumerable<DomainVM> IAdminRepository.GetAllDomains(int Id)
        {
            try
            {
                string sql = @"SELECT
                                     c.Id ,
                                     d.Title,
                                     d.Id AS DomainId,
                                     d.Name,
                                     d.IpAddress,
                                     d.CriticalPort,
                                     d.OpenPort,
                                     d.WebServer,
                                     d.Round,
                                     p.Max_Rounds
                                 FROM
                                     Customers c
                                 INNER JOIN
                                     Transactions t ON c.id = t.CustomerId
                                 INNER JOIN
                                     Plans p ON p.Id = t.PlanId
                                 INNER JOIN
                                     Domains d ON d.Customer_Id = c.Id
                                 WHERE
                                     c.Id = @Cus_Id AND t.IsLatest = 1
                                 ORDER BY
                                     d.Round DESC;";
                
                var domains = _dbConnection.Query<DomainVM>
                (sql, new { @Cus_Id = Id }, transaction: _transaction);
                return domains.ToList();
            }

            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                throw;
            }
        }

        bool IAdminRepository.UpdateCustomer(CustomerDetailsVM obj)
        {
            try
            {
                _dbConnection.Execute("Update_Customer", new
                {
                    Customer_Id = obj.Id,
                    Email = obj.Email,
                    First_Name = obj.First_Name,
                    Last_Name = obj.Last_Name,
                    City = obj.City,
                    Address = obj.Address,
                    Current_Plan = obj.Plan_Name,
                    Organization = obj.Organization
                }, commandType: CommandType.StoredProcedure, transaction: _transaction);


                return true;
            }


            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
                return false;
            }
        }

        bool IAdminRepository.DeleteDomain(int Customer_Id, string IpAddress)
        {

            try
            {
                string sql = @"DELETE FROM Domains WHERE IpAddress = @IpAddress AND Customer_Id = @Customer_Id";
                var result = _dbConnection.Execute(sql, new { IpAddress, Customer_Id },transaction:_transaction);

                if(result == 0)
                {
                    return false;
                }
                else
                {
                 _transaction.Commit();
                  return true; 
                }
            }


            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                throw;
            }
        }

        bool IAdminRepository.AddDomain(Domain domain)
        {
            try
            {
               var Result = _dbConnection.Execute("Add_New_Domain", new
                {
                   domain.Id,
                    domain.Title,
                    domain.Name,
                    domain.IpAddress,
                    domain.CriticalPort,
                    domain.OpenPort,
                    domain.WebServer,
                }, commandType: CommandType.StoredProcedure, transaction: _transaction); 


                if(Result != 0)
                {
                    _transaction.Commit();
                    return true;
                }
                else
                {
                    throw new Exception("An Error occur while adding  new Domain");
                }
            }


            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
                throw;
            }

        }

        IEnumerable<VulneribilitiesVM> IAdminRepository.GetVulneribilities(int Id)
        {
            try
            {
                var result = _dbConnection.Query<VulneribilitiesVM>(@"
                                                                     SELECT
                                                         v.Id AS VulnerabilityId,
                                                         v.Name AS VulnerabilityName,
                                                         v.Description AS VulnerabilityDescription,
                                                         v.Path AS VulnerabilityPath,
                                                         v.SeverityRank AS SeverityRanking,
                                                         v.Remidiation AS RemediationInfo,
                                                         d.Name AS DomainName,
                                                         d.IpAddress AS DomainIpAddress           
                                                     FROM
                                                         Domains d
                                                     JOIN
                                                         Customers c ON d.Customer_Id = c.Id
                                                     JOIN
                                                         Vulnerabilities v ON v.DomainId = d.Id
                                                     WHERE
                                                         c.Id = @CustomerId
                                                     ORDER BY
                                                         v.SeverityRank DESC
                          ", new { CustomerId = Id },transaction:_transaction);
   
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving vulnerabilities: {ex.Message}");
                throw;
            }
        }

        int IAdminRepository.DomainCount(int id)
        {
            try
            {
                var query = @"select COUNT(*) from Domains where Customer_Id = @CustomerId";

                var Result = _dbConnection.QuerySingle<int>(query, new { CustomerId = @id }, transaction: _transaction);

                return Result;
            }

            catch(Exception ex)
            {
                Console.WriteLine("Error occured:" + ex.Message);
                throw;
            }
        }
    }
}


