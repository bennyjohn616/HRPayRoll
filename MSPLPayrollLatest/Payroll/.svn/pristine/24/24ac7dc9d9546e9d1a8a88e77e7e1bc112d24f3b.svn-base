using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.CustomFilter
{
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            Controller controller = filterContext.Controller as Controller;

            if (controller != null)
            {
                if (Convert.ToString(filterContext.Controller.ControllerContext.RouteData.Values["action"]) != "ApproveRejectionByMail")
                {
                    if (session != null && session["UserId"] == null)
                    {
                        filterContext.Result = new RedirectResult("~/Login/Index");

                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }

}
