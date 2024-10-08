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
using PayrollBO.Tax;
using System.Web.UI.WebControls;
using System.Web.UI;
using Payroll.CustomFilter;

namespace Payroll.Controllers.Tax
{
    [SessionExpireAttribute]
    public class TaxUtilController : BaseController
    {
        // GET: Util
        List<string> error = new List<string>();
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
            jsonTaxXlImport xlimport = new jsonTaxXlImport();
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
            ret.Add(TaxImportTables.GetImportTable(companyId));
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
        public JsonResult ProcessImport(jsonTaxXlImport xlImport, string table, int startRow, int endRow, string fromRange, string toRange, string EffMonth, string EffYear, string ImportFile)
        {
            Decimal num1 = 0;
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            string companyId = Session["CompanyId"].ToString();
            int userId = Convert.ToInt32(Session["UserId"]);
            var path = Path.Combine(Server.MapPath("~/CompanyData/" + companyId + "/Import/" + Convert.ToString(xlImport.fileId)), xlImport.fileName);
            int finishRow = endRow - startRow + 1;
            string reg = Regex.Replace(fromRange, "[^0-9]+", string.Empty);
            int stRow = startRow - Convert.ToInt16(reg) - 1;
            DataSet datas = readXL(path, stRow, finishRow, fromRange, toRange);
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            DataTable dtFinal = new DataTable();
            dtFinal.Columns.Add("Emp Code", typeof(string));
            dtFinal.Columns.Add("Effective Month", typeof(string));
            dtFinal.Columns.Add("Effective Year", typeof(string));
            dtFinal.Columns.Add("CompanyId", typeof(string));
            dtFinal.Columns.Add("Section Name", typeof(string));
            dtFinal.Columns.Add("Declared Value", typeof(string));
            if (ImportFile == "Actual Rent Paid")
            {
                DataTable dtCloned = datas.Tables[table].Clone();
                dtCloned.Columns[0].DataType = typeof(string);
                foreach (DataRow row in datas.Tables[table].Rows)
                {
                    dtCloned.ImportRow(row);
                }
                dt = dtCloned;
            }
            else
            {
                dt = datas.Tables[table];
            }
                
            TaxImportColumns txcolums = new TaxImportColumns();
            if (ImportFile == "Actual Rent Paid")
            {
                for (int rowCount = 0; rowCount < dt.Rows.Count; rowCount++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName.ToString() == "Emp Code")
                        {
                            validator.ValidateRequired(Convert.ToString(dt.Rows[rowCount]["Emp Code"]), ref error, table, rowCount + startRow - 1, dt.Columns[j].ColumnName.ToString());
                        }
                        else if (dt.Columns[j].ColumnName.ToString() == "MetroRent")
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(dt.Rows[rowCount]["MetroRent"].ToString())))
                            {
                                dt.Rows[rowCount]["MetroRent"] = "0";
                                //error.Add("Please provide valid data at row " + (rowCount + startRow) + " and column " + dt.Columns[j].ColumnName.ToString() + " in the sheet of " + table);
                            }
                            else if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[rowCount]["MetroRent"].ToString())))
                            {
                                if (!Decimal.TryParse(dt.Rows[rowCount]["MetroRent"].ToString(), out num1))
                                {
                                    error.Add("Please provide valid number at row " + (rowCount + startRow) + " and column " + dt.Columns[j].ColumnName.ToString() + " in the sheet of " + table);
                                }
                            }
                        }
                        else if (dt.Columns[j].ColumnName.ToString() == "NonMetroRent")
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(dt.Rows[rowCount]["NonMetroRent"].ToString())))
                            {
                                dt.Rows[rowCount]["NonMetroRent"] = "0";
                                //error.Add("Please provide valid data at row " + (rowCount + startRow) + " and column " + dt.Columns[j].ColumnName.ToString() + " in the sheet of " + table);
                            }
                            else if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[rowCount]["NonMetroRent"].ToString())))
                            {
                                if (!Decimal.TryParse(dt.Rows[rowCount]["NonMetroRent"].ToString(), out num1))
                                {
                                    error.Add("Please provide valid number at row " + (rowCount + startRow) + " and column " + dt.Columns[j].ColumnName.ToString() + " in the sheet of " + table);
                                }
                            }
                        }
                    }
                }
                if (error.Count <= 0)
                {
                    dtt = txcolums.SaveActualRentImportExcel(dt, Convert.ToInt32(companyId), EffMonth, EffYear);
                }
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dtrow = dtFinal.NewRow();
                    for (int j = 3; j < dt.Columns.Count; j++)
                    {
                        dtrow = dtFinal.NewRow();
                        dtrow["Emp Code"] = dt.Rows[i][0].ToString();
                        dtrow["Effective Month"] = dt.Rows[i][1].ToString();
                        dtrow["Effective Year"] = dt.Rows[i][2].ToString();
                        dtrow["CompanyId"] = companyId;
                        dtrow["Section Name"] = dt.Columns[j].ColumnName.ToString();
                        if (string.IsNullOrEmpty(Convert.ToString(dt.Rows[i][j].ToString())))
                        {
                            dt.Rows[i][j] = "0";
                            //error.Add("Please provide valid data at row " + (i + startRow) + " and column " + dt.Columns[j].ColumnName.ToString() + " in the sheet of " + table);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i][j].ToString())))
                        {
                            if (!Decimal.TryParse(dt.Rows[i][j].ToString(), out num1))
                            {
                                error.Add("Please provide valid number at row " + (i + startRow) + " and column " + dt.Columns[j].ColumnName.ToString() + " in the sheet of " + table);
                            }
                        }
                        dtrow["Declared Value"] = dt.Rows[i][j].ToString();
                        dtFinal.Rows.Add(dtrow);
                    }
                }
                if (error.Count <= 0)
                {
                    dtt = txcolums.SaveImportExcel(dtFinal);
                }
            }
            return base.BuildJson(true, 200, "success", error);
        }
        public JsonResult GetSampleExcel(string ImportFile)
        {
            string path = string.Empty;
            DataTable dtActual = new DataTable();

            if (ImportFile == "Actual Rent Paid")
            {
                dtActual.Columns.Add("Emp Code", typeof(string));
                dtActual.Columns.Add("Rent Month", typeof(string));
                dtActual.Columns.Add("MetroRent", typeof(string));
                dtActual.Columns.Add("NonMetroRent", typeof(string));
                DataRow dtrow = dtActual.NewRow();
                dtrow["Emp Code"] = "0";
                dtrow["Rent Month"] = "0";
                dtrow["MetroRent"] = "0";
                dtrow["NonMetroRent"] = "0";
                dtActual.Rows.Add(dtrow);
                path = generateExcel(dtActual, "ActualRentPaidImport");
            }
            else
            {
                TaxImportColumns txcolums = new TaxImportColumns();
                DataTable dtDeclaration = new DataTable();
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                dtDeclaration = txcolums.GetExportSampleExcel(companyId);
                path = generateExcel(dtDeclaration, "TaxDeclarationImport");
            }
            return base.BuildJson(true, 200, "success", path);
        }
        private string generateExcel(DataTable dt, string TableName)
        {
            GridView GridView1 = new GridView();
            GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell1 = new TableCell();
            GridView1.AllowPaging = false;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Paysheet.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView1.RenderControl(hw);
            string PDFFilePath = DocumentProcessingSettings.TempDirectoryPath + "/" + TableName + ".xls";
            string renderedGridView = sw.ToString();
            System.IO.File.WriteAllText(PDFFilePath, renderedGridView);
            return PDFFilePath;
        }

        private jsonTaxXlImport readSheetName(string sourceFile, string fromRange, string torange)
        {
            jsonTaxXlImport retObj = new jsonTaxXlImport();
            retObj.XlSheet = new List<XlSheet>();
            string strConn = GetXLOldebConnection(sourceFile);
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
            string strConn = GetXLOldebConnection(sourceFile);
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
        public static string GetXLOldebConnection(string sourceFile)
        {
            string strConn;
            if (sourceFile.Contains(".xlsx"))
            {
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sourceFile + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
            }

            else
            {
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sourceFile + @";Extended Properties=""Excel 8.0;IMEX=1;HDR=Yes;TypeGuessRows=0;ImportMixedTypes=Text""";
                //strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sourceFile + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                //strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sourceFile + ";Extended Properties=HTML Import";
            }

            return strConn;
        }
    }

    public class jsonTaxXlImport
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

        public Guid OtherTableUniqueId { get; set; }
        public static jsonImportColumn toJson(ImportColumn input)
        {
            jsonImportColumn retObj = new jsonImportColumn();
            retObj.Name = input.Name;
            retObj.MappedColumnName = input.MappedColumnName;
            retObj.MinVal = input.MinVal;
            retObj.MaxVal = input.MaxLength;
            retObj.IsRequired = input.IsRequired;
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
}