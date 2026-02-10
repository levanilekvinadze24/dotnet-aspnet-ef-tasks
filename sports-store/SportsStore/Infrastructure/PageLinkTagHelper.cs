using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsStore.Models.ViewModels;
using System.Globalization;

namespace SportsStore.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            this.urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }

        public PagingInfo? PageModel { get; set; }

        public string? PageAction { get; set; }

        public bool PageClassesEnabled { get; set; }

        public string PageClass { get; set; } = string.Empty;

        public string PageClassNormal { get; set; } = string.Empty;

        public string PageClassSelected { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (this.ViewContext == null || this.PageModel == null)
            {
                return;
            }

            IUrlHelper urlHelper = this.urlHelperFactory.GetUrlHelper(this.ViewContext);
            TagBuilder result = new TagBuilder("div");

            for (int i = 1; i <= this.PageModel.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.Attributes["href"] = urlHelper.Action(
                    this.PageAction,
                    new { productPage = i }
                );

                // Use invariant culture for consistent formatting
                _ = tag.InnerHtml.Append(i.ToString(CultureInfo.InvariantCulture));

                if (this.PageClassesEnabled)
                {
                    tag.AddCssClass(this.PageClass);
                    tag.AddCssClass(i == this.PageModel.CurrentPage
                        ? this.PageClassSelected
                        : this.PageClassNormal);
                }

                _ = result.InnerHtml.AppendHtml(tag);
            }

            _ = output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
