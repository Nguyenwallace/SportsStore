using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "test-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }
        /*
          The ViewContext object is the object that provides access to things like the HttpContext, HttpRequest, HttpResponse and so on. 
           The way that you can gain access to it in a TagHelper is via a property but in such a case you need to set the [ViewContext] attribute 
            so that the property gets set to the current ViewContext. 
            So for example you can gain access to the current request via the following:
             var currentRequest = ViewContext.HttpContext.Request;
            [HtmlAttributeNotBound] basically says that this attribute isn't one that you intend to set via a tag helper attribute in the html. 
            Your tag helper may not need access to the ViewContext object and all it's subobjects. If not, you can omit the ViewContext property 
            
            and associated attributes from your TagHelper. It's certainly not a required property for a TagHelper and most of my own tag helpers so far have not needed access to it.
         */
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PagingInfo PageModel { get; set; }
        public int TestModel { get; set; }
        public string PageAction { get; set; }
        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }
        public override void Process(TagHelperContext context,
        TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new TagBuilder("div");
            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                //public virtual string Action (string actionName, object routeValues);
                //actionName
                //String
                //The name of the action method.
                //routeValues
                //Object
                //An object that contains the parameters for a route. The parameters are retrieved 
                //through reflection by examining the properties of the object.The object is typically 
                //created by using object initializer syntax.
               tag.Attributes["href"] = urlHelper.Action(PageAction,
                new { productPage = i });
                if (PageClassesEnabled)
                {
                    tag.AddCssClass(PageClass);
                    tag.AddCssClass(i == PageModel.CurrentPage
                    ? PageClassSelected : PageClassNormal);
                }
                tag.InnerHtml.Append(i.ToString());
                result.InnerHtml.AppendHtml(tag);
            }
            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
