using PayrollBO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TraceError;
using Payroll.CustomFilter;
using Payroll.Helpers;
using Microsoft.Ajax.Utilities;
using PayrollBO.Leave;

namespace Payroll.Controllers
{
    [SessionExpireAttribute]
    public class UtilController : BaseController
    {
        // GET: Util
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetIncrement(Guid employeeId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            return base.BuildJson(true, 200, "success", null);
        }

        public JsonResult GetImportTemplates()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            ImportTemplateList importTemplate = new ImportTemplateList(companyId);
            importTemplate.InitializeEmptyObject();
            List<jsonImportTemplate> retObject = new List<jsonImportTemplate>();

            importTemplate.ForEach(u =>
            {
                retObject.Add(jsonImportTemplate.toJson(u));
            });
            return base.BuildJson(true, 200, "success", retObject);
        }

        public JsonResult GetImportTemplate(Guid templateId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            ImportTemplate importTemplate = new ImportTemplate(templateId, companyId);
            jsonImportTemplate retObject = new jsonImportTemplate();
            retObject = jsonImportTemplate.toJson(importTemplate);
            return base.BuildJson(true, 200, "success", retObject);
        }

        public JsonResult SaveImportTemplate(jsonImportTemplate importTemplate, bool currentSetting)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            ImportTemplate saveObj = jsonImportTemplate.toObject(importTemplate);
            saveObj.CompanyId = companyId;
            saveObj.CreatedBy = userId;
            saveObj.ModifiedBy = userId;
            if (saveObj.Save())
            {
                if (currentSetting)
                {
                    ImportTemplateDetail tmp = new ImportTemplateDetail();
                    tmp.Delete(companyId, saveObj.Id);
                    saveObj.ImportTemplateDetailsList.ForEach(u =>
                    {
                        u.ImportTemplateId = saveObj.Id;
                        u.ModifiedBy = userId;
                        u.CompanyId = companyId;
                        u.CreatedBy = userId;
                        u.Save();
                    });
                }
                return base.BuildJson(true, 200, "success", importTemplate);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving a record.", importTemplate);
            }

        }


        [HttpPost]
        public JsonResult UploadFile()//HttpPostedFileBase file
        {

            List<object> ret = new List<object>();
            jsonXlImport xlimport = new jsonXlImport();
            int companyId = Convert.ToInt32(Session["CompanyId"]);
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
                        var fileName = Path.GetFileName(file);
                        string fileid = Guid.NewGuid().ToString().ToUpper();
                        string dirpath = Server.MapPath("~/CompanyData/" + companyId + "/Import/");
                        HelperCls dirDel = new HelperCls();
                        dirDel.ImportOldDirectoryDelete(dirpath);

                        var path = Path.Combine(Server.MapPath("~/CompanyData/" + companyId + "/Import/" + fileid), fileName);
                        if (!System.IO.Directory.Exists(path))
                        {
                            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                        }
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        xlimport = readSheetName(path, Request.Form["fromRange"], Request.Form["toRange"]);
                        xlimport.fileId = new Guid(fileid);
                        xlimport.fileName = fileName;
                        xlimport.Filepath = path;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                string exss = ex.Message;
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }

            ret.Add(xlimport);
            ret.Add(importTable.GetImportTable(companyId));
            return Json(ret);
        }
        public JsonResult DownLoadTemp()//HttpPostedFileBase file
        {

            int companyId = Convert.ToInt32(Session["CompanyId"]);

            jsonImportTemplate Obj = new jsonImportTemplate();

            try
            {

                var fileName = "LeaveOpeningImportTemplate.xlsx";
                Obj.filePath = Path.Combine(Server.MapPath("~/Content/"), fileName);


                return base.BuildJson(true, 200, "success", Obj);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 400, "failure", ex.Message);

            }
        }
        public JsonResult TableNames(string importTableName)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            List<string> TABLETYPE = new List<string>();
            string TableName = string.Empty;
            List<object> retobj = new List<object>();
            List<importTable> imptabl = new List<importTable>();
            if (importTableName.Trim().ToUpper() == "EMPLOYEE MASTER IMPORT")
            {
                imptabl = importTable.GetEmployeeImportTable(companyId);
            }
            else if (importTableName.Trim().ToUpper() == "INCREMENT IMPORT")
            {
                imptabl = importTable.GetIncrementImportTable(companyId);
            }
            else if (importTableName.Trim().ToUpper() == "MONTHLY INPUT IMPORT")
            {
                imptabl = importTable.GetMonthlyInputImportTable(companyId);
            }
            else if (importTableName.Trim().ToUpper() == "EMPLOYEE SEPARATION IMPORT")
            {
                imptabl = importTable.GetEmployeeSeparationImportTable(companyId);
            }
            else if (importTableName.Trim().ToUpper() == "EMPLOYEE RELEASE IMPORT")
            {
                imptabl = importTable.GetEmployeeReleaseImportTable(companyId);
            }
            else if (importTableName.Trim().ToUpper() == "PAST DATA IMPORT")
            {
                imptabl = importTable.GetPostDataImportTable(companyId);
            }
            else if (importTableName.Trim().ToUpper() == "POPUP DATA IMPORT")
            {
                imptabl = importTable.GetPopUpImportTable(companyId);
            }
            else if(importTableName.Trim().ToUpper() == "LOAN IMPORT")
            {
                imptabl = importTable.GetLoanImportTable(companyId);
            }
            else if (importTableName.Trim().ToUpper() == "EMPLOYEE CODE CHANGE IMPORT")
            {
                imptabl = importTable.GetEmployeeCodeChangeImportTable(companyId);
            }
            List<jsonTableNames> jsondata = new List<jsonTableNames>();
            imptabl.ForEach(u =>
            {
                string id = u.Name.Replace(" ", "#");
                jsondata.Add(jsonTableNames.tojson(u.Name.ToString(), id.ToString()));
            });
            retobj.Add(jsondata);
            return base.BuildJson(true, 200, "success", retobj);
        }
        public JsonResult ColumnNames(string ColumnName)
        {

            ColumnName = ColumnName.Replace("#", " ");
            string[] columnPar = ColumnName.Split(',');
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            List<string> TABLETYPE = new List<string>();
            string TableName = string.Empty;
            List<object> retobj = new List<object>();
            List<importTable> imptabl = new List<importTable>();
            imptabl = importTable.GetImportTable(companyId);
            List<jsonTableNames> jsondata = new List<jsonTableNames>();
            var tblcolumname = importTable.GetImportTable(companyId);
            List<ImportColumn> strclumnnam = new List<ImportColumn>();
            imptabl.ForEach(u =>
            {
                string id = u.Name.Replace(" ", "#");
                jsondata.Add(jsonTableNames.tojson(u.Name.ToString(), id.ToString()));
            });
            DataTable dtFinal = new DataTable();
            dtFinal.Columns.Add("Names", typeof(string));
            for (int i = 0; i < columnPar.Length; i++)
            {
                string name = columnPar[i].ToString();
                for (int j = 0; j < tblcolumname.Count; j++)
                {
                    string tblnme = tblcolumname[j].Name.ToString();
                    if (tblnme.Trim().ToUpper() == name.Trim().ToUpper())
                    {
                        List<ImportColumn> imporcol = new List<ImportColumn>();
                        imporcol = tblcolumname[j].ImportColumns.ToList();
                        for (int k = 0; k < imporcol.Count; k++)
                        {
                            DataRow dtrow = dtFinal.NewRow();
                            strclumnnam.Add(imporcol[k]);
                            dtrow["Names"] = imporcol[k].Name.ToString();
                            dtFinal.Rows.Add(dtrow);
                        }
                    }
                }
            }
            List<ImportColumn> strclumnnam1 = strclumnnam.DistinctBy(x => x.Name).ToList();
            retobj.Add(strclumnnam1);
            return base.BuildJson(true, 200, "success", retobj);
        }
        public JsonResult ProcessImport(jsonXlImport xlImport, List<importTable> table, bool IsAmendment, bool addMaster, int startRow, int endRow, string fromRange, string toRange)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            table = table.OrderBy(u => u.order).ToList();
            var path = Path.Combine(Server.MapPath("~/CompanyData/" + companyId + "/Import/" + Convert.ToString(xlImport.fileId)), xlImport.fileName);
            int finishRow = endRow - startRow + 1;
            string reg = Regex.Replace(fromRange, "[^0-9]+", string.Empty);
            string toreg = Regex.Replace(toRange, "[^A-Z]+", string.Empty);
            string tonge = toreg + endRow;
            toRange = tonge;
            int stRow = startRow - Convert.ToInt16(reg) - 1;
            DataSet datas = readXL(path, stRow, finishRow, fromRange, toRange);
            Import import = new Import();
            List<string> retError = new List<string>();
            List<string> error = new List<string>();
            table.ForEach(u =>
            {
                u.IsAmendment = IsAmendment;
                u.AddMasterValue = addMaster;
                if (!string.IsNullOrEmpty(u.MappedSheet))
                {
                    DataTable dt = new DataTable();
                    dt = datas.Tables[u.MappedSheet];
                    int DBConnectionId = Convert.ToInt32(Session["DBConnectionId"]);
                    switch (u.Name)
                    {
                        case "Employee":
                            error = import.importEmployee(u, dt, companyId, userId, startRow, DBConnectionId);
                            break;
                        case "Employee Training":
                            error = import.importEmployeeTraining(u, dt, companyId, userId, startRow);
                            break;
                        case "Employee Family":
                            error = import.importEmployeeFamily(u, dt, companyId, userId, startRow);
                            break;
                        case "Employee Academic":
                            error = import.importEmployeeAcademic(u, dt, companyId, userId, startRow);
                            break;
                        case "Employee Language":
                            error = import.importEmployeeLanguage(u, dt, companyId, userId, startRow);
                            break;
                        case "Employee Address":
                            error = import.importEmployeeAddress(u, dt, companyId, userId, startRow);
                            break;
                        case "Employee Nominee":
                            error = import.importEmployeeNominee(u, dt, companyId, userId, startRow);
                            break;
                        case "Employee Benefit Component":
                            error = import.importEmployeeBenefitComponent(u, dt, companyId, userId, startRow);
                            break;
                        case "Employee Emergency Contact":
                            error = import.importEmployeeEmergencyContact(u, dt, companyId, userId, startRow);
                            break;
                        case "Employee Employeement":
                            error = import.importEmployeeEmployeement(u, dt, companyId, userId, startRow);
                            break;
                        case "Employee Personal":
                            error = import.importEmployeeEmployeePersonal(u, dt, companyId, userId, startRow);
                            break;
                        case "Employee AdditionalInfo":
                            error = import.importEmployeeAdditionalInfo(u, dt, companyId, userId, startRow);
                            break;
                        case "Employee Bank Details":
                            error = import.importEmployeeEmployeeBankDetails(u, dt, companyId, userId, startRow);
                            break;
                        case "Category":
                            error = import.importCategory(u, dt, companyId, userId, startRow);
                            break;
                        case "Branch":
                            error = import.importBranch(u, dt, companyId, userId, startRow);
                            break;
                        case "Cost Centre":
                            error = import.importCostCentre(u, dt, companyId, userId, startRow);
                            break;
                        case "Department":
                            error = import.importDepartment(u, dt, companyId, userId, startRow);
                            break;
                        case "Designation":
                            error = import.importDesignation(u, dt, companyId, userId, startRow);
                            break;
                        case "ESI Dispensary":
                            error = import.importESIDespensary(u, dt, companyId, userId, startRow);
                            break;
                        case "ESI Location":
                            error = import.importESILocation(u, dt, companyId, userId, startRow);
                            break;
                        case "Grade":
                            error = import.importGrade(u, dt, companyId, userId, startRow);
                            break;
                        case "HR Component":
                            error = import.importHRComponent(u, dt, companyId, userId, startRow);
                            break;
                        case "Joining Document":
                            error = import.importJoiningDocument(u, dt, companyId, userId, startRow);
                            break;
                        case "Location":
                            error = import.importLocation(u, dt, companyId, userId, startRow);
                            break;
                        case "PT Location":
                            error = import.importPTLocation(u, dt, companyId, userId, startRow);
                            break;
                        case "Monthly Input":
                            foreach (DataRow row in dt.Rows)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (row.IsNull(column) || row[column].ToString().ToUpper() == "NIL")
                                        row.SetField(column, 0);
                                }
                            }
                            error = import.importMonthlyInput(u, dt, companyId, userId, startRow);
                            break;
                        case "Salary":
                            error = import.importSalary(u, dt, companyId, userId, startRow);
                            break;
                        case "Increment":
                            foreach (DataRow row in dt.Rows)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (row.IsNull(column) || row[column].ToString().Trim().ToUpper() == "NIL" || row[column].ToString().Trim().ToUpper() == "-")
                                        row.SetField(column, 0);
                                }
                            }
                            error = import.importIncrement(u, dt, companyId, userId, startRow);
                            break;
                        case "Salary Master":
                            foreach (DataRow row in dt.Rows)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (row.IsNull(column) || row[column].ToString().ToUpper() == "NIL")
                                        row.SetField(column, 0);
                                }
                            }
                            error = import.importSalaryMaster(u, dt, companyId, userId, startRow);
                            break;
                        case "Loan Master":
                            error = import.importLoanMaster(u, dt, companyId, userId, startRow);
                            break;
                        case "Loan Entry":
                            error = import.importLoanEntry(u, dt, companyId, userId, startRow);
                            break;
                        case "Loan Transaction":
                            error = import.importLoanTransaction(u, dt, companyId, userId, startRow);
                            break;
                        case "Employee Separation":
                            error = import.importEmployeeSeperation(u, dt, companyId, userId, startRow);
                            break;
                        case "Employee Release":
                            error = import.importEmployeeRelease(u, dt, companyId, userId, startRow);
                            break;
                        case "Tax Declaration":
                            error = import.importTaxDeclaration(u, dt, companyId, userId, startRow);
                            break;
                        case "Employee Code Change":
                            error = import.importEmployeeCodeChange(u, dt, companyId, userId, startRow);
                            break;
                        default:
                            break;
                    }
                }
                error.ForEach(p =>
                {
                    retError.Add(p);
                });

                //Clear Error List For Next Iteration
                error.Clear();
            });
            return base.BuildJson(true, 200, "success", retError);
        }

        private jsonXlImport readSheetName(string sourceFile, string fromRange, string torange)
        {
            jsonXlImport retObj = new jsonXlImport();
            retObj.XlSheet = new List<XlSheet>();
            string strConn = ComValue.GetXLOldebConnection(sourceFile);
            OleDbConnection conn;
            OleDbCommand cmd;
            conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable Sheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            try
            {
                for (int i = 0; i < Sheets.Rows.Count; i++)
                {
                    string worksheets = Sheets.Rows[i]["TABLE_NAME"].ToString();
                    if (!worksheets.EndsWith("$") && !worksheets.EndsWith("$'"))
                        continue;
                    if (worksheets.EndsWith("$'"))
                    {
                        worksheets = Regex.Replace(worksheets, "^'|'$", "");
                    }
                    String range = fromRange + ":" + torange;
                    string sqlQuery = String.Format("SELECT * FROM [{0}]", worksheets + range);
                    cmd = new OleDbCommand(sqlQuery, conn);
                    cmd.CommandType = CommandType.Text;
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    List<xlColumn> lstCol = new List<xlColumn>();
                    for (int x = 0; x < dt.Columns.Count; x++)
                    {
                        lstCol.Add(new xlColumn() { mapColumn = Convert.ToString(dt.Columns[x]), isMapped = false });
                    }
                    retObj.XlSheet.Add(new XlSheet() { sheetName = worksheets, xlColumns = lstCol, isMapped = false });
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                conn.Dispose();
            }
            return retObj;
        }
        private DataSet readXL(string sourceFile, int stRow, int maxRow, string fromRange, string torange)
        {
            string strConn = ComValue.GetXLOldebConnection(sourceFile);
            DataSet datas = new DataSet();
            OleDbConnection conn;
            OleDbCommand cmd;
            conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable Sheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            try
            {
                for (int i = 0; i < Sheets.Rows.Count; i++)
                {
                    string worksheets = Sheets.Rows[i]["TABLE_NAME"].ToString();
                    if (!worksheets.EndsWith("$") && !worksheets.EndsWith("$'"))
                        continue;
                    if (worksheets.EndsWith("$'"))
                    {
                        worksheets = Regex.Replace(worksheets, "^'|'$", "");
                    }
                    String range = fromRange + ":" + torange;
                    string sqlQuery = String.Format("SELECT * FROM [{0}]", worksheets + range);
                    Console.WriteLine(worksheets);
                    cmd = new OleDbCommand(sqlQuery, conn);
                    cmd.CommandType = CommandType.Text;
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(stRow, maxRow, dt);
                    dt.TableName = worksheets;
                    datas.Tables.Add(dt);
                    //var filteredRows = uploadDataTable.Rows.Cast<DataRow>().Where(
                    // row => row.ItemArray.Any(field => !(field is System.DBNull)));

                    //for (int x = 0; x < dt.Rows.Count; x++)
                    //{
                    //    string rowString = "";
                    //    for (int y = 0; y < dt.Columns.Count; y++)
                    //    {
                    //        rowString += "\"" + dt.Rows[x][y].ToString() + "\",";
                    //    }
                    //    Console.WriteLine(rowString);
                    //    //  wrtr.WriteLine(rowString);
                    //}
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                conn.Dispose();

            }
            return datas;

        }

        public JsonResult ImportLeaveData(string FilePath, Guid LeaveType)
        {
            List<string> retError = new List<string>();
            List<string> error = new List<string>();
            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                bool ConfigSetStat = true;
                bool LeaveOpeningLmtExcdStat = false;
                double MaxLeavOpening = 0;
                Guid EmployeeExceedsLmt = Guid.Empty;
                LeaveRequest LeaverequestBO = new LeaveRequest();
                LeaveFinanceYear DefaultFinancialid = new LeaveFinanceYear(companyId);
                DataSet datas = new DataSet();
                string strConn = GetXLOldebConnection(FilePath);
                OleDbConnection conn;
                OleDbCommand cmd;
                conn = new OleDbConnection(strConn);
                conn.Open();
                DataTable Sheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string worksheets = Sheets.Rows[0]["TABLE_NAME"].ToString();
                string sqlQuery = String.Format("SELECT * FROM [{0}]", worksheets);
                Console.WriteLine(worksheets);
                cmd = new OleDbCommand(sqlQuery, conn);
                cmd.CommandType = CommandType.Text;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dt.TableName = worksheets;
                datas.Tables.Add(dt);
                Employee Employee = new Employee();
                EmployeeList Employeelist = new EmployeeList(companyId);
                LeaveMasterList levmasterlist = new LeaveMasterList(companyId, DefaultFinancialid.Id);
                for (int v = 0; v < dt.Rows.Count; v++)
                {
                    if (ConfigSetStat == true && LeaveOpeningLmtExcdStat == false)
                    {
                        Employee employeeDetail = new Employee();
                        employeeDetail = Employeelist.Where(a => a.EmployeeCode == dt.Rows[v]["EmployeeCode"].ToString().Trim()).FirstOrDefault();
                        string LeaveConfigurationparameterid = LeaverequestBO.Parametersavailablecheck(levmasterlist[0].leaveparameter, employeeDetail);                       //LeaveConfigurationparameterid
                        string[] LCvalues = LeaveConfigurationparameterid.Split(',');
                        for (int i = 0; i < LCvalues.Length; i++)
                        {
                            LCvalues[i] = LCvalues[i].Trim();
                        }
                        if (LCvalues[1] != "")
                        {
                            LeaveConfigurationparameterid = LCvalues[1].ToString();
                        }
                        else
                        {
                            LeaveConfigurationparameterid = "00000000-0000-0000-0000-000000000000";
                        }
                        LeaveConfigList Levconfiglist = new LeaveConfigList(DefaultFinancialid.Id, LeaveType, new Guid(LeaveConfigurationparameterid));
                        if (Levconfiglist.Count > 0)
                        {
                            MaxLeavOpening = 0;
                            double LeaveOpenings = 0;
                            double LeaveCredit = 0;
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[v]["LeaveOpenings"])))
                            {
                                LeaveOpenings = Convert.ToDouble(dt.Rows[v]["LeaveOpenings"]);
                            }
                            else
                            {
                                LeaveOpenings = 0;
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[v]["LeaveCredit"])))
                            {
                                LeaveCredit = Convert.ToDouble(dt.Rows[v]["LeaveCredit"]);
                            }
                            else
                            {
                                LeaveCredit = 0;
                            }
                            MaxLeavOpening = Convert.ToDouble(Levconfiglist[0].overallMax);
                            double ActMaxOpenCred = 0;
                            ActMaxOpenCred = LeaveOpenings + LeaveCredit;
                            if (ActMaxOpenCred > MaxLeavOpening)
                            {
                                LeaveOpeningLmtExcdStat = true;
                                EmployeeExceedsLmt = employeeDetail.Id;
                            }
                        }
                        else
                        {
                            ConfigSetStat = false;
                        }
                    }
                }
                Employee LmtExceedsEmployee = new Employee(EmployeeExceedsLmt);
                if (ConfigSetStat == true && LeaveOpeningLmtExcdStat == false)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Guid EmpID = Guid.Empty;
                        double LeaveOpenings = 0;
                        double LeaveCredit = 0;
                        string EmployeeCode = dt.Rows[i]["EmployeeCode"].ToString().Trim();
                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LeaveOpenings"])))
                        {
                            LeaveOpenings = Convert.ToDouble(dt.Rows[i]["LeaveOpenings"]);
                        }
                        else
                        {
                            LeaveOpenings = 0;
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LeaveCredit"])))
                        {
                            LeaveCredit = Convert.ToDouble(dt.Rows[i]["LeaveCredit"]);
                        }
                        else
                        {
                            LeaveCredit = 0;
                        }

                        Employee = Employeelist.Where(a => a.EmployeeCode == EmployeeCode).FirstOrDefault();
                        if (Employee != null)
                        {
                            LeaveOpenings leaveopening = new LeaveOpenings(LeaveType, companyId, DefaultFinancialid.Id, EmployeeCode);
                            LeaveOpenings leaveopen = new LeaveOpenings();
                            //if (!object.ReferenceEquals(leaveopening.EmployeeId, Guid.Empty))
                            if (leaveopening.EmployeeId != Guid.Empty)
                            {

                                leaveopen.Id = leaveopening.Id;
                                leaveopen.FinanceYearId = leaveopening.FinanceYearId;
                                leaveopen.EmployeeId = leaveopening.EmployeeId;
                                leaveopen.LeaveType = leaveopening.LeaveType;
                                leaveopen.LeaveOpening = LeaveOpenings;
                                leaveopen.LeaveCredit = LeaveCredit;
                                leaveopen.ModifiedBy = userId;
                                leaveopen.Save();
                            }
                            else
                            {
                                leaveopen.Id = Guid.Empty;
                                leaveopen.FinanceYearId = DefaultFinancialid.Id;
                                leaveopen.EmployeeId = Employee.Id;
                                leaveopen.LeaveType = LeaveType;
                                leaveopen.LeaveOpening = LeaveOpenings;
                                leaveopen.LeaveCredit = LeaveCredit;
                                leaveopen.ModifiedBy = userId;
                                leaveopen.Save();
                            }
                        }
                        else
                        {
                            error.Add(" " + EmployeeCode + " Employee is not found in this company ");
                        }
                    }
                    if (error.Count > 0)
                    {
                        error.ForEach(p =>
                        {
                            retError.Add(p);
                        });
                        return base.BuildJson(false, 100, "Error While inserting Data", retError);
                    }
                    return base.BuildJson(true, 200, "All Data inserted Succesfully ", retError);
                }
                else if (ConfigSetStat == false)
                {
                    error.Add("Kindly Set the leave configuration");
                    error.ForEach(p =>
                    {
                        retError.Add(p);
                    });
                    return base.BuildJson(false, 200, "Kindly Set the leave configuration", retError);
                }
                else
                {
                    error.Add("Leave Opening and Credits of employee " + LmtExceedsEmployee.FirstName + " " + LmtExceedsEmployee.LastName + " (" + LmtExceedsEmployee.EmployeeCode + ")" + "exceeds the Maximum Limit.For more Details, Kindly check the Leave configuration setting of Particular Employee.");
                    error.ForEach(p =>
                    {
                        retError.Add(p);
                    });
                    return base.BuildJson(false, 200, "Leave Opening and Credits of employee " + LmtExceedsEmployee.FirstName + " " + LmtExceedsEmployee.LastName + " (" + LmtExceedsEmployee.EmployeeCode + ")" + "exceeds the Maximum Limit.For more Details, Kindly check the Leave configuration setting of Particular Employee.", retError);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                retError.Add(ex.Message);
                return base.BuildJson(false, 100, "Error While inserting Data", retError);
            }
        }

        public static string GetXLOldebConnection(string sourceFile)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sourceFile + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            if (sourceFile.Contains(".xlsx"))
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sourceFile + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
            return strConn;
        }

    }

    public class jsonXlImport
    {
        public Guid fileId { get; set; }

        public string fileName { get; set; }

        public List<XlSheet> XlSheet { get; set; }

        public string Filepath { get; set; }

    }

    public class XlSheet
    {
        public int startingRow { get; set; }

        public int endRow { get; set; }

        public string sheetName { get; set; }

        // public string mappingTbl { get; set; }

        // public string empCodeColumn { get; set; }
        public bool isMapped { get; set; }
        public List<xlColumn> xlColumns { get; set; }
    }

    public class xlColumn
    {
        // public Guid attrId { get; set; }

        //  public string attrName { get; set; }

        //public int allowedLength { get; set; }
        public bool isMapped { get; set; }
        public string mapColumn { get; set; }
    }

    public class jsonImportTable
    {
        public string Name { get; set; }

        public string MappedSheet { get; set; }

        public int order { get; set; }
        public List<jsonImportColumn> ImportColumns { get; set; }

        public static jsonImportTable toJson(importTable input)
        {
            jsonImportTable retObj = new jsonImportTable();
            retObj.ImportColumns = new List<jsonImportColumn>();
            retObj.MappedSheet = input.MappedSheet;
            retObj.Name = input.Name;
            retObj.order = input.order;
            input.ImportColumns.ForEach(u =>
            {
                retObj.ImportColumns.Add(jsonImportColumn.toJson(u));
            });
            return retObj;
        }
        public static importTable toObject(jsonImportTable input)
        {
            importTable retObj = new importTable();
            retObj.ImportColumns = new List<ImportColumn>();
            retObj.MappedSheet = input.MappedSheet;
            retObj.Name = input.Name;
            retObj.order = input.order;
            input.ImportColumns.ForEach(u =>
            {
                retObj.ImportColumns.Add(jsonImportColumn.toObject(u));
            });
            return retObj;
        }

    }
    public class jsonImportColumn
    {
        public string Name { get; set; }

        public string MappedColumnName { get; set; }

        public string MinVal { get; set; }

        public string MaxVal { get; set; }

        public bool IsRequired { get; set; }
        public string IsRequiredstr { get; set; }
        public string TableName { get; set; }
        public Guid OtherTableUniqueId { get; set; }
        public static jsonImportColumn toJson(ImportColumn input)
        {
            jsonImportColumn retObj = new jsonImportColumn();
            retObj.Name = input.Name;
            retObj.MappedColumnName = input.MappedColumnName;
            retObj.MinVal = input.MinVal;
            retObj.MaxVal = input.MaxLength;
            retObj.IsRequired = input.IsRequired;
            retObj.IsRequiredstr = input.IsRequired == true ? "*" : "";
            retObj.TableName = input.TableName;
            retObj.OtherTableUniqueId = input.OtherTableUniqueId;
            return retObj;
        }

        public static ImportColumn toObject(jsonImportColumn input)
        {
            ImportColumn retObj = new ImportColumn();
            retObj.Name = input.Name;
            retObj.MappedColumnName = input.MappedColumnName;
            retObj.MinVal = input.MinVal;
            retObj.MaxLength = input.MaxVal;
            retObj.IsRequired = input.IsRequired;
            retObj.IsRequiredstr = input.IsRequired == true ? "*" : "";
            retObj.TableName = input.TableName;
            retObj.OtherTableUniqueId = input.OtherTableUniqueId;
            return retObj;
        }
    }

    public class jsonImportTemplate
    {
        public string filePath { get; set; }
        public Guid id { get; set; }
        public string name { get; set; }

        public List<jsonImportTemplateDetail> jsonImportTemplateDetails { get; set; }

        public static jsonImportTemplate toJson(ImportTemplate input)
        {
            jsonImportTemplate retObj = new jsonImportTemplate();
            retObj.jsonImportTemplateDetails = new List<jsonImportTemplateDetail>();
            retObj.name = input.Name;
            retObj.id = input.Id;

            input.ImportTemplateDetailsList.ForEach(u =>
            {
                retObj.jsonImportTemplateDetails.Add(jsonImportTemplateDetail.toJson(u));
            });
            return retObj;
        }
        public static ImportTemplate toObject(jsonImportTemplate input)
        {
            ImportTemplate retObj = new ImportTemplate();
            retObj.ImportTemplateDetailsList = new ImportTemplateDetailsList();
            retObj.Name = input.name;
            retObj.Id = input.id;
            if (!object.ReferenceEquals(input.jsonImportTemplateDetails, null))
            {
                input.jsonImportTemplateDetails.ForEach(u =>
                {
                    retObj.ImportTemplateDetailsList.Add(jsonImportTemplateDetail.toObject(u));
                });
            }
            return retObj;
        }

    }

    public class jsonImportTemplateDetail
    {
        public Guid id { get; set; }
        public Guid importTemplateId { get; set; }
        public string tableName { get; set; }
        public string mappedSheetName { get; set; }
        public string tableColumn { get; set; }
        public string mappedSheetColumn { get; set; }


        public static jsonImportTemplateDetail toJson(ImportTemplateDetail input)
        {
            jsonImportTemplateDetail retObj = new jsonImportTemplateDetail();
            retObj.id = input.Id;
            retObj.importTemplateId = input.ImportTemplateId;
            retObj.tableName = input.TableName;
            retObj.mappedSheetName = input.MappedSheetName;
            retObj.tableColumn = input.TableColumn;
            retObj.mappedSheetColumn = input.MappedSheetColumn;
            return retObj;
        }
        public static ImportTemplateDetail toObject(jsonImportTemplateDetail input)
        {
            ImportTemplateDetail retObj = new ImportTemplateDetail();
            retObj.Id = input.id;
            retObj.ImportTemplateId = input.importTemplateId;
            retObj.TableName = input.tableName;
            retObj.MappedSheetName = input.mappedSheetName;
            retObj.TableColumn = input.tableColumn;
            retObj.MappedSheetColumn = input.mappedSheetColumn;
            return retObj;

        }

    }
    public class jsonTableNames
    {
        public string tablename { get; set; }
        public string tablenameid { get; set; }
        public bool view { get; set; }
        public bool edit { get; set; }

        public static jsonTableNames tojson(string optfield, string OPTFIELDDESC)
        {
            return new jsonTableNames()
            {
                tablename = optfield,
                tablenameid = OPTFIELDDESC,
                view = true,
                edit = true
            };
        }

    }
}