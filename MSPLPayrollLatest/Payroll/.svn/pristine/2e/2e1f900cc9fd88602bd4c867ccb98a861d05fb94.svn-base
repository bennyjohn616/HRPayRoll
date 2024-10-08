using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TraceError;

namespace Payroll
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //find the default JsonVAlueProviderFactory
            JsonValueProviderFactory jsonValueProviderFactory = null;

            foreach (var factory in ValueProviderFactories.Factories)
            {
                if (factory is JsonValueProviderFactory)
                {
                    jsonValueProviderFactory = factory as JsonValueProviderFactory;
                }
            }

            //remove the default JsonVAlueProviderFactory
            if (jsonValueProviderFactory != null) ValueProviderFactories.Factories.Remove(jsonValueProviderFactory);

            //add the custom one
            ValueProviderFactories.Factories.Add(new CustomJsonValueProviderFactory());
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            newCulture.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = newCulture;
        }
        void Application_Error(object sender, EventArgs e)
        {

            HttpContext ctxt = HttpContext.Current;

            Exception ex = ctxt.Server.GetLastError();

            HttpException httpException = ex as HttpException;
            if (ex.InnerException != null)
            {
                ex = ex.InnerException;

                Application[ErrorLog.EXCEPTION_URL] = ctxt.Request.Url.ToString();
                Application[ErrorLog.EXCEPTION_MESSAGE] = ex.Message;
                Application[ErrorLog.EXCEPTION_STACK_TRACE] = ex.StackTrace;
            }
            ErrorLog.Log(ex);
        }
        void session_start()
        {
            Session["userProfileImage"] = string.Empty;
        }
        void Application_End(object sender, EventArgs e)
        {
           
        }
    }
}
