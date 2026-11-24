using MedicaidEmploymentVerificationApplication.Models;
using MedicaidEmploymentVerificationApplication.Models.DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.NativeInterop;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MedicaidEmploymentVerificationApplication.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {

        private UserManager<User> userManager;
        private SignInManager<User> signInManager;

        //private MevaContext context;

        private IRepository<ApplicationStatus> statusData { get; set; }
        private IRepository<Application> applicationData { get; set; }
        private IRepository<ApplicationDocument> documentData { get; set; }

        public EmployeeController(UserManager<User> userManager, SignInManager<User> signInManager, 
            IRepository<ApplicationStatus> status, IRepository<Application> app, IRepository<ApplicationDocument> doc)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.statusData = status;
            this.applicationData = app;
            this.documentData = doc;
        }

        [HttpGet]
        public IActionResult Dashboard(DashboardGridData gridData)
        {

            var sort = gridData.SortField;

            var query = new QueryOptions<ApplicationStatus>
            {
                Includes = "Application,Employee",
                PageNumber = gridData.PageNumber,
                PageSize = gridData.PageSize,
                OrderByDirection = gridData.SortDirection,
                OrderBy = s => s.SeenByEmployee,
                ThenBy = s => s.IsApproved,
            };

            //if (!string.IsNullOrWhiteSpace(sort) && !sort.EqualsNoCase("all"))
            //{
            //    if(sort.EqualsNoCase(GridData.GetStatus(GridData.Status.Pending)))
            //    {

            //    }
            //    else if(sort.EqualsNoCase(GridData.GetStatus(GridData.Status.Verified)))
            //    {

            //    }
            //    else if(sort.EqualsNoCase(GridData.GetStatus(GridData.Status.Declined)))
            //    {

            //    }
            //}
            //else
            //    query.OrderBy = s => s.SeenByEmployee;


            var applicationStatuses = statusData.List(query).ToList();


            var documents = documentData.List(new QueryOptions<ApplicationDocument>
            {
                Includes = "Application"
            }).ToList();

            int matchingCourseCount = query.HasWhere ? statusData.List(new QueryOptions<ApplicationStatus> { Where = query.Where }).Count() : statusData.Count;
            var model = new DashboardViewModel
            {
                ApplicationDocuments = documents,
                ApplicationStatuses = applicationStatuses,
                CurrentRoute = gridData,
                TotalPages = gridData.GetTotalPages(matchingCourseCount)
            };

            
            return View(model);
        }

        
        [HttpGet]
        [Route("[Controller]/Review/APP_{applicationStatusId}")]
        public IActionResult Review(int applicationStatusId)
        {
            if (applicationStatusId == 0)
            {
                return Content("The application Status with that ID could not be found");
            }

            var model = statusData.Get(new QueryOptions<ApplicationStatus>
            {
                Includes = "Application,Employee",
                Where = s => s.Id == applicationStatusId
            });

            
            if (model == null || model.Application == null)
            {
                return Content("The application could not be found");
            }

            List<ApplicationDocument> docs = documentData.List(new QueryOptions<ApplicationDocument>
            {
                Includes = "Application",
                Where = d => d.ApplicationId == model.ApplicationId
            }).ToList();
            
            return View((model, docs));
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public IActionResult ViewDocument(int documentId)
        {

            var doc = documentData.Get(documentId);

            if (doc == null)
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

        [HttpPost]
        public async Task<IActionResult> DocumentDecisionAsync(DocumentDecisionModel model)
        {

            var document = documentData.Get(new QueryOptions<ApplicationDocument>
            {
                Where = d => d.Id == model.DocumentId
            });

            if (document == null)
            {
                return Content("The document could not be found.");
            }

            document.Reviewed = true;
            document.Rejected = !model.Approved;
            document.EmployeeId = userManager.GetUserId(User);
            document.DateApproved = model.Approved ? DateTime.Now : null;

            var status = statusData.Get(model.StatusId);

            if(status != null)
            {
                status.SeenByApplicant = false;
            }

            statusData.Save();

            return RedirectToAction("Review", "Employee", new { applicationStatusId = model.StatusId });
        }

        
        [HttpPost]
        public async Task<IActionResult> ApplicationDecisionAsync(int statusId, bool approved)
        {
            var success = await UpdateApplicationStatusAsync(statusId, approved);

            
            if (!success)
            {
                return Content("The application status could not be found.");
            }

            var status = statusData.Get(new QueryOptions<ApplicationStatus>
            {
                Includes = "Application",
                Where = s => s.Id == statusId

            });

            return RedirectToAction("Dashboard", status?.Id);
        }

        [HttpGet]
        public IActionResult Notes(int statusId, bool rejected)
        {
            

            var notesModel = new NotesViewModel
            {
                StatusId = statusId,
                IsRejected = rejected
            };

            return View(notesModel);
        }

        [HttpPost]
        public async Task<IActionResult> Notes(NotesViewModel model)
        {

            var status = statusData.Get(model.StatusId);

            if (status == null)
                return Content("Error locating Application Status.");

            status.Notes = model.Notes;
            statusData.Save();

            await UpdateApplicationStatusAsync(model.StatusId, model.IsRejected ? false : null);

            return RedirectToAction("Dashboard", "Employee");
        }

        private async Task<bool> UpdateApplicationStatusAsync(int statusId, bool? approved)
        {
            var status = statusData.Get(new QueryOptions<ApplicationStatus>
            {
                Includes = "Application",
                Where = s => s.Id == statusId
            });

            
            if (status == null)
            {
                return false;
            }

            status.IsApproved = approved;
            status.EmployeeId = userManager.GetUserId(User);
            status.DateApproved = DateTime.Now;
            status.SeenByApplicant = false;
            status.SeenByEmployee = true;

            if(approved != null && approved == true)
            {
                status.Notes = string.Empty;
            }
            statusData.Save();

            return true;
        }
    }
}
