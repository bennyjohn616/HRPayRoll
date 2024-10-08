using PayrollBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TraceError;
using System.Web.Mvc;
using System.Data.OleDb;
using Payroll.CustomFilter;

namespace Payroll.Controllers
{
    [SessionExpireAttribute]
    public class FinanceYearController : BaseController
    {
        // GET: FinanceYear
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetFinanceYears()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            TXFinanceYearList financeYearlist = new TXFinanceYearList(companyId);
            List<jsonTxFinanceYear> jsondata = new List<jsonTxFinanceYear>();
            financeYearlist.ForEach(u => { jsondata.Add(jsonTxFinanceYear.toJson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);

        }
        public JsonResult GetFinanceYear(Guid financeYearId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            TXFinanceYear financeYearlist = new TXFinanceYear();
            financeYearlist = new TXFinanceYear(financeYearId, companyId);
            if (!object.ReferenceEquals(financeYearlist, null))
            {

                return base.BuildJson(true, 200, "success", jsonTxFinanceYear.toJson(financeYearlist));
            }
            else
            {
                return base.BuildJson(false, 200, "Please Complete Finance Year Setting", jsonTxFinanceYear.toJson(financeYearlist));
            }

        }
        public JsonResult GetDefaultFinaceYear()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            TXFinanceYearList financeYearlist = new TXFinanceYearList(companyId);
            TXFinanceYear defaultyear = new TXFinanceYear();
            defaultyear = financeYearlist.Where(e => e.IsActive).FirstOrDefault();
            if (defaultyear == null)
            {
                return base.BuildJson(false, 200, "Please Create Finance Year Setting", null);
            }
            return base.BuildJson(true, 200, "success", jsonTxFinanceYear.toJson(defaultyear));

        }

        public JsonResult EmployeeDefaultFinaceYear(DateTime date1)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            TXFinanceYearList financeYearlist = new TXFinanceYearList(companyId);
            TXFinanceYear defaultyear = new TXFinanceYear();
            defaultyear = financeYearlist.Where(e => e.StartingDate <= date1 && e.EndingDate >=date1 && e.IsDeleted == false).FirstOrDefault();
            if (defaultyear == null)
            {
                return base.BuildJson(false, 200, "Please Create Finance Year Setting", null);
            }
            return base.BuildJson(true, 200, "success", jsonTxFinanceYear.toJson(defaultyear));

        }

