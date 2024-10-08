using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraceError;
using System.IO;
namespace Payroll
{
    public class CustomExceptionFilter : FilterAttribute, IExceptionFilter
    {

        void IExceptionFilter.OnException(ExceptionContext filterContext)
        {
            //_Error.cshtml
            string browser = filterContext.RequestContext.HttpContext.Request.Browser.Browser;
            var actioname = filterContext.RouteData.Values["action"].ToString();
            var controllername = filterContext.RouteData.Values["controller"].ToString();
            filterContext.ExceptionHandled = true;
            var model = new HandleErrorInfo(filterContext.Exception, actioname, controllername);
            filterContext.Result = new ViewResult()
            {
                ViewName = "~/Views/Shared/Error.cshtml",
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model)
            };
            //Need to store the exception in Error info table

            ErrorInfo erorInfo = new ErrorInfo();
            erorInfo.MethodName = actioname;
            erorInfo.BrowserInfo = browser;
            erorInfo.CompanyId = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);
            erorInfo.ControllerName = controllername;
            erorInfo.ErrorMessage = filterContext.Exception.Message;
            erorInfo.OtherInfo = filterContext.Exception.StackTrace;
            erorInfo.UserHost = filterContext.RequestContext.HttpContext.Request.UserHostAddress;//ipaddres of user
            erorInfo.UserId= Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            erorInfo.Save();


        }

    }
    public class DeleteFileAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Flush();
            var filePathResult = filterContext.Result as FilePathResult;
            if (filePathResult != null)
            {
                System.IO.File.Delete(filePathResult.FileName);
                string filename = System.IO.Path.GetFileName(filePathResult.FileName);
                System.IO.File.Delete(filePathResult.FileName);
                string deldir = filePathResult.FileName.Remove(filePathResult.FileName.Length - (filename.Length + 1));
                var dir = new DirectoryInfo(deldir);
                dir.Delete(true);
            }
           
        }
    }
}