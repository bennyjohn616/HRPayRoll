using PayrollBO;
using PayrollBO.Tax;
using PayrollBO.AWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Web.Script.Serialization;
using Payroll.CustomFilter;
using System.IO;
using TraceError;
using Amazon.S3;
using Amazon;
using Amazon.S3.Model;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime;

namespace Payroll.Controllers
{
    [CustomExceptionFilter]
    [SessionExpireAttribute]
    public class TaxSectionController : BaseController
    {
        // GET: TaxSection
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetTaxSection(Guid financialyearId, string parentId, string type = "")
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            TXSectionList txSectionlist = new TXSectionList(companyId, financialyearId);
            List<jsonTaxSection> jsondata = new List<jsonTaxSection>();
            switch (type)
            {
                case "SubSection":
                    if (parentId == string.Empty || (new Guid(parentId)) == Guid.Empty)
                    {
                        txSectionlist.Where(s => s.ParentId != Guid.Empty).ToList().ForEach(u => { jsondata.Add(jsonTaxSection.toJson(u, type)); });
                    }
                    else
                        txSectionlist.Where(s => s.ParentId == new Guid(parentId)).ToList().ForEach(u => { jsondata.Add(jsonTaxSection.toJson(u, type)); });

                    break;
                case "Section":

                    txSectionlist.Where(s => s.ParentId == Guid.Empty && s.SectionType != "Others").ToList().ForEach(u => { jsondata.Add(jsonTaxSection.toJson(u, type)); });
                    break;
                case "Others":
                    OtherExamptionList otherExamptionList = new OtherExamptionList(type);
                    txSectionlist.Where(s => s.SectionType == type).ToList().ForEach(u =>
                   otherExamptionList.ForEach(f =>
                   {
                       if (f.Id == u.IncomeTypeId)
                       {
                           u.IncomeTypeName = f.Name;
                           { jsondata.Add(jsonTaxSection.toJson(u, type)); }
                       }

                   }));
                    //   { jsondata.Add(jsonTaxSection.toJson(u, type)); });
                    break;
                case "Declaration":

                    TaxBehaviorList behv = new TaxBehaviorList(financialyearId, Guid.Empty, Guid.Empty); //add monthly declaration
                                                                                                         //  TXEmployeeSectionList getdata = new TXEmployeeSectionList(employeeId, effectiveDate);
                    behv.Where(f => f.InputType == 2).ToList().ForEach(f =>
                    {
                        AttributeModel attr = new AttributeModel(f.AttributemodelId, companyId);
                        jsondata.Add(new jsonTaxSection
                        {
                            id = f.AttributemodelId,
                            parentSection = attr.DisplayAs,
                            sectionType = "Declaration",
                            formulaType = 2,
                            displayAs = attr.DisplayAs,
                            name = attr.DisplayAs

                        });
                    });

                    List<TXSection> taxHeaderSection = txSectionlist.Where(u => u.ParentId != Guid.Empty).ToList();
                    taxHeaderSection.Where(s => s.SectionType != "Others").ToList().ForEach(u =>
                    {
                        // u.Name = u.Name + "[" + u.ParentSection.Name + "]";
                        jsondata.Add(jsonTaxSection.toJson(u, type));
                    });
                    break;
                default:

                    //  txSectionlist.Where(s => s.SectionType != "Others").ToList().ForEach(u => { jsondata.Add(jsonTaxSection.toJson(u, type)); });

                    List<TXSection> taxHeader = txSectionlist.Where(u => u.ParentId == Guid.Empty).ToList();
                    taxHeader.ForEach(Hearder =>
                    {
                        if (Hearder.SectionType != "Others")
                        {
                            Hearder.DisorderNo = Convert.ToDouble(Hearder.OrderNo.ToString() + ".0");
                            jsondata.Add(jsonTaxSection.toJson(Hearder, type));
                            List<TXSection> taxSubSection = txSectionlist.Where(d => d.ParentId == Hearder.Id).ToList();
                            taxSubSection.Where(s => s.SectionType != "Others").ToList().ForEach(SubSec =>
                            {
                                SubSec.DisorderNo = Convert.ToDouble(Hearder.OrderNo.ToString() + '.' + SubSec.OrderNo.ToString());
                            });
                            taxSubSection.OrderBy(s => s.DisorderNo).ToList().ForEach(SubSec =>
                              {
                                  jsondata.Add(jsonTaxSection.toJson(SubSec, type));
                              });
                        }
                    });
                    jsondata.ForEach(f =>
                    {
                        if (f.parentId == Guid.Empty)
                        {
                            f.parentSection = f.name;
                            f.name = "";
                        }
                        else
                        {
                            f.parentSection = "";
                        }
                    });


                    break;
            }
            return base.BuildJson(true, 200, "success", jsondata);

        }
        public JsonResult GetSelectiveData(string Condition, List<jsonTaxCol> dataCol, Guid financialyearId, DateTime effectiveDate)
        {
            dataCol.ForEach(col =>
            {
                var d = col;


            });
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            string[] WCondition;
            WCondition = Condition.Split('.');
            EmployeeList employeeList = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
            TXEmployeeSectionList getdata = new TXEmployeeSectionList(Guid.Empty, effectiveDate);

            Guid selectedCatagoryId = new Guid(WCondition[1]);
            var seletedEmpl = employeeList.Where(u => u.CategoryId == selectedCatagoryId && u.Status == 1).ToList();
            employeeList.Initialize();
            List<jsonEmployee> jsondata = new List<jsonEmployee>();
            seletedEmpl.ForEach(u => { jsondata.Add(jsonEmployee.tojson(u)); });


            DataTable dt = new DataTable();
            dt.Columns.Add("empid");
            dt.Columns.Add("empCode");
            dt.Columns.Add("empFName");
            dataCol.ForEach(col =>
            {
                dt.Columns.Add(col.tableValue.ToString());
            });

            seletedEmpl.ForEach(u =>
            {
                DataRow row;
                row = dt.NewRow();
                row["empid"] = u.Id;
                row["empCode"] = u.EmployeeCode;
                row["empFName"] = u.FirstName;
                for (int i = 3; i < dt.Columns.Count; i++)
                {
                    var section = getdata.Where(d => d.SectionId == new Guid(dt.Columns[i].ColumnName) && d.EmployeeId == u.Id).FirstOrDefault();
                    row[dt.Columns[i].ColumnName] = object.ReferenceEquals(section, null) ? "0" : section.DeclaredValue;
                }
                dt.Rows.Add(row);

            });
            var dataVal = ConvertDataTabletoString(dt);
            // var daa=new JavaScriptSerializer().Deserialize<object>(dataVal);
            //var jsonString = Newtonsoft.Json.JsonConvert.DeserializeObject(dataVal);

            return base.BuildJson(true, 200, "success", dataVal);
        }
        public string ConvertDataTabletoString(DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }
        /// <summary>
        /// Modified By:Sharmila
        /// Modified On:10.05.17
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="effectiveDate"></param>
        /// <returns></returns>
        /// 
        public JsonResult GetSavgProof(Guid employeeId, Guid financeyear)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            int companyId = Convert.ToInt32(Session["CompanyId"]);
            SavgProofList savgProofs = new SavgProofList(financeyear, employeeId, companyId);
            JsonSavgProofList data = new JsonSavgProofList();
            Employee emp1 = new Employee(employeeId);
            data.emp = emp1;
            savgProofs.OrderBy(s => s.SerialNo);
            data.SavgProofList = savgProofs;
            return base.BuildJson(true, 200, "success", data);
        }



        public JsonResult DeleteProof(Guid EmployeeId, Guid FinanceYear,int SerialNo,string Path)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            //string awsAccessKeyId = "AKIAUAQYHWR3HJBU6Y5Q";
            //string awsSecretAccessKey = "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";
            Amazon.Runtime.AWSCredentials credentials = new Amazon.Runtime.StoredProfileAWSCredentials("default");
            RegionEndpoint bucketRegion = RegionEndpoint.APSouth1;

            IAmazonS3 client = new AmazonS3Client(credentials,RegionEndpoint.APSouth1);
            string[] keysplit = Path.Split('/');
            string orgfile = keysplit[keysplit.Length - 1];
            string fileName = "";
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

            var deleteFileRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
            };

            SavgProof savgProof = new SavgProof();
            savgProof.companyId = companyId;
            savgProof.EmployeeId = EmployeeId;
            savgProof.financeyearId = FinanceYear;
            savgProof.SerialNo = SerialNo;
            bool status = savgProof.Delete();
            if (status == true)
            {
                try
                {
                    DeleteObjectResponse fileDeleteResponse = client.DeleteObject(deleteFileRequest);
                    if (fileDeleteResponse.DeleteMarker != "true")
                    {
                        return base.BuildJson(false, 100, "Error While Deletion", "");
                    }
                }
                catch (AmazonS3Exception aex)
                {
                    ErrorLog.Log(aex);
                    return base.BuildJson(false, 100, "Error In Deletion", "");
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                    return base.BuildJson(false, 100, "Error In Deletion", "");
                }
            }
            SavgProofList savgProofs = new SavgProofList(FinanceYear, EmployeeId, companyId);
            JsonSavgProofList data = new JsonSavgProofList();
            Employee emp1 = new Employee(EmployeeId);
            data.emp = emp1;
            data.SavgProofList = savgProofs;
            return base.BuildJson(true, 200, "success", data);
        }


        public class SavgProofinput
        {
            public string EmployeeCode { get; set; }
            public Guid EmployeeId { get; set; }
            public string Description { get; set; }
            public Guid Financeyear { get; set; }
            public int LastNo { get; set; }
            public int Finyearref { get; set; }
        }

        [HttpPost]
        public JsonResult SaveProof(SavgProofinput proofinput)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            string dbconnection = Convert.ToString(Session["DBString"]);
            string[] dbsplit = dbconnection.Split(';');
            string dbname = "";
            foreach (string s in dbsplit)
            {
                if (s.ToLower().Contains("initial catalog") == true)
                {
                    string[] split1 = s.Split('=');
                    if (split1[1] != "")
                    {
                        dbname = split1[1];
                    }
                }
            }



            if (dbname == "")
            {
                {
                    return base.BuildJson(false, 100, "Internal Error, Contact System Administrator","");
                }
            }

            if (proofinput.Finyearref == 0)
            {
                return base.BuildJson(false, 100, "Error in Financial Year.", "");
            }

            string bucketname = "savgproof";
            string filepath = proofinput.Finyearref + @"/" + dbname.ToLower() + "-mindscape";

            if (proofinput.EmployeeId == null || proofinput.EmployeeId == Guid.Empty)
            {
                return base.BuildJson(false, 100, "The Form should not be empty.", "");
            }

            foreach (string file in Request.Files)
            { 
                var fileContent = Request.Files[file];
                if (fileContent == null || fileContent.ContentLength == 0)
                {
                    return base.BuildJson(false, 100, "The Form should not be empty.", "");
                }
            }

            SavgProof datavalue = new SavgProof();

            datavalue.companyId = companyId;
            datavalue.Description = proofinput.Description;
            datavalue.EmployeeId = proofinput.EmployeeId;
            datavalue.financeyearId = proofinput.Financeyear;
            datavalue.Remarks = "";
            datavalue.SerialNo = Convert.ToInt32(proofinput.LastNo);
            datavalue.Status = "";
            datavalue.Uploaddate = DateTime.Now;

            string strExistPath = string.Empty;
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        var fileName1 = Path.GetFileName(file);
                        string[] ext = fileName1.Split('.');
                        string extension = ext[ext.Length - 1];
                        var fileName = Convert.ToString(proofinput.EmployeeCode) + Convert.ToString(datavalue.SerialNo) + "." + extension;

                        string myBucketName = bucketname;
                        string s3DirectoryName = filepath;
                        string s3FileName = fileName;
                        bool a;
                        AmazonUploader myUploader = new AmazonUploader();
                        a = myUploader.sendMyFileToS3(stream, myBucketName, s3DirectoryName, s3FileName);
                        if (a != true)
                        {
                            return base.BuildJson(false, 100, "There is some error while saving the file.", datavalue);
                        }
                        datavalue.Filename = bucketname + "/" +  filepath + "/" + fileName; 
                    }
                }
                SavgProof savg = new SavgProof();
                savg.Save(datavalue);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                string exss = ex.Message;
                // Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //  return Json("Upload failed");
                return base.BuildJson(false, 100, "There is some error while saving the file.", datavalue);
            }
            return base.BuildJson(true, 200, "Data saved successfully", datavalue);
        }

        public JsonResult GetTaxSectionId(Guid financialyearId, Guid employeeId, DateTime effectiveDate, string parentId, string type = "")
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            TXSectionList txSectionlist = new TXSectionList(companyId, financialyearId);
            List<jsonTaxSection> jsondata = new List<jsonTaxSection>();
            switch (type)
            {
                case "SubSection":
                    if (parentId == string.Empty || (new Guid(parentId)) == Guid.Empty)
                    {
                        txSectionlist.Where(s => s.ParentId != Guid.Empty).ToList().ForEach(u => { jsondata.Add(jsonTaxSection.toJson(u, type)); });
                    }
                    else
                        txSectionlist.Where(s => s.ParentId == new Guid(parentId)).ToList().ForEach(u => { jsondata.Add(jsonTaxSection.toJson(u, type)); });

                    break;
                case "Section":

                    txSectionlist.Where(s => s.ParentId == Guid.Empty && s.SectionType != "Others").ToList().ForEach(u => { jsondata.Add(jsonTaxSection.toJson(u, type)); });
                    break;
                case "Others":

                    txSectionlist.Where(s => s.SectionType == type).ToList().ForEach(u => { jsondata.Add(jsonTaxSection.toJson(u, type)); });
                    break;
                case "Declaration":

                    TaxBehaviorList behv = new TaxBehaviorList(financialyearId, Guid.Empty, Guid.Empty); //add monthly declaration
                    TXEmployeeSectionList getdata = new TXEmployeeSectionList(employeeId, effectiveDate);
                    behv.Where(f => f.InputType == 2).ToList().ForEach(f =>
                    {
                        AttributeModel attr = new AttributeModel(f.AttributemodelId, companyId);
                        if (!object.ReferenceEquals(attr, null) && attr.Id != Guid.Empty)
                        {
                            TXEmployeeSection taxSection_Declar = getdata.Where(u => u.SectionId == attr.Id).FirstOrDefault();
                            jsondata.Add(new jsonTaxSection
                            {
                                id = f.AttributemodelId,
                                parentSection = attr.DisplayAs,
                                sectionType = "Declaration",
                                formulaType = 2,
                                displayAs = attr.DisplayAs,
                                declaredValue = object.ReferenceEquals(taxSection_Declar, null) ? "" : taxSection_Declar.DeclaredValue,
                                HasPan = object.ReferenceEquals(taxSection_Declar, null) ? null : taxSection_Declar.HasPan,
                                PanNumber = object.ReferenceEquals(taxSection_Declar, null) ? "" : taxSection_Declar.PanNumber,
                                HasDeclaration = object.ReferenceEquals(taxSection_Declar, null) ? null : taxSection_Declar.HasDeclaration,
                                LandLordName = object.ReferenceEquals(taxSection_Declar, null) ? "" : taxSection_Declar.LandLordName,
                                LandLordAddress = object.ReferenceEquals(taxSection_Declar, null) ? "" : taxSection_Declar.LandLordAddress,
                                disorderNo = attr.OrderNumber
                            });
                        }
                    });
                    List<TXSection> taxHeaderSection = txSectionlist.Where(u => u.ParentId == Guid.Empty).OrderBy(f => f.OrderNo).ToList();
                    taxHeaderSection.ForEach(Hearder =>
                    {
                        if (Hearder.SectionType != "Others")
                        {
                            Hearder.DisorderNo = Convert.ToDouble(Hearder.OrderNo.ToString() + ".0");
                            //jsondata.Add(jsonTaxSection.toJson(Hearder, type));
                            List<TXSection> taxSubSection = txSectionlist.Where(d => d.ParentId == Hearder.Id).OrderBy(j => j.OrderNo).ToList();
                            if (taxSubSection.Count != 0)
                            {
                                jsondata.Add(jsonTaxSection.toJson(Hearder, type));
                                taxSubSection.Where(s => s.SectionType != "Others" && s.FormulaType == 2).OrderBy(k => k.OrderNo).ToList().ForEach(SubSec =>
                                   {
                                       var section = getdata.Where(d => SubSec.Id == d.SectionId).FirstOrDefault();
                                       if (!string.IsNullOrEmpty(Convert.ToString(section)))
                                           SubSec.DeclaredValue = object.ReferenceEquals(section.DeclaredValue, null) ? "" : section.DeclaredValue;
                                       SubSec.DisorderNo = Convert.ToDouble(Hearder.OrderNo.ToString() + '.' + SubSec.OrderNo.ToString());
                                       jsondata.Add(jsonTaxSection.toJson(SubSec, type));
                                   });
                            }
                        }
                    });
                    List<TXSection> taxHeader_Section = txSectionlist.Where(u => u.SectionType == "Others").ToList();
                    taxHeader_Section.ForEach(Hearder1 =>
                    {
                        if (Hearder1.SectionType == "Others")
                        {
                            if (taxHeader_Section.Count != 0)
                            {
                                var section = getdata.Where(d => Hearder1.Id == d.SectionId).FirstOrDefault();
                                if (!string.IsNullOrEmpty(Convert.ToString(section)))
                                    Hearder1.DeclaredValue = object.ReferenceEquals(section.DeclaredValue, null) ? "" : section.DeclaredValue;
                                jsondata.Add(jsonTaxSection.toJson(Hearder1, type = "Others"));
                            }
                            //var section = getdata.Where(d => Hearder1.Id == d.SectionId).ToList();

                            //Hearder1.DeclaredValue = object.ReferenceEquals(section., null) ? "" : section.DeclaredValue;                            
                        }

                    });
                    //txSectionlist.Where(s => s.SectionType != "Others").ToList().ForEach(u =>
                    //{
                    //    var section = getdata.Where(d => u.Id == d.SectionId).FirstOrDefault();
                    //    if (!string.IsNullOrEmpty(Convert.ToString(section)))
                    //        u.DeclaredValue = object.ReferenceEquals(section.DeclaredValue,null)? "": section.DeclaredValue;
                    //    jsondata.Add(jsonTaxSection.toJson(u, type));
                    //});
                    break;
                default:

                    txSectionlist.Where(s => s.SectionType != "Others").ToList().ForEach(u => { jsondata.Add(jsonTaxSection.toJson(u, type)); });
                    break;
            }
            List<jsonTaxSection> Finaljsondata = new List<jsonTaxSection>();
            Finaljsondata = jsondata.OrderBy(d => d.disorderNo).ToList();

            TXFinanceYear txf = new TXFinanceYear(financialyearId,companyId);
            DateTime joiningdate = new Employee(employeeId).DateOfJoining;
            AttributeModelList attrList = new AttributeModelList(companyId);
            if (joiningdate>txf.StartingDate && joiningdate<=txf.EndingDate)
            {
               
            
            }
            else
            {
                if (!object.ReferenceEquals(attrList.Where(w => w.Name.Trim().ToUpper() == "TDSPREV").FirstOrDefault(), null))
                {

                    jsondata.RemoveAll(r => r.id == attrList.Where(w => w.Name.Trim().ToUpper() == "TDSPREV").FirstOrDefault().Id);
                }
                var pre = txSectionlist.Where(w => w.Name == "Previous employer income after exemptions u/s 10 and professional tax").FirstOrDefault();

                if (!object.ReferenceEquals(pre, null))
                {
                    jsondata.RemoveAll(r => r.id == pre.Id);
                }
            }


            return base.BuildJson(true, 200, "success", jsondata);

        }
        public JsonResult TDSPreviousEmployer()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

        
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            TXFinanceYearList txf = new TXFinanceYearList(companyId);

            var fin = txf.Where(w => w.IsActive == true).FirstOrDefault();
           

            if(!object.ReferenceEquals(new AttributeModelList(companyId).Where(w => w.Name.Trim().ToUpper() == "TDSPREV").FirstOrDefault(), null))
            {
                var att = new AttributeModelList(companyId).Where(w => w.Name.Trim().ToUpper() == "TDSPREV").FirstOrDefault();
                DataTable dt = new TXFinanceYear().getTdsPreviousEmployer(att.Id, fin.StartingDate, fin.EndingDate);

                List<jsonTDSproviousemployer> list = new List<jsonTDSproviousemployer>();

                if (dt.Rows.Count > 0)
                {
                    for (int rowcount = 0; rowcount < dt.Rows.Count; rowcount++)
                    {
                        jsonTDSproviousemployer js = new jsonTDSproviousemployer();
                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[rowcount]["EmployeeCode"])))
                            js.EmployeeCode = Convert.ToString(dt.Rows[rowcount]["EmployeeCode"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[rowcount]["FirstName"])))
                            js.Name = Convert.ToString(dt.Rows[rowcount]["FirstName"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[rowcount]["DateOfJoining"])))
                            js.DateOfJoining = Convert.ToDateTime(dt.Rows[rowcount]["DateOfJoining"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[rowcount]["DeclaredValue"])))
                            js.Tax = Convert.ToString(dt.Rows[rowcount]["DeclaredValue"]);
                        list.Add(js);
                    }
                }


                return base.BuildJson(true, 200, "success", list);


            }

            return base.BuildJson(true, 200, "success", null);
        }
        public JsonResult SaveTaxSection(jsonTaxSection dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);


            TXSection tx = jsonTaxSection.convertObject(dataValue);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            tx.Id = dataValue.id == Guid.Empty ? Guid.NewGuid() : dataValue.id;
            tx.CompanyId = companyId;
            tx.CreatedBy = userId;
            tx.ModifiedBy = tx.CreatedBy;
            tx.CreatedOn = DateTime.Now;
            tx.ModifiedOn = DateTime.Now;
            tx.IsDeleted = false;
            isSaved = tx.Save();

            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }
        public JsonResult SaveSection(Guid ID, Guid financialYearID, string Name, string Displayas, decimal Limit, string Status, int OrderNO)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            TXSection tx;
            if (ID == Guid.Empty)
            {
                tx = new TXSection();
                tx.CreatedBy = userId;
                tx.CreatedOn = DateTime.Now;
            }
            else
            {
                tx = new TXSection(ID, companyId);
                tx.ModifiedBy = userId;
                tx.ModifiedOn = DateTime.Now;
            }
            tx.FinancialYearId = financialYearID;
            tx.Name = Name;
            tx.DisplayAs = Displayas;
            tx.Id = ID == Guid.Empty ? Guid.NewGuid() : ID;
            tx.Limit = Limit;
            tx.OrderNo = OrderNO;
            tx.CompanyId = companyId; 
            tx.IsDeleted = false;
            isSaved = tx.Save();

            List<jsonTaxSection> jsondata = new List<jsonTaxSection>();
            jsondata.Add(jsonTaxSection.toJson(tx, string.Empty));

            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", jsondata);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", jsondata);
            }
        }

        public JsonResult SaveSubSection(Guid ID, Guid parentId, Guid financialYearId, string name, string displayAs, Decimal limit, int exemptionType, int orderNo, string documentReq, string Eligible)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            TXSection tx;
            if (ID == Guid.Empty)
            {
                tx = new TXSection();
                tx.CreatedBy = userId;
                tx.CreatedOn = DateTime.Now;
            }
            else
            {
                tx = new TXSection(ID, companyId);
                tx.ModifiedBy = userId;
                tx.ModifiedOn = DateTime.Now;
            }
            
            string swapvalue = tx != null ? tx.Formula : "";
            string swapFormula = tx != null ? tx.Value : "";
            tx.Value = swapvalue;
            tx.Formula = swapFormula;
            tx.ParentId = parentId;
            tx.FinancialYearId = financialYearId;
            tx.Name = name;
            tx.DisplayAs = displayAs;
            tx.Limit = limit;
            tx.ExemptionType = exemptionType;
            tx.IsDocumentRequired = documentReq == "No" ? Convert.ToBoolean("False") : Convert.ToBoolean("True");
            tx.Eligible = Eligible == "No" ? Convert.ToBoolean("False") : Convert.ToBoolean("True");
            tx.Id = ID == Guid.Empty ? Guid.NewGuid() : ID;
            tx.OrderNo = orderNo;
            tx.CompanyId = companyId;
            tx.IsDeleted = false;
            isSaved = tx.Save();

            List<jsonTaxSection> jsondata = new List<jsonTaxSection>();
            jsondata.Add(jsonTaxSection.toJson(tx, string.Empty));

            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", jsondata);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", jsondata);
            }
        }

        public JsonResult SaveOtherTaxSection(Guid ID, Guid Financeyear, string Name, string Displayas, int OrderNo, decimal limit, int IncomeType, string sectionType)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            TXSection tx = new TXSection();
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            //tx.ParentId = parentId;
            tx.FinancialYearId = Financeyear;
            tx.Name = Name;
            tx.DisplayAs = Displayas;
            tx.Limit = limit;
            tx.IncomeTypeId = IncomeType;
            tx.SectionType = sectionType;
            tx.Id = ID == Guid.Empty ? Guid.NewGuid() : ID;
            tx.OrderNo = OrderNo;
            tx.CompanyId = companyId;
            tx.CreatedBy = userId;
            tx.ModifiedBy = tx.CreatedBy;
            tx.CreatedOn = DateTime.Now;
            tx.ModifiedOn = DateTime.Now;
            tx.IsDeleted = false;
            isSaved = tx.Save();
            List<jsonTaxSection> jsondata = new List<jsonTaxSection>();
            jsondata.Add(jsonTaxSection.toJson(tx, string.Empty));

            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", jsondata);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", jsondata);
            }
        }
        public JsonResult SaveIncomeMatching(jsonIncomeMatch dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);


            IncomeMatching imatch = jsonIncomeMatch.convertObject(dataValue);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            imatch.CreatedBy = userId;
            imatch.ModifiedBy = imatch.CreatedBy;
            imatch.IsDeleted = false;
            isSaved = imatch.Save();
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }
        public JsonResult SaveTaxSectionMatching(Guid Id, string projection, string formula, string value, int formulaType, string baseFormula, string baseValue, string matchingComponent)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            projection = formulaType == 7 ? "Yes" : "No";
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            TXSection taxSec = new TXSection(Id, companyId);
            List<jsonTaxSection> jsondata = new List<jsonTaxSection>();
            jsondata.Add(jsonTaxSection.toJson(taxSec, string.Empty));
            if (formulaType == 7)
            {
                IncomeMatching checkExpectionType = new IncomeMatching(taxSec.FinancialYearId, new Guid(formula));
                if (checkExpectionType!=null && checkExpectionType.TaxDeductionMode!=null && checkExpectionType.TaxDeductionMode.ToLower() == "onetime" && formulaType == 7)
                    return base.BuildJson(false, 100, "One time component not set for projection", jsondata);
            }
            taxSec.Projection = projection;
            taxSec.Formula = value;
            taxSec.Value = formula;
            taxSec.FormulaType = formulaType;
            taxSec.BaseValue = baseValue;
            taxSec.BaseFormula = baseFormula;
            taxSec.ModifiedBy = userId;
            taxSec.IsDeleted = false;
            taxSec.MatchingComponent = matchingComponent;
            isSaved = taxSec.Save();

            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", jsondata);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", jsondata);
            }
        }
        public JsonResult DeleteTaxSection(Guid sectionId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            TXSection txSection = new TXSection();
            txSection.Id = sectionId;
            txSection.CompanyId = companyId;
            txSection.ModifiedBy = userId;
            txSection.Delete();
            return base.BuildJson(true, 200, "success", null);

        }
        public JsonResult GetTaxSectionDefinition(Guid sectionId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            //  Guid financialyearId = new Guid("E876FBB5-16B0-4E0D-94D4-C8663D09AA7B");// Guid.Empty; //
            TXSectionDefinitionList txSectionlist = new TXSectionDefinitionList(companyId, sectionId);
            List<jsonTaxSectionDefinition> jsondata = new List<jsonTaxSectionDefinition>();
            txSectionlist.ForEach(u => { jsondata.Add(jsonTaxSectionDefinition.toJson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);

        }
        public JsonResult SaveTaxSectionDefinition(jsonTaxSectionDefinition dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            // Guid financialyearId = new Guid("E876FBB5-16B0-4E0D-94D4-C8663D09AA7B");
            //dataValue.sectionId = financialyearId;
            TXSectionDefinition loanentry = jsonTaxSectionDefinition.convertObject(dataValue);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            loanentry.CompanyId = companyId;
            loanentry.CreatedBy = userId;
            loanentry.ModifiedBy = loanentry.CreatedBy;
            loanentry.IsDeleted = false;
            isSaved = loanentry.Save();
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

    }

    public class jsonTaxSection
    {

        public Guid id { get; set; }

        public string employeeCode { get; set; }

        public string employeeName { get; set; }


        public Guid parentId { get; set; }


        public Guid financialYearId { get; set; }


        public string name { get; set; }

        public string displayAs { get; set; }


        public int orderNo { get; set; }


        public decimal limit { get; set; }

        public string status { get; set; }

        public string exemptionType { get; set; }

        public string grossDeductable { get; set; }

        public string documentReq { get; set; }

        public string parentSection { get; set; }

        public string approvelReq { get; set; }
        public string projection { get; set; }
        public string formula { get; set; }
        public int incomeType { get; set; }
        public string sectionType { get; set; }
        public string incomeTypeName { get; set; }
        public string value { get; set; }
        public int formulaType { get; set; }
        public string baseValue { get; set; }
        public string baseFormula { get; set; }
        public string declaredValue { get; set; }
        public bool? HasPan { get; set; }
        public string PanNumber { get; set; }
        public bool? HasDeclaration { get; set; }

        public string LandLordName { get; set; }

        public string LandLordAddress { get; set; }
        public double disorderNo { get; set; }
        public string IncomeTypeName { get; set; }
        public string MatchingComponent { get; set; }

        public string Eligible { get; set; }

        public static jsonTaxSection toJson(TXSection tx, string type)
        {
            string name = tx.Name;
            string parentName = tx.ParentSection.Name;
            if (type == "")
            {

                if (tx.ParentId == Guid.Empty)
                {
                    parentName = tx.Name;
                    name = string.Empty;
                }
                else
                {
                    name = tx.Name;
                    parentName = string.Empty;
                }
            }
            return new jsonTaxSection()
            {
                id = tx.Id,
                parentId = tx.ParentId,
                financialYearId = tx.FinancialYearId,
                name = name,
                displayAs = tx.DisplayAs,
                limit = tx.Limit,
                orderNo = tx.OrderNo,
                exemptionType = tx.ExemptionType == 1 ? "Yearly" : "Monthly",
                grossDeductable = tx.IsGrossDeductable == true ? "Yes" : "No",
                approvelReq = tx.IsApprovelRequired == true ? "Yes" : "No",
                documentReq = tx.IsDocumentRequired == true ? "Yes" : "No",
                status = tx.IsActive == true ? "Active" : "InActive",
                parentSection = parentName,//ltrans.ParentSection.Name,
                projection = tx.Projection,
                formula = tx.Formula,
                value = tx.Value,
                formulaType = tx.FormulaType,
                incomeType = tx.IncomeTypeId,
                sectionType = tx.SectionType,
                baseValue = tx.BaseValue,
                declaredValue = tx.DeclaredValue == null ? "0" : tx.DeclaredValue,
                baseFormula = tx.BaseFormula,
                incomeTypeName = tx.IncomeTypeId == 0 ? string.Empty : tx.IncomeType.Name,
                disorderNo = tx.DisorderNo,
                IncomeTypeName = tx.IncomeTypeName,
                MatchingComponent = tx.MatchingComponent,
                Eligible = tx.Eligible == true? "Yes" : "No"
            };
        }
        public static TXSection convertObject(jsonTaxSection tx)
        {
            return new TXSection()
            {
                Id = tx.id,
                ParentId = tx.parentId,
                FinancialYearId = tx.financialYearId,
                Name = tx.name,
                DisplayAs = tx.displayAs,
                Limit = tx.limit,
                OrderNo = tx.orderNo,
                ExemptionType = Convert.ToInt16(tx.exemptionType), /*== "Yearly" ? 1 : 2,*/
                IsGrossDeductable = tx.grossDeductable == "Yes" ? true : false,
                IsApprovelRequired = tx.approvelReq == "Yes" ? true : false,
                IsDocumentRequired = tx.documentReq == "Yes" ? true : false,
                IsActive = tx.status == "Yes" ? true : false,
                Projection = tx.projection,
                Formula = tx.formula,
                IncomeTypeId = tx.incomeType,
                SectionType = tx.sectionType,
                FormulaType = tx.formulaType,
                Value = tx.value,
                BaseFormula = tx.baseFormula,
                BaseValue = tx.baseValue,
                DeclaredValue = tx.declaredValue == null ? "0" : tx.declaredValue,
                IncomeTypeName = tx.IncomeTypeName,
                Eligible = tx.Eligible == "Yes" ? true : false

            };
        }

        public static jsonTaxSection toDeclareJson(TXSection tx, string type, string empCode, string empName)
        {
            string name = tx.Name;
            string parentName = tx.ParentSection.Name;
            if (type == "")
            {

                if (tx.ParentId == Guid.Empty)
                {
                    parentName = tx.Name;
                    name = string.Empty;
                }
                else
                {
                    name = tx.Name;
                    parentName = string.Empty;
                }
            }
            return new jsonTaxSection()
            {
                id = tx.Id,
                employeeCode = empCode,
                employeeName = empName,
                parentId = tx.ParentId,
                financialYearId = tx.FinancialYearId,
                name = name,
                displayAs = tx.DisplayAs,
                limit = tx.Limit,
                orderNo = tx.OrderNo,
                exemptionType = tx.ExemptionType == 1 ? "Yearly" : "Monthly",
                grossDeductable = tx.IsGrossDeductable == true ? "Yes" : "No",
                approvelReq = tx.IsApprovelRequired == true ? "Yes" : "No",
                documentReq = tx.IsDocumentRequired == true ? "Yes" : "No",
                status = tx.IsActive == true ? "Active" : "InActive",
                parentSection = parentName,//ltrans.ParentSection.Name,
                projection = tx.Projection,
                formula = tx.Formula,
                value = tx.Value,
                formulaType = tx.FormulaType,
                incomeType = tx.IncomeTypeId,
                sectionType = tx.SectionType,
                baseValue = tx.BaseValue,
                declaredValue = tx.DeclaredValue == null ? "0.00" : tx.DeclaredValue == string.Empty ? "0.00" : Math.Round(Convert.ToDecimal(tx.DeclaredValue), 2).ToString("#,##0.00"),
                baseFormula = tx.BaseFormula,
                incomeTypeName = tx.IncomeTypeId == 0 ? string.Empty : tx.IncomeType.Name,
                disorderNo = tx.DisorderNo,
                IncomeTypeName = tx.IncomeTypeName,
                Eligible = tx.Eligible == true ? "Yes" : "No"

            };
        }





    }

    public class jsonTaxSectionDefinition
    {

        public Guid id { get; set; }


        public Guid parentId { get; set; }


        public Guid sectionId { get; set; }


        public string name { get; set; }

        public string displayAs { get; set; }


        public int computeType { get; set; }

        public string controlType { get; set; }

        public string dataType { get; set; }
        public string definitionValue { get; set; }

        public string status { get; set; }

        public string required { get; set; }

        public string documentReq { get; set; }

        public string approvelReq { get; set; }

        public string Eligible { get; set; }

        public static jsonTaxSectionDefinition toJson(TXSectionDefinition ltrans)
        {
            return new jsonTaxSectionDefinition()
            {
                id = ltrans.Id,
                parentId = ltrans.ParentId,
                sectionId = ltrans.SectionId,
                name = ltrans.Name,
                displayAs = ltrans.DisplayAs,
                controlType = ltrans.ControlType,
                dataType = ltrans.DataType,
                definitionValue = ltrans.DefinitionValue,
                computeType = ltrans.ComputeType,
                required = ltrans.IsRequired == true ? "Yes" : "No",
                approvelReq = ltrans.IsApprovalRequired == true ? "Yes" : "No",
                documentReq = ltrans.IsDocumentRequired == true ? "Yes" : "No",
                status = ltrans.IsActive == true ? "Active" : "InActive",

            };
        }
        public static TXSectionDefinition convertObject(jsonTaxSectionDefinition ltrans)
        {

            return new TXSectionDefinition()
            {
                Id = ltrans.id,
                ParentId = ltrans.parentId,
                SectionId = ltrans.sectionId,
                Name = ltrans.name,
                DisplayAs = ltrans.displayAs,
                DefinitionValue = ltrans.definitionValue,
                ComputeType = ltrans.computeType,
                IsRequired = ltrans.required == "Yes" ? true : false,
                IsApprovalRequired = ltrans.approvelReq == "Yes" ? true : false,
                IsDocumentRequired = ltrans.documentReq == "Yes" ? true : false,
                IsActive = ltrans.status == "Yes" ? true : false,
                DataType = ltrans.dataType,
                ControlType = ltrans.controlType
            };
        }
    }

    public class jsonIncomeMatch
    {

        public Guid id { get; set; }

        public string name { get; set; }
        public string displayAs { get; set; }
        public Guid financeYearid { get; set; }


        public Guid attributeId { get; set; }


        public string projection { get; set; }

        public string formula { get; set; }


        public Guid examptionComponent { get; set; }

        public Guid matchingComponent { get; set; }
        public string otherComponent { get; set; }
        public string operators { get; set; }
        public string taxDeductionmode { get; set; }
        public int orderno { get; set; }
        public int grossSection { get; set; }

        public string matchingCompName { get; set; }
        public string otherCompName { get; set; }
        public string examptionCompName { get; set; }
        public string grossSectionName { get; set; }

        public static jsonIncomeMatch toJson(IncomeMatching imatch)
        {
            return new jsonIncomeMatch()
            {
                id = imatch.Id,
                financeYearid = imatch.FinancialYearId,
                attributeId = imatch.AttributemodelId,
                projection = imatch.Projection == true ? "Yes" : "No",
                formula = imatch.Formula,
                matchingComponent = imatch.MatchingComponent,
                otherComponent = imatch.OtherComponent,
                examptionComponent = imatch.ExemptionComponent,
                taxDeductionmode = imatch.TaxDeductionMode,
                operators = imatch.Operator,
                grossSection = imatch.GrossSection,
                orderno = imatch.OrderNo

            };
        }
        public static IncomeMatching convertObject(jsonIncomeMatch imatch)
        {

            return new IncomeMatching()
            {
                Id = imatch.id,
                FinancialYearId = imatch.financeYearid,
                AttributemodelId = imatch.attributeId,
                Projection = imatch.projection == "Yes" ? true : false,
                Formula = imatch.formula,
                MatchingComponent = imatch.matchingComponent,
                OtherComponent = imatch.otherComponent,
                ExemptionComponent = imatch.examptionComponent,
                TaxDeductionMode = imatch.taxDeductionmode,
                Operator = imatch.operators,
                GrossSection = imatch.grossSection,
                OrderNo = imatch.orderno
            };
        }
    }

    public class jsonTDSproviousemployer
    {
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public DateTime DateOfJoining { get; set; }

        public string Tax { get; set; }

    }
    public class jsonTaxCol
    {
        public string tableHeader { get; set; }
        public Guid tableValue { get; set; }
    }

    public class jsonSavgProof
    {
        public int companyId { get; set; }
        public Guid financeyearId { get; set; }
        public Guid EmployeeId { get; set; }
        public int SerialNo { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Filename { get; set; }

        //public string CreatedBy { get; set; }

        //public string IsDeleted { get; set; }

        public static jsonSavgProof tojson(SavgProof SavgProofs, SavgProofList SavgProofList)
        {
            jsonSavgProof retObject = new jsonSavgProof();
            retObject.Filename = SavgProofs.Filename;
            var tmp = SavgProofList.Where(u => u.Id == SavgProofs.Id).FirstOrDefault();
            if (!object.ReferenceEquals(tmp, null))
            {
                retObject.EmployeeId =  SavgProofs.EmployeeId;
                retObject.SerialNo = tmp.SerialNo;
                retObject.Filename = tmp.Filename;
                retObject.Status = "";

            }
            return retObject;
        }

        //public static EmployeeJoingDocument convertObject(jsonEmployeeJoiningDoc jsonjoinDocs)
        //{
        //    EmployeeJoingDocument retObject = new EmployeeJoingDocument();
        //    // retObject.documentName = joinDocs.Where(u => u.Id == empJoinDoc.JoiningDocumentId).FirstOrDefault().DocumentName;
        //    retObject.EmployeeId = jsonjoinDocs.employeeId;
        //    retObject.Id = jsonjoinDocs.id;
        //    retObject.JoiningDocumentId = jsonjoinDocs.joingDocumentId;
        //    retObject.FilePath = jsonjoinDocs.filePath;
        //    return retObject;

        //}



    }


}