        public JsonResult GetTaxable(Guid financeYearid)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);


            TXSectionList txSectionlist = new TXSectionList(companyId);
            EntityAttributeModelList entitModel = new EntityModel(ComValue.SalaryTable, companyId).EntityAttributeModelList;
            List<jsonIncomeMatch> result = new List<jsonIncomeMatch>();
            IncomeMatchingList incomeMatchList = new IncomeMatchingList(financeYearid);
            List<TXSection> txsec = txSectionlist.Where(s => s.ParentId != Guid.Empty).ToList();

            //  List <jsonTaxSection>=
            entitModel.Where(d => d.AttributeModel.IsTaxable && d.AttributeModel.BehaviorType == "Earning").ToList().ForEach(d =>
            {
                IncomeMatching imatch = new IncomeMatching();
                imatch = incomeMatchList.Where(i => i.AttributemodelId == d.AttributeModelId).FirstOrDefault();
                TXSection ts = imatch != null ? txsec.Where(a => a.Id == imatch.ExemptionComponent).FirstOrDefault() : null;
                result.Add(new jsonIncomeMatch
                {
                    attributeId = d.AttributeModelId,
                    name = d.AttributeModel.Name,
                    displayAs = d.AttributeModel.DisplayAs,
                    matchingComponent = imatch != null ? imatch.MatchingComponent : Guid.Empty,
                    examptionComponent = imatch != null ? imatch.ExemptionComponent : Guid.Empty,
                    otherComponent = imatch != null ? imatch.OtherComponent : string.Empty,
                    matchingCompName = imatch != null ?
                              entitModel.Where(a => a.AttributeModelId == imatch.MatchingComponent).FirstOrDefault() != null ? entitModel.Where(a => a.AttributeModelId == imatch.MatchingComponent).FirstOrDefault().AttributeModel.Name : string.Empty : string.Empty,
                    examptionCompName = ts != null ? ts.Name : string.Empty,
                    operators = imatch != null ? imatch.Operator : string.Empty,
                    formula = imatch != null ? imatch.Formula : string.Empty,
                    taxDeductionmode = imatch != null ? imatch.TaxDeductionMode : "Normal",
                    orderno = imatch != null ? imatch.OrderNo : 0,
                    grossSection = imatch != null ? imatch.GrossSection : 0,
                    projection = imatch != null ? imatch.Projection ? "Yes" : "No" : "No",
                });
            });


            return base.BuildJson(true, 200, "success", result);

        }
        public JsonResult GetOtherExamption(string type)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            OtherExamptionList otherExamptionList = new OtherExamptionList(type);


            return base.BuildJson(true, 200, "success", otherExamptionList);

        }

        public JsonResult SaveFinanceYear(jsonTxFinanceYear dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);


            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            DateTime st = Convert.ToDateTime(dataValue.startDate);
            DateTime ed = Convert.ToDateTime(dataValue.EndDate);
            TXFinanceYearList txfl = new TXFinanceYearList(companyId);



            if (object.ReferenceEquals(txfl.Where(w => w.Id == dataValue.id).FirstOrDefault(), null))
            {
                //check date already exist:
                List<TXFinanceYear> txf = txfl.Where(w => (w.StartingDate <= st && w.EndingDate >= st) || (w.StartingDate <= ed && w.EndingDate >= ed)).ToList();

                if (txf.Count > 0)
                {
                    return base.BuildJson(false, 100, "Finance year Already Exist!!!", dataValue);
                }

            }


            TXFinanceYear txfinanceYear = jsonTxFinanceYear.convertObject(dataValue);
            txfinanceYear.CompanyId = companyId;
            txfinanceYear.CreatedBy = userId;
            txfinanceYear.ModifiedBy = txfinanceYear.CreatedBy;
            txfinanceYear.IsDeleted = false;
            txfinanceYear.IsActive = dataValue.defaultyear;
            isSaved = txfinanceYear.Save();
            // var save = txfinanceYear.Save();
            Guid financeYrID = dataValue.id == Guid.Empty ? txfinanceYear.Id : Guid.Empty;
            if (isSaved)
            {
                if (financeYrID != Guid.Empty)
                {
                    dataValue.InsertDefaultForFinancialYr(financeYrID, companyId, userId, Server.MapPath("~"));
                }
                if (dataValue.otherExemption != null && dataValue.otherExemption.Count > 0)
                {
                    dataValue.otherExemption.ForEach(e =>
                    {
                        OtherExamption otherExamp = new OtherExamption();
                        otherExamp = e;
                        otherExamp.FinanceYearId = txfinanceYear.Id;
                        otherExamp.Save();
                    });


                }
                return base.BuildJson(true, 200, "Financial Year saved sucessfully.Save all fromulas in tax field.", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }
        public JsonResult DeleteFinanceYear(jsonTxFinanceYear Data)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            if (Data.defaultyear == true)
            {
                return base.BuildJson(false, 100, "Unable to delete the default financial year.", Data);
            }
            TXFinanceYear TXFinanceYr = new TXFinanceYear();
            TXFinanceYr.Id = Data.id;
            TXFinanceYr.CreatedBy = Convert.ToInt32(Session["UserId"]);
            TXFinanceYr.ModifiedBy = TXFinanceYr.CreatedBy;
            TXFinanceYr.IsDeleted = true;
            if (TXFinanceYr.Delete())
            {
                return base.BuildJson(true, 200, "Data Deleted successfully", TXFinanceYr);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while deleting the data.", TXFinanceYr);
            }
        }

        public DateTime date1 { get; set; }

        public class jsonTxFinanceYear
        {
            /// <summary>
            /// Created By:Sharmila
            /// Created On:6.06.17
            /// </summary>
            /// <param name="financeYrID"></param>
            /// <param name="companyId"></param>
            /// <param name="userId"></param>
            /// <param name="filepath"></param>
            public void InsertDefaultForFinancialYr(Guid financeYrID, int companyId, int userId, string filepath)
            {
                DataSet datas = readXL(filepath);
                //  ErrorLog.LogTestWrite("TEST WRITE: Table Count is " + datas.Tables.Count);
                InsertTaxSection(financeYrID, companyId, userId, datas);
            }
            private DataSet readXL(string srcfile)
            {
                DataSet datas = new DataSet();
                try
                {

                    string sourceFile = srcfile + "\\StaticData\\Company Import.xls";

                    string strConn = ComValue.GetXLOldebConnection(sourceFile);

                    //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sourceFile + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                    //if (sourceFile.Contains(".xlsx"))
                    //    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sourceFile + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";

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

                            string sqlQuery = String.Format("SELECT * FROM [{0}]", worksheets);
                            Console.WriteLine(worksheets);
                            cmd = new OleDbCommand(sqlQuery, conn);
                            cmd.CommandType = CommandType.Text;
                            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
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

                }
                catch (Exception ex)
                {

                    ErrorLog.Log(ex);
                }
                return datas;

            }
            public Guid id { get; set; }

            public string startDate { get; set; }

            public string EndDate { get; set; }


            public string tanNo { get; set; }


            public string tdsCircle { get; set; }

            public string panNo { get; set; }

            public string taxDedAccNo { get; set; }

            public Guid employeeId { get; set; }
            public string empName { get; set; }

            public string place { get; set; }

            public bool defaultyear { get; set; }
            public OtherExamptionList otherExemption { get; set; }

            public static jsonTxFinanceYear toJson(TXFinanceYear txFinance)
            {
                return new jsonTxFinanceYear()
                {
                    id = txFinance.Id,
                    startDate = txFinance.StartingDate.ToString("dd/MMM/yyyy"),
                    EndDate = txFinance.EndingDate.ToString("dd/MMM/yyyy"),
                    tanNo = txFinance.TanNo,
                    taxDedAccNo = txFinance.TaxDeuctionAcNo,
                    panNo = txFinance.PANorGIRNO,
                    tdsCircle = txFinance.TDSCircle,
                    employeeId = txFinance.InchargeEmployeeId,
                    empName = new Employee(txFinance.CompanyId, txFinance.InchargeEmployeeId).FirstName,
                    otherExemption = txFinance.OtherExemptionList,
                    place = txFinance.Place,
                    defaultyear = txFinance.IsActive
                };
            }
            public static TXFinanceYear convertObject(jsonTxFinanceYear txFinance)
            {
                return new TXFinanceYear()
                {
                    Id = txFinance.id,
                    StartingDate = Convert.ToDateTime(txFinance.startDate),
                    EndingDate = Convert.ToDateTime(txFinance.EndDate),
                    TanNo = txFinance.tanNo,
                    TaxDeuctionAcNo = txFinance.taxDedAccNo,
                    PANorGIRNO = txFinance.panNo,
                    TDSCircle = txFinance.tdsCircle,
                    InchargeEmployeeId = txFinance.employeeId,
                    Place = txFinance.place,
                    IsActive = txFinance.defaultyear

                };
            }
            private void InsertTaxSection(Guid financeYrID, int companyId, int userId, DataSet ds)
            {
                try
                {
                    AttributeModelList Modelobj = new AttributeModelList(companyId);

                    DataTable dtValue = ds.Tables["AttributeModel$"];
                    for (int cnt = 0; cnt < dtValue.Rows.Count; cnt++)
                    {
                        AttributeModel attrMod = new AttributeModel();
                        attrMod.Name = Convert.ToString(dtValue.Rows[cnt]["Name"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[cnt]["Id"])) && Convert.ToString(dtValue.Rows[cnt]["Id"]) != "NULL")
                            attrMod.Id = new Guid(Convert.ToString(dtValue.Rows[cnt]["Id"]));
                        Modelobj.Add(attrMod);
                    }
                    TXFinanceYearList FinanceYrList = new TXFinanceYearList(companyId);
                    TXSectionList TSLObj = new TXSectionList(companyId, financeYrID);
                    TSLObj.ForEach(ts => ts.Delete());
                    DataTable dttblvalue = ds.Tables["TaxSections$"];

                    for (int count = 0; count < dttblvalue.Rows.Count; count++)
                    {

                        TXSection TSObj = new TXSection();
                        TSObj.Id = Guid.NewGuid();
                        if (!string.IsNullOrEmpty(Convert.ToString(dttblvalue.Rows[count]["Parent Section"])) && Convert.ToString(dttblvalue.Rows[count]["Parent Section"]) != "NULL")
                        {
                            var ParrentName = Convert.ToString(dttblvalue.Rows[count]["Parent Section"]);

                            var currentTSL = TSLObj.Where(d => d.Name == ParrentName).FirstOrDefault();
                            TSObj.ParentId = currentTSL.Id;
                        }
                        TSObj.CompanyId = companyId;
                        //if (!string.IsNullOrEmpty(Convert.ToString(dttblvalue.Rows[count]["Financial YearId"])) && Convert.ToString(dttblvalue.Rows[count]["Financial YearId"]) != "NULL")
                        //{
                        //    // var FinancialId = Convert.ToString(dttblvalue.Rows[count]["Financial YearId"]);
                        //    // TSObj.FinancialYearId = new Guid(FinancialId);
                        //    TSObj.FinancialYearId = FinanceYr.Id;

                        //}
                        TSObj.FinancialYearId = financeYrID;
                        TSObj.Name = Convert.ToString(dttblvalue.Rows[count]["Name"]);
                        TSObj.DisplayAs = Convert.ToString(dttblvalue.Rows[count]["Name"]);
                        TSObj.OrderNo = Convert.ToInt32(dttblvalue.Rows[count]["Order"]);
                        var lmt = (dttblvalue.Rows[count]["Limit"].ToString());
                        if ((!string.IsNullOrEmpty(Convert.ToString(lmt))) && lmt != "Null")
                            TSObj.Limit = Convert.ToDecimal(lmt);
                        var Examp = (dttblvalue.Rows[count]["Examption Method"].ToString());
                        if (!string.IsNullOrEmpty(Convert.ToString(Examp)))
                        {
                            if (Examp == "Monthly")
                                TSObj.ExemptionType = Convert.ToInt32("0");
                            if (Examp == "Yearly")
                                TSObj.ExemptionType = Convert.ToInt32("1");
                            if (Examp == "")
                                TSObj.ExemptionType = Convert.ToInt32("");

                        }
                        var GrossDeduct = Convert.ToString(dttblvalue.Rows[count]["IsGrossDeductable"]);
                        if ((!string.IsNullOrEmpty(Convert.ToString(GrossDeduct))) && GrossDeduct != "Null")
                            TSObj.IsGrossDeductable = Convert.ToBoolean(GrossDeduct);

                        var DocRequired = Convert.ToString(dttblvalue.Rows[count]["IsDocumentRequired"]);
                        if ((!string.IsNullOrEmpty(Convert.ToString(DocRequired))) && DocRequired != "Null")
                            TSObj.IsDocumentRequired = Convert.ToBoolean(DocRequired);

                        var ApprovelReq = Convert.ToString(dttblvalue.Rows[count]["IsApprovelRequired"]);
                        if ((!string.IsNullOrEmpty(Convert.ToString(ApprovelReq))) && ApprovelReq != "Null")
                            TSObj.IsApprovelRequired = Convert.ToBoolean(ApprovelReq);

                        var Active = Convert.ToString(dttblvalue.Rows[count]["IsActive"]);
                        if ((!string.IsNullOrEmpty(Convert.ToString(Active))) && Active != "Null")
                            TSObj.IsActive = false;
                        TSObj.CreatedOn = DateTime.Now;
                        TSObj.CreatedBy = userId;
                        TSObj.ModifiedOn = DateTime.Now;
                        TSObj.ModifiedBy = TSObj.CreatedBy;
                        TSObj.IsDeleted = false;

                        TSObj.Formula = Convert.ToString(dttblvalue.Rows[count]["Formula"]);
                        string formulatoken = Convert.ToString(dttblvalue.Rows[count]["Formula"]);
                        char[] delimiterChars = { ',', ' ', '[', ']', '*', '-', '(', ')' };
                        string[] words = formulatoken.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);

                        if (words.Length != 0)
                        {
                            for (int c = 0; c < words.Length; c++)
                            {
                                AttributeModel newmod = new AttributeModel();
                                newmod = Modelobj.Where(d => d.Name == words[c]).FirstOrDefault();
                                if (newmod != null)
                                {
                                    string FetchedID = Convert.ToString(string.Concat("{" + newmod.Id + "}"));
                                    formulatoken = formulatoken.Replace(words[c], FetchedID);
                                    TSObj.Value = Convert.ToString(formulatoken);
                                    TSObj.FormulaValue = formulatoken;
                                }

                            }
                        }
                        var projection = Convert.ToString(dttblvalue.Rows[count]["Projection"]);
                        if ((!string.IsNullOrEmpty(Convert.ToString(projection))) && projection != "Null")
                            TSObj.Projection = Convert.ToString(projection);

                        var DocReq = Convert.ToString(dttblvalue.Rows[count]["SectionType"]);
                        if ((!string.IsNullOrEmpty(Convert.ToString(DocReq))) && DocReq != "Null")
                            TSObj.SectionType = Convert.ToString(DocReq);


                        var IncomeTypeId = Convert.ToString(dttblvalue.Rows[count]["IncomeType"]);
                        if ((!string.IsNullOrEmpty(Convert.ToString(IncomeTypeId))) && IncomeTypeId != "Null")
                            TSObj.IncomeTypeId = Convert.ToInt32(IncomeTypeId);
                        var Formulatype = Convert.ToString(dttblvalue.Rows[count]["FormulaType"]);
                        if ((!string.IsNullOrEmpty(Convert.ToString(Formulatype))) && Formulatype != "Null")
                            TSObj.FormulaType = Convert.ToInt32(Formulatype);

                        var baseformula = Convert.ToString(dttblvalue.Rows[count]["BaseFormula"]);
                        if ((!string.IsNullOrEmpty(Convert.ToString(baseformula))) && baseformula != "Null")
                            TSObj.BaseFormula = Convert.ToString(baseformula);

                        var basevalue = Convert.ToString(dttblvalue.Rows[count]["BaseValue"]);
                        if ((!string.IsNullOrEmpty(Convert.ToString(basevalue))) && basevalue != "Null")
                            TSObj.BaseValue = Convert.ToString(basevalue);

                        TSLObj.Add(TSObj);

                    }

                    TSLObj.ForEach(l =>
                    {
                        l.Save();
                    });

                    // Command by Muthu 19th march 2018 not required and avoid duplicate
                    //Lookup Data import
                    // OtherExamptionList otherExpList = new OtherExamptionList(financeYrID);
                    // otherExpList.ForEach(exp => exp.Delete()); --already command by someone
                    //DataTable dtLoopup = ds.Tables["Lookups$"];
                    //for (int count = 0; count < dtLoopup.Rows.Count; count++)
                    //{
                    //    TXFinanceYear FinanceYr = new TXFinanceYear();
                    //    OtherExamption otherExp = new OtherExamption();



                    //    otherExp.FinanceYearId = financeYrID;
                    //    var Lookupname = Convert.ToString(dtLoopup.Rows[count]["Name"]);
                    //    if ((!string.IsNullOrEmpty(Convert.ToString(Lookupname))) && Lookupname != "Null")
                    //        otherExp.Name = Convert.ToString(dtLoopup.Rows[count]["Name"]);
                    //    var LookupType = Convert.ToString(dtLoopup.Rows[count]["Type"]);
                    //    if ((!string.IsNullOrEmpty(Convert.ToString(LookupType))) && LookupType != "Null")
                    //        otherExp.Type = Convert.ToString(dtLoopup.Rows[count]["Type"]);
                    //    otherExp.Value = 0;
                    //    otherExp.CreatedOn = DateTime.Now;
                    //    otherExp.CreatedBy = userId;
                    //    otherExp.IsDeleted = false;
                    //    otherExpList.Add(otherExp);

                    //}
                    //otherExpList.ForEach(exp =>
                    //{
                    //    exp.Save();
                    //});



                    //Tax Behaviour
                    DataTable dtTaxBhaviour = ds.Tables["'TAX Behaviour$'"];
                    AttributeModelList attrmodellist = new AttributeModelList(companyId);
                    TaxBehaviorList TaxBehaviourlist = new TaxBehaviorList();
                    for (int count = 0; count < dtTaxBhaviour.Rows.Count; count++)
                    {
                        var input = "";
                        string atrrname = Convert.ToString(dtTaxBhaviour.Rows[count]["Name"]);
                        var HiddenFormula = Convert.ToString(dtTaxBhaviour.Rows[count]["Formula"]);
                        string[] HiddenForm = HiddenFormula.Split(new string[] { " ", ":", "<", ">", ">=", "<=", "=", "AND", "OR", "If", "Else", "THEN", "{", "}", "(", ")", "[", "]", "MIN", "MAX", "/", "*", "+", "-" }, StringSplitOptions.None);
                        for (int i = 0; i < HiddenForm.Length; i++)
                        {
                            var attmodelid = attrmodellist.Where(p => p.Name.ToUpper() == HiddenForm[i].ToUpper()).FirstOrDefault();
                            if (attmodelid != null)
                            {
                                var formula = "{" + attmodelid.Id + "}";
                                var formula1 = HiddenForm[i];
                                input = HiddenFormula.Replace(formula1, formula);
                                HiddenFormula = input;
                            }
                        }
                        if (input != "")
                        {
                            HiddenFormula = input;
                        }
                        Guid baseform = Guid.Empty;
                        AttributeModel attrmodel = new AttributeModel();
                        if ((!string.IsNullOrEmpty(atrrname)))
                            attrmodel = attrmodellist.Where(f => f.Name.ToUpper() == atrrname.ToUpper()).FirstOrDefault();

                        TaxBehavior taxbehaviour = new TaxBehavior();
                        taxbehaviour.FinanceYearId = financeYrID;
                        if (attrmodel != null)
                            taxbehaviour.AttributemodelId = attrmodel.Id;
                        var TXFormula = Convert.ToString(dtTaxBhaviour.Rows[count]["Formula"]);
                        if ((!string.IsNullOrEmpty(Convert.ToString(TXFormula))) && TXFormula != "Null")
                            taxbehaviour.Value = Convert.ToString(dtTaxBhaviour.Rows[count]["Formula"]);

                        if ((!string.IsNullOrEmpty(Convert.ToString(HiddenFormula))) && HiddenFormula != "Null")
                            taxbehaviour.Formula = Convert.ToString(HiddenFormula);

                        var TXTYPE = Convert.ToString(dtTaxBhaviour.Rows[count]["TYPE"]);
                        if ((!string.IsNullOrEmpty(Convert.ToString(TXTYPE))) && TXTYPE != "Null")
                            taxbehaviour.InputType = Convert.ToInt32(dtTaxBhaviour.Rows[count]["TYPE"]);

                        var TXFieldFor = Convert.ToString(dtTaxBhaviour.Rows[count]["Field For"]);
                        if ((!string.IsNullOrEmpty(Convert.ToString(TXFieldFor))) && TXFieldFor != "Null")
                            taxbehaviour.FieldFor = Convert.ToString(dtTaxBhaviour.Rows[count]["Field For"]);

                        var TXBaseValue = Convert.ToString(dtTaxBhaviour.Rows[count]["Base Value"]);
                        if ((!string.IsNullOrEmpty(Convert.ToString(TXBaseValue))) && TXBaseValue != "Null")
                        {
                            taxbehaviour.BaseValue = Convert.ToString(dtTaxBhaviour.Rows[count]["Base Value"]);
                            var baseValue = attrmodellist.Where(d => d.Name == TXBaseValue).FirstOrDefault();
                            baseform = baseValue.Id;
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(baseform)))
                            taxbehaviour.BaseFormula = Convert.ToString(baseform);

                        taxbehaviour.CreatedOn = DateTime.Now;
                        taxbehaviour.CreatedBy = userId;

                        taxbehaviour.IsDeleted = false;

                        TaxBehaviourlist.Add(taxbehaviour);
                    }
                    TaxBehaviourlist.ForEach(txbehv =>
                    {
                        txbehv.Save();
                    });

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
    }
}