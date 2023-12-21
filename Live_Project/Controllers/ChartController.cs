using Dapper;
using LP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text.Json.Nodes;
using System.Transactions;

namespace Live_Project.Controllers
{
    public class ChartController : Controller
    {
        private readonly IDbConnection _db;
        public ChartController(IDbConnection db)
        {
            _db = db;
        }

        [Route("/charts/RoundsLeft/{id}")]
        public IActionResult CustomerChartRounds(int id)
        {
            try
            {
                string sqlQuery = @"SELECT t.Current_Round, p.Max_Rounds
                                FROM Customers c
                                JOIN Transactions t ON c.Id = t.CustomerId
                                JOIN Plans p ON c.Current_Plan = p.Id
                                WHERE c.Id = @id
                                AND t.IsLatest = 1";

                var result = _db.QueryFirstOrDefault(sqlQuery, new { id });

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (SqlException ex)
            {
                
                return StatusCode(500, "An error occurred while processing the request." + ex.Message);
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, "An unexpected error occurred." + ex.Message);
            }

        }

        [Route("/charts/PlanValidity/{id}")]
        public IActionResult CustomerPlanValidity(int id)
        {
            try
            {
                string sqlQuery = @"SELECT t.Plan_Start, t.Plan_End
                                    FROM Customers c
                                    join Transactions t on t.CustomerId = c.Id
                                    where c.Id = @id
                                    and t.IsLatest = 1";

                var result = _db.QueryFirstOrDefault(sqlQuery, new { id });

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (SqlException ex)
            {

                return StatusCode(500, "An error occurred while processing the request." + ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An unexpected error occurred." + ex.Message);
            }

        }


        [Route("/charts/ThreatLevel/{id}")]
        public IActionResult ThreatLevel(int id)
        {
            try
            {
                string sqlQuery = "GetVulnerabilityCounts";

                var p = new DynamicParameters();
                p.Add("@customer_Id", id);
                p.Add("@Low_threat", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@Medium_threat", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@High_threat", dbType: DbType.Int32, direction: ParameterDirection.Output);

                _db.QueryFirstOrDefault(sqlQuery, p, commandType: CommandType.StoredProcedure);

                int[] Result = new int [3];
                Result[0] = p.Get<int>("@Low_threat");
                Result[1] = p.Get<int>("@Medium_threat");
                Result[2] = p.Get<int>("@High_threat");

                if (Result != null)
                {
                    return Ok(Result);
                }
                else
                {
                    return NotFound();
                }
            }

            catch (SqlException ex)
            {
                return StatusCode(500, "An error occurred while processing the request." + ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An unexpected error occurred." + ex.Message);
            }
        }
    }
}
