/*
 * Written by Josh Green
 */

using MedicaidEmploymentVerificationApplication.Models;
using MedicaidEmploymentVerificationApplication.Models.DataLayer;
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

        private IRepository<ApplicationStatus> statusData { get; set; }
        private IRepository<Application> applicationData { get; set; }
        private IRepository<ApplicationDocument> documentData { get; set; }

        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager, IRepository<ApplicationStatus> statuses, 
            IRepository<ApplicationDocument> documents,IRepository<Application> appData) 
        { 
            this.userManager = userManager; 
            this.signInManager = signInManager;
            this.statusData = statuses;
            this.documentData = documents;
            this.applicationData = appData;
        }


        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
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
                catch
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

            var application = applicationData.Get(new QueryOptions<Application>
            {
                Where = a => a.UserId == userId
            });

            var status = statusData.Get(new QueryOptions<ApplicationStatus>
            {
                Where = s => s.Application.UserId == userId,
                Includes = "Application"
            });

            

            status?.SeenByApplicant = true;
            statusData.Save();
         
            return View(status);
        }

        [HttpGet]
        public IActionResult SubmitDocument()
        {
            string userId = userManager.GetUserId(User)!;

            if (userId == null)
                return Content("User not found.");

            var applicationId = applicationData.Get(new QueryOptions<Application>
            {
                Where = a => a.UserId == userId
            })?.Id;

            if (applicationId == null)
                return Content("Appliation not found.");

            var status = statusData.Get(new QueryOptions<ApplicationStatus>
            {
                Includes = "Application",
                Where = s => s.ApplicationId == applicationId
            })?.Id;

            if (status == null)
                return Content("Application Status not found.");


            return View(new SubmitDocumentViewModel { ApplicationId = applicationId.Value, ApplicationStatusId = status.Value});
        }

        [HttpPost]
        public IActionResult SubmitDocument(SubmitDocumentViewModel model)
        {

            var application = applicationData.Get(model.ApplicationId);

            if (application == null)
                return Content("Application not found");

            SaveDocuments(model.Documents, application, true);
            var status = statusData.Get(model.ApplicationStatusId);
            status?.SeenByEmployee = false;
            statusData.Save();

            return RedirectToAction("Index", "Home");
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

            applicationData.Insert(application);

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

            statusData.Insert(status);

            statusData.Save();
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
                documentData.Insert(doc);
            }

            if(saveDocs)
            {
                documentData.Save();
            }   
        }
        
    }
}
