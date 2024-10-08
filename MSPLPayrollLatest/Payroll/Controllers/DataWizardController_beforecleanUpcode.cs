using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollBO;
using PayRollReports;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel.DataAnnotations;
using Payroll.Controllers.Tax;
using TraceError;
using System.Web.Hosting;
using System.Configuration;
using Payroll.CustomFilter;

namespace Payroll.Controllers
{
    [SessionExpireAttribute]
    public class DataWizardController : BaseController
    {
        int DetailStartIndex = 1;
        int CellCount = 0;
        int MasterEndIndex = 0;
        bool ByGroup = true;
        bool IsDetail = true;
        GridView GridView1 = new GridView();
        // GET: DataWizard
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult saveSetting(jsonPaySlipSetting setting, List<jsonPaySlipattributes> attr)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            bool result = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            var CategoryCheck = attr.Where(w => w.tableName == "Category" && w.type == "Category").ToList();
            CategoryList Categorylist = new CategoryList(companyId);
            Category categoryCheck = new Category();
            PayslipsettingList catcount = new PayslipsettingList();                                         //
            var FieldNameNull = "1";                                                                             //
            PaySlipSetting settingcatcount = new PaySlipSetting();                                              //
            PayslipsettingList Setting = new PayslipsettingList(setting.CofigurationId);
            PaySlipSetting objSetting = jsonPaySlipSetting.convertobject(setting);
            PayslipsettingList catSetting = new PayslipsettingList(companyId);                                   //To get the configID from company in payslipattr.
            var catname1 = catSetting.Where(j => j.TableName == "Category" && j.Type == "Category").ToList();    //To get the catergory name from company.
            if (setting.CofigurationId == Guid.Empty && setting.description != null && setting.title != null)    //forsaving  new payslip.(changed OR to AND in title)
            {
                for (var a = 0; a < CategoryCheck.Count; a++)
                {
                    //var catcount1 = catname1.Where(q => q.CompanyId == companyId && q.FieldName == CategoryCheck[a].fieldName).ToList();
                    catcount.Add(catname1.Where(q => q.CompanyId == companyId && q.FieldName == CategoryCheck[a].fieldName).FirstOrDefault());
                }
                for (int cnt = 0; cnt < catcount.Count; cnt++)
                {
                    var ccc = catcount[cnt];
                    if (ccc != null)
                    {
                        if (!string.IsNullOrEmpty(catcount[cnt].FieldName))
                        {
                            FieldNameNull = "0";
                        }
                    }
                }
                if (FieldNameNull.ToString() != "0")
                {
                    isSaved = false;
                    objSetting.CompanyId = companyId;
                    isSaved = objSetting.Save();
                }
                else
                {
                    return base.BuildJson(false, 100, "category name already exist.", setting);
                }
            }
            else
            {
                isSaved = false;
                objSetting.CompanyId = companyId;
                isSaved = objSetting.Save();
            }
            PaySlipAttributeList Allpayslipattrlist = new PaySlipAttributeList();
            var CheckList = Setting.Where(k => k.CompanyId == companyId).ToList();
            if (CheckList.Count != 0)
            {
                for (var i = 0; i < CheckList.Count; i++)
                {
                    PaySlipAttributeList payslipattrlist = new PaySlipAttributeList(CheckList[i].ConfigurationId);
                    PaySlipAttributes Payslipattr = new PaySlipAttributes(CheckList[i].ConfigurationId);
                    var CategoryAttrCheck = payslipattrlist.Where(a => a.TableName == "Category" && a.Type == "Category").ToList();
                    if (payslipattrlist.Count > 0 && Payslipattr != null)
                    {
                        for (var k = 0; k < CategoryCheck.Count; k++)
                        {
                            Payslipattr = CategoryAttrCheck.Where(A => A.FieldName == CategoryCheck[k].fieldName).FirstOrDefault();
                            if (Payslipattr != null)
                            {
                                Allpayslipattrlist.Add(Payslipattr);
                                isSaved = true;
                            }
                        }
                    }
                }
            }
            if (isSaved)
            {
                string Categoryname = "";
                if (Allpayslipattrlist.Count > 0)
                {

                    for (var j = 0; j < Allpayslipattrlist.Count; j++)
                    {
                        categoryCheck = Categorylist.Where(c => c.Id == new Guid(Convert.ToString((Allpayslipattrlist[j].FieldName)))).FirstOrDefault();
                        Categorylist.Add(categoryCheck);
                        if (categoryCheck != null && Categoryname != "")
                        {
                            Categoryname = Categoryname + "," + categoryCheck.Name;
                        }
                        else
                        {
                            Categoryname = categoryCheck.Name;
                        }
                    }
                    if (Allpayslipattrlist.Count == 0 && Categoryname != "")
                    {
                        PaySlipAttributes objattr = new PaySlipAttributes();
                        objattr.Delete(objSetting.ConfigurationId);
                        attr.ForEach(a =>
                        {
                            a.CofigurationId = objSetting.ConfigurationId;
                            PaySlipAttributes atrr = jsonPaySlipattributes.convertobject(a);
                            atrr.Save();
                            result = true;
                        }
                        );
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    PaySlipAttributes objattr = new PaySlipAttributes();
                    objattr.Delete(objSetting.ConfigurationId);
                    attr.ForEach(a =>
                    {
                        a.CofigurationId = objSetting.ConfigurationId;
                        PaySlipAttributes atrr = jsonPaySlipattributes.convertobject(a);
                        atrr.Save();
                        result = true;
                    }
                    );
                }
                if (result)
                {
                    return base.BuildJson(true, 200, "Data saved successfully", setting);
                }
                else
                {
                    //created by suriyaprakash
                    PayslipsettingList counttcat = new PayslipsettingList();
                    PayslipsettingList catgeoryListcat = new PayslipsettingList(setting.CofigurationId, companyId);      //Category ID of Remaining configurationID in Company
                    for (var o = 0; o < CategoryCheck.Count; o++)
                    {
                        //var catcount1 = catname1.Where(q => q.CompanyId == companyId && q.FieldName == CategoryCheck[a].fieldName).ToList();
                        counttcat.AddRange(catgeoryListcat.Where(w => Convert.ToString(w.Id) == Convert.ToString((CategoryCheck[o].fieldName))).ToList());   //Comparing the catesgoryID
                    }
                    if (counttcat.Count == 0)
                    {

                        //DeleteSetting(objSetting.ConfigurationId);
                        //Created by AjithPanner on 8/11/2017
                        PaySlipAttributes objattr = new PaySlipAttributes();
                        objattr.Delete(objSetting.ConfigurationId);
                        attr.ForEach(a =>
                        {
                            a.CofigurationId = objSetting.ConfigurationId;
                            PaySlipAttributes atrr = jsonPaySlipattributes.convertobject(a);
                            atrr.Save();
                            result = true;
                        }
                        );

                    }
                    else
                    {
                        return base.BuildJson(false, 100, "Category name already Exist.", setting);
                    }
                    return base.BuildJson(true, 200, "Payslip for " + Categoryname + " Updated Successfully ", setting);
                    //return base.BuildJson(false, 100, "Payslip for " + Categoryname + " already exist ", setting);
                }


            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", setting);
            }


        }
        //Created by Keerthika on 14/06/2017
        public JsonResult DeleteSetting(Guid configId)
        {
            bool isDeleted = false;
            bool isDeleteStatus = false;
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            PaySlipAttributes payslipattributes = new PaySlipAttributes();
            isDeleted = payslipattributes.Delete(configId);
            if (isDeleted)
            {
                PaySlipSetting payslipsetting = new PaySlipSetting();
                isDeleteStatus = payslipsetting.Delete(configId);
                if (isDeleteStatus)
                {
                    return base.BuildJson(true, 200, "Data Deleted successfully", isDeleteStatus);
                }
                else
                {
                    return base.BuildJson(false, 100, "There is some error while Delete the data.", isDeleteStatus);
                }

            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while Delete the data.", isDeleteStatus);
            }

        }
        public JsonResult GetSettings(Guid id)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            PayslipsettingList setting = new PayslipsettingList(id);
            //PaySlipSetting setting = new PaySlipSetting(id);
            List<PaySlipSetting> Company_setting = setting.Where(d => d.CompanyId == companyId).ToList();

            List<jsonPaySlipSetting> jsetting = new List<jsonPaySlipSetting>();
            Company_setting.ForEach(u => { jsetting.Add(jsonPaySlipSetting.tojson(u)); });


            PaySlipAttributeList attr = new PaySlipAttributeList(id);
            List<jsonPaySlipattributes> jattr = new List<jsonPaySlipattributes>();
            AttributeModelList mdl = new AttributeModelList(companyId);
            attr.ForEach(u =>
            {
                if (u.Type == "Earnings" || u.Type == "Deductions")
                {
                    u.DisplayAs = !string.IsNullOrWhiteSpace(u.DisplayAs) ? u.DisplayAs : mdl.Where(m => m.Id.ToString() == u.FieldName).FirstOrDefault().DisplayAs;
                    u.IsIncludeGrossPay = mdl.Where(m => m.Id.ToString() == u.FieldName).FirstOrDefault().IsIncludeForGrossPay;
                }

                jattr.Add(jsonPaySlipattributes.tojson(u));
            });
            //Modified by AjithPanner on 8/11/2017
            AttributeModelList attributeModelList = new AttributeModelList(companyId, Guid.Empty);
            //var eslAttribute = attributeModelList.Where(u => u.BehaviorType == "Earning" && u.IsIncludeForGrossPay == true).ToList();
            //var dslAttribute = attributeModelList.Where(u => u.BehaviorType == "Deduction" && u.IsIncludeForGrossPay == true).ToList();
            var eslAttribute = attributeModelList.Where(u => u.BehaviorType == "Earning").ToList();
            var dslAttribute = attributeModelList.Where(u => u.BehaviorType == "Deduction").ToList();
            List<jsonPaySlipattributes> jattre = new List<jsonPaySlipattributes>();
            List<jsonPaySlipattributes> jattrd = new List<jsonPaySlipattributes>();
            eslAttribute.ForEach(s =>
            {

                PaySlipAttributes newPA = new PaySlipAttributes();
                newPA.FieldName = s.Id.ToString();
                newPA.DisplayAs = s.DisplayAs;
                newPA.IsIncludeGrossPay = s.IsIncludeForGrossPay;
                jattre.Add(jsonPaySlipattributes.tojson(newPA));

            });
            dslAttribute.ForEach(s =>
            {

                PaySlipAttributes newPAd = new PaySlipAttributes();
                newPAd.FieldName = s.Id.ToString();
                newPAd.DisplayAs = s.DisplayAs;
                newPAd.IsIncludeGrossPay = s.IsIncludeForGrossPay;
                jattrd.Add(jsonPaySlipattributes.tojson(newPAd));

            });

            var a = jattr.Where(j => j.type == "Earnings").ToList();
            var b = jattre.ToList();
            b.RemoveAll(x => a.Any(y => y.fieldName == x.fieldName));
            var addearn = a.Concat(b);

            var c = jattr.Where(j => j.type == "Deductions").ToList();
            var e = jattrd.ToList();
            e.RemoveAll(x => c.Any(y => y.fieldName == x.fieldName));
            var adddeduc = c.Concat(e);
            //new

            return base.BuildJson(true, 200, "success", new
            {
                setting = jsetting,
                attrCategory = jattr.Where(j => j.type == "Category").ToList(),
                //attrEarnings = jattr.Where(j => j.type == "Earnings").ToList(),
                attrEarnings = addearn.ToList(),
                //attrDeductions = jattr.Where(j => j.type == "Deductions").ToList(),
                attrDeductions = adddeduc.ToList(),
                attrMaster = jattr.Where(j => j.type == "Master").ToList()
            });
        }

