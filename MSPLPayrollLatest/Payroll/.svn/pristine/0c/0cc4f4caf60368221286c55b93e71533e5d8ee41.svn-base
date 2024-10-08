using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Payroll.CustomFilter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using TraceError;

namespace Payroll.Controllers
{
    [SessionExpireAttribute]
    public class DownLoadController : BaseController
    {
        // GET: DownLoad
        public ActionResult Index()
        {
            if (object.ReferenceEquals(Session["UserId"], null))
                return RedirectToAction("Index", "Login");
            if (object.ReferenceEquals(Session["CompanyId"], null))
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost, FileDownload]
        public FilePathResult DownloadEmpJoinDoc(jsonEmployeeJoiningDoc inout)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            if (inout.employeeId != Guid.Empty && !string.IsNullOrEmpty(inout.filePath))
            {
                string fileName = System.IO.Path.GetFileName(inout.filePath);
                string ext = System.IO.Path.GetExtension(inout.filePath);
                string tempPath = @"CompanyData\" + companyId + @"\Employee\" + inout.employeeId + @"\JoiningDocument\" + inout.id + @"\" + fileName;
                string path = Server.MapPath("~/" + tempPath);
                if (System.IO.File.Exists(path))
                {
                    return File(path, "application/" + ext, string.Format("{0}", fileName));
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;

            }
        }

        public JsonResult DownloadProof(string data)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            string orgfile = "";
            string fileName = "";
            string[] extarray = data.Split('.');
            string ext = extarray[extarray.Length - 1];
            try
            {
                IAmazonS3 client = new AmazonS3Client(RegionEndpoint.APSouth1);
                string[] keysplit = data.Split('/');
                orgfile = keysplit[keysplit.Length - 1];
                string[] fileext1 = orgfile.Split('.');
                string fileext = fileext1[fileext1.Length - 1];
                string bucketName = keysplit[0];
                for (int i = 1; i < keysplit.Length; i++)
                {
                    if (fileName == "")
                    {
                        fileName = keysplit[i];
                    }
                    else
                    {
                        fileName = fileName + @"/" + keysplit[i];
                    }
                }
                string file1 = "proof." + fileext;
                string dest = DocumentProcessingSettings.TempDirectoryPath + "/" + file1;
                string path = dest;


                GetObjectRequest request =
                new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName
                };

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                using (GetObjectResponse response = client.GetObject(request))
                    response.WriteResponseStreamToFile(path, false);

                if (System.IO.File.Exists(path))
                {
                    return BuildJsonResult(true, 1, "File Downloaded Successfully", new { filePath = path });
                }
               else
                {
                    return null;
                }
            }
            catch (AmazonS3Exception aex)
            {
                ErrorLog.Log(aex);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
            }
                return null;
        }

        [HttpPost, FileDownload, DeleteFileAttribute]
        public FilePathResult DownloadPaySlip(jsonEmployeeJoiningDoc inout)
        {
            try
            {
                //int companyId = Convert.ToInt32(Session["CompanyId"]);
                if (!string.IsNullOrEmpty(inout.filePath))
                {
                    string fileName = System.IO.Path.GetFileName(inout.filePath);
                    string ext = System.IO.Path.GetExtension(inout.filePath);

                    if (System.IO.File.Exists(inout.filePath))
                    {
                        return File(inout.filePath, "application/" + ext, string.Format("{0}", fileName));
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    return null;

                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);

            }
            finally
            {
                //string filename = System.IO.Path.GetFileName(inout.filePath);
                //System.IO.File.Delete(inout.filePath);
                //string deldir = inout.filePath.Remove(inout.filePath.Length - (filename.Length + 1));
                //var dir = new DirectoryInfo(deldir);
                //dir.Delete();
            }
            return null;
        }
        //
        [HttpGet, FileDownload]
        public FilePathResult DownloadReport(int id)
        {
            return GetReport(id);
        }

        [HttpPost, FileDownload]
        public FilePathResult DownloadReportPost(int foo)
        {
            return GetReport(foo);
        }
        private FilePathResult GetReport(int id)
        {
            //simulate generating the report
            Thread.Sleep(3000);

            //only even file ids will work
            string path = Server.MapPath("~/Report.pdf");
            // if (id % 2 == 0)
            //the required cookie for jquery.fileDownload is written by the FileDownloadAttribute for all
            //result types that inherit from FileResult but could be done manually here if desired
            return File(path, "application/pdf", string.Format("Report{0}.pdf", id));


            // throw new Exception(string.Format("File Report{0}.pdf could not be found. \r\n\r\n NOTE: This is for demonstration purposes only, customErrors should always be enabled in a production environment.", id));
        }

        public ActionResult DownloadFile(string path)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            if (System.IO.File.Exists(path))
            {
                string filename = System.IO.Path.GetFileName(path);
                string filepath = path; //AppDomain.CurrentDomain.BaseDirectory + "/Path/To/File/" + filename;
                byte[] filedata = System.IO.File.ReadAllBytes(filepath);
                string contentType = MimeMapping.GetMimeMapping(filepath);

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = true,
                };

                Response.AppendHeader("Content-Disposition", cd.ToString());

                return File(filedata, contentType);
            }
            else
            {
                return base.BuildJson(false, 100, "File is not available.", null);
            }

        }
            public HttpResponseBase GetResponse(string filePath, string contentype, string fileName)
        {
            using (FileStream sourceFile = new FileStream(filePath, FileMode.Open))
            {
                float FileSize;
                FileSize = sourceFile.Length;
                byte[] getContent = new byte[(int)FileSize];

                sourceFile.Close();
                sourceFile.Dispose();
                String FilePath = filePath;

                Response.ClearContent();
                Response.ClearHeaders();
                Response.Clear();
                Response.ContentType = contentype;
                Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                Response.AppendHeader("Content-Length", getContent.Length.ToString());
                Response.TransmitFile(FilePath);
                Response.Flush();
                return Response;
            }
        }
        [HttpPost, FileDownload]
        public FilePathResult DownLevOpenTemp(jsonImportTemplate path)
        {

            string fileName = System.IO.Path.GetFileName(path.filePath);
            string ext = System.IO.Path.GetExtension(path.filePath);
            //string tempPath = @"CompanyData\" + companyId + @"\Employee\" + inout.employeeId + @"\JoiningDocument\" + inout.id + @"\" + fileName;
            //string paths = Server.MapPath("~/" + path);
            if (System.IO.File.Exists(path.filePath))
            {
                return File(path.filePath, "application/" + ext, string.Format("{0}", fileName));
            }
            else
            {
                return null;
            }



        }
    }
}