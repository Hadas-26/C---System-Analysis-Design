/*
 * Written by Josh Green
 */

using MedicaidEmploymentVerificationApplication.Models;
using MedicaidEmploymentVerificationApplication.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Security.Claims;

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
            var applicationsStatuses = context.ApplicationStatuses.Include(a => a.Application).Include(a => a.Employee).ToList();
            var documents = context.Documents.Include(d => d.Application).ToList();
            DashboardViewModel model = new DashboardViewModel { ApplicationDocuments = documents, ApplicationStatuses = applicationsStatuses};
            return View(model);
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
                        return Content("There was an error creating your application.");
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

        [Authorize(Roles = "Applicant")]
        [HttpGet]
        public IActionResult CheckStatus()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Application? application = context.Applications.Where(a => a.UserId == userId).FirstOrDefault();

            var status = context.ApplicationStatuses.Include(s => s.Application).Where(a => a.Application.UserId == userId).FirstOrDefault();

            return View(status);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public IActionResult Review(int applicationStatusId)
        {
            if(applicationStatusId == 0)
            {
                return Content("The application Status with that ID could not be found");
            }

            var model = context.ApplicationStatuses.Include(s => s.Application).Include(s => s.Employee).FirstOrDefault(s => s.Id == applicationStatusId);
            if(model == null || model.Application == null)
            {
                return Content("The application could not be found");
            }

            List<ApplicationDocument> docs = context.Documents.Include(d => d.Application).Where(d => d.ApplicationId == model.ApplicationId).ToList();
            return View((model, docs));
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public IActionResult ViewDocument(int documentId)
        {
            var doc = context.Documents.Find(documentId);

            

            if(doc == null)
            {
                return Content("Could not open document.");
            }

            string? ext = doc.Extension.ToLowerInvariant() switch
            {
                ".pdf" => "application/pdf",
                ".jpeg" or ".jpg" => "image/jpeg",
                ".png" => "image/png",
                _ => null
            };

            if (ext == null)
            {
                return Content("Unsupported file format.");
            }
            return File(doc.Document, ext);
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
                Sex = model.Sex,
                City = model.City,
                State = model.State,
                StreetAddress = model.StreetAddress,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                ZipCode = model.ZipCode,
                EducationStatus = model.EducationStatus,
                EducationHours = model.EducationHours,
                EmploymentStatus = model.EmploymentStatus,
                HourlyRate = model.HourlyRate,
                Employer = model.EmployerName,
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

            ApplicationStatus status = new ApplicationStatus
            {
                Application = application,
                DateSubmitted = DateTime.Now
            };

            context.ApplicationStatuses.Add(status);

            await context.SaveChangesAsync();
            return true;
        }
        
    }
}