        public JsonResult GetMasterFields(string tableName)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            List<PaySlipAttributes> ps = PaySlip.GetMasterFields(tableName, companyId);
            List<jsonPaySlipattributes> jattr = new List<jsonPaySlipattributes>();
            ps.ForEach(p =>
            {
                jattr.Add(jsonPaySlipattributes.tojson(p));
            });
            return new JsonResult { Data = jattr };
        }
        public JsonResult GetEarningDeductionFields(string behaviorType, string IsPaysheet)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            AttributeModelList attributeModelList = new AttributeModelList(companyId, Guid.Empty);
            EntityModel entity = new EntityModel("salary", companyId);
            var slAttribute = attributeModelList.Where(u => u.BehaviorType == behaviorType).ToList();
            AttributeModelList behaviourTypeBased = new AttributeModelList();
            if (IsPaysheet == "Payslip")
            {
                //entity.EntityAttributeModelList.ForEach(x =>
                //{
                //    if (x.AttributeModel.BehaviorType == behaviorType)
                //        behaviourTypeBased.Add(x.AttributeModel);
                //});  
                // slAttribute = attributeModelList.Where(u => u.BehaviorType == behaviorType && u.IsIncludeForGrossPay == true).ToList();
            }
            List<jsonPaySlipattributes> jattr = new List<jsonPaySlipattributes>();
            slAttribute.ForEach(s =>
            {

                PaySlipAttributes newPA = new PaySlipAttributes();
                newPA.FieldName = s.Id.ToString();
                newPA.DisplayAs = s.DisplayAs;
                newPA.IsIncludeGrossPay = s.IsIncludeForGrossPay;
                jattr.Add(jsonPaySlipattributes.tojson(newPA));

            });
            return new JsonResult { Data = jattr };
        }
        //modified by Ajithpanner on 20/11/17
        public static void GetPaySlip(PayrollHistory payhis, PaySlipList payslipattr, string PDFFilePath, int month, int year, bool displayCumulative, int companyId, int userId)
        {
            //Remove UnMappped fields from payslip Modified on 04/11/2018 Muthu
            foreach (var item in payslipattr.ToList())
            {
                if (item.Value1 == null)
                {
                    payslipattr.Remove(payslipattr.Where(y => y.FieldName == item.FieldName).FirstOrDefault());
                }

            }

            AttributeModelList attmod = new AttributeModelList(companyId);

            var NetPay = attmod.Where(a => a.Name == "NETPAY").FirstOrDefault();
            var EarnedGross = attmod.Where(b => b.Name == "EG").FirstOrDefault();
            var TotalDeduction = attmod.Where(c => c.Name == "TOTDED").FirstOrDefault();
            Company comp = new Company(companyId, userId);

            string title = string.Empty;
            title = comp.CompanyName + "*";
            if (comp.AddressLine1.Trim() != string.Empty)
            {
                title = title + comp.AddressLine1.Trim() + ",";
            }
            if (comp.AddressLine2.Trim() != string.Empty)
            {
                title = title + comp.AddressLine2.Trim() + ",";
            }
            if (comp.City.Trim() != string.Empty)
            {
                title = title + comp.City.Trim() + ",";
            }
            if (!String.IsNullOrEmpty(comp.PinCode))
            {
                title = title.TrimEnd(',') + "- " + comp.PinCode.Trim() + ",";
            }
            title = title.TrimEnd(',');
            title = title + "**" + "PAYSLIP FOR THE MONTH OF " + ((MonthEnum)Convert.ToInt16(month)).ToString().ToUpper() + " - " + year;
            var netpayValue = payhis.PayrollHistoryValueList.Where(d => d.AttributeModelId == NetPay.Id).ToList();
            var netpayVal = netpayValue[0].Value;
            var egValue = payhis.PayrollHistoryValueList.Where(d => d.AttributeModelId == EarnedGross.Id).ToList();
            var egVal = egValue[0].Value;
            var tdValue = payhis.PayrollHistoryValueList.Where(d => d.AttributeModelId == TotalDeduction.Id).ToList();
            var tdVal = tdValue[0].Value;

            payslipattr.ForEach(u =>
            {
                if (u.FieldName == "Gender")
                {
                    if (u.Value1 == "1")
                    {
                        u.Value1 = "Male";
                    }
                    else
                    {
                        u.Value1 = "Female";
                    }
                }
                if (u.Value1 == "01/Jan/0001")
                {
                    u.Value1 = "";
                }

            });
            payslipattr.ToList().ForEach(p =>
            {
                if ((string.IsNullOrEmpty(p.Value1) || p.Value1 == "0") && (p.Section == "Deductions" || p.Section == "Earnings"))
                {
                    payslipattr.Remove(p);
                }
            });
            var EarningCount = payslipattr.Where(x => x.Section == "Earnings").ToList().Count;
            var DeductionsCount = payslipattr.Where(x => x.Section == "Deductions").ToList().Count;
            int hCount = Convert.ToInt32(payslipattr.Where(u => u.Section == "Header").Count() % 2);
            int fCount = Convert.ToInt32(payslipattr.Where(u => u.Section == "Footer").Count() % 2);
            if (hCount > 0)
            {
                Payattr emptyObj = new Payattr();
                emptyObj.Section = "Header";
                emptyObj.TableName = "";
                emptyObj.DisplayOrder = payslipattr.Where(x => x.Section == "Header").ToList().OrderByDescending(x => x.DisplayOrder).FirstOrDefault().DisplayOrder + 1;
                payslipattr.Add(emptyObj);
            }
            if (fCount > 0)
            {
                Payattr emptyObj = new Payattr();
                emptyObj.Section = "Footer";
                emptyObj.TableName = "";
                emptyObj.DisplayOrder = payslipattr.Where(x => x.Section == "Footer").ToList().OrderByDescending(x => x.DisplayOrder).FirstOrDefault().DisplayOrder + 1;
                payslipattr.Add(emptyObj);
            }
            if (EarningCount > DeductionsCount)
            {
                int tempCount = payslipattr.Where(x => x.Section == "Deductions").ToList().OrderByDescending(x => x.DisplayOrder).FirstOrDefault().DisplayOrder + 1;
                for (int i = 1; i <= EarningCount - DeductionsCount; i++)
                {
                    Payattr emptyObj = new Payattr();
                    emptyObj.Section = "Deductions";
                    emptyObj.TableName = "";
                    emptyObj.DisplayOrder = tempCount + i;
                    payslipattr.Add(emptyObj);
                }
            }
            if (EarningCount < DeductionsCount)
            {
                int tempCount = payslipattr.Where(x => x.Section == "Earnings").ToList().OrderByDescending(x => x.DisplayOrder).FirstOrDefault().DisplayOrder + 1;
                if (!ReferenceEquals(tempCount, null))
                {
                    for (int i = 1; i <= DeductionsCount - EarningCount; i++)
                    {
                        Payattr emptyObj = new Payattr();
                        emptyObj.Section = "Earnings";
                        emptyObj.TableName = "";
                        emptyObj.DisplayOrder = tempCount + i;
                        payslipattr.Add(emptyObj);
                    }
                }
            }

            DataTable dt = CommonData.ConvertListToTable(payslipattr);
            //need to work
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



            rpt.EnableExternalImages = true;
            string imagePath = string.IsNullOrEmpty(comp.Companylogo) ? "" : new Uri(HostingEnvironment.MapPath(comp.Companylogo)).AbsoluteUri;
            ReportDataSource rptDs = new ReportDataSource("DSPaySlip", dt);
            rpt.DataSources.Add(rptDs);
            ReportParameterCollection rpcollection = new ReportParameterCollection();
            rpcollection.Add(new ReportParameter("Title", title));
            rpcollection.Add(new ReportParameter("HeaderLimit", ((payslipattr.Where(u => u.Section == "Header").Count() / 2)).ToString()));
            rpcollection.Add(new ReportParameter("FooterLimit", ((payslipattr.Where(u => u.Section == "Footer").Count() / 2)).ToString()));
            rpcollection.Add(new ReportParameter("NetPay", netpayVal));
            rpcollection.Add(new ReportParameter("EarnedGross", egVal));
            rpcollection.Add(new ReportParameter("TotalDeduction", tdVal));
            rpcollection.Add(new ReportParameter("ImagePath", imagePath));
            if (!displayCumulative)
            {
                rpcollection.Add(new ReportParameter("ThirdColumn", string.Empty));
                rpt.ReportPath = "Reports/PaySlip.rdlc";
            }
            else
            {
                rpcollection.Add(new ReportParameter("ThirdColumn", "show"));
                rpt.ReportPath = "Reports/PaySlipWithCumulative.rdlc";
            }
            rpt.SetParameters(rpcollection);
            rpt.Refresh();
            byte[] renderedBytes = null;

            renderedBytes = rpt.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);



            string contentype = mimeType;


            using (FileStream fs = System.IO.File.Create(PDFFilePath))
            {
                fs.Write(renderedBytes, 0, (int)renderedBytes.Length);
            }

            //{
            //    contentype = "application/zip";
            //    outFilePath = DocumentProcessingSettings.TempDirectoryPath + "PaySlip.zip";
            //    ZipPath(outFilePath, PDFFilePath, null, true, null);
            //}


            //  DownLoadController dowload = new DownLoadController();
            // return dowload.GetResponse(outFilePath, contentype);

        }

        bool MonthWiseReport = false;
        public JsonResult GetPaySheet(List<jsonPaySheetattr> paysheetattr, string category, string title, int smonth, int? syear, int nMonth, int? nYear, bool isDetail, string[] groupby, List<jsonPaySheetattr> filters)
        {
            string[] categories = category.TrimEnd(',').Split(',');
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            IsDetail = isDetail;
            // Entity entity;
            string outfilePath = DocumentProcessingSettings.TempDirectoryPath;
            PaySlip payslip = new PaySlip();
            List<Employee> emp = new List<Employee>();
            List<Paysheetatrr> ps = new List<Paysheetatrr>();
            Dictionary<string, string> filterExpr = new Dictionary<string, string>();
            if (isDetail) MonthWiseReport = true;
            //if (smonth != nMonth && nYear != syear)
            //{
            //    if (groupby == null)
            //    {
            //        MonthWiseReport = true;
            //        paysheetattr.Insert(0, new jsonPaySheetattr { fieldName = "month", displayAs = "Month", type = "Group", tableName = "" });

            //        groupby = new string[1];
            //        groupby[0] = "month";
            //    }
            //    else
            //    {
            //        /* if (!groupby.Contains("month"))
            //         {
            //             Array.Resize(ref groupby, groupby.Count() + 1);
            //             groupby[groupby.Count() - 1] = "month";
            //         }*/
            //    }

            //}
            if (filters != null)
            {
                filterExpr = GetFilterExpr(companyId, filters);
            }
            if (groupby == null || !isDetail)
            {
                ByGroup = false;
            }
            if (groupby != null && groupby.Count() > 0)
            {
                DetailStartIndex = 0;
            }
            List<Paysheetatrr> columnHeader = new List<Paysheetatrr>();

            var check = paysheetattr.Where(d => d.fieldName.ToLower() == "employeecode").ToList();
            if (check == null)
            {
                paysheetattr.Insert(smonth == nMonth && syear == nYear ? 0 : 1, new jsonPaySheetattr { fieldName = "employeecode", displayAs = "Emp Code" });
            }
            check = paysheetattr.Where(d => d.fieldName.ToLower() == "firstname").ToList();
            if (check == null)
            {
                paysheetattr.Insert(smonth == nMonth && syear == nYear ? 1 : 2, new jsonPaySheetattr { fieldName = "firstname", displayAs = "Emp Name" });
            }

            if (IsDetail)
            {
                paysheetattr.ForEach(a =>
                {
                    if (a.type.ToLower() == "detail")
                    {
                        DateTime startDate = new DateTime((int)syear, smonth, 1);// DateTime.Parse("8/13/2010 8:33:21 AM");
                        DateTime endDate = new DateTime((int)nYear, nMonth, 1);
                        string tempCol = a.displayAs;
                        string fieldName = tempCol;
                        do
                        {
                            a.displayAs = fieldName + "(" + (MonthEnum)startDate.Month + "_" + startDate.Year + ")";
                            columnHeader.Add(jsonPaySheetattr.convertobject(a));
                            startDate = startDate.AddMonths(1);
                        }
                        while (startDate <= endDate);

                    }
                    else
                    {
                        columnHeader.Add(jsonPaySheetattr.convertobject(a));
                    }

                });
            }
            else
            {
                paysheetattr.ForEach(a =>
                {
                    columnHeader.Add(jsonPaySheetattr.convertobject(a));
                });
            }



            //Initialize Column Header
            GenerateDataView paysheetDataView = new GenerateDataView(columnHeader, isDetail);
            CellCount = paysheetDataView.PaySheetDataView.Columns.Count;
            total = new decimal[CellCount];

            TotEmpCount = 0;
            for (int i = 0; i < paysheetDataView.PaySheetDataView.Columns.Count; i++)
            {
                if (paysheetDataView.PaySheetDataView.Columns[i].Caption != "Emp Code")
                {
                    paysheetattr.ForEach(a =>
                    {
                        if (paysheetDataView.PaySheetDataView.Columns[i].Caption == (a.displayAs == null ? a.fieldName : a.displayAs))
                        {
                            if (a.type == "Group")
                            {
                                MasterEndIndex = i;
                                currentId.Add("");
                            }

                            if (a.type == "Detail")
                            {

                                DetailStartIndex = i;

                            }
                        }

                    });
                }
                else
                {
                    DetailStartIndex = i;
                }
                if (DetailStartIndex >= 1)
                {
                    break;
                }

            }


            foreach (string st in categories)
            {
                emp.AddRange((new EmployeeList(companyId, new Guid(st), (from k in filterExpr where string.Compare(k.Key, "employee", true) == 0 select k.Value).FirstOrDefault(), userId, new Guid(Convert.ToString(Session["EmployeeGUID"])))));
            }

            CategoryList categoryList = new CategoryList(companyId);
            DepartmentList deptList = new DepartmentList(companyId);
            BranchList branchList = new BranchList(companyId);
            CostCentreList costCentreList = new CostCentreList(companyId);
            DesignationList desgntionList = new DesignationList(companyId);
            EsiLocationList esiLocationList = new EsiLocationList(companyId);
            GradeList gradeList = new GradeList(companyId);
            PTLocationList ptLocList = new PTLocationList(companyId);
            LocationList locationlist = new LocationList(companyId);
            ESIDespensaryList esidispensarylist = new ESIDespensaryList(companyId);
            BankList banklist = new BankList(companyId);
            PayrollHistoryList history = new PayrollHistoryList();
            if (syear != null && nYear != null)
                history = new PayrollHistoryList(companyId, syear.Value, smonth, nYear.Value, nMonth, Guid.Empty);
            PayrollHistoryList payrollHistory = new PayrollHistoryList();
            payrollHistory.AddRange(history.OrderBy(p => p.Month).ToList());
            EntityModel entityModel = new EntityModel("AddtionalInfo", companyId);


            emp.ForEach(e =>
            {
                bool nopay = false;
                List<PayrollHistory> pay = payrollHistory.Where(p => p.EmployeeId == e.Id).ToList();
                if (pay == null || pay.Count == 0)
                {
                    pay = new List<PayrollHistory>();
                    pay.Add(new PayrollHistory());
                    nopay = true;
                }
                if (pay.Count == 0)
                {

                }

                // pay.ForEach(payHistory =>
                //  {


                bool continueList = true;
                string bankfilter = (from k in filterExpr where string.Compare(k.Key, "emp_bank", true) == 0 select k.Value).FirstOrDefault();
                string addrfilter = (from k in filterExpr where string.Compare(k.Key, "emp_Address", true) == 0 select k.Value).FirstOrDefault();
                string personalFilter = (from k in filterExpr where string.Compare(k.Key, "emp_personal", true) == 0 select k.Value).FirstOrDefault();
                List<PayrollError> payErrors = new List<PayrollError>();

                Emp_BankList empbank = new Emp_BankList(e.Id, bankfilter);
                EmployeeAddressList empaddr = new EmployeeAddressList(e.Id, addrfilter);
                Emp_Personal emppersonal = new Emp_Personal(e.Id, personalFilter);

                EntityAdditionalInfoList empAddInfoList = new EntityAdditionalInfoList(companyId, entityModel.Id, e.Id);

                if (!string.IsNullOrEmpty(bankfilter))
                {
                    if (empbank.Count == 0)
                    {
                        continueList = false;
                    }
                }
                if (!string.IsNullOrEmpty(addrfilter))
                {
                    if (empaddr.Count == 0)
                    {
                        continueList = false;
                    }
                }
                if (!string.IsNullOrEmpty(personalFilter))
                {
                    if (object.ReferenceEquals(emppersonal, null))
                    {
                        continueList = false;
                    }
                }
                if (continueList)
                {
                    //if (!object.ReferenceEquals(payHistory, null) || DetailStartIndex == 0)
                    //{
                    columnHeader.ForEach(a =>
                    {

                        //if (payHistory.Status == ComValue.payrollProcessStatus[0] || DetailStartIndex == 0 || nopay)
                        //{

                        Paysheetatrr newps = new Paysheetatrr();
                        newps.EmpCode = e.EmployeeCode;
                        newps.EmpName = e.FirstName + " " + e.LastName;
                        //if (a.fieldName.ToLower() == "month")
                        //{
                        //    newps.Value = Convert.ToString((MonthEnum)payHistory.Month);
                        //    newps.DisplayAs = "Month";
                        //}
                        //Assign Master Values from Physical Table
                        switch (Convert.ToString(a.TableName).ToLower())
                        {
                            case "category":
                                newps.DisplayAs = string.IsNullOrEmpty(a.DisplayAs) ? a.FieldName : a.DisplayAs;
                                if (categoryList.Count > 0)
                                {

                                    newps.Value = categoryList.Where(c => c.Id == e.CategoryId).FirstOrDefault().Name;
                                    newps.OrderBy = a.OrderBy;
                                    newps.Type = a.Type;
                                }
                                break;
                            case "employee":

                                newps.DisplayAs = string.IsNullOrEmpty(a.DisplayAs) ? a.FieldName : a.DisplayAs;
                                var type = e.GetType();
                                newps.Value = Convert.ToString(e.GetType().GetProperty(a.FieldName).GetValue(e, null));
                                if (a.FieldName.ToLower() == "gender")
                                {
                                    newps.Value = (newps.Value == "1" ? GenderEnum.Male.ToString() : GenderEnum.Female.ToString());
                                }

                                if (e.GetType().GetProperty(a.FieldName).PropertyType.Name == "Guid")
                                {
                                    switch (a.FieldName)
                                    {
                                        case "CategoryId":
                                            newps.Value = categoryList.Where(c => c.Id == new Guid(newps.Value)).FirstOrDefault() == null ? "" : categoryList.Where(c => c.Id == new Guid(newps.Value)).FirstOrDefault().Name;
                                            break;
                                        case "Category":
                                            newps.Value = categoryList.Where(c => c.Id == new Guid(newps.Value)).FirstOrDefault() == null ? "" : categoryList.Where(c => c.Id == new Guid(newps.Value)).FirstOrDefault().Name;
                                            break;
                                        case "Department":
                                            newps.Value = deptList.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault() == null ? "" : deptList.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault().DepartmentName;
                                            break;
                                        case "Branch":
                                            newps.Value = branchList.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault() == null ? "" : branchList.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault().BranchName;
                                            break;
                                        case "Designation":
                                            newps.Value = desgntionList.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault() == null ? "" : desgntionList.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault().DesignationName;
                                            break;
                                        case "CostCentre":
                                            newps.Value = costCentreList.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault() == null ? "" : costCentreList.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault().CostCentreName;
                                            break;
                                        case "Grade":
                                            newps.Value = gradeList.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault() == null ? "" : gradeList.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault().GradeName;
                                            break;
                                        case "ESILocation":
                                            newps.Value = esiLocationList.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault() == null ? "" : esiLocationList.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault().LocationName;
                                            break;
                                        case "PTLocation":
                                            newps.Value = ptLocList.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault() == null ? "" : ptLocList.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault().PTLocationName;
                                            break;
                                        case "Location":
                                            newps.Value = locationlist.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault() == null ? "" : locationlist.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault().LocationName;
                                            break;
                                        case "ESIDespensary":
                                            newps.Value = esidispensarylist.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault() == null ? "" : esidispensarylist.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault().ESIDespensary;
                                            break;

                                    }
                                }

                                newps.OrderBy = a.OrderBy;
                                newps.Type = a.Type;
                                break;

                            case "emp_bank":

                                newps.DisplayAs = string.IsNullOrEmpty(a.DisplayAs) ? a.FieldName : a.DisplayAs;
                                if (empbank.Count > 0)
                                {

                                    newps.Value = Convert.ToString(empbank[0].GetType().GetProperty(a.FieldName).GetValue(empbank[0], null));
                                    newps.OrderBy = a.OrderBy;
                                    newps.Type = a.Type;
                                    if (e.GetType().GetProperty(a.FieldName).PropertyType.Name == "Guid")
                                    {
                                        switch (a.FieldName)
                                        {
                                            case "BankId":
                                                newps.Value = banklist.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault() == null ? "" : banklist.Where(d => d.Id == new Guid(newps.Value)).FirstOrDefault().BankName;
                                                break;
                                        }
                                    }
                                }
                                break;
                            case "emp_personal":

                                newps.DisplayAs = string.IsNullOrEmpty(a.DisplayAs) ? a.FieldName : a.DisplayAs;
                                newps.Value = Convert.ToString(emppersonal.GetType().GetProperty(a.FieldName).GetValue(emppersonal, null));
                                newps.OrderBy = a.OrderBy;
                                newps.Type = a.Type;
                                break;
                            case "emp_address":

                                newps.DisplayAs = string.IsNullOrEmpty(a.DisplayAs) ? a.FieldName : a.DisplayAs;
                                if (empaddr.Count > 0)
                                {
                                    newps.DisplayAs = string.IsNullOrEmpty(a.DisplayAs) ? a.FieldName : a.DisplayAs;
                                    newps.Value = Convert.ToString(empaddr[0].GetType().GetProperty(a.FieldName).GetValue(empaddr[0], null));
                                    newps.Type = a.Type;
                                }
                                break;
                            case "additionalinfo":

                                newps.DisplayAs = string.IsNullOrEmpty(a.DisplayAs) ? a.FieldName : a.DisplayAs;
                                if (empAddInfoList.Count > 0)
                                {
                                    for (int cnt = 0; cnt < empAddInfoList.Count; cnt++)
                                    {

                                        if (a.FieldName == empAddInfoList[cnt].AttributeModelId.ToString())
                                        {

                                            AttributeModel at = new AttributeModel(empAddInfoList[cnt].AttributeModelId, companyId);
                                            newps.Value = empAddInfoList[cnt].Value;
                                            newps.DisplayAs = string.IsNullOrEmpty(a.DisplayAs) ? a.FieldName : a.DisplayAs;
                                            newps.DisplayAs = string.IsNullOrEmpty(a.DisplayAs) ? at.DisplayAs : a.DisplayAs;
                                            newps.Type = a.Type;
                                        }
                                    }
                                }
                                break;
                        }
                        if (a.Type == "Master" || a.Type.ToLower() == "group")
                        {
                            if (a.DisplayAs == newps.DisplayAs) ps.Add(newps);
                        }
                        else
                        {
                            decimal attVal = 0;
                            pay.ForEach(payHistory =>
                            {

                                if (!object.ReferenceEquals(payHistory, null) || DetailStartIndex == 0)
                                {
                                    if (payHistory.Status == ComValue.payrollProcessStatus[0] || DetailStartIndex == 0 || nopay)
                                    {
                                        if (a.FieldName.ToLower() == "month")
                                        {
                                            newps.Value = Convert.ToString((MonthEnum)payHistory.Month);
                                            newps.DisplayAs = "Month";
                                        }

                                        //Assign payroll History                       
                                        for (int cnt = 0; cnt < payHistory.PayrollHistoryValueList.Count; cnt++)
                                        {

                                            if (a.FieldName == payHistory.PayrollHistoryValueList[cnt].AttributeModelId.ToString())
                                            {
                                                string temp = string.IsNullOrEmpty(a.DisplayAs) ? a.DisplayAs : a.DisplayAs;
                                                var strTemp = temp.Split('(');
                                                string tempDisplayAs = strTemp[0];

                                                AttributeModel at = new AttributeModel(payHistory.PayrollHistoryValueList[cnt].AttributeModelId, companyId);
                                                Paysheetatrr newps1 = new Paysheetatrr();
                                                newps1.EmpCode = e.EmployeeCode;
                                                newps1.EmpName = e.FirstName + " " + e.LastName;
                                                newps1.Type = a.Type;
                                                newps1.Month = payHistory.Month;
                                                newps1.Year = payHistory.Year;
                                                if (IsDetail)
                                                {
                                                    newps1.DisplayAs = string.IsNullOrEmpty(tempDisplayAs) ? at.DisplayAs + "(" + (MonthEnum)payHistory.Month + "_" + payHistory.Year + ")" : tempDisplayAs + "(" + (MonthEnum)payHistory.Month + "_" + payHistory.Year + ")";
                                                    newps1.Value = payHistory.PayrollHistoryValueList[cnt].Value;
                                                }
                                                else
                                                {
                                                    newps1.DisplayAs = string.IsNullOrEmpty(tempDisplayAs) ? at.DisplayAs : tempDisplayAs;
                                                    attVal = Convert.ToDecimal(string.IsNullOrEmpty(payHistory.PayrollHistoryValueList[cnt].Value) ? 0 : Convert.ToDecimal(payHistory.PayrollHistoryValueList[cnt].Value));
                                                    newps1.Value = Convert.ToString(attVal);
                                                    ps.Add(newps1);
                                                }

                                                if (!object.ReferenceEquals(filters, null) && filters.Any(f => f.fieldName == a.FieldName))
                                                {
                                                    switch (filters.Where(f => f.fieldName == a.FieldName).FirstOrDefault().operation)
                                                    {
                                                        case ">":
                                                            if (Convert.ToDecimal(newps1.Value) > Convert.ToDecimal(filters.Where(f => f.fieldName == a.FieldName).FirstOrDefault().value))
                                                            {
                                                                continueList = true;
                                                            }
                                                            else { continueList = false; }
                                                            break;
                                                        case "<":
                                                            if (Convert.ToDecimal(newps1.Value) < Convert.ToDecimal(filters.Where(f => f.fieldName == a.FieldName).FirstOrDefault().value))
                                                            {
                                                                continueList = true;
                                                            }
                                                            else { continueList = false; }
                                                            break;
                                                        case "<=":
                                                            if (Convert.ToDecimal(newps1.Value) <= Convert.ToDecimal(filters.Where(f => f.fieldName == a.FieldName).FirstOrDefault().value))
                                                            {
                                                                continueList = true;
                                                            }
                                                            else { continueList = false; }
                                                            break;
                                                        case ">=":
                                                            if (Convert.ToDecimal(newps1.Value) >= Convert.ToDecimal(filters.Where(f => f.fieldName == a.FieldName).FirstOrDefault().value))
                                                            {
                                                                continueList = true;
                                                            }
                                                            else { continueList = false; }
                                                            break;
                                                        case "<>":
                                                            if (Convert.ToDecimal(newps1.Value) != Convert.ToDecimal(filters.Where(f => f.fieldName == a.FieldName).FirstOrDefault().value))
                                                            {
                                                                continueList = true;
                                                            }
                                                            else { continueList = false; }
                                                            break;
                                                        default:
                                                            if (Convert.ToDecimal(newps1.Value) == Convert.ToDecimal(filters.Where(f => f.fieldName == a.FieldName).FirstOrDefault().value))
                                                            {
                                                                continueList = true;
                                                            }
                                                            else { continueList = false; }
                                                            break;

                                                    }

                                                }
                                                if (IsDetail)
                                                {
                                                    if (a.DisplayAs == newps1.DisplayAs)
                                                    {
                                                        if (ps.Where(x => x.EmpCode == newps.EmpCode && x.DisplayAs == newps1.DisplayAs).Count() == 0)
                                                            ps.Add(newps1);
                                                    }
                                                }
                                                if (!continueList)
                                                {
                                                    ps.Remove(newps1);
                                                }
                                            }
                                        }

                                    }
                                }
                            });
                            //if (continueList)
                            //{
                            //    ps.Add(newps);
                            //}
                        }
                    });//End attr

                    //Add Row
                    if (continueList && ps.Count > 0)
                    {
                        paysheetDataView.AddDataRow(ps.Where(d => d.EmpCode == e.EmployeeCode).ToList(), isDetail);
                        continueList = true;
                    }
                    // payroll  history end
                }// Valid data end

            });//End emp
            List<Paysheetatrr> fields = new List<Paysheetatrr>();
            paysheetattr.ForEach(p =>
            {
                fields.Add(jsonPaySheetattr.convertobject(p));
            });

            paysheetDataView.GropingData(groupby, isDetail, fields);
            DataView view = new DataView(paysheetDataView.PaySheetDataView);
            DataTable distinctValues = new DataTable();

            if (paysheetDataView.PaySheetDataView.Columns.Contains("EmployeeCode"))
            {
                distinctValues = view.ToTable(true, "EmployeeCode");
            }


            string path = string.Empty;
            //if (smonth == nMonth && syear == nYear)
            //{
            //    if (isDetail) MonthWiseReport = true;
            //    path = generateExcel(paysheetDataView.PaySheetDataView, title + " " + (MonthEnum)smonth + " " + syear);
            //}
            //else
            //{

            int MasterCnt = paysheetattr.Where(u => u.type == "Master").ToList().Count;
            int GroupCnt = paysheetattr.Where(u => u.type == "Group").ToList().Count;

            DataTable dtFinal = new DataTable();

            if (paysheetDataView.PaySheetDataView != null && paysheetDataView.PaySheetDataView.Rows.Count > 0)
            {
                DataRow dtrow = dtFinal.NewRow();

                //for (int j = 0; j < paysheetDataView.PaySheetDataView.Columns.Count; j++)
                //{
                //    if (j < MasterCnt && j >= GroupCnt)
                //    {
                //        dtFinal.Columns.Add(paysheetDataView.PaySheetDataView.Columns[j].ColumnName.ToString());
                //    }
                //    if (j >= MasterCnt)
                //    {
                //        if (smonth >= nMonth)
                //        {

                //            int tempint = smonth > nMonth ? smonth + 1 : smonth;
                //            for (int k = smonth; k <= tempint && (syear <= nYear); k++)
                //            {
                //                for (int y = (int)syear; y <= nYear; y++)
                //                {    
                //                    if (!dtFinal.Columns.Contains(paysheetDataView.PaySheetDataView.Columns[j].ColumnName))
                //                    {
                //                        dtFinal.Columns.Add(paysheetDataView.PaySheetDataView.Columns[j].ColumnName.ToString());
                //                    }
                //                    k = k == 12 ? 0 : k;
                //                    tempint = k == 0 ? 0 : k + 1;
                //                }
                //            }

                //        }
                //        else
                //        {
                //            for (int k = smonth; k <= nMonth; k++)
                //            {                                    
                //                if (!dtFinal.Columns.Contains(paysheetDataView.PaySheetDataView.Columns[j].ColumnName))
                //                {
                //                    dtFinal.Columns.Add(paysheetDataView.PaySheetDataView.Columns[j].ColumnName.ToString());
                //                }
                //            }
                //        }

                //    }

                //}
                if (IsDetail)
                {
                    dtFinal = paysheetDataView.PaySheetDataView;
                    //for (int a = 0; a < distinctValues.Rows.Count; a++)
                    //{
                    //    dtrow = dtFinal.NewRow();
                    //    string empcode = distinctValues.Rows[a]["EmployeeCode"].ToString();
                    //    empcode = empcode.Replace("'", string.Empty);
                    //    foreach (DataRow drOutput in paysheetDataView.PaySheetDataView.Rows)
                    //    {
                    //        drOutput["EmployeeCode"] = drOutput["EmployeeCode"].ToString().Replace("'", string.Empty);
                    //    }
                    //    string filter = string.Format("EmployeeCode = '" + empcode + "'");
                    //    DataRow[] results = paysheetDataView.PaySheetDataView.Select(filter);
                    //    DataTable dataTable = new DataTable();
                    //    dataTable = paysheetDataView.PaySheetDataView.Clone();

                    //    dataTable = results.CopyToDataTable();
                    //    for (int b = 0; b < dataTable.Rows.Count; b++)
                    //    {
                    //        for (int j = 0; j < paysheetDataView.PaySheetDataView.Columns.Count; j++)
                    //        {
                    //            if (j < MasterCnt && j >= GroupCnt)
                    //            {
                    //                dtrow[paysheetDataView.PaySheetDataView.Columns[j].ColumnName.ToString()] = dataTable.Rows[b][j].ToString();
                    //            }
                    //            if (j >= MasterCnt)
                    //            {
                    //                int i = 0;
                    //                int tempint = smonth > nMonth ? smonth + 1 : smonth;
                    //                for (int k = smonth; k <= tempint; k++)
                    //                {
                    //                    for (int y = (int)syear; y <= nYear; y++)
                    //                    {
                    //                        if (i < dataTable.Rows.Count)
                    //                        {
                    //                            if (IsDetail)
                    //                            {
                    //                                if (paysheetDataView.PaySheetDataView.Columns[j].ColumnName.Contains("EmployeeCode"))
                    //                                {
                    //                                    dtrow[paysheetDataView.PaySheetDataView.Columns[j].ColumnName.ToString()] = dataTable.Rows[i][j].ToString();
                    //                                }
                    //                                else
                    //                                {
                    //                                    dtrow[paysheetDataView.PaySheetDataView.Columns[j].ColumnName.ToString()] = dataTable.Rows[i][j].ToString();
                    //                                    // dtrow[paysheetDataView.PaySheetDataView.Columns[j].ColumnName.ToString() + "(" + (MonthEnum)k + "_" + y + ")"] = dataTable.Rows[i][j].ToString();
                    //                                    //   dtrow[paysheetDataView.PaySheetDataView.Columns[j].ColumnName.ToString()] = dataTable.Rows[i][j].ToString();
                    //                                }
                    //                            }
                    //                            else
                    //                            {
                    //                                dtrow[paysheetDataView.PaySheetDataView.Columns[j].ColumnName.ToString()] = dataTable.Rows[i][j].ToString();
                    //                            }
                    //                            i++;
                    //                        }
                    //                        k = k == 12 ? 0 : k;
                    //                        tempint = k == 0 ? 0 : k + 1;
                    //                    }
                    //                }
                    //            }

                    //        }
                    //    }
                    //    dtFinal.Rows.Add(dtrow);
                    //}

                    //foreach (DataRow drOutput1 in dtFinal.Rows)
                    //{
                    //    drOutput1["EmployeeCode"] = drOutput1["EmployeeCode"].ToString().Replace(drOutput1["EmployeeCode"].ToString(), "'" + drOutput1["EmployeeCode"].ToString());
                    //}
                }
                else
                {
                    if (paysheetDataView.PaySheetDataView.Columns.Contains("month"))
                    {
                        paysheetDataView.PaySheetDataView.Columns.Remove("month");
                    }

                    dtFinal = paysheetDataView.PaySheetDataView.Clone();
                    // dtFinal.Columns.Remove("month");
                    DataRow dtrow1 = dtFinal.NewRow();
                    dtFinal.Rows.Add(dtrow1);
                    int removemastergroupcol = GroupCnt + MasterCnt;
                    for (int i = 0; i < dtFinal.Rows.Count; i++)
                    {
                        for (int j = removemastergroupcol; j < paysheetDataView.PaySheetDataView.Columns.Count; j++)
                        {
                            //  dtrow = dtFinal.NewRow();
                            var value = paysheetDataView.PaySheetDataView.AsEnumerable()
                                            .Where(x => x[paysheetDataView.PaySheetDataView.Columns[j].ColumnName] != DBNull.Value)
                                            .Sum(row => Convert.ToDecimal(row.Field<string>(paysheetDataView.PaySheetDataView.Columns[j].ColumnName)));
                            dtFinal.Rows[i][paysheetDataView.PaySheetDataView.Columns[j].ColumnName] = value;
                        }
                        for (int m = 0; m < removemastergroupcol; m++)
                        {
                            dtFinal.Rows[i][paysheetDataView.PaySheetDataView.Columns[m].ColumnName] = (paysheetDataView.PaySheetDataView.Rows[m]).ItemArray[m];
                        }
                    }
                    // MonthWiseReport = true;
                    //dtFinal = paysheetDataView.PaySheetDataView;
                }

                path = generateExcel(dtFinal, title + " " + (MonthEnum)smonth + " " + syear + " - " + (MonthEnum)nMonth + " " + nYear);
            }
            //}
            return BuildJson(true, 1, "File Saved Successfully", new { filePath = path });
        }

        private string generateExcel(DataTable dt, string title)
        {
            CellCount = dt.Columns.Count; //total = dt.Columns.Count;
            total = new decimal[CellCount];
            GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell1 = new TableCell();


            HeaderCell1.Font.Bold = true;
            HeaderCell1.ColumnSpan = 10;
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;

            HeaderRow.Cells.Add(HeaderCell1);
            Company comp = new Company(Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["UserId"]));


            GridViewRow HeaderRow1 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell2 = new TableCell();
            HeaderCell2.Text = comp.CompanyName;
            HeaderCell2.ColumnSpan = 5;
            HeaderCell2.Font.Bold = true;
            HeaderRow1.Cells.Add(HeaderCell2);

            GridViewRow HeaderRow2 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell3 = new TableCell();
            HeaderCell3.Text = comp.AddressLine1;
            HeaderCell3.ColumnSpan = 5;
            HeaderCell3.HorizontalAlign = HorizontalAlign.Left;
            HeaderRow2.Cells.Add(HeaderCell3);

            GridViewRow HeaderRow3 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell4 = new TableCell();
            HeaderCell4.Text = comp.AddressLine2;
            HeaderCell4.ColumnSpan = 5;
            HeaderCell4.HorizontalAlign = HorizontalAlign.Left;
            HeaderRow3.Cells.Add(HeaderCell4);

            GridViewRow HeaderRow4 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell5 = new TableCell();
            HeaderCell5.Text = comp.City;
            HeaderCell5.ColumnSpan = 5;
            HeaderCell5.HorizontalAlign = HorizontalAlign.Left;
            HeaderRow4.Cells.Add(HeaderCell5);

            GridViewRow HeaderRow5 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell6 = new TableCell();
            HeaderCell6.Text = "";
            HeaderCell6.Font.Bold = true;
            HeaderCell6.ColumnSpan = 5;
            HeaderCell6.HorizontalAlign = HorizontalAlign.Left;
            HeaderRow5.Cells.Add(HeaderCell6);

            GridViewRow HeaderRow6 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell7 = new TableCell();
            HeaderCell7.Text = title;
            HeaderCell7.Font.Bold = true;
            HeaderCell7.ColumnSpan = 5;
            HeaderRow6.HorizontalAlign = HorizontalAlign.Left;
            HeaderRow6.Cells.Add(HeaderCell7);



            GridViewRow HeaderRow7 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell8 = new TableCell();
            HeaderCell8.Text = "";
            HeaderCell8.Font.Bold = true;
            HeaderCell8.ColumnSpan = 5;
            HeaderRow7.HorizontalAlign = HorizontalAlign.Left;
            HeaderRow7.Cells.Add(HeaderCell8);





            GridView1.DataBound += new EventHandler(OnDataBound);
            GridView1.RowCreated += new GridViewRowEventHandler(OnRowCreated);

            GridView1.AllowPaging = false;

            GridView1.DataSource = dt;
            GridView1.DataBind();

            GridView1.Controls[0].Controls.AddAt(0, HeaderRow);
            GridView1.Controls[0].Controls.AddAt(1, HeaderRow1);
            GridView1.Controls[0].Controls.AddAt(2, HeaderRow2);
            GridView1.Controls[0].Controls.AddAt(3, HeaderRow3);
            GridView1.Controls[0].Controls.AddAt(4, HeaderRow4);
            GridView1.Controls[0].Controls.AddAt(5, HeaderRow5);
            GridView1.Controls[0].Controls.AddAt(6, HeaderRow6);
            GridView1.Controls[0].Controls.AddAt(7, HeaderRow7);
            if (GridView1.Rows.Count > 8)
            {
                for (int i = 0; i < GridView1.Rows[9].Cells.Count; i++)
                {
                    if (i > 1)
                    {
                        GridView1.Rows[9].Cells[i].Width = 100;
                    }
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Paysheet.xls");
            Response.Charset = "";

            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //Apply text style to each Row
                GridView1.Rows[i].Attributes.Add("class", "textmode");
                // GridView1.Rows[i].Attributes.Add("style", "word-wrap: break-word");
            }
            GridView1.RenderControl(hw);
            string PDFFilePath = DocumentProcessingSettings.TempDirectoryPath + "\\Paysheet" + ".xls";
            string renderedGridView = sw.ToString();
            System.IO.File.WriteAllText(PDFFilePath, renderedGridView);
            return PDFFilePath;

        }

        protected void OnDataBound(object sender, EventArgs e)
        {
            if (MonthWiseReport)
            {
                // for (int i = GridView1.Rows.Count - 1; i > 0; i--)
                //{
                //    GridViewRow row = GridView1.Rows[i];
                //    GridViewRow previousRow = GridView1.Rows[i - 1];
                //   for (int j = 0; j < row.Cells.Count; j++)
                // {
                // Command for grouping value show for all employee's row. June 07-2018
                //if (j <= MasterEndIndex)
                //{
                //    if (row.Cells[j].Text == previousRow.Cells[j].Text)
                //    {
                //        if (previousRow.Cells[j].RowSpan == 0)
                //        {
                //            if (row.Cells[j].RowSpan == 0)
                //            {
                //                previousRow.Cells[j].RowSpan += 2;
                //            }
                //            else
                //            {
                //                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                //            }
                //            row.Cells[j].Visible = false;
                //        }
                //    }

                //}
                // }
                //}

                for (int i = subTotalRowIndex; i < GridView1.Rows.Count; i++)
                {

                    TotEmpCount++;
                    EmpCount++;
                    GridViewRow row = GridView1.Rows[i];
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        if (j >= DetailStartIndex)
                        {
                            decimal val;
                            if (Decimal.TryParse(GridView1.Rows[i].Cells[j].Text, out val))
                            {
                                subTotal[j] += Convert.ToDecimal(GridView1.Rows[i].Cells[j].Text);
                                total[j] += Convert.ToDecimal(GridView1.Rows[i].Cells[j].Text);
                                if (IsDetail)
                                    GridView1.Rows[i].Cells[j].Text = Convert.ToDecimal(GridView1.Rows[i].Cells[j].Text).ToString("0.00");
                            }
                        }
                    }
                }
                if (ByGroup)
                {
                    //  this.AddTotalRow("Sub Total", subTotal, true, currentId, 0);
                    // subTotal = new decimal[CellCount];


                }
                this.AddTotalRow("Grand Total", total, true, currentId, 0, true);
            }
        }
        List<string> currentId = new List<string>();
        decimal[] subTotal;
        decimal[] total;
        int EmpCount = 0;
        int TotEmpCount = 0;
        int subTotalRowIndex = 0;
        int subtotalColumnIndex = 0;
        protected void OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            if (MonthWiseReport)
            {
                subTotal = new decimal[CellCount];

                EmpCount = 0;
                for (int i = 0; i < subTotal.Count(); i++)
                {
                    decimal outval;
                    if (decimal.TryParse(Convert.ToString(subTotal[i]), out outval))
                    {
                        subTotal[i] = 0;
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataTable dt = (e.Row.DataItem as DataRowView).DataView.Table;

                    for (int masterIndex = MasterEndIndex; masterIndex >= 0; masterIndex--)
                    {
                        string orderId = Convert.ToString(dt.Rows[e.Row.RowIndex][masterIndex]);
                        decimal value;


                        if (currentId.Count != 0 && orderId != currentId[masterIndex])
                        {

                            if (e.Row.RowIndex > 0)
                            {
                                for (int i = subTotalRowIndex; i < e.Row.RowIndex; i++)
                                {
                                    EmpCount++;
                                    TotEmpCount++;
                                    GridViewRow row = GridView1.Rows[i];
                                    for (int j = 0; j < row.Cells.Count; j++)
                                    {
                                        if (j >= DetailStartIndex)
                                        {
                                            if (Decimal.TryParse(GridView1.Rows[i].Cells[j].Text, out value))
                                            {
                                                subTotal[j] += Convert.ToDecimal(GridView1.Rows[i].Cells[j].Text);
                                                total[j] += Convert.ToDecimal(GridView1.Rows[i].Cells[j].Text);
                                                GridView1.Rows[i].Cells[j].Text = Convert.ToDecimal(GridView1.Rows[i].Cells[j].Text).ToString("0.00");
                                                if (subtotalColumnIndex == 0)
                                                {
                                                    subtotalColumnIndex = j;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (ByGroup)
                                {
                                    //  this.AddTotalRow("Sub Total", subTotal, masterIndex == 0 ? true : false, currentId, masterIndex, false);// masterIndex == 0 ? true : false);
                                }
                                subTotalRowIndex = e.Row.RowIndex;
                            }
                            currentId[masterIndex] = orderId;
                        }
                    }


                }
            }
        }

        private void AddTotalRow(string labelText, decimal[] value, bool goInitial, List<string> masterValues, int masterIndex, bool isGrandTotal = false)
        {
            //   GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
            if (value != null)
            {
                TableCell[] newCell = new TableCell[1];
                int startColumnIndex = masterIndex;
                if (!object.ReferenceEquals(value, null))
                {
                    newCell = new TableCell[value.Count()];
                    newCell[0] = new TableCell { Text = labelText, HorizontalAlign = HorizontalAlign.Right };// 

                }
                else
                {
                    newCell[0] = new TableCell { Text = "", ForeColor = ColorTranslator.FromHtml("#0000FF") };
                }
                for (int i = 0; i < masterValues.Count; i++)
                {
                    newCell[i] = new TableCell { Text = "", ForeColor = ColorTranslator.FromHtml("#0000FF") };//Text = masterValues[i] remove for bind group name in grant total row.
                }

                GridViewRow row1 = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
                //row1.BackColor = ColorTranslator.FromHtml("#F9F9F9");
                row1.ForeColor = ColorTranslator.FromHtml("#0000FF");
                if (isGrandTotal)
                {
                    newCell[startColumnIndex] = (new TableCell { Text = "Total Employees:  " + TotEmpCount + " <br/> " + labelText, ForeColor = ColorTranslator.FromHtml("#0000FF"), HorizontalAlign = HorizontalAlign.Right });

                }
                else if (IsDetail)
                {
                    newCell[startColumnIndex] = (new TableCell { Text = "Total Employees  " + EmpCount + " <br/> " + labelText, ForeColor = ColorTranslator.FromHtml("#0000FF"), HorizontalAlign = HorizontalAlign.Right });

                }

                for (int i = startColumnIndex + 1; i < value.Count(); i++)
                {

                    if (i >= DetailStartIndex)
                    {
                        newCell[i] = new TableCell { Text = value[i].ToString("0.00"), ForeColor = ColorTranslator.FromHtml("#0000FF"), HorizontalAlign = HorizontalAlign.Right };
                    }
                    else if (i - MasterEndIndex != 0)
                    {
                        newCell[i] = new TableCell { Text = "", ForeColor = ColorTranslator.FromHtml("#0000FF"), HorizontalAlign = HorizontalAlign.Right };

                    }

                }

                row1.Cells.AddRange(newCell);
                //   row1.BackColor = ColorTranslator.FromHtml("#d3e8e4");
                // row1.ForeColor = ColorTranslator.FromHtml(Color.Blue.ToString());
                if (GridView1.Controls.Count > 0)
                {
                    GridView1.Controls[0].Controls.Add(row1);


                }
                // GridView1.Controls[0].Controls.Add(row);
                if (goInitial)
                {
                    subTotal = new decimal[CellCount];
                    EmpCount = 0;
                }
            }
        }
        public JsonResult PrintTaxDeclaration(Guid financialyearId, Guid employeeId, DateTime effectiveDate, string parentId, string EffectiveYear, string EffectiveMonth, string type = "")
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int userId = Convert.ToInt32(Session["UserId"]);
                Company company = new Company(companyId, userId);
                Employee emp = new Employee(companyId, employeeId);
                TXFinanceYearList finList = new TXFinanceYearList(companyId);
                TXFinanceYear finYear = finList.Where(f => f.Id == financialyearId).FirstOrDefault();
                DateTime sdate = Convert.ToDateTime(finYear.StartingDate);
                DateTime edate = Convert.ToDateTime(finYear.EndingDate);
                string financeyear = Convert.ToString(sdate.Year + "-" + edate.Year);
                AttributeModelList attrList = new AttributeModelList(companyId);
                TXSectionList txsecList = new TXSectionList(companyId, financialyearId);
                TXEmployeeSectionList getdata = new TXEmployeeSectionList(employeeId, effectiveDate);
                AttributeModel rentAttr = attrList.Where(a => a.Name == "ARP").FirstOrDefault();
                TXEmployeeSection rentSec = getdata.Where(a => a.SectionId == (rentAttr == null ? Guid.Empty : rentAttr.Id)).FirstOrDefault();
                TXSection LTAExemption = txsecList.Where(l => l.Name == "LTA Exemption").FirstOrDefault();
                TXSection inFromHouProp = txsecList.Where(n => n.Name == "Income from house property").FirstOrDefault();
                TXEmployeeSection ltaExemption = getdata.Where(o => o.SectionId == LTAExemption.Id).FirstOrDefault();
                HousePropertyList hsProplist = new HousePropertyList(employeeId, companyId, financialyearId);
                decimal intrestTolender = 0;
                string lenderName = "";
                string lenderAddress = "";
                string lenderPan = "";
                string financeInstitute = "";
                string employer = "";
                string others = "";

                hsProplist.ForEach(f =>
                {
                    if (f.TxSectionId == inFromHouProp.Id)
                    {
                        if (f.TotalInterestOfYear != null && f.EffectiveMonth == effectiveDate.Month.ToString() && f.EffectiveYear == effectiveDate.Year.ToString())
                        {
                            intrestTolender = intrestTolender + f.PayableHousingLoanPerYear;
                        }
                        if (f.LenderName != null && f.EffectiveMonth == effectiveDate.Month.ToString() && f.EffectiveYear == effectiveDate.Year.ToString())
                        {
                            lenderName = (lenderName == "" ? lenderName : lenderName + ",") + (f.LenderName == "" ? null : f.LenderName);
                        }
                        if (f.LenderHRAAddress != null && f.EffectiveMonth == effectiveDate.Month.ToString() && f.EffectiveYear == effectiveDate.Year.ToString())
                        {
                            lenderAddress = (lenderAddress == "" ? lenderAddress : lenderAddress + ",") + (f.LenderHRAAddress == "" ? null : f.LenderHRAAddress);
                        }

                        if (f.LenderType == 1 && f.EffectiveMonth == effectiveDate.Month.ToString() && f.EffectiveYear == effectiveDate.Year.ToString())
                        {
                            financeInstitute = (financeInstitute == "" ? financeInstitute : financeInstitute + ",") + (f.LenderPAN == "" ? null : f.LenderPAN);
                        }
                        if (f.LenderType == 2 && f.EffectiveMonth == effectiveDate.Month.ToString() && f.EffectiveYear == effectiveDate.Year.ToString())
                        {
                            employer = (employer == "" ? employer : employer + ",") + (f.LenderPAN == "" ? null : f.LenderPAN);
                        }
                        if (f.LenderType == 3 && f.EffectiveMonth == effectiveDate.Month.ToString() && f.EffectiveYear == effectiveDate.Year.ToString())
                        {
                            others = (others == "" ? others : others + ",") + (f.LenderPAN == "" ? null : f.LenderPAN);
                        }
                    }
                }
                );

                TXSection txsectionc = txsecList.Where(c => c.Name.Contains("80CCE")).FirstOrDefault();
                TXSection txsectionccd = txsecList.Where(c => c.Name.Trim().Contains("Under Section 80CCD")).FirstOrDefault();
                TXSection txsectionc80d = txsecList.Where(c => c.Name.Contains("(80D)")).FirstOrDefault();
                TXSection txsectioncVIA = txsecList.Where(c => c.Name.Contains("VI A")).FirstOrDefault();
                TXSectionList txsectionccdp = new TXSectionList(companyId, financialyearId);
                List<jsonDeclarationSheet> result = new List<jsonDeclarationSheet>();

                if (txsectionc != null)
                {
                    TXSectionList txsectioncp = new TXSectionList(companyId, financialyearId);
                    List<TXSection> txcp = txsectioncp.Where(c => c.ParentId == txsectionc.Id).ToList();
                    txcp.ForEach(f =>
                    {
                        if (!f.Name.Contains("80CCC"))
                            result.Add(jsonDeclarationSheet.tojson(f, "80CCE"));

                    });
                    txcp.ForEach(f =>
                    {
                        if (f.Name.Contains("80CCC"))
                            result.Add(jsonDeclarationSheet.tojson(f, "80CCE80CCC"));

                    });
                }


                if (txsectionccd != null)
                {
                    List<TXSection> txcdp = txsectionccdp.Where(c => c.ParentId == txsectionccd.Id).ToList();
                    txcdp.ForEach(f =>
                    {
                        result.Add(jsonDeclarationSheet.tojson(f, "80CCD"));
                    });
                }
                decimal sum = 0;
                if (txsectionc80d != null)
                {
                    List<TXSection> txc80d = txsectionccdp.Where(c => c.ParentId == txsectionc80d.Id).ToList();
                    txc80d.ForEach(f =>
                    {
                        TXEmployeeSection amoun = getdata.Where(a => a.SectionId == f.Id).FirstOrDefault();
                        if (amoun != null)
                        {
                            sum = sum + Convert.ToDecimal(amoun.DeclaredValue);
                        }

                    });
                    TXSection total80d = new TXSection();
                    total80d.Id = Guid.Empty;
                    total80d.Name = "80DTOTALSUM";
                    total80d.Value = Convert.ToString(sum);
                    result.Add(jsonDeclarationSheet.tjson(total80d, "80DTOTAL"));
                }
                if (txsectioncVIA != null)
                {
                    List<TXSection> txcVIA = txsectionccdp.Where(c => c.ParentId == txsectioncVIA.Id).ToList();
                    txcVIA.ForEach(f =>
                    {
                        result.Add(jsonDeclarationSheet.tojson(f, "VI A"));
                    });
                }


                result.ForEach(f =>
                {
                    if (f.ParentSectionName != "80DTOTAL")
                    {
                        TXEmployeeSection amount = getdata.Where(a => a.SectionId == f.id).FirstOrDefault();
                        if (amount != null)
                        {
                            f.Value = amount.DeclaredValue;
                        }
                        else
                        {
                            f.Value = "";
                        }
                    }
                });






                EmployeeAddress addr = emp.EmployeeAddressList.Where(a => a.AddressType == 1).FirstOrDefault();
                TXSectionList txSectionlist = new TXSectionList(companyId, financialyearId);
                List<jsonTaxSection> jsondata = new List<jsonTaxSection>();
                string reportPath = DocumentProcessingSettings.TempDirectoryPath + "/";
                TaxBehaviorList behv = new TaxBehaviorList(financialyearId, Guid.Empty, Guid.Empty); //add monthly declaration

                string currdate = Convert.ToString(effectiveDate);
                string[] datesplit = currdate.Split('-', '/');
                string outputfilePath = string.Empty;
                behv.Where(f => f.InputType == 2).ToList().ForEach(f =>
                {
                    AttributeModel attr = new AttributeModel(f.AttributemodelId, companyId);
                    if (!object.ReferenceEquals(attr, null))
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
                            disorderNo = attr.OrderNumber
                        });
                    }
                });
                List<TXSection> taxHeaderSection = txSectionlist.Where(u => u.ParentId == Guid.Empty).ToList();
                List<TXSection> finalresult = new List<TXSection>();
                taxHeaderSection.ForEach(Hearder =>
                {
                    if (Hearder.SectionType != "Others")
                    {
                        Hearder.DisorderNo = Convert.ToDouble(Hearder.OrderNo.ToString() + ".0");
                        jsondata.Add(jsonTaxSection.toJson(Hearder, type));
                        finalresult.Add(Hearder);
                        List<TXSection> taxSubSection = txSectionlist.Where(d => d.ParentId == Hearder.Id).ToList();
                        taxSubSection.Where(s => s.SectionType != "Others" && s.FormulaType != 6).ToList().ForEach(SubSec =>
                        {
                            var section = getdata.Where(d => SubSec.Id == d.SectionId).FirstOrDefault();
                            if (!string.IsNullOrEmpty(Convert.ToString(section)))
                                SubSec.DeclaredValue = object.ReferenceEquals(section.DeclaredValue, null) ? "" : section.DeclaredValue;
                            SubSec.DisorderNo = Convert.ToDouble(Hearder.OrderNo.ToString() + '.' + SubSec.OrderNo.ToString());
                            finalresult.Add(SubSec);
                            jsondata.Add(jsonTaxSection.toJson(SubSec, type));
                        });

                    }

                });
                List<TXSection> taxHeader_Section = txSectionlist.Where(u => u.SectionType == "Others").ToList();
                taxHeader_Section.ForEach(Hearder1 =>
                {
                    if (Hearder1.SectionType == "Others")
                    {
                        finalresult.Add(Hearder1);
                        jsondata.Add(jsonTaxSection.toJson(Hearder1, type = "Others"));
                    }

                });

                string empAddress = addr != null ? !string.IsNullOrEmpty(addr.AddressLine1) ? addr.AddressLine1 + (!string.IsNullOrEmpty(addr.AddressLine2) ? "*" + addr.AddressLine2 : "")
                     : !string.IsNullOrEmpty(addr.AddressLine2) ? addr.AddressLine2 + "*" : "" : "";
                empAddress = !string.IsNullOrEmpty(empAddress) ? addr.City + "*" : "";
                empAddress = emp.FirstName + "*" + empAddress;
                string pdfPath = string.Empty;
                List<string> rptparms = new List<string>();
                List<rptWorkSheet> worksheetList = new List<rptWorkSheet>();
                pdfPath = reportPath + datesplit[1].ToString() + "_" + datesplit[0].ToString() + "_" + emp.EmployeeCode + ".pdf";
                rptparms.Add(empAddress);
                rptparms.Add(emp.EmployeePersonal.PANNumber);
                rptparms.Add(financeyear);
                rentSec.TotalRent = Convert.ToDecimal(rentSec.DeclaredValue);
                rptparms.Add(rentSec == null ? "" : rentSec.TotalRent == null ? "" : rentSec.TotalRent.ToString());
                rptparms.Add(rentSec == null ? "" : rentSec.LandLordName == null ? "" : rentSec.LandLordName);
                rptparms.Add(rentSec == null ? "" : rentSec.LandLordAddress == null ? "" : rentSec.LandLordAddress);
                rptparms.Add(rentSec == null ? "" : rentSec.PanNumber == null ? "" : rentSec.PanNumber);
                rptparms.Add(ltaExemption == null ? "0" : ltaExemption.DeclaredValue == null ? "" : ltaExemption.DeclaredValue);
                rptparms.Add(intrestTolender.ToString());
                rptparms.Add(lenderName.ToString());
                rptparms.Add(lenderAddress);
                rptparms.Add(lenderPan);
                rptparms.Add(financeInstitute.ToString());
                rptparms.Add(employer.ToString());
                rptparms.Add(others.ToString());
                rptparms.Add(emp.EmployeeCode.ToString());
                generatDeclarationSheet(pdfPath, result, rptparms);
                outputfilePath = pdfPath;

                return base.BuildJson(true, 200, "success", outputfilePath);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return BuildJsonResult(false, 200, "Failure", ex.Message);
            }
        }

        public bool generatDeclarationSheet(string PDFFilePath, List<jsonDeclarationSheet> dt, List<string> parameters)
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
                rpt.ReportPath = "Reports/Form12BB.rdlc";

                ReportDataSource rptDs = new ReportDataSource("DSDeclaration", dt);
                rpt.DataSources.Add(rptDs);
                ReportParameterCollection rpcollection = new ReportParameterCollection();
                rpcollection.Add(new ReportParameter("Name", parameters[0]));
                rpcollection.Add(new ReportParameter("AccountNo", parameters[1]));
                rpcollection.Add(new ReportParameter("FinancialYear", parameters[2]));
                rpcollection.Add(new ReportParameter("Rentpaid", parameters[3]));
                rpcollection.Add(new ReportParameter("LandLordName", parameters[4]));
                rpcollection.Add(new ReportParameter("LandLordAddress", parameters[5]));
                rpcollection.Add(new ReportParameter("LandLordAccountNo", parameters[6]));
                rpcollection.Add(new ReportParameter("LTA", parameters[7]));
                rpcollection.Add(new ReportParameter("IntrestToLender", parameters[8]));
                rpcollection.Add(new ReportParameter("NameOfTheLender", parameters[9]));
                rpcollection.Add(new ReportParameter("AddressOfTheLender", parameters[10]));
                rpcollection.Add(new ReportParameter("PanOfLender", parameters[11]));
                rpcollection.Add(new ReportParameter("FinancialInstitute", parameters[12]));
                rpcollection.Add(new ReportParameter("Employer", parameters[13]));
                rpcollection.Add(new ReportParameter("Others", parameters[14]));
                rpcollection.Add(new ReportParameter("EmpCode", parameters[15]));
                rpt.SetParameters(rpcollection);
                byte[] renderedBytes = null;

                renderedBytes = rpt.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);



                string contentype = mimeType;


                using (FileStream fs = System.IO.File.Create(PDFFilePath))
                {
                    fs.Write(renderedBytes, 0, (int)renderedBytes.Length);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                // ErrorLog.Log(ex);
            }

        }

        public JsonResult PrintDeclarationPdf(Guid financialyearId, Guid employeeId, DateTime effectiveDate, string parentId, string EffectiveYear, string EffectiveMonth, string type = "")
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int userId = Convert.ToInt32(Session["UserId"]);
                Company company = new Company(companyId, userId);
                Employee emp = new Employee(companyId, employeeId);
                AttributeModelList attrList = new AttributeModelList(companyId);

                TXFinanceYearList finList = new TXFinanceYearList(companyId);
                TXFinanceYear finYear = finList.Where(f => f.Id == financialyearId).FirstOrDefault();
                DateTime sdate = Convert.ToDateTime(finYear.StartingDate);
                DateTime edate = Convert.ToDateTime(finYear.EndingDate);
                string financeyear = Convert.ToString(sdate.Year + "-" + edate.Year);

                Employee employee = new Employee();
                DataTable dtValue = employee.GetEmployess(companyId, Guid.Empty, Guid.Empty);


                List<jsonRentpaidDeclarationSheet> rentResult = new List<jsonRentpaidDeclarationSheet>();
                List<jsonLifeInsurance> licResult = new List<jsonLifeInsurance>();
                List<jsonTaxSection> declarationResult = new List<jsonTaxSection>();
                TXSectionList txSectionlist = new TXSectionList(companyId, financialyearId);
                List<jsonMedicalInsurance> medicalResult = new List<jsonMedicalInsurance>();
                HousePropertyList houseProperty = new HousePropertyList();


                //rent
                TXEmployeeSectionList getdata = new TXEmployeeSectionList(employeeId, effectiveDate);
                AttributeModel rentAttr = attrList.Where(a => a.Name == "ARP").FirstOrDefault();
                TXEmployeeSection rentSec = getdata.Where(a => a.SectionId == (rentAttr == null ? Guid.Empty : rentAttr.Id)).FirstOrDefault();
                if (rentSec != null)
                {
                    TxActualRentPaidList Arp = new TxActualRentPaidList(financialyearId, employeeId, rentSec.Id);
                    Arp.ForEach(j =>
                    {
                        rentResult.Add(jsonRentpaidDeclarationSheet.tojson(j, emp.EmployeeCode, emp.FirstName, rentSec.HasPan, rentSec.PanNumber, rentSec.HasDeclaration, rentSec.LandLordName, rentSec.LandLordAddress, rentSec.DeclaredValue, rentSec.TotalRent, sdate.Year, edate.Year));
                    });
                }

                //lic
                LifeInsuranceList lifeInsuranceList = new LifeInsuranceList(companyId, financialyearId, employeeId, effectiveDate.Month, effectiveDate.Year);

                DateTime EffectiveDate = Convert.ToDateTime(effectiveDate.Year + "-" + effectiveDate.Month + "-" + "01 00:00:00.000 ");
                TXEmployeeSectionList TXEmployeeSectionList = new TXEmployeeSectionList(employeeId, effectiveDate, true);
                decimal totaldeduction = 0;
                decimal TotalDeclarevalue = 0;
                lifeInsuranceList.ForEach(p =>
                {
                    if (p.PolicyDate == null || p.Relationship == "" || p.InsuredPersonName == "" || p.SumAssured == 0)
                    {
                        totaldeduction = 0;
                    }
                    else
                    {
                        if (p.annualPremium > 0 && p.SumAssured > 0)
                        {
                            if (p.PolicyDate >= Convert.ToDateTime("01/04/2013"))
                            {
                                if (p.IsDisabilityPerson == 0 || p.IsPersonTakingTreatement == 0)
                                {
                                    totaldeduction = p.SumAssured / 100 * 15;
                                }
                                else
                                {
                                    totaldeduction = p.SumAssured / 100 * 10;
                                }
                            }
                            else if (p.PolicyDate >= Convert.ToDateTime("01/04/2012"))
                            {
                                totaldeduction = p.SumAssured / 100 * 10;
                            }
                            else if (p.PolicyDate <= Convert.ToDateTime("01/04/2012"))
                            {
                                totaldeduction = p.SumAssured / 100 * 20;
                            }
                            else
                            {
                                totaldeduction = 0;
                            }
                        }
                        p.Premiumdeduction = Math.Min(p.annualPremium, totaldeduction);
                        TotalDeclarevalue = TotalDeclarevalue + p.Premiumdeduction;
                    }
                });



                lifeInsuranceList.ForEach(u =>
                {
                    u.totaldeclarevalue = TotalDeclarevalue;
                    licResult.Add(jsonLifeInsurance.toJson(u));
                });

                //declaration
                TaxBehaviorList beh = new TaxBehaviorList(financialyearId, Guid.Empty, Guid.Empty);
                beh.Where(e => e.InputType == 2).ToList().ForEach(e =>
                {
                    AttributeModel attr = new AttributeModel(e.AttributemodelId, companyId);
                    if (!object.ReferenceEquals(attr, null) && attr.Id != Guid.Empty)
                    {
                        TXEmployeeSection taxSection_Declar = getdata.Where(u => u.SectionId == attr.Id).FirstOrDefault();
                        declarationResult.Add(new jsonTaxSection
                        {
                            id = e.AttributemodelId,
                            employeeCode = emp.EmployeeCode,
                            employeeName = emp.FirstName,
                            parentSection = attr.DisplayAs,
                            sectionType = "Declaration",
                            formulaType = 2,
                            displayAs = attr.DisplayAs,
                            declaredValue = object.ReferenceEquals(taxSection_Declar, null) ? "0.00" : taxSection_Declar.DeclaredValue == "" ? "0.00" : Math.Round(Convert.ToDecimal(taxSection_Declar.DeclaredValue), 2).ToString("#,##0.00"),
                            HasPan = object.ReferenceEquals(taxSection_Declar, null) ? null : taxSection_Declar.HasPan,
                            PanNumber = object.ReferenceEquals(taxSection_Declar, null) ? "" : taxSection_Declar.PanNumber,
                            HasDeclaration = object.ReferenceEquals(taxSection_Declar, null) ? null : taxSection_Declar.HasDeclaration,
                            LandLordName = object.ReferenceEquals(taxSection_Declar, null) ? "" : taxSection_Declar.LandLordName,
                            LandLordAddress = object.ReferenceEquals(taxSection_Declar, null) ? "" : taxSection_Declar.LandLordAddress,
                            disorderNo = attr.OrderNumber
                        });
                    }
                });
                List<TXSection> taxHeaderSection = txSectionlist.Where(u => u.ParentId == Guid.Empty).OrderBy(e => e.OrderNo).ToList();
                taxHeaderSection.ForEach(Hearder =>
                {
                    if (Hearder.SectionType != "Others")
                    {
                        Hearder.DisorderNo = Convert.ToDouble(Hearder.OrderNo.ToString() + ".0");
                        List<TXSection> taxSubSection = txSectionlist.Where(d => d.ParentId == Hearder.Id).OrderBy(j => j.OrderNo).ToList();
                        if (taxSubSection.Count != 0)
                        {
                            if (Hearder.ParentId != Guid.Empty)
                            {
                                declarationResult.Add(jsonTaxSection.toDeclareJson(Hearder, type, emp.EmployeeCode, emp.FirstName));
                            }
                            taxSubSection.Where(s => s.SectionType != "Others" && s.FormulaType == 2).OrderBy(k => k.OrderNo).ToList().ForEach(SubSec =>
                            {
                                var section = getdata.Where(d => SubSec.Id == d.SectionId).FirstOrDefault();
                                SubSec.DeclaredValue = section == null ? "" : section.DeclaredValue;
                                if (!string.IsNullOrEmpty(Convert.ToString(section)))
                                    SubSec.DeclaredValue = object.ReferenceEquals(section.DeclaredValue, null) ? "" : section.DeclaredValue;
                                SubSec.DisorderNo = Convert.ToDouble(Hearder.OrderNo.ToString() + '.' + SubSec.OrderNo.ToString());
                                declarationResult.Add(jsonTaxSection.toDeclareJson(SubSec, type, emp.EmployeeCode, emp.FirstName));
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
                            Hearder1.DeclaredValue = section == null ? "" : Hearder1.DeclaredValue;
                            if (!string.IsNullOrEmpty(Convert.ToString(section)))
                                Hearder1.DeclaredValue = object.ReferenceEquals(section.DeclaredValue, null) ? "" : section.DeclaredValue;
                            declarationResult.Add(jsonTaxSection.toDeclareJson(Hearder1, type = "Others", emp.EmployeeCode, emp.FirstName));
                        }

                    }

                });



                //medical insurance
                MedicalInsuranceList medicalinsuranceList = new MedicalInsuranceList(employeeId, companyId, Convert.ToInt16(EffectiveMonth), Convert.ToInt16(EffectiveYear), "Grid");
                MedicalInsurance mspt = new MedicalInsurance(employeeId, companyId, Convert.ToInt16(EffectiveMonth), Convert.ToInt16(EffectiveYear), financialyearId);
                medicalinsuranceList.ForEach(u => medicalResult.Add(jsonMedicalInsurance.toJsonreport(u, mspt.ParentOveralldeduction, mspt.SelfSpouseChildOveralldeduction, mspt.TotalDeduction, mspt.TotalEligibleDeduction, mspt.TotalAmount)));

                // house property

                HousePropertyList HouseIncom = new HousePropertyList(financialyearId, companyId, EffectiveMonth, EffectiveYear, emp.EmployeeCode, emp.EmployeeCode);
                HouseIncom.ForEach(y => houseProperty.Add(y));



                licResult.ForEach(f =>
                {
                    Employee empd = new Employee(companyId, f.employeeId);
                    f.employeeCode = empd.EmployeeCode;
                    f.employeeName = empd.FirstName;

                });


                //house property
                //HousePropertyList HouseIncome = new HousePropertyList(financialyearId, companyId, EffectiveMonth, EffectiveYear,Guid.Empty);






                EmployeeAddress addr = emp.EmployeeAddressList.Where(a => a.AddressType == 1).FirstOrDefault();

                string reportPath = DocumentProcessingSettings.TempDirectoryPath + "/";

                string currdate = Convert.ToString(effectiveDate);
                string[] datesplit = currdate.Split('-', '/');
                string outputfilePath = string.Empty;
                string empAddress = addr != null ? !string.IsNullOrEmpty(addr.AddressLine1) ? addr.AddressLine1 + (!string.IsNullOrEmpty(addr.AddressLine2) ? "*" + addr.AddressLine2 : "")
                     : !string.IsNullOrEmpty(addr.AddressLine2) ? addr.AddressLine2 + "*" : "" : "";
                empAddress = !string.IsNullOrEmpty(empAddress) ? addr.City + "*" : "";
                empAddress = emp.FirstName + "*" + empAddress;
                string pdfPath = string.Empty;
                string companyDetails = "Company Name and Address:" + company.CompanyName + "," + company.AddressLine1;


                List<rptWorkSheet> worksheetList = new List<rptWorkSheet>();
                pdfPath = reportPath + datesplit[1].ToString() + "_" + datesplit[0].ToString() + "_" + emp.EmployeeCode + ".pdf";


                generatDeclarationPdfSheet(pdfPath, rentResult, licResult, declarationResult, houseProperty, medicalResult, companyDetails, empAddress);
                outputfilePath = pdfPath;

                return base.BuildJson(true, 200, "success", outputfilePath);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return BuildJsonResult(false, 200, "Failure", ex.Message);
            }
        }


        public bool generatDeclarationPdfSheet(string PDFFilePath, List<jsonRentpaidDeclarationSheet> dt, List<jsonLifeInsurance> dl, List<jsonTaxSection> dd, HousePropertyList hp, List<jsonMedicalInsurance> Mi, string cmpDetails, string emp)
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
                rpt.ReportPath = "Reports/DeclarationPdf.rdlc";

                ReportDataSource rptDs = new ReportDataSource("DsDeclare", dt);
                ReportDataSource rptDl = new ReportDataSource("DsLic", dl);
                ReportDataSource rptDd = new ReportDataSource("DsEmpDeclarePdf", dd);
                ReportDataSource rptHp = new ReportDataSource("DsHouseProperty", hp);
                ReportDataSource rptMi = new ReportDataSource("DsMedicalProperty", Mi);
                rpt.DataSources.Add(rptDs);
                rpt.DataSources.Add(rptDl);
                rpt.DataSources.Add(rptDd);
                rpt.DataSources.Add(rptHp);
                rpt.DataSources.Add(rptMi);
                ReportParameterCollection rpcollection = new ReportParameterCollection();
                rpcollection.Add(new ReportParameter("CompanyDetails", cmpDetails));
                rpcollection.Add(new ReportParameter("employee", emp));
                rpt.SetParameters(rpcollection);
                byte[] renderedBytes = null;

                renderedBytes = rpt.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);



                string contentype = mimeType;


                using (FileStream fs = System.IO.File.Create(PDFFilePath))
                {
                    fs.Write(renderedBytes, 0, (int)renderedBytes.Length);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                // ErrorLog.Log(ex);
            }

        }
        public JsonResult PrintXlRent(Guid financialyearId, DateTime effectiveDate, string EffectiveYear, string EffectiveMonth, string sCode, string eCode)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            string exp = "";
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int userId = Convert.ToInt32(Session["UserId"]);
                Company company = new Company(companyId, userId);
                Employee emp = new Employee(companyId, Guid.Empty);

                HousePropertyList houseProperty = new HousePropertyList();

                //rent
                DataTable dtARP = new TXActualRentPaid().GetTableValuesForReport(financialyearId, effectiveDate, sCode, eCode, companyId);
                //house rent property
                houseProperty = new HousePropertyList(financialyearId, companyId, EffectiveMonth, EffectiveYear, sCode, eCode);
                //declaration
                DataTable dtdec = new TXEmployeeSection().GetDeclareValuesForReport(financialyearId, effectiveDate, sCode, eCode, companyId);

                DataTable dtsec = new TXEmployeeSection().GetSectionValuesForReport(financialyearId, effectiveDate, sCode, eCode, companyId);

                dtdec.Merge(dtsec);
                //lic
                DataTable dtLic = new LifeInsurance().GetLicValuesForReport(financialyearId, effectiveDate, sCode, eCode, companyId);
                //medicalinsurance
                DataTable dtMed = new MedicalInsurance().GetTableRoprotValues(companyId, effectiveDate.Month, effectiveDate.Year, sCode, eCode);


                EmployeeAddress addr = emp.EmployeeAddressList.Where(a => a.AddressType == 1).FirstOrDefault();

                string reportPath = DocumentProcessingSettings.TempDirectoryPath + "/";

                string currdate = Convert.ToString(effectiveDate);
                string[] datesplit = currdate.Split('-', '/');
                string outputfilePath = string.Empty;
                string empAddress = addr != null ? !string.IsNullOrEmpty(addr.AddressLine1) ? addr.AddressLine1 + (!string.IsNullOrEmpty(addr.AddressLine2) ? "*" + addr.AddressLine2 : "")
                     : !string.IsNullOrEmpty(addr.AddressLine2) ? addr.AddressLine2 + "*" : "" : "";
                empAddress = !string.IsNullOrEmpty(empAddress) ? addr.City + "*" : "";
                empAddress = emp.FirstName + "*" + empAddress;
                string pdfPath = string.Empty;
                string companyDetails = "Company Name and Address:" + company.CompanyName + "," + company.AddressLine1;


                List<rptWorkSheet> worksheetList = new List<rptWorkSheet>();
                pdfPath = reportPath + datesplit[1].ToString() + "_" + datesplit[0].ToString() + "_" + emp.EmployeeCode + ".xls";


                generatRentSheet(pdfPath, dtARP, dtLic, dtdec, houseProperty, dtMed, companyDetails);
                outputfilePath = pdfPath;

                return base.BuildJson(true, 200, "success", outputfilePath);
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);

                return BuildJsonResult(false, 200, "Failure", "(" + exp + ")" + ex.Message);
            }
        }

        public bool generatRentSheet(string PDFFilePath, DataTable dt, DataTable dl, DataTable dd, HousePropertyList hp, DataTable med, string cmpDetails)
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
                rpt.ReportPath = "Reports/PrintRent.rdlc";

                ReportDataSource rptDs = new ReportDataSource("DsDeclare", dt);
                ReportDataSource rptDl = new ReportDataSource("DsLic", dl);
                ReportDataSource rptDd = new ReportDataSource("DsEmpDeclare", dd);
                ReportDataSource rptHp = new ReportDataSource("DsHouseProperty", hp);
                ReportDataSource rptMi = new ReportDataSource("DsMedicalProperty", med);
                rpt.DataSources.Add(rptDs);
                rpt.DataSources.Add(rptDl);
                rpt.DataSources.Add(rptDd);
                rpt.DataSources.Add(rptHp);
                rpt.DataSources.Add(rptMi);
                ReportParameterCollection rpcollection = new ReportParameterCollection();
                rpcollection.Add(new ReportParameter("CompanyDetails", cmpDetails));
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
                return false;
                // ErrorLog.Log(ex);
            }

        }
        public JsonResult GetPayrollHistory(string categories, int month, int year, string empCode)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Guid employeeid = new Guid(Convert.ToString(Session["EmployeeId"]));
            return DownloadPayslip(categories, month, year, empCode, companyId, userId, employeeid);
        }
        public static JsonResult DownloadPayslip(string categories, int month, int year, string empCode, int companyId, int userId, Guid employeeid)
        {
            categories = categories.TrimEnd(',');


            Entity entity;
            string outfilePath = String.IsNullOrEmpty(Convert.ToString(ConfigurationManager.AppSettings["PayslipLocation"])) ? DocumentProcessingSettings.TempDirectoryPath : (Convert.ToString(ConfigurationManager.AppSettings["PayslipLocation"]));

            if (!Directory.Exists(outfilePath + "\\" + Enum.GetName(typeof(MonthEnum), month) + "_" + year))
            {
                Directory.CreateDirectory(outfilePath + "\\" + Enum.GetName(typeof(MonthEnum), month) + "_" + year);
                outfilePath = outfilePath + "\\" + Enum.GetName(typeof(MonthEnum), month) + "_" + year;
            }
            else
            {
                outfilePath = outfilePath + "\\" + Enum.GetName(typeof(MonthEnum), month) + "_" + year;
            }

            PaySlip payslip = new PaySlip();
            List<Employee> emp = new List<Employee>();
            EmployeeList employees = new EmployeeList(companyId, userId, employeeid);
            CategoryList categoryList = new CategoryList(companyId);
            DepartmentList deptList = new DepartmentList(companyId);
            BranchList branchList = new BranchList(companyId);
            CostCentreList costCentreList = new CostCentreList(companyId);
            DesignationList desgntionList = new DesignationList(companyId);
            PTLocationList ptLocList = new PTLocationList(companyId);
            GradeList gradeList = new GradeList(companyId);
            ESIDespensaryList esiDespen = new ESIDespensaryList(companyId);
            LocationList locationList = new LocationList(companyId);
            EntityModel entityModel = new EntityModel("AddtionalInfo", companyId);
            BankList banklist = new BankList(companyId);
            if (!string.IsNullOrEmpty(empCode))
            {
                emp = employees.Where(e => e.EmployeeCode == empCode).ToList();
                if (emp != null && emp.Count > 0)
                {
                    categories = "'" + emp[0].CategoryId + "'";
                }
            }
            DataTable dtCategories = payslip.GetSetting(categories);
            {
                PayrollHistoryList payrollHistory = new PayrollHistoryList();
                PayrollHistory payHistory = new PayrollHistory();
                PayrollHistoryList Cumulativepay = new PayrollHistoryList();

                if (string.IsNullOrEmpty(empCode))
                {
                    payrollHistory = new PayrollHistoryList(companyId, year, month);
                }
                //---- Modified by Keerthika S on 19/04/2017
                if (dtCategories.Rows.Count > 0)
                {

                    foreach (DataRow drCat in dtCategories.Rows)
                    {
                        if (!string.IsNullOrEmpty(empCode))
                        {
                            emp = employees.Where(e => e.EmployeeCode == empCode).ToList();
                            if (!object.ReferenceEquals(emp, null))
                            {
                                payHistory = new PayrollHistory(companyId, emp[0].Id, year, month);
                            }
                        }
                        else
                        {
                            emp = employees.Where(e => e.CategoryId == new Guid(drCat["FieldName"].ToString())).ToList();

                        }
                        PaySlipList setting = new PaySlipList(new Guid(drCat["CofigurationId"].ToString()));
                        PaySlipSetting ps = new PaySlipSetting(new Guid(drCat["CofigurationId"].ToString()));
                        PayrollHistoryList cumulativePayHistory = new PayrollHistoryList();



                        emp.ForEach(e =>
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(ps.CumulativeMonth)))
                            {

                                if (ps.CumulativeMonth > month)
                                {
                                    DateTime cumulative = new DateTime(year, ps.CumulativeMonth, 1);
                                    cumulativePayHistory = new PayrollHistoryList(companyId, year - 1, ps.CumulativeMonth, year, month, e.Id);
                                }
                                else
                                {
                                    cumulativePayHistory = new PayrollHistoryList(companyId, year, ps.CumulativeMonth, year, month, e.Id);
                                }
                            }
                            List<PayrollError> payErrors = new List<PayrollError>();

                            if (string.IsNullOrEmpty(empCode))
                            {
                                payHistory = payrollHistory.Where(p => p.EmployeeId == e.Id).FirstOrDefault();
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(ps.CumulativeMonth)))
                            {

                                Cumulativepay.AddRange(cumulativePayHistory.Where(p => p.EmployeeId == e.Id).ToList());

                            }

                            PaySlipList result = setting;
                            Emp_BankList empbank = new Emp_BankList(e.Id);
                            EmployeeAddressList empaddr = new EmployeeAddressList(e.Id);
                            Emp_Personal emppersonal = new Emp_Personal(e.Id);

                            EntityAdditionalInfoList empAddInfoList = new EntityAdditionalInfoList(companyId, entityModel.Id, e.Id);
                            //  payHistory.PayrollHistoryValueList = payHistory.PayrollHistoryValueList;
                            if (!object.ReferenceEquals(payHistory, null) && payHistory.Status == ComValue.payrollProcessStatus[0])
                            {
                                result.ForEach(r =>
                                {
                                    //Assign Master Values from Physical Table
                                    switch (r.TableName.ToLower())
                                    {
                                        case "employee":

                                            r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.FieldName : r.DisplayAs;
                                            var empDetails = e.GetType().GetProperty(r.FieldName).GetValue(e, null);
                                            r.Value1 = Convert.ToString(empDetails) != null ? Convert.ToString(empDetails) : "";
                                            // r.Value1 = e.GetType().GetProperty(r.FieldName).GetValue(e, null).ToString();
                                            if (e.GetType().GetProperty(r.FieldName).PropertyType.Name == "Guid")
                                            {
                                                if (new Guid(r.Value1) != Guid.Empty)
                                                {
                                                    switch (r.FieldName)
                                                    {
                                                        case "CategoryId":
                                                            r.Value1 = categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().Name;
                                                            break;
                                                        case "Category":
                                                            r.Value1 = categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().Name;
                                                            break;
                                                        case "Department":
                                                            r.Value1 = deptList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().DepartmentName;
                                                            break;
                                                        case "Branch":
                                                            r.Value1 = branchList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().BranchName;
                                                            break;
                                                        case "Designation":
                                                            r.Value1 = desgntionList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().DesignationName;
                                                            break;
                                                        case "CostCentre":
                                                            r.Value1 = costCentreList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().CostCentreName;
                                                            break;
                                                        case "Grade":
                                                            r.Value1 = gradeList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().GradeName;
                                                            break;
                                                        case "PTLocation":
                                                            r.Value1 = ptLocList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().PTLocationName;
                                                            break;
                                                        case "ESIDespensary":
                                                            r.Value1 = esiDespen.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().ESIDespensary;
                                                            break;
                                                        case "Location":
                                                            r.Value1 = locationList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().LocationName;
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    r.Value1 = "";
                                                }
                                            }
                                            else if (e.GetType().GetProperty(r.FieldName).PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                r.Value1 = Convert.ToDateTime(r.Value1).ToString("dd/MMM/yyyy");
                                            }


                                            break;
                                        case "emp_bank":

                                            r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.FieldName : r.DisplayAs;
                                            if (empbank.Count > 0)
                                            {
                                                var bankDetails = empbank[0].GetType().GetProperty(r.FieldName).GetValue(empbank[0], null);
                                                r.Value1 = Convert.ToString(bankDetails) != null ? Convert.ToString(bankDetails) : "";
                                                if (empbank[0].GetType().GetProperty(r.FieldName).PropertyType.Name == "Guid")
                                                {
                                                    switch (r.FieldName)
                                                    {
                                                        case "BankId":
                                                            r.Value1 = banklist.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().BankName;
                                                            break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                r.Value1 = string.Empty;
                                            }

                                            break;
                                        case "emp_personal":


                                            r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.FieldName : r.DisplayAs;

                                            if (r.FieldName == "BloodGroup")
                                            {
                                                r.Value1 = new BloodGroup(Convert.ToInt32(r.Value1)).BloodGroupName;
                                            }
                                            else
                                            {
                                                var OtherDetail = emppersonal.GetType().GetProperty(r.FieldName).GetValue(emppersonal, null);
                                                r.Value1 = Convert.ToString(OtherDetail) != null ? Convert.ToString(OtherDetail) : "";
                                                // r.Value1 = emppersonal.GetType().GetProperty(r.FieldName).GetValue(emppersonal, null).ToString();
                                            }
                                            if (emppersonal.GetType().GetProperty(r.FieldName).PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                r.Value1 = Convert.ToDateTime(r.Value1).ToString("dd/MMM/yyyy");
                                            }
                                            break;
                                        case "emp_address":

                                            if (empaddr.Count > 0)
                                            {
                                                r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.FieldName : r.DisplayAs;
                                                var AddressDetail = empaddr[0].GetType().GetProperty(r.FieldName).GetValue(empaddr[0], null);
                                                r.Value1 = Convert.ToString(AddressDetail) != null ? Convert.ToString(AddressDetail) : "";
                                                //r.Value1 = empaddr[0].GetType().GetProperty(r.FieldName).GetValue(empaddr[0], null).ToString();
                                            }
                                            break;
                                        case "additionalinfo":

                                            r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.FieldName : r.DisplayAs;
                                            if (empAddInfoList.Count > 0)
                                            {
                                                for (int cnt = 0; cnt < empAddInfoList.Count; cnt++)
                                                {

                                                    if (r.FieldName == empAddInfoList[cnt].AttributeModelId.ToString())
                                                    {

                                                        AttributeModel at = new AttributeModel(empAddInfoList[cnt].AttributeModelId, companyId);
                                                        r.Value1 = empAddInfoList[cnt].Value;

                                                        r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? at.DisplayAs : r.DisplayAs;

                                                    }
                                                }
                                            }
                                            break;
                                        case "category":
                                            r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.FieldName : r.DisplayAs;
                                            var empDetails1 = e.CategoryId;
                                            r.Value1 = Convert.ToString(empDetails1) != null ? Convert.ToString(empDetails1) : "";
                                            switch (r.FieldName)
                                            {
                                                case "CategoryId":
                                                    r.Value1 = categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().Name;
                                                    break;
                                                case "Category":
                                                    r.Value1 = categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().Name;
                                                    break;
                                            }
                                            break;
                                    }
                                    //Assign payroll History

                                    for (int cnt = 0; cnt < payHistory.PayrollHistoryValueList.Count; cnt++)
                                    {
                                        if (r.FieldName == payHistory.PayrollHistoryValueList[cnt].AttributeModelId.ToString())
                                        {
                                            AttributeModel a = new AttributeModel(payHistory.PayrollHistoryValueList[cnt].AttributeModelId, companyId);
                                            r.Value1 = payHistory.PayrollHistoryValueList[cnt].Value;
                                            r.DisplayAs = r.DisplayAs == string.Empty ? a.DisplayAs : r.DisplayAs;
                                        }
                                    }
                                    PayrollHistoryValueList payValue = new PayrollHistoryValueList();
                                    if (!object.ReferenceEquals(Cumulativepay, null) && Cumulativepay.Count > 0)
                                    {
                                        Cumulativepay.ForEach(cu =>
                                        {
                                            payValue.AddRange(cu.PayrollHistoryValueList
                                                      .GroupBy(f => new { f.AttributeModelId })
                                                      .Select(group => new PayrollHistoryValue { AttributeModelId = group.Key.AttributeModelId, Value = Convert.ToString(group.Sum(f => string.IsNullOrEmpty(f.Value) ? 0 : Convert.ToDecimal(f.Value))) }));
                                            for (int cnt = 0; cnt < cu.PayrollHistoryValueList.Count; cnt++)
                                            {
                                                if (r.FieldName == cu.PayrollHistoryValueList[cnt].AttributeModelId.ToString())
                                                {
                                                    AttributeModel a = new AttributeModel(cu.PayrollHistoryValueList[cnt].AttributeModelId, companyId);
                                                    r.Value2 = (Convert.ToDecimal(r.Value2) + Convert.ToDecimal(cu.PayrollHistoryValueList[cnt].Value)).ToString();
                                                    //  r.DisplayAs = r.DisplayAs == string.Empty ? a.DisplayAs : r.DisplayAs;
                                                }
                                            }
                                        });
                                    }
                                });//attr End
                                //modified by AjithPanner on 20/11/2017
                                GetPaySlip(payHistory, result, outfilePath + "/" + e.EmployeeCode + ".pdf", month, year, ps.DisplayCumulative, companyId, userId);
                            }

                        });//Employee end

                    }//Categories end


                }
                else
                {

                    return BuildJsonResult(false, 100, "Set the PaySlip Setting in Setting Tab", null);
                }

                if (!string.IsNullOrEmpty(empCode))
                {
                    outfilePath = outfilePath + "/" + empCode + ".pdf";
                }
                else
                {
                    if (categories != "" && string.IsNullOrEmpty(Convert.ToString(ConfigurationManager.AppSettings["PayslipLocation"])))
                    {


                        string PDFFilePath = DocumentProcessingSettings.TempDirectoryPath + "PaySlip.zip";
                        ZipPath(PDFFilePath, outfilePath, null, true, null);
                        outfilePath = PDFFilePath;
                    }
                    else
                    {
                        return BuildJsonResult(false, 1, "File Saved Successfully at " + outfilePath, null);
                    }
                }
                return BuildJsonResult(true, 1, "File Saved Successfully", new { filePath = outfilePath });
            }

        }

        #region "Paysheet Settings"
        public JsonResult savePaysheetSetting(List<jsonPaySheetattr> paysheetattr, jsonPaySheet jpaysheet)
        {
            try
            {
                if (!base.checkSession())
                    return base.BuildJson(true, 0, "Invalid user", null);
                bool isSaved = false;
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int userId = Convert.ToInt32(Session["UserId"]);


                if (jpaysheet.id == 0)
                {
                    isSaved = false;

                }
                else
                {
                    new Paysheetatrr().Delete(jpaysheet.id);
                }
                Paysheet objSetting = jsonPaySheet.convertobject(jpaysheet);
                objSetting.CreatedBy = userId;
                objSetting.ModifiedBy = userId;
                objSetting.CreatedOn = DateTime.Now;
                objSetting.ModifiedOn = DateTime.Now;
                objSetting.CompanyId = companyId;
                isSaved = objSetting.Save();

                if (isSaved)
                {
                    if (!ReferenceEquals(paysheetattr, null))
                    {
                        paysheetattr.ForEach(a =>
                        {
                            a.paysheetId = objSetting.Id;
                            Paysheetatrr atrr = jsonPaySheetattr.convertobject(a);
                            atrr.Save();
                        });
                    }
                    return base.BuildJson(true, 200, "Data saved successfully", paysheetattr);
                }
                else
                {

                    return base.BuildJson(false, 100, "There is some error while saving the data.", paysheetattr);
                }
            }
            catch (Exception ex)
            {
                TraceError.ErrorLog.Log(ex); return base.BuildJson(false, 100, "There is some error while saving the data.", paysheetattr);
            }

        }

        //created by suriya

        public JsonResult DeletepaysheetSetting(int iD)
        {
            bool isDeleted = false;
            bool isDeleteStatus = false;
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Paysheetatrr objSetting = new Paysheetatrr();
            isDeleted = objSetting.Delete(iD);
            if (isDeleted)
            {
                Paysheetatrr objattr = new Paysheetatrr();
                isDeleteStatus = objattr.Delete(iD);
                if (isDeleteStatus)
                {
                    return base.BuildJson(true, 200, "Data deleted successfully", isDeleteStatus);
                }
                else
                {
                    return base.BuildJson(false, 100, "There is some error while Delete the data.", isDeleteStatus);
                }
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while Delete the data.", isDeleteStatus);
            }
        }
        //created by suriya
        public JsonResult Descriptioncheck(int id, string Description)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            PaySheetList Deslist = new PaySheetList(id, companyId);
            var Descrip = Deslist.Where(d => d.CompanyId == companyId && d.Description.ToLower().Trim() == Description.ToLower().Trim()).ToList();
            if (Descrip.Count == 0)
            {
                return base.BuildJson(true, 200, string.Empty, true);
            }
            else
            {
                return base.BuildJson(false, 200, "Name is already Exist.", false);
            }

        }


        public JsonResult GetPaysheetSettings(int id)
        {

            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            PaySheetList setting = new PaySheetList(id, companyId);

            List<jsonPaySheet> jsetting = new List<jsonPaySheet>();
            setting.ForEach(s =>
            {
                jsetting.Add(jsonPaySheet.tojson(s));
            });

            PayheetAttributesList attr = new PayheetAttributesList(id);
            List<jsonPaySheetattr> jattr = new List<jsonPaySheetattr>();
            AttributeModelList mdl = new AttributeModelList(companyId);
            attr.ForEach(u =>
            {
                jattr.Add(jsonPaySheetattr.tojson(u));
            });

            return base.BuildJson(true, 200, "success", new
            {
                setting = jsetting,
                attrCategory = jsetting.Count > 0 ? jsetting[0].category.TrimEnd(',').Split(',') : null,
                attrGroup = jattr.Where(j => j.type == "Group").ToList(),
                attrMaster = jattr.Where(j => j.type == "Master").ToList(),
                attrDetail = jattr.Where(j => j.type == "Detail").ToList(),

            });
        }


        public static Dictionary<string, string> GetFilterExpr(int companyId, List<jsonPaySheetattr> filters)
        {
            Dictionary<string, string> filterExpr = new Dictionary<string, string>();
            string expr = string.Empty;
            filters.Where(f => f.tableName.ToLower() == "employee").ToList().ForEach(f =>
            {
                switch (f.fieldName)
                {
                    case "Category":
                        if (new CategoryList(companyId).Where(c => c.Name == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new CategoryList(companyId).Where(c => c.Name == f.value).FirstOrDefault().Id);
                        }
                        else
                            f.value = string.Empty;
                        break;
                    case "Department":
                        if (new DepartmentList(companyId).Where(c => c.DepartmentName == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new DepartmentList(companyId).Where(c => c.DepartmentName == f.value).FirstOrDefault().Id);
                        }
                        else
                            f.value = string.Empty;
                        break;
                    case "Branch":
                        if (new BranchList(companyId).Where(c => c.BranchName == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new BranchList(companyId).Where(c => c.BranchName == f.value).FirstOrDefault().Id);
                        }
                        else { f.value = string.Empty; }
                        break;
                    case "Designation":
                        if (new DesignationList(companyId).Where(c => c.DesignationName == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new DesignationList(companyId).Where(c => c.DesignationName == f.value).FirstOrDefault().Id);
                        }
                        else { f.value = string.Empty; }
                        break;
                    case "CostCentre":
                        if (new CostCentreList(companyId).Where(c => c.CostCentreName == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new CostCentreList(companyId).Where(c => c.CostCentreName == f.value).FirstOrDefault().Id);
                        }
                        else { f.value = string.Empty; }
                        break;
                    case "Grade":
                        if (new GradeList(companyId).Where(c => c.GradeName == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new GradeList(companyId).Where(c => c.GradeName == f.value).FirstOrDefault().Id);
                        }
                        else { f.value = string.Empty; }
                        break;
                    case "ESILocation":
                        if (new EsiLocationList(companyId).Where(c => c.LocationName == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new EsiLocationList(companyId).Where(c => c.LocationName == f.value).FirstOrDefault().Id);
                        }
                        else { f.value = string.Empty; }
                        break;
                    case "PTLocation":
                        if (new PTLocationList(companyId).Where(c => c.PTLocationName == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new PTLocationList(companyId).Where(c => c.PTLocationName == f.value).FirstOrDefault().Id);
                        }
                        else { f.value = string.Empty; }
                        break;
                    case "Location":
                        if (new LocationList(companyId).Where(c => c.LocationName == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new LocationList(companyId).Where(c => c.LocationName == f.value).FirstOrDefault().Id);
                        }
                        else { f.value = string.Empty; }
                        break;
                    case "ESIDespensary":
                        if (new ESIDespensaryList(companyId).Where(c => c.ESIDespensary == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new ESIDespensaryList(companyId).Where(c => c.ESIDespensary == f.value).FirstOrDefault().Id);
                        }
                        else { f.value = string.Empty; }
                        break;
                    case "Bank":
                        if (new BankList(companyId).Where(c => c.BankName == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new BankList(companyId).Where(c => c.BankName == f.value).FirstOrDefault().Id);
                        }
                        else { f.value = string.Empty; }
                        break;
                }
                if (!string.IsNullOrEmpty(f.value))
                {
                    expr = expr + " AND " + f.fieldName + " " + f.operation + " CAST('" + f.value + "' as " + f.datatype + ")";
                }


            });
            filterExpr.Add("employee", expr);
            expr = string.Empty;
            filters.Where(f => f.tableName.ToLower() == "emp_bank").ToList().ForEach(f =>
            {
                expr = expr + " AND " + f.fieldName + " " + f.operation + " CAST('" + f.value + "' as " + f.datatype + ")";
            });
            filterExpr.Add("emp_bank", expr);
            expr = string.Empty;
            filters.Where(f => f.tableName.ToLower() == "emp_Address").ToList().ForEach(f =>
            {
                expr = expr + " AND " + f.fieldName + " " + f.operation + " CAST('" + f.value + "' as " + f.datatype + ")";
            });
            filterExpr.Add("emp_Address", expr);
            expr = string.Empty;
            filters.Where(f => f.tableName.ToLower() == "emp_personal").ToList().ForEach(f =>
            {
                expr = expr + " AND " + f.fieldName + " " + f.operation + " CAST('" + f.value + "' as " + f.datatype + ")";
            });
            filterExpr.Add("emp_personal", expr);
            expr = string.Empty;
            filters.Where(f => f.type == "Master" || f.type == "Deductions" || f.type == "Earnings").ToList().ForEach(f =>
            {
                expr = expr + " AND " + f.fieldName + " " + f.operation + " CAST('" + f.value + "' as " + f.datatype + ")";
            });
            filterExpr.Add("PayrollHistory", expr);
            expr = string.Empty;
            return filterExpr;
        }
        #endregion

        public class jsonPaySlipSetting
        {
            public Guid CofigurationId { get; set; }
            public int companyId { get; set; }
            public string description { get; set; }
            public string logo { get; set; }
            public string title { get; set; }
            public string isDetail { get; set; }
            public string string1 { get; set; }
            public string string2 { get; set; }
            public string footerstring1 { get; set; }
            public string footerstring2 { get; set; }

            public bool displayCumulative { get; set; }

            public int cumulativeMonth { get; set; }
            public static jsonPaySlipSetting tojson(PaySlipSetting setting)
            {
                return new jsonPaySlipSetting()
                {
                    CofigurationId = setting.ConfigurationId,
                    companyId = setting.CompanyId,
                    description = setting.Description,
                    title = setting.Title,
                    logo = setting.Logo,
                    string1 = setting.String1,
                    string2 = setting.String2,
                    footerstring1 = setting.FooterString1,
                    footerstring2 = setting.FooterString2,
                    displayCumulative = setting.CumulativeMonth == 0 ? false : true,
                    cumulativeMonth = setting.CumulativeMonth
                };
            }
            public static PaySlipSetting convertobject(jsonPaySlipSetting setting)
            {
                return new PaySlipSetting()
                {
                    ConfigurationId = setting.CofigurationId,
                    CompanyId = setting.companyId,
                    Description = setting.description,
                    Title = setting.title,
                    Logo = setting.logo,
                    String1 = setting.string1,
                    String2 = setting.string2,
                    FooterString1 = setting.footerstring1,
                    FooterString2 = setting.footerstring2,
                    DisplayCumulative = setting.cumulativeMonth == 0 ? false : true,
                    CumulativeMonth = setting.cumulativeMonth

                };
            }


        }





        public class jsonPaySlipattributes
        {
            public int Id { get; set; }

            public Guid CofigurationId { get; set; }
            public int companyId { get; set; }

            public Guid categoryId { get; set; }

            public Guid attributeId { get; set; }
            public string tableName { get; set; }
            public string fieldName { get; set; }
            public int? hOrder { get; set; }
            public int? fOrder { get; set; }
            public int? eOrder { get; set; }
            public int? dOrder { get; set; }
            public string type { get; set; }
            public string displayAs { get; set; }
            public bool isPhysicalTbl { get; set; }

            public string datatype { get; set; }
            public bool IsincludeGrosspay { get; set; }
            public static jsonPaySlipattributes tojson(PaySlipAttributes attr)
            {
                return new jsonPaySlipattributes()
                {
                    Id = attr.Id,
                    CofigurationId = attr.CofigurationId,
                    categoryId = attr.CategoryId,
                    tableName = attr.TableName,
                    fieldName = attr.FieldName,
                    displayAs = attr.DisplayAs,
                    datatype = attr.Data_Type,
                    hOrder = attr.HeaderDisplayOrder == 0 ? (int?)null : attr.HeaderDisplayOrder,
                    fOrder = attr.FooterDisplayOrder == 0 ? (int?)null : attr.FooterDisplayOrder,
                    eOrder = attr.EarningDisplayOrder == 0 ? (int?)null : attr.EarningDisplayOrder,
                    dOrder = attr.DeductionDisplayOrder == 0 ? (int?)null : attr.DeductionDisplayOrder,
                    type = attr.Type,
                    isPhysicalTbl = attr.IsPhysicalTable,
                    attributeId = attr.AttributeId,
                    IsincludeGrosspay = attr.IsIncludeGrossPay
                };
            }
            public static PaySlipAttributes convertobject(jsonPaySlipattributes attr)
            {
                return new PaySlipAttributes()
                {
                    Id = attr.Id,
                    CofigurationId = attr.CofigurationId,
                    CategoryId = attr.categoryId,
                    TableName = attr.tableName,
                    FieldName = attr.fieldName,
                    DisplayAs = attr.displayAs,
                    HeaderDisplayOrder = attr.hOrder == null ? 0 : (int)attr.hOrder,
                    FooterDisplayOrder = attr.fOrder == null ? 0 : (int)attr.fOrder,
                    EarningDisplayOrder = attr.eOrder == null ? 0 : (int)attr.eOrder,
                    DeductionDisplayOrder = attr.dOrder == null ? 0 : (int)attr.dOrder,
                    Type = attr.type,
                    IsPhysicalTable = attr.isPhysicalTbl,
                    AttributeId = attr.attributeId

                };
            }
        }

        public class jsonPaySheetattr
        {
            public int id { get; set; }
            public int paysheetId { get; set; }
            public int month { get; set; }
            public int year { get; set; }

            public string tableName { get; set; }
            public string fieldName { get; set; }
            public int order { get; set; }
            public string datatype { get; set; }
            public string displayAs { get; set; }
            public string value { get; set; }
            public string type { get; set; }
            public string operation { get; set; }
            public bool isPhysicalTbl { get; set; }

            public static jsonPaySheetattr tojson(Paysheetatrr attr)
            {
                return new jsonPaySheetattr()
                {
                    id = attr.Id,
                    paysheetId = attr.PaySheetId,

                    tableName = attr.TableName,
                    fieldName = attr.FieldName,
                    displayAs = attr.DisplayAs,
                    order = attr.OrderBy,
                    type = attr.Type

                };
            }
            public static Paysheetatrr convertobject(jsonPaySheetattr attr)
            {
                return new Paysheetatrr()
                {
                    Id = attr.id,
                    PaySheetId = attr.paysheetId,

                    TableName = attr.tableName,
                    FieldName = attr.fieldName,
                    DisplayAs = attr.displayAs,
                    OrderBy = attr.order,
                    Type = attr.type

                };
            }
        }
        public class jsonPaySheet
        {
            public int id { get; set; }
            public int companyId { get; set; }
            public string description { get; set; }
            public string category { get; set; }
            public string title { get; set; }
            public bool isDetail { get; set; }
            public int month { get; set; }
            public int year { get; set; }
            public static jsonPaySheet tojson(Paysheet setting)
            {
                return new jsonPaySheet()
                {
                    id = setting.Id,
                    companyId = setting.CompanyId,
                    description = setting.Description,

                    category = setting.Categories,
                    title = setting.Title,
                    isDetail = setting.IsDetail
                };
            }
            public static Paysheet convertobject(jsonPaySheet setting)
            {
                return new Paysheet()
                {

                    Id = setting.id,
                    CompanyId = setting.companyId,
                    Description = setting.description,

                    Categories = setting.category,
                    Title = setting.title,
                    IsDetail = setting.isDetail
                };
            }
        }
        public class jsonPaySlip
        {
            public string empcode { get; set; }
            public string category { get; set; }
            public string configurationId { get; set; }
            public int month { get; set; }
            public int year { get; set; }
        }
        public class jsonPaySheetFilter
        {


            public string tableName { get; set; }
            public string fieldName { get; set; }
            public string value { get; set; }
            public string operation { get; set; }
            public string displayAs { get; set; }
            public string datatype { get; set; }
            public string type { get; set; }
        }
        public class jsonDeclarationSheet
        {
            public Guid id { get; set; }
            public string SectionName { get; set; }
            public string ParentSectionName { get; set; }

            public string Value { get; set; }

            public static jsonDeclarationSheet tojson(TXSection setting, string parentSection)
            {
                return new jsonDeclarationSheet()
                {
                    id = setting.Id,
                    SectionName = setting.Name,
                    ParentSectionName = parentSection


                };
            }
            public static jsonDeclarationSheet tjson(TXSection setting, string parentSection)
            {
                return new jsonDeclarationSheet()
                {
                    id = setting.Id,
                    SectionName = setting.Name,
                    ParentSectionName = parentSection,
                    Value = setting.Value


                };
            }

        }
        public class jsonRentpaidDeclarationSheet
        {
            public Guid id { get; set; }
            public string EmployeeCode { get; set; }
            public string EmployeeName { get; set; }
            public string Month { get; set; }

            public int year { get; set; }

            public string MonthOrder { get; set; }

            public decimal MetroRent { get; set; }

            public decimal NonMetroRent { get; set; }

            public bool? Haspan { get; set; }

            public string PanNumber { get; set; }

            public bool? HasDeclaration { get; set; }

            public string LandLordName { get; set; }

            public string LandLordAddress { get; set; }

            public string Declaredvalue { get; set; }

            public decimal TotalRent { get; set; }


            public static jsonRentpaidDeclarationSheet tojson(TXActualRentPaid arp, string empcode, string empname, bool? HasPan, string PanNumber, bool? HasDeclaration, string LandLordName, string LandLordAddress, string DeclaredValue, decimal TotalRent, int syear, int eyear)
            {
                string[] mOrder = { "J", "K", "L", "A", "B", "C", "D", "E", "F", "G", "H", "I" };
                int iMonthNo = arp.Month;
                DateTime dtDate = new DateTime(2000, iMonthNo, 1);
                string sMonthFullName = dtDate.ToString("MMMM");
                int iyear = arp.Month == 1 || arp.Month == 2 || arp.Month == 3 ? eyear : syear;
                return new jsonRentpaidDeclarationSheet()
                {
                    id = arp.Id,
                    EmployeeCode = empcode,
                    EmployeeName = empname,
                    Month = sMonthFullName,
                    MonthOrder = mOrder[arp.Month - 1],
                    year = iyear,
                    MetroRent = arp.MetroRent,
                    NonMetroRent = arp.NonMetroRent,
                    Haspan = HasPan,
                    PanNumber = PanNumber,
                    HasDeclaration = HasDeclaration,
                    LandLordName = LandLordName,
                    LandLordAddress = LandLordAddress,
                    Declaredvalue = DeclaredValue,
                    TotalRent = TotalRent


                };
            }
        }

    }

}

