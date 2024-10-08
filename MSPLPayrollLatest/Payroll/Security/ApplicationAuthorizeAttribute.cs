using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Payroll
{
    //public class ApplicationAuthorizeAttribute : AuthorizeAttribute
    //{
    //    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    //    {
    //        var httpContext = filterContext.HttpContext;
    //        var request = httpContext.Request;
    //        var response = httpContext.Response;
    //        var user = httpContext.User;
    //        if (request.IsAjaxRequest())
    //        {
    //            if (user.Identity.IsAuthenticated == false)
    //                response.StatusCode = (int)HttpStatusCode.Unauthorized;
    //            else
    //                response.StatusCode = (int)HttpStatusCode.Forbidden;

    //            response.SuppressFormsAuthenticationRedirect = true;
    //            response.End();
    //        }
    //        base.HandleUnauthorizedRequest(filterContext);
    //    }

    //}

    public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                // Don't check for authorization as AllowAnonymous filter is applied to the action or controller
                return;
            }

            // Check for authorization
            if (HttpContext.Current.Session["UserId"] == null)
            {
                filterContext.Result = filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}