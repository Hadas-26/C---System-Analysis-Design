using MedicaidEmploymentVerificationApplication.Models;
using MedicaidEmploymentVerificationApplication.Models.DataLayer;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MedicaidEmploymentVerificationApplication.TagHelpers
{
    [HtmlTargetElement("my-paging-link")]
    public class PagingLinkTagHelper : TagHelper
    {
        private LinkGenerator linkBuilder;
        public PagingLinkTagHelper(LinkGenerator linkGenerator) => this.linkBuilder = linkGenerator;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = null!;

        public int Number { get; set; }
        public GridData Current { get; set; } = null!;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var routes = Current.Clone();
            routes.PageNumber = Number;

            string controller = ViewContext.RouteData.Values["controller"]?.ToString() ?? "";
            string action = ViewContext.RouteData.Values["action"]?.ToString() ?? "";
            string url = linkBuilder.GetPathByAction(action, controller, routes.ToDictionary()) ?? "";

            string linkClasses = "btn btn-outline-primary";
            if (Number == Current.PageNumber)
            {
                linkClasses += " active";
            }

            output.BuildLink(url, linkClasses);
            output.Content.SetContent(Number.ToString());
        }

    }
}
