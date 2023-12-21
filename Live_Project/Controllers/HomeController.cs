using Microsoft.AspNetCore.Mvc;
using System;
using LP.Models;
using LP.Repository;

namespace Live_Project.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("usr") != null)
            {
                if(HttpContext.Session.GetString("UserType") == "Admin")
                {
                    return RedirectToAction("Index","Dashboard");
                }
                else
                {
                    int? id = HttpContext.Session.GetInt32("CurrentUser");
                    return RedirectToAction("ShowCustomerData", "Customer",new { id });
                }
            }
            return View();
        }

        public IActionResult About()
        {
            return View();
        } 
        
        public IActionResult Contact()
        {
            return View();
        }
        
        public IActionResult Services()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Enquiry()
        {
            try
            {
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}