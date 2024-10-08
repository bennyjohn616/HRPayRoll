using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollBO;
using Payroll.CustomFilter;

namespace Payroll.Controllers
{
    [SessionExpireAttribute]
    public class PremiumSettingController : BaseController
    {
        // GET: PremiumSetting
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult SavePremiumSettingComponent(jsonpremimumsettingcomponent dataValue)
        {
            bool result = new bool();
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            PremiumSettingComponent premiumSettingComponent = jsonpremimumsettingcomponent.convertobject(dataValue);
            premiumSettingComponent.ModifiedBy = userId;
            premiumSettingComponent.CreatedBy = userId;
            premiumSettingComponent.CompanyId = companyId;
            string[] component = dataValue.PAttrId.TrimEnd(',').Split(',');

            //Delete Records before saving
            premiumSettingComponent.Delete();

            foreach (string id in component)
            {
                premiumSettingComponent.AttrId = new Guid(id);
                if (premiumSettingComponent.Save())
                {
                    result = true;
                }
                else
                {
                    result = false;
                    break;
                }
            }
            if (result)
            {
                result = true;
                return base.BuildJson(true, 200, "Success", dataValue);
            }
            else
            {
                result = false;
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }

        }

        public JsonResult SavePremiumSetting(jsonpremimumsetting dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            PremiumSetting premiumSetting = jsonpremimumsetting.convertobject(dataValue);
            premiumSetting.ModifiedBy = userId;
            premiumSetting.CreatedBy = userId;
            premiumSetting.CompanyId = companyId;
            if (premiumSetting.Save())
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }

            //Delete Records before saving
            //premiumSettingComponent.Delete();

            //foreach (string id in component)
            //{
            //    premiumSetting.AttrId = new Guid(id);
            //    if (premiumSetting.Save())
            //    {
            //        result = true;
            //    }
            //    else
            //    {
            //        result = false;
            //        break;
            //    }
            //}
            //if (result)
            //{
            //    result = true;
            //    return base.BuildJson(true, 200, "Success", dataValue);
            //}
            //else
            //{
            //    result = false;
            //    return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            //}

        }
        public JsonResult GetComponents()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            AttributeModelList attrlist = new AttributeModelList(companyId);

            EntityModel entitymodel = new EntityModel(ComValue.SalaryTable, companyId);
            //  EntityModelList entitymodellist = new EntityModelList(Guid.Empty);
            //EntityBehavior entitybehaviour = new EntityBehavior();
            //EntityBehaviorList entitybehaviourlist = new EntityBehaviorList();
            List<jsonComponent> jcmpnt = new List<jsonComponent>();

            //if (entitymodel.Id != null)
            //{
            //    Entity entitylist = new EntityList(entitymodel.Id).FirstOrDefault();
            //    if (entitylist.EntityModelId == entitymodel.Id)
            //    {
            //        EntityBehaviorList entitybehaviourlist = new EntityBehaviorList(entitylist.Id, entitymodel.Id);
            //        entitybehaviourlist.ForEach(u =>
            //        {
            //            if (u.EntityModelId == entitymodel.Id && u.ValueType == 3)
            //            {
            //                attrlist.ForEach(r =>
            //                {
            //                    if (r.Id == u.AttributeModelId)
            //                    {
            //                        jcmpnt.Add(new jsonComponent() { Id = r.Id, displayAs = r.DisplayAs });
            //                    }
            //                });
            //            }

            //        });
            //    }
            //}
            attrlist.ForEach(r =>
            {
                if (r.IsIncludeForGrossPay && r.IsMonthlyInput==false)
                {
                    jcmpnt.Add(new jsonComponent() { Id = r.Id, displayAs = r.DisplayAs });
                }
            });
            return base.BuildJson(true, 200, "success", jcmpnt);
        }
        public JsonResult GetlopcreditComponents()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            AttributeModelList attrlist = new AttributeModelList(companyId);

            //EntityModel entitymodel = new EntityModel();
            EntityModelList entitymodellist = new EntityModelList(Guid.Empty);
            //EntityBehavior entitybehaviour = new EntityBehavior();
            //EntityBehaviorList entitybehaviourlist = new EntityBehaviorList();
            List<jsonComponent> jcmpnt = new List<jsonComponent>();
            entitymodellist.ForEach(p =>
            {
                if (p.CompanyId == companyId && p.Name == "Salary")
                {
                    Entity entitylist = new EntityList(p.Id).FirstOrDefault();
                    if (entitylist.EntityModelId == p.Id)
                    {
                        EntityBehaviorList entitybehaviourlist = new EntityBehaviorList(entitylist.Id, p.Id);
                        entitybehaviourlist.ForEach(u =>
                        {
                            if (u.EntityModelId == p.Id && u.ValueType == 1)
                            {
                                attrlist.ForEach(r =>
                                {
                                    if (r.Id == u.AttributeModelId)
                                    {
                                        jcmpnt.Add(new jsonComponent() { Id = r.Id, displayAs = r.DisplayAs });
                                    }
                                });
                            }

                        });
                    }
                }
            });
            return base.BuildJson(true, 200, "success", jcmpnt);
        }
        public JsonResult GetSavedComponents(Guid CategoryId, string Type)
        {
            int companyid = Convert.ToInt32(Session["CompanyId"]);
            PremiumSettingComponentList premiumcomponent = new PremiumSettingComponentList(companyid, Type, CategoryId);
            List<jsonpremimumsettingcomponent> jsoncomponent = new List<jsonpremimumsettingcomponent>();
            premiumcomponent.ForEach(u =>
            {
                jsoncomponent.Add(jsonpremimumsettingcomponent.tojson(u));
            });

            return base.BuildJson(true, 200, "success", jsoncomponent);
        }
        public class jsonComponent
        {
            public Guid Id { get; set; }

            public string displayAs { get; set; }
        }
        public class jsonpremimumsettingcomponent
        {

            public int PId { get; set; }

            public string PAttrId { get; set; }

            public Guid PCategory { get; set; }

            public string PAttributeName { get; set; }

            public string PType { get; set; }

            public static jsonpremimumsettingcomponent tojson(PremiumSettingComponent premiumsettingcomponent)
            {
                return new jsonpremimumsettingcomponent()
                {
                    PId = premiumsettingcomponent.Id,
                    PCategory = premiumsettingcomponent.AttrId,
                    PAttributeName = premiumsettingcomponent.AttributeName,
                    PType = premiumsettingcomponent.Type

                };
            }
            public static PremiumSettingComponent convertobject(jsonpremimumsettingcomponent premiumsettingcomponent)
            {
                return new PremiumSettingComponent()
                {
                    Id = premiumsettingcomponent.PId,
                    Type = premiumsettingcomponent.PType,
                    CategoryId = premiumsettingcomponent.PCategory
                };
            }

        }
        public class jsonpremimumsetting
        {

            public int PId { get; set; }

            public Guid PComponent { get; set; }

            public int PBackMonth { get; set; }
            public string PType { get; set; }


            public static jsonpremimumsetting tojson(PremiumSetting premiumsetting)
            {
                return new jsonpremimumsetting()
                {
                    PId = premiumsetting.Id,
                    PComponent = premiumsetting.Component,
                    PBackMonth = premiumsetting.BackMonth,
                    PType = premiumsetting.Type

                };
            }
            public static PremiumSetting convertobject(jsonpremimumsetting premiumsetting)
            {
                return new PremiumSetting()
                {
                    Id = premiumsetting.PId,
                    Component = premiumsetting.PComponent,
                    BackMonth = premiumsetting.PBackMonth,
                    Type = premiumsetting.PType
                };
            }

        }
    }
}