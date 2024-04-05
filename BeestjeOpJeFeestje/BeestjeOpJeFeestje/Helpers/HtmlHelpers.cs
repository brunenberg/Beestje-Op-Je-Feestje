using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeestjeOpJeFeestje.Helpers {
    public static class HtmlHelpers {
        public static IHtmlContent DisabledIfLoggedIn(this IHtmlHelper htmlHelper) {
            if(htmlHelper.ViewContext.HttpContext.User.Identity.IsAuthenticated) {
                return new HtmlString("disabled");
            }
            return HtmlString.Empty;
        }
    }
}
