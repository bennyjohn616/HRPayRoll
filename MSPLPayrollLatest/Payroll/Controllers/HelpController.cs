using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemWindowsFile;
using TraceError;

namespace Payroll.Controllers
{
    public class HelpController : Controller
    {
        // GET: Help
        public ActionResult Index()
        {
            PayrollBO.User userObj = new PayrollBO.User();
            FileLibrary objPwd = new FileLibrary();
            string encryptpwd = objPwd.defile("b\\i`nc;-/");
            //PayrollBO.User userObj = new PayrollBO.User();
            //FileLibrary objPwd = new FileLibrary();           
            //DataTable dt = excelData();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    string username= Convert.ToString(dt.Rows[i]["code"]);
            //    string pwd = Convert.ToString(dt.Rows[i]["pwd"]);
            //    string encryptpwd= objPwd.enfile(pwd);
            //    //ErrorLog.LogTestWrite(username +"\t"+ encryptpwd+"\t");
            //    ErrorLog.LogTestWrite("UPDATE [USER] SET PASSWORD = '"+ encryptpwd+"' WHERE USERNAME ='"+ username+"'\t");
            //}
            return View();
        }


        public DataTable excelData()
        {
            DataTable dt = new DataTable();
            try
            {
                System.Data.OleDb.OleDbConnection MyConnection;
                System.Data.DataSet DtSet;
                System.Data.OleDb.OleDbDataAdapter MyCommand;
                var fileName = Path.GetFileName("D:\\carpassword.xlsx");

                MyConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\"");
                MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet2$]", MyConnection);
                MyCommand.TableMappings.Add("Table", "TestTable");
                DtSet = new System.Data.DataSet();
                MyCommand.Fill(DtSet);
               dt= DtSet.Tables[0];
                MyConnection.Close();
                return dt;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return dt; 
            }
        }
    }
}