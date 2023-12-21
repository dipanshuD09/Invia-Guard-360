using System;
using System.Data;
using System.Net;
using AspNetCoreHero.ToastNotification.Abstractions;
using Dapper;
using LP.Models;
using LP.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Live_Project.Controllers
{
    public class LoginController : Controller
    {
        private readonly IDbConnection _dbConnection;
        private readonly INotyfService _notyf;

        public LoginController(IDbConnection dbConnection, INotyfService notyf)
        {
            _dbConnection = dbConnection;
            _notyf = notyf;
        }
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel obj)
        {
            try
            {
                var parameters = new
                {
                    obj.Username,
                    obj.Password
                };

                var user = _dbConnection.QueryFirstOrDefault<User>("CheckUser", parameters, commandType: CommandType.StoredProcedure);

                if ( user != null)
                {
                    if (user.IsActive == 1)
                    {

                        HttpContext.Session.SetString("usr", user.Username);

                        if (user.IsAdmin == 1)
                        {
                            HttpContext.Session.SetString("UserType", "Admin");
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            HttpContext.Session.SetString("UserType", "Customer");
                            HttpContext.Session.SetString("CurrentUser", user.CustomerId.ToString());
                            return RedirectToAction("ShowCustomerData", "Customer", new { Id = user.CustomerId });
                        }
                    }
                    else
                    {
                        throw new Exception("User has been revoked Access, please Contact the Admin");

                    }
                }
                else
                {
                
                    throw new Exception("User not exist");
                }

            }

            catch(Exception ex)
            {
                return StatusCode(500, "An Error Occured:" + ex.Message);
            }
        }


        public IActionResult UserSignOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}   