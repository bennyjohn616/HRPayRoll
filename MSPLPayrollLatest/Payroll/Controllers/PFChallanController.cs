using PayrollBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayRollReports;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Data;
using Payroll.CustomFilter;
using System.Globalization;
using TraceError;

namespace Payroll.Controllers
{
    [SessionExpireAttribute]
    public class PFChallanController : BaseController
    {
        // GET: PFChallan
        public ActionResult TemplateEdit()
        {
            return View();
        }
        public JsonResult SavePFChallanTemplate(jsonPFChallanTemplate dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);


            if (dataValue.id == 0)
            {
                isSaved = false;

            }
            PayrollBO.PFChallan pfchallan = jsonPFChallanTemplate.convertobject(dataValue);
            pfchallan.CompanyId = companyId;
            pfchallan.CreatedBy = userId;
            pfchallan.ModifiedBy = userId;
            pfchallan.IsActive = false;
            pfchallan.Id = dataValue.id;
            isSaved = pfchallan.Save();

            if (isSaved)
            {
                dataValue.id = pfchallan.Id;
                return base.BuildJson(true, 200, "Data saved successfully", jsonPFChallanTemplate.tojson(pfchallan));
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult GetColumns(string tableName)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            if (tableName == "SalaryBase" || tableName == "Salary")
            {
                tableName = "Salary";
            }

            PayRollReports.PFChallan pfchallan = new PayRollReports.PFChallan();
            PFChallanList columnlist = pfchallan.GetColumns(tableName, companyId);
            List<jsonPFChallanTemplate> jsondata = new List<jsonPFChallanTemplate>();
            columnlist.ForEach(u => { jsondata.Add(jsonPFChallanTemplate.tojson(u)); });

            return base.BuildJson(true, 200, "success", jsondata);
        }
        public JsonResult GetPFChallanTemplate(int id)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            PFChallanList pfchallanList = new PFChallanList(companyId, id);
            List<jsonPFChallanTemplate> jsondata = new List<jsonPFChallanTemplate>();
            pfchallanList.ForEach(u => { jsondata.Add(jsonPFChallanTemplate.tojson(u)); });

            return base.BuildJson(true, 200, "success", jsondata);
        }

        public void DeleteRow(int id)
        {
            PayrollBO.PFChallan challan = new PayrollBO.PFChallan();
            challan.Id = id;
            challan.Delete();

        }

        public JsonResult GetPFChallan(int month, int year)
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int userId = Convert.ToInt32(Session["UserId"]);
                // string outFilePath = DocumentProcessingSettings.TempDirectoryPath + ".txt";
                string outFilePath = string.Empty;
                string reportPath = DocumentProcessingSettings.TempDirectoryPath + "/";

                outFilePath = reportPath + companyId + "-" + (MonthEnum)month + "-" + year + ".txt";
                PayRollReports.PFChallan ps = new PayRollReports.PFChallan();
                string response = ps.pfchallanTxt(month, year, companyId, outFilePath, userId);

