using Dapper;
using LP.Models;
using LP.Models.ViewModels;
using LP.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using AspNetCoreHero.ToastNotification.Abstractions;
using LP.Repository;
using System.Net;

namespace Live_Project.Controllers
{
    [Authorized_Access_Only]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;

        public DashboardController(IUnitOfWork UnitOfWork, INotyfService notyf)
        {
            _unitOfWork = UnitOfWork;
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserType") == "Admin")
            {
                return View();
            }
            else
            {
                return StatusCode(401, "Unauthorized access");
                

            }
        }
 
        public IActionResult Enquires()
        {
            try
            {
                var Result = _unitOfWork.AdminRepository.Enquires();

                if(Result != null)
                {
                    return View(Result);
                }

                else
                {
                    _notyf.Information("No notification found");
                    return View();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
                return StatusCode(500, "An Error Occured");
            }
        }

        public IActionResult ActiveEnquires()
        {
            try
            {
                int unresolvedInquiryCount = _unitOfWork.AdminRepository.ActiveEnquires();

                if (unresolvedInquiryCount > 0)
                {
                    _notyf.Information("You have new Enquires");
                }

                return Json(new { success = true, message = "Enquiry data Received", unresolvedInquiries = unresolvedInquiryCount });
            }

            catch (Exception ex)
            {
                return StatusCode(500, "An error occured" + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AddCustomer(Customer obj)
        {
            try
            {
                var status = _unitOfWork.AdminRepository.AddCustomer(obj);
                _unitOfWork.SaveChanges();
                _notyf.Success("Customer added successfully");
                return RedirectToAction("Index");
            }

            catch (Exception ex)
            {
                
                return StatusCode(500, "An error occured" + ex.Message);
            }
        }

        public IActionResult DeleteCustomer(int id)
        {
            try
            {
                var status = _unitOfWork.AdminRepository.DeleteCustomer(id);
                if (status)
                {
                    _notyf.Success("User Successfully Deleted");
                    _unitOfWork.SaveChanges();
                    return Json(new { success = true});
                }
                else
                {
                    _notyf.Error("Error Occured while Deleting Customer");
                    throw new Exception("Error Occured while Deleting Customer");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occured" + ex.Message);
            }
        }   

        public IActionResult DisableUser(int id)
        {
            try
            {
                var status = _unitOfWork.AdminRepository.DisableUser(id);
                if (status)
                {
                    _notyf.Success("User Status Updated");
                    _unitOfWork.SaveChanges();                    
                    return Json(new { success = true});
                }
                else
                {
                    throw new Exception("Error Occured while updating User status");
                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, "An error occured" + ex.Message);
            }
        }

        public IActionResult UpdateRound(int Id)
        {
            try
            {
                var Roundstatus = _unitOfWork.AdminRepository.UpdateRound(Id);
                if (Roundstatus)
                {
                    _unitOfWork.SaveChanges();
                    _notyf.Success("Next Round Started Successfully");
                    return Json(new { success = true });
                }
                else
                {
                    _notyf.Warning("Rounds can't be Updated,Please Consider Upgrading Plan");
                    return Json(new { success = false });

                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occured" + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AddDomain(Domain domain)
        {
            try
            {
                var status = _unitOfWork.AdminRepository.AddDomain(domain);

                if (status)
                {
                return Json(new { success = true, message = "message received"});
                }

                return Json(new { success = false});
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured" + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdateCustomer(CustomerDetailsVM obj)
        {
            try
            {
                var status = _unitOfWork.AdminRepository.UpdateCustomer(obj);
                if (status)
                {
                    _unitOfWork.SaveChanges();
                    _notyf.Success("User Updated Successfully");
                    int Cus_Id = Convert.ToInt32(HttpContext.Session.GetString("CurrentUser"));
                    return RedirectToAction("ShowCustomerData", "Customer", new { Id = Cus_Id });
                }
                else
                {
                    _notyf.Error("Error Occured while Updating Customer");
                    throw new Exception("Error Occured while Updating Customer");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured" + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult DeleteDomain(int customerId, string ip)
        {
            try
            {
                var status = _unitOfWork.AdminRepository.DeleteDomain(customerId, ip);

                if (status)
                {
                return Json(new { success = true, message = "message received" });
                }
                else
                {
                    return Json(new { success = false, message = "Some Error Occured" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured" + ex.Message);
            }
        }

        public IActionResult DomainCount(int id)
        {
            try
            {
                int DomainCount = _unitOfWork.AdminRepository.DomainCount(id);

                if(DomainCount <= 0)
                {
                    return NotFound();
                }

                else
                {
                return Json(new { success = true, Count= DomainCount });
                }

            }

            catch (Exception ex)
            {
                return StatusCode(500, "An error occured" + ex.Message);
            }
        }
    }
}

