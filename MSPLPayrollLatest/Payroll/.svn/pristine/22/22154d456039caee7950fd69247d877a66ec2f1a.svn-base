using ICSharpCode.SharpZipLib.Zip;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using Payroll.CustomFilter;
using PayrollBO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string strSeeionId = filterContext.HttpContext.Request.RequestContext.HttpContext.Session.SessionID;
            if (!object.ReferenceEquals(Session["UserId"], null))
            {
                LoginHistory loginHistory = new LoginHistory();
                if (false)//(!loginHistory.CheckCanProceesRequest(strSeeionId, Convert.ToInt32(Session["UserId"])))
                {
                    //Session["UserId"] = null;
                    //RedirectToAction("Index", "Login");
                    //throw new Exception();
                }
            }
            else
            {
                Session["UserId"] = null;
                RedirectToAction("Index", "Login");
            }

            /*
            if(!object.ReferenceEquals(filterContext.HttpContext.Request.Headers["X-Request-Guid"],null))
            {
                string str = filterContext.HttpContext.Request.Headers["X-Request-Guid"].ToString();
            }
           
            string sd = Guid.NewGuid().ToString();
            filterContext.HttpContext.Response.AddHeader("X-Request-Guid", sd);//.Headers.Add("X-Request-Guid", sd);
            int x = 0;
            if (x == 0)
            {
                // do something
                // e.g. Set ActionParameters etc
            }
            else
            {
                // do something else
            }
            */
        }

        protected bool checkSession()
        {
            if (!object.ReferenceEquals(Session["UserId"], null))// && !object.ReferenceEquals(Session["CompanyId"], null)
            {
                return true;
            }
            else
                return false;
        }
        protected string encrypt(string str)
        {
            string _result = string.Empty;
            char[] temp = str.ToCharArray();
            foreach (var _singleChar in temp)
            {
                var i = (int)_singleChar;
                i = i - 2;
                _result += (char)i;
            }
            return _result;
        }
        protected JsonResult BuildJson(bool status, int statusCode, string message, object resultData)
        {
            return new JsonResult
            {
                MaxJsonLength = int.MaxValue,
                Data = new { Status = status, StatusCode = statusCode, Message = message, result = resultData },
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public static JsonResult BuildJsonResult(bool status, int statusCode, string message, object resultData)
        {
            return new JsonResult
            {
                Data = new { Status = status, StatusCode = statusCode, Message = message, result = resultData },
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public static string jsonSerializedDtToString(object resultdata)
        {
            var result = JsonConvert.SerializeObject(resultdata, Newtonsoft.Json.Formatting.Indented,
                          new JsonSerializerSettings
                          {
                              ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                          });

            return result;
        }
        public static void ZipPath(string targetFile, string sourceDir, string pattern, bool withSubdirs, string password)
        {
            FastZip fz = new FastZip();
            if (password != null)
                fz.Password = password;

            fz.CreateZip(targetFile, sourceDir, withSubdirs, pattern);
        }

        public static void MergePDF(List<string> pdf, string outFilePath)
        {
            Document document = null;
            PdfCopy writer = null;
            try
            {
                document = new Document();

                FileStream fs = System.IO.File.Create(outFilePath);
                writer = new PdfCopy(document, fs);

                document.Open();
                foreach (string pdf_path in pdf)
                {
                    PdfReader reader = new PdfReader(new RandomAccessFileOrArray(pdf_path), null);
                    int n = reader.NumberOfPages;
                    // add content, page-by-page
                    PdfImportedPage page;
                    for (int p = 0; p < n;)
                    {
                        ++p;
                        page = writer.GetImportedPage(reader, p);
                        writer.AddPage(page);
                    }
                    reader.Close();
                }

                writer.Flush();
                writer.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (document != null && document.IsOpen())
                {
                    document.Close();
                    writer.Close();
                    writer.Dispose();

                }
                // string[] array1 = Directory.GetFiles(tempPath);
            }
        }
        public class DocumentProcessingSettings
        {

            public static string TempDirectoryPath
            {
                get
                {
                    Guid tempFolder = Guid.NewGuid();
                    if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(@"~/tempfiles")))
                    {
                        Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(@"~/tempfiles"));
                    }
                    else
                    {
                        if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(@"~/tempfiles/") + tempFolder))
                        {
                            Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(@"~/tempfiles/") + tempFolder);
                        }
                    }
                    return System.Web.HttpContext.Current.Server.MapPath(@"~/tempfiles/") + tempFolder;
                }
            }

        }
        public class rptParameters
        {
            public rptParameters()
            {
                this.rpc = new ReportParameterCollection();
            }
            public string rptPath { get; set; }
            public object rptDataSet { get; set; }
            public string rptDataSetName { get; set; }
            public ReportParameterCollection rpc { get; set; }
            public string exportto { get; set; }
            public string filePath { get; set; }
            public static string generateprt(rptParameters rParams)
            {

                string outfilePath = rParams.filePath;
                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                ReportViewer viewer = new ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.EnableExternalImages = true;

                ReportDataSource rds = new ReportDataSource();
                viewer.ProcessingMode = ProcessingMode.Local;

                LocalReport rpt = viewer.LocalReport;
                rpt.Refresh();
                rpt.ReportPath = rParams.rptPath;

                ReportDataSource rptDs = new ReportDataSource(rParams.rptDataSetName, rParams.rptDataSet);
                rpt.DataSources.Add(rptDs);
                rpt.SetParameters(rParams.rpc);
                byte[] renderedBytes = null;

                renderedBytes = rpt.Render(rParams.exportto, null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                string contentype = mimeType;


                using (FileStream fs = System.IO.File.Create(outfilePath))
                {
                    fs.Write(renderedBytes, 0, (int)renderedBytes.Length);
                }
                return outfilePath;
            }
        }
    }
}