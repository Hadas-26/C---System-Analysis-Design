using MedicaidEmploymentVerificationApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MedicaidEmploymentVerificationApplication.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        //The employee is not allowed to apply from their company account.
        //They must apply under a personal account and they must not review their own application
        [Authorize(Roles = "Applicant")] 
        [HttpGet]
        public IActionResult Apply()
        {
            return View();
        }

        [Authorize(Roles = "Applicant")] 
        [HttpPost]
        public IActionResult Apply(ApplyModel model)
        {
            if(ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        [AllowAnonymous]
        public IActionResult LearnMore()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult SeeIfIQualify()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult FAQs()
        {
            return View();
        }
        
    }
}
