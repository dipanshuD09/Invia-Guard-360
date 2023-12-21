using AspNetCoreHero.ToastNotification.Abstractions;
using LP.Models.ViewModels;
using LP.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Live_Project.Controllers
{
    [Authorized_Access_Only]
    public class SecurityDashboard : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;
        public SecurityDashboard(IUnitOfWork unitOfWork, INotyfService notyf)
        {
            _notyf = notyf;
           _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowCustomerDomains(int id)
        {
            try
            {
                var Result = _unitOfWork.AdminRepository.GetAllDomains(id);
                if(Result.Count() == 0)
                {
                    _notyf.Error("No Domains Found");
                    return RedirectToAction("Index");
                }
                _unitOfWork.SaveChanges();

                return View(Result);
            }

            catch (Exception e)
            {
                return StatusCode(500, "An Error Occured:" + e.Message);
            }
        }

        public IActionResult Vulnerabilities(int id)
        {
            try
            {
                if(id == Convert.ToInt32(HttpContext.Session.GetString("CurrentUser")) || HttpContext.Session.GetString("UserType") == "Admin")
                {
                var vulnerabilities = _unitOfWork.AdminRepository.GetVulneribilities(id);

                if (vulnerabilities == null || !vulnerabilities.Any())
                {
                    return NotFound(); 
                }
                   return View(vulnerabilities);
                }

                else
                {
                    return BadRequest(); //400 status code
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
