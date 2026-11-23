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
        public async Task<IActionResult> ApplyAsync()
        {
            var user = await userManager.GetUserAsync(User);

            if(user == null)
            {
                return View();
            }

            var applyModel = new ApplyModel
            {
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                EmailAddress = user.Email ?? string.Empty
            };


            return View(applyModel);
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
        [Route("[Controller]/Review/APP_{applicationStatusId}")]
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

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> DocumentDecisionAsync(DocumentDecisionModel model)
        {
            
            var document = context.Documents.FirstOrDefault(d => d.Id == model.DocumentId);

            if(document == null)
            {
                return Content("The document could not be found.");
            }

            document.Reviewed = true;
            document.Rejected = !model.Approved;
            document.EmployeeId = userManager.GetUserId(User);
            document.DateApproved = model.Approved ? DateTime.Now : null;

            await context.SaveChangesAsync();

            return RedirectToAction("Review", new { applicationStatusId = model.StatusId });
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> ApplicationDecisionAsync(int statusId, bool approved)
        {
            var status = context.ApplicationStatuses.Include(s => s.Application).FirstOrDefault(s => s.Id == statusId);
            if(status == null)
            {
                return Content("The application status could not be found.");
            }

            status.IsApproved = approved;
            status.EmployeeId = userManager.GetUserId(User);
            status.DateApproved = DateTime.Now;
            await context.SaveChangesAsync();

            return RedirectToAction("Dashboard", status.Id); //Confirmation page should be added
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

            var documents = model.Documents?.Where(d => d != null && d.Length > 0).ToList();

            if(documents != null && documents.Count > 0)
            {
                SaveDocuments(documents, application, false);
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

        private void SaveDocuments(List<IFormFile> documents, Application application, bool saveDocs = true)
        {
            foreach (var document in documents)
            {
                if (document == null || document.Length == 0)
                    continue;
                ApplicationDocument doc = new ApplicationDocument
                {
                    Document = DocumentToByteConverter.Convert(document).Result,
                    Application = application,
                    Extension = Path.GetExtension(document.FileName).ToLowerInvariant()
                };
                context.Documents.Add(doc);
            }

            if(saveDocs)
            {
                context.SaveChanges();
            }   
        }
        
    }
}
