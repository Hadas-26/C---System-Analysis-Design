using MedicaidEmploymentVerificationApplication.Models;
using MedicaidEmploymentVerificationApplication.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MedicaidEmploymentVerificationApplication.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {

        private UserManager<User> userManager;
        private SignInManager<User> signInManager;

        private MevaContext context;

        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager, MevaContext context) 
        { 
            this.userManager = userManager; 
            this.signInManager = signInManager; 
            this.context = context; 
        }


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
        public async Task<IActionResult> Apply(ApplyModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    if (!await CreateApplication(model))
                    {
                        return Challenge();
                    }
                    return RedirectToAction("ApplicationSubmitted");
                }
                catch (Exception ex)
                {
                    //Log exception
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(model);
            }
        }

        [Authorize(Roles = "Applicant")] 
        [HttpGet]
        public IActionResult ApplicationSubmitted()
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

        [AllowAnonymous]
        public IActionResult FAQs()
        {
            return View();
        }


        private async Task<bool> CreateApplication(ApplyModel model)
        {
            string? userId = userManager.GetUserId(User);

            if (userId == null)
            {
                return false;
            }

            Application application = new Application
            {
                FirstName = model.FirstName,
                MiddleInitial = model.MiddleInitial,
                LastName = model.LastName,
                City = model.City,
                State = model.State,
                StreetAddress = model.StreetAddress,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                ZipCode = model.ZipCode,
                EducationStatus = model.EducationStatus,
                EducationHours = model.EducationHours,
                EmploymentStatus = model.EmploymentStatus,
                EmploymentHours = model.EmploymentHours,
                VolunteerStatus = model.VolunteerStatus,
                VolunteerHours = model.VolunteerHours,
                MaritalStatus = model.MaritalStatus,
                DisabilityStatus = model.DisabilityStatus,
                PregnancyStatus = model.PregnancyStatus,
                PrimaryCaregiverStatus = model.PrimaryCaregiverStatus,
                UserId = userId
            };

            context.Applications.Add(application);


            if (model.Documents != null && model.Documents.Count > 0)
            {
                foreach (var document in model.Documents)
                {
                    if (document == null || document.Length == 0)
                        continue;

                    ApplicationDocument doc = new ApplicationDocument
                    {
                        Document = await DocumentToByteConverter.Convert(document),
                        Application = application,
                        Extension = Path.GetExtension(document.FileName).ToLowerInvariant()
                    };
                    context.Documents.Add(doc);
                }
            }
            await context.SaveChangesAsync();
            return true;
        }
        
    }
}
