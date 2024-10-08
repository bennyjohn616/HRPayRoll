using PayrollBO.SuperAdmin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers
{
    public class NewCompCreationController : BaseController
    {
        // GET: NewCompCreation
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult CheckExistDB(string dbName,string Username,string password)
        {
            bool isExistDB = false;
            bool returnVal = false;
            try
            {             
               
                SACompCreation sacomp = new SACompCreation();
                isExistDB = sacomp.CheckExistDB(dbName);
                if (!isExistDB)
                {
                    sacomp.CreateNewDB(dbName);
                    sacomp.ExcuteNewDBScript(dbName);
                }
                else
                {
                    return base.BuildJson(false, 100, "DB already created", returnVal);
                }

                return base.BuildJson(true, 100, "Sucsess",returnVal);
            }
            catch (Exception)
            {

                return base.BuildJson(false, 100, "Failed", returnVal);
            }
        }


    }
}