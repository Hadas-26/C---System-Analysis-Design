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
        
    }
}
