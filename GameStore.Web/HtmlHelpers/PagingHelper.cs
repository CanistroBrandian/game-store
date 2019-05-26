using GameStore.Web.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Text;
using System.Text.Encodings.Web;

namespace GameStore.Web.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static HtmlString PageLinks(this IHtmlHelper html,
                                              PagingInfo pagingInfo,
                                              Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml.Append(i.ToString());
                if (i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                var writer = new System.IO.StringWriter();
                tag.WriteTo(writer, HtmlEncoder.Default);
                result.Append(writer.ToString());
            }
            return new HtmlString(result.ToString());
        }
    }
}