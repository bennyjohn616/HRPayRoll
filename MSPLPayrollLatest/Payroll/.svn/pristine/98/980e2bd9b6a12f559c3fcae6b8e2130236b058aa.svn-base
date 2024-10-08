using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using PayrollBO;
using System.Data;
using System.Data.SqlClient;
using Amazon.S3;
using Amazon;
using Amazon.S3.Model;
using TraceError;
using NotificationEngine;
using Microsoft.Office.Interop.Word;


namespace Payroll.Controllers
{
    public class VerifyController : BaseController
    {
        public JsonResult GetCompanyList(Guid finyear, Guid empid)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            VerifierList Vlist = new VerifierList(finyear, empid);
            List<jsonVerifier> jsondata = new List<jsonVerifier>();
            Vlist.ForEach(u => { jsondata.Add(jsonVerifier.tojson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }

        public JsonResult GetAllCompanyList(Guid finyear)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            VerifierList Vlist = new VerifierList(finyear);
            Guid VerifierID = new Guid(Convert.ToString(Session["EmployeeId"]));
            Vlist.RemoveAll(v => v.VerifierID != VerifierID);
            List<jsonVerifier> jsondata = new List<jsonVerifier>();
            Vlist.ForEach(u => { jsondata.Add(jsonVerifier.tojson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);
        }


        public JsonResult GetVerifyEmpList(DateTime startdate, DateTime enddate, int DBConnection,Guid finyear)
        {
            User userobj = new User();
            System.Data.DataTable dtDBconn = new System.Data.DataTable();
            dtDBconn = userobj.GetUserDBconnectionValues(Convert.ToInt32(DBConnection));
            if (dtDBconn.Rows.Count > 0)
            {
                Session["VerifyDBString"] = dtDBconn.Rows[0]["DBString"].ToString();
            }
            else
            {
                return base.BuildJson(false, 100, "Error in Fetching Data", "");
            }
            VerifyFinYrList vflist = new VerifyFinYrList(startdate, enddate);
            var defaultyearlist = vflist.Where(e => e.StartingDate <= startdate && e.EndingDate >= enddate).ToList();
            var defaultyear = new VerifyFinYr();
            if (DBConnection == 2028)
            {
                defaultyear = vflist.Where(e => e.StartingDate <= startdate && e.EndingDate >= enddate && e.CompanyId == 1).FirstOrDefault();
            }
            else
            {
                defaultyear = vflist.Where(e => e.StartingDate <= startdate && e.EndingDate >= enddate).FirstOrDefault();
            }
            

            if (!object.ReferenceEquals(defaultyear, null))
            {
                VerifyEmpList verifyemplist = new VerifyEmpList(defaultyear.Id);
                Guid VerifierID = new Guid(Convert.ToString(Session["EmployeeId"]));
                VEmpList vEmpList = new VEmpList(VerifierID,finyear,DBConnection);
                VerifyEmpList newList = new VerifyEmpList();

                for (int i=0;i< vEmpList.Count;i++)
                {
                    foreach(var Id in verifyemplist)
                    {
                        if (Id.DBConnectionId == vEmpList[i].DBConnectionId)
                        {
                            if (vEmpList[i].EmployeeId == Id.Id)
                            {
                                newList.Add(Id);
                            }
                        }
                        else
                        {
                            return base.BuildJson(false, 100, "Error In Fetching Data", "");
                        }

                    }
                }

                VerifyData1 data1 = new VerifyData1();
                data1.defaultyear = defaultyear.Id;
                List<jsonVerifyEmp> jsondata = new List<jsonVerifyEmp>();
                newList.ForEach(u => { jsondata.Add(jsonVerifyEmp.tojson(u)); });
                data1.jsonVerifyEmps = jsondata;
                return base.BuildJson(true, 200, "success", data1);
            }
            else
            {
                return base.BuildJson(false, 100, "Error in Assigning Financial year", "");
            }
        }

        public JsonResult GetAllEmpList(DateTime startdate, DateTime enddate, int DBConnection, Guid finyear)
        {
            User userobj = new User();
            System.Data.DataTable dtDBconn = new System.Data.DataTable();
            dtDBconn = userobj.GetUserDBconnectionValues(Convert.ToInt32(DBConnection));
            if (dtDBconn.Rows.Count > 0)
            {
                Session["VerifyDBString"] = dtDBconn.Rows[0]["DBString"].ToString();
            }
            else
            {
                return base.BuildJson(false, 100, "Error in Fetching Data", "");
            }
            VerifyFinYrList vflist = new VerifyFinYrList(startdate, enddate);
            var defaultyearlist = vflist.Where(e => e.StartingDate <= startdate && e.EndingDate >= enddate).ToList();
            var defaultyear = new VerifyFinYr();
            if (DBConnection == 2028)
            {
                defaultyear = vflist.Where(e => e.StartingDate <= startdate && e.EndingDate >= enddate && e.CompanyId == 1).FirstOrDefault();
            }
            else
            {
                defaultyear = vflist.Where(e => e.StartingDate <= startdate && e.EndingDate >= enddate).FirstOrDefault();
            }


            if (!object.ReferenceEquals(defaultyear, null))
            {
                VerifyEmpList verifyemplist = new VerifyEmpList(defaultyear.Id);
                Guid VerifierID = new Guid(Convert.ToString(Session["EmployeeId"]));
                VEmpList vEmpList = new VEmpList(finyear, DBConnection);
                VerifyEmpList newList = new VerifyEmpList();

                for (int i = 0; i < vEmpList.Count; i++)
                {
                    foreach (var Id in verifyemplist)
                    {
                        if (Id.DBConnectionId == vEmpList[i].DBConnectionId)
                        {
                            if (vEmpList[i].EmployeeId == Id.Id)
                            {
                                newList.Add(Id);
                            }
                        }
                        else
                        {
                            return base.BuildJson(false, 100, "Error In Fetching Data", "");
                        }

                    }
                }

                VerifyData1 data1 = new VerifyData1();
                data1.defaultyear = defaultyear.Id;
                List<jsonVerifyEmp> jsondata = new List<jsonVerifyEmp>();
                newList.ForEach(u => { jsondata.Add(jsonVerifyEmp.tojson(u)); });
                data1.jsonVerifyEmps = jsondata;
                return base.BuildJson(true, 200, "success", data1);
            }
            else
            {
                return base.BuildJson(false, 100, "Error in Assigning Financial year", "");
            }
        }


        public Microsoft.Office.Interop.Word.Document wordDocument { get; set; }

        public JsonResult GetVerifyProof(Guid employeeId, Guid financeyear)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            VerifyProofList VProofs = new VerifyProofList(financeyear, employeeId);
            for (int i=0;i<VProofs.Count;i++)
            {
                if (VProofs[i].Mailsent == "1")
                {
                    return base.BuildJson(false, 100, "Mail Already sent for this Employee,Cannot Alter Data","");
                }
            }
            JsonVerifyProofList data = new JsonVerifyProofList();
            VProofs.OrderBy(s => s.SerialNo);
            data.VerifyProofList = VProofs;
            return base.BuildJson(true, 200, "success", data);
        }

        public JsonResult GetCheckProof(Guid employeeId, Guid financeyear)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            VerifyProofList VProofs = new VerifyProofList(financeyear, employeeId);
            JsonVerifyProofList data = new JsonVerifyProofList();
            VProofs.OrderBy(s => s.SerialNo);
            data.VerifyProofList = VProofs;
            return base.BuildJson(true, 200, "success", data);
        }


        public JsonResult GetImage(string data)
        {

            if (!base.checkSession())
                return null;

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
                string file1 = orgfile;
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

                if (fileext == "doc" || fileext == "docx")
                {
                    Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
                    wordDocument = appWord.Documents.Open(path);
                    file1 = fileext1[0]  + ".pdf";
                    dest = DocumentProcessingSettings.TempDirectoryPath + "/" + file1;
                    path = dest;
                    wordDocument.ExportAsFixedFormat(path, WdExportFormat.wdExportFormatPDF);
                }

                if (System.IO.File.Exists(path))
                {
                    byte[] imageByteData = System.IO.File.ReadAllBytes(path);
                    //string base64String = Convert.ToBase64String(imageByteData);

                    return base.BuildJson(true,200,"",path);
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

        public JsonResult VerifySave(Verifyinput data)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            VerifyProof datavalue = new  VerifyProof();

            datavalue.EmployeeId = data.EmployeeId;
            datavalue.SerialNo = data.SerialNo;
            datavalue.EmployeeId = data.EmployeeId;
            datavalue.financeyearId = data.Financeyear;
            datavalue.Remarks = data.Remarks;
            if (data.Status.ToLower() == "no")
            {
                datavalue.Status = "R";
            }
            else
            {
                datavalue.Status = "A";
            }

            if (datavalue.Status == "A")
            {
                datavalue.Remarks = " ";
            }

            datavalue.VerifiedBy = data.VerifiedBy;

            bool save =  datavalue.Save(datavalue);

            if (save == false)
            {
                return base.BuildJson(false, 100, "Error in Saving Data", "");
            }
            else
            {
                return base.BuildJson(true, 200, "Data Saved Successfully", "");
            }
        }

        public JsonResult SendVerifyMail(Guid employeeId,Guid financeyear,string email,string name,string companymail)
        {
            bool status = false;
            try
            {
                VerifyProofList VProofs = new VerifyProofList(financeyear, employeeId);
                int errors = 0;
                string err = "";
                if (VProofs.Count > 0)
                {
                    for (int i = 0; i < VProofs.Count; i++)
                    {
                        if (VProofs[i].Status != "Accepted" && VProofs[i].Status != "Rejected")
                        {
                            return base.BuildJson(true, 200, "All the Documents not verified, Cannot send Mail","");
                        }

                        if (VProofs[i].Status == "Accepted" || VProofs[i].Mailsent == "1")
                            continue;
                        if (errors == 0)
                        {
                            err = "<table style=border:solid 1px;text-align:left;>" +
                                "<tr style=border:solid 1px;text-align:left;>" +
                                "<th style=border:solid 1px;text-align:left;>Serial No</th>" +
                                "<th style=border:solid 1px;text-align:left;>Description</th>" +
                                "<th style=border:solid 1px;text-align:left;>Remarks</th></tr><br/>";
                        }
                        errors = errors + 1;
                        err = err + "<tr style=border:solid 1px;text-align:left><td style=border:solid 1px;text-align:left>" +
                            VProofs[i].SerialNo + "</td><td style=border:solid 1px;text-align:left>" + 
                            VProofs[i].Description + "</td><td style=border:solid 1px;text-align:left>" + VProofs[i].Remarks + "</td></tr><br/>";
                        VerifyProof vupd = new VerifyProof();
                        vupd.EmployeeId = employeeId;
                        vupd.financeyearId = financeyear;
                        vupd.SerialNo = VProofs[i].SerialNo;
                        vupd.Mailsent = "1";
                        bool issave = vupd.SaveMailSent(vupd);
                    }

                    if (errors == 0 )
                    {
                        return base.BuildJson(true, 200, "No Rejected Records to Send Mail", "");
                    }
                    else
                    {
                        err = err + "</table>";
                    }


                    if (!string.IsNullOrEmpty(email))
                    {
                        string addmsg = "";
                        if (companymail == "enlightedtds@mindscapesolutions.co.in")
                        {
                            addmsg = " on or before 22-02-2021.";
                        }
                        string message = "<p>Hi " + name + "<br/><br/>" + "We have verified the proof submitted by you in respect of your tax related investments and we seek clarification and/or additional supporting documents in respect of the same. Your reply with supporting documents, wherever required, may be sent to " + companymail + addmsg + "</p>";
                        string message1 = err;
                        string message2 = "<br/><br/><br/><br/>Regards,<br/>HR";
                        string msg = message + message1 + message2;
                        string subject = "Errors in Proof Submission ";

                        PayRoleMail payrolemail = new PayRoleMail(email, subject, msg);
                        status = payrolemail.Send();

                    }
                }
                else
                {
                    return base.BuildJson(true, 200, "No Records, Mail Not Sent", "");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 100, "Error in Sending mail","");
            }

            return base.BuildJson(true, 200, "Mail sent Successfully", "");
        }


    }

    public class jsonVerifier
     {
        public Guid finyear { get; set; }
        public Guid VerifierID { get; set; }
        public string CompanyName { get; set; }
        public int DBConnectionId { get; set; }
        public string MailID { get; set; }

        public string FirstName { get; set; }


        public static jsonVerifier tojson(Verifier verifier)
        {
            return new jsonVerifier()
            {
                finyear = verifier.finyear,
                VerifierID = verifier.VerifierID,
                CompanyName = verifier.CompanyName,
                DBConnectionId = verifier.DBConnectionId,
                MailID = verifier.MailID,
                FirstName = verifier.FirstName
            };
        }
    }

    public class VerifyData1
    {
        public Guid defaultyear;
        public List<jsonVerifyEmp> jsonVerifyEmps;
    }

    public class Verifyinput
    {
        public Guid EmployeeId { get; set; }
        public Guid Financeyear { get; set; }
        public int SerialNo { get; set; }
        public Guid VerifiedBy { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
    }

    public class jsonVerifyEmp
    {
        public Guid Id { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }


        public static jsonVerifyEmp tojson(VerifyEmp verifyEmp)
        {
            return new jsonVerifyEmp()
            {
                Id = verifyEmp.Id,
                EmployeeCode = verifyEmp.EmployeeCode,
                FirstName = verifyEmp.FirstName,
                Email = verifyEmp.Email
            };
        }
    }


}
