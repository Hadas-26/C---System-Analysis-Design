using MedicaidEmploymentVerificationApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicaidEmploymentVerificationApplication.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {

        private UserManager<User> userManager;
        private SignInManager<User> signInManager;

        private MevaContext context;

        public EmployeeController(UserManager<User> userManager, SignInManager<User> signInManager, MevaContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var applicationsStatuses = context.ApplicationStatuses.Include(a => a.Application).Include(a => a.Employee).ToList();
            var documents = context.Documents.Include(d => d.Application).ToList();
            DashboardViewModel model = new DashboardViewModel { ApplicationDocuments = documents, ApplicationStatuses = applicationsStatuses };
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

            var model = context.ApplicationStatuses.Include(s => s.Application).Include(s => s.Employee).FirstOrDefault(s => s.Id == applicationStatusId);
            if (model == null || model.Application == null)
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

            var document = context.Documents.FirstOrDefault(d => d.Id == model.DocumentId);

            if (document == null)
            {
                return Content("The document could not be found.");
            }

            document.Reviewed = true;
            document.Rejected = !model.Approved;
            document.EmployeeId = userManager.GetUserId(User);
            document.DateApproved = model.Approved ? DateTime.Now : null;

            var status = context.ApplicationStatuses.Find(model.StatusId);

            if(status != null)
            {
                status.Seen = false;
            }

            await context.SaveChangesAsync();

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
            var status = context.ApplicationStatuses.Include(s => s.Application).FirstOrDefault(s => s.Id == statusId);


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

            var status = context.ApplicationStatuses.Find(model.StatusId);

            if (status == null)
                return Content("Error locating Application Status.");

            status.Notes = model.Notes;
            context.SaveChanges();

            await UpdateApplicationStatusAsync(model.StatusId, model.IsRejected ? false : null);

            return RedirectToAction("Dashboard", "Employee");
        }

        private async Task<bool> UpdateApplicationStatusAsync(int statusId, bool? approved)
        {
            var status = context.ApplicationStatuses.Include(s => s.Application).FirstOrDefault(s => s.Id == statusId);
            if (status == null)
            {
                return false;
            }

            status.IsApproved = approved;
            status.EmployeeId = userManager.GetUserId(User);
            status.DateApproved = DateTime.Now;
            status.Seen = false;

            if(approved != null && approved == true)
            {
                status.Notes = string.Empty;
            }
            await context.SaveChangesAsync();

            return true;
        }
    }
}