                return base.BuildJson(true, 200, "success", new { filepath = outFilePath, response= response });
            }
            catch (Exception ex)
            {
                TraceError.ErrorLog.Log(ex);
                return base.BuildJson(false, 400, "Failure", "There is some error while fetching Data");
            }
        }

        public JsonResult GetPfChallanXlsFormat(int month, int year)
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int userId = Convert.ToInt32(Session["UserId"]);
                string pdfPath = string.Empty;
                string reportPath = DocumentProcessingSettings.TempDirectoryPath + "/";

                pdfPath = reportPath + companyId + "-" + (MonthEnum)month + "-" + year + ".xls";
                // DataTable dt = new DataTable();
                PayRollReports.PFChallan ps = new PayRollReports.PFChallan();
                List<PayRollReports.PFChallan> pfchallanList = ps.pfchalanList(month, year, companyId, reportPath, userId);

                if (generatPfChallanSheet(pdfPath, pfchallanList))
                    return base.BuildJson(true, 200, "success", new { filepath = pdfPath });
                else
                    return base.BuildJson(false, 200, "Failed", new { filepath = pdfPath });
            }
            catch (Exception ex)
            {
                TraceError.ErrorLog.Log(ex);
                return base.BuildJson(false, 400, "Failure", "There is some error while fetching Data");
            }
        }

        public JsonResult GetESIDataXlsFormat(int month, int year)
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int userId = Convert.ToInt32(Session["UserId"]);
                string pdfPath = string.Empty;
                string reportPath = DocumentProcessingSettings.TempDirectoryPath + "/";

                pdfPath = reportPath + companyId + "-" + (MonthEnum)month + "-" + year + ".xls";         
                PayRollReports.PFChallan ps = new PayRollReports.PFChallan();
                DataSet ds = ps.ESIchalanList(month, year, companyId, reportPath, userId);
                DataTable dt = ds.Tables[0];
                DataTable temp = ds.Tables[0];
                for (int i = 0; i < temp.Rows.Count; i++)
                {
                    EmployeeList emplis = new EmployeeList(companyId);

                    var emp = emplis.Where(w => w.EmployeeCode == Convert.ToString(temp.Rows[i]["EmployeeCode"])).FirstOrDefault();
                    if (!object.ReferenceEquals(emp, null))
                    {
                        if ((emp.SeparationDate > DateTime.MinValue && emp.SeparationDate < DateTime.Parse(DateTime.DaysInMonth(year, month) + "/" + month + "/" + year, new CultureInfo("en-GB")) && (emp.LastWorkingDate.Month != month && emp.LastWorkingDate.Year != year)))
                        {
                            dt.Rows.Remove(temp.Rows[i]);
                        }
                    }

                    
                }
                DataTable dtESINullEmp = ds.Tables[1];
                string NullESIEmployeesCodes = string.Empty;
                if (dtESINullEmp.Rows.Count>0)
                {
                    for (int i = 0; i < dtESINullEmp.Rows.Count; i++)
                    {
                        NullESIEmployeesCodes = NullESIEmployeesCodes +" "+ Convert.ToString(dtESINullEmp.Rows[i]["EmployeeCode"])+",";
                    }
                }
                if (generatESIChallanSheet(pdfPath, dt))
                    return base.BuildJson(true, 200, "success", new { filepath = pdfPath, Message = NullESIEmployeesCodes });
                else
                    return base.BuildJson(false, 200, "Failed", new { filepath = pdfPath,Message= NullESIEmployeesCodes });
            }
            catch (Exception ex)
            {
                TraceError.ErrorLog.Log(ex);
                return base.BuildJson(false, 400, "Failure", "There is some error while fetching Data");
            }
        }

        public JsonResult GetESIExtractData(int month, int year)
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int userId = Convert.ToInt32(Session["UserId"]);
                string pdfPath = string.Empty;
                string reportPath = DocumentProcessingSettings.TempDirectoryPath + "/";

                pdfPath = reportPath + companyId + "-" + (MonthEnum)month + "-" + year + ".xls";
                PayRollReports.PFChallan ps = new PayRollReports.PFChallan();
                DataSet ds = ps.GetESIExtractData(month, year, companyId, reportPath, userId);
                DataTable dt = ds.Tables[0];
                DataTable temp = ds.Tables[0];
                for (int i = 0; i < temp.Rows.Count; i++)
                {
                    EmployeeList emplis = new EmployeeList(companyId);

                    var emp = emplis.Where(w => w.EmployeeCode == Convert.ToString(temp.Rows[i]["EmployeeCode"])).FirstOrDefault();
                    if (!object.ReferenceEquals(emp, null))
                    {
                        if ((emp.SeparationDate > DateTime.MinValue && emp.SeparationDate < DateTime.Parse(DateTime.DaysInMonth(year, month) + "/" + month + "/" + year, new CultureInfo("en-GB")) && (emp.LastWorkingDate.Month != month && emp.LastWorkingDate.Year != year)))
                        {
                            dt.Rows.Remove(temp.Rows[i]);
                        }
                    }


                }
                DataTable dtESINullEmp = ds.Tables[1];
                string NullESIEmployeesCodes = string.Empty;
                if (dtESINullEmp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtESINullEmp.Rows.Count; i++)
                    {
                        NullESIEmployeesCodes = NullESIEmployeesCodes + " " + Convert.ToString(dtESINullEmp.Rows[i]["EmployeeCode"]) + ",";
                    }
                }
                Company com = new Company(companyId);
                
                string compDetails = com.CompanyName.ToUpper() + "*" + com.AddressLine1 + "*" + com.AddressLine2+ "*" + com.City+" ,"+ com.State+"-"+com.PinCode ;
                if (generatESIExtractData (pdfPath, dt, compDetails.ToUpper()))
                    return base.BuildJson(true, 200, "success", new { filepath = pdfPath, Message = NullESIEmployeesCodes });
                else
                    return base.BuildJson(false, 200, "Failed", new { filepath = pdfPath, Message = NullESIEmployeesCodes });
            }
            catch (Exception ex)
            {
                TraceError.ErrorLog.Log(ex);
                return base.BuildJson(false, 400, "Failure", "There is some error while fetching Data");
            }
        }


        public bool generatPfChallanSheet(string PDFFilePath, List<PayRollReports.PFChallan> dt)
        {
            try
            {

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
                rpt.ReportPath = "Reports/PFChallanXLReport.rdlc";

                ReportDataSource rptDs = new ReportDataSource("PFChallan", dt);

                rpt.DataSources.Add(rptDs);

                byte[] renderedBytes = null;

                renderedBytes = rpt.Render("Excel", null, out mimeType, out encoding, out extension, out streamIds, out warnings);



                string contentype = mimeType;


                using (FileStream fs = System.IO.File.Create(PDFFilePath))
                {
                    fs.Write(renderedBytes, 0, (int)renderedBytes.Length);
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return false;
            }

        }

        public bool generatESIChallanSheet(string PDFFilePath, DataTable dt)
        {
            try
            {

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
                rpt.ReportPath = "Reports/ESIExtractNew.rdlc";

                ReportDataSource rptDs = new ReportDataSource("ESIExtract", dt);

                rpt.DataSources.Add(rptDs);

                byte[] renderedBytes = null;

                renderedBytes = rpt.Render("Excel", null, out mimeType, out encoding, out extension, out streamIds, out warnings);



                string contentype = mimeType;


                using (FileStream fs = System.IO.File.Create(PDFFilePath))
                {
                    fs.Write(renderedBytes, 0, (int)renderedBytes.Length);
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return false;
            }

        }
        public bool generatESIExtractData(string PDFFilePath, DataTable dt,string Company)
        {
            try
            {

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
                rpt.ReportPath = "Reports/ESIExtract.rdlc";

                ReportDataSource rptDs = new ReportDataSource("DSESIExtract", dt);

                rpt.DataSources.Add(rptDs);
                ReportParameterCollection rpcollection = new ReportParameterCollection();

                rpcollection.Add(new ReportParameter("Company", Company));
                rpt.SetParameters(rpcollection);

                byte[] renderedBytes = null;

                renderedBytes = rpt.Render("Excel", null, out mimeType, out encoding, out extension, out streamIds, out warnings);



                string contentype = mimeType;


                using (FileStream fs = System.IO.File.Create(PDFFilePath))
                {
                    fs.Write(renderedBytes, 0, (int)renderedBytes.Length);
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return false;
            }

        }


    }


    public class jsonPFChallanTemplate
    {

        public int id { get; set; }
        public int companyId { get; set; }
        public string tableName { get; set; }
        public string columnName { get; set; }

        public string fieldName { get; set; }
        public string displayAs { get; set; }

        public int displayOrder { get; set; }
        public string tableValue { get; set; }
        // Modified By Keerthika on 20/0412017
        public static jsonPFChallanTemplate tojson(PayrollBO.PFChallan pfChallan)
        {

            return new jsonPFChallanTemplate()
            {
                id = pfChallan.Id,
                companyId = pfChallan.CompanyId,
                tableName = pfChallan.TableName,
                tableValue = pfChallan.TableName == "Salary" ? "PayrollHistory" : pfChallan.TableName == "SalaryBase" ? "PayrollHistoryBaseValue" : pfChallan.TableName,
                columnName = pfChallan.ColumnName,
                displayAs = pfChallan.DisplayAs,
                fieldName = pfChallan.AttributeModel.DisplayAs,
                displayOrder = pfChallan.DisplayOrder

            };
        }
        public static PayrollBO.PFChallan convertobject(jsonPFChallanTemplate pfChallan)
        {
            return new PayrollBO.PFChallan()
            {
                Id = pfChallan.id,
                CompanyId = pfChallan.companyId,
                TableName = pfChallan.tableName,
                ColumnName = pfChallan.columnName,
                DisplayAs = pfChallan.displayAs,
                DisplayOrder = pfChallan.displayOrder

            };
        }

    }
}