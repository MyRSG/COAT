using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace COAT.Extension
{
    public static class UrlExtension
    {
        public static string AbsoluteAction(this UrlHelper url, string action, string controller, object routeValues)
        {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;
            string absoluteAction = string.Format("{0}{1}",
                                                                    requestUrl.GetLeftPart(UriPartial.Authority),
                                                                     url.Action(action, controller, routeValues));
            return absoluteAction;
        }
    }
}
