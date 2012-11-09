using System;
using System.Web.Mvc;

namespace COAT.Util.Extension
{
    public static class UrlExtension
    {
        public static string AbsoluteAction(this UrlHelper url, string action, string controller, object routeValues)
        {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;
            if (requestUrl != null)
            {
                string absoluteAction = string.Format("{0}{1}",
                                                      requestUrl.GetLeftPart(UriPartial.Authority),
                                                      url.Action(action, controller, routeValues));
                return absoluteAction;
            }
            return null;
        }
    }
}