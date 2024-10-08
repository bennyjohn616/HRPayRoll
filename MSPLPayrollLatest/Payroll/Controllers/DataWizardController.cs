﻿using System;
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
using System.Text;
using SelectPdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
 

namespace Payroll.Controllers
{
    [SessionExpireAttribute]
    public class DataWizardController : BaseController
    {
        int DetailStartIndex = 0;
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

        public JsonResult saveSetting(jsonPaySlipSetting setting, List<jsonPaySlipattributes> attr, List<previousComponents> matchingComp)
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
                    objSetting.Matchingtype = setting.MatchingSettingsFor;
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
                objSetting.Matchingtype = setting.MatchingSettingsFor;
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
                            PaySlipAttributes atrr = jsonPaySlipattributes.convertobject(a, matchingComp);
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
                        PaySlipAttributes atrr = jsonPaySlipattributes.convertobject(a, matchingComp);
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
                            if (a.displayAs == "Bran")
                            {

                            }
                            a.CofigurationId = objSetting.ConfigurationId;
                            PaySlipAttributes atrr = jsonPaySlipattributes.convertobject(a, matchingComp);
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
            int defaultEaringsOrder = eslAttribute.OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber;
            int defaultdeductionOrder = dslAttribute.OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber;
            eslAttribute.ForEach(s =>
            {

                PaySlipAttributes newPA = new PaySlipAttributes();
                newPA.FieldName = s.Id.ToString();
                newPA.DisplayAs = s.DisplayAs;
                newPA.IsIncludeGrossPay = s.IsIncludeForGrossPay;
                if (s.IsIncludeForGrossPay && id == Guid.Empty)
                {
                    defaultEaringsOrder = defaultEaringsOrder + 1;
                    newPA.EarningDisplayOrder = defaultEaringsOrder;
                }
                jattre.Add(jsonPaySlipattributes.tojson(newPA));

            });
            dslAttribute.ForEach(s =>
            {

                PaySlipAttributes newPAd = new PaySlipAttributes();
                newPAd.FieldName = s.Id.ToString();
                newPAd.DisplayAs = s.DisplayAs;
                if (s.IsIncludeForGrossPay && id == Guid.Empty)
                {
                    defaultdeductionOrder = defaultdeductionOrder + 1;
                    newPAd.DeductionDisplayOrder = defaultdeductionOrder;
                }
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
            if (jattr.Count == 0)
            {
                string tables = "'Employee','Emp_Address','Emp_Bank','Emp_Personal'";

                List<PaySlipAttributes> ps = PaySlip.GetMasterFields(tables, companyId);
                List<jsonPaySlipattributes> jattrMaster = new List<jsonPaySlipattributes>();
                ps.ForEach(p =>
                {
                    p.Type = "Master";
                    jattrMaster.Add(jsonPaySlipattributes.tojsonmater(p));
                });
                jattr = jattrMaster.Where(j => j.type == "Master").ToList();
            }
            else
            {

                string tables = "'EntityAdditionalInfo'";
                List<PaySlipAttributes> ps = PaySlip.GetMasterFields(tables, companyId);
                List<jsonPaySlipattributes> jattrMaste = new List<jsonPaySlipattributes>();
                ps.ForEach(p =>
                {
                    p.Type = "Master";

                    var existjsps = jattr.Where(w => w.CofigurationId == p.AttributeId).FirstOrDefault();
                    if (!object.ReferenceEquals(existjsps, null))
                    {
                        jattr.Add(jsonPaySlipattributes.tojsonmater(p));
                    }
                });
                // jattr.AddRange(jattrMaste);

            }
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
                jattr.Add(jsonPaySlipattributes.tojsonmater(p));
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
            int descOrder = slAttribute.OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber;
            slAttribute.ForEach(s =>
            {
                PaySlipAttributes newPA = new PaySlipAttributes();
                newPA.FieldName = s.Id.ToString();
                newPA.DisplayAs = s.DisplayAs;
                newPA.Type = behaviorType;
                if (s.IsIncludeForGrossPay)
                {
                    descOrder = descOrder + 1;
                    if (behaviorType == "Deduction") newPA.DeductionDisplayOrder = descOrder;
                    if (behaviorType == "Earning") newPA.EarningDisplayOrder = descOrder;
                }
                newPA.IsIncludeGrossPay = s.IsIncludeForGrossPay;
                jattr.Add(jsonPaySlipattributes.tojson(newPA));

            });
            return new JsonResult { Data = jattr };
        }
        //modified by Ajithpanner on 20/11/17
        public static void GetPaySlip(PayrollHistory payhis, PaySlipList payslipattr, PayrollHistoryList cum, string PDFFilePath, int month, int year, PaySlipSetting pSetting, int companyId, int userId, string baseurl, bool singlePdf)
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


            var netpayValue = payhis.PayrollHistoryValueList.Where(d => d.AttributeModelId == NetPay.Id).ToList();
            var netpayVal = netpayValue[0].Value;
            var egValue = payhis.PayrollHistoryValueList.Where(d => d.AttributeModelId == EarnedGross.Id).ToList();
            var egVal = egValue[0].Value;
            var tdValue = payhis.PayrollHistoryValueList.Where(d => d.AttributeModelId == TotalDeduction.Id).ToList();
            var tdVal = tdValue[0].Value;



            PayrollHistoryValueList cumlist = new PayrollHistoryValueList();
            cum.ForEach(f =>
            {
                cumlist.AddRange(f.PayrollHistoryValueList);
            });
            var netpayValcum = cumlist.Where(d => d.AttributeModelId == NetPay.Id).Sum(s => Convert.ToDecimal(s.Value));
            var egValcum = cumlist.Where(d => d.AttributeModelId == EarnedGross.Id).Sum(s => Convert.ToDecimal(s.Value));
            var tdValcum = cumlist.Where(d => d.AttributeModelId == TotalDeduction.Id).Sum(s => Convert.ToDecimal(s.Value));

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
                    if ((string.IsNullOrEmpty(p.Value2) || p.Value2 == "0") && (p.Section == "Deductions" || p.Section == "Earnings"))
                    {
                        payslipattr.Remove(p);
                    }
                }
            });


            DataTable dt = CommonData.ConvertListToTable(payslipattr);

            List<tablePayslip> tblist = new List<tablePayslip>();
            if (dt.Rows.Count > 0)
            {

                for (int rowcount = 0; rowcount < dt.Rows.Count; rowcount++)
                {
                    tablePayslip tb = new tablePayslip();
                    tb.fieldName = Convert.ToString(dt.Rows[rowcount]["FieldName"]);
                    tb.displayAs = Convert.ToString(dt.Rows[rowcount]["DisplayAs"]);
                    tb.valueOne = Convert.ToString(dt.Rows[rowcount]["Value1"]);
                    tb.valueTwo = Convert.ToString(dt.Rows[rowcount]["Value2"]);
                    tb.section = Convert.ToString(dt.Rows[rowcount]["Section"]);
                    tb.diaplayOrder = Convert.ToInt16(dt.Rows[rowcount]["DisplayOrder"]);
                    tblist.Add(tb);
                }
            }

            //html
            string html = "";
            string header = "";
            string earn = "";
            string dedu = "";
            string foot = "";
            html = "<html><style>body{padding:30px;font-family: 'Times New Roman', Times, serif;}table.cl {border:none; border-collapse: collapse;}table.cl td.rw { border-left: 1px solid #000; border-right: 1px solid #000;text-transform: uppercase;text-align: right;padding-right:10px;}table.cl td.rw:first-child {border-left: none;padding:5px;padding-left:10px;text-align: left}table.cl td.rw:last-child {border-right: none;text-align: right;padding-right:10px;}" +
            "table{max-width:2480px; width:100%;border-collapse: collapse;}tr.border_top ,td.bds{border-top:1pt solid black;}tr.border_bottom ,td.bd{border-bottom:1pt solid black;}.tab{ border:1px solid black;}.pdl{padding-left:10px;}.bpt{padding-top:10px;}.bpb{padding-bottom:10px;}</style><body>";


            //Header

            var Header = new List<tablePayslip>();

            tblist.Where(w => w.section.Trim().ToLower() == "header").OrderBy(o => o.diaplayOrder).ToList().ForEach(f =>
            {
                Header.Add(f);
            });

            if (!object.ReferenceEquals(Header, null))
            {
                int count = (Header.Count() % 2) == 1 ? (Header.Count() / 2) + 1 : Header.Count() / 2;


                header = "<table style='font-size:15px;font-weight:500;'><br/>";
                for (int i = 0; i < count; i++)
                {
                    //int k = i + 2;
                    header += "<tr>";
                    string dis = ((i + 1) * 2) == (Header.Count() + 1) ? "" : Header[i + count].displayAs;
                    string val = ((i + 1) * 2) == (Header.Count() + 1) ? "" : Header[i + count].valueOne;

                    header += "<td style=\"width:6cm;\" class=\"pdl\" >" + Header[i].displayAs + "</td>";
                    header += "<td style=\"width:6cm;\" >" + Header[i].valueOne + "</td>";
                    header += "<td style=\"width:6cm;\" >" + dis + "</td>";
                    header += "<td style=\"width:6cm;\" >" + val + "</td>";


                    header += "</tr>";
                }
                header += "</table><br/>";

            }



            //earnings And deduction
            var ear = new List<tablePayslip>();
            tblist.Where(w => w.section.Trim().ToLower() == "earnings").OrderBy(o => o.diaplayOrder).ToList().ForEach(f =>
            {
                ear.Add(f);
            });
            var ded = new List<tablePayslip>();
            tblist.Where(w => w.section.Trim().ToLower() == "deductions").OrderBy(o => o.diaplayOrder).ToList().ForEach(f =>
            {
                ded.Add(f);
            });
            int ecount = tblist.Where(w => w.section.Trim().ToLower() == "earnings").OrderBy(o => o.diaplayOrder).ToList().Count();
            int dcount = tblist.Where(w => w.section.Trim().ToLower() == "deductions").OrderBy(o => o.diaplayOrder).ToList().Count();


            int counts = Math.Max(ecount, dcount);

            if (!object.ReferenceEquals(ear, null))
            {
                var egFixedSum = ear.Where(d => d.valueTwo != null && d.valueTwo != "").ToList().Sum(x => Convert.ToDecimal(x.valueTwo));

                earn = "<table class=\"cl\">";
                earn += "<tr class=\"border_bottom\">";
                earn += "<td class=\"bd rw\"><b>" + "EARNINGS" + "</b></td>";

                if (pSetting.Matchingtype.ToLower().Trim() == "cumulative")
                {
                    earn += "<td class=\"bd rw\"><b>" + "AMOUNT" + "</b></td>";
                    earn += "<td class=\"bd rw\"><b>" + "CUMULATIVE" + "</b></td>";
                }
                else if (pSetting.Matchingtype.ToLower().Trim() == "matching")
                {
                    earn += "<td class=\"bd rw\"><b>" + "FIXED" + "</b></td>";
                    earn += "<td class=\"bd rw\"><b>" + "AMOUNT" + "</b></td>";
                }
                else
                {
                    earn += "<td class=\"bd rw\"><b>" + "AMOUNT" + "</b></td>";
                }
                for (int i = 0; i < counts; i++)
                {
                    earn += "<tr>";
                    earn += "<td class=\"rw\">" + (i >= ecount ? "&nbsp&nbsp&nbsp&nbsp;" : ear[i].displayAs) + "</td>";
                    if (pSetting.Matchingtype.ToLower().Trim() == "cumulative")
                    {
                        earn += "<td class=\"rw\">" + (i >= ecount ? "&nbsp&nbsp&nbsp&nbsp;" : String.Format("{0:n}", Convert.ToDecimal(ear[i].valueOne))) + "</td>";
                        earn += "<td class=\"rw\">" + (i >= ecount ? "&nbsp&nbsp&nbsp&nbsp;" : String.Format("{0:n}", Convert.ToDecimal(ear[i].valueTwo))) + "</td>";
                    }
                    else if (pSetting.Matchingtype.ToLower().Trim() == "matching")
                    {
                        earn += "<td class=\"rw\">" + (i >= ecount ? "&nbsp&nbsp&nbsp&nbsp;" : ear[i].valueTwo != "" ? String.Format("{0:n}", Convert.ToDecimal(ear[i].valueTwo)) : "") + "</td>";
                        earn += "<td class=\"rw\">" + (i >= ecount ? "&nbsp&nbsp&nbsp&nbsp;" : String.Format("{0:n}", Convert.ToDecimal(ear[i].valueOne))) + "</td>";
                    }
                    else
                    {
                        earn += "<td class=\"rw\">" + (i >= ecount ? "&nbsp&nbsp&nbsp&nbsp;" : String.Format("{0:n}", Convert.ToDecimal(ear[i].valueOne))) + "</td>";
                    }
                    earn += "</tr>";
                }
                earn += "<tr class=\"border_top\">";
                earn += "<td class=\"bds rw\"><b>" + "Earned Gross" + "</b></td>";
                if (pSetting.Matchingtype.ToLower().Trim() == "cumulative")
                {
                    earn += "<td class=\"bds rw\"><b>" + String.Format("{0:n}", Convert.ToDecimal(egVal)) + "</b></td>";
                    earn += "<td class=\"bds rw\"><b>" + String.Format("{0:n}", Convert.ToDecimal(egValcum)) + "</b></td>";
                }
                else if (pSetting.Matchingtype.ToLower().Trim() == "matching")
                {
                    earn += "<td class=\"bds rw\"><b>" + String.Format("{0:n}", Convert.ToDecimal(egFixedSum)) + "</b></td>";
                    earn += "<td class=\"bds rw\"><b>" + String.Format("{0:n}", Convert.ToDecimal(egVal)) + "</b></td>";
                }
                else
                {
                    earn += "<td class=\"bds rw\"><b>" + String.Format("{0:n}", Convert.ToDecimal(egVal)) + "</b></td>";
                }
                earn += "</tr>";

                earn += "<tr class=\"border_top\">";
                earn += "<td class=\"bds rw\" style =\"border: none\"><b>" + "Net Pay :" + "</b></td>";

                if (pSetting.Matchingtype.ToLower().Trim() == "cumulative")
                {
                    earn += "<td class=\"bds rw\" style =\"border: none\"><b>" + String.Format("{0:n}", Convert.ToDecimal(netpayVal)) + "</b></td>";
                    earn += "<td class=\"bds rw\" style =\"border: none\"><b>" + String.Format("{0:n}", Convert.ToDecimal(netpayValcum)) + "</b></td>";
                }
                else if (pSetting.Matchingtype.ToLower().Trim() == "matching")
                {
                    //earn += "<td class=\"bds rw\">" + String.Format("{0:0.00}", Convert.ToDecimal(egFixedSum)) + "</td>";
                    earn += "<td class=\"bds rw\" style =\"border: none\">" + "" + "</td>";
                    earn += "<td class=\"bds rw\"style =\"border: none\"><b>" + String.Format("{0:n}", Convert.ToDecimal(netpayVal)) + "</b></td>";
                }
                else
                {
                    earn += "<td class=\"bds rw\"style =\"border: none\"><b>" + String.Format("{0:n}", Convert.ToDecimal(netpayVal)) + "</b></td>";
                }
                earn += "</tr>";


                earn += "</table>";
            }


            if (!object.ReferenceEquals(ded, null))
            {


                dedu = "<table class=\"cl\">";
                dedu += "<tr class=\"border_bottom\">";
                dedu += "<td style=\"border-left: 1px solid #000;\" class=\"bd rw\"><b>" + "DEDUCTIONS" + "</b></td>";
                dedu += "<td class=\"bd rw\"><b>" + "AMOUNT" + "</b></td>";
                if (pSetting.Matchingtype.ToLower().Trim() == "cumulative")
                {
                    dedu += "<td class=\"bd rw\"><b>" + "CUMULATIVE" + "</b></td>";
                }
                dedu += "</tr>";
                for (int i = 0; i < counts; i++)
                {
                    dedu += "<tr>";
                    dedu += "<td style=\"border-left:1px solid #000;\" class=\"rw\">" + (i >= dcount ? "&nbsp&nbsp&nbsp&nbsp;" : ded[i].displayAs) + "</td>";
                    dedu += "<td class=\"rw\">" + (i >= dcount ? "&nbsp;&nbsp&nbsp&nbsp" : String.Format("{0:n}", Convert.ToDecimal(ded[i].valueOne))) + "</td>";
                    if (pSetting.Matchingtype.ToLower().Trim() == "cumulative")
                    {
                        dedu += "<td class=\"rw\">" + (i >= dcount ? "&nbsp&nbsp&nbsp&nbsp;" : String.Format("{0:n}", Convert.ToDecimal(ded[i].valueTwo))) + "</td>";
                    }
                    dedu += "</tr>";

                }
                dedu += "<tr class=\"border_top\">";
                dedu += "<td  style=\"border-left:1px solid #000;\" class=\"bds rw\"><b>" + "Total Deduction" + "<b></td>";
                dedu += "<td class=\"bds rw\"><b>" + String.Format("{0:n}", Convert.ToDecimal(tdVal)) + "<b></td>";
                if (pSetting.Matchingtype.ToLower().Trim() == "cumulative")
                {
                    dedu += "<td class=\"bds rw\"><b>" + String.Format("{0:n}", Convert.ToDecimal(tdValcum)) + "<b></td>";
                }
                dedu += "</tr>";
                dedu += "<tr class=\"border_top\">";
                dedu += "<td class=\"bds rw\"style =\"border: none\">" + "&nbsp&nbsp&nbsp&nbsp;" + "</td>";
                dedu += "<td class=\"bds rw\"style =\"border: none\">" + "&nbsp&nbsp&nbsp&nbsp;" + "</td>";
                if (pSetting.Matchingtype.ToLower().Trim() == "cumulative")
                {
                    dedu += "<td class=\"bds rw\"style =\"border: none\">" + "&nbsp&nbsp&nbsp&nbsp;" + "</td>";
                }
                dedu += "</tr>";
                dedu += "</table>";
            }

            //footer

            var footer = new List<tablePayslip>();
            tblist.Where(w => w.section.Trim().ToLower() == "footer").OrderBy(o => o.diaplayOrder).ToList().ForEach(f =>
            {
                footer.Add(f);
            });
            if (!object.ReferenceEquals(footer, null))
            {
                int count = (footer.Count() % 2) == 1 ? (footer.Count() / 2) + 1 : footer.Count() / 2;

                if (count > 0)
                {
                    foot = "<table>";
                    for (int i = 0; i < count; i++)
                    {
                        //int k = i + 2;
                        foot += "<tr>";
                        string dis = ((i + 1) * 2) == (footer.Count() + 1) ? "" : footer[i + count].displayAs;
                        string val = ((i + 1) * 2) == (footer.Count() + 1) ? "" : footer[i + count].valueOne;

                        foot += "<td style=\"width:6cm;\">" + footer[i].displayAs + "</td>";
                        foot += "<td style=\"width:6cm;\">" + footer[i].valueOne + "</td>";
                        foot += "<td style=\"width:6cm;\">" + dis + "</td>";
                        foot += "<td style=\"width:6cm;\">" + val + "</td>";


                        foot += "</tr>";
                    }
                    foot += "</table>";
                }

            }


            baseurl = "src=\"" + baseurl;
            pSetting.Header = pSetting.Header.Replace("src=\"/", baseurl);
            pSetting.Footer = pSetting.Footer.Replace("src=\"/", baseurl);


            html += "<table class=\"tab\">";
            if (pSetting.Header != "")
            {
                html += "<tr class=\"border_bottom bpb\"><td class=\"bd\" colspan = \"6\"> " + pSetting.Header + " </td></tr>";
            }
            if (header != "<table></table>")
            {
                html += "<tr class=\"border_bottom bpt bpb\"><td  class=\"bd\" colspan = \"6\">" + header + " </td></tr>";
            }

            html += "<tr class=\"border_bottom bpt bpb\"><td  class=\"bd\" colspan = \"6\" style=\"text-align:center;text-transform: uppercase ;padding: 15px;\"><b> Payslip For The Month Of " + ((MonthEnum)Convert.ToInt16(month)).ToString().ToUpper() + " - " + year + "</b></td><br/></tr>";
            html += "<tr class=\"border_bottom\"><td class=\"bd\" colspan=\"3\"> " + earn + " </td><td class=\"bd\" colspan=\"3\">   " + dedu + "  </td></tr>";
            html += "<tr class=\"border_bottom\"><br/><td class=\"bd\"  colspan = \"6\" style=\"text-transform: uppercase;padding-bottom: 5px;padding-top:5px;padding-left:10px;\"> <b>RUPEES IN WORDS:</b> " + ConvertNumbertoWords((long)Convert.ToDouble(netpayVal)) + " ONLY </td><br/></tr>";
            if (foot != "<table></table>")
            {
                html += "<tr class=\"border_bottom\"><td class=\"bd\"  colspan = \"6\"> " + foot + " </td></tr>";
            }
            if (pSetting.Footer != "")
            {
                html += "<tr ><td style=\"padding-top: 5px;padding-bottom: 5px;margin:1px;\" colspan = \"6\">  " + pSetting.Footer + " </td></tr>";
            }

            html += "</table></body></html> ";



            string pdf_page_size = "A4";
            PdfPageSize pageSize =
                (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);


            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation =
                (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                pdf_orientation, true);

            int webPageWidth = 1024;

            int webPageHeight = 0;


            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;


            Employee emp = new Employee(companyId, payhis.EmployeeId);

            // set document passwords
            if (!singlePdf)
            {
                comp.CompanyName = Regex.Replace(comp.CompanyName, @"\s+", "");
                emp.FirstName = Regex.Replace(emp.FirstName, @"\s+", "");
                string compPassword = comp.CompanyName.ToUpper().Replace(" ", "").Trim();
                compPassword = comp.CompanyName.Trim().Length <= 5 ? comp.CompanyName.Trim() : comp.CompanyName.Trim().Substring(0, 5);
                string tempdate = Convert.ToString(emp.DateOfBirth.Day).Length == 1 ? "0" + Convert.ToString(emp.DateOfBirth.Day) : Convert.ToString(emp.DateOfBirth.Day);
                string tempmonth = Convert.ToString(emp.DateOfBirth.Month).Length == 1 ? "0" + Convert.ToString(emp.DateOfBirth.Month) : Convert.ToString(emp.DateOfBirth.Month);
                string userpassword = emp.FirstName.Trim().Replace(" ", "").Length <= 4 ? emp.FirstName.Replace(" ", "").Trim() + tempdate + tempmonth : emp.FirstName.Replace(" ", "").Trim().Substring(0, 4) + tempdate + tempmonth;// + tempmonth + year;

                converter.Options.SecurityOptions.OwnerPassword = compPassword.ToUpper().Trim();
                converter.Options.SecurityOptions.UserPassword = userpassword.ToUpper().Trim();
            }

            //set document permissions
            converter.Options.SecurityOptions.CanAssembleDocument = false;
            converter.Options.SecurityOptions.CanCopyContent = false;
            converter.Options.SecurityOptions.CanEditAnnotations = false;
            converter.Options.SecurityOptions.CanEditContent = false;
            converter.Options.SecurityOptions.CanFillFormFields = false;
            converter.Options.SecurityOptions.CanPrint = true;

            // create a new pdf document converting an url
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(html, "");

            // save pdf document
            byte[] pdf = doc.Save();

            // close pdf document
            doc.Close();

            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = "Document.pdf";




            using (FileStream fs = System.IO.File.Create(PDFFilePath))
            {
                fs.Write(pdf, 0, (int)pdf.Length);
            }



        }

        public ActionResult DownloadGuidelineFile()
        {

            if (!base.checkSession())
                return View("~/Views/Error/Error.cshtml");


            int companyId = Convert.ToInt32(Session["CompanyId"]);
            string filename ; string filepath ;
            if (companyId!=0)
            {
           try
                {
                    //filename ="Tax Proof Submission for FY2020-21.zip"; //Commented by Tamilvanan on 09-01-2023
                    filename = Convert.ToString(ConfigurationManager.AppSettings["IncomeTaxGuideLineFile"]); //Added by Tamilvanan on 09-01-2023
                    filepath = AppDomain.CurrentDomain.BaseDirectory + "/CompanyData/" + filename;
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
                catch(Exception ex)
                {
                    ErrorLog.Log(ex);
                     return View("~/Views/Error/NotFound.cshtml");
                }
             
            }
            return base.BuildJson(false, 0, "Error while loading", null);

        }


        //public static string ConvertNumbertoWords(long rupees)
        //{
        //    string result = "";
        //    Int64 res;
        //    if ((rupees / 10000000) > 0)
        //    {
        //        res = rupees / 10000000;
        //        rupees = rupees % 10000000;
        //        result = result + ' ' + rupeestowords(res) + " Crore";
        //    }
        //    if ((rupees / 100000) > 0)
        //    {
        //        res = rupees / 100000;
        //        rupees = rupees % 100000;
        //        result = result + ' ' + rupeestowords(res) + " Lack";
        //    }
        //    if ((rupees / 1000) > 0)
        //    {
        //        res = rupees / 1000;
        //        rupees = rupees % 1000;
        //        result = result + ' ' + rupeestowords(res) + " Thousand";
        //    }
        //    if ((rupees / 100) > 0)
        //    {
        //        res = rupees / 100;
        //        rupees = rupees % 100;
        //        result = result + ' ' + rupeestowords(res) + " Hundred";
        //    }
        //    if ((rupees) > 0)
        //    {
        //        res = rupees;
        //        rupees = rupees % 100;
        //        result = result + ' ' + rupeestowords(res) + "";
        //    }

        //    result = result + ' ' + " only";
        //    result = "( " + result + " )";
        //    return result;
        //}

        public static string ConvertNumbertoWords(long number)
        {
            if (number == 0) return "ZERO";
            if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " LAKH ";
                number %= 1000000;
                number %= 100000;
            }
            if ((number / 100000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " LAKH ";
                number %= 100000;
            
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            //if ((number / 10) > 0)  
            //{  
            // words += ConvertNumbertoWords(number / 10) + " RUPEES ";  
            // number %= 10;  
            //}  
            if (number > 0)
            {
                if (words != "") words += "AND ";
                var unitsMap = new[]
                {
            "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
        };
                var tensMap = new[]
                {
            "ZERO", "TEN", "TWENTY", "THIRTY", "FOURTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
        };
                if (number < 20) words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }

        public static string rupeestowords(Int64 rupees)
        {
            string result = "";
            if ((rupees >= 1) && (rupees <= 10))
            {
                if ((rupees % 10) == 1) result = "One";
                if ((rupees % 10) == 2) result = "Two";
                if ((rupees % 10) == 3) result = "Three";
                if ((rupees % 10) == 4) result = "Four";
                if ((rupees % 10) == 5) result = "Five";
                if ((rupees % 10) == 6) result = "Six";
                if ((rupees % 10) == 7) result = "Seven";
                if ((rupees % 10) == 8) result = "Eight";
                if ((rupees % 10) == 9) result = "Nine";
                if ((rupees % 10) == 0) result = "Ten";
            }
            if (rupees > 9 && rupees < 20)
            {
                if (rupees == 11) result = "Eleven";
                if (rupees == 12) result = "Twelve";
                if (rupees == 13) result = "Thirteen";
                if (rupees == 14) result = "Forteen";
                if (rupees == 15) result = "Fifteen";
                if (rupees == 16) result = "Sixteen";
                if (rupees == 17) result = "Seventeen";
                if (rupees == 18) result = "Eighteen";
                if (rupees == 19) result = "Nineteen";
            }
            if (rupees > 20 && (rupees / 10) == 2 && (rupees % 10) == 0) result = "Twenty";
            if (rupees > 20 && (rupees / 10) == 3 && (rupees % 10) == 0) result = "Thirty";
            if (rupees > 20 && (rupees / 10) == 4 && (rupees % 10) == 0) result = "Forty";
            if (rupees > 20 && (rupees / 10) == 5 && (rupees % 10) == 0) result = "Fifty";
            if (rupees > 20 && (rupees / 10) == 6 && (rupees % 10) == 0) result = "Sixty";
            if (rupees > 20 && (rupees / 10) == 7 && (rupees % 10) == 0) result = "Seventy";
            if (rupees > 20 && (rupees / 10) == 8 && (rupees % 10) == 0) result = "Eighty";
            if (rupees > 20 && (rupees / 10) == 9 && (rupees % 10) == 0) result = "Ninty";

            if (rupees > 20 && (rupees / 10) == 2 && (rupees % 10) != 0)
            {
                if ((rupees % 10) == 1) result = "Twenty One";
                if ((rupees % 10) == 2) result = "Twenty Two";
                if ((rupees % 10) == 3) result = "Twenty Three";
                if ((rupees % 10) == 4) result = "Twenty Four";
                if ((rupees % 10) == 5) result = "Twenty Five";
                if ((rupees % 10) == 6) result = "Twenty Six";
                if ((rupees % 10) == 7) result = "Twenty Seven";
                if ((rupees % 10) == 8) result = "Twenty Eight";
                if ((rupees % 10) == 9) result = "Twenty Nine";
            }
            if (rupees > 20 && (rupees / 10) == 3 && (rupees % 10) != 0)
            {
                if ((rupees % 10) == 1) result = "Thirty One";
                if ((rupees % 10) == 2) result = "Thirty Two";
                if ((rupees % 10) == 3) result = "Thirty Three";
                if ((rupees % 10) == 4) result = "Thirty Four";
                if ((rupees % 10) == 5) result = "Thirty Five";
                if ((rupees % 10) == 6) result = "Thirty Six";
                if ((rupees % 10) == 7) result = "Thirty Seven";
                if ((rupees % 10) == 8) result = "Thirty Eight";
                if ((rupees % 10) == 9) result = "Thirty Nine";
            }
            if (rupees > 20 && (rupees / 10) == 4 && (rupees % 10) != 0)
            {
                if ((rupees % 10) == 1) result = "Forty One";
                if ((rupees % 10) == 2) result = "Forty Two";
                if ((rupees % 10) == 3) result = "Forty Three";
                if ((rupees % 10) == 4) result = "Forty Four";
                if ((rupees % 10) == 5) result = "Forty Five";
                if ((rupees % 10) == 6) result = "Forty Six";
                if ((rupees % 10) == 7) result = "Forty Seven";
                if ((rupees % 10) == 8) result = "Forty Eight";
                if ((rupees % 10) == 9) result = "Forty Nine";
            }
            if (rupees > 20 && (rupees / 10) == 5 && (rupees % 10) != 0)
            {
                if ((rupees % 10) == 1) result = "Fifty One";
                if ((rupees % 10) == 2) result = "Fifty Two";
                if ((rupees % 10) == 3) result = "Fifty Three";
                if ((rupees % 10) == 4) result = "Fifty Four";
                if ((rupees % 10) == 5) result = "Fifty Five";
                if ((rupees % 10) == 6) result = "Fifty Six";
                if ((rupees % 10) == 7) result = "Fifty Seven";
                if ((rupees % 10) == 8) result = "Fifty Eight";
                if ((rupees % 10) == 9) result = "Fifty Nine";
            }
            if (rupees > 20 && (rupees / 10) == 6 && (rupees % 10) != 0)
            {
                if ((rupees % 10) == 1) result = "Sixty One";
                if ((rupees % 10) == 2) result = "Sixty Two";
                if ((rupees % 10) == 3) result = "Sixty Three";
                if ((rupees % 10) == 4) result = "Sixty Four";
                if ((rupees % 10) == 5) result = "Sixty Five";
                if ((rupees % 10) == 6) result = "Sixty Six";
                if ((rupees % 10) == 7) result = "Sixty Seven";
                if ((rupees % 10) == 8) result = "Sixty Eight";
                if ((rupees % 10) == 9) result = "Sixty Nine";
            }
            if (rupees > 20 && (rupees / 10) == 7 && (rupees % 10) != 0)
            {
                if ((rupees % 10) == 1) result = "Seventy One";
                if ((rupees % 10) == 2) result = "Seventy Two";
                if ((rupees % 10) == 3) result = "Seventy Three";
                if ((rupees % 10) == 4) result = "Seventy Four";
                if ((rupees % 10) == 5) result = "Seventy Five";
                if ((rupees % 10) == 6) result = "Seventy Six";
                if ((rupees % 10) == 7) result = "Seventy Seven";
                if ((rupees % 10) == 8) result = "Seventy Eight";
                if ((rupees % 10) == 9) result = "Seventy Nine";
            }
            if (rupees > 20 && (rupees / 10) == 8 && (rupees % 10) != 0)
            {
                if ((rupees % 10) == 1) result = "Eighty One";
                if ((rupees % 10) == 2) result = "Eighty Two";
                if ((rupees % 10) == 3) result = "Eighty Three";
                if ((rupees % 10) == 4) result = "Eighty Four";
                if ((rupees % 10) == 5) result = "Eighty Five";
                if ((rupees % 10) == 6) result = "Eighty Six";
                if ((rupees % 10) == 7) result = "Eighty Seven";
                if ((rupees % 10) == 8) result = "Eighty Eight";
                if ((rupees % 10) == 9) result = "Eighty Nine";
            }
            if (rupees > 20 && (rupees / 10) == 9 && (rupees % 10) != 0)
            {
                if ((rupees % 10) == 1) result = "Ninty One";
                if ((rupees % 10) == 2) result = "Ninty Two";
                if ((rupees % 10) == 3) result = "Ninty Three";
                if ((rupees % 10) == 4) result = "Ninty Four";
                if ((rupees % 10) == 5) result = "Ninty Five";
                if ((rupees % 10) == 6) result = "Ninty Six";
                if ((rupees % 10) == 7) result = "Ninty Seven";
                if ((rupees % 10) == 8) result = "Ninty Eight";
                if ((rupees % 10) == 9) result = "Ninty Nine";
            }
            return result;
        }



        public static void GetForm16partBReport(List<rptWorkSheet> dt, string PDFFilePath, int companyId, int userId, Employee emp, string companyName, Form16PartBGrossValues dtGross, string inchargeEmpName, string inchargeDesg, string txYr, string inchargeFatherName)
        {

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
            decimal underivTotal = Convert.ToDecimal(dt.Where(x => x.FormulaType == 5).ToList().Sum(x => x.Actual)) + Convert.ToDecimal(dt.Where(x => x.FormulaType == 7).ToList().Sum(x => x.Actual));
            decimal temp = Convert.ToDecimal(dt.Where(x => x.FormulaType == 4).ToList().Sum(x => x.Actual)) + Convert.ToDecimal(dt.Where(x => x.FormulaType == 6).ToList().Sum(x => x.Actual));
            underivTotal = underivTotal + temp;
            ReportDataSource rptDs = new ReportDataSource("Form16PartB", dt);
            rpt.DataSources.Add(rptDs);
            ReportParameterCollection rpcollection = new ReportParameterCollection();
            rpcollection.Add(new ReportParameter("EmployeeName", emp.FirstName + " " + emp.LastName));
            rpcollection.Add(new ReportParameter("FatherName", inchargeFatherName));
            rpcollection.Add(new ReportParameter("CompanyName", companyName));
            rpcollection.Add(new ReportParameter("PAN", emp.EmployeePersonal.PANNumber));
            rpcollection.Add(new ReportParameter("FINYear", txYr));
            rpcollection.Add(new ReportParameter("Amount", dtGross.TaxPayable));
            rpcollection.Add(new ReportParameter("Date", DateTime.Now.ToString("dd/MM/yyyy")));
            rpcollection.Add(new ReportParameter("InChargeName", inchargeEmpName));
            rpcollection.Add(new ReportParameter("InChargeDesignation", inchargeDesg));
            rpcollection.Add(new ReportParameter("EmpCode", emp.EmployeeCode));

            rpcollection.Add(new ReportParameter("Grosssalary", dtGross.Grosssalary));
            rpcollection.Add(new ReportParameter("ECess", dtGross.Ecess));
            rpcollection.Add(new ReportParameter("section10", dtGross.section10));
            rpcollection.Add(new ReportParameter("section16", dtGross.section16));
            rpcollection.Add(new ReportParameter("Section80C", dtGross.Section80C));
            rpcollection.Add(new ReportParameter("Section80CC", dtGross.Section80CC));
            rpcollection.Add(new ReportParameter("Section80CCD", dtGross.Section80CCD));
            rpcollection.Add(new ReportParameter("Mediclaim80D", dtGross.Mediclaim80D));
            rpcollection.Add(new ReportParameter("UnderChapterVIA", Convert.ToString(underivTotal)));
            rpcollection.Add(new ReportParameter("TotalIncome", dtGross.TotalIncome));
            rpcollection.Add(new ReportParameter("OtherIncomeTotal", dtGross.OtherIncomeTotal));
            rpcollection.Add(new ReportParameter("TaxOnTotalIncome", dtGross.TaxOnTotalIncome));
            rpcollection.Add(new ReportParameter("TaxPayable", dtGross.TaxPayable));
            rpcollection.Add(new ReportParameter("Surcharge", dtGross.Surcharge));
            rpcollection.Add(new ReportParameter("GrossSection1", dtGross.GrossSection1));
            rpcollection.Add(new ReportParameter("GrossSection2", dtGross.GrossSection2));
            rpcollection.Add(new ReportParameter("GrossSection3", dtGross.GrossSection3));
            rpcollection.Add(new ReportParameter("FinalTaxPayable", dtGross.FinalTaxPayable));
            rpcollection.Add(new ReportParameter("Section89", dtGross.section89));
            rpcollection.Add(new ReportParameter("Section87", dtGross.section87));
            rpt.ReportPath = "Reports/Form16PartB.rdlc";

            rpt.SetParameters(rpcollection);
            rpt.Refresh();
            byte[] renderedBytes = null;

            renderedBytes = rpt.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);



            string contentype = mimeType;


            using (FileStream fs = System.IO.File.Create(PDFFilePath))
            {
                fs.Write(renderedBytes, 0, (int)renderedBytes.Length);
            }


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
            if (isDetail && object.ReferenceEquals(groupby, null))
            {
                paysheetattr.Insert(0, new jsonPaySheetattr()
                {
                    paysheetId = paysheetattr.FirstOrDefault().paysheetId,
                    id = paysheetattr.FirstOrDefault().id,
                    displayAs = "EmployeeCode",
                    fieldName = "Employee EmployeeCode",
                    tableName = "Employee",
                    type = "Mater",
                    order = 1

                });
                paysheetattr.Insert(1, new jsonPaySheetattr()
                {
                    paysheetId = paysheetattr.FirstOrDefault().paysheetId,
                    id = paysheetattr.FirstOrDefault().id,
                    displayAs = "FirstName",
                    fieldName = "Employee FirstName",
                    tableName = "Employee",
                    type = "Mater",
                    order = 1

                });
            }
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

            var check = paysheetattr.Where(d => d.fieldName.ToLower() == "employee employeecode").ToList();
            if (check == null)
            {
                paysheetattr.Insert(smonth == nMonth && syear == nYear ? 0 : 1, new jsonPaySheetattr { fieldName = "employee employeecode", displayAs = "Emp Code" });
            }
            check = paysheetattr.Where(d => d.fieldName.ToLower() == "employee firstname").ToList();
            if (check == null)
            {
                paysheetattr.Insert(smonth == nMonth && syear == nYear ? 1 : 2, new jsonPaySheetattr { fieldName = "employee firstname", displayAs = "Emp Name" });
            }
            DateTime sdate = new DateTime((int)syear, smonth, 1);// DateTime.Parse("8/13/2010 8:33:21 AM");
            DateTime eDate = new DateTime((int)nYear, nMonth, 1);
            if (IsDetail && sdate != eDate)
            {
                paysheetattr.ForEach(a =>
                {
                    if (a.type.ToLower() == "detail" || a.tableName.ToLower() == "emp_bank")
                    {
                        DateTime startDate = new DateTime((int)syear, smonth, 1);// DateTime.Parse("8/13/2010 8:33:21 AM");
                        DateTime endDate = new DateTime((int)nYear, nMonth, 1);
                        string tempCol = a.displayAs;
                        string fieldName = tempCol;
                        do
                        {
                            a.month = startDate.Month;
                            a.year = startDate.Year;
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
                    a.month = sdate.Month;
                    a.year = sdate.Year;
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
            //     DetailStartIndex = DetailStartIndex - 1;

            foreach (string st in categories)
            {
                emp.AddRange((new EmployeeList(companyId, new Guid(st), (from k in filterExpr where string.Compare(k.Key, "employee", true) == 0 select k.Value.Replace("Employee ", "Emp.")).FirstOrDefault(), userId, new Guid(Convert.ToString(Session["EmployeeGUID"])))));
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
            AttributeModelList at = new AttributeModelList(companyId);
            PayrollHistoryList history = new PayrollHistoryList();
            if (syear != null && nYear != null)
                history = new PayrollHistoryList(companyId, syear.Value, smonth, nYear.Value, nMonth, Guid.Empty);
            PayrollHistoryList payrollHistory = new PayrollHistoryList();
            payrollHistory.AddRange(history.OrderBy(p => p.Month).ToList());

            // emp.RemoveAll(e => e.SeparationDate!=DateTime.MinValue && e.SeparationDate < new DateTime((int)nYear, nMonth, 1));

            EntityModel entityModel = new EntityModel("AddtionalInfo", companyId);

            EmployeeList eplist = new EmployeeList(companyId);
            string Id = "ALL";
            Emp_BankList empbanklist = new Emp_BankList(Id);
            EmployeeAddressList empaddrlist = new EmployeeAddressList(Id);
            Emp_PersonalList emppersonallist = new Emp_PersonalList(Id);
            EntityAdditionalInfoList empAddInfoList = new EntityAdditionalInfoList(companyId, entityModel.Id, Guid.Empty);
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("PayrollHistoryId", typeof(Guid));
            payrollHistory.ForEach(p =>
            {
                dt1.Rows.Add(p.Id);
            });
            var paytranList = new PaytranList(dt1);
            


            emp.OrderBy(e => e.EmployeeCode).ToList().ForEach(e =>
              {
                  string bankname = string.Empty;
                  string accNo = string.Empty;
                  bool nopay = false;
                  List<PayrollHistory> pay = payrollHistory.Where(p => p.EmployeeId == e.Id).ToList();
                  if (pay == null || pay.Count == 0)
                  {
                      pay = new List<PayrollHistory>();
                      pay.Add(new PayrollHistory());
                      nopay = true;
                  }

                  EmployeeList ep1 = new EmployeeList();
                  var empbank = empbanklist.Where(ebl => ebl.EmployeeId == e.Id && ebl.IsActive == false).FirstOrDefault();
                  ep1.Add(eplist.Where(el => el.Id == e.Id).FirstOrDefault());
                 // Guid payid = new Guid(pay.Where(p => p.EmployeeId == e.Id).FirstOrDefault().Id.ToString());
                  List<PayrollTransaction> eptran = new List<PayrollTransaction>();
                  eptran =  paytranList.Where(pt => pt.EmployeeId == e.Id).ToList();
                  
 /*                 if (pay.Count == 1 && pay.Count > 0)
                  {

                      // Employee ep = new Employee(companyId, e.Id);
                      
                      ep1 = PayrollTransaction.GetEmployeeTrasaction("Employee", (dynamic)e, pay[0].Id,banklist,empbanklist,empaddrlist,emppersonallist);
                      if (!object.ReferenceEquals(e.EmployeeBankList.AcctNo, null))
                      {
                        //  if (!accNo.Contains(e.EmployeePersonal.BankAccountNo))
                              accNo = accNo + e.EmployeeBankList.AcctNo.ToString();
                      }
                      if (!object.ReferenceEquals(e.EmployeeBankList.BankId, null))
                      {
                          bankname = bankname + e.EmployeeBankList.BankId.ToString();
                      }
                  }
                  else
                  {
                     

                      pay.ToList().ForEach(f =>
                      {
                          ep = PayrollTransaction.GetEmployeeTrasaction("Employee", (dynamic)ep, f.Id,banklist,empbanklist,empaddrlist,emppersonallist);
                          if (!object.ReferenceEquals(ep.EmployeePersonal.BankAccountNo, null))
                          {
                              if (!accNo.Contains(ep.EmployeePersonal.BankAccountNo))
                                  accNo = accNo  + ep.EmployeePersonal.BankAccountNo.ToString()+ ",";
                          }
                          if (!object.ReferenceEquals(ep.EmployeePersonal.Bank, null))
                          {
                              if (!bankname.Contains(ep.EmployeePersonal.Bank))
                                  bankname = bankname + ep.EmployeePersonal.Bank.ToString() + ",";
                          }
                     
                                        //  ep.EmployeeBankList.Id
                      });
                  }*/

                  // pay.ForEach(payHistory =>
                  //  {


                  bool continueList = true;
                  string bankfilter = (from k in filterExpr where string.Compare(k.Key, "emp_bank", true) == 0 select k.Value).FirstOrDefault();
                  string addrfilter = (from k in filterExpr where string.Compare(k.Key, "emp_Address", true) == 0 select k.Value).FirstOrDefault();
                  string personalFilter = (from k in filterExpr where string.Compare(k.Key, "emp_personal", true) == 0 select k.Value).FirstOrDefault();

                  List<PayrollError> payErrors = new List<PayrollError>();


                  //                string strwhere = bankfilter.ToString();
                  //                  var empbank1 = empbanklist.Where(strwhere);
                  // Emp_BankList empbank = new Emp_BankList(e.Id, bankfilter);
                  int empaddr_count = 0;
                  var empaddr = empaddrlist.Where(eal => eal.EmployeeId == e.Id).FirstOrDefault();
                  if (!ReferenceEquals(empaddr, null) && (!ReferenceEquals(filters, null)))
                  {
                      filters.Where(f => f.tableName.ToLower() == "emp_Address").ToList().ForEach(f =>
                      {
                          string[] fldname = f.fieldName.Split(' ');
                          var addrval = empaddr.GetType().GetProperty(fldname[1]).GetValue(empaddr);
                          string expr = "if " + Convert.ToString(addrval) + f.operation + Convert.ToString(f.value) + " THEN true";

                          ifElseStmt obj = new ifElseStmt();
                          List<ifElseStmt> iflst = obj.MatchGetifElse(expr);
                          iflst = obj.MatchExecution(iflst);
                          var result = iflst.Where(l => l.CorrectExecuteionBlock == true).FirstOrDefault();
                          if (!ReferenceEquals(result, null))
                          {
                              if (result.thenVal.Trim().ToLower() == "true")
                              {
                                  empaddr_count = empaddr_count + 1;
                              }
                          }
                      });
                  }

                  int emppersonal_count = 0;
                  var emppersonal = emppersonallist.Where(epl => epl.EmployeeId == e.Id).FirstOrDefault();
                  if (!ReferenceEquals(emppersonal, null) && (!ReferenceEquals(filters, null)))
                  {
                      filters.Where(f => f.tableName.ToLower() == "emp_personal").ToList().ForEach(f =>
                      {
                          string[] fldname = f.fieldName.Split(' ');
                          var personalval = emppersonallist.GetType().GetProperty(fldname[1]).GetValue(empaddr);
                          string expr = "if " + Convert.ToString(personalval) + f.operation + Convert.ToString(f.value) + " THEN true";

                          ifElseStmt obj = new ifElseStmt();
                          List<ifElseStmt> iflst = obj.MatchGetifElse(expr);
                          iflst = obj.MatchExecution(iflst);
                          var result = iflst.Where(l => l.CorrectExecuteionBlock == true).FirstOrDefault();
                          if (!ReferenceEquals(result, null))
                          {
                              if (result.thenVal.Trim().ToLower() == "true")
                              {
                                  emppersonal_count = emppersonal_count + 1;
                              }
                          }
                      });
                  }



                  // EmployeeAddressList empaddr = e.EmployeeAddressList.Count == 0 ? new EmployeeAddressList(e.Id, addrfilter) : e.EmployeeAddressList;
                  // Emp_Personal emppersonal = e.EmployeePersonal == null ? new Emp_Personal(e.Id, personalFilter) : e.EmployeePersonal;

                  var empAddInfo = empAddInfoList.Where(el => el.EmployeeId == e.Id).FirstOrDefault();
                  // EntityAdditionalInfoList empAddInfoList = new EntityAdditionalInfoList(companyId, entityModel.Id, e.Id);


                  if (!string.IsNullOrEmpty(addrfilter))
                  {
                      if (empaddr_count == 0)
                      {
                          continueList = false;
                      }
                  }
                  if (!string.IsNullOrEmpty(personalFilter))
                  {
                      if (emppersonal_count == 0)
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

                            char[] splitchar = { ' ' };
                            string[] split = a.FieldName.Split(splitchar);

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
                                    if (split.Count() > 1)
                                    {
                                        a.FieldName = split[1];
                                    }
                                    newps.DisplayAs = string.IsNullOrEmpty(a.DisplayAs) ? a.FieldName : a.DisplayAs;
                                    if (categoryList.Count > 0)
                                    {

                                        newps.Value = (categoryList.Where(c => c.Id == e.CategoryId).FirstOrDefault().Name);
                                        newps.OrderBy = a.OrderBy;
                                        newps.Type = a.Type;
                                    }
                                    break;
                                case "employee":
                                    if (split.Count() > 1)
                                    {
                                        a.FieldName = split[1];
                                    }
                                    newps.DisplayAs = string.IsNullOrEmpty(a.DisplayAs) ? a.FieldName : a.DisplayAs;
                                    var type = e.GetType();
                                    newps.Value = (Convert.ToString(e.GetType().GetProperty(a.FieldName).GetValue(e, null)));
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

                                case "emp_personal":
                                    if (split.Count() > 1)
                                    {
                                        a.FieldName = split[1];
                                    }
                                    newps.DisplayAs = string.IsNullOrEmpty(a.DisplayAs) ? a.FieldName : a.DisplayAs;
                                    if (!ReferenceEquals(emppersonal, null))
                                    {
                                        newps.Value = Convert.ToString(emppersonal.GetType().GetProperty(a.FieldName).GetValue(emppersonal, null));
                                    }
                                    newps.OrderBy = a.OrderBy;
                                    newps.Type = a.Type;
                                    break;

                                case "emp_bank":
                                    {
                                        break;
                                    }

                                case "emp_address":
                                    if (split.Count() > 1)
                                    {
                                        a.FieldName = split[1];
                                    }
                                    newps.DisplayAs = string.IsNullOrEmpty(a.DisplayAs) ? a.FieldName : a.DisplayAs;
                                    if (!ReferenceEquals(empaddr,null))
                                    {
                                        newps.DisplayAs = string.IsNullOrEmpty(a.DisplayAs) ? a.FieldName : a.DisplayAs;
                                        newps.Value = Convert.ToString(empaddr.GetType().GetProperty(a.FieldName).GetValue(empaddr, null));
                                        newps.Type = a.Type;
                                    }
                                    break;

                                case "additionalinfo":
                                    if (split.Count() > 1)
                                    {
                                        a.FieldName = split[1];
                                    }
                                    newps.DisplayAs = string.IsNullOrEmpty(a.DisplayAs) ? a.FieldName : a.DisplayAs;
                                    if (!ReferenceEquals(empAddInfo,null))
                                    {
                                        for (int cnt = 0; cnt < empAddInfoList.Count; cnt++)
                                        {

                                            if (a.FieldName == empAddInfoList[cnt].AttributeModelId.ToString())
                                            {

                                                AttributeModel at1 = at.Where(al=>al.Id == empAddInfoList[cnt].AttributeModelId).FirstOrDefault();
                                                newps.Value = empAddInfoList[cnt].Value;
                                                newps.DisplayAs = string.IsNullOrEmpty(a.DisplayAs) ? a.FieldName : a.DisplayAs;
                                                newps.DisplayAs = string.IsNullOrEmpty(a.DisplayAs) ? at1.DisplayAs : a.DisplayAs;
                                                newps.Type = a.Type;
                                            }
                                        }
                                    }
                                    break;
                            }

                            if (a.Type == "Master" || a.Type.ToLower() == "group")
                            {
                              /*  if (a.FieldName == "BankId")
                                {
                                    newps.Value = bankname.TrimEnd(',');
                                }
                                  if (a.FieldName == "AcctNo")
                                  {
                                      newps.Value = accNo.TrimEnd(',');
                                  }*/

                                if (a.DisplayAs == newps.DisplayAs) ps.Add(newps);
                            }
                            else
                            {
                                if (a.Type.ToLower() != "master" && a.Type.ToLower() != "mater")
                                {
                                    decimal attVal = 0;
                                    pay.ForEach(payHistory =>
                                    {

                                        if (!object.ReferenceEquals(payHistory, null) || DetailStartIndex == 0)
                                        {
                                            if (payHistory.Status == ComValue.payrollProcessStatus[0] || DetailStartIndex == 0 || nopay ||
                                            payHistory.Status == ComValue.payrollProcessStatus[1])
                                            {
                                                if (a.FieldName.ToLower() == "month")
                                                {
                                                    newps.Value = Convert.ToString((MonthEnum)payHistory.Month);
                                                    newps.DisplayAs = "Month";
                                                }


                                            //Assign payroll History                       
                                          //  for (int cnt = 0; cnt < payHistory.PayrollHistoryValueList.Count; cnt++)
                                            //    {
                                                    Guid field = new Guid(a.FieldName);
                                                var payhisvalue = "";
                                                PayrollHistoryValue payval = new PayrollHistoryValue();
                                                if (payHistory.PayrollHistoryValueList.Count > 0)
                                                {
                                                    string value = " ";
                                                    Guid payid = payHistory.PayrollHistoryValueList.PayrollHistroyId;
                                                    value = check_bank(pay, banklist, filters, payid, eptran, empbank, columnHeader, ep1, sdate, eDate, ps);

                                                    if (value != "false")
                                                    {
                                                        payval = payHistory.PayrollHistoryValueList.Where(ph => ph.AttributeModelId == field).FirstOrDefault();
                                                        if (!ReferenceEquals(payval, null))
                                                        {
                                                            payhisvalue = payHistory.PayrollHistoryValueList.Where(ph => ph.AttributeModelId == field).FirstOrDefault().Value.ToString();
                                                        }


                                                        // if (a.FieldName == payHistory.PayrollHistoryValueList[cnt].AttributeModelId.ToString())
                                                        // {
                                                        string temp = string.IsNullOrEmpty(a.DisplayAs) ? a.DisplayAs : a.DisplayAs;
                                                        var strTemp = temp.Split('(');
                                                        string tempDisplayAs = strTemp[0];

                                                        // AttributeModel at1 = at.Where(al => al.Id == payHistory.PayrollHistoryValueList[cnt].AttributeModelId).FirstOrDefault();
                                                        AttributeModel at1 = at.Where(al => al.Id == field).FirstOrDefault();
                                                        Paysheetatrr newps1 = new Paysheetatrr();
                                                        newps1.EmpCode = e.EmployeeCode;
                                                        newps1.EmpName = e.FirstName + " " + e.LastName;
                                                        newps1.Type = a.Type;
                                                        newps1.Month = payHistory.Month;
                                                        newps1.Year = payHistory.Year;
                                                        if (IsDetail && sdate != eDate)
                                                        {
                                                            newps1.DisplayAs = string.IsNullOrEmpty(tempDisplayAs) ? at1.DisplayAs + "(" + (MonthEnum)payHistory.Month + "_" + payHistory.Year + ")" : tempDisplayAs + "(" + (MonthEnum)payHistory.Month + "_" + payHistory.Year + ")";
                                                            // newps1.Value = payHistory.PayrollHistoryValueList[cnt].Value;
                                                            newps1.Value = payhisvalue;
                                                        }
                                                        else if (IsDetail && sdate == eDate)
                                                        {
                                                            newps1.DisplayAs = string.IsNullOrEmpty(tempDisplayAs) ? at1.DisplayAs : tempDisplayAs;
                                                            // newps1.Value = payHistory.PayrollHistoryValueList[cnt].Value;
                                                            newps1.Value = payhisvalue;
                                                        }
                                                        else
                                                        {
                                                            newps1.DisplayAs = string.IsNullOrEmpty(tempDisplayAs) ? at1.DisplayAs : tempDisplayAs;
                                                            // attVal = Convert.ToDecimal(string.IsNullOrEmpty(payHistory.PayrollHistoryValueList[cnt].Value) ? 0 : Convert.ToDecimal(payHistory.PayrollHistoryValueList[cnt].Value));
                                                            attVal = Convert.ToDecimal(string.IsNullOrEmpty(payhisvalue) ? 0 : Convert.ToDecimal(payhisvalue));
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
                                                   // }
                                               // }

                                            }
                                        }
                                    });
                                }
                                //if (continueList)
                                //{
                                //    ps.Add(newps);
                                //}
                            }
                        });//End attr

                      //Add Row
                      if (continueList && ps.Count > 0)
                      {
                          if (ps.Where(d => d.EmpCode == e.EmployeeCode && d.Type == "Detail").ToList().Count > 0)
                              paysheetDataView.AddDataRow(ps.Where(d => d.EmpCode == e.EmployeeCode).ToList(), isDetail, groupby);
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
                //DataRow dtrow = dtFinal.NewRow();

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
                    int removemastergroupcol = GroupCnt + MasterCnt;
                    for (int i = 0; i < removemastergroupcol; i++)
                    {
                        paysheetDataView.PaySheetDataView.Columns.RemoveAt(i);
                    }
                    dtFinal = paysheetDataView.PaySheetDataView.Clone();
                    // dtFinal.Columns.Remove("month");
                    DataRow dtrow1 = dtFinal.NewRow();
                    dtFinal.Rows.Add(dtrow1);

                    for (int i = 0; i < dtFinal.Rows.Count; i++)
                    {
                        for (int j = 0; j < paysheetDataView.PaySheetDataView.Columns.Count; j++)
                        {
                            //  dtrow = dtFinal.NewRow();
                            var value = paysheetDataView.PaySheetDataView.AsEnumerable().Where(x => x[paysheetDataView.PaySheetDataView.Columns[j].ColumnName] != DBNull.Value).Sum(row => Convert.ToDecimal(row.Field<string>(paysheetDataView.PaySheetDataView.Columns[j].ColumnName)));
                            dtFinal.Rows[i][paysheetDataView.PaySheetDataView.Columns[j].ColumnName] = value;
                        }
                    }
                    if (!dtFinal.Columns.Contains("No fo Employees"))
                    {
                        dtFinal.Columns.Add("No fo Employees").SetOrdinal(0);
                        dtFinal.Rows[0]["No fo Employees"] = paysheetDataView.PaySheetDataView.Rows.Count;

                    }
                    else
                    {
                        dtFinal.Rows[0]["No fo Employees"] = paysheetDataView.PaySheetDataView.Rows.Count;
                    }
                }
                if (smonth == nMonth && syear == nYear)
                    title = title + " " + (MonthEnum)smonth + " " + syear;
                else
                    title = title + " " + (MonthEnum)smonth + " " + syear + " - " + (MonthEnum)nMonth + " " + nYear;

                path = generateExcel(dtFinal, title);
                // Session["dtdatawizard"] = ps; // added for report viewer 
            }
            //}
            return BuildJson(true, 1, "File Saved Successfully", new { filePath = path });
        }

        public string check_bank(List<PayrollHistory> payhistory,BankList banklist,List<jsonPaySheetattr> filters,Guid payid,List<PayrollTransaction> eptran,Emp_Bank empbank,List<Paysheetatrr> columnheader,EmployeeList ep1,DateTime sdate,DateTime edate,List<Paysheetatrr> ps)
        {
            string value = "";
            PayrollHistory payhis = payhistory.Where(ph => ph.Id == payid).FirstOrDefault();
            Guid empid = Guid.Empty;
            if (!ReferenceEquals(eptran, null) && eptran.Count > 0)
            {
                empid = new Guid(eptran.Find(et => et.EmployeeId == payhis.EmployeeId).EmployeeId.ToString());
                Employee ep2 = ep1.Where(el => el.Id == empid).FirstOrDefault();
                var month = payhis.Month;
                var year = payhis.Year;
                List<Paysheetatrr> columnheader1 = columnheader.Where(ch => ch.TableName.ToLower() == "emp_bank" && ch.Month == payhis.Month && ch.Year == payhis.Year).ToList();
                List<PayrollTransaction> paytran = eptran.Where(et => et.EmployeeId == payhis.EmployeeId && et.PayrollHistoryId == payid).ToList();
                if (!ReferenceEquals(paytran, null))
                {
                    if (!ReferenceEquals(filters, null) && filters.Count > 0)
                    {
                        filters.Where(f => f.tableName.ToLower() == "emp_bank").ToList().ForEach(f =>
                        {
                            char[] split1 = { ' ' };
                            string[] split2 = f.fieldName.Split(split1);
                            if (split2.Count() > 1)
                            {
                                if (split2[1].ToLower() == "bankname")
                                {
                                    split2[1] = "bankid";
                                }
                            }
                            PayrollTransaction ptran1 = paytran.Where(pt => pt.ColumnName.ToLower() == split2[1].ToLower()).FirstOrDefault();
                            var val2 = "";
                            if (!ReferenceEquals(ptran1, null))
                            {
                                val2 = ptran1.Value;
                            }

                            if (payhis.Year < 2021 && payhis.Month < 06 || val2 == "")
                            {
                                if (!ReferenceEquals(empbank, null))
                                {
                                    if (split2[1].ToLower() == "bankid" && val2 == "")
                                    {
                                        val2 = empbank.BankId.ToString();
                                    }

                                    if (split2[1].ToLower() == "branchname" && val2 == "")
                                    {
                                        val2 = empbank.BranchName;
                                    }
                                }
                            }

                            if (split2[1].ToLower() == "bankid" && val2 != "")
                            {
                                val2 = banklist.Where(d => d.Id == new Guid(val2)).FirstOrDefault() == null ? "" : banklist.Where(d => d.Id == new Guid(val2)).FirstOrDefault().BankName;
                            }


                            string expr = "if " + Convert.ToString(val2) + f.operation + Convert.ToString(f.value) + " THEN true";

                            ifElseStmt obj = new ifElseStmt();
                            List<ifElseStmt> iflst = obj.MatchGetifElse(expr);
                            iflst = obj.MatchExecution(iflst);
                            var result = iflst.Where(l => l.CorrectExecuteionBlock == true).FirstOrDefault();
                            if (!ReferenceEquals(result, null))
                            {
                                if (result.thenVal.Trim().ToLower() == "true")
                                {
                                    value = "true";
                                }
                                else
                                {
                                    value = "false";
                                    return;
                                }
                            }
                            else
                            {
                                value = "false";
                                return;
                            }
                        });
                    }

                    if (value != "false")
                    {

                        columnheader1.ForEach(col =>
                         {
                             char[] splitchar = { ' ' };
                             string[] split = col.FieldName.Split(splitchar);
                             if (split.Count() > 1)
                             {
                                 if (split[1].ToLower() == "bankname")
                                 {
                                     split[1] = "bankid";
                                 }
                             }
                             Paysheetatrr newps1 = new Paysheetatrr();
                             if (split.Count() > 1)
                             {
                                 col.FieldName = split[1];
                             }
                             PayrollTransaction ptran = paytran.Where(pt => pt.ColumnName.ToLower() == col.FieldName.ToLower()).FirstOrDefault();
                             newps1.EmpCode = ep2.EmployeeCode;
                             newps1.EmpName = ep2.FirstName + " " + ep2.LastName;
                             newps1.Type = col.Type;
                             newps1.Month = payhis.Month;
                             newps1.Year = payhis.Year;
                             var val = "";
                             if (!ReferenceEquals(ptran, null))
                             {
                                 val = ptran.Value;
                             }

                             if (col.Year < 2021 && col.Month < 06 || val == "")
                             {
                                 if (!ReferenceEquals(empbank, null))
                                 {
                                     if (col.FieldName.ToLower() == "bankid" && val == "")
                                     {
                                         val = empbank.BankId.ToString();
                                     }

                                     if (col.FieldName.ToLower() == "branchname" && val == "")
                                     {
                                         val = empbank.BranchName;
                                     }
                                 }
                             }

                             if (col.FieldName.ToLower() == "bankid" && val != "")
                             {
                                 val = banklist.Where(d => d.Id == new Guid(val)).FirstOrDefault() == null ? "" : banklist.Where(d => d.Id == new Guid(val)).FirstOrDefault().BankName;
                             }


                             if (IsDetail && sdate != edate && col.Month == payhis.Month && col.Year == payhis.Year)
                             {
                                 newps1.DisplayAs = string.IsNullOrEmpty(col.DisplayAs) ? col.FieldName : col.DisplayAs;
                                 newps1.Value = val;
                             }
                             else
                                 if (IsDetail && sdate == edate && col.Month == payhis.Month && col.Year == payhis.Year)
                             {
                                 newps1.DisplayAs = string.IsNullOrEmpty(col.DisplayAs) ? col.FieldName : col.DisplayAs;
                                 newps1.Value = val;
                             }
                             ps.Add(newps1);
                         });
                    }
                }
            }
            return value;
        }
        
        public string convString(string value)
        {
            decimal decVal = 0;
            if (!value.Contains("'"))
            {
                if (!string.IsNullOrEmpty(value) && decimal.TryParse(value.Substring(0, 1), out decVal))
                {
                    if (decVal == 0)
                    {
                        value = "'" + value.Replace("'", string.Empty);
                    }
                }
            }

            return value;
        }
        private string generateFlexipayExcel(DataTable mastels , string Title )
        {
             
            CellCount = mastels.Rows.Count;
            total = new decimal[CellCount];
            GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell tableCell = new TableCell();
            tableCell.Font.Bold = true;
            tableCell.ColumnSpan = 8;
            tableCell.HorizontalAlign = HorizontalAlign.Center;
            tableCell.BorderWidth = 0;
            HeaderRow.Cells.Add(tableCell);
            Company comp = new Company(Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["UserId"]));


            GridViewRow HeaderRow1 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell2 = new TableCell();
            HeaderCell2.Text = comp.CompanyName;
            HeaderCell2.ColumnSpan = 5;
            HeaderCell2.Font.Bold = true;
            HeaderCell2.BorderWidth = 0;
            HeaderRow1.Cells.Add(HeaderCell2);

            GridViewRow HeaderRow2 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell3 = new TableCell();
            HeaderCell3.Text = comp.AddressLine1;
            HeaderCell3.ColumnSpan = 5;
            HeaderCell3.HorizontalAlign = HorizontalAlign.Left;
            HeaderCell3.BorderWidth = 0;
            HeaderRow2.Cells.Add(HeaderCell3);

            GridViewRow HeaderRow3 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell4 = new TableCell();
            HeaderCell4.Text = comp.AddressLine2;
            HeaderCell4.ColumnSpan = 5;
            HeaderCell4.HorizontalAlign = HorizontalAlign.Left;
            HeaderCell4.BorderWidth = 0;
            HeaderRow3.Cells.Add(HeaderCell4);

            GridViewRow HeaderRow4 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell5 = new TableCell();
            HeaderCell5.Text = comp.City;
            HeaderCell5.ColumnSpan = 5;
            HeaderCell5.HorizontalAlign = HorizontalAlign.Left;
            HeaderCell5.BorderWidth = 0;
            HeaderRow4.Cells.Add(HeaderCell5);

            GridViewRow HeaderRow5 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell6 = new TableCell();
            HeaderCell6.Text = "";
            HeaderCell6.Font.Bold = true;
            HeaderCell6.ColumnSpan = 5;
            HeaderCell6.HorizontalAlign = HorizontalAlign.Left;
            HeaderCell6.BorderWidth = 0;
            HeaderRow5.Cells.Add(HeaderCell6);

            GridViewRow HeaderRow6 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell7 = new TableCell();
            HeaderCell7.Text = Title;
            HeaderCell7.Font.Bold = true;
            HeaderCell7.ColumnSpan = 5;
            HeaderCell7.BorderWidth = 0;
            HeaderRow6.HorizontalAlign = HorizontalAlign.Left;
            HeaderRow6.Cells.Add(HeaderCell7);



            GridViewRow HeaderRow7 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell8 = new TableCell();
            HeaderCell8.Text = "";
            HeaderCell8.Font.Bold = true;
            HeaderCell8.ColumnSpan = 5;
            HeaderCell8.BorderWidth = 0;
            HeaderRow7.HorizontalAlign = HorizontalAlign.Left;
            HeaderRow7.Cells.Add(HeaderCell8);
            GridView1.DataBound += new EventHandler(OnDataBound);
            GridView1.RowCreated += new GridViewRowEventHandler(OnRowCreated);
            GridView1.RowDataBound += new GridViewRowEventHandler(gvExcel_RowDataBound);


            GridView1.AllowPaging = false;

            GridView1.DataSource = mastels;
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
                for (int i = 0; i < GridView1.Rows[8].Cells.Count; i++)
                {
                    if (i > 1)
                    {
                        GridView1.Rows[8].Cells[i].Width = 100;
                    }
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + Title + ".xls");
            Response.Charset = "";
            //string style = @"<style> .textmode {'\\'@\' } </style>";
            //  Response.Write(style);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //Apply text style to each Row
                //  GridView1.Rows[i].Attributes.Add("class", "textmode");
                // GridView1.Rows[i].Attributes.Add("style", "word-wrap: break-word");
            }
            GridView1.RenderControl(hw);
            string PDFFilePath = DocumentProcessingSettings.TempDirectoryPath + "\\" + Title + ".xls";
            string renderedGridView = sw.ToString();
            System.IO.File.WriteAllText(PDFFilePath, renderedGridView);
            return PDFFilePath;
            
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
            HeaderCell1.BorderWidth = 0;
            HeaderRow.Cells.Add(HeaderCell1);
            Company comp = new Company(Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["UserId"]));


            GridViewRow HeaderRow1 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell2 = new TableCell();
            HeaderCell2.Text = comp.CompanyName;
            HeaderCell2.ColumnSpan = 5;
            HeaderCell2.Font.Bold = true;
            HeaderCell2.BorderWidth = 0;
            HeaderRow1.Cells.Add(HeaderCell2);

            GridViewRow HeaderRow2 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell3 = new TableCell();
            HeaderCell3.Text = comp.AddressLine1;
            HeaderCell3.ColumnSpan = 5;
            HeaderCell3.HorizontalAlign = HorizontalAlign.Left;
            HeaderCell3.BorderWidth = 0;
            HeaderRow2.Cells.Add(HeaderCell3);

            GridViewRow HeaderRow3 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell4 = new TableCell();
            HeaderCell4.Text = comp.AddressLine2;
            HeaderCell4.ColumnSpan = 5;
            HeaderCell4.HorizontalAlign = HorizontalAlign.Left;
            HeaderCell4.BorderWidth = 0;
            HeaderRow3.Cells.Add(HeaderCell4);

            GridViewRow HeaderRow4 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell5 = new TableCell();
            HeaderCell5.Text = comp.City;
            HeaderCell5.ColumnSpan = 5;
            HeaderCell5.HorizontalAlign = HorizontalAlign.Left;
            HeaderCell5.BorderWidth = 0;
            HeaderRow4.Cells.Add(HeaderCell5);

            GridViewRow HeaderRow5 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell6 = new TableCell();
            HeaderCell6.Text = "";
            HeaderCell6.Font.Bold = true;
            HeaderCell6.ColumnSpan = 5;
            HeaderCell6.HorizontalAlign = HorizontalAlign.Left;
            HeaderCell6.BorderWidth = 0;
            HeaderRow5.Cells.Add(HeaderCell6);

            GridViewRow HeaderRow6 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell7 = new TableCell();
            HeaderCell7.Text = title;
            HeaderCell7.Font.Bold = true;
            HeaderCell7.ColumnSpan = 5;
            HeaderCell7.BorderWidth = 0;
            HeaderRow6.HorizontalAlign = HorizontalAlign.Left;
            HeaderRow6.Cells.Add(HeaderCell7);



            GridViewRow HeaderRow7 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell8 = new TableCell();
            HeaderCell8.Text = "";
            HeaderCell8.Font.Bold = true;
            HeaderCell8.ColumnSpan = 5;
            HeaderCell8.BorderWidth = 0;
            HeaderRow7.HorizontalAlign = HorizontalAlign.Left;
            HeaderRow7.Cells.Add(HeaderCell8);





            GridView1.DataBound += new EventHandler(OnDataBound);
            GridView1.RowCreated += new GridViewRowEventHandler(OnRowCreated);
            GridView1.RowDataBound += new GridViewRowEventHandler(gvExcel_RowDataBound);


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
                for (int i = 0; i < GridView1.Rows[8].Cells.Count; i++)
                {
                    if (i > 1)
                    {
                        GridView1.Rows[8].Cells[i].Width = 100;
                    }
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + title + ".xls");
            Response.Charset = "";
            //string style = @"<style> .textmode {'\\'@\' } </style>";
            //  Response.Write(style);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //Apply text style to each Row
                //  GridView1.Rows[i].Attributes.Add("class", "textmode");
                // GridView1.Rows[i].Attributes.Add("style", "word-wrap: break-word");
            }
            GridView1.RenderControl(hw);
            string PDFFilePath = DocumentProcessingSettings.TempDirectoryPath + "\\" + title + ".xls";
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
        protected void gvExcel_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {

                    //e.Row.Cells[i].Style.Add("mso-number-format", "0\\.00");


                    BoundField field = (BoundField)((DataControlFieldCell)e.Row.Cells[i]).ContainingField;
                    if (field.HeaderText == "EmployeeCode")
                    {

                        //   e.Row.Cells[i].Text = convString(e.Row.Cells[i].Text);
                        string value = e.Row.Cells[i].Text;
                        decimal decVal = 0;
                        if (!string.IsNullOrEmpty(value) && decimal.TryParse(value.Substring(0, 1), out decVal))
                        {
                            if (decVal == 0)
                            {
                                //  e.Row.Cells[i].Attributes.Add("class", "textmode");
                                int no0 = e.Row.Cells[i].Text.Length;// value.LastIndexOf('0');
                                string zeroappend = "";
                                for (int j = 0; j < no0; j++)
                                {
                                    zeroappend = zeroappend + "0";
                                }
                                e.Row.Cells[i].Style.Add("mso-number-format", zeroappend);
                            }

                        }


                    }
                    else if (i >= DetailStartIndex)
                    {
                        e.Row.Cells[i].Style.Add("mso-number-format", "0\\.00");
                    }
                    else if (field.HeaderText == "DateOfBirth" || field.HeaderText == "DateOfJoining" || field.HeaderText == "DateOfWedding")
                    {
                        e.Row.Cells[i].Text = string.IsNullOrEmpty(e.Row.Cells[i].Text) ? "" : (Convert.ToDateTime(e.Row.Cells[i].Text)).ToString("dd/MM/yyyy");
                        e.Row.Cells[i].Style.Add("mso-number-format", "\\@");
                    }
                    else
                    {
                        e.Row.Cells[i].Style.Add("mso-number-format", "\\@");
                    }


                }

            }
        }
        protected void OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            foreach (TableCell cell in e.Row.Cells)
            {
                cell.Style.Add("mso-style-parent", "Text");
                cell.Style.Add("mso-number-format", "@");
            }
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
                    newCell[startColumnIndex] = (new TableCell { Text = labelText, ForeColor = ColorTranslator.FromHtml("#0000FF"), HorizontalAlign = HorizontalAlign.Right });

                }
                else if (IsDetail)
                {
                    newCell[startColumnIndex] = (new TableCell { Text = labelText, ForeColor = ColorTranslator.FromHtml("#0000FF"), HorizontalAlign = HorizontalAlign.Right });

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
                for (int i = 0; i < row1.Cells.Count; i++)
                {
                    row1.Cells[i].Style.Add("mso-number-format", "0\\.00");
                }

                if (GridView1.Controls.Count > 0)
                {
                    GridView1.Controls[0].Controls.Add(row1);
                    GridViewRow row2 = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
                    row2.Cells.Add(new TableCell
                    {
                        Text = "Total Employees  " + EmpCount + " <br/> "
                    });
                    GridView1.Controls[0].Controls.Add(row2);
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
                        if (f.TotalInterestOfYear != 0 && f.EffectiveMonth == effectiveDate.Month.ToString() && f.EffectiveYear == effectiveDate.Year.ToString())
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
                if (!ReferenceEquals(rentSec,null))
                {
                    rentSec.TotalRent = Convert.ToDecimal(rentSec.DeclaredValue);
                }
                rptparms.Add(rentSec == null ? "" : rentSec.TotalRent == 0 ? "" : rentSec.TotalRent.ToString());
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
                ErrorLog.Log(ex);
                return false;
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
                ErrorLog.Log(ex);
                return false;
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
                ErrorLog.Log(ex);
                return false;

            }

        }
        public JsonResult GetPayrollHistory(string categories, int month, int year, string empCode, string type, bool singlePDF)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Guid employeeid = new Guid(Convert.ToString(Session["EmployeeId"]));
            string Authority = Request.Url.GetLeftPart(UriPartial.Authority) + "/";
            string baseUr = Authority;
            string sessionEmpCode = Convert.ToString(Session["EmployeeCode"]);
            return DownloadPayslip(categories, month, year, empCode, companyId, userId, employeeid, baseUr, type, sessionEmpCode, singlePDF);
        }

        public JsonResult GetForm16PartB(string categories, string empCode, string FinYrId, string FinYrText)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            string[] tempFinYr = FinYrText.Split(new string[] { "TO" }, StringSplitOptions.None);
            string startYear = tempFinYr[0].Trim().Substring(tempFinYr[0].Trim().Length - 4, 4);// tempFinYr[0].Contains("/") ? tempFinYr[0].Split('/').ToList().Last() : tempFinYr[0].Split('-').ToList().Last();
            string endYear = tempFinYr[1].Trim().Substring(tempFinYr[1].Trim().Length - 4, 4);// tempFinYr[1].Contains("/") ? tempFinYr[1].Split('/').ToList().Last(): tempFinYr[1].Split('-').ToList().Last();
            int userId = Convert.ToInt32(Session["UserId"]);
            Guid employeeid = new Guid(Convert.ToString(Session["EmployeeId"]));
            return DownloadForm16PartB(categories, new Guid(FinYrId), empCode, userId, companyId, employeeid, startYear, endYear);
        }


        public static JsonResult DownloadPayslip(string categories, int month, int year, string empCode, int companyId, int userId, Guid employeeid, string baseurl, string type, string sessionEmpCode, bool singlePDF = false)
        {
            LockSetting selectVal = new LockSetting(month, year, companyId, "Select");
            if (selectVal.PayrollLock || string.IsNullOrEmpty(sessionEmpCode) || sessionEmpCode == "0")
            {
                categories = categories.TrimEnd(',');
                List<string> sepEmployees = new List<string>();
                List<string> paySlipsEmp = new List<string>();
                MailConfig mailconf = new MailConfig();
                if (type.ToLower() == "sendmail")
                {
                    mailconf = new MailConfig(companyId);
                    if (string.IsNullOrEmpty(mailconf.IPAddress))
                        return BuildJsonResult(false, 100, "Set the Mail configuration in Setting Tab", null);
                }

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
                Company compDetails = new Company(companyId);
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
                EntityModel entityModeladdi = new EntityModel("AdditionalInfo", companyId);
                BankList banklist = new BankList(companyId);
                EsiLocationList esiloclist = new EsiLocationList(companyId);
                PaySlipSendMail payslipSendMail = new PaySlipSendMail();
                AttributeModelList attlist = new AttributeModelList(companyId);
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

                                //emp.RemoveAll(r => r.EmployeeCode!= "KDPL00283");
                                //emp.Add(employees.Where(e => e.EmployeeCode == "KDPL00264").FirstOrDefault());

                            }
                            PaySlipSetting ps = new PaySlipSetting(new Guid(drCat["CofigurationId"].ToString()));
                            PayrollHistoryList cumulativePayHistory = new PayrollHistoryList();
                            emp.RemoveAll(e => (e.SeparationDate > DateTime.MinValue && (e.SeparationDate < DateTime.Parse(DateTime.DaysInMonth(year, month) + "/" + month + "/" + year, new CultureInfo("en-GB")) || e.SeparationDate.Month == month && e.SeparationDate.Year == year)));
                            emp.OrderBy(e => (e.EmployeeCode).ToUpper()).ToList().ForEach(e =>
                              {
                                  cumulativePayHistory = new PayrollHistoryList();
                                  Cumulativepay = new PayrollHistoryList();

                                  if (string.IsNullOrEmpty(empCode))
                                  {
                                      payHistory = new PayrollHistory();
                                  }
                                  PaySlipList setting = new PaySlipList(new Guid(drCat["CofigurationId"].ToString()));
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
                                  EmployeePastDataList empPast = new EmployeePastDataList(e.Id, companyId);
                                  if (empPast.Count > 0)
                                  {
                                      DateTime date = payHistory.ModifiedOn == DateTime.MinValue ? payHistory.CreatedOn : payHistory.ModifiedOn;
                                      EmployeePastData empd = empPast.Where(w => w.FromDate < date && w.ToDate > date).FirstOrDefault();
                                      if (!Object.ReferenceEquals(empd, null))
                                      {
                                          e.EmployeeCode = empd.EmployeeCode;
                                          if (!string.IsNullOrEmpty(empCode))
                                          {
                                              empCode = empd.EmployeeCode;
                                          }
                                      }


                                  }
                                  if (!string.IsNullOrEmpty(Convert.ToString(ps.CumulativeMonth)))
                                  {

                                      Cumulativepay.AddRange(cumulativePayHistory.Where(p => p.EmployeeId == e.Id).ToList());

                                  }

                                  PaySlipList result = setting;
                                  Emp_BankList empbank = new Emp_BankList(e.Id);
                                  EmployeeAddressList empaddr = e.EmployeeAddressList.Count == 0 ? new EmployeeAddressList(e.Id) : e.EmployeeAddressList;
                                  Emp_Personal emppersonal = e.EmployeePersonal == null ? new Emp_Personal(e.Id) : e.EmployeePersonal;

                                  EntityAdditionalInfoList empAddInfoList = new EntityAdditionalInfoList(companyId, entityModeladdi.Id, e.Id);
                                  //  payHistory.PayrollHistoryValueList = payHistory.PayrollHistoryValueList;
                                  if (!object.ReferenceEquals(payHistory, null) && payHistory.Status == ComValue.payrollProcessStatus[0])
                                  {
                                      result.Where(w => w.Section != "FandFHeader").ToList().ForEach(r =>
                                      {
                                          char[] splitchar = { ' ' };
                                          string[] split = r.FieldName.Split(splitchar);
                                          //Assign Master Values from Physical Table
                                          e = PayrollTransaction.GetEmployeeTrasaction("Employee", DateTime.Now.ToShortDateString(), (dynamic)e, payHistory.Id);
                                          switch (r.TableName.ToLower())
                                          {
                                              case "employee":
                                                  var s = e;                                                

                                                  if (split.Count() > 1)
                                                  {
                                                      r.FieldName = split[1];
                                                  }

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
                                                                  r.Value1 = categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault()==null?"": categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().Name;
                                                                  break;
                                                              case "Category":
                                                                  r.Value1 = categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault()==null?"": categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().Name;
                                                                  break;
                                                              case "Department":
                                                                  r.Value1 = deptList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault()==null?"": deptList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().DepartmentName;
                                                                  break;
                                                              case "Branch":
                                                                  r.Value1 = branchList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault()==null?"": branchList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().BranchName;
                                                                  break;
                                                              case "Designation":
                                                                  r.Value1 = desgntionList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault()==null?"": desgntionList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().DesignationName;
                                                                  break;
                                                              case "CostCentre":
                                                                  r.Value1 = costCentreList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault()==null?"": costCentreList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().CostCentreName;
                                                                  break;
                                                              case "Grade":
                                                                  r.Value1 = gradeList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault()==null?"":gradeList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().GradeName;
                                                                  break;
                                                              case "PTLocation":
                                                                  r.Value1 = ptLocList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault()==null?"": ptLocList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().PTLocationName;
                                                                  break;
                                                              case "ESIDespensary":
                                                                  r.Value1 = esiDespen.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault()==null?"": esiDespen.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().ESIDespensary;
                                                                  break;
                                                              case "Location":
                                                                  r.Value1 = locationList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault() == null ? "":locationList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().LocationName;
                                                                  break;
                                                              case "ESILocation":
                                                                  r.Value1 = esiloclist.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault()==null?"":esiloclist.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().LocationName;
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
                                                      r.Value1 = r.Value1==null?"": Convert.ToDateTime(r.Value1).ToString("dd/MM/yyyy");
                                                  }


                                                  break;
                                              case "emp_bank":


                                                  if (split.Count() > 1)
                                                  {
                                                      r.FieldName = split[1];
                                                  }

                                                  r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.FieldName : r.DisplayAs;
                                                  if (empbank.Count > 0)
                                                  {
                                                      if (r.FieldName != "BankName")
                                                      {

                                                          var temp = e.EmployeeBankList[0].GetType().GetProperty(r.FieldName).GetValue(e.EmployeeBankList[0], null);
                                                          r.Value1 = Convert.ToString(temp) != null ? Convert.ToString(temp) : "";


                                                          if (e.EmployeeBankList.Count() == 0)
                                                          {
                                                              var bankDetails = empbank[0].GetType().GetProperty(r.FieldName).GetValue(empbank[0], null);
                                                              r.Value1 = Convert.ToString(bankDetails) != null ? Convert.ToString(bankDetails) : "";
                                                          }

                                                      }


                                                      switch (r.FieldName)
                                                      {
                                                          case "BankName":
                                                              if (e.EmployeeBankList.Count() == 0)
                                                              {
                                                                  r.Value1 = empbank[0].BankId == Guid.Empty ? "" : banklist.Where(d => d.Id == empbank[0].BankId).FirstOrDefault().BankName;
                                                              }
                                                              else
                                                              {
                                                                  r.Value1 = e.EmployeeBankList[0].BankId == Guid.Empty ? "" : banklist.Where(d => d.Id == e.EmployeeBankList[0].BankId).FirstOrDefault().BankName;
                                                              }

                                                              break;
                                                      }

                                                  }
                                                  else
                                                  {
                                                      r.Value1 = string.Empty;
                                                  }

                                                  break;
                                              case "emp_personal":
                                                  if (split.Count() > 1)
                                                  {
                                                      r.FieldName = split[1];
                                                  }
                                                  r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.FieldName : r.DisplayAs;

                                                  {
                                                      var OtherDetail = emppersonal.GetType().GetProperty(r.FieldName).GetValue(emppersonal, null);
                                                      r.Value1 = Convert.ToString(OtherDetail) != null ? Convert.ToString(OtherDetail) : "";
                                                      // r.Value1 = emppersonal.GetType().GetProperty(r.FieldName).GetValue(emppersonal, null).ToString();
                                                      if (r.FieldName == "PFNumber")
                                                      {
                                                          r.Value1 =  string.IsNullOrEmpty(r.Value1)?"": compDetails.PFEmployeerCode + (r.Value1);
                                                      }
                                                      else if (r.FieldName == "BloodGroup")
                                                      {
                                                          r.Value1 = new BloodGroup(Convert.ToInt32(r.Value1)).BloodGroupName;
                                                      }
                                                  }
                                                  if (emppersonal.GetType().GetProperty(r.FieldName).PropertyType.Name.ToUpper() == "DATETIME")
                                                  {
                                                      r.Value1 =string.IsNullOrEmpty(r.Value1)?"":Convert.ToDateTime(r.Value1).ToString("dd/MMM/yyyy");
                                                  }
                                                  break;
                                              case "emp_address":


                                                  if (split.Count() > 1)
                                                  {
                                                      r.FieldName = split[1];
                                                  }

                                                  if (empaddr.Count > 0)
                                                  {
                                                      r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.FieldName : r.DisplayAs;
                                                      var AddressDetail = empaddr[0].GetType().GetProperty(r.FieldName).GetValue(empaddr[0], null);
                                                      r.Value1 = Convert.ToString(AddressDetail) != null ? Convert.ToString(AddressDetail) : "";
                                                      //r.Value1 = empaddr[0].GetType().GetProperty(r.FieldName).GetValue(empaddr[0], null).ToString();
                                                  }
                                                  break;
                                              case "additionalinfo":


                                                  if (split.Count() > 1)
                                                  {
                                                      r.FieldName = split[1];
                                                  }

                                                  var additional = entityModeladdi.EntityAttributeModelList.ToList().Select(at=>at.AttributeModel).ToList();

                                                  if (additional.Count > 0)
                                                  {
                                                      var currentaddinfo = additional.Where(w => w.Name.Trim() == r.DisplayAs.Trim()).FirstOrDefault();
                                                      if (!Object.ReferenceEquals(currentaddinfo, null)){
                                                          r.FieldName = currentaddinfo.Id.ToString();
                                                      }
                                                  }


                                                  r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.FieldName : r.DisplayAs;
                                                  if (empAddInfoList.Count > 0)
                                                  {
                                                      for (int cnt = 0; cnt < empAddInfoList.Count; cnt++)
                                                      {

                                                          if (r.FieldName == empAddInfoList[cnt].AttributeModelId.ToString())
                                                          {

                                                              // AttributeModel at = new AttributeModel(empAddInfoList[cnt].AttributeModelId, companyId);
                                                              AttributeModel at = attlist.Where(at1 => at1.Id == empAddInfoList[cnt].AttributeModelId).FirstOrDefault();
                                                              r.Value1 = empAddInfoList[cnt].Value;

                                                              r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? at.DisplayAs : r.DisplayAs;

                                                          }
                                                      }
                                                  }
                                                  break;
                                              case "category":

                                                  if (split.Count() > 1)
                                                  {
                                                      r.FieldName = split[1];
                                                  }
                                                  r.DisplayAs = string.IsNullOrEmpty(r.DisplayAs) ? r.FieldName : r.DisplayAs;
                                                  var empDetails1 = e.CategoryId;
                                                  r.Value1 = Convert.ToString(empDetails1) != null ? Convert.ToString(empDetails1) : "";
                                                  switch (r.FieldName)
                                                  {
                                                      case "CategoryId":
                                                          r.Value1 = categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault()==null?"": categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().Name;
                                                          break;
                                                      case "Category":
                                                          r.Value1 = categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault()==null?"": categoryList.Where(d => d.Id == new Guid(r.Value1)).FirstOrDefault().Name;
                                                          break;
                                                  }
                                                  break;
                                          }
                                          //Assign payroll History


                                          for (int cnt = 0; cnt < payHistory.PayrollHistoryValueList.Count; cnt++)
                                          {
                                              if (r.FieldName == payHistory.PayrollHistoryValueList[cnt].AttributeModelId.ToString())
                                              {
                                                  //AttributeModel a = new AttributeModel(payHistory.PayrollHistoryValueList[cnt].AttributeModelId, companyId);
                                                  string at2 = attlist.Where(at => at.Id == payHistory.PayrollHistoryValueList[cnt].AttributeModelId).FirstOrDefault().DisplayAs;
                                                  r.Value1 = payHistory.PayrollHistoryValueList[cnt].Value;
                                                  //r.DisplayAs = r.DisplayAs == string.Empty ? a.DisplayAs : r.DisplayAs;
                                                  r.DisplayAs = r.DisplayAs == string.Empty ? at2 : r.DisplayAs;


                                                  if (r.MatchingId != Guid.Empty && r.MatchingId != null && payHistory.PayrollHistoryValueList.Where(x => x.AttributeModelId == r.MatchingId).FirstOrDefault() != null)
                                                  {
                                                      r.Value2 = payHistory.PayrollHistoryValueList.Where(x => x.AttributeModelId == r.MatchingId).FirstOrDefault().Value;
                                                  }
                                                  else
                                                  {
                                                      r.Value2 = "0";
                                                  }
                                              }
                                          }
                                          if (ps.Matchingtype.ToLower().Trim() == "cumulative")
                                          {
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
                                                              // AttributeModel a = new AttributeModel(cu.PayrollHistoryValueList[cnt].AttributeModelId, companyId);
                                                              r.Value2 = (Convert.ToDecimal(r.Value2) + Convert.ToDecimal(cu.PayrollHistoryValueList[cnt].Value)).ToString();
                                                              //  r.DisplayAs = r.DisplayAs == string.Empty ? a.DisplayAs : r.DisplayAs;
                                                          }
                                                      }
                                                  });
                                              }
                                          }
                                      });//attr End
                                         //modified by AjithPanner on 20/11/2017
                                      GetPaySlip(payHistory, result, Cumulativepay, outfilePath + "/" + e.EmployeeCode + ".pdf", month, year, ps, companyId, userId, baseurl, singlePDF);
                                      paySlipsEmp.Add(outfilePath + "/" + e.EmployeeCode + ".pdf");

                                      if (type.ToLower() == "sendmail" && !string.IsNullOrEmpty(e.Email) && e.SeparationDate == DateTime.MinValue)
                                      {
                                          payslipSendMail.PaySlipSend(compDetails, e, month, year, mailconf, outfilePath + "/" + e.EmployeeCode + ".pdf");
                                      }
                                      else
                                      {
                                          sepEmployees.Add(e.EmployeeCode);
                                      }
                                  }

                              });//Employee end

                        }//Categories end


                    }
                    else
                    {

                        return BuildJsonResult(false, 100, "Set the PaySlip Setting in Setting Tab", null);
                    }
                    if (type.ToLower() == "generatepdf")
                    {
                        if (!string.IsNullOrEmpty(empCode))
                        {
                            outfilePath = outfilePath + "/" + empCode + ".pdf";
                        }
                        else
                        {
                            if (singlePDF)
                            {
                                string tempoutfilePath = DocumentProcessingSettings.TempDirectoryPath + "/PaySlip_" + month + "_" + year + ".pdf";
                                MergePDF(paySlipsEmp, tempoutfilePath);
                                var dir = new DirectoryInfo(outfilePath);
                                dir.Delete(true);
                                outfilePath = tempoutfilePath;
                            }
                            else
                            if (categories != "" && string.IsNullOrEmpty(Convert.ToString(ConfigurationManager.AppSettings["PayslipLocation"])))
                            {


                                string PDFFilePath = DocumentProcessingSettings.TempDirectoryPath + "PaySlip.zip";
                                ZipPath(PDFFilePath, outfilePath, null, true, null);
                                outfilePath = PDFFilePath;
                            }
                            else
                            {
                                return BuildJsonResult(true, 1, "File Saved Successfully at " + outfilePath, null);
                            }
                        }
                    }
                    else if (type.ToLower() == "sendmail" && !string.IsNullOrEmpty(empCode) && sepEmployees.Count == 0)
                    {
                        return BuildJsonResult(true, 1, "Mail Send Successfully", new { filePath = "" });
                    }
                    else if (type.ToLower() == "sendmail" && string.IsNullOrEmpty(empCode) && categories != "" && sepEmployees.Count == 0)
                    {
                        return BuildJsonResult(true, 1, "Mail Send Successfully", new { filePath = "" });
                    }
                    else if (type.ToLower() == "sendmail" && sepEmployees.Count > 0)
                    {
                        return BuildJsonResult(true, 1, "Mail not send for separated employees", new { filePath = "" });
                    }
                    return BuildJsonResult(true, 1, "File Saved Successfully", new { filePath = outfilePath });
                }
            }
            else
            {
                return BuildJsonResult(false, 1, "Payroll not yet process", new { filePath = "" });
            }
        }

        public static JsonResult DownloadForm16PartB(string categories, Guid FinYrId, string empCode, int userId, int companyId, Guid employeeid, string startYear, string endYear)
        {
            categories = categories.TrimEnd(',');
            Company cmp = new Company(companyId);
            string outfilePath = DocumentProcessingSettings.TempDirectoryPath;

            if (!Directory.Exists(outfilePath + "\\" + "Form16 Part B_" + startYear + " - " + endYear))
            {
                Directory.CreateDirectory(outfilePath + "\\" + "Form16 Part B_" + startYear + " - " + endYear);
                outfilePath = outfilePath + "\\" + "Form16 Part B_" + startYear + " - " + endYear;
            }
            else
            {
                outfilePath = outfilePath + "\\" + "Form16 Part B_" + startYear + " - " + endYear;
            }
            List<Employee> emp = new List<Employee>();
            var categoriesId = categories.Split(',');
            for (int i = 0; i < categoriesId.Count(); i++)
            {
                string categoryId = categoriesId[i].Replace("'", "");
                if (!string.IsNullOrEmpty(categoryId))
                {
                    EmployeeList employees = new EmployeeList(companyId, new Guid(categoryId));
                    emp.AddRange(employees.ToList());
                }

            }
            if (!string.IsNullOrEmpty(categories))
                employeeid = Guid.Empty;
            else
            {
                EmployeeList employees = new EmployeeList(companyId);
                employeeid = employees.Where(e => e.EmployeeCode.ToLower().Trim() == empCode.ToLower().Trim()).FirstOrDefault().Id;
                List<Employee> emplist = new List<Employee>();
                emp.Add(employees.Where(e => e.EmployeeCode == empCode).FirstOrDefault());
            }
            CategoryList categoryList = new CategoryList(companyId);
            TXFinanceYear finyearDetails = new TXFinanceYear(FinYrId, companyId);
            Employee inchargeEmployeeDetail = new Employee();
            string taxYear = string.Empty;
            string inchargeEmployeeName = string.Empty;
            string inchargeEmployeeDesgination = string.Empty;
            string inchargeEmployeeFatherName = string.Empty;
            if (finyearDetails != null)
            {
                int taxStartYr = finyearDetails.StartingDate.Year;
                int taxEndYr = finyearDetails.EndingDate.Year;
                taxYear = "A.Y -" + taxStartYr + "-" + taxEndYr;
                EmployeeList employees = new EmployeeList(companyId);

                inchargeEmployeeDetail = employees.Where(e => e.Id == finyearDetails.InchargeEmployeeId).FirstOrDefault();
                if (inchargeEmployeeDetail != null)
                {
                    Designation des = new Designation(inchargeEmployeeDetail.Designation, companyId);
                    inchargeEmployeeName = inchargeEmployeeDetail.FirstName + " " + inchargeEmployeeDetail.LastName;
                    inchargeEmployeeDesgination = des.DesignationName;
                    inchargeEmployeeFatherName = string.IsNullOrEmpty(inchargeEmployeeDetail.EmployeePersonal.FatherName) ? "________________" : inchargeEmployeeDetail.EmployeePersonal.FatherName;
                }

            }
            TaxHistory tx = new TaxHistory();
            DataTable form16partB = tx.Form16PartB(FinYrId, employeeid, companyId);
            DataSet dtset = tx.Form16PartBGrossValues(FinYrId, employeeid, companyId);
            DataTable form16partBStaticDatas = dtset.Tables[0];
            DataTable GrossSalarySection = dtset.Tables[1];
            DataTable SectionTotalValue = dtset.Tables[2];

            string[] columnNames = (from dc in form16partB.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();

            string[] y = { "Section 10 Exemptions", "Section 16", "Other Income", "Section 80CCE: 80C, 80CCC,80-CCD(1)", "Under Section 80CCC", "Under Section 80CCD", "Medical insurance premium (Mediclaim)  (80D)", "Other Deductible amounts under Chapter VI A" };

            TXSectionList secList = new TXSectionList(companyId, FinYrId);
            if (!string.IsNullOrEmpty(empCode))
            {
                emp = emp.Where(e => e.EmployeeCode.ToLower().Trim() == empCode.ToLower().Trim()).ToList();
                if (emp != null && emp.Count > 0)
                {
                    categories = "'" + emp[0].CategoryId + "'";
                }
            }
            string employeeCodes = string.Empty;
            emp.ForEach(e =>
            {
                if (form16partB.AsEnumerable().Where(r => r.Field<string>("Employeecode") == e.EmployeeCode).Count() > 0)
                {
                    DataTable dtSectionvalues = form16partB.AsEnumerable().Where(r => r.Field<string>("Employeecode") == e.EmployeeCode).OrderBy(x => x.Field<string>("Result"))
                                 .CopyToDataTable();
                    DataTable dtGrossSection = form16partBStaticDatas.AsEnumerable()
                                 .Where(r => r.Field<string>("Employeecode") == e.EmployeeCode)
                                 .CopyToDataTable();
                    DataTable dtGrossSalarySection = GrossSalarySection.AsEnumerable()
                                 .Where(r => r.Field<string>("Employeecode") == e.EmployeeCode)
                                 .CopyToDataTable();
                    DataTable dtSectionTotalValue = SectionTotalValue.AsEnumerable()
                                .Where(r => r.Field<string>("Employeecode") == e.EmployeeCode)
                                .CopyToDataTable();
                    List<rptWorkSheet> worksheetList = new List<rptWorkSheet>();
                    foreach (var col in columnNames)
                    {

                        TXSection txsec = secList.Where(s => s.Name.ToLower().Trim().Contains(col.ToLower().Trim())).FirstOrDefault();
                        if (txsec != null)
                        {
                            string Acutal = Convert.ToString(dtSectionvalues.Rows[0][col]);
                            string TotalVal = Convert.ToString(dtSectionvalues.Rows[1][col]);
                            if (col.Trim() == "Other Deductible amounts under Chapter VI A")//|| col.Trim() == "Medical insurance premium (Mediclaim)  (80D)")
                                txsec.SectionType = "Others";
                            if (txsec.IncomeTypeId > 3)
                                txsec.SectionType = "Others";
                            else
                                txsec.SectionType = "";

                            if (txsec.ParentId != Guid.Empty || txsec.SectionType == "Others")
                            {
                                if ((!string.IsNullOrEmpty(Acutal) && (Acutal != "0.00" && Acutal != "0")) || (!string.IsNullOrEmpty(TotalVal) && (TotalVal != "0.00" && TotalVal != "0")))
                                {
                                    rptWorkSheet rws = new rptWorkSheet();
                                    rws.Description = txsec.Name.Trim();
                                    rws.Total = TotalVal == "" ? 0 : Convert.ToDecimal(Convert.ToString(TotalVal));
                                    rws.Actual = Acutal == "" ? 0 : Convert.ToDecimal(Convert.ToString(Acutal));
                                    rws.ParentSection = txsec.ParentSection.DisplayAs != null ? txsec.ParentSection.DisplayAs.Trim() : "Other Income";// "Section 10 Exemptions";
                                    rws.Type = "SubSection";
                                    if (rws.Description == "Contribution to pension fund covered u/s 80CCC" && rws.ParentSection == "Section 80CCE: 80C, 80CCC,80-CCD(1)")
                                    {
                                        rws.ParentSection = "Under Section 80CCC";
                                    }
                                    if (rws.Description == "Employee contribution to National Pension Scheme u/s 80CCD(1)" && rws.ParentSection == "Section 80CCE: 80C, 80CCC,80-CCD(1)")
                                    {
                                        rws.ParentSection = "Under Section 80CCD";
                                    }
                                    if (rws.Description == "Other Deductible amounts under Chapter VI A")
                                    {
                                        rws.ParentSection = "Under Section 80CCD";
                                    }

                                    worksheetList.Add(rws);
                                }
                            }

                        }
                    }
                    List<rptWorkSheet> worksheetListorder = new List<rptWorkSheet>();
                    for (int j = 0; j < y.Length; j++)
                    {
                        var data = worksheetList.Where(p => p.ParentSection.ToLower().Trim() == y[j].ToLower().Trim()).ToList();
                        if (data.Count > 0)
                        {
                            data.ForEach(d =>
                            {
                                d.FormulaType = j + 1;
                                d.FormulaType = d.FormulaType > 7 ? 7 : d.FormulaType;
                                d.OrderG = ConvertAlpha(j + 1);
                            });
                            worksheetListorder.AddRange(data);
                        }
                        else
                        {
                            rptWorkSheet temp = new rptWorkSheet();
                            temp.FormulaType = j + 1;
                            temp.FormulaType = temp.FormulaType > 7 ? 7 : temp.FormulaType;
                            temp.OrderG = ConvertAlpha(j + 1);
                            //temp.Actual = 0;temp.Total = 0;
                            worksheetListorder.Add(temp);
                        }
                    }

                    e.EmployeePersonal.PANNumber = string.IsNullOrEmpty(e.EmployeePersonal.PANNumber) ? "xxxxxx" : e.EmployeePersonal.PANNumber;
                    string path = outfilePath.Trim() + "/" + e.EmployeeCode.Trim() + "-" + e.EmployeePersonal.PANNumber.Trim() + "_F16 Part B_AY" + startYear.Trim() + " - " + endYear.Trim() + ".pdf";
                    Form16PartBGrossValues grossval = new Form16PartBGrossValues(dtGrossSection, dtGrossSalarySection, dtSectionTotalValue);

                    GetForm16partBReport(worksheetListorder, path, companyId, userId, e, cmp.CompanyName, grossval, inchargeEmployeeName, inchargeEmployeeDesgination, taxYear, inchargeEmployeeFatherName);
                }
                else
                {
                    employeeCodes += e.EmployeeCode + ",";
                }
            });
            employeeCodes = employeeCodes.TrimEnd(',');
            if (!string.IsNullOrEmpty(empCode))
            {
                if (string.IsNullOrEmpty(employeeCodes))
                {
                    var e = emp.Where(x => x.EmployeeCode == empCode).FirstOrDefault();
                    e.EmployeePersonal.PANNumber = string.IsNullOrEmpty(e.EmployeePersonal.PANNumber) ? "xxxxxx" : e.EmployeePersonal.PANNumber;
                    string path = outfilePath.Trim() + "\\" + e.EmployeeCode.Trim() + "-" + e.EmployeePersonal.PANNumber.Trim() + "_F16 Part B_AY" + startYear.Trim() + " - " + endYear.Trim() + ".pdf";
                    outfilePath = path;
                }
                else
                {
                    outfilePath = "";
                }
            }
            else
            {
                if (categories != "")
                {
                    string PDFFilePath = outfilePath.Trim() + ".zip";
                    ZipPath(PDFFilePath, outfilePath, null, true, null);
                    outfilePath = PDFFilePath;
                }
                else
                {
                    return BuildJsonResult(true, 1, "File Saved Successfully.", new { filepath = outfilePath, empcodes = employeeCodes });
                }
            }
            return BuildJsonResult(true, 1, "File Saved Successfully.", new { filepath = outfilePath, empcodes = employeeCodes });
        }

        public JsonResult ViewSalarySummary(List<jsonPaySheetattr> paysheetattr, Guid category, Guid EmpId, int smonth, int syear, int nMonth, int? nYear)
        {
            if (nYear == null)
            {
                nMonth = smonth; nYear = syear;
            }
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            PayrollHistoryList history = new PayrollHistoryList(companyId, syear, smonth, nYear.Value, nMonth, EmpId);
            AttributeModelList attributelist = new AttributeModelList(companyId);
            List<jsonMonthlyInput> entity = new List<jsonMonthlyInput>();
            string[] header = new string[paysheetattr.Count() + 5];
            var EG = attributelist.Where(x => x.Name == "EG").FirstOrDefault();
            var TotDed = attributelist.Where(x => x.Name == "TOTDED").FirstOrDefault();
            var netPay = attributelist.Where(x => x.Name == "NETPAY").FirstOrDefault();
            paysheetattr.Remove(paysheetattr.Where(x => x.fieldName == EG.Id.ToString()).FirstOrDefault());
            paysheetattr.Remove(paysheetattr.Where(x => x.fieldName == TotDed.Id.ToString()).FirstOrDefault());
            paysheetattr.Remove(paysheetattr.Where(x => x.fieldName == netPay.Id.ToString()).FirstOrDefault());
            int i = 4;
            header[0] = "Month";
            header[1] = "year";
            header[2] = EG.DisplayAs;
            header[3] = TotDed.DisplayAs;
            header[4] = netPay.DisplayAs;

            paysheetattr.GroupBy(g => g.displayAs).ToList().ForEach(f =>
            {
                i++;
                header[i] = f.Key;
            });
            List<string[]> rows = new List<string[]>();
            header = header.Where(x => x != null).ToArray();
            history.ForEach(h =>
            {
                if (h.Status.ToLower() == "processed")
                {
                    string[] li = new string[header.Count()];
                    jsonMonthlyInput temp = new jsonMonthlyInput();
                    li[0] = Convert.ToString((MonthEnum)(Convert.ToInt32(h.Month)));
                    li[1] = Convert.ToString(h.Year);
                    li[2] = h.PayrollHistoryValueList.Where(ph => ph.AttributeModelId == (EG.Id)).FirstOrDefault().Value;
                    li[3] = h.PayrollHistoryValueList.Where(ph => ph.AttributeModelId == (TotDed.Id)).FirstOrDefault().Value;
                    li[4] = h.PayrollHistoryValueList.Where(ph => ph.AttributeModelId == (netPay.Id)).FirstOrDefault().Value;
                    paysheetattr.ForEach(a =>
                    {
                        int index1 = Array.IndexOf(header, a.displayAs);
                        if (h.PayrollHistoryValueList.Where(ph => ph.AttributeModelId == new Guid(a.fieldName)).FirstOrDefault() != null)
                        {
                            li[index1] = (h.PayrollHistoryValueList.Where(ph => ph.AttributeModelId == new Guid(a.fieldName)).FirstOrDefault() == null ? "0" : (h.PayrollHistoryValueList.Where(ph => ph.AttributeModelId == new Guid(a.fieldName)).FirstOrDefault().Value == null ? "0" : (h.PayrollHistoryValueList.Where(ph => ph.AttributeModelId == new Guid(a.fieldName)).FirstOrDefault().Value)));
                            li[index1] = li[index1] == "" ? "0" : li[index1];
                        }
                        else
                        {
                            li[index1] = "0";
                        }

                    });
                    li = li.Where(x => x != null && x != "").ToArray();
                    rows.Add(li);
                }
            });

            // decimal finalTotal = 0;
            //Footer
            string[] footer = new string[header.Count()];
            footer[0] = "";
            footer[1] = "Total";
            if (rows.Count > 0)
            {
                for (int j = 2; j < rows[0].Count(); j++)
                {
                    footer[j] = Convert.ToString(rows.Sum(k => Convert.ToDecimal((k[j]))));
                }
            }
            List<jsonPaySheetattr> newheader = new List<jsonPaySheetattr>();
            newheader.Add(tempsalarysummary("Month"));
            newheader.Add(tempsalarysummary("Year"));
            newheader.Add(tempsalarysummary(EG.DisplayAs));
            newheader.Add(tempsalarysummary(TotDed.DisplayAs));
            newheader.Add(tempsalarysummary(netPay.DisplayAs));
            newheader.AddRange(paysheetattr);

            return base.BuildJson(true, 200, "success", new { rowheader = newheader, rows = rows, rowfooter = footer });
        }

        public jsonPaySheetattr tempsalarysummary(string compname)
        {
            jsonPaySheetattr tempobj = new jsonPaySheetattr();
            tempobj.displayAs = compname;
            tempobj.type = "Static";
            return tempobj;
        }
        #region Form 24Q report download

        public JsonResult GetForm24quaterly(Guid FinYrId, string FinYrText, string Quaterly)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            string[] tempFinYr = FinYrText.Split(new string[] { "TO" }, StringSplitOptions.None);
            int startYear = Convert.ToInt32(tempFinYr[0].Trim().Substring(tempFinYr[0].Trim().Length - 4, 4));// tempFinYr[0].Contains("/") ? tempFinYr[0].Split('/').ToList().Last() : tempFinYr[0].Split('-').ToList().Last();
            int endYear = Convert.ToInt32(tempFinYr[1].Trim().Substring(tempFinYr[1].Trim().Length - 4, 4));
            int userId = Convert.ToInt32(Session["UserId"]);
            int startMonth = Form24QuaterStartMonth(Quaterly);
            DateTime startDate = new DateTime(startMonth == 1 ? endYear : startYear, startMonth, 1);
            DateTime endDate = startDate.AddMonths(2);
            string QuaterEnd = Enum.GetName(typeof(MonthEnum), startDate.AddMonths(2).Month) + " " + startDate.AddMonths(2).Year + "(Year)";
            string FinYr = startYear + "-" + endYear;
            string AssmentYr = endYear + "-" + Convert.ToInt32(endYear + 1);
            TXFinanceYear finYrdetails = new TXFinanceYear(FinYrId, companyId);
            Company compDetails = new Company(companyId);
            Employee InchargeEmpDetails = new Employee(companyId, finYrdetails.InchargeEmployeeId);
            string inchargeEmployeeDesgination = string.Empty;
            if (InchargeEmpDetails != null)
            {
                Designation des = new Designation(InchargeEmpDetails.Designation, companyId);
                inchargeEmployeeDesgination = des.DesignationName;

            }
            PayrollBO.PFChallan pf = new PayrollBO.PFChallan();

            TaxHistory tx = new TaxHistory();
            //DataTable form24Quaterly = tx.Form24Quaterly(FinYrId, startDate, companyId);
            DataSet dsForm24Q = new DataSet();
            DataSet dsForm24QTotal = new DataSet();
            int i = 1;
            do
            {
                DataSet form24QMonthly = new DataSet();
                form24QMonthly = tx.Form24Quaterly(FinYrId, startDate, companyId);
                DataTable dt = form24QMonthly.Tables[0];
                DataTable dt1 = form24QMonthly.Tables[1];
                form24QMonthly.Tables.Remove(dt);
                form24QMonthly.Tables.Remove(dt1);
                dt.TableName = startDate.ToString("MMM") + "_" + startDate.ToString("yyyy");// "M" + i; 
                dt1.TableName = "M" + i;
                dsForm24Q.Tables.Add(dt);
                dsForm24QTotal.Tables.Add(dt1);
                startDate = startDate.AddMonths(1);
                i = i + 1;
            }
            while (startDate <= endDate);
            DataTable dtM1 = dsForm24Q.Tables[0];
            DataTable dtM2 = dsForm24Q.Tables[1];
            DataTable dtM3 = dsForm24Q.Tables[2];
            DataTable challantotal = new DataTable();
            challantotal.Merge(dsForm24QTotal.Tables[0]);
            challantotal.Merge(dsForm24QTotal.Tables[1]);
            challantotal.Merge(dsForm24QTotal.Tables[2]);
            Guid employeeid = new Guid(Convert.ToString(Session["EmployeeId"]));
            string FilePath = DocumentProcessingSettings.TempDirectoryPath + "\\" + compDetails.CompanyName + "_" + Quaterly + "_" + startYear + "-" + endYear + ".xls";
            generateForm24Q(FilePath, dtM1, dtM2, dtM3, challantotal, finYrdetails, compDetails, InchargeEmpDetails, QuaterEnd, FinYr, AssmentYr, inchargeEmployeeDesgination);
            return base.BuildJson(true, 200, "success", FilePath);
        }

        public int Form24QuaterStartMonth(string Quaterly)
        {
            int StartMonth = 0;
            switch (Quaterly.ToUpper())
            {
                case "Q1":
                    StartMonth = 4;
                    break;
                case "Q2":
                    StartMonth = 7;
                    break;
                case "Q3":
                    StartMonth = 10;
                    break;
                case "Q4":
                    StartMonth = 1;
                    break;
                default:
                    break;
            }
            return StartMonth;
        }


        public bool generateForm24Q(string FilePath, DataTable dt1, DataTable dt2, DataTable dt3, DataTable challantotal,
            TXFinanceYear finYrDetails, Company compdetails, Employee InchargeEmpDetails, string QuaterEnd, String FinYr, String AssYr, string InchargeDesgination)
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
                rpt.ReportPath = "Reports/Form24.rdlc";

                ReportDataSource rptDs1 = new ReportDataSource("Form24Q1", dt1);
                ReportDataSource rptDs2 = new ReportDataSource("Form24Q2", dt2);
                ReportDataSource rptDs3 = new ReportDataSource("Form24Q3", dt3);
                ReportDataSource rptDs4 = new ReportDataSource("Form24QChallanTotal", challantotal);

                rpt.DataSources.Add(rptDs1);
                rpt.DataSources.Add(rptDs2);
                rpt.DataSources.Add(rptDs3);
                rpt.DataSources.Add(rptDs4);

                ReportParameterCollection rpcollection = new ReportParameterCollection();
                rpcollection.Add(new ReportParameter("CompanyName", compdetails.CompanyName));
                rpcollection.Add(new ReportParameter("TaxAccountNo", finYrDetails.TanNo));
                rpcollection.Add(new ReportParameter("PAN", finYrDetails.PANorGIRNO));
                rpcollection.Add(new ReportParameter("FinYear", FinYr));
                rpcollection.Add(new ReportParameter("AssessmentYear", AssYr));
                rpcollection.Add(new ReportParameter("CompAddress1", compdetails.AddressLine1));
                rpcollection.Add(new ReportParameter("CompAddress2", compdetails.AddressLine2));
                rpcollection.Add(new ReportParameter("CompCity", compdetails.City));
                rpcollection.Add(new ReportParameter("CompState", compdetails.State));
                rpcollection.Add(new ReportParameter("CompPincode", compdetails.PinCode));
                rpcollection.Add(new ReportParameter("CompEmail", compdetails.EMail));
                rpcollection.Add(new ReportParameter("InchargeName", InchargeEmpDetails.FirstName + " " + InchargeEmpDetails.LastName));
                rpcollection.Add(new ReportParameter("InchargeDesgination", InchargeDesgination));
                rpcollection.Add(new ReportParameter("QuaterEnded", QuaterEnd));
                rpcollection.Add(new ReportParameter("M1", dt1.TableName));
                rpcollection.Add(new ReportParameter("M2", dt2.TableName));
                rpcollection.Add(new ReportParameter("M3", dt3.TableName));
                rpt.SetParameters(rpcollection);
                byte[] renderedBytes = null;

                renderedBytes = rpt.Render("Excel", null, out mimeType, out encoding, out extension, out streamIds, out warnings);



                string contentype = mimeType;


                using (FileStream fs = System.IO.File.Create(FilePath))
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


        #endregion

        #region Form 24 Annenture report download
        public JsonResult GetForm24QAnnenture(Guid FinYrId, string FinYrText)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            string[] tempFinYr = FinYrText.Split(new string[] { "TO" }, StringSplitOptions.None);
            int startYear = Convert.ToInt32(tempFinYr[0].Trim().Substring(tempFinYr[0].Trim().Length - 4, 4));// tempFinYr[0].Contains("/") ? tempFinYr[0].Split('/').ToList().Last() : tempFinYr[0].Split('-').ToList().Last();
            int endYear = Convert.ToInt32(tempFinYr[1].Trim().Substring(tempFinYr[1].Trim().Length - 4, 4));
            int userId = Convert.ToInt32(Session["UserId"]);

            string FinYr = startYear + "-" + endYear;
            string AssmentYr = endYear + "-" + Convert.ToInt32(endYear + 1);
            TXFinanceYear finYrdetails = new TXFinanceYear(FinYrId, companyId);
            Company compDetails = new Company(companyId);
            Employee InchargeEmpDetails = new Employee(companyId, finYrdetails.InchargeEmployeeId);
            string inchargeEmployeeDesgination = string.Empty;
            if (InchargeEmpDetails != null)
            {
                Designation des = new Designation(InchargeEmpDetails.Designation, companyId);
                inchargeEmployeeDesgination = des.DesignationName;

            }

            TaxHistory tx = new TaxHistory();
            DataTable form24A = new DataTable();
            form24A = tx.Form24A(FinYrId, Guid.Empty, companyId);



            Guid employeeid = new Guid(Convert.ToString(Session["EmployeeId"]));
            string FilePath = DocumentProcessingSettings.TempDirectoryPath + "\\" + compDetails.CompanyName + "_" + "Form24QA" + "_" + startYear + "-" + endYear + ".xls";
            generateForm24A(FilePath, form24A);
            return base.BuildJson(true, 200, "success", FilePath);
        }

        public bool generateForm24A(string FilePath, DataTable dt)
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
                rpt.ReportPath = "Reports/Form24A.rdlc";
                ReportDataSource rptDs1 = new ReportDataSource("DSForm24A", dt);
                rpt.DataSources.Add(rptDs1);

                byte[] renderedBytes = null;
                renderedBytes = rpt.Render("Excel", null, out mimeType, out encoding, out extension, out streamIds, out warnings);



                string contentype = mimeType;


                using (FileStream fs = System.IO.File.Create(FilePath))
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
        #endregion

        public static string ConvertAlpha(int value)
        {
            const int a = (int)'A';
            value = value - 1;
            var returnValue = new StringBuilder();
            while (value > -1)
            {
                var remainder = value % 26;
                returnValue.Insert(0, (char)(a + remainder));
                value = value / 26 - 1;
            }
            return returnValue.ToString();
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
        //created by maddy 
        public JsonResult FlxiPayReoprt(Guid SelectInput)
        {
            if (!base.checkSession())
                return base.BuildJson(false, 100, "Invalied User", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            EntityMasterValue entity = new EntityMasterValue();
            DataTable dataTable = entity.GetFlexiPayRPT(SelectInput);
            if (dataTable.Rows.Count == 1)
            {
                string json = jsonSerializedDtToString(dataTable);
                return base.BuildJson(true, 100, "load Data", json);
            }
            if (dataTable.Rows.Count > 1)
            {
                string path = this.generateFlexipayExcel(dataTable, "FlexiPay Report");
                return base.BuildJson(true, 100, "Load data", new { filePath = path });
            }
            return base.BuildJson(false, 100, "Data Not Found", null);
        }

        public static Dictionary<string, string> GetFilterExpr(int companyId, List<jsonPaySheetattr> filters)
        {
            Dictionary<string, string> filterExpr = new Dictionary<string, string>();
            string expr = string.Empty;
            filters.Where(f => f.tableName.ToLower() == "employee").ToList().ForEach(f =>
            {
                string[] temp = f.fieldName.Split(' ');
                switch (temp[1].Trim().ToLower())
                {
                    case "category":
                        if (new CategoryList(companyId).Where(c => c.Name == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new CategoryList(companyId).Where(c => c.Name == f.value).FirstOrDefault().Id);
                        }
                        else
                            f.value = string.Empty;
                        break;
                    case "department":
                        if (new DepartmentList(companyId).Where(c => c.DepartmentName == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new DepartmentList(companyId).Where(c => c.DepartmentName == f.value).FirstOrDefault().Id);
                        }
                        else
                            f.value = string.Empty;
                        break;
                    case "branch":
                        if (new BranchList(companyId).Where(c => c.BranchName == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new BranchList(companyId).Where(c => c.BranchName == f.value).FirstOrDefault().Id);
                        }
                        else { f.value = string.Empty; }
                        break;
                    case "designation":
                        if (new DesignationList(companyId).Where(c => c.DesignationName == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new DesignationList(companyId).Where(c => c.DesignationName == f.value).FirstOrDefault().Id);
                        }
                        else { f.value = string.Empty; }
                        break;
                    case "costcentre":
                        if (new CostCentreList(companyId).Where(c => c.CostCentreName == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new CostCentreList(companyId).Where(c => c.CostCentreName == f.value).FirstOrDefault().Id);
                        }
                        else { f.value = string.Empty; }
                        break;
                    case "grade":
                        if (new GradeList(companyId).Where(c => c.GradeName == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new GradeList(companyId).Where(c => c.GradeName == f.value).FirstOrDefault().Id);
                        }
                        else { f.value = string.Empty; }
                        break;
                    case "esilocation":
                        if (new EsiLocationList(companyId).Where(c => c.LocationName == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new EsiLocationList(companyId).Where(c => c.LocationName == f.value).FirstOrDefault().Id);
                        }
                        else { f.value = string.Empty; }
                        break;
                    case "ptlocation":
                        if (new PTLocationList(companyId).Where(c => c.PTLocationName == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new PTLocationList(companyId).Where(c => c.PTLocationName == f.value).FirstOrDefault().Id);
                        }
                        else { f.value = string.Empty; }
                        break;
                    case "location":
                        if (new LocationList(companyId).Where(c => c.LocationName == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new LocationList(companyId).Where(c => c.LocationName == f.value).FirstOrDefault().Id);
                        }
                        else { f.value = string.Empty; }
                        break;
                    case "esidespensary":
                        if (new ESIDespensaryList(companyId).Where(c => c.ESIDespensary == f.value).FirstOrDefault() != null)
                        {
                            f.value = Convert.ToString(new ESIDespensaryList(companyId).Where(c => c.ESIDespensary == f.value).FirstOrDefault().Id);
                        }
                        else { f.value = string.Empty; }
                        break;
                    case "bank":
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
            public string Header { get; set; }
            public string Footer { get; set; }

            public string MatchingSettingsFor { get; set; }
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
                    cumulativeMonth = setting.CumulativeMonth,
                    Header = setting.Header,
                    Footer = setting.Footer,
                    MatchingSettingsFor = setting.Matchingtype
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
                    CumulativeMonth = setting.cumulativeMonth,
                    Header = setting.Header,
                    Footer = setting.Footer


                };
            }


        }

        public class tablePayslip
        {
            public string fieldName { get; set; }
            public string displayAs { get; set; }
            public string valueOne { get; set; }
            public string valueTwo { get; set; }
            public string section { get; set; }
            public int diaplayOrder { get; set; }

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

            public int? ffhOrder { get; set; }
            public int? eOrder { get; set; }
            public int? dOrder { get; set; }
            public string type { get; set; }
            public string displayAs { get; set; }
            public bool isPhysicalTbl { get; set; }

            public string datatype { get; set; }
            public bool IsincludeGrosspay { get; set; }
            public Guid MatchingId { get; set; }

            public static jsonPaySlipattributes tojsonmater(PaySlipAttributes attr)
            {
                return new jsonPaySlipattributes()
                {
                    Id = attr.Id,
                    CofigurationId = attr.CofigurationId,
                    categoryId = attr.CategoryId,
                    tableName = attr.TableName,
                    fieldName = attr.TableName + " " + attr.FieldName,
                    displayAs = attr.FieldName,
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
                    ffhOrder = attr.FandFHeaderDisplayOrder == 0 ? (int?)null : attr.FandFHeaderDisplayOrder,
                    eOrder = attr.EarningDisplayOrder == 0 ? (int?)null : attr.EarningDisplayOrder,
                    dOrder = attr.DeductionDisplayOrder == 0 ? (int?)null : attr.DeductionDisplayOrder,
                    type = attr.Type,
                    isPhysicalTbl = attr.IsPhysicalTable,
                    attributeId = attr.AttributeId,
                    IsincludeGrosspay = attr.IsIncludeGrossPay,
                    MatchingId = attr.MatchingId
                };
            }
            public static PaySlipAttributes convertobject(jsonPaySlipattributes attr, List<previousComponents> match)
            {
                Guid tempMatchingId = Guid.Empty;
                if (attr.type == "Earnings")
                {
                    var matching = match.Count > 0 ? match.Where(m => m.Id == new Guid(attr.fieldName) && attr.type == "Earnings").FirstOrDefault() : null;
                    tempMatchingId = matching == null ? Guid.Empty : matching.MappedId;
                }
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
                    FandFHeaderDisplayOrder = attr.ffhOrder == null ? 0 : (int)attr.ffhOrder,
                    EarningDisplayOrder = attr.eOrder == null ? 0 : (int)attr.eOrder,
                    DeductionDisplayOrder = attr.dOrder == null ? 0 : (int)attr.dOrder,
                    Type = attr.type,
                    IsPhysicalTable = attr.isPhysicalTbl,
                    AttributeId = attr.attributeId,
                    MatchingId = tempMatchingId

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
                    Month = attr.month,
                    Year = attr.year,
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

        public class Form16PartBGrossValues
        {
            public string Ecess { get; set; }
            public string Grosssalary { get; set; }
            public string OtherIncomeTotal { get; set; }
            public string Mediclaim80D { get; set; }
            public string UnderChapterVIA { get; set; }
            public string section10 { get; set; }
            public string section16 { get; set; }
            public string Section80C { get; set; }
            public string Section80CC { get; set; }
            public string Section80CCD { get; set; }
            public string TotalIncome { get; set; }
            public string TaxOnTotalIncome { get; set; }
            public string TaxPayable { get; set; }
            public string section87 { get; set; }
            public string section89 { get; set; }
            public string GrossSection1 { get; set; }
            public string GrossSection2 { get; set; }
            public string GrossSection3 { get; set; }
            public string Surcharge { get; set; }
            public string FinalTaxPayable { get; set; }


            public Form16PartBGrossValues(DataTable dt, DataTable GrossSalarySection, DataTable SectionDeductible)
            {

                if (dt.Rows.Count > 0)
                {
                    this.Ecess = Convert.ToString(dt.Rows[0]["HECESS"]);
                    this.Grosssalary = Convert.ToString(dt.Rows[0]["Gross Salary"]);
                    this.Mediclaim80D = Convert.ToString(dt.Rows[0]["Medical insurance premium (Mediclaim)  (80D)"]);
                    //   this.UnderChapterVIA = Convert.ToString(dt.Rows[0]["Other Deductible amounts under Chapter VI A"]);
                    this.OtherIncomeTotal = Convert.ToString(dt.Rows[0]["otherincometotal"]);
                    this.section10 = Convert.ToString(dt.Rows[0]["Section 10 Exemptions"]);
                    this.section16 = Convert.ToString(dt.Rows[0]["Section 16"]);
                    //this.Section80C = Convert.ToString(dt.Rows[0]["Section 80CCE: 80C, 80CCC,80-CCD(1)"]);
                    this.Section80CCD = Convert.ToString(dt.Rows[0]["Under Section 80CCD"]);

                    this.TotalIncome = Convert.ToString(dt.Rows[0]["Total Income(Round By 10 Rupess)"]);
                    this.TaxOnTotalIncome = Convert.ToString(dt.Rows[0]["TOTITAX"]);
                    this.section87 = Convert.ToString(dt.Rows[0]["RU87A"]);
                    this.section89 = Convert.ToString(dt.Rows[0]["RU 89"]);
                    this.Surcharge = Convert.ToString(dt.Rows[0]["SURCHARGE"]);
                    this.FinalTaxPayable = Convert.ToString(dt.Rows[0]["TOTTAX"]);
                    this.TaxPayable = Convert.ToString(Convert.ToDecimal(this.FinalTaxPayable) + Convert.ToDecimal(this.section89));
                }


                GrossSalarySection = GrossSalarySection.AsEnumerable()
                         .Where(r => r.Field<DateTime>("applydate") == Convert.ToDateTime(dt.Rows[0]["Applydate"]))
                         .CopyToDataTable();




                if (GrossSalarySection.Rows.Count > 0)
                {
                    //for (int i = 0; i < GrossSalarySection.Rows.Count; i++)
                    //{
                    //    if (Convert.ToString(GrossSalarySection.Rows[i]["Grosssection"]) == "3")
                    //        this.GrossSection3 = Convert.ToString(GrossSalarySection.Rows[i]["GrossSectionTotal"]);
                    //    else if (Convert.ToString(GrossSalarySection.Rows[i]["Grosssection"]) == "2")
                    //        this.GrossSection2 = Convert.ToString(GrossSalarySection.Rows[i]["GrossSectionTotal"]);
                    //    else
                    //        this.GrossSection1 = Convert.ToString(GrossSalarySection.Rows[i]["GrossSectionTotal"]);
                    //}

                    var GrossSection3 = GrossSalarySection.AsEnumerable()
                               .Where(r => r.Field<int?>("Grosssection") == 3)
                                .Sum(r => r.Field<decimal>("GrossSectionTotal"));
                    var GrossSection2 = GrossSalarySection.AsEnumerable()
                              .Where(r => r.Field<int?>("Grosssection") == 2)
                               .Sum(r => r.Field<decimal>("GrossSectionTotal"));
                    var GrossSection1 = GrossSalarySection.AsEnumerable()
                              .Where(r => r.Field<int?>("Grosssection") == 1)
                               .Sum(r => r.Field<decimal>("GrossSectionTotal"));

                    //  this.Grosssalary= Convert.ToString(GrossSection3+ GrossSection2+ GrossSection1);
                    this.GrossSection3 = Convert.ToString(GrossSection3);
                    this.GrossSection2 = Convert.ToString(GrossSection2);
                    this.GrossSection1 = Convert.ToString(GrossSection1);

                }
                if (SectionDeductible.Rows.Count > 0)
                {
                    this.Section80C = Convert.ToString(SectionDeductible.Rows[0]["Section 80CCE: 80C, 80CCC,80-CCD(1)"]);
                    //   this.UnderChapterVIA = Convert.ToString(SectionDeductible.Rows[0]["Other Deductible amounts under Chapter VI A"]);
                    this.UnderChapterVIA = string.IsNullOrEmpty(this.UnderChapterVIA) ? "0" : this.UnderChapterVIA;
                    this.UnderChapterVIA = Convert.ToString(Convert.ToDecimal(this.Section80C) + Convert.ToDecimal(this.UnderChapterVIA));
                }
            }

        }
    }

}

