using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;

namespace NgTrade.Helpers.Paging
{
    public class PagingHtmlBuilder
    {
        public string BuildHtmlItem(string url, string display, bool active = false, bool disabled = false)
        {
            // every item wrapped in LI tag
            TagBuilder liTag = new TagBuilder("li");
            if (disabled)
            {
                // add disabled class and use a SPAN tag inside
                liTag.MergeAttribute("class", "disabled");
                var spanTag = new TagBuilder("span") {InnerHtml = display};
                liTag.InnerHtml = spanTag.ToString();
            }
            else
            {
                if (active)
                {
                    liTag.MergeAttribute("class", "active");
                }
                // use inner A tag
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", url);
                tag.InnerHtml = display;
                liTag.InnerHtml = tag.ToString();
            }
            return liTag.ToString();
        }
    }

    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(
            this HtmlHelper html,
            PagingInfo pagingInfo,
            Func<int, string> pageUrl
        )
        {
            var pagingBuilder = new PagingHtmlBuilder();
            StringBuilder result = new StringBuilder();
            //previous link
            string prevLink = (pagingInfo.CurrentPage == 1)
                ? pagingBuilder.BuildHtmlItem(pageUrl(pagingInfo.CurrentPage - 1), "Prev", false, true)
                : pagingBuilder.BuildHtmlItem(pageUrl(pagingInfo.CurrentPage - 1), "Prev");
            result.Append(prevLink);

            // only show up to 5 links to the left of the current page
            var start = (pagingInfo.CurrentPage <= 6) ? 1 : (pagingInfo.CurrentPage - 5);
            // only show up to 5 links to the right of the current page
            var end = (pagingInfo.CurrentPage > (pagingInfo.TotalPages - 5)) ? pagingInfo.TotalPages : pagingInfo.CurrentPage + 5;

            for (int i = start; i <= end; i++)
            {
                string pageHtml = (i == pagingInfo.CurrentPage)
                                      ? pagingBuilder.BuildHtmlItem(pageUrl(i), i.ToString(CultureInfo.CreateSpecificCulture("en-US")), true)
                                      : pagingBuilder.BuildHtmlItem(pageUrl(i), i.ToString(CultureInfo.CreateSpecificCulture("en-US")));
                result.Append(pageHtml);
            }

            // next link
            string nextLink = (pagingInfo.CurrentPage == pagingInfo.TotalPages)
                ? pagingBuilder.BuildHtmlItem(pageUrl(pagingInfo.CurrentPage + 1), "Next", false, true)
                : pagingBuilder.BuildHtmlItem(pageUrl(pagingInfo.CurrentPage + 1), "Next");
            result.Append(nextLink);

            return MvcHtmlString.Create(result.ToString());
        }
    }
}