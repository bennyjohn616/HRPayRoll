using Payroll.CustomFilter;
using PayrollBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers
{
    [SessionExpireAttribute]
    public class TaxEntityController : BaseController
    {
        // GET: TaxEntity
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetTaxBehavior(Guid financeYearId, Guid attributeid, string type = "")
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            TaxBehaviorList txSectionlist = new TaxBehaviorList(financeYearId, Guid.Empty, attributeid);
            List<jsonTxBehavior> jsondata = new List<jsonTxBehavior>();
            txSectionlist.ForEach(u => { jsondata.Add(jsonTxBehavior.toJson(u)); });
            return base.BuildJson(true, 200, "success", jsondata);

        }
        public JsonResult SaveTaxBehavior(jsonTxBehavior dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            TaxBehavior txbehavior = jsonTxBehavior.convertObject(dataValue);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            txbehavior.CreatedBy = userId;
            txbehavior.ModifiedBy = txbehavior.CreatedBy;
            txbehavior.IsDeleted = false;
            isSaved = txbehavior.Save();
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }
        public JsonResult DeleteTaxBehavior(Guid id)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            TXSection txSection = new TXSection();
            txSection.Id = id;
            txSection.CompanyId = companyId;
            txSection.ModifiedBy = userId;
            txSection.Delete();
            return base.BuildJson(true, 200, "success", null);
        }

    }
    public class jsonTxBehavior
    {

        public Guid id { get; set; }


        public Guid financeyearId { get; set; }


        public Guid attributeId { get; set; }

        public int category { get; set; }
        public string value { get; set; }

        public string formula { get; set; }

        public string fieldfor { get; set; }
        public int inputtype { get; set; }

        public string baseValue { get; set; }
        public string baseFormula { get; set; }

        public string fieldtype { get; set; }


        public static jsonTxBehavior toJson(TaxBehavior txbehaviour)
        {
            return new jsonTxBehavior()
            {
                id = txbehaviour.Id,
                attributeId = txbehaviour.AttributemodelId,
                financeyearId = txbehaviour.FinanceYearId,
                value = txbehaviour.Value,
                formula = txbehaviour.Formula,
                inputtype = txbehaviour.InputType,
                fieldtype = txbehaviour.FieldType,
                category = txbehaviour.SlabCategory,
                fieldfor = txbehaviour.FieldFor,
                baseFormula = txbehaviour.BaseFormula,
                baseValue =txbehaviour.BaseValue,
            };
        }
        public static TaxBehavior convertObject(jsonTxBehavior txbehaviour)
        {
            return new TaxBehavior()
            {
                Id = txbehaviour.id,
                AttributemodelId = txbehaviour.attributeId,
                FinanceYearId = txbehaviour.financeyearId,
                Value = txbehaviour.value,
                Formula = txbehaviour.formula,
                InputType = txbehaviour.inputtype,
                FieldType = txbehaviour.fieldtype,
                SlabCategory = txbehaviour.category,
                FieldFor = txbehaviour.fieldfor,
                BaseFormula=txbehaviour.baseFormula,
                BaseValue=txbehaviour.baseValue
            };
        }
    }
}