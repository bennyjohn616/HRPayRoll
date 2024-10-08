using Payroll.Controllers.Tax;
using Payroll.CustomFilter;
using PayrollBO;
using System;
using System.Collections.Generic;
//using PayrollBO.Util;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using TraceError;

namespace Payroll.Controllers
{
    [SessionExpireAttribute]
    public class EntityController : BaseController//Controller
    {
        //
        // GET: /Entity/

        public ActionResult Index()
        {
            if (object.ReferenceEquals(Session["UserId"], null))
                return RedirectToAction("Index", "Login");
            return View("~/Views/Company/CompanyView.cshtml");
        }

        public JsonResult GetEntityRelationtable()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            List<object> lstrekation = new List<object>();
            lstrekation.Add(new { Name = "Employee", DisplayAs = "Employee" });
            return base.BuildJson(true, 200, "success", lstrekation);
        }

        public JsonResult GetEntityModels()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            TableCategoryList tablcategory = new TableCategoryList(companyId);
            var categry = tablcategory.Where(u => u.Name.Contains("Payroll")).FirstOrDefault();
            EntityModelList data = new EntityModelList(categry.Id);
            EntityModelList ml = new EntityModelList();
            data.ForEach(d =>
            {
                var entity = data.Where(dat => d.Name.Trim() != "ITax").FirstOrDefault();
                if (entity != null)
                {
                    ml.Add(d);
                }

            });
            return base.BuildJson(true, 200, "success", ml);
        }

        public JsonResult SaveEntityModel(EntityModel dataValue, EntityModelMapping entitymap)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            dataValue.CompanyId = companyId;
            dataValue.CreatedBy = userId;
            isSaved = dataValue.Save();


            if (isSaved)
            {
                entitymap.EntityTableName = dataValue.Id.ToString();
                entitymap.CompanyId = companyId;
                entitymap.Save();
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult SaveAttributeModel(AttributeModel dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            dataValue.CompanyId = companyId;
            dataValue.CreatedBy = userId;
            dataValue.ModifiedBy = userId;
            dataValue.IsTaxable = dataValue.BehaviorType == "Deduction" ? false : dataValue.IsTaxable;
            isSaved = dataValue.Save();
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult SaveEntityAttributeModel(EntityAttributeModel dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            EntityAttributeModelList entityAttr = new EntityAttributeModelList(dataValue.EntityModelId);
            // Modified By AjithPanner on 16/11/2017
            EntityModel Enm = new EntityModel(dataValue.EntityModelId);
            if (Enm.Name == "Salary" && dataValue.AttributeModel.BehaviorType == "Master")
            {
                return base.BuildJson(false, 100, "Cannot Map Master Component With Salary Field", dataValue);
            }

            if (!object.ReferenceEquals(entityAttr.Where(u => u.AttributeModelId == dataValue.AttributeModelId).FirstOrDefault(), null))
            {
                return base.BuildJson(false, 100, "The column '" + entityAttr.Where(u => u.AttributeModelId == dataValue.AttributeModelId).FirstOrDefault().AttributeModel.DisplayAs + "' already Exist", dataValue);
            }

            if (dataValue.Id != Guid.Empty)
            {
                dataValue.AttributeModelId = entityAttr.Where(u => u.Id == dataValue.Id).FirstOrDefault() != null ? entityAttr.Where(u => u.Id == dataValue.Id).FirstOrDefault().AttributeModelId : Guid.Empty;
            }


            if (dataValue.AttributeModelId == Guid.Empty)
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
            int disOrder = 0;
            if (entityAttr.Count <= 0)
                disOrder = 0;
            else
                disOrder = entityAttr.Max(p => p.DisplayOrder);
            if (dataValue.DisplayOrder == 0)
                dataValue.DisplayOrder = disOrder + 1;
            isSaved = dataValue.Save();
            if (isSaved)
            {
                isSaved = dataValue.Merge(companyId, dataValue.EntityModelId, dataValue.AttributeModelId);    //To save in the dynamic module and formula for payroll
                AttributeModel attModel = new AttributeModel(dataValue.AttributeModelId, companyId);
                if (attModel.IsInstallment)
                {
                    LoanMaster loanmaster = new LoanMaster();
                    loanmaster.AttributeModelId = attModel.Id;
                    loanmaster.LoanCode = attModel.Name;
                    loanmaster.LoanDesc = attModel.DisplayAs;
                    loanmaster.IsInterest = false;
                    loanmaster.InterestPercent = 0;
                    loanmaster.CreatedBy = attModel.CreatedBy;
                    loanmaster.CompanyId = attModel.CompanyId;
                    loanmaster.Save();
                }
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }

        }

        public JsonResult GetEntityModel(Guid tablecategoryId, Guid id)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EntityModel data = new EntityModel(tablecategoryId, id);
            data.EntityAttributeModelList = new EntityAttributeModelList();
            EntityModelMapping entMap = new EntityModelMapping(Guid.Empty, id, companyId);
            return base.BuildJson(true, 200, "success", new { entityModel = data, entityMap = entMap });
        }

        public JsonResult GetEntityAttributeModel(Guid id, Guid entityModelId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EntityAttributeModel entityAttributeModel = new EntityAttributeModel(entityModelId, id);
            return base.BuildJson(true, 200, "success", entityAttributeModel);
        }
        public JsonResult GetEntityAttributeValue(Guid id)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EntityList entity = new EntityList(id);
            return base.BuildJson(true, 200, "success", entity);
        }
        public JsonResult GetFormulas(Guid categoryId, string type)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            AttributeModelTypeList attributeModelTypelist = new AttributeModelTypeList(companyId);
            AttributeModelType attributeModelType = attributeModelTypelist.Where(u => u.Name == type).FirstOrDefault();
            AttributeModelList attributeModelList = new AttributeModelList(companyId, attributeModelType.Id);
            AttributeModelBehaviorList attributeModelBehaviorList = new AttributeModelBehaviorList(categoryId, companyId);

            IEnumerable<AttributeModelBehavior> removeresult = attributeModelBehaviorList.Where(p => !attributeModelList.Any(p2 => p2.Id == p.AttrubuteModelId)) as IEnumerable<AttributeModelBehavior>;
            for (int count = 0; count < removeresult.Count(); count++)
            {
                attributeModelBehaviorList.Remove(removeresult.ToList()[count]);
            }
            //add the item which is not there in attribute model behavir
            var result = attributeModelList.Where(p => !attributeModelBehaviorList.Any(p2 => p2.AttrubuteModelId == p.Id));
            foreach (AttributeModel temp in result)
            {
                attributeModelBehaviorList.Add(new AttributeModelBehavior { AttrubuteModelId = temp.Id });
            }

            AttributeModelList masterAttributeModelList = new AttributeModelList();
            foreach (var tmp in attributeModelTypelist)
            {
                AttributeModelList temp = new AttributeModelList(companyId, tmp.Id);
                foreach (var atrmodel in temp)
                {
                    masterAttributeModelList.Add(atrmodel);
                }


            }


            List<jsonFormula> formla = new List<jsonFormula>();
            foreach (AttributeModelBehavior temp in attributeModelBehaviorList)
            {
                var atrModel = attributeModelList.Where(u => u.Id == temp.AttrubuteModelId).FirstOrDefault();
                var roun = Rounding.Roundings().Where(u => u.Id == temp.RoundingId).FirstOrDefault();
                var val = FormulaValueType.FormulaValueTypes().Where(u => u.Id == temp.ValueType).FirstOrDefault();
                formla.Add(new jsonFormula
                {
                    CategoryId = temp.CategoryId,
                    displayAs = atrModel != null ? atrModel.DisplayAs : "",
                    Formula = ProcessFormula(masterAttributeModelList, temp),
                    hiddenform = temp.Formula,
                    FormulaId = temp.AttrubuteModelId,
                    Maximum = temp.Maximum,
                    name = atrModel != null ? atrModel.Name : "",
                    Percentage = temp.Percentage,
                    rounding = roun != null ? roun.Name : "",
                    RoundingId = temp.RoundingId,
                    type = val != null ? val.Name : "",
                    ValueType = temp.ValueType
                });

            }
            return base.BuildJson(true, 200, "success", formla);


        }

        public JsonResult GetEntityList(Guid id)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EntityList entityList = new EntityList(id);


            List<object> retobject = new List<object>();
            entityList.ToList().ForEach(e =>
           {
               e.EntityAttributeModelList.ToList().ForEach(a =>
               {
                   if (a.AttributeModel.Name != "EG" && a.AttributeModel.Name != "TOTDED")
                   {
                       e.EntityAttributeModelList.Remove(a);
                   }
               });
           });

            retobject.Add(entityList);
            List<object> refEntity = new List<object>();
            entityList[0].EntityAttributeModelList.ForEach(u =>
            {
                if (u.AttributeModel.RefEntityModelId != Guid.Empty)
                {
                    List<EntityTemp> lstTemp = new List<EntityTemp>();
                    EntityList tefEnt = new EntityList(u.AttributeModel.RefEntityModelId);
                    tefEnt.ForEach(p =>
                    {
                        lstTemp.Add(EntityTemp.toJson(p));
                    });
                    var t = new { refEntityModelId = u.AttributeModel.RefEntityModelId, refEntityList = lstTemp };
                    refEntity.Add(t);
                }
            });
            retobject.Add(refEntity);
            return base.BuildJson(true, 200, "success", retobject);
        }
        /// <summary>
        /// CreatedBy:Sharmila
        /// CreatedOn:25.04.17
        /// </summary>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public JsonResult GetEntityValueList(jsonValue dataValue)
        {
            if (dataValue.EntityId == Guid.Empty && dataValue.EntityModelId == Guid.Empty)
            {
                return base.BuildJson(true, 200, "Inserted" + dataValue.textValue, dataValue);
            }
            if (!base.checkSession() && dataValue.textValue != "+")
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            string NewValue = dataValue.textValue;
            string[] newsplit = NewValue.Split('[', ']');
            string txtName = newsplit[0];
            EntityAttributeValueList ValueList = new EntityAttributeValueList(dataValue.EntityModelId);
            AttributeModelList AttModelList = new AttributeModelList(dataValue.EntityModelId);
            EntityAttributeValue entityAttributeValue = new EntityAttributeValue();
            AttributeModel AttModel = new AttributeModel();
            AttModelList.Add(AttModel);
            ValueList.Add(entityAttributeValue);
            EntityBehaviorList entitybehaviourlist = new EntityBehaviorList(dataValue.EntityAttModelId, dataValue.EntityModelId);
            // var entitybehalist= entitybehaviourlist.Where(eb=>eb.ArrearAttributeModelId==)
            if (dataValue.textValue != "+" && dataValue.textValue != "-" && dataValue.textValue != "*" && dataValue.textValue != "-" && dataValue.textValue != "/" && dataValue.textValue != "(" && dataValue.textValue != ")")
            {
                AttModel = AttModelList.Where(k => k.Name == txtName.Trim()).FirstOrDefault();
                if (AttModel != null)
                {
                    EntityBehavior entbehavior = new EntityBehavior(AttModel.AttributeModelTypeId, dataValue.EntityModelId, dataValue.EntityId);
                    EntityBehaviorList EntbehaviorList = new EntityBehaviorList(dataValue.EntityId, dataValue.EntityModelId);
                    EntbehaviorList.Add(entbehavior);
                    AttributeModelList AttrModelList = new AttributeModelList(companyId, entbehavior.AttributeModelId);
                    AttrModelList.Add(AttModel);
                    AttModel = AttrModelList.Where(k => k.Name == txtName.Trim()).FirstOrDefault();
                    EntityAttributeModelList EntModelList = new EntityAttributeModelList(dataValue.EntityModelId);
                    EntityAttributeModel EntModel = new EntityAttributeModel();
                    EntModelList.Add(EntModel);
                    EntModel = EntModelList.Where(s => s.AttributeModelId == AttModel.Id).FirstOrDefault();
                    var ValueCheck = ValueList.Where(d => d.EntityAttributeModelId == EntModel.Id).FirstOrDefault();
                    string value = "";
                    if (dataValue.textName == "txtFormula")
                    {
                        if (dataValue.textValue != "+" && dataValue.textValue != "-" && dataValue.textValue != "*" && dataValue.textValue != "-" && dataValue.textValue != "/" && dataValue.textValue != "(" && dataValue.textValue != ")")
                        {

                            if (AttModel != null)
                            {
                                if (ValueCheck != null)
                                {
                                    value = ValueCheck.Value;
                                }
                                if (string.IsNullOrEmpty(value) || value == "NULL")
                                {

                                    return base.BuildJson(false, 200, "No Formula for " + txtName, dataValue);
                                }
                                else
                                {
                                    return base.BuildJson(true, 200, "Formula for " + txtName, dataValue);
                                }

                            }
                        }
                    }
                    else if (dataValue.textName == "txtArrearMapField")
                    {
                        if (dataValue.textValue != "+" && dataValue.textValue != "-" && dataValue.textValue != "*" && dataValue.textValue != "-" && dataValue.textValue != "/" && dataValue.textValue != "(" && dataValue.textValue != ")")
                        {
                            AttModel = AttModelList.Where(k => k.Name == txtName.Trim()).FirstOrDefault();
                            var arrearattrId = AttModel.Id;
                            // Created by AjithPanner on 11/11/2017
                            AttributeModelType arrearattmodeltype = new AttributeModelType(AttModel.AttributeModelTypeId, companyId);
                            if (arrearattmodeltype.Name != "Earning")
                            {
                                return base.BuildJson(false, 200, "Only Earning master component allowed for Arrear Mapping", dataValue);
                            }

                            var arreattr = entitybehaviourlist.Where(eb => eb.AttributeModelId == arrearattrId).FirstOrDefault();
                            var arr = entitybehaviourlist.Where(eb => eb.AttributeModelId != arrearattrId && eb.ArrearAttributeModelId == arrearattrId).FirstOrDefault();


                            // Modified by AjithPanner on 3/11/2017
                            if (arreattr == null)
                            {
                                return base.BuildJson(false, 200, "Arrear component not yet saved" + txtName, dataValue);
                            }
                            else if (arreattr != null && arreattr.ValueType != 1 && dataValue.textName == "txtArrearMapField")
                            {
                                return base.BuildJson(false, 200, "Not as Master" + txtName, dataValue);

                            }
                            else if (arr == null)
                            {
                                EntityBehavior arrearbehavio = new EntityBehavior(arreattr.ArrearAttributeModelId, arreattr.EntityModelId, arreattr.EntityId);
                                arrearbehavio.ArrearAttributeModelId = Guid.Empty;
                                arrearbehavio.Save();
                                return base.BuildJson(true, 200, "success", dataValue);
                            }
                            else if (arreattr != null && arr.ArrearAttributeModelId == arrearattrId && dataValue.textName == "txtArrearMapField")
                            {
                                return base.BuildJson(false, 200, "Already Mapped" + txtName, dataValue);
                            }
                        }
                    }
                }
            }
            return base.BuildJson(true, 200, "success", dataValue);
        }
        public class jsonValue
        {
            public Guid EntityModelId { get; set; }
            public string textValue { get; set; }
            public Guid EntityAttModelId { get; set; }
            public Guid EntityId { get; set; }
            public string textName { set; get; }
            public string AttributeCode { get; set; }
            public Guid AttributeId { get; set; }
        }

        public class jsonEntityMasterTemp
        {

            public Guid EmployeeId { get; set; }

            public Guid financeyearId { get; set; }
            public DateTime Effectivedate { get; set; }
            public char Applymmyy { get; set; }
            public Guid CompId { get; set; }

            public decimal Value { get; set; }

        }

        public static jsonEntityMasterTemp toJson(EntityTempData entTemp)
        {

            return new jsonEntityMasterTemp()
            {
                EmployeeId = entTemp.EmployeeId,
                financeyearId = new Guid(Convert.ToString(entTemp.financeyearId)),
                CompId = new Guid(Convert.ToString(entTemp.CompId)),
                Value = Convert.ToDecimal(entTemp.Value),
            };
        }



        public JsonResult GetEntity(Guid id, Guid entitymodelId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            Entity entityList;
            if (id != Guid.Empty)
            {
                entityList = new Entity(entitymodelId, id);
            }
            else
            {
                entityList = new Entity();
                entityList.EntityModelId = entitymodelId;
            }
            entityList.EntityAttributeModelList.OrderBy(p => p.AttributeModel.DisplayAs);

            Entity newEntityList = new Entity();
            newEntityList.CreatedBy = entityList.CreatedBy;
            newEntityList.CreatedOn = entityList.CreatedOn;
            // newEntityList.EntityModelId = entityList.EntityModelId;
            newEntityList.Id = entityList.Id;
            newEntityList.IsActive = entityList.IsActive;
            newEntityList.IsDeleted = entityList.IsDeleted;
            newEntityList.ModifiedBy = entityList.ModifiedBy;
            newEntityList.ModifiedOn = entityList.ModifiedOn;
            newEntityList.Name = entityList.Name;


            entityList.EntityAttributeModelList.OrderBy(p => p.AttributeModel.DisplayAs).ToList().ForEach(f =>
            {
                newEntityList.EntityAttributeModelList.Add(f);
            });
            newEntityList.EntityModelId = entityList.EntityModelId;

            return base.BuildJson(true, 200, "success", newEntityList);
        }

        public JsonResult SaveEntity(jsonEntity dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Entity entity = ConvertEntityObject(dataValue);
            entity.CreatedBy = userId;
            entity.ModifiedBy = userId;
            isSaved = entity.Save();
            if (isSaved)
            {
                entity.EntityAttributeModelList.ForEach(u =>
                {
                    // Command on 11th july 2018 set monthly input round off value 
                    //if (u.AttributeModel.IsMonthlyInput)
                    //{
                    //    EntityBehavior entBehavior = new EntityBehavior();
                    //    entBehavior.EntityId = entity.Id;
                    //    entBehavior.EntityModelId = entity.EntityModelId;
                    //    entBehavior.AttributeModelId = u.AttributeModelId;
                    //    entBehavior.ValueType = 2;
                    //    entBehavior.Save();
                    //}
                    if (u.AttributeModel.Name.ToUpper() == "EG" || u.AttributeModel.Name.ToUpper() == "TOTDED" || u.AttributeModel.Name.ToUpper() == "NETPAY")
                    {
                        AttributeModelList attributeModelList = new AttributeModelList(companyId); // Guid.Empty, companyId);
                        if (!string.IsNullOrEmpty(Convert.ToString(u.EntityAttributeValue.Value)))
                        {

                            string[] name = u.EntityAttributeValue.Value.Replace("+", ",").Replace("-", ",").Replace("*", ",").Replace("/", ",").Replace("(", ",").Replace(")", ",").Replace("=", ",").Split(',');
                            u.EntityAttributeValue.Value = u.EntityAttributeValue.Value.Replace("+", " + ").Replace("-", " - ").Replace("*", " * ").Replace("/", " / ").Replace("(", " ( ").Replace(")", " ) ").Replace("=", " = ");
                            u.EntityAttributeValue.Value = u.EntityAttributeValue.Value + " ";
                            foreach (object s in name)
                            {

                                AttributeModel attr = new AttributeModel();


                                attr = attributeModelList.FirstOrDefault(e => e.Name == s.ToString() && e.BehaviorType != "Master");

                                if (!object.ReferenceEquals(attr, null))
                                {
                                    //  u.EntityAttributeValue.Value= Regex.Replace(u.EntityAttributeValue.Value, @"\b"+ s.ToString() + "\b" , "{" + attr.Id.ToString() + "}");
                                    //u.EntityAttributeValue.Value = u.EntityAttributeValue.Value.Replace( s.ToString() + " ", "{" + attr.Id.ToString() + "}");
                                    u.EntityAttributeValue.Value = Regex.Replace(u.EntityAttributeValue.Value, @"\b" + s.ToString() + @"\b ", "{" + attr.Id.ToString() + "}");

                                }


                            }
                        }
                        EntityBehavior enbehv = new EntityBehavior(u.AttributeModelId, entity.EntityModelId, entity.Id);
                        EntityBehavior entBehavior = new EntityBehavior();
                        entBehavior.EntityId = entity.Id;
                        entBehavior.EntityModelId = entity.EntityModelId;
                        entBehavior.AttributeModelId = u.AttributeModelId;
                        entBehavior.ValueType = 3;
                        entBehavior.Percentage = "100";
                        entBehavior.RoundingId = enbehv.RoundingId;
                        if (!string.IsNullOrEmpty(Convert.ToString(u.EntityAttributeValue.Value)))
                        {
                            entBehavior.Formula = u.EntityAttributeValue.Value.Trim(' ');
                        }
                        entBehavior.Save();
                    }
                });
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }

        }

        public JsonResult CopyEntity(Guid id, Guid entityModelId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Entity entity = new Entity();
            isSaved = entity.Copy(id, entityModelId, userId);
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data Deleted successfully", null);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while delete the data.", null);
            }

        }

        public JsonResult DeleteEntity(Guid id, Guid entityModelId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Entity entity = new Entity();
            entity.CreatedBy = userId;
            entity.ModifiedBy = userId;
            entity.Id = id;
            entity.EntityModelId = entityModelId;
            isSaved = entity.Delete();
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data Deleted successfully", null);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while delete the data.", null);
            }

        }


        public JsonResult CopyDynamicGroupEntity(Guid id, Guid entityModelId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSaved = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Entity entity = new Entity();
            isSaved = entity.Copy(id, entityModelId, userId);
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data Deleted successfully", null);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while delete the data.", null);
            }

        }

        private Entity ConvertEntityObject(jsonEntity input)
        {

            Entity entity;
            if (input.entityId == Guid.Empty)
            {
                entity = new Entity();
                entity.EntityModelId = input.entityModelId;
            }
            else
                entity = new Entity(input.entityModelId, input.entityId);
            foreach (jsonEntityKeyValue attVal in input.EntityKeyValues)
            {
                if (attVal.id == "Name")
                    entity.Name = attVal.value;
                else
                {
                    for (int cnt = 0; cnt < entity.EntityAttributeModelList.Count; cnt++)
                    {
                        if (entity.EntityAttributeModelList[cnt].Id == Guid.Parse(attVal.id))
                        {
                            if (object.ReferenceEquals(entity.EntityAttributeModelList[cnt].EntityAttributeValue, null))
                            {
                                if (object.ReferenceEquals(entity.EntityAttributeModelList[cnt].EntityAttributeValue.Id, null))
                                {
                                    entity.EntityAttributeModelList[cnt].EntityAttributeValue.ValueCode = string.Empty;
                                    if (!string.IsNullOrEmpty(attVal.name))
                                    {
                                        entity.EntityAttributeModelList[cnt].EntityAttributeValue.Value = attVal.name;
                                        entity.EntityAttributeModelList[cnt].EntityAttributeValue.ValueCode = attVal.value;
                                    }
                                    else
                                        entity.EntityAttributeModelList[cnt].EntityAttributeValue.Value = attVal.value;
                                }
                            }
                            else
                            {
                                if (object.ReferenceEquals(entity.EntityAttributeModelList[cnt].EntityAttributeValue.Id, null))
                                {
                                    entity.EntityAttributeModelList[cnt].EntityAttributeValue.ValueCode = string.Empty;
                                    if (!string.IsNullOrEmpty(attVal.name))
                                    {
                                        entity.EntityAttributeModelList[cnt].EntityAttributeValue.Value = attVal.name;
                                        entity.EntityAttributeModelList[cnt].EntityAttributeValue.ValueCode = attVal.value;
                                    }
                                    else
                                        entity.EntityAttributeModelList[cnt].EntityAttributeValue.Value = attVal.value;
                                }
                                else
                                {
                                    entity.EntityAttributeModelList[cnt].EntityAttributeValue.ValueCode = string.Empty;
                                    if (!string.IsNullOrEmpty(attVal.name))
                                    {
                                        entity.EntityAttributeModelList[cnt].EntityAttributeValue.Value = attVal.name;
                                        entity.EntityAttributeModelList[cnt].EntityAttributeValue.ValueCode = attVal.value;
                                    }
                                    else
                                        entity.EntityAttributeModelList[cnt].EntityAttributeValue.Value = attVal.value;
                                }
                            }
                        }
                    }

                }
            }
            return entity;

        }



        private string ProcessFormula(AttributeModelList attrlist, AttributeModelBehavior attrBehavior)
        {
            string formula = attrBehavior.Formula;
            string tempFormula = string.Empty;
            if (!string.IsNullOrEmpty(formula))
            {
                if (formula.IndexOf('{') >= 0)
                {
                    do
                    {
                        int startIndex = formula.IndexOf('{');
                        int endIndex = formula.IndexOf('}');
                        //formula = formula.Remove(formula.IndexOf('{'), 1);
                        //string id = formula.Substring(0, formula.IndexOf('}'));
                        string id = formula.Substring(startIndex + 1, endIndex - (startIndex + 1));
                        //formula = formula.Remove(0, formula.IndexOf('}') + 1);
                        var tempAttrmodel = attrlist.Where(u => u.Id == new Guid(id)).FirstOrDefault();
                        if (!object.ReferenceEquals(tempAttrmodel, null))
                        {
                            // tempFormula = tempFormula + tempAttrmodel.Name;
                            formula = formula.Remove(startIndex, endIndex - (startIndex - 1));
                            formula = formula.Insert(startIndex, tempAttrmodel.Name);
                        }
                        else
                        {
                            formula = formula.Remove(startIndex, endIndex - (startIndex - 1));
                        }

                        //if (!string.IsNullOrEmpty(formula))
                        //{
                        //    tempFormula = tempFormula + formula.Substring(0, formula.IndexOf('{'));
                        //    formula = formula.Remove(0, formula.IndexOf('{'));
                        //}
                        /*
                        int startIndex = formula.IndexOf('{');
                        int endIndex = formula.IndexOf('}');
                        formula = formula.Remove(formula.IndexOf('{'), 1);
                        //string id = formula.Substring(0, formula.IndexOf('}'));
                        string id = formula.Substring(startIndex, endIndex);
                        formula = formula.Remove(0, formula.IndexOf('}') + 1);
                        var tempAttrmodel = attrlist.Where(u => u.Id == new Guid(id)).FirstOrDefault();
                        if (!object.ReferenceEquals(tempAttrmodel, null))
                        {
                            tempFormula = tempFormula + tempAttrmodel.Name;
                        }
                        if (!string.IsNullOrEmpty(formula))
                        {
                            tempFormula = tempFormula + formula.Substring(0, formula.IndexOf('{'));
                            formula = formula.Remove(0, formula.IndexOf('{'));
                        }*/
                    } while (formula.IndexOf('{') >= 0);
                }
            }
            tempFormula = formula;
            if (!string.IsNullOrEmpty(tempFormula))
            {
                char lastchar = tempFormula[tempFormula.Length - 1];
                if (lastchar == '+' || lastchar == '-' || lastchar == '*' || lastchar == '/')
                    tempFormula = tempFormula.Remove(tempFormula.Length - 1, 1);
            }
            return tempFormula;
        }
        public JsonResult GetPayrollmodels(string name, string isTaxable = "")
        {
            // Boolean includeMaster = false;



            if (Convert.ToString(Session["Title"]) == "Tax")
            {
                // includeMaster = true;
            }
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EntityModel entity = new EntityModel(name, companyId);
            AttributeModelTypeList data = new AttributeModelTypeList(companyId);

            var temp = data;
            //var temp = includeMaster ? data : data.Where(u => !u.Name.Contains("Master")).ToList();
            temp.ForEach(u =>
            {
                u.AttributeModelList = u.AttributeModelList.FilterByContributionType(1);
            });
            EntityAttributeModelList entityAttributrmodels = entity.EntityAttributeModelList;//new EntityAttributeModelList(entity.Id);
            for (int cnt = 0; cnt < temp.Count; cnt++)
            {
                if (temp[cnt].Name != "Tax" && temp[cnt].Name != "Master")
                {
                    for (int count = 0; count < temp[cnt].AttributeModelList.Count; count++)
                    {
                        EntityAttributeModel t1 = new PayrollBO.EntityAttributeModel();
                        if (isTaxable == "isTaxable")
                        {
                            t1 = entityAttributrmodels.Where(p => p.AttributeModel.Id == temp[cnt].AttributeModelList[count].Id && temp[cnt].AttributeModelList[count].IsTaxable == false).FirstOrDefault();
                            if (t1 != null)
                            {
                                temp[cnt].AttributeModelList.RemoveAt(count);
                                //temp[cnt].AttributeModelList.Remove(t1.AttributeModel);
                            }
                        }
                        else
                        {
                            t1 = entityAttributrmodels.Where(p => p.AttributeModel.Id == temp[cnt].AttributeModelList[count].Id).FirstOrDefault();
                            if (t1 == null)
                            {
                                temp[cnt].AttributeModelList.RemoveAt(count);
                                //temp[cnt].AttributeModelList.Remove(temp[cnt].AttributeModelList[count]);
                            }
                        }

                    }
                }
            }


            return base.BuildJson(true, 200, "success", temp);
        }
        public JsonResult GetFormulaAttributeModelList(Guid entityModelId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            AttributeModelTypeList data = new AttributeModelTypeList(companyId);
            var temp = data.Where(u => !u.Name.Contains("Master")).ToList();
            temp.ForEach(u =>
            {
                u.AttributeModelList = u.AttributeModelList.FilterByContributionType(1);
            });
            EntityAttributeModelList entityAttributrmodels = new EntityAttributeModelList(entityModelId);
            for (int cnt = 0; cnt < temp.Count; cnt++)
            {
                for (int count = 0; count < temp[cnt].AttributeModelList.Count; count++)
                {
                    var t1 = entityAttributrmodels.Where(p => p.AttributeModel.Id == temp[cnt].AttributeModelList[count].Id || p.AttributeModel.IsSetting == false).FirstOrDefault();
                    if (t1 == null)
                    {
                        temp[cnt].AttributeModelList.RemoveAt(count);
                        //temp[cnt].AttributeModelList.Remove(temp[cnt].AttributeModelList[count]);
                    }
                }
            }
            return base.BuildJson(true, 200, "success", temp);
        }

        public JsonResult SaveFormula(jsonFormula dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            AttributeModelBehavior behavior = new AttributeModelBehavior(dataValue.FormulaId, dataValue.CategoryId, companyId);
            behavior.Formula = dataValue.hiddenform;
            behavior.Maximum = dataValue.Maximum;
            behavior.ModifiedBy = userId;
            behavior.Percentage = dataValue.Percentage;
            behavior.RoundingId = dataValue.RoundingId;
            behavior.ValueType = FormulaValueType.FormulaValueTypes().Where(u => u.Name == dataValue.type).FirstOrDefault().Id;
            if (behavior.Save())
            {
                return base.BuildJson(true, 200, "success", true);
            }
            else
            {
                return base.BuildJson(false, 0, "Error", false);
            }
        }

        public JsonResult GetEntityBehavior(Guid attributeModelId, Guid entityId, Guid entityModelId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            AttributeModelList attributeModelList = new AttributeModelList(companyId, Guid.Empty);
            EntityBehavior data = new EntityBehavior(attributeModelId, entityModelId, entityId);
            jsonEntityBehavior jsonData = jsonEntityBehavior.toJson(data, attributeModelList);
            if (jsonData.name == "PTAX")
            {

                var eligible = attributeModelList.Where(w => w.Name == "FG").FirstOrDefault();
                if (!Object.ReferenceEquals(eligible, null))
                {
                    EntityAttributeModelList entAttributemodel = new EntityAttributeModelList(jsonData.entityModelId);

                    var eligib = entAttributemodel.Where(w => w.AttributeModelId == eligible.Id).FirstOrDefault();
                    if (!Object.ReferenceEquals(eligib, null))
                    {
                        jsonData.eligibilityFormula = "FG";
                        jsonData.hiddenEligibilityFormula = "{" + eligible.Id.ToString() + "}";
                    }

                }
            }
            return base.BuildJson(true, 200, "success", jsonData);
        }

        public JsonResult SaveEntityBehavior(jsonEntityBehavior dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            if (dataValue.name == "PTAX")
            {
                AttributeModelList attributeModelList = new AttributeModelList(companyId, Guid.Empty);
                var eligible = attributeModelList.Where(w => w.Name == "FG").FirstOrDefault();
                if (!Object.ReferenceEquals(eligible, null))
                {
                    EntityAttributeModelList entAttributemodel = new EntityAttributeModelList(dataValue.entityModelId);

                    var eligib = entAttributemodel.Where(w => w.AttributeModelId == eligible.Id).FirstOrDefault();
                    if (!Object.ReferenceEquals(eligib, null))
                    {
                        dataValue.hiddenEligibilityFormula = "{" + eligible.Id.ToString() + "}";
                    }

                }
            }
            if (dataValue.entityId == Guid.Empty)
            {
                Entity entity = new Entity();
                entity.EntityModelId = dataValue.entityModelId;
                entity.Name = "New value";
                entity.CreatedBy = userId;
                if (entity.Save())
                    dataValue.entityId = entity.Id;
                else
                    return base.BuildJson(false, 0, "Error", false);
            }
            //EntityAttributeValueList ValueList = new EntityAttributeValueList(dataValue.entityModelId);
            EntityBehavior behavior = new EntityBehavior(dataValue.attrubuteModelId, dataValue.entityModelId, dataValue.entityId);
            behavior.Formula = dataValue.hiddenform;
            behavior.Maximum = dataValue.maximum;
            behavior.ModifiedBy = userId;
            behavior.Percentage = dataValue.percentage == null ? "" : dataValue.percentage;
            behavior.RoundingId = dataValue.roundingId;
            behavior.EligibiltyFormula = dataValue.hiddenEligibilityFormula;
            if (dataValue.valueType == 5)
            {
                behavior.Formula = dataValue.formula;
                behavior.BaseFormula = dataValue.baseFormula == null ? "" : dataValue.baseFormula;
                behavior.BaseValue = dataValue.baseValue == null ? "" : dataValue.baseValue;
            }

            if (!string.IsNullOrEmpty(dataValue.arrearMatchField))
                behavior.ArrearAttributeModelId = new Guid(dataValue.arrearMatchField);
            //  Created By AjithPanner on 11/11/2017
            if (dataValue.arrearMatchField == null)
            {
                EntityBehavior arrearbehavior = new EntityBehavior(behavior.ArrearAttributeModelId, dataValue.entityModelId, dataValue.entityId);
                arrearbehavior.ArrearAttributeModelId = Guid.Empty;
                arrearbehavior.Save();
                behavior.ArrearAttributeModelId = Guid.Empty;

            }
            //--------Modified on 20/07/2017

            behavior.ValueType = FormulaValueType.FormulaValueTypes().Where(u => u.Id == dataValue.valueType).FirstOrDefault().Id;
            if (behavior.Save())
            {

                EntityAttributeModelList entAttributemodel = new EntityAttributeModelList(dataValue.entityModelId);
                EntityAttributeValue entityAttributeValue = new EntityAttributeValue();
                entityAttributeValue.EntityAttributeModelId = entAttributemodel.Where(u => u.AttributeModelId == dataValue.attrubuteModelId).FirstOrDefault().Id;
                entityAttributeValue.EntityId = dataValue.entityId;
                entityAttributeValue.EntityModelId = dataValue.entityModelId;
                entityAttributeValue.Value = dataValue.formula;
                //ValueList.Add(entityAttributeValue);

                entityAttributeValue.Save();

                if (behavior.ArrearAttributeModelId != Guid.Empty)
                {
                    EntityBehavior behaviorArrear = new EntityBehavior(behavior.ArrearAttributeModelId, dataValue.entityModelId, dataValue.entityId);
                    behaviorArrear.Formula = "0";
                    behaviorArrear.ModifiedBy = userId;
                    behaviorArrear.RoundingId = dataValue.roundingId;
                    if (!string.IsNullOrEmpty(dataValue.arrearMatchField))
                        behaviorArrear.ArrearAttributeModelId = new Guid(dataValue.arrearMatchField);
                    // behaviorArrear.ValueType = 2;//Employee master Arrear compnent
                    behaviorArrear.Save();
                }
                //save the child behavior
                if (dataValue.childBehavior != null && dataValue.childBehavior.Count > 0)
                {
                    dataValue.childBehavior.ForEach(p =>
                    {
                        EntityBehavior Childbehavior = new EntityBehavior(p.attrubuteModelId, dataValue.entityModelId, dataValue.entityId);
                        Childbehavior.Formula = dataValue.hiddenform;
                        Childbehavior.Maximum = dataValue.maximum;
                        Childbehavior.ModifiedBy = userId;
                        Childbehavior.Percentage = p.percentage;
                        Childbehavior.RoundingId = dataValue.roundingId;
                        Childbehavior.EligibiltyFormula = dataValue.hiddenEligibilityFormula;
                        Childbehavior.ValueType = FormulaValueType.FormulaValueTypes().Where(u => u.Id == dataValue.valueType).FirstOrDefault().Id;
                        Childbehavior.Save();

                    });

                }


                return base.BuildJson(true, 200, "success", behavior);
            }
            else
            {
                return base.BuildJson(false, 0, "Error", false);
            }
        }

        public JsonResult GetEntityModelMap(string refModelName, string refEntityId, Guid entityModelId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EntityModelMappingList entitymodelmapList = new EntityModelMappingList();
            TableCategoryList tablCat = new TableCategoryList(companyId);
            EntityModelList entityModelList = new EntityModelList();
            if (entityModelId != Guid.Empty)
            {
                EntityModel entimodel = new EntityModel(entityModelId);
                entityModelList.Add(entimodel);
                EntityModelMapping entitymap = new EntityModelMapping(Guid.Empty, entityModelId, companyId);
                entitymodelmapList.Add(entitymap);
            }
            else
            {
                entityModelList = new EntityModelList(tablCat.Where(u => u.Name.Contains("Payroll")).FirstOrDefault().Id);
                entitymodelmapList = new EntityModelMappingList(refModelName, companyId);
            }
            List<jsonEntityModelMap> jsonData = new List<jsonEntityModelMap>();
            entitymodelmapList.ForEach(u => { jsonData.Add(jsonEntityModelMap.toJson(u, entityModelList, refEntityId, refModelName)); });
            return base.BuildJson(true, 200, "success", jsonData);
        }
        public JsonResult GetEntityWithFormula(Guid id, Guid entitymodelId, string refEntityId, string refEntityModel)//need to delete
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            Entity entity;
            if (id != Guid.Empty)
            {
                List<PayrollError> payErrors = new List<PayrollError>();
                PayrollHistory payHistory = new PayrollHistory();
                Employee emp = new Employee(companyId, new Guid(refEntityId));
                entity = payHistory.ExecuteProcessTemp(companyId, new Guid(refEntityId), DateTime.Now.Year, DateTime.Now.Month, id, entitymodelId, out payErrors);
                // entity=new Entity(entitymodelId, id);
            }
            else
            {
                entity = new Entity();
                entity.EntityModelId = entitymodelId;
            }
            return base.BuildJson(true, 200, "success", entity);
        }

        public JsonResult GetBABehavior(Guid attributeModelId, Guid entityId, Guid entityModelId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            AttributeModelList attributeModelList = new AttributeModelList(companyId, Guid.Empty);
            attributeModelList.Where(att => att.IsIncrement == true || att.IsTaxable == true).ToList();
            BABehavior data = new BABehavior(attributeModelId, entityModelId, entityId);
            jsonBABehavior jsonData = jsonBABehavior.toJson(data, attributeModelList);
            return base.BuildJson(true, 200, "success", jsonData);
        }

        /// <summary>
        /// ModifiedBy:sharmila
        /// </summary>
        /// <param name="entitymodelId"></param>
        /// <param name="refEntityId"></param>
        /// <param name="refEntityModel"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public JsonResult GetEntityMapping(Guid entityId, string Name)
        {

            EntityMapping entityMapping = new EntityMapping(entityId);
            PayrollHistory PayrollHistory = new PayrollHistory(entityId);
            string PayrollMapped = Convert.ToString(PayrollHistory.EntityId);
            string MappedValue = entityMapping.EntityId;
            if (string.IsNullOrEmpty(Convert.ToString(MappedValue)) || string.IsNullOrEmpty(PayrollMapped))
            {
                return base.BuildJson(false, 200, "success", MappedValue);
            }
            return base.BuildJson(true, 200, "Failed!  " + Name + " is in Use", MappedValue);
        }


        public JsonResult GetCatagoryMapping(Guid entityId, int cmpid, string Name)
        {

            //  EmployeeList emplist = new EmployeeList(cmpid,entityId);
            EntityMapping entityMapping = new EntityMapping(entityId);

            PayrollHistory PayrollHistory = new PayrollHistory(entityId);
            string PayrollMapped = Convert.ToString(PayrollHistory.EntityId);
            string MappedValue = entityMapping.EntityId;
            if (string.IsNullOrEmpty(Convert.ToString(MappedValue)) || string.IsNullOrEmpty(PayrollMapped))
            {
                return base.BuildJson(false, 200, "success", MappedValue);
            }
            return base.BuildJson(true, 200, "Failed!  " + Name + " is in Use", MappedValue);
        }
        public JsonResult GetEntityForEmployee(Guid entitymodelId, string refEntityId, string refEntityModel, Guid employeeId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Entity entity = new Entity();
            try
            {
                EntityModel entityModel = new EntityModel(entitymodelId);
                EntityMapping entityMapping = new EntityMapping(refEntityModel, refEntityId, entityModel.Id);
                if (!string.IsNullOrEmpty(entityMapping.EntityId) && entityMapping.EntityId != "00000000-0000-0000-0000-000000000000" && new Guid(entityMapping.EntityTableName) == entitymodelId)
                {
                    Guid tmp;
                    if (Guid.TryParse(entityMapping.EntityTableName, out tmp))
                    {

                        if (entityModel.Name.ToUpper() == ComValue.SalaryTable.ToUpper())
                        {
                            List<PayrollError> payErrors = new List<PayrollError>();
                            PayrollHistory payHistory = new PayrollHistory();
                            Employee emp = new Employee(companyId, new Guid(refEntityId));
                            //      PayrollHistoryList payrollHistoryList = new PayrollHistoryList(companyId);//Get All processed payroll Month and year
                            //     if (payrollHistoryList.Count > 0)
                            //   {

                            //        var firstElement = payrollHistoryList.First();

                            //        entity = payHistory.ExecuteProcessTemp(companyId, emp, firstElement.Year, firstElement.Month, new Guid(entityMapping.EntityId), entitymodelId, out payErrors);
                            //    }
                            //     else
                            //     {
                            entity = payHistory.ExecuteProcessTemp(companyId, new Guid(refEntityId), DateTime.Now.Year, DateTime.Now.Month, new Guid(entityMapping.EntityId), entitymodelId, out payErrors);
                            // }


                        }
                        else
                        {
                            entity = new Entity(new Guid(entityMapping.EntityTableName), new Guid(entityMapping.EntityId));
                            var entytMasterValues = new EntityMasterValueList(new Guid(refEntityId), refEntityModel).Where(u => u.EntityModelId == entitymodelId).ToList();
                            EntityAdditionalInfoList entyAdditionalInfo = new EntityAdditionalInfoList(companyId, entitymodelId, employeeId);
                            if (object.ReferenceEquals(entytMasterValues, null))
                                entytMasterValues = new List<EntityMasterValue>();
                            for (int cnt = 0; cnt < entytMasterValues.Count; cnt++)
                            {
                                if (entity.EntityAttributeModelList.Where(u => u.AttributeModelId == entytMasterValues[cnt].AttributeModelId).FirstOrDefault() != null)
                                    entity.EntityAttributeModelList.Where(u => u.AttributeModelId == entytMasterValues[cnt].AttributeModelId).FirstOrDefault().EntityAttributeValue.Value = entytMasterValues[cnt].Value;
                            }
                            if (object.ReferenceEquals(entyAdditionalInfo, null))
                            {
                                for (int cnt = 0; cnt < entyAdditionalInfo.Count; cnt++)
                                {

                                    if (entity.EntityAttributeModelList.Where(u => u.AttributeModelId == entyAdditionalInfo[cnt].AttributeModelId).FirstOrDefault() != null)
                                        entity.EntityAttributeModelList.Where(u => u.AttributeModelId == entyAdditionalInfo[cnt].AttributeModelId).FirstOrDefault().EntityAttributeValue.Value = entyAdditionalInfo[cnt].RefEntityId == Guid.Empty ? entyAdditionalInfo[cnt].Value : entyAdditionalInfo[cnt].RefEntityId.ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        entity = new Entity();
                    }

                }
                else if (entityModel.Name == "AdditionalInfo")
                {
                    entity = new Entity();
                    EntityAttributeModelList attr = new EntityAttributeModelList(entitymodelId);

                    EntityAdditionalInfoList entyAdditionalInfo = new EntityAdditionalInfoList(companyId, entitymodelId, employeeId);

                    if (!object.ReferenceEquals(attr, null))
                    {
                        for (int cnt = 0; cnt < attr.Count; cnt++)
                        {
                            entity.EntityAttributeModelList.Add(new EntityAttributeModel
                            {
                                AttributeModelId = attr[cnt].AttributeModelId,
                                AttributeModel = new AttributeModel(attr[cnt].AttributeModelId, companyId)

                            });

                        }
                        for (int cnt = 0; cnt < entyAdditionalInfo.Count; cnt++)
                        {

                            if (entity.EntityAttributeModelList.Where(u => u.AttributeModelId == attr[cnt].AttributeModelId).FirstOrDefault() != null)
                                entity.EntityAttributeModelList.Where(u => u.AttributeModelId == entyAdditionalInfo[cnt].AttributeModelId).FirstOrDefault().EntityAttributeValue.Value = entyAdditionalInfo[cnt].RefEntityId == Guid.Empty ? entyAdditionalInfo[cnt].Value : entyAdditionalInfo[cnt].RefEntityId.ToString();
                        }

                    }
                }
                else
                {
                    entity = new Entity();
                    entity.EntityModelId = entitymodelId;
                }
                return base.BuildJson(true, 200, "success", entity);
            }
            catch (Exception ex)
            {

                return base.BuildJson(false, 200, "Dynamic group formula Error " + ex.Message, entity);
            }
        }

        public JsonResult GetEntityMasterValue(Guid id, Guid entitymodelId, string refEntityId, string refEntityModel)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Entity entity;
            if (id != Guid.Empty)
            {
                entity = new Entity(entitymodelId, id);
            }
            else
            {
                entity = new Entity();
                entity.EntityModelId = entitymodelId;
            }
            var entytMasterValues = new EntityMasterValueList(new Guid(refEntityId), refEntityModel).Where(u => u.EntityModelId == entitymodelId).ToList();
            MonthlyInputList monthlyinputlist = new MonthlyInputList();
            monthlyinputlist = monthlyinputlist.emplastEntryMonthInput(id, entitymodelId, new Guid(refEntityId));
            // PayrollHistory payrollHistory = new PayrollHistory(companyId,new Guid(refEntityId), 0, 0);
            EntityMasterSettings masSettings = new EntityMasterSettings();
            var masterSettingsList = masSettings.entityMastersettingList(id);

            if (object.ReferenceEquals(entytMasterValues, null))
                entytMasterValues = new List<EntityMasterValue>();
            EntityBehaviorList data = new EntityBehaviorList(entity.Id, entity.EntityModelId);
            //Modified by AjithPanner on 07/11/2017
            for (int cnt = 0; cnt < data.Count; cnt++)
            {
                if (data[cnt].ArrearAttributeModelId != Guid.Empty && data[cnt].ValueType == 1)
                {
                    entity.EntityAttributeModelList.Remove(entity.EntityAttributeModelList.Where(x => x.AttributeModelId == data[cnt].ArrearAttributeModelId).FirstOrDefault());
                }
                if (data[cnt].ValueType != 1)
                {
                    var masterinput = masterSettingsList.Where(x => x.AttributeId == data[cnt].AttributeModelId).FirstOrDefault();
                    if (masterinput == null)
                    {
                        entity.EntityAttributeModelList.Remove(entity.EntityAttributeModelList.Where(x => x.AttributeModelId == data[cnt].AttributeModelId).FirstOrDefault());
                    }
                    else if (data[cnt].ValueType != 1 && data[cnt].ValueType != 2)// && entytMasterValues.Count==0)
                    {
                        entity.EntityAttributeModelList.Where(x => x.AttributeModelId == data[cnt].AttributeModelId).FirstOrDefault().EntityAttributeValue.Value = data[cnt].Percentage == null ? "100" : data[cnt].Percentage;
                    }
                }


            }
            //for (int cnt = 0; cnt < entytMasterValues.Count; cnt++)
            //{
            //    var t2 = entity.EntityAttributeModelList.Where(u => u.AttributeModelId == entytMasterValues[cnt].AttributeModelId).FirstOrDefault();
            //    entity.EntityAttributeModelList.Remove(t2);
            //}


            for (int cnt = 0; cnt < entytMasterValues.Count; cnt++)
            {
                if (entity.EntityAttributeModelList.Where(u => u.AttributeModelId == entytMasterValues[cnt].AttributeModelId).FirstOrDefault() != null)
                    entity.EntityAttributeModelList.Where(u => u.AttributeModelId == entytMasterValues[cnt].AttributeModelId).FirstOrDefault().EntityAttributeValue.Value = entytMasterValues[cnt].Value;
                //entity.EntityAttributeModelList.Remove(t2);
            }
            for (int cnt = 0; cnt < monthlyinputlist.Count; cnt++)
            {
                if (entity.EntityAttributeModelList.Where(u => u.AttributeModelId == monthlyinputlist[cnt].AttributeModelId).FirstOrDefault() != null)
                    entity.EntityAttributeModelList.Where(u => u.AttributeModelId == monthlyinputlist[cnt].AttributeModelId).FirstOrDefault().EntityAttributeValue.Value = monthlyinputlist[cnt].Value;

            }

            for (int cnt = 0; cnt < entity.EntityAttributeModelList.Count; cnt++)
            {
                var masterinput = masterSettingsList.Where(x => x.AttributeId == entity.EntityAttributeModelList[cnt].AttributeModelId).FirstOrDefault();
                if (masterinput == null)
                {
                    if (entity.EntityAttributeModelList[cnt].AttributeModel.IsDefault == true || entity.EntityAttributeModelList[cnt].AttributeModel.IsInstallment || entity.EntityAttributeModelList[cnt].AttributeModel.IsSetting == true)
                    {
                        entity.EntityAttributeModelList.Remove(entity.EntityAttributeModelList[cnt]);
                    }
                }
            }
            for (int cnt = 0; cnt < entity.EntityAttributeModelList.Count; cnt++)
            {
                var temp = data.Where(x => x.AttributeModelId == entity.EntityAttributeModelList[cnt].AttributeModelId).FirstOrDefault();
                if (temp != null)
                    entity.EntityAttributeModelList.Where(u => u.AttributeModelId == entity.EntityAttributeModelList[cnt].AttributeModelId).FirstOrDefault().ValueType = Convert.ToString(temp.ValueType);
                else
                    entity.EntityAttributeModelList.Where(u => u.AttributeModelId == entity.EntityAttributeModelList[cnt].AttributeModelId).FirstOrDefault().ValueType = "1";

                //   entity.EntityAttributeModelList.Remove(entity.EntityAttributeModelList.Where(x => x.ValueType == "2").FirstOrDefault());
            }

            return base.BuildJson(true, 200, "success", entity);
        }



        public JsonResult ChangeEntityMasterValue(Guid id, Guid entitymodelId, string refEntityId, string refEntityModel, Guid oldEntityId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Entity entity;
            if (id != Guid.Empty)
            {
                entity = new Entity(entitymodelId, id);
            }
            else
            {
                entity = new Entity();
                entity.EntityModelId = entitymodelId;
            }
            var entytMasterValues = new EntityMasterValueList(oldEntityId, new Guid(refEntityId), refEntityModel).Where(u => u.EntityModelId == entitymodelId).ToList();
            MonthlyInputList monthlyinputlist = new MonthlyInputList();
            monthlyinputlist = monthlyinputlist.emplastEntryMonthInput(id, entitymodelId, new Guid(refEntityId));
            // PayrollHistory payrollHistory = new PayrollHistory(companyId,new Guid(refEntityId), 0, 0);
            EntityMasterSettings masSettings = new EntityMasterSettings();
            var masterSettingsList = masSettings.entityMastersettingList(id);

            if (object.ReferenceEquals(entytMasterValues, null))
                entytMasterValues = new List<EntityMasterValue>();
            EntityBehaviorList data = new EntityBehaviorList(entity.Id, entity.EntityModelId);
            //Modified by AjithPanner on 07/11/2017
            for (int cnt = 0; cnt < data.Count; cnt++)
            {
                if (data[cnt].ArrearAttributeModelId != Guid.Empty && data[cnt].ValueType == 1)
                {
                    entity.EntityAttributeModelList.Remove(entity.EntityAttributeModelList.Where(x => x.AttributeModelId == data[cnt].ArrearAttributeModelId).FirstOrDefault());
                }
                if (data[cnt].ValueType != 1)
                {
                    var masterinput = masterSettingsList.Where(x => x.AttributeId == data[cnt].AttributeModelId).FirstOrDefault();
                    if (masterinput == null)
                    {
                        entity.EntityAttributeModelList.Remove(entity.EntityAttributeModelList.Where(x => x.AttributeModelId == data[cnt].AttributeModelId).FirstOrDefault());
                    }
                    else if (data[cnt].ValueType != 1 && data[cnt].ValueType != 2)// && entytMasterValues.Count==0)
                    {
                        entity.EntityAttributeModelList.Where(x => x.AttributeModelId == data[cnt].AttributeModelId).FirstOrDefault().EntityAttributeValue.Value = data[cnt].Percentage == null ? "100" : data[cnt].Percentage;
                    }
                }


            }
            //for (int cnt = 0; cnt < entytMasterValues.Count; cnt++)
            //{
            //    var t2 = entity.EntityAttributeModelList.Where(u => u.AttributeModelId == entytMasterValues[cnt].AttributeModelId).FirstOrDefault();
            //    entity.EntityAttributeModelList.Remove(t2);
            //}


            for (int cnt = 0; cnt < entytMasterValues.Count; cnt++)
            {
                if (entity.EntityAttributeModelList.Where(u => u.AttributeModelId == entytMasterValues[cnt].AttributeModelId).FirstOrDefault() != null)
                    entity.EntityAttributeModelList.Where(u => u.AttributeModelId == entytMasterValues[cnt].AttributeModelId).FirstOrDefault().EntityAttributeValue.Value = entytMasterValues[cnt].Value;
                //entity.EntityAttributeModelList.Remove(t2);
            }
            for (int cnt = 0; cnt < monthlyinputlist.Count; cnt++)
            {
                if (entity.EntityAttributeModelList.Where(u => u.AttributeModelId == monthlyinputlist[cnt].AttributeModelId).FirstOrDefault() != null)
                    entity.EntityAttributeModelList.Where(u => u.AttributeModelId == monthlyinputlist[cnt].AttributeModelId).FirstOrDefault().EntityAttributeValue.Value = monthlyinputlist[cnt].Value;

            }

            for (int cnt = 0; cnt < entity.EntityAttributeModelList.Count; cnt++)
            {
                var masterinput = masterSettingsList.Where(x => x.AttributeId == entity.EntityAttributeModelList[cnt].AttributeModelId).FirstOrDefault();
                if (masterinput == null)
                {
                    if (entity.EntityAttributeModelList[cnt].AttributeModel.IsDefault == true || entity.EntityAttributeModelList[cnt].AttributeModel.IsInstallment || entity.EntityAttributeModelList[cnt].AttributeModel.IsSetting == true)
                    {
                        entity.EntityAttributeModelList.Remove(entity.EntityAttributeModelList[cnt]);
                    }
                }
            }
            for (int cnt = 0; cnt < entity.EntityAttributeModelList.Count; cnt++)
            {
                var temp = data.Where(x => x.AttributeModelId == entity.EntityAttributeModelList[cnt].AttributeModelId).FirstOrDefault();
                if (temp != null)
                    entity.EntityAttributeModelList.Where(u => u.AttributeModelId == entity.EntityAttributeModelList[cnt].AttributeModelId).FirstOrDefault().ValueType = Convert.ToString(temp.ValueType);
                else
                    entity.EntityAttributeModelList.Where(u => u.AttributeModelId == entity.EntityAttributeModelList[cnt].AttributeModelId).FirstOrDefault().ValueType = "1";

                //   entity.EntityAttributeModelList.Remove(entity.EntityAttributeModelList.Where(x => x.ValueType == "2").FirstOrDefault());
            }

            return base.BuildJson(true, 200, "success", entity);
        }


        public JsonResult GetEntityTempData(string refEntityId, string financeyear)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            Employee emp1 = new Employee(new Guid(refEntityId));
            EntityTempDataList entTemp1 = new EntityTempDataList(new Guid(financeyear), new Guid(refEntityId), companyId);
            AttributeModelList attlist = new AttributeModelList(companyId);
            EntityMapping mapping = new EntityMapping("Employee", refEntityId, Guid.Empty);
            EntityBehaviorList Entbehav = new EntityBehaviorList(new Guid(mapping.EntityId), new Guid(mapping.EntityTableName));
            JsonEmpTempDataList data = new JsonEmpTempDataList();
            var attlist1 = attlist.Where(al => al.IsMandatory == false && al.IsInstallment == false && al.IsMonthlyInput == false && al.IsIncrement == true).ToList();
            var entytMasterValues = new EntityMasterValueList(new Guid(refEntityId), "Employee").Where(u => u.EntityModelId == new Guid(mapping.EntityTableName)).ToList();
            data.AttributeModels = new AttributeModelList();
            attlist1.ForEach(al =>
            {
                var eb1 = Entbehav.Where(eb => eb.AttributeModelId == al.Id).FirstOrDefault();
                if (!ReferenceEquals(eb1, null))
                {
                    if (eb1.ValueType == 1)
                    {
                        var id = al.Id;
                        data.AttributeModels.Add(attlist1.Where(al1 => al1.Id == id).FirstOrDefault());
                    }
                }
            });
            AttributeModel att1 = new AttributeModel();
            att1.Id = new Guid("00000000-0000-0000-0000-000000000000");
            att1.DisplayAs = "Basket Allowance";
            att1.BehaviorType = "Earning";
            att1.CompanyId = companyId;
            data.AttributeModels.Add(att1);
            data.EntityId = mapping.EntityId;
            data.EntityModelId = mapping.EntityTableName;
            data.entTemp = entTemp1;
            data.emp = emp1;
            data.entityMasterValues = new EntityMasterValueList();
            entytMasterValues.ForEach(em =>
            {
                data.entityMasterValues.Add(em);
            });

            return base.BuildJson(true, 200, "success", data);
        }

        public JsonResult SaveEntityTempMaster(jsonEntityMasterTemp[] dataValue)
        {
            if (!base.checkSession())
            {
                return base.BuildJson(true, 0, "Invalid user", null);
            }
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EntityTempDataList entTemp1 = new EntityTempDataList(dataValue[0].financeyearId, dataValue[0].EmployeeId, companyId);
            if (entTemp1.Count > 0)
            {
                EntityTempData Temp1 = new EntityTempData();
                Temp1.companyId = companyId;
                Temp1.EmployeeId = dataValue[0].EmployeeId;
                Temp1.financeyearId = dataValue[0].financeyearId;
                Temp1.Delete();

            }

            bool isSaved = true;

            for (int i = 0; i < dataValue.Length; i++)
            {
                EntityTempData masterTemp = new EntityTempData();
                masterTemp.companyId = Convert.ToInt32(Session["CompanyId"]);
                masterTemp.financeyearId = new Guid(Convert.ToString(dataValue[i].financeyearId));
                masterTemp.EmployeeId = new Guid(Convert.ToString(dataValue[i].EmployeeId));
                masterTemp.CompId = new Guid(Convert.ToString(dataValue[i].CompId));
                masterTemp.Value = Convert.ToDecimal(dataValue[i].Value);
                bool save = masterTemp.Save();
                if (save == false)
                {
                    isSaved = false;
                }
            }


            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }



        public JsonResult SaveEntityMap(jsonEntityMap[] dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            for (int cnt = 0; cnt < dataValue.Length; cnt++)
            {
                EntityMapping entityMap = new EntityMapping();
                entityMap.EntityId = dataValue[cnt].entityId.ToString();
                entityMap.EntityTableName = dataValue[cnt].entityModelId.ToString();
                entityMap.IsPhysicalEntity = false;
                entityMap.RefEntityId = dataValue[cnt].refEntityId.ToString();
                entityMap.RefEntityModelId = dataValue[cnt].refEntitymodelId.ToString();
                entityMap.Save();
            }
            bool isSaved = true;
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }

        public JsonResult SaveEntityAdditionalInfo(jsonEntityAddtionalInfo[] dataValue)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            for (int cnt = 0; cnt < dataValue.Length; cnt++)
            {
                EntityAdditionalInfo entityMap = new EntityAdditionalInfo();
                entityMap.EntityModelId = dataValue[cnt].entitymodelId;
                entityMap.Value = Convert.ToString(dataValue[cnt].value);
                entityMap.AttributeModelId = dataValue[cnt].attributeId;
                entityMap.RefEntityId = dataValue[cnt].refEntityId;
                entityMap.EmployeeId = dataValue[cnt].employeeId;
                entityMap.CompanyId = companyId;
                entityMap.Save();
            }
            bool isSaved = true;
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }
        public class JsonFlexiPay  
        {
            public string FlexPay { get; set; }
            public string basicPay { get; set; }
            public string CutOffDate { get; set; }
            public bool PayrollProc { get; set; }
            public string IsIncrement { get; set; }
            public bool IsFlexPay { get; set; }
            public bool IsbasicPay { get; set; }
        }

        public JsonResult GetMasterValue()//maddy
        {
            Guid refEntityId = new Guid(Convert.ToString(Session["EmployeeId"]));

            string Cutfdate = Convert.ToDateTime(Session["EntryDate"]).ToString("MM/dd/yyyy");
            EntityMasterValue entityValue = new EntityMasterValue();
            string flexi = "7b9591be-2c76-4c9a-a4c0-2f2b470ea180";
            string basic = "a01fc9a8-5842-4ae4-b62a-7808cfbe3e7e";
            Guid FlexiPay = new Guid(flexi.ToUpper());
            Guid BasicPay = new Guid(basic.ToUpper());
            DataTable FlexiPayval = entityValue.MasterFlexipay(FlexiPay, refEntityId);
            DataTable BasicPayval = entityValue.MasterBasicpay(BasicPay, refEntityId);
            DataTable Increment = entityValue.IncrementCheck(refEntityId);
            DataTable PoyrolHistor = entityValue.PayrollHistory(refEntityId);
            JsonFlexiPay jsonFlexi = new JsonFlexiPay();
            if (FlexiPayval.Rows.Count > 0)
            {
                jsonFlexi.FlexPay = FlexiPayval.Rows[0][6].ToString();
                if (BasicPayval.Rows.Count > 0)
                {
                    jsonFlexi.basicPay = BasicPayval.Rows[0][6].ToString();
                }
                if (PoyrolHistor.Rows.Count == 0)
                {
                    jsonFlexi.PayrollProc = true;
                }
                else
                {
                    jsonFlexi.PayrollProc = false;
                }
                if (Increment.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(Increment.Rows[0][0].ToString()))
                    {
                        var moth = Convert.ToDateTime(Increment.Rows[0][0].ToString()).Month;
                        var year = Convert.ToDateTime(Increment.Rows[0][0].ToString()).Year;
                        jsonFlexi.IsIncrement = moth + "-" + year;
                    }
                    else
                    {
                        jsonFlexi.IsIncrement = DateTime.Now.AddMonths(-1).ToString();
                    }
                }
                jsonFlexi.CutOffDate = Cutfdate;
                return base.BuildJson(true, 200, "Loading ..", jsonFlexi);
            }
            return base.BuildJson(false, 200, "", string.Empty);
        }

        public  JsonResult CreateFlexiPayTable()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Guid EmployeeGuid = new Guid(Convert.ToString(Session["EmployeeGUID"]));
            AttributeModelType attributes = new AttributeModelType();
            List<AttributeModelType> Datavalue = attributes.Flexipay(companyId, Guid.Empty);
            return base.BuildJson(true, 100, "", Datavalue);
        }
        public JsonResult GetFlexiPayData()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Guid refEntityId = new Guid(Convert.ToString(Session["EmployeeGUID"]));
            AttributeModelType attributes = new AttributeModelType();
            List<AttributeModelType> Datavalue = attributes.Flexipay(companyId, Guid.Empty);
            List<AttributeModelType> filteredData = Datavalue
           .Where(type => type.IsFlexiPay == true || type.IsBasicPay == true)
           .ToList();
            EntityMasterValueList Result = new EntityMasterValueList(Guid.Empty,"Employee");
            var result = Result.Where( R=> filteredData.Any(type => R.AttributeModelId == type.MasterCompentId && R.RefEntityId == refEntityId)).ToList();

            //List<object> list = result.Concat(filteredData).Tolist();


            JsonFlexiPay jsonFlexi = new JsonFlexiPay();

            //var result = Result.Where(R => R.RefEntityId == filteredData.MasterCompentId).ToList();
            if (filteredData.Count > 0)
            {
                for (int i = 0; i < filteredData.Count; i++)
                {
                    if (result[i].Id == filteredData[i].MasterCompentId ) { 

                    }
                }

            }
            return base.BuildJson(true, 200, "success", filteredData);
        }
        public JsonResult LoadFlexiPay(List<string> Component)
        {

            Guid refEntityId = new Guid(Convert.ToString(Session["EmployeeId"]));
            EntityMasterValueList EmvList = new EntityMasterValueList(refEntityId, "Employee");
            List<EntityMasterValue> resultList = new List<EntityMasterValue>();
            foreach (string componentValue in Component)
            {
                var result = EmvList.Where(x => x.AttributeModelId.ToString().ToUpper() == componentValue.ToUpper()).FirstOrDefault();
                resultList.Add(result);
            }
            if (resultList.Count > 0)
            {
                return base.BuildJson(true, 100, "Load Success Full", resultList);
            }
            else
            {
                return base.BuildJson(false, 100, "No data found", "");
            }
        }

        //maddy
        //public JsonResult SaveIncrementDate(DateTime IncrementDate,string Emplid)
        //{
        //    if (!base.checkSession())
        //        return base.BuildJson(true, 0, "Invalid user", null);


        //    return base.BuildJson(true, 200, "Data saved successfully", null);
        //}
        //public JsonResult SelectIncrementDate(  string Emplid)
        //{
        //    if (!base.checkSession())
        //        return base.BuildJson(true, 0, "Invalid user", null);


        //    return base.BuildJson(true, 200, "Data saved successfully", null);
        //}
        public JsonResult SaveEntityFlexValue(jsonEntity dataValue, string refEntityIdVal, string refEntityModel, string saveMappingId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            string refEntityId = Session["EmployeeId"].ToString();
            jsonEntityKeyValue kV = new jsonEntityKeyValue();
            var entitMastervalues = new EntityMasterValueList(new Guid(refEntityId), refEntityModel).ToList();
            if (object.ReferenceEquals(entitMastervalues, null))
                entitMastervalues = new List<EntityMasterValue>();
            // entitMastervalues = entitMastervalues.Where(u => u.EntityModelId == dataValue.entityModelId).ToList();
            bool isSaved = false;
            if (dataValue.EntityKeyValues.Count > 0)
            {


                foreach (var t1 in dataValue.EntityKeyValues)
                {
                    if (t1.id == "Name")
                        continue;
                    EntityMasterValue tmp = entitMastervalues != null ? entitMastervalues.Where(u => u.AttributeModelId == new Guid(t1.id)).FirstOrDefault() : null;
                    if (object.ReferenceEquals(tmp, null))
                    {
                        EntityMasterValue entTemp = new EntityMasterValue()
                        {
                            AttributeModelId = new Guid(t1.id),
                            EntityModelId = dataValue.entityModelId,
                            EntityId = dataValue.entityId,
                            RefEntityId = new Guid(refEntityId),
                            RefEntityModelId = refEntityModel,
                            Value = t1.value
                        };
                        isSaved = entTemp.SaveFlaxe();

                    }
                    else
                    {
                        if (tmp.Value != t1.value || tmp.EntityId != dataValue.entityId)
                        {
                            tmp.Value = t1.value;
                            tmp.EntityId = dataValue.entityId;
                            isSaved = tmp.SaveFlaxe();
                        }
                    }
                }
            }
            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }
        }


        public JsonResult SaveEntityMasterValue(jsonEntity dataValue, string refEntityId, string refEntityModel, string saveMappingId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            var entitMastervalues = new EntityMasterValueList(new Guid(refEntityId), refEntityModel).Where(u => u.EntityModelId == dataValue.entityModelId).ToList();
            if (object.ReferenceEquals(entitMastervalues, null))
                entitMastervalues = new List<EntityMasterValue>();
            // entitMastervalues = entitMastervalues.Where(u => u.EntityModelId == dataValue.entityModelId).ToList();
            EntityBehaviorList entityBehave = new EntityBehaviorList(dataValue.entityId, dataValue.entityModelId);
            bool isSaved = true;
            if (entityBehave.Count <= 0)
            {
                foreach (var t1 in dataValue.EntityKeyValues)
                {
                    if (t1.id == "Name")
                        continue;
                    EntityMasterValue tmp = entitMastervalues != null ? entitMastervalues.Where(u => u.AttributeModelId == new Guid(t1.id)).FirstOrDefault() : null;
                    if (object.ReferenceEquals(tmp, null))
                    {
                        EntityMasterValue entTemp = new EntityMasterValue()
                        {
                            AttributeModelId = new Guid(t1.id),
                            EntityModelId = dataValue.entityModelId,
                            EntityId = dataValue.entityId,
                            RefEntityId = new Guid(refEntityId),
                            RefEntityModelId = refEntityModel,
                            Value = t1.value
                        };
                        isSaved = entTemp.Save();

                    }
                    else
                    {
                        if (tmp.Value != t1.value || tmp.EntityId != dataValue.entityId)
                        {
                            tmp.Value = t1.value;
                            tmp.EntityId = dataValue.entityId;
                            isSaved = tmp.Save();
                        }
                    }
                }
            }
            else
            {
                foreach (var temp in entityBehave)
                {
                    if (temp.ValueType == 1)
                    {
                        foreach (var t1 in dataValue.EntityKeyValues)
                        {
                            if (t1.id == "Name")
                                continue;
                            EntityMasterValue tmp = entitMastervalues != null ? entitMastervalues.Where(u => u.AttributeModelId == new Guid(t1.id)).FirstOrDefault() : null;
                            if (object.ReferenceEquals(tmp, null))
                            {
                                EntityMasterValue entTemp = new EntityMasterValue()
                                {
                                    AttributeModelId = new Guid(t1.id),
                                    EntityModelId = dataValue.entityModelId,
                                    EntityId = dataValue.entityId,
                                    RefEntityId = new Guid(refEntityId),
                                    RefEntityModelId = refEntityModel,
                                    Value = t1.value
                                };
                                isSaved = entTemp.Save();

                            }
                            else
                            {
                                if (tmp.Value != t1.value || tmp.EntityId != dataValue.entityId)
                                {
                                    tmp.Value = t1.value;
                                    tmp.EntityId = dataValue.entityId;
                                    isSaved = tmp.Save();
                                }
                            }
                        }
                    }
                }
            }

            if (saveMappingId == "true")
            {
                EntityMapping entityMap = new EntityMapping();
                entityMap.EntityId = dataValue.entityId.ToString();
                entityMap.EntityTableName = dataValue.entityModelId.ToString();
                entityMap.IsPhysicalEntity = false;
                entityMap.RefEntityId = refEntityId.ToString();
                entityMap.RefEntityModelId = refEntityModel.ToString();
                entityMap.Save();
            }

            if (isSaved)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }

        }

        public JsonResult GetLockLoad(int month, int year)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            LockSetting selectVal = new LockSetting(month, year, companyId, "Select");
            if (selectVal.PaySheetLockid == Guid.Empty)
            {
                return base.BuildJson(false, 200, "success", null);
            }
            else
            {
                return base.BuildJson(true, 200, "success", selectVal);
            }
        }

        public JsonResult PreviousComponentsSettingSave(List<previousComponents> things, Guid entityId, Guid categoryId, Guid entitymodelId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Entity ent = new Entity(entityId, Guid.Empty);
            things.ForEach(f =>
            {
                new MonthlyInput().SaveSetting(f, entityId, categoryId, entitymodelId);
            });
            return base.BuildJson(true, 200, "saved successfully ", null);
        }
        public JsonResult GetPreviousComponents(Guid? entitymodelId, Guid entityId, Guid categoryId, int month, int year, Guid employeeId = new Guid())
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);


            previousComponentslist things = new previousComponentslist(categoryId, entityId, new Guid(entitymodelId.ToString()));



            EntityAttributeModelList EnttModelList = new EntityAttributeModelList(entitymodelId.Value, entityId);

            EntityBehaviorList entityBehavi = new EntityBehaviorList(entityId, entitymodelId.Value);

            List<EntityBehavior> seletedEntityBehav = entityBehavi.Where(u => u.ValueType == 2).ToList();


            seletedEntityBehav.ToList().ForEach(s =>
            {
                if (EnttModelList.Where(x => x.AttributeModelId == s.AttributeModelId).FirstOrDefault() == null)
                    seletedEntityBehav.Remove(seletedEntityBehav.Where(x => x.AttributeModelId == s.AttributeModelId).FirstOrDefault());
            });
            List<newComponents> newComp = new List<newComponents>();
            entityBehavi.Where(w => w.ValueType != 1 && w.ValueType != 2).ToList().ForEach(f =>
                {
                    newComp.Add(new newComponents
                    {
                        Id = f.AttributeModelId,
                        Name = new AttributeModel(f.AttributeModelId, companyId).Name

                    });
                });
            List<previousComponents> list = new List<previousComponents>();
            seletedEntityBehav.ForEach(f =>
            {
                list.Add(new previousComponents()
                {
                    Id = f.AttributeModelId,
                    Name = new AttributeModel(f.AttributeModelId, companyId).Name,
                    MappedColumn = new AttributeModel(f.AttributeModelId, companyId).Name,
                    attr = newComp

                });
            });

            MonthlyInputList monthlyinputlist = new MonthlyInputList(entityId, categoryId, "");
            string past = "";
            if (monthlyinputlist.Count > 0)
            {
                string lastmonth = Convert.ToString((MonthEnum)monthlyinputlist.FirstOrDefault().Month);
                int lastyear = monthlyinputlist.FirstOrDefault().Year;
                past = lastmonth + "," + lastyear;
            }

            if (things.Count > 0)
            {
                things.ForEach(f =>
                {
                    f.attr = newComp;
                });
                return base.BuildJson(true, 200, "", things);
            }


            return base.BuildJson(true, 200, past, list);
        }
        //new code

        public JsonResult GetMonthlyInput(Guid? entitymodelId, Guid entityId, Guid categoryId, int month, int year, Guid employeeId = new Guid())
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            //

            List<Employee> seletedEmpl = new List<Employee>();
            // int seletedEmpl=0;
            //  var isEmployee = "";
            if (entityId == Guid.Empty && entitymodelId == Guid.Empty)
            {
                EntityModel entModel = new EntityModel(ComValue.SalaryTable, companyId);
                EntityMapping entityMapping = new EntityMapping(ComValue.EmployeeTable, Convert.ToString(employeeId), entModel.Id);
                entitymodelId = entModel.Id;
                entityId = new Guid(entityMapping.EntityId);
            }
            EntityAttributeModelList EntList = new EntityAttributeModelList(entitymodelId.Value);
            EntityAttributeValueList entityAttributeValues = new EntityAttributeValueList(entitymodelId.Value);
            EntityMappingList entitMapp = new EntityMappingList(entityId);
            List<jsonMonthlyInput> entity = new List<jsonMonthlyInput>();

            //Commented in order to resolve the MI data binding incase of Dynamic grp changes.
            //MonthlyInputList monthlyinputlist = new MonthlyInputList(entityId, employeeId, month, year);

            MonthlyInputList monthlyinputlist = new MonthlyInputList(Guid.Empty, employeeId, month, year);
            PayrollHistoryList payrollHistorylist = new PayrollHistoryList(companyId, year, month);
            payrollHistorylist.RemoveAll(p => p.Status != "Processed");

            //New Code Started
            entitMapp.ForEach(o =>
            {
                var temp = payrollHistorylist.Where(u => u.EmployeeId == new Guid(o.RefEntityId) && u.IsDeleted == false && u.Status == "Processed").FirstOrDefault();
                if (!object.ReferenceEquals(temp, null))
                {
                    o.EntityId = temp.EntityId.ToString();
                }
            });

            //New Code End

            //testing
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            AttributeModelList attributemodellist = new AttributeModelList(companyId);
            AttributeModel AttrModel = new AttributeModel();
            AttrModel = attributemodellist.Where(u => u.Name == "LD").FirstOrDefault();
            LeaveRequestList GetLeaveList = new LeaveRequestList(DefaultFinancialYr.Id, companyId, month, year);
            EntityBehaviorList entityBehavior = new EntityBehaviorList(entityId, entitymodelId.Value);
            DefaultLOPid lossofpayid = new DefaultLOPid(companyId);



            if (entitMapp.Count > 0)
            {
                if (entitMapp[0].RefEntityModelId == ComValue.EmployeeTable)//"Employee"
                {
                    EmployeeList employee;
                    if (employeeId == Guid.Empty)
                    {
                        employee = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
                    }
                    else
                    {
                        employee = new EmployeeList(companyId, userId, employeeId, 0, 0);
                    }



                    //  var isEmployee;
                    DateTime CurrPayrollmonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    if (employeeId == Guid.Empty)
                    {
                        seletedEmpl = employee.Where(u => u.DateOfJoining <= CurrPayrollmonth && u.CategoryId == categoryId && u.SeparationDate == DateTime.MinValue).ToList();
                    }
                    else
                    {
                        seletedEmpl = employee.Where(u => u.DateOfJoining <= CurrPayrollmonth && u.CategoryId == categoryId).ToList();
                    }


                    entitMapp.ForEach(u =>
                    {
                        // EntityAttributeModelList EnttModelList = new EntityAttributeModelList(entitymodelId.Value, new Guid(u.EntityId));
                        var EnttModelList = EntList.Where(el => el.EntityModelId == entitymodelId.Value);
                        EntityAttributeModel EntityList = new EntityAttributeModel();
                        EntityList = EnttModelList.Where(s => s.AttributeModelId == AttrModel.Id).FirstOrDefault();
                        //EntityAttributeValueList EnttAttValueList = new EntityAttributeValueList(EntityList.EntityModelId);
                        var EnttAttValueList = entityAttributeValues.Where(ea => ea.EntityModelId == EntityList.EntityModelId).ToList();
                        EntityAttributeValue EntityValue = new EntityAttributeValue();
                        EntityValue = EnttAttValueList.Where(q => q.EntityAttributeModelId == EntityList.Id).FirstOrDefault();
                        var LdaysValue = EntityValue.Value;

                        // EntityBehaviorList entityBehavior = new EntityBehaviorList(new Guid(u.EntityId), entitymodelId.Value);
                        List<EntityBehavior> seletedEntityBehavir = entityBehavior.Where(w => w.ValueType == 2).ToList();


                        // Remove UnMapped component from monthly component type. Modified on 04/11/2018 By Muthu
                        seletedEntityBehavir.ToList().ForEach(s =>
                        {
                            if (EnttModelList.Where(x => x.AttributeModelId == s.AttributeModelId).FirstOrDefault() == null)
                                seletedEntityBehavir.Remove(seletedEntityBehavir.Where(x => x.AttributeModelId == s.AttributeModelId).FirstOrDefault());
                        });
                        var isEmployee = seletedEmpl.Where(t => t.Id == new Guid(u.RefEntityId)).ToList();
                        if (isEmployee.Count > 0)
                        {
                            //var LDays = GetLeaveList.Where(k => k.EmployeeId == new Guid(u.RefEntityId)).ToList().Count;
                            //added now
                            var Tempp = GetLeaveList.Where(k => k.EmployeeId == new Guid(u.RefEntityId) && k.LeaveType == new Guid(lossofpayid.LOPId.ToString())).ToList();//"199f5db2-14b7-46d3-a0e4-715d56682277"
                            var LDays = 0.0;
                            for (int i = 0; i < Tempp.Count; i++)
                            {

                                if (Tempp[i].LeaveType == new Guid(lossofpayid.LOPId.ToString()) && Tempp[i].EmployeeId == new Guid(u.RefEntityId))//"199f5db2-14b7-46d3-a0e4-715d56682277"
                                {
                                    LDays = Convert.ToDouble(LDays) + Convert.ToDouble(Tempp[i].HFDay);
                                }
                            }

                            //added end


                            var payHist = payrollHistorylist.Where(s => s.EmployeeId == new Guid(u.RefEntityId) && s.Month == month && s.Year == year).FirstOrDefault();
                            string strStatus = "CanEdit";
                            if (!object.ReferenceEquals(payHist, null) && (payHist.Status == ComValue.payrollProcessStatus[0] || payHist.Status == ComValue.payrollProcessStatus[1]))
                                strStatus = "CanNotEdit";
                            jsonMonthlyInput temp = new jsonMonthlyInput();
                            temp.EmployeeId = new Guid(u.RefEntityId);
                            temp.EmployeeCode = seletedEmpl.Where(t => t.Id == new Guid(u.RefEntityId)).FirstOrDefault().EmployeeCode;//employee.Where(t => t.Id == new Guid(u.RefEntityId)).FirstOrDefault().FirstName;
                            temp.EmployeeName = seletedEmpl.Where(t => t.Id == new Guid(u.RefEntityId)).FirstOrDefault().FirstName + " " + seletedEmpl.Where(t => t.Id == new Guid(u.RefEntityId)).FirstOrDefault().LastName;//employee.Where(t => t.Id == new Guid(u.RefEntityId)).FirstOrDefault().FirstName;
                            temp.Month = month;
                            temp.year = year;
                            temp.Id = new Guid(u.EntityId);
                            temp.editStatus = strStatus;
                            temp.MonthlyInputAttributes = new List<MonthlyInputAttribute>();
                            seletedEntityBehavir.ForEach(p =>
                            {
                                var mon = monthlyinputlist.Where(s => s.AttributeModelId == p.AttributeModelId && s.Year == year && s.Month == month && s.EmployeeId == temp.EmployeeId && s.EntityId == new Guid(u.EntityId)).FirstOrDefault();
                                string miValue = mon != null ? mon.Value : "0";


                                if (p.AttributeModelId == AttrModel.Id)
                                {
                                    AttributeModel AttrMod = new AttributeModel();
                                    AttrMod = attributemodellist.Where(s => s.Name == "MD").FirstOrDefault();
                                    MonthlyInput MI = new MonthlyInput();
                                    var MInput = monthlyinputlist.Where(k => k.AttributeModelId == AttrMod.Id).FirstOrDefault();
                                    string MIValue = MInput != null ? MInput.Value : "0";
                                    // var Mdays = MI.Value != null ? MI.Value : "0";
                                    if (LdaysValue == "0" || LdaysValue == "")
                                    {
                                        //var TotalMdays = Convert.ToDecimal(miValue) + Convert.ToDecimal(LDays);--existing
                                        //change added
                                        double TotalMdays = 0;
                                        if (Convert.ToDecimal(miValue) == 0)
                                        {
                                            TotalMdays = Convert.ToDouble(LDays);
                                        }
                                        else
                                        {
                                            TotalMdays = Convert.ToDouble(miValue);
                                            //+ Convert.ToDouble(LDays);
                                        }
                                        //change end
                                        temp.MonthlyInputAttributes.Add(new MonthlyInputAttribute()
                                        {
                                            AttributeModId = p.AttributeModelId,
                                            Name = attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().DisplayAs,
                                            AttributeBehaviorType = (attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "PD" || attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "LD" || attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "MD") ? "others" : attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().BehaviorType.ToLower(),
                                            //MIValue = miValue + LDays
                                            MIValue = Convert.ToString(TotalMdays),
                                            MIDisplay = attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name,
                                        });
                                    }
                                    else
                                    {

                                        var TotalMdays = Convert.ToInt32(MIValue) + Convert.ToInt32(LDays);
                                        temp.MonthlyInputAttributes.Add(new MonthlyInputAttribute()
                                        {
                                            AttributeModId = p.AttributeModelId,
                                            Name = attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().DisplayAs,
                                            AttributeBehaviorType = (attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "PD" || attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "LD" || attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "MD") ? "others" : attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().BehaviorType.ToLower(),
                                            MIValue = Convert.ToString(TotalMdays),
                                            MIDisplay = attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name
                                        });
                                    }

                                }
                                else
                                {
                                    temp.MonthlyInputAttributes.Add(new MonthlyInputAttribute()
                                    {

                                        AttributeModId = p.AttributeModelId,
                                        Name = attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().DisplayAs,
                                        AttributeBehaviorType = (attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "PD" || attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "LD" || attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "MD") ? "others" : attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().BehaviorType.ToLower(),
                                        MIValue = miValue,
                                        MIDisplay = attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name

                                    });
                                }
                            });
                            entity.Add(temp);
                        }
                    });
                }

            }

            entity.Where(w => w.editStatus == "CanEdit").ToList().ForEach(f =>
            {
                // PayrollHistoryList paylist = new PayrollHistoryList(f.EmployeeId, companyId);
                var paylist = payrollHistorylist.Where(ph => ph.EmployeeId == f.EmployeeId && ph.CompanyId == companyId).ToList();

                // paylist.RemoveAll(p => p.Status != "Processed");

                var payhist = paylist.FirstOrDefault();

                if (!object.ReferenceEquals(payhist, null))
                {
                    // MonthlyInputList mon = new MonthlyInputList(f.Id, f.EmployeeId, f.Month, f.year);
                    var mon = monthlyinputlist.Where(mi => mi.Id == f.Id && mi.EmployeeId == f.EmployeeId && mi.Month == f.Month && mi.Year == f.year).ToList();
                    if (mon.Count == 0)
                    {
                        savemonthlycarryforward(companyId, entitymodelId, entityId, categoryId, payhist.Month, payhist.Year, month, year, f.EmployeeId, payrollHistorylist);
                    }

                }

            });
            List<jsonMonthlyInput> result = new List<jsonMonthlyInput>();

            result = GetMonthlyInputvalues(DefaultFinancialYr, GetLeaveList, lossofpayid, attributemodellist, entitymodelId, entityId, categoryId, month, year, payrollHistorylist, employeeId);

            entity.ForEach(f =>
            {
                previousComponentslist things = new previousComponentslist(categoryId, f.Id, new Guid(entitymodelId.ToString()));
                things.Where(w => w.MappedId != Guid.Empty).ToList().ForEach(t =>
                {
                    if (t.radio == "hide" || t.radio == "sne")
                    {
                        if (t.radio == "hide")
                        {
                            f.MonthlyInputAttributes.RemoveAll(r => r.AttributeModId == t.Id);
                        }

                        if (t.radio == "sne")
                        {
                            f.MonthlyInputAttributes.Where(r => r.AttributeModId == t.Id).ToList().ForEach(j => j.status = "sne");
                        }
                    }
                });

            });


            return base.BuildJson(true, 200, "success", result);
        }

        //end of new code

        public JsonResult GetMasterInput(Guid entitymodelId, Guid entityId, Guid categoryId, Guid employeeId = new Guid())
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            EntityMappingList entitMapp = new EntityMappingList(entityId);
            List<jsonMonthlyInput> entity = new List<jsonMonthlyInput>();
            EntityMasterSettings masSettings = new EntityMasterSettings();
            var masterSettingsList = masSettings.entityMastersettingList(entityId);

            Entity entitylist;
            if (entityId != Guid.Empty)
            {
                entitylist = new Entity((entitymodelId), entityId);
            }
            else
            {
                entitylist = new Entity();
                entitylist.EntityModelId = entitymodelId;
            }
            EntityBehaviorList data = new EntityBehaviorList(entitylist.Id, entitylist.EntityModelId);
            if (entitMapp.Count > 0)
            {
                if (entitMapp[0].RefEntityModelId == ComValue.EmployeeTable)//"Employee"
                {
                    EmployeeList employee;
                    if (employeeId == Guid.Empty)
                    {
                        employee = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
                    }
                    else
                    {
                        employee = new EmployeeList(companyId, userId, employeeId, 0, 0);
                    }
                    if (categoryId != Guid.Empty)
                    {
                        employee.RemoveAll(x => x.CategoryId != categoryId);
                    }

                    EntityBehaviorList entityBehavior = new EntityBehaviorList(entityId, entitymodelId);
                    List<EntityBehavior> seletedEntityBehavir = entityBehavior.Where(u => u.ValueType == 1).ToList();
                    AttributeModelList attributemodellist = new AttributeModelList(companyId);
                    // seletedEntityBehavir.RemoveAll(x => x.ArrearAttributeModelId == Guid.Empty);
                    var ESI = attributemodellist.Where(x => x.DisplayAs == "ESI" || x.Name == "ESI").FirstOrDefault();
                    var PF = attributemodellist.Where(x => x.DisplayAs == "PF" || x.Name == "PF").FirstOrDefault();
                    EntityBehavior ESIData = new EntityBehavior(ESI.Id, entitymodelId, entityId);
                    EntityBehavior PFData = new EntityBehavior(PF.Id, entitymodelId, entityId);
                    for (int cnt = 0; cnt < data.Count; cnt++)
                    {
                        if (data[cnt].ArrearAttributeModelId != Guid.Empty && data[cnt].ValueType == 1)
                        {
                            entitylist.EntityAttributeModelList.Remove(entitylist.EntityAttributeModelList.Where(x => x.AttributeModelId == data[cnt].ArrearAttributeModelId).FirstOrDefault());
                        }

                        if (data[cnt].ValueType != 1)
                        {
                            var masterinput = masterSettingsList.Where(x => x.AttributeId == data[cnt].AttributeModelId).FirstOrDefault();
                            if (masterinput == null || (masterinput != null && data[cnt].ValueType == 2))
                            {
                                entitylist.EntityAttributeModelList.Remove(entitylist.EntityAttributeModelList.Where(x => x.AttributeModelId == data[cnt].AttributeModelId).FirstOrDefault());
                            }

                        }
                    }


                    for (int cnt = 0; cnt < entitylist.EntityAttributeModelList.Count; cnt++)
                    {
                        var masterinput = masterSettingsList.Where(x => x.AttributeId == entitylist.EntityAttributeModelList[cnt].AttributeModelId).FirstOrDefault();
                        if (masterinput == null)
                        {
                            if (entitylist.EntityAttributeModelList[cnt].AttributeModel.IsDefault == true || entitylist.EntityAttributeModelList[cnt].AttributeModel.IsInstallment == true || entitylist.EntityAttributeModelList[cnt].AttributeModel.IsSetting == true)
                            {
                                entitylist.EntityAttributeModelList.Remove(entitylist.EntityAttributeModelList.Where(x => x.AttributeModelId == entitylist.EntityAttributeModelList[cnt].AttributeModelId).FirstOrDefault());
                            }
                        }
                    }
                    // entitylist.EntityAttributeModelList.RemoveAll(entitylist.EntityAttributeModelList.Where(x=>x.AttributeModelId == seletedEntityBehavir.Where(y => y.ArrearAttributeModelId == Guid.Empty).ToList()));
                    //Get Additional Info model Id
                    EntityModel entityModel = new EntityModel("AdditionalInfo", companyId);

                    entitMapp.ForEach(u =>
                    {
                        var isEmployee = employee.Where(t => t.Id == new Guid(u.RefEntityId)).ToList();

                        if (isEmployee.Count > 0)
                        {
                            var entytMasterValues = new EntityMasterValueList(new Guid(u.RefEntityId), u.RefEntityModelId).Where(x => x.EntityModelId == entitymodelId).ToList();

                            EntityAdditionalInfoList entyAdditionalInfo = new EntityAdditionalInfoList(companyId, entityModel.Id, new Guid(u.RefEntityId));
                            if (object.ReferenceEquals(entytMasterValues, null))
                                entytMasterValues = new List<EntityMasterValue>();


                            //for (int cnt = 0; cnt < entyAdditionalInfo.Count; cnt++)
                            //{

                            //    if (entitylist.EntityAttributeModelList.Where(x => x.AttributeModelId == entyAdditionalInfo[cnt].AttributeModelId).FirstOrDefault() != null)
                            //        entitylist.EntityAttributeModelList.Where(x => x.AttributeModelId == entyAdditionalInfo[cnt].AttributeModelId).FirstOrDefault().EntityAttributeValue.Value = entyAdditionalInfo[cnt].RefEntityId == Guid.Empty ? 
                            //        entyAdditionalInfo[cnt].Value : entyAdditionalInfo[cnt].RefEntityId.ToString();
                            //}
                            string strStatus = "CanEdit";

                            jsonMonthlyInput temp = new jsonMonthlyInput();
                            temp.EmployeeId = new Guid(u.RefEntityId);
                            temp.EmployeeCode = employee.Where(t => t.Id == new Guid(u.RefEntityId)).FirstOrDefault().EmployeeCode;//employee.Where(t => t.Id == new Guid(u.RefEntityId)).FirstOrDefault().FirstName;
                            temp.EmployeeName = employee.Where(t => t.Id == new Guid(u.RefEntityId)).FirstOrDefault().FirstName + " " + employee.Where(t => t.Id == new Guid(u.RefEntityId)).FirstOrDefault().LastName;//employee.Where(t => t.Id == new Guid(u.RefEntityId)).FirstOrDefault().FirstName;
                                                                                                                                                                                                                       // temp.Month = month;
                                                                                                                                                                                                                       //temp.year = year;
                            temp.Id = entityId;
                            temp.editStatus = strStatus;
                            temp.MonthlyInputAttributes = new List<MonthlyInputAttribute>();
                            entitylist.EntityAttributeModelList.ForEach(p =>
                            {
                                var masterval = entytMasterValues.Where(x => x.AttributeModelId == p.AttributeModelId).FirstOrDefault();
                                var behaviourvalPercentage = data.Where(x => x.AttributeModelId == p.AttributeModelId && x.ValueType != 1).FirstOrDefault();
                                string tempMIValue = masterval == null ? behaviourvalPercentage == null ? "0" : behaviourvalPercentage.Percentage == null ? "100" : behaviourvalPercentage.Percentage : masterval.Value == null ? "0" : masterval.Value;
                                tempMIValue = tempMIValue == "null" ? "100" : tempMIValue;

                                temp.MonthlyInputAttributes.Add(new MonthlyInputAttribute()
                                {
                                    AttributeModId = p.AttributeModelId,
                                    Name = attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().DisplayAs,
                                    AttributeBehaviorType = (attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "PD" || attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "LD" || attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "MD") ? "others" : attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().BehaviorType.ToLower(),
                                    MIValue = tempMIValue,// entytMasterValues.Where(x => x.AttributeModelId == p.AttributeModelId).FirstOrDefault() == null ? "0" : (entytMasterValues.Where(x => x.AttributeModelId == p.AttributeModelId).FirstOrDefault().Value == null ? "0" : entytMasterValues.Where(x => x.AttributeModelId == p.AttributeModelId).FirstOrDefault().Value),
                                });
                            });
                            //need to add ESI Percentage
                            //temp.MonthlyInputAttributes.Add(new MonthlyInputAttribute()
                            //{
                            //    AttributeModId = ESI.Id,
                            //    Name = ESI.Name,
                            //    AttributeBehaviorType = ESI.BehaviorType,
                            //    MIValue = entyAdditionalInfo.Count == 0 ? ESIData.Percentage : entyAdditionalInfo.Where(x => x.AttributeModelId == ESI.Id).FirstOrDefault() == null ? ESIData.Percentage : entyAdditionalInfo.Where(x => x.AttributeModelId == ESI.Id).FirstOrDefault().Value
                            //});
                            //////PF Percentage
                            //temp.MonthlyInputAttributes.Add(new MonthlyInputAttribute()
                            //{
                            //    AttributeModId = PF.Id,
                            //    Name = PF.Name,
                            //    AttributeBehaviorType = PF.BehaviorType,
                            //    MIValue = entyAdditionalInfo.Count == 0 ? PFData.Percentage : entyAdditionalInfo.Where(x => x.AttributeModelId == PF.Id).FirstOrDefault() == null ? PFData.Percentage : entyAdditionalInfo.Where(x => x.AttributeModelId == PF.Id).FirstOrDefault().Value
                            //});

                            entity.Add(temp);


                        }
                    });
                }

            }
            return base.BuildJson(true, 200, "success", entity);
        }



        public JsonResult GetEmpEntity(Guid entityId, Guid entitymodelId, Guid employeeId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            // MonthlyInputList empMonthlyInput = new MonthlyInputList(entityId, entitymodelId, employeeId);
            MonthlyInput monthlyInput = new MonthlyInput();
            //var empEntityId = empMonthlyInput.Where(emi => emi.EmployeeId == employeeId).FirstOrDefault();
            //if (empEntityId != null)
            //{
            //    entityId = empEntityId.EntityId;
            //    entitymodelId = empEntityId.EntityModelId;
            //    monthlyInput.EntityId = entityId;
            //    monthlyInput.EntityModelId = entitymodelId;
            //}
            try
            {
                EntityModel entModel = new EntityModel(ComValue.SalaryTable, Convert.ToInt32(Session["CompanyId"]));
                EntityMapping entityMapping = new EntityMapping(ComValue.EmployeeTable, Convert.ToString(employeeId), entModel.Id);
                entitymodelId = entModel.Id;
                entityId = new Guid(entityMapping.EntityId);
                monthlyInput.EntityId = entityId;
                monthlyInput.EntityModelId = entitymodelId;


                return base.BuildJson(true, 200, "success", monthlyInput);

            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 200, "Failed", monthlyInput);
            }
        }
        public JsonResult GetMonthlyEntityList()
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            TableCategoryList cat = new TableCategoryList(companyId);
            EntityModelList models = new EntityModelList(cat.Where(u => u.Name == "Payroll").FirstOrDefault().Id);
            EntityList entityList = new EntityList(models.Where(u => u.Name == "Salary").First().Id);
            return base.BuildJson(true, 200, "success", entityList);
        }


        public JsonResult GetMasEarnDedCompSettingList(Guid id)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            TableCategoryList cat = new TableCategoryList(companyId);
            EntityModelList models = new EntityModelList(cat.Where(u => u.Name == "Payroll").FirstOrDefault().Id);
            EntityList entityList = new EntityList(models.Where(u => u.Name == "Salary").First().Id);

            var salaryId = models.Where(u => u.Name == "Salary").First().Id;

            List<MasterInputEarnDedSettings> masterSettings = new List<MasterInputEarnDedSettings>();
            Entity entity;

            entity = new Entity(salaryId, id);
            EntityBehaviorList entbehav = new EntityBehaviorList(id, salaryId);
            EntityMasterSettings masSettings = new EntityMasterSettings();
            var masterSettingsList = masSettings.entityMastersettingList(id);
            entity.EntityAttributeModelList.OrderBy(p => p.AttributeModel.DisplayAs);
            entity.EntityAttributeModelList.ToList().ForEach(a =>
            {
                var attBeh = entbehav.Where(x => x.AttributeModelId == a.AttributeModelId).FirstOrDefault();
                if (attBeh != null)
                {
                    var masterisVisible = masterSettingsList.Where(x => x.AttributeId == a.AttributeModelId).FirstOrDefault();
                    bool AttributeIsVisible = masterisVisible != null ? true : false;
                    if ((attBeh.ArrearAttributeModelId == Guid.Empty && attBeh.ValueType == 1))
                    {
                        masterSettings.Add(MasterInputEarnDedSettings.tojson(a, attBeh, AttributeIsVisible));
                    }
                    else if (attBeh.ValueType != 1)
                    {
                        masterSettings.Add(MasterInputEarnDedSettings.tojson(a, attBeh, AttributeIsVisible));
                    }
                }
            });

            return base.BuildJson(true, 200, "success", masterSettings);
        }
        public JsonResult DeleteMonthlyInput(jsonMonthlyInput[] dataValue, Guid entitymodeId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Id", typeof(Guid));
            dt1.Columns.Add("EmployeeId", typeof(Guid));
            dt1.Columns.Add("EntityId", typeof(Guid));
            dt1.Columns.Add("EntityModelId", typeof(Guid));
            dt1.Columns.Add("Month", typeof(int));
            dt1.Columns.Add("Year", typeof(int));
            dt1.Columns.Add("AttributeModelId", typeof(Guid));
            dt1.Columns.Add("Value", typeof(String));
            dt1.Columns.Add("IsDeleted", typeof(byte));

            for (int cnt = 0; cnt < dataValue.Length; cnt++)
            {
                List<MonthlyInput> list = jsonMonthlyInput.ConvertObject(dataValue[cnt], entitymodeId);
                list.ForEach(val =>
                {
                    dt1.Rows.Add(Guid.Empty, val.EmployeeId, val.EntityId, val.EntityModelId, val.Month, val.Year, val.AttributeModelId, val.Value, val.IsDeleted);
                });
            }

            MonthlyInput minput = new MonthlyInput();
            bool result = minput.BulkDelete(dt1);

            if (result == true)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }

        }
        public JsonResult SaveMonthlyInput(jsonMonthlyInput[] dataValue, Guid entitymodeId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Id", typeof(Guid));
            dt1.Columns.Add("EmployeeId", typeof(Guid));
            dt1.Columns.Add("EntityId", typeof(Guid));
            dt1.Columns.Add("EntityModelId", typeof(Guid));
            dt1.Columns.Add("Month", typeof(int));
            dt1.Columns.Add("Year", typeof(int));
            dt1.Columns.Add("AttributeModelId", typeof(Guid));
            dt1.Columns.Add("Value", typeof(String));
            dt1.Columns.Add("IsDeleted", typeof(byte));
            for (int cnt = 0; cnt < dataValue.Length; cnt++)
            {
                List<MonthlyInput> list = jsonMonthlyInput.ConvertObject(dataValue[cnt], entitymodeId);
                list.ForEach(val =>
                {
                    dt1.Rows.Add(Guid.Empty, val.EmployeeId, val.EntityId, val.EntityModelId, val.Month, val.Year, val.AttributeModelId, val.Value, val.IsDeleted);
                });
            }

            MonthlyInput minput = new MonthlyInput();
            bool result = minput.BulkSave(dt1);
            // list.ForEach(u => u.Save());
            if (result == true)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }

        }

        /// <summary>
        /// Save Multi entry Master values 
        /// </summary>
        /// <param name="dataValue"></param>
        /// <param name="entitymodeId"></param>
        /// <returns></returns>
        public JsonResult SaveMultiEntryMasterInput(jsonMonthlyInput[] dataValue, Guid entitymodeId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isSuccess = true;
            AttributeModelList attributemodellist = new AttributeModelList(entitymodeId);
            // var ESI = attributemodellist.Where(x => x.DisplayAs == "ESI" || x.Name == "ESI").FirstOrDefault();
            // var PF = attributemodellist.Where(x => x.DisplayAs == "PF" || x.Name == "PF").FirstOrDefault();
            //Get Additional Info model Id
            //   EntityModel addtinalInfoEntity = new EntityModel("AdditionalInfo", ESI.CompanyId);

            for (int cnt = 0; cnt < dataValue.Length; cnt++)
            {
                List<MonthlyInput> list = jsonMonthlyInput.ConvertObject(dataValue[cnt], entitymodeId);
                list.ForEach(u =>
                {
                    //if (ESI.Id == u.AttributeModelId || PF.Id == u.AttributeModelId)
                    //{
                    //    EntityAdditionalInfo entityMap = new EntityAdditionalInfo();
                    //    entityMap.EntityModelId = addtinalInfoEntity.Id;
                    //    entityMap.Value = u.Value;
                    //    entityMap.AttributeModelId = (u.AttributeModelId);
                    //    entityMap.RefEntityId = Guid.Empty;
                    //    entityMap.EmployeeId = u.EmployeeId;
                    //    entityMap.CompanyId = ESI.CompanyId;
                    //    entityMap.Save();
                    //}
                    //else
                    //{
                    EntityMasterValue entTemp = new EntityMasterValue()
                    {
                        AttributeModelId = (u.AttributeModelId),
                        EntityModelId = u.EntityModelId,
                        EntityId = u.EntityId,
                        RefEntityId = u.EmployeeId,
                        RefEntityModelId = ComValue.EmployeeTable,
                        Value = u.Value
                    };
                    isSuccess = entTemp.Save();
                    // }

                });

            }
            if (isSuccess)
            {
                return base.BuildJson(true, 200, "Data saved successfully", dataValue);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", dataValue);
            }

        }


        public JsonResult SaveMasterInputSettings(Guid entityId, string attributesIds)
        {
            EntityMasterValue mastIOSave = new PayrollBO.EntityMasterValue();
            attributesIds = attributesIds.TrimEnd(',');

            var listAtt = attributesIds.Split(',').ToList();

            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("EntityId", typeof(Guid));
            table.Columns.Add("AttributeId", typeof(Guid));
            table.Columns.Add("IsVisible", typeof(bool));
            table.Columns.Add("IsDeleted", typeof(bool));
            table.Columns.Add("CreatedOn", typeof(DateTime));
            DateTime now = DateTime.Now;
            for (int i = 0; i < listAtt.Count; i++)
            {
                table.Rows.Add(Guid.NewGuid(), entityId, new Guid(listAtt[i].ToString()), true, false, now);
            }
            mastIOSave.SaveMasterInputSettings(table, entityId);
            return base.BuildJson(true, 200, "Data saved successfully", attributesIds);
        }
        public JsonResult GetPayrollHistory(Guid selectedId, int month, int year, string type)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            List<jsonPayroll> entity = new List<jsonPayroll>();
            EmployeeList employeelist = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));


            var emplist = employeelist.ToList();
            if (type == "Category")
            {
                emplist = employeelist.Where(u => u.CategoryId == selectedId).ToList();
            }
            if (type == "Single Employee")
            {
                emplist = employeelist.Where(u => u.Id == selectedId).ToList();
            }
            else if (type == "CostCentre")
            {
                emplist = employeelist.Where(u => u.CostCentre == selectedId).ToList();
            }
            else if (type == "Designation")
            {
                emplist = employeelist.Where(u => u.Designation == selectedId).ToList();
            }
            else if (type == "Branch")
            {
                emplist = employeelist.Where(u => u.Branch == selectedId).ToList();
            }
            else if (type == "Department")
            {
                emplist = employeelist.Where(u => u.Department == selectedId).ToList();

            }
            else if (type == "Location")
            {
                emplist = employeelist.Where(u => u.Location == selectedId).ToList();

            }

            PayrollHistoryList payrollhistorylist = new PayrollHistoryList(companyId, year, month);
            DateTime CurrPayrollmonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            emplist.Where(e => (e.SeparationDate == DateTime.MinValue) || e.SeparationDate > new DateTime(year, month, DateTime.DaysInMonth(year, month))).ToList().ForEach(u =>
            {
                var tmp = payrollhistorylist.Where(s => s.EmployeeId == u.Id).FirstOrDefault();
                string status = string.Empty;
                Guid payrollId = Guid.Empty;
                bool canAdd = false;
                if (!object.ReferenceEquals(tmp, null))//alerady processed
                {
                    canAdd = true;
                    status = tmp.Status;
                    payrollId = tmp.Id;

                }
                else
                {
                    if (u.DateOfJoining <= CurrPayrollmonth && u.PayrollProcess == true && u.Status == 1)
                    {
                        canAdd = true;
                        status = "Not Process";
                    }

                }
                if (canAdd)
                {
                    entity.Add(new jsonPayroll()
                    {
                        employeeCode = u.EmployeeCode,
                        employeeId = u.Id,
                        employeeName = u.FirstName + " " + u.LastName,
                        year = year,
                        month = month,
                        status = status,
                        payrollId = payrollId
                    });

                }
            });


            //payrollhistorylist.ForEach(u =>
            //{
            //    var employee = employeelist.Where(p => p.Id == u.EmployeeId).FirstOrDefault();
            //    entity.Add(new jsonPayroll()
            //    {
            //        employeeCode = employee.EmployeeCode,
            //        employeeId = employee.Id,
            //        employeeName = employee.FirstName + " " + employee.LastName,
            //        year = year,
            //        month = month,
            //        status = u != null ? u.Status : "Not Process",
            //        payrollId = u != null ? u.Id : Guid.Empty
            //    });
            //});

            //if (type == "All")
            //{
            //    employeelist = new EmployeeList(companyId);
            //}
            //else if (type == "Cat")
            //{
            //    employeelist = new EmployeeList(companyId, selectedId);
            //}


            //var seletedEmpl = employeelist.Where(u => u.DateOfJoining < CurrPayrollmonth).ToList();
            //if (type == "Emp")
            //{
            //    seletedEmpl = employeelist.Where(u => u.Id == selectedId && u.DateOfJoining < CurrPayrollmonth).ToList();
            //}
            //else if (type == "Cost")
            //{
            //    seletedEmpl = employeelist.Where(u => u.CostCentre == selectedId && u.DateOfJoining < CurrPayrollmonth && u.PayrollProcess == true).ToList();
            //}
            //else if (type == "Grd")
            //{
            //    seletedEmpl = employeelist.Where(u => u.Grade == selectedId && u.DateOfJoining < CurrPayrollmonth && u.PayrollProcess == true).ToList();
            //}



            //seletedEmpl.ForEach(u =>
            //{
            //    var payHistroy = payrollhistorylist.Where(p => p.EmployeeId == u.Id).FirstOrDefault();
            //    entity.Add(new jsonPayroll()
            //    {
            //        employeeCode = u.EmployeeCode,
            //        employeeId = u.Id,
            //        employeeName = u.FirstName + " " + u.LastName,
            //        year = year,
            //        month = month,
            //        status = payHistroy != null ? payHistroy.Status : "Not Process",
            //        payrollId = payHistroy != null ? payHistroy.Id : Guid.Empty
            //    });
            //});

            return base.BuildJson(true, 200, "success", entity);
        }


        public JsonResult DeletePayroll(Guid selectedId, int year, int month, string type)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            EmployeeList employeelist = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
            var emplist = employeelist.ToList();
            if (type == "Category")
            {
                emplist = employeelist.Where(u => u.CategoryId == selectedId).ToList();
            }
            if (type == "Single Employee")
            {
                emplist = employeelist.Where(u => u.Id == selectedId).ToList();
            }
            else if (type == "CostCentre")
            {
                emplist = employeelist.Where(u => u.CostCentre == selectedId).ToList();
            }
            else if (type == "Designation")
            {
                emplist = employeelist.Where(u => u.Designation == selectedId).ToList();
            }
            else if (type == "Branch")
            {
                emplist = employeelist.Where(u => u.Branch == selectedId).ToList();
            }
            else if (type == "Department")
            {
                emplist = employeelist.Where(u => u.Department == selectedId).ToList();

            }
            else if (type == "Location")
            {
                emplist = employeelist.Where(u => u.Location == selectedId).ToList();

            }

            int processcount = 0;
            string returnval = "";
            string prname = "";
            emplist.ForEach(u =>
            {
                PayrollHistory payHist = new PayrollHistory();
                string retval = payHist.DeletePayrollProcess(companyId, u.Id, year, month, userId, prname);
                returnval += retval;
                processcount = processcount + 1;
            });
            returnval = returnval.TrimEnd(',');
            if (returnval == "")
            {
                return base.BuildJson(true, 200, processcount > 0 ? "Deleted Payroll successfully" : "No Record(s) Processed", null);
            }
            else if (returnval != "")
            {
                return base.BuildJson(true, 200, "Some records are not deleted, please check future month payroll", null);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", null);
            }
        }
        public ActionResult ProcessPayroll(Guid selectedId, int year, int month, string type, bool includeTax = false)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            string Msg = string.Empty;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            var emproll = Convert.ToString(Session["RoleName"]);
            var username = Convert.ToString(Session["username"]);
            bool flag = false;
            if (type == "FandF")
            {
                flag = true;
            }
            //   try
            //   {
            List<jsonPayroll> entity = new List<jsonPayroll>();
            EmployeeList employeelist = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));

            var emplist = employeelist.Where(u => u.Status == 1).ToList();
            if (type == "Category")
            {
                emplist = employeelist.Where(u => u.CategoryId == selectedId).ToList();
            }
            else if (type == "Single Employee" || type.ToUpper() == "FANDF")
            {
                emplist = employeelist.Where(u => u.Id == selectedId).ToList();
            }
            else if (type == "CostCentre")
            {
                emplist = employeelist.Where(u => u.CostCentre == selectedId).ToList();
            }
            else if (type == "Designation")
            {
                emplist = employeelist.Where(u => u.Designation == selectedId).ToList();
            }
            else if (type == "Branch")
            {
                emplist = employeelist.Where(u => u.Branch == selectedId).ToList();
            }
            else if (type == "Department")
            {
                emplist = employeelist.Where(u => u.Department == selectedId).ToList();

            }
            else if (type == "Location")
            {
                emplist = employeelist.Where(u => u.Location == selectedId).ToList();

            }
            else
            {
                // emplist = employeelist.Where(u => u.Id == selectedId ).ToList();
                emplist = employeelist.Where(u => u.Status == 1).ToList();
            }
            if (!flag)
            {
                emplist.RemoveAll(e => (e.SeparationDate > DateTime.MinValue && (e.SeparationDate < DateTime.Parse(DateTime.DaysInMonth(year, month) + "/" + month + "/" + year, new CultureInfo("en-GB")) || e.SeparationDate.Month == month && e.SeparationDate.Year == year)));
            }
            int processcount = 0;
            //Worker wkr = new Worker {
            //    emplist= emplist,
            //    type=type,
            //    includeTax=includeTax,
            //    companyId=companyId,
            //    userId=userId,
            //    year=year,
            //    month=month,
            //    processcount=processcount
            //};

            // HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => wkr.StartProcessing(cancellationToken));
            //   MindspayServices.PayrollEngineClient payrolClient = new MindspayServices.PayrollEngineClient();
            //    var response =payrolClient.ProcessPayroll(emplist, type, includeTax, companyId, userId, year, month, ref processcount);

            EntityModel entModel = new EntityModel(ComValue.SalaryTable, companyId);
            Entity ent = new Entity(entModel.Id, Guid.Empty);
            string empty = "";
            EntityMappingList entMappinglist = new EntityMappingList(ent.EntityModelId, empty);


            PayrollHistoryList payrollhistorylist = new PayrollHistoryList();// new PayrollHistoryList(companyId, year, month);
            PayrollHistoryValueList payhisValues = new PayrollHistoryValueList();
            DateTime CurrPayrollmonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            List<PayrollTransaction> payTrans = new List<PayrollTransaction>();
            emplist.ForEach(u =>
            {
                if (u.DateOfJoining <= CurrPayrollmonth && u.PayrollProcess == true)
                {

                    PayrollHistory payHist = new PayrollHistory();
                    string DbId = Session["DBConnectionId"].ToString();
                    PayrollHistory payHistory = payHist.PayrollProcess(companyId, u.Id, year, month, userId, flag, DbId);

                    payrollhistorylist.Add(payHistory);
                    if (!ReferenceEquals(payHistory.currentPayValues, null) && payHistory.currentPayValues.Count > 0)
                    {
                        payhisValues.AddRange(payHistory.currentPayValues);
                    }
                    payTrans.Add(new PayrollTransaction
                    {
                        PayrollHistoryId = payHist.Id,
                        EmployeeId = u.Id,
                        EmployeeDetails = u,

                    });


                    processcount = processcount + 1;
                }
                else
                {
                    Msg = Msg + "Cannot process payroll for " + u.EmployeeCode + " check PayrollProcess Status for the Employee";
                }

            });
            StringWriter stringWriter = new StringWriter();
            XmlDocument xmlDoc = new XmlDocument();

            XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);


            XmlSerializer serializer = new XmlSerializer(typeof(PayrollHistoryList));

            serializer.Serialize(xmlWriter, payrollhistorylist);

            string xmlResult = stringWriter.ToString();

            PayrollHistory pay = new PayrollHistory();
            pay.Importxmlstring = xmlResult;
            pay.ImportPay();

            serializer = new XmlSerializer(typeof(PayrollHistoryValueList));
            stringWriter = new StringWriter();
            xmlWriter = new XmlTextWriter(stringWriter);
            serializer.Serialize(xmlWriter, payhisValues);

            xmlResult = stringWriter.ToString();

            PayrollHistoryValue payValues = new PayrollHistoryValue();
            payValues.Importxmlstring = xmlResult;
            payValues.ImportpayValues();


            TXFinanceYearList financeYearlist = new TXFinanceYearList(companyId);
            TXFinanceYear defaultyear = new TXFinanceYear();
            defaultyear = financeYearlist.Where(e => e.IsActive).FirstOrDefault();
            Guid financeYearId = defaultyear.Id;


            PayrollTransaction.InertPayrollTransaction(payTrans, companyId);

            TaxProcessController tpc = new TaxProcessController();

            if (includeTax)
            {
                string processtype = "";
                tpc.It_Init_Para(entMappinglist, emplist, selectedId, year, month, type, financeYearId, companyId, emproll, userId, username, Msg, processtype);
                Msg = Msg == string.Empty ? "Data saved successfully" : Msg;

            }


            if (true)
            {
                if (processcount > 0)
                {
                    return base.BuildJson(true, 200, Msg == string.Empty ? "Data saved successfully" : Msg, null);
                }
                else
                {
                    return base.BuildJson(true, 200, "No Record(s) Processed", null);
                }
            }
            else
            {
                return base.BuildJson(false, 100, Msg == string.Empty ? "There is some error while saving the data." : Msg, null);
            }


            //}
            //catch (Exception ex)
            //{

            //    return base.BuildJson(false, 200, ex.Message, null);
            //}

        }

        public JsonResult ProcessFandF(Guid selectedId, int year, int month, DateTime resignationDate, DateTime lastWorkingDate, string type, Guid empId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            string Msg = string.Empty;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);

            List<jsonPayroll> entity = new List<jsonPayroll>();
            EmployeeList employeelist = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));

            var emplist = employeelist.Where(u => u.Status == 1).ToList();
            if (type == "Category")
            {
                emplist = employeelist.Where(u => u.CategoryId == selectedId).ToList();
            }
            if (type == "Single Employee")
            {
                emplist = employeelist.Where(u => u.Id == selectedId).ToList();
            }
            else if (type == "CostCentre")
            {
                emplist = employeelist.Where(u => u.CostCentre == selectedId).ToList();
            }
            else if (type == "Designation")
            {
                emplist = employeelist.Where(u => u.Designation == selectedId).ToList();
            }
            else if (type == "Branch")
            {
                emplist = employeelist.Where(u => u.Branch == selectedId).ToList();
            }
            else if (type == "Department")
            {
                emplist = employeelist.Where(u => u.Department == selectedId).ToList();

            }
            else if (type == "Location")
            {
                emplist = employeelist.Where(u => u.Location == selectedId).ToList();

            }
            TimeSpan paidDays = lastWorkingDate - resignationDate;
            int noofmonth = lastWorkingDate.Month - resignationDate.Month;
            DateTime[] paidMonths = new DateTime[noofmonth];
            int lastmont = DateTime.DaysInMonth(lastWorkingDate.Year, lastWorkingDate.Month) == lastWorkingDate.Day ? lastWorkingDate.Month : lastWorkingDate.Month - 1;
            int index = 0;
            int curyear = resignationDate.Year;
            for (int smonth = resignationDate.Month; smonth <= lastmont; smonth++)
            {

                paidMonths[index] = new DateTime(curyear, smonth, 1);
                index++;
                if (smonth == 12)
                {
                    smonth = 0;
                    curyear = resignationDate.Year + 1;
                }
            }
            for (int monthIndex = 0; monthIndex < paidMonths.Count(); monthIndex++)
            {
                PayrollHistoryList payrollhistorylist = new PayrollHistoryList(companyId, paidMonths[monthIndex].Year, paidMonths[monthIndex].Month);
                DateTime CurrPayrollmonth = new DateTime(year, month, DateTime.DaysInMonth(paidMonths[monthIndex].Year, paidMonths[monthIndex].Month));
                Employee emp = new Employee(companyId, empId);
                if (emp.DateOfJoining < CurrPayrollmonth && emp.PayrollProcess == true)
                {
                    PayrollHistory payHist = new PayrollHistory();

                    payHist.FandFProcess(companyId, emp.Id, paidMonths[monthIndex].Year, paidMonths[monthIndex].Month, userId);

                }
                else
                {
                    Msg = Msg + "Cannot process payroll for " + emp.EmployeeCode + " check PayrollProcess Status for the Employee";
                }
            }

            if (lastmont != lastWorkingDate.Month)
            {
                PayrollHistoryList payrollhistorylist = new PayrollHistoryList(companyId, lastWorkingDate.Year, lastWorkingDate.Month);
                DateTime CurrPayrollmonth = new DateTime(year, month, DateTime.DaysInMonth(lastWorkingDate.Year, lastWorkingDate.Month));
                Employee emp = new Employee(companyId, empId);
                if (emp.DateOfJoining < CurrPayrollmonth && emp.PayrollProcess == true)
                {
                    PayrollHistory payHist = new PayrollHistory();

                    payHist.FandFProcess(companyId, emp.Id, lastWorkingDate.Year, lastWorkingDate.Month, userId, lastWorkingDate.Day);

                }
                else
                {
                    Msg = Msg + "Cannot process payroll for " + emp.EmployeeCode + " check PayrollProcess Status for the Employee";
                }
            }
            //// DateTime CurrPayrollmonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            // var seletedEmpllist = emplist.Where(u => u.DateOfJoining < CurrPayrollmonth && u.PayrollProcess == true).ToList();
            // if (type == "Emp")
            // {
            //     seletedEmpllist = employeelist.Where(u => u.Id == selectedId && u.DateOfJoining < CurrPayrollmonth && u.PayrollProcess == true).ToList();
            //     if (seletedEmpllist.Count <= 0)
            //     {
            //         return base.BuildJson(false, 100, "Cannot process payroll for this Employee.", null);
            //     }
            // }
            // else if (type == "Cost")
            // {
            //     seletedEmpllist = employeelist.Where(u => u.CostCentre == selectedId && u.DateOfJoining < CurrPayrollmonth && u.PayrollProcess == true).ToList();
            // }
            // else if (type == "Grd")
            // {
            //     seletedEmpllist = employeelist.Where(u => u.Grade == selectedId && u.DateOfJoining < CurrPayrollmonth && u.PayrollProcess == true).ToList();
            // }

            // seletedEmpllist.ForEach(u =>
            // {
            //     PayrollHistory payHist = new PayrollHistory();
            //     //  payHist.ProcessPayroll(companyId, u.Id, year, month, userId);
            //     payHist.PayrollProcess(companyId, u.Id, year, month, userId);
            // });

            if (true)
            {
                //if (processcount > 0)
                //{
                return base.BuildJson(true, 200, Msg == string.Empty ? "Data saved successfully" : Msg, null);
                //}
                //else
                //{
                //    return base.BuildJson(true, 200, "No Record(s) Processed", null);
                //}
            }
            else
            {
                return base.BuildJson(false, 100, Msg == string.Empty ? "There is some error while saving the data." : Msg, null);
            }

        }
        public JsonResult GetPayrollProcessdHistory(Guid employeeId, int month, int year)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Entity entity;
            if (employeeId != Guid.Empty)
            {
                List<PayrollError> payErrors = new List<PayrollError>();
                List<ArrearHistory> saveArrearHistory = new List<ArrearHistory>(); ;
                PayrollHistory payHistory = new PayrollHistory();
                Employee emp = new Employee(companyId, employeeId);
                List<PayrollBO.ArrearHistory> dummyobj = new List<PayrollBO.ArrearHistory>();
                entity = payHistory.ExecuteProcess(companyId, emp, year, month, userId, out dummyobj, out payErrors, false, "history");
            }
            else
            {
                entity = new Entity();

            }

            return base.BuildJson(true, 200, "success", entity);
        }
        public PartialViewResult Download()
        {
            return PartialView("DownloadFile");
        }
        //[HttpPost]
        public JsonResult PrintPayrollProcessdHistory(Guid employeeId, string month, string year)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            Guid employeeid = new Guid(Convert.ToString(Session["EmployeeId"]));
            string Authority = Request.Url.GetLeftPart(UriPartial.Authority) + "/";
            string sessionEmpCode = Convert.ToString(Session["EmployeeCode"]);
            Employee emp = new Employee(companyId, employeeId);
            string baseUr = Authority;// Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
            return DataWizardController.DownloadPayslip("", Convert.ToInt32(month), Convert.ToInt32(year), emp.EmployeeCode, companyId, userId, employeeId, baseUr, "GeneratePDF", sessionEmpCode);
            //Guid Empid = new Guid(employeeId);
            //int companyId = Convert.ToInt32(Session["CompanyId"]);
            //int userId = Convert.ToInt32(Session["UserId"]);
            //decimal totgross = 0, totdeduction = 0;
            //Entity entity;
            //Employee employee = new Employee(companyId, Empid);
            //Company comp = new Company(companyId, Convert.ToInt32(Session["UserId"]));
            //ViewBag.CompanyName = comp.CompanyName;
            //if (Empid != Guid.Empty)
            //{
            //    List<PayrollError> payErrors = new List<PayrollError>();
            //    PayrollHistory payHistory = new PayrollHistory();
            //    entity = payHistory.ExecuteProcess(companyId, Empid, Convert.ToInt32(year), Convert.ToInt32(month), userId, out payErrors);


            //    string earningTable = "<table style=\"width:100%;\"> <thead><tr><th style=\"width:50% \"> EARNINGS </th><th style=\"width:25% \" align=\"right\"> Amount </th><th style=\"width:25% \" align=\"right\"> Cumulative </th ></tr></thead><tbody> ";
            //    string deductionTable = "<table style=\"width:100%;\"> <thead><tr><th style=\"width:50% \"> DEDUCTIONS </th><th style=\"width:25% \" align=\"right\"> Amount </th><th style=\"width:25% \" align=\"right\"> Cumulative </th ></tr></thead><tbody> ";
            //    for (int cnt = 0; cnt < entity.EntityAttributeModelList.Count; cnt++)
            //    {
            //        if (entity.EntityAttributeModelList[cnt].AttributeModel.IsIncludeForGrossPay == true)
            //        {
            //            if (entity.EntityAttributeModelList[cnt].AttributeModel.BehaviorType == "Earning")
            //            {
            //                earningTable = earningTable + "<tr><td>" + entity.EntityAttributeModelList[cnt].AttributeModel.DisplayAs + "</td>"
            //                                  + "<td align=\"right\">" + entity.EntityAttributeModelList[cnt].EntityAttributeValue.Value + "</td>"
            //                                   + "<td align=\"right\">" + entity.EntityAttributeModelList[cnt].EntityAttributeValue.Value + "</td></tr>";
            //                totgross = totgross + Convert.ToDecimal(entity.EntityAttributeModelList[cnt].EntityAttributeValue.Value);
            //            }
            //            else if (entity.EntityAttributeModelList[cnt].AttributeModel.BehaviorType == "Deduction")
            //            {
            //                deductionTable = deductionTable + "<tr><td style=\"width:100% \">" + entity.EntityAttributeModelList[cnt].AttributeModel.DisplayAs + "</td>"
            //           + "<td align=\"right\">" + entity.EntityAttributeModelList[cnt].EntityAttributeValue.Value + "</td>"
            //          + "<td align=\"right\">" + entity.EntityAttributeModelList[cnt].EntityAttributeValue.Value + "</td></tr>";
            //                totdeduction = totdeduction + Convert.ToDecimal(entity.EntityAttributeModelList[cnt].EntityAttributeValue.Value);
            //            }
            //        }



            //    }
            //    //   earningTable = earningTable + "<tr><td>Gross</td><td>" + totgross + "</td><td>" + totgross + "</td></tr>";
            //    //  deductionTable = deductionTable + "<tr><td>Total Deductions</td><td>" + totdeduction + "</td><td>" + totdeduction + "</td></tr>";

            //    earningTable = earningTable + "</tbody></table>";
            //    deductionTable = deductionTable + "</tbody></table>";
            //    string adds = string.Empty;
            //    if (comp.AddressLine1.Trim() != string.Empty)
            //    {
            //        adds = adds + comp.AddressLine1.Trim() + ",";
            //    }
            //    if (comp.AddressLine2.Trim() != string.Empty)
            //    {
            //        adds = adds + comp.AddressLine2.Trim() + ",";
            //    }
            //    if (comp.City.Trim() != string.Empty)
            //    {
            //        adds = adds + comp.City.Trim() + ",";
            //    }
            //    adds = adds.TrimEnd(',');
            //    string htmlData = "<html><body><table frame=\"border\" style=\" font-size:small;  border-style: solid; border-width:thin; border-color:Black;font-family:Calibri;width:100%; margin-right:0; margin-left:0; \" >"
            //       + "<tbody>< tr >< td style =\" border-style: solid; border-width:thin; border-color:Black;\"align=\"center\" colspan=\"4\">" + comp.CompanyName + "<br/>" + adds + "<br/><br/>PAYSLIP FOR THE MONTH OF " + ((MonthEnum)Convert.ToInt16(month)).ToString().ToUpper() + " - 2015</td></tr>"
            //       + "<tr  style=\" height: 10px\"><td colspan=\"4\">" + string.Empty + " </td></tr>"
            //       + "<tr><td>EMP.CODE</td><td>" + employee.EmployeeCode + "</td><td>NAME</td><td>" + employee.FirstName + "</td></tr>"
            //       + "<tr><td>DATE OF BIRTH</td><td>" + employee.DateOfBirth.ToString("dd/MMM/yyyy") + "</td><td>DATE OF JOINING</td><td>" + employee.DateOfJoining.ToString("dd/MMM/yyyy") + "</td></tr>"
            //       + "<tr><td>DESIGNATION</td><td>" + employee.DesignationName + "</td><td>BRANCH</td><td>" + employee.BranchName + "</td></tr>"
            //       + "<tr  style=\" height: 20px\"><td colspan=\"4\">" + string.Empty + " </td></tr>"
            //       + "<tr><td colspan=\"2\" style=\"vertical-align:top;  border-style: solid; border-width:thin; border-color:Black;\" >" + earningTable + "</td><td colspan=\"2\" style=\"vertical-align:top;  border-style: solid; border-width:thin; border-color:Black;\">" + deductionTable + "</td></tr> "
            //       + "<tr><td colspan=\"2\" ><table style=\"width:100% \"><tbody><tr><td style=\"width:50% \">Gross</td><td style=\"width:25% \" align=\"right\" >" + totgross + "</td><td style=\"width:25% \" align=\"right\">" + totgross + "</td></tr></tbody></table></td>"
            //       + "<td colspan=\"2\" ><table style=\"width:100% \"><tbody><tr><td style=\"width:50% \">Total Deductions</td><td style=\"width:25% \" align=\"right\" >" + totdeduction + "</td><td style=\"width:25% \" align=\"right\" >" + totdeduction + "</td></tr></tbody></table></td></tr>"
            //       + "<tr><td colspan=\"4\">Net Pay: " + (totgross - totdeduction).ToString() + " </td></tr>"
            //       + "<tr  style=\" height: 30px\"><td colspan=\"4\">" + string.Empty + " </td></tr>"
            //       + "<tr  style=\" height: 50px\"><td colspan=\"4\">Rupees In Words: " + string.Empty + " </td></tr>"
            //       + "<tr><td colspan=\"4\">Remarks: " + string.Empty + " </td></tr>"
            //       + "<tr  style=\" height: 100px\"><td colspan=\"2\"  align=\"left\">Signature of the employee</td><td colspan=\"2\" align=\"right\">Administration Executive </td></tr>";
            //    htmlData = htmlData + "</tbody></table></body></html>";
            //    // create a stream that we can write to, in this case a MemoryStream  
            //    using (var stream = new MemoryStream())
            //    {
            //        // create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF  
            //        using (var document = new Document())
            //        {
            //            // create a writer that's bound to our PDF abstraction and our stream  
            //            using (var writer = PdfWriter.GetInstance(document, stream))
            //            {
            //                // open the document for writing  
            //                document.Open();

            //                // read html data to StringReader  
            //                using (var html = new StringReader(htmlData))
            //                {
            //                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, html);
            //                    XMLWorkerHelper.GetInstance().GetDefaultCSS();
            //                }

            //                // close document  
            //                document.Close();
            //            }
            //        }

            //        // get bytes from stream  
            //        Byte[] bytes = stream.ToArray();

            //        using (FileStream str = new FileStream(Server.MapPath(@"~\payslip.pdf"), FileMode.Create))
            //        {
            //            using (StreamWriter writer = new StreamWriter(str, Encoding.UTF8))
            //            {
            //                str.Write(bytes, 0, bytes.Length);
            //                writer.Close();
            //            }
            //        }

            //    }
            //    return base.BuildJson(true, 200, "File saved successfully", new { filePath = Server.MapPath(@"~\payslip.pdf") });


            //}
            //else
            //{
            //    return null;

            //}
        }
        public byte[] GetBytes(string str)
        {
            Byte[] bytes = new Byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        public JsonResult InterChangeOrder(Guid entityModelId, Guid entityAttributeModelId, string actionVal)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            EntityAttributeModel entiAttribute = new EntityAttributeModel();
            if (entiAttribute.InterChangeOrder(entityModelId, entityAttributeModelId, actionVal))
            {
                EntityModel entimod = new EntityModel(entityModelId);
                return base.BuildJson(true, 200, "Data saved successfully", entimod);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while saving the data.", false);
            }
        }

        public JsonResult DeleteEntityAttributeModel(Guid entityModelId, Guid entityAttributeModelId)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            string[] SplitArrayDed = new string[40];
            EntityAttributeValue tempDed1 = new EntityAttributeValue();
            EntityBehavior tempBehav = new EntityBehavior();
            var tempDed2 = string.Empty;
            var tempAttrModid = string.Empty;
            string[] SplitArrayEar = new string[40];
            string NewBehavTodeducValue = string.Empty;
            string NewBehavTodEarValue = string.Empty;
            EntityAttributeValue entAttValObj = new EntityAttributeValue();
            EntityBehavior entbehavObjValue = new EntityBehavior();
            bool isDontdelete = false;
            bool isLoanComponent = false;
            try
            {
                EntityList entilist = new EntityList(entityModelId);
                entilist.ForEach(u =>
                {


                    EntityBehaviorList entbehav = new EntityBehaviorList(u.Id, u.EntityModelId);
                    entbehav.ForEach(p =>
                    {
                        if (p.Formula.ToUpper().Contains(entityAttributeModelId.ToString().ToUpper()))
                        {
                            isDontdelete = true;
                        }
                        else if (p.ArrearFormula.ToUpper().Contains(entityAttributeModelId.ToString().ToUpper()))
                        {
                            isDontdelete = true;
                        }
                    });
                });
                if (isDontdelete)
                {
                    return base.BuildJson(false, 100, "You can't delete the column which is used in the application/Formula.", false);
                }
                //then will delete the column
                EntityAttributeModel entiAttribute = new EntityAttributeModel(entityModelId, entityAttributeModelId);
                //InActive  loan component from loan master check unpaid count > 0 not allow to delete that component
                AttributeModel attModel = new AttributeModel(entiAttribute.AttributeModelId, companyId);
                if (attModel.IsInstallment)
                {
                    LoanMasterList loanMasterList = new LoanMasterList(companyId);
                    var temploanMasterList = loanMasterList.Where(x => x.CompanyId == companyId && x.AttributeModelId == attModel.Id).FirstOrDefault();
                    if (temploanMasterList != null)
                    {
                        LoanEntryList tempLoanEntryList = new LoanEntryList(Guid.Empty, temploanMasterList.Id);
                        //tempLoanEntryList.ForEach(l => 
                        //{
                        //    LoanTransactionList loanTranList = new LoanTransactionList(l.Id);
                        //    if (loanTranList.Where(lt => lt.Status.ToLower() == "unpaid").Count() > 0)
                        //        isLoanComponent = true;
                        //    break;
                        //});
                        foreach (var item in tempLoanEntryList)
                        {
                            LoanTransactionList loanTranList = new LoanTransactionList(item.Id);
                            if (loanTranList.Where(lt => lt.Status.ToLower() == "unpaid").Count() > 0)
                            {
                                isLoanComponent = true;
                                break;
                            }

                        }
                        if (isLoanComponent == false)
                        {
                            temploanMasterList.IsActive = false;
                            temploanMasterList.IsDeleted = true;
                            temploanMasterList.ModifiedOn = DateTime.Now;
                            temploanMasterList.ModifiedBy = Convert.ToInt32(Session["UserId"]);
                            temploanMasterList.Delete();
                        }
                    }
                }
                if (isLoanComponent)
                {
                    return base.BuildJson(false, 100, "You can't delete the column which is used in the loan component", false);
                }


                if (entiAttribute.Delete())
                {
                    //Changes made by mubarak in order to solve the issue in entitybehaviour and attributevalue table
                    EntityModel entimod = new EntityModel(entityModelId);
                    AttributeModelList attributemodel = new AttributeModelList(entityModelId, companyId);
                    var temp3 = entiAttribute.AttributeModel;

                    EntityList entilistCheck = new EntityList(entityModelId);
                    entilistCheck.ForEach(u =>
                    {
                        for (int i = 0; i < u.EntityAttributeModelList.Count; i++)
                        {
                            if (temp3.BehaviorType == "Deduction")
                            {
                                if (u.EntityAttributeModelList[i].AttributeModel.Name == "TOTDED")
                                {

                                    tempDed1 = u.EntityAttributeModelList[i].EntityAttributeValue;
                                    tempAttrModid = u.EntityAttributeModelList[i].AttributeModelId.ToString();
                                    tempDed2 = u.EntityAttributeModelList[i].EntityAttributeValue.Value;
                                    SplitArrayDed = tempDed2.Split('+');
                                }
                            }
                            else
                            {
                                if (u.EntityAttributeModelList[i].AttributeModel.Name == "EG")
                                {

                                    tempDed1 = u.EntityAttributeModelList[i].EntityAttributeValue;
                                    tempAttrModid = u.EntityAttributeModelList[i].AttributeModelId.ToString();
                                    tempDed2 = u.EntityAttributeModelList[i].EntityAttributeValue.Value;
                                    SplitArrayDed = tempDed2.Split('+');
                                }
                            }


                        }

                        var temp4 = attributemodel.Where(Ear => Ear.Id == temp3.Id).FirstOrDefault();
                        for (int i = 0; i < SplitArrayDed.Length; i++)
                        {
                            if (temp4.Name == SplitArrayDed[i].ToString())
                            {

                            }
                            else
                            {
                                if (NewBehavTodeducValue == string.Empty)
                                {
                                    NewBehavTodeducValue = SplitArrayDed[i].ToString();
                                }
                                else
                                {
                                    NewBehavTodeducValue = NewBehavTodeducValue + "+" + SplitArrayDed[i].ToString();
                                }

                            }
                        }
                        entAttValObj.EntityAttributeModelId = tempDed1.EntityAttributeModelId;
                        entAttValObj.EntityId = tempDed1.EntityId;
                        entAttValObj.EntityModelId = tempDed1.EntityModelId;
                        entAttValObj.Value = NewBehavTodeducValue;
                        entAttValObj.Save();

                    });
                    EntityList EntObj = new EntityList(entityModelId);
                    EntObj.ForEach(u =>
                    {
                        EntityBehaviorList EntBehaveObj = new EntityBehaviorList(u.Id, entityModelId);
                        for (int i = 0; i < EntBehaveObj.Count; i++)
                        {
                            if (EntBehaveObj[i].AttributeModelId == new Guid(tempAttrModid))
                            {
                                tempBehav = EntBehaveObj[i];
                                var tempbehave = EntBehaveObj[i].Formula;
                                SplitArrayEar = tempbehave.Split('+');
                            }
                        }
                        var temp5 = attributemodel.Where(Ear => Ear.Id == temp3.Id).FirstOrDefault();
                        for (int i = 0; i < SplitArrayEar.Length; i++)
                        {
                            if (new Guid(temp5.Id.ToString()) == new Guid(SplitArrayEar[i].ToString()))
                            {

                            }
                            else
                            {
                                if (NewBehavTodEarValue == string.Empty)
                                {
                                    NewBehavTodEarValue = SplitArrayEar[i].ToString();
                                }
                                else
                                {
                                    NewBehavTodEarValue = NewBehavTodEarValue + "+" + SplitArrayEar[i].ToString();
                                }

                            }
                        }
                        entbehavObjValue.EntityId = tempBehav.EntityId;
                        entbehavObjValue.EntityModelId = tempBehav.EntityModelId;
                        entbehavObjValue.AttributeModelId = tempBehav.AttributeModelId;
                        entbehavObjValue.Formula = NewBehavTodEarValue;
                        entbehavObjValue.Save();
                    });
                    //Changes end Here By mubarak.


                    List<AttributeModel> Earning = attributemodel.Where(Ear => Ear.BehaviorType == "Earning").ToList();
                    List<AttributeModel> Deduction = attributemodel.Where(Ear => Ear.BehaviorType == "Deduction").ToList();
                    return base.BuildJson(true, 200, "Data Deleted successfully", entimod);
                }
                else
                {
                    return base.BuildJson(false, 100, "There is some error while deleting the data.", false);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return base.BuildJson(false, 100, "There is some error while deleting the data.", false);
            }
        }
        /// <summary>
        /// Modified By:Sharmila
        /// Modified On:5.06.17
        /// </summary>
        /// <param name="attributeModelId"></param>
        /// <returns></returns>
        public JsonResult DeleteAttributeModel(Guid attributeModelId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);
            bool isDontdelete = false;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            TableCategoryList tabcat = new TableCategoryList(companyId);
            tabcat.ForEach(u =>
            {
                EntityModelList entimodls = new EntityModelList(u.Id);
                entimodls.ForEach(p =>
                {
                    p.EntityAttributeModelList.ForEach(q =>
                    {
                        if (q.AttributeModelId == attributeModelId)
                        {
                            isDontdelete = true;
                        }
                    });
                });

            });
            //then will delete the column
            AttributeModel entiAttribute = new AttributeModel(attributeModelId, companyId);
            var DispValue = entiAttribute.DisplayAs;
            if (entiAttribute.IsSetting || entiAttribute.IsInstallment)
            {
                isDontdelete = true;
            }
            if (isDontdelete)
            {
                return base.BuildJson(false, 100, "You can't delete the column which is default or used in the application.", false);
            }
            var DeleteStatus = entiAttribute.Delete();
            if (DeleteStatus == true)
            {

                AttributeModelTypeList data = new AttributeModelTypeList(companyId);

                return base.BuildJson(true, 200, "Data deleted successfully", data);
            }
            else
            {
                return base.BuildJson(false, 100, "Delete failed!" + DispValue + "Field is in use ", false);
            }
            // if (entiAttribute.Delete())
            //{

            //    AttributeModelTypeList data = new AttributeModelTypeList(companyId);

            //    return base.BuildJson(true, 200, "Data deleted successfully", data);
            //}
            //else
            //{
            //    return base.BuildJson(false, 100, "There is some error while deleting the data.", false);
            //}
        }

        public JsonResult DeleteEntityModel(Guid entityModelId)
        {
            if (!base.checkSession())
                return base.BuildJson(true, 0, "Invalid user", null);

            int companyId = Convert.ToInt32(Session["CompanyId"]);
            EntityModel entiModel = new EntityModel(entityModelId);

            if (entiModel.Name == "Salary")
            {
                return base.BuildJson(false, 100, "You can't delete the table which is default in the application.", false);
            }
            //then will delete the column

            if (entiModel.Delete())
            {
                TableCategoryList data = new TableCategoryList(companyId);
                return base.BuildJson(true, 200, "Data deleted successfully", data);
            }
            else
            {
                return base.BuildJson(false, 100, "There is some error while deleting the data.", false);
            }
        }


        public void savemonthlycarryforward(int companyId, Guid? entitymodelId, Guid entityId, Guid categoryId, int month, int year, int curmonth, int curyear, Guid emp, PayrollHistoryList historylist)
        {
            MonthlyInputList monthlyinputlis = new MonthlyInputList(entityId, emp, month, year);
            if (monthlyinputlis.Count > 0)
            {
                int mont = monthlyinputlis.FirstOrDefault().Month;
                int yea = monthlyinputlis.FirstOrDefault().Year;
                DateTime date = new DateTime(curyear, curmonth, 1);
                // date = date.AddMonths(1);

                PayrollHistoryValueList payvaluelist = new PayrollHistoryValueList(year, month);

                DataTable dt1 = new DataTable();
                dt1.Columns.Add("Id", typeof(Guid));
                dt1.Columns.Add("EmployeeId", typeof(Guid));
                dt1.Columns.Add("EntityId", typeof(Guid));
                dt1.Columns.Add("EntityModelId", typeof(Guid));
                dt1.Columns.Add("Month", typeof(int));
                dt1.Columns.Add("Year", typeof(int));
                dt1.Columns.Add("AttributeModelId", typeof(Guid));
                dt1.Columns.Add("Value", typeof(String));
                dt1.Columns.Add("IsDeleted", typeof(byte));


                MonthlyInput monthlynew = new MonthlyInput();



                monthlyinputlis.GroupBy(g => g.EmployeeId).ToList().ForEach(f =>
                {

                    //var paylist =  new PayrollHistory(companyId, f.Key, year, month);
                    var paylist = historylist.Where(hl => hl.CompanyId == companyId && hl.EmployeeId == f.Key && hl.Year == year && hl.Month == month).FirstOrDefault();
                    previousComponentslist things = new previousComponentslist(categoryId, entityId, new Guid(entitymodelId.ToString()));

                    things.Where(w => w.MappedId != Guid.Empty).ToList().ForEach(t =>
                    {
                        var mon = monthlyinputlis.Where(w => w.AttributeModelId == t.MappedId && w.EmployeeId == f.Key).FirstOrDefault();


                        if (!object.ReferenceEquals(mon, null))
                        {
                            //monthlynew.Id = Guid.NewGuid();
                            //monthlynew.AttributeModelId = t.Id;
                            //monthlynew.EmployeeId = f.Key;
                            //monthlynew.EntityId = entityId;
                            //monthlynew.Year = date.Year;
                            //monthlynew.Month = date.Month;
                            //monthlynew.Value = mon.Value;
                            //monthlynew.EntityModelId = new Guid(entitymodelId.ToString());
                            //monthlynew.Save();
                            dt1.Rows.Add(Guid.Empty, f.Key, entityId, new Guid(entitymodelId.ToString()), date.Month, date.Year, t.Id, mon.Value, 0);
                        }
                        else
                        {
                            if (paylist.Id != Guid.Empty)
                            {
                                //PayrollHistoryValueList pay = new PayrollHistoryValueList(paylist.Id);
                                var pay = payvaluelist.Where(pv => pv.Id == paylist.Id).ToList();
                                var payhis = pay.Where(w => w.AttributeModelId == t.MappedId).FirstOrDefault();
                                if (!object.ReferenceEquals(payhis, null))
                                {
                                    //monthlynew.Id = Guid.NewGuid();
                                    //monthlynew.AttributeModelId = t.Id;
                                    //monthlynew.EmployeeId = f.Key;
                                    //monthlynew.EntityId = entityId;
                                    //monthlynew.Year = date.Year;
                                    //monthlynew.Month = date.Month;
                                    //monthlynew.Value = payhis.Value;
                                    //monthlynew.EntityModelId = new Guid(entitymodelId.ToString());
                                    //monthlynew.Save();
                                    dt1.Rows.Add(Guid.Empty, f.Key, entityId, new Guid(entitymodelId.ToString()), date.Month, date.Year, t.Id, payhis.Value, 0);
                                }
                            }
                        }
                    });
                    if (dt1.Rows.Count > 0)
                    {
                        MonthlyInput minput = new MonthlyInput();
                        minput.BulkSave(dt1);
                    }

                });

            }
        }


        public List<jsonMonthlyInput> GetMonthlyInputvalues(LeaveFinanceYear DefaultFinancialYr, LeaveRequestList GetLeaveList, DefaultLOPid lossofpayid, AttributeModelList attributemodellist, Guid? entitymodelId, Guid entityId, Guid categoryId, int month, int year, PayrollHistoryList historylist, Guid employeeId = new Guid())
        {

            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int userId = Convert.ToInt32(Session["UserId"]);



            List<Employee> seletedEmpl = new List<Employee>();
            // int seletedEmpl=0;
            //  var isEmployee = "";
            if (entityId == Guid.Empty && entitymodelId == Guid.Empty)
            {
                MonthlyInputList empMonthlyInput = new MonthlyInputList(entityId, entitymodelId.Value, employeeId);
                var empEntityId = empMonthlyInput.Where(emi => emi.EmployeeId == employeeId).FirstOrDefault();
                if (empEntityId != null)
                {
                    entityId = empEntityId.EntityId;
                    entitymodelId = empEntityId.EntityModelId;
                }
            }
            EntityMappingList entitMapp = new EntityMappingList(entityId);
            List<jsonMonthlyInput> entity = new List<jsonMonthlyInput>();

            //Commented in order to resolve the MI data binding incase of Dynamic grp changes.
            //MonthlyInputList monthlyinputlist = new MonthlyInputList(entityId, employeeId, month, year);

            MonthlyInputList monthlyinputlist = new MonthlyInputList(Guid.Empty, employeeId, month, year);

            var payrollHistorylist = historylist.Where(hl => hl.CompanyId == companyId && hl.Year == year && hl.Month == month).ToList();

            //New Code Started
            entitMapp.ForEach(o =>
            {
                var temp = payrollHistorylist.Where(u => u.EmployeeId == new Guid(o.RefEntityId) && u.IsDeleted == false && u.Status == "Processed").FirstOrDefault();
                if (!object.ReferenceEquals(temp, null))
                {
                    o.EntityId = temp.EntityId.ToString();
                }
            });

            //New Code End

            //testing
            // LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyId);
            // AttributeModelList attributemodellist = new AttributeModelList(companyId);
            AttributeModel AttrModel = new AttributeModel();
            AttrModel = attributemodellist.Where(u => u.Name == "LD").FirstOrDefault();


            EntityAttributeModelList EnttModelList = new EntityAttributeModelList(entitymodelId.Value, entityId);
            EntityAttributeModel EntityList = new EntityAttributeModel();
            EntityList = EnttModelList.Where(s => s.AttributeModelId == AttrModel.Id).FirstOrDefault();
            EntityAttributeValueList EnttAttValueList = new EntityAttributeValueList(EntityList.EntityModelId);
            EntityAttributeValue EntityValue = new EntityAttributeValue();
            EntityValue = EnttAttValueList.Where(q => q.EntityAttributeModelId == EntityList.Id).FirstOrDefault();
            var LdaysValue = EntityValue.Value;
            // LeaveRequestList GetLeaveList = new LeaveRequestList(DefaultFinancialYr.Id, companyId, month, year);
            // DefaultLOPid lossofpayid = new DefaultLOPid(companyId);


            if (entitMapp.Count > 0)
            {
                if (entitMapp[0].RefEntityModelId == ComValue.EmployeeTable)//"Employee"
                {
                    EmployeeList employee;
                    if (employeeId == Guid.Empty)
                    {
                        employee = new EmployeeList(companyId, userId, new Guid(Convert.ToString(Session["EmployeeGUID"])));
                    }
                    else
                    {
                        employee = new EmployeeList(companyId, userId, employeeId, 0, 0);
                    }



                    //  var isEmployee;
                    DateTime CurrPayrollmonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    if (employeeId == Guid.Empty)
                    {
                        seletedEmpl = employee.Where(u => u.DateOfJoining <= CurrPayrollmonth && u.CategoryId == categoryId && u.SeparationDate == DateTime.MinValue).ToList();
                    }
                    else
                    {
                        seletedEmpl = employee.Where(u => u.DateOfJoining <= CurrPayrollmonth && u.CategoryId == categoryId).ToList();
                    }


                    entitMapp.ForEach(u =>
                    {
                        EntityBehaviorList entityBehavior = new EntityBehaviorList(new Guid(u.EntityId), entitymodelId.Value);
                        List<EntityBehavior> seletedEntityBehavir = entityBehavior.Where(w => w.ValueType == 2).ToList();


                        // Remove UnMapped component from monthly component type. Modified on 04/11/2018 By Muthu
                        seletedEntityBehavir.ToList().ForEach(s =>
                        {
                            if (EnttModelList.Where(x => x.AttributeModelId == s.AttributeModelId).FirstOrDefault() == null)
                                seletedEntityBehavir.Remove(seletedEntityBehavir.Where(x => x.AttributeModelId == s.AttributeModelId).FirstOrDefault());
                        });
                        var isEmployee = seletedEmpl.Where(t => t.Id == new Guid(u.RefEntityId)).ToList();
                        if (isEmployee.Count > 0)
                        {
                            //var LDays = GetLeaveList.Where(k => k.EmployeeId == new Guid(u.RefEntityId)).ToList().Count;
                            //added now
                            var Tempp = GetLeaveList.Where(k => k.EmployeeId == new Guid(u.RefEntityId) && k.LeaveType == new Guid(lossofpayid.LOPId.ToString())).ToList();//"199f5db2-14b7-46d3-a0e4-715d56682277"
                            var LDays = 0.0;
                            for (int i = 0; i < Tempp.Count; i++)
                            {

                                if (Tempp[i].LeaveType == new Guid(lossofpayid.LOPId.ToString()) && Tempp[i].EmployeeId == new Guid(u.RefEntityId))//"199f5db2-14b7-46d3-a0e4-715d56682277"
                                {
                                    LDays = Convert.ToDouble(LDays) + Convert.ToDouble(Tempp[i].HFDay);
                                }
                            }

                            //added end


                            var payHist = payrollHistorylist.Where(s => s.EmployeeId == new Guid(u.RefEntityId) && s.Month == month && s.Year == year).FirstOrDefault();
                            string strStatus = "CanEdit";
                            if (!object.ReferenceEquals(payHist, null) && (payHist.Status == ComValue.payrollProcessStatus[0] || payHist.Status == ComValue.payrollProcessStatus[1]))
                                strStatus = "CanNotEdit";
                            jsonMonthlyInput temp = new jsonMonthlyInput();
                            temp.EmployeeId = new Guid(u.RefEntityId);
                            temp.EmployeeCode = seletedEmpl.Where(t => t.Id == new Guid(u.RefEntityId)).FirstOrDefault().EmployeeCode;//employee.Where(t => t.Id == new Guid(u.RefEntityId)).FirstOrDefault().FirstName;
                            temp.EmployeeName = seletedEmpl.Where(t => t.Id == new Guid(u.RefEntityId)).FirstOrDefault().FirstName + " " + seletedEmpl.Where(t => t.Id == new Guid(u.RefEntityId)).FirstOrDefault().LastName;//employee.Where(t => t.Id == new Guid(u.RefEntityId)).FirstOrDefault().FirstName;
                            temp.Month = month;
                            temp.year = year;
                            temp.Id = new Guid(u.EntityId);
                            temp.editStatus = strStatus;
                            temp.MonthlyInputAttributes = new List<MonthlyInputAttribute>();
                            seletedEntityBehavir.ForEach(p =>
                            {
                                var mon = monthlyinputlist.Where(s => s.AttributeModelId == p.AttributeModelId && s.Year == year && s.Month == month && s.EmployeeId == temp.EmployeeId && s.EntityId == new Guid(u.EntityId)).FirstOrDefault();
                                string miValue = mon != null ? mon.Value : "0";


                                if (p.AttributeModelId == AttrModel.Id)
                                {
                                    AttributeModel AttrMod = new AttributeModel();
                                    AttrMod = attributemodellist.Where(s => s.Name == "MD").FirstOrDefault();
                                    MonthlyInput MI = new MonthlyInput();
                                    var MInput = monthlyinputlist.Where(k => k.AttributeModelId == AttrMod.Id).FirstOrDefault();
                                    string MIValue = MInput != null ? MInput.Value : "0";
                                    // var Mdays = MI.Value != null ? MI.Value : "0";
                                    if (LdaysValue == "0" || LdaysValue == "")
                                    {
                                        //var TotalMdays = Convert.ToDecimal(miValue) + Convert.ToDecimal(LDays);--existing
                                        //change added
                                        double TotalMdays = 0;
                                        if (Convert.ToDecimal(miValue) == 0)
                                        {
                                            TotalMdays = Convert.ToDouble(LDays);
                                        }
                                        else
                                        {
                                            TotalMdays = Convert.ToDouble(miValue);
                                            //+ Convert.ToDouble(LDays);
                                        }
                                        //change end
                                        temp.MonthlyInputAttributes.Add(new MonthlyInputAttribute()
                                        {
                                            AttributeModId = p.AttributeModelId,
                                            Name = attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().DisplayAs,
                                            AttributeBehaviorType = (attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "PD" || attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "LD" || attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "MD") ? "others" : attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().BehaviorType.ToLower(),
                                            //MIValue = miValue + LDays
                                            MIValue = Convert.ToString(TotalMdays),
                                            MIDisplay = attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name,
                                        });
                                    }
                                    else
                                    {

                                        var TotalMdays = Convert.ToInt32(MIValue) + Convert.ToInt32(LDays);
                                        temp.MonthlyInputAttributes.Add(new MonthlyInputAttribute()
                                        {
                                            AttributeModId = p.AttributeModelId,
                                            Name = attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().DisplayAs,
                                            AttributeBehaviorType = (attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "PD" || attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "LD" || attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "MD") ? "others" : attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().BehaviorType.ToLower(),
                                            MIValue = Convert.ToString(TotalMdays),
                                            MIDisplay = attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name
                                        });
                                    }

                                }
                                else
                                {
                                    temp.MonthlyInputAttributes.Add(new MonthlyInputAttribute()
                                    {

                                        AttributeModId = p.AttributeModelId,
                                        Name = attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().DisplayAs,
                                        AttributeBehaviorType = (attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "PD" || attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "LD" || attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name == "MD") ? "others" : attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().BehaviorType.ToLower(),
                                        MIValue = miValue,
                                        MIDisplay = attributemodellist.Where(q => q.Id == p.AttributeModelId).FirstOrDefault().Name

                                    });
                                }
                            });
                            entity.Add(temp);
                        }
                    });
                }

            }


            entity.ForEach(f =>
            {
                previousComponentslist things = new previousComponentslist(categoryId, f.Id, new Guid(entitymodelId.ToString()));
                things.Where(w => w.MappedId != Guid.Empty).ToList().ForEach(t =>
                {
                    if (t.radio == "hide" || t.radio == "sne")
                    {
                        if (t.radio == "hide")
                        {
                            f.MonthlyInputAttributes.RemoveAll(r => r.AttributeModId == t.Id);
                        }

                        if (t.radio == "sne")
                        {
                            f.MonthlyInputAttributes.Where(r => r.AttributeModId == t.Id).ToList().ForEach(j => j.status = "sne");
                        }
                    }
                });

            });







            return entity;
        }
    }

    public class jsonEntity
    {
        public jsonEntity()
        {
            this.EntityKeyValues = new List<jsonEntityKeyValue>();
        }
        public Guid entityId { get; set; }
        public Guid entityModelId { get; set; }
        public List<jsonEntityKeyValue> EntityKeyValues { get; set; }
    }
    public class jsonEntityKeyValue
    {
        public string id { get; set; }
        public string name { get; set; }
        public string value { get; set; }

    }

    public class jsonFormula
    {
        /// <summary>
        /// Get or Set the AttrubuteModelId
        /// </summary>
        public Guid FormulaId { get; set; }

        /// <summary>
        /// Get or Set the CategoryId
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Get or Set the ValueType
        /// </summary>
        public int ValueType { get; set; }

        public string type { get; set; }

        /// <summary>
        /// Get or Set the Formula
        /// </summary>
        public string Formula { get; set; }

        public string hiddenform { get; set; }

        /// <summary>
        /// Get or Set the Percentage
        /// </summary>
        public decimal Percentage { get; set; }

        /// <summary>
        /// Get or Set the Maximum
        /// </summary>
        public decimal Maximum { get; set; }

        /// <summary>
        /// Get or Set the RoundingId
        /// </summary>
        public int RoundingId { get; set; }

        public string rounding { get; set; }
        public string name { get; set; }
        public string displayAs { get; set; }
    }

    public class jsonEntityBehavior
    {
        public Guid attrubuteModelId { get; set; }

        public Guid entityModelId { get; set; }

        public Guid entityId { get; set; }

        public int valueType { get; set; }

        /// <summary>
        /// Get or Set the Formula
        /// </summary>
        public string formula { get; set; }

        public string hiddenform { get; set; }

        /// <summary>
        /// Get or Set the Percentage
        /// </summary>
        public string percentage { get; set; }

        /// <summary>
        /// Get or Set the Maximum
        /// </summary>
        public string maximum { get; set; }

        public int roundingId { get; set; }

        public string name { get; set; }

        public string Displayname { get; set; }

        public string arrearMatchField { get; set; }

        public string arrearMatchfieldName { get; set; }

        public string eligibilityFormula { get; set; }

        public string hiddenEligibilityFormula { get; set; }

        public string contributionType { get; set; }

        public string defaultValue { get; set; }

        public string baseValue { get; set; }
        public string baseFormula { get; set; }

        public bool IsMonthlyInput { get; set; }
        public List<jsonEntityBehavior> childBehavior { get; set; }

        public static jsonEntityBehavior toJson(EntityBehavior data, AttributeModelList masterAttributeModelList)
        {
            string arrField = string.Empty;
            string arrFiledName = string.Empty;
            if (data.ArrearAttributeModelId != Guid.Empty)
            {
                arrField = data.ArrearAttributeModelId.ToString();
                arrFiledName = masterAttributeModelList.Where(u => u.Id == data.ArrearAttributeModelId).FirstOrDefault().Name;
            }
            List<jsonEntityBehavior> childBehaviors = new List<jsonEntityBehavior>();
            var Child = masterAttributeModelList.Where(u => u.ParentId == data.AttributeModelId);

            if (!object.ReferenceEquals(Child, null))
            {
                Child = Child.ToList();
                EntityBehaviorList childTemp = new EntityBehaviorList(data.EntityId, data.EntityModelId);
                Child.ToList().ForEach(u =>
                {
                    var temp = childTemp.Where(p => p.AttributeModelId == u.Id).FirstOrDefault();
                    if (!object.ReferenceEquals(temp, null))
                    {

                        childBehaviors.Add(jsonEntityBehavior.toJson(temp, masterAttributeModelList));
                    }
                    else
                    {
                        childBehaviors.Add(new jsonEntityBehavior()
                        {
                            entityId = data.EntityId,
                            attrubuteModelId = u.Id,
                            entityModelId = data.EntityModelId,
                            name = u.Name,
                            valueType = data.ValueType,
                            contributionType = u.ContributionType == 2 ? "Employeer" : "Employee",
                            percentage = data.Percentage,
                            Displayname = u.DisplayAs,
                            defaultValue = u.DefaultValue,
                        });

                    }
                });

            }
            var attributeMod = masterAttributeModelList.Where(u => u.Id == data.AttributeModelId).FirstOrDefault();
            //sharmi
            if (!string.IsNullOrEmpty(Convert.ToString(attributeMod)))
            {
                var defpercent = attributeMod.DefaultValue.Split('%');
                if (defpercent.Length > 1 && data.Percentage == null)
                {
                    data.Percentage = defpercent[1];
                }
            }
            jsonEntityBehavior retObj = new jsonEntityBehavior();
            retObj.childBehavior = childBehaviors;
            retObj.contributionType = attributeMod != null ? attributeMod.ContributionType == 2 ? "Employeer" : "Employee" : "Employee";
            retObj.attrubuteModelId = data.AttributeModelId;
            retObj.entityId = data.EntityId;
            retObj.entityModelId = data.EntityModelId;
            retObj.formula = ConvertFormula(masterAttributeModelList, data.Formula);
            // retObj.hiddenform = attributeMod.Name=="PF"?"":data.Formula; //Modifed
            retObj.hiddenform = data.Formula;
            retObj.maximum = data.Maximum;
            retObj.name = attributeMod != null ? attributeMod.Name : "";
            retObj.Displayname = attributeMod != null ? attributeMod.DisplayAs : "";
            //retObj.percentage = attributeMod.Name == "PF" ? data.Formula : data.Percentage;
            retObj.percentage = data.Percentage;
            retObj.roundingId = data.RoundingId;
            retObj.valueType = (data.ValueType == 0) ? (attributeMod.IsMonthlyInput == true ? 2 : 3) : data.ValueType;
            retObj.arrearMatchField = arrField;
            retObj.arrearMatchfieldName = arrFiledName;
            retObj.hiddenEligibilityFormula = data.EligibiltyFormula;
            retObj.eligibilityFormula = ConvertFormula(masterAttributeModelList, data.EligibiltyFormula);
            retObj.baseFormula = ConvertFormula(masterAttributeModelList, data.BaseFormula);
            retObj.baseValue = data.BaseValue;
            retObj.IsMonthlyInput = attributeMod.IsMonthlyInput;
            return retObj;
        }
        private static string ConvertFormula(AttributeModelList attrlist, string formula)
        {
            string tempFormula = string.Empty;
            if (!string.IsNullOrEmpty(formula))
            {
                if (formula.IndexOf('{') >= 0)
                {
                    do
                    {
                        int startIndex = formula.IndexOf('{');
                        int endIndex = formula.IndexOf('}');
                        string id = formula.Substring(startIndex + 1, endIndex - (startIndex + 1));
                        var tempAttrmodel = attrlist.Where(u => u.Id == new Guid(id)).FirstOrDefault();
                        if (!object.ReferenceEquals(tempAttrmodel, null))
                        {
                            formula = formula.Remove(startIndex, endIndex - (startIndex - 1));
                            formula = formula.Insert(startIndex, tempAttrmodel.Name);
                        }
                        else
                        {
                            formula = formula.Remove(startIndex, endIndex - (startIndex - 1));
                        }
                    } while (formula.IndexOf('{') >= 0);
                }
            }
            tempFormula = formula;
            if (!string.IsNullOrEmpty(tempFormula))
            {
                char lastchar = tempFormula[tempFormula.Length - 1];
                if (lastchar == '+' || lastchar == '-' || lastchar == '*' || lastchar == '/')
                    tempFormula = tempFormula.Remove(tempFormula.Length - 1, 1);
            }
            return tempFormula;
        }
    }

    public class jsonBABehavior
    {
        public Guid attrubuteModelId { get; set; }

        public Guid entityModelId { get; set; }

        public Guid entityId { get; set; }

        public int valueType { get; set; }

        /// <summary>
        /// Get or Set the Formula
        /// </summary>
        public string formula { get; set; }

        public string hiddenform { get; set; }

        /// <summary>
        /// Get or Set the Percentage
        /// </summary>
        public string percentage { get; set; }

        /// <summary>
        /// Get or Set the Maximum
        /// </summary>
        public string maximum { get; set; }

        public int roundingId { get; set; }

        public string name { get; set; }

        public string Displayname { get; set; }

        public string arrearMatchField { get; set; }

        public string arrearMatchfieldName { get; set; }

        public string eligibilityFormula { get; set; }

        public string hiddenEligibilityFormula { get; set; }

        public string contributionType { get; set; }

        public string defaultValue { get; set; }

        public string baseValue { get; set; }
        public string baseFormula { get; set; }

        public bool IsMonthlyInput { get; set; }
        public string CompType { get; set; }
        public List<jsonBABehavior> childBehavior { get; set; }

        public static jsonBABehavior toJson(BABehavior data, AttributeModelList masterAttributeModelList)
        {
            string arrField = string.Empty;
            string arrFiledName = string.Empty;
            if (data.ArrearAttributeModelId != Guid.Empty)
            {
                arrField = data.ArrearAttributeModelId.ToString();
                arrFiledName = masterAttributeModelList.Where(u => u.Id == data.ArrearAttributeModelId).FirstOrDefault().Name;
            }
            List<jsonBABehavior> childBehaviors = new List<jsonBABehavior>();
            var Child = masterAttributeModelList.Where(u => u.ParentId == data.AttributeModelId);

            if (!object.ReferenceEquals(Child, null))
            {
                Child = Child.ToList();
                BABehaviorList childTemp = new BABehaviorList(data.EntityId, data.EntityModelId);
                Child.ToList().ForEach(u =>
                {
                    var temp = childTemp.Where(p => p.AttributeModelId == u.Id).FirstOrDefault();
                    if (!object.ReferenceEquals(temp, null))
                    {

                        childBehaviors.Add(jsonBABehavior.toJson(temp, masterAttributeModelList));
                    }
                    else
                    {
                        childBehaviors.Add(new jsonBABehavior()
                        {
                            entityId = data.EntityId,
                            attrubuteModelId = u.Id,
                            entityModelId = data.EntityModelId,
                            name = u.Name,
                            valueType = data.ValueType,
                            contributionType = u.ContributionType == 2 ? "Employeer" : "Employee",
                            percentage = data.Percentage,
                            Displayname = u.DisplayAs,
                            defaultValue = u.DefaultValue,
                        });

                    }
                });

            }
            var attributeMod = masterAttributeModelList.Where(u => u.Id == data.AttributeModelId).FirstOrDefault();
            //sharmi
            if (!string.IsNullOrEmpty(Convert.ToString(attributeMod)))
            {
                var defpercent = attributeMod.DefaultValue.Split('%');
                if (defpercent.Length > 1 && data.Percentage == null)
                {
                    data.Percentage = defpercent[1];
                }
            }
            jsonBABehavior retObj = new jsonBABehavior();
            retObj.childBehavior = childBehaviors;
            retObj.contributionType = attributeMod != null ? attributeMod.ContributionType == 2 ? "Employeer" : "Employee" : "Employee";
            retObj.attrubuteModelId = data.AttributeModelId;
            retObj.entityId = data.EntityId;
            retObj.entityModelId = data.EntityModelId;
            retObj.formula = ConvertFormula(masterAttributeModelList, data.Formula);
            // retObj.hiddenform = attributeMod.Name=="PF"?"":data.Formula; //Modifed
            retObj.hiddenform = data.Formula;
            retObj.maximum = data.Maximum;
            retObj.name = attributeMod != null ? attributeMod.Name : "";
            retObj.Displayname = attributeMod != null ? attributeMod.DisplayAs : "";
            //retObj.percentage = attributeMod.Name == "PF" ? data.Formula : data.Percentage;
            retObj.percentage = data.Percentage;
            retObj.roundingId = data.RoundingId;
            retObj.valueType = (data.ValueType == 0) ? (attributeMod.IsMonthlyInput == true ? 2 : 3) : data.ValueType;
            retObj.arrearMatchField = arrField;
            retObj.arrearMatchfieldName = arrFiledName;
            retObj.hiddenEligibilityFormula = data.EligibiltyFormula;
            retObj.eligibilityFormula = ConvertFormula(masterAttributeModelList, data.EligibiltyFormula);
            retObj.baseFormula = ConvertFormula(masterAttributeModelList, data.BaseFormula);
            retObj.baseValue = data.BaseValue;
            retObj.IsMonthlyInput = attributeMod.IsMonthlyInput;
            return retObj;
        }
        private static string ConvertFormula(AttributeModelList attrlist, string formula)
        {
            string tempFormula = string.Empty;
            if (!string.IsNullOrEmpty(formula))
            {
                if (formula.IndexOf('{') >= 0)
                {
                    do
                    {
                        int startIndex = formula.IndexOf('{');
                        int endIndex = formula.IndexOf('}');
                        string id = formula.Substring(startIndex + 1, endIndex - (startIndex + 1));
                        var tempAttrmodel = attrlist.Where(u => u.Id == new Guid(id)).FirstOrDefault();
                        if (!object.ReferenceEquals(tempAttrmodel, null))
                        {
                            formula = formula.Remove(startIndex, endIndex - (startIndex - 1));
                            formula = formula.Insert(startIndex, tempAttrmodel.Name);
                        }
                        else
                        {
                            formula = formula.Remove(startIndex, endIndex - (startIndex - 1));
                        }
                    } while (formula.IndexOf('{') >= 0);
                }
            }
            tempFormula = formula;
            if (!string.IsNullOrEmpty(tempFormula))
            {
                char lastchar = tempFormula[tempFormula.Length - 1];
                if (lastchar == '+' || lastchar == '-' || lastchar == '*' || lastchar == '/')
                    tempFormula = tempFormula.Remove(tempFormula.Length - 1, 1);
            }
            return tempFormula;
        }
    }


    public class jsonEntityModelMap
    {
        public Guid id { get; set; }

        public string entityModelId { get; set; }

        public string refEntityModelId { get; set; }

        public string DisplayAs { get; set; }

        public List<EntityTemp> seletedEntity { get; set; }

        public List<EntityTemp> entityCollection { get; set; }

        public static jsonEntityModelMap toJson(EntityModelMapping data, EntityModelList masterAttributeModelList, string refEntityId, string refEntityModelName)
        {
            string dispalAs = string.Empty;
            Guid newGuid;
            if (Guid.TryParse(data.EntityTableName, out newGuid))
                dispalAs = (masterAttributeModelList.Where(u => u.Id == newGuid).FirstOrDefault()) != null ? masterAttributeModelList.Where(u => u.Id == newGuid).FirstOrDefault().Name : "";
            else
                dispalAs = data.EntityTableName;

            var retobject = new jsonEntityModelMap()
            {
                id = data.Id,
                entityModelId = data.EntityTableName,
                refEntityModelId = data.RefEntityModelName,
                DisplayAs = dispalAs,
                seletedEntity = new List<EntityTemp>(),
                entityCollection = new List<EntityTemp>()

            };
            EntityList entlist = new EntityList(newGuid);
            entlist.ForEach(u => { retobject.entityCollection.Add(EntityTemp.toJson(u)); });
            EntityMappingList entMaplist = new EntityMappingList(refEntityModelName);

            entMaplist.Where(u => Convert.ToString(u.RefEntityId).ToUpper() == refEntityId.ToUpper()).ToList().ForEach(v =>
            {
                Entity test = entlist.Where(s => s.Id.ToString().ToUpper() == v.EntityId.ToString().ToUpper()).FirstOrDefault();
                retobject.seletedEntity.Add(EntityTemp.toJson(test));
            });

            return retobject;
        }

    }
    public class EntityTemp
    {
        public string entityId { get; set; }
        public string entityName { get; set; }

        public static EntityTemp toJson(Entity entity)
        {
            if (entity == null)
                return new EntityTemp();
            return new EntityTemp()
            {
                entityId = Convert.ToString(entity.Id),
                entityName = entity.Name
            };
        }
    }

    public class jsonEntityMap
    {
        public Guid entityId { get; set; }

        public Guid entityModelId { get; set; }
        public Guid refEntityId { get; set; }
        public string refEntitymodelId { get; set; }



    }
    public class jsonEntityAddtionalInfo
    {
        public Guid entitymodelId { get; set; }

        public Guid employeeId { get; set; }
        public Guid attributeId { get; set; }
        public string value { get; set; }

        public Guid refEntityId { get; set; }

    }
    public class FormulaValueType
    {
        private static List<FormulaValueType> _formulaValueTypeList;
        public int Id { get; set; }

        public string Name { get; set; }

        public FormulaValueType()
        {

        }

        public static List<FormulaValueType> FormulaValueTypes()
        {
            if (object.ReferenceEquals(_formulaValueTypeList, null))
            {
                _formulaValueTypeList = new List<FormulaValueType>();
                _formulaValueTypeList.Add(new FormulaValueType { Id = 1, Name = "Direct" });
                _formulaValueTypeList.Add(new FormulaValueType { Id = 2, Name = "Monthly Input" });
                _formulaValueTypeList.Add(new FormulaValueType { Id = 3, Name = "Percentage" });
                _formulaValueTypeList.Add(new FormulaValueType { Id = 4, Name = "Conditional" });
                _formulaValueTypeList.Add(new FormulaValueType { Id = 5, Name = "Range" });
            }
            return _formulaValueTypeList;
        }
    }

    public class Rounding
    {
        private static List<Rounding> _roundingList;
        public int Id { get; set; }

        public string Name { get; set; }

        private Rounding()
        {

        }

        public static List<Rounding> Roundings()
        {
            if (object.ReferenceEquals(_roundingList, null))
            {
                _roundingList = new List<Rounding>();
                _roundingList.Add(new Rounding { Id = 1, Name = "NORMAL" });
                _roundingList.Add(new Rounding { Id = 2, Name = ">1RUPEE" });
                _roundingList.Add(new Rounding { Id = 3, Name = "<1RUPEE" });
                _roundingList.Add(new Rounding { Id = 4, Name = "50 PAISE" });
                _roundingList.Add(new Rounding { Id = 5, Name = ">50 PAISE" });
                _roundingList.Add(new Rounding { Id = 6, Name = "<50 PAISE" });
                _roundingList.Add(new Rounding { Id = 7, Name = "10 PAISE" });
                _roundingList.Add(new Rounding { Id = 8, Name = ">10 PAISE" });
                _roundingList.Add(new Rounding { Id = 9, Name = "5 PAISE" });
                _roundingList.Add(new Rounding { Id = 10, Name = ">5 PAISE" });
                _roundingList.Add(new Rounding { Id = 11, Name = "5 RUPEES" });
                _roundingList.Add(new Rounding { Id = 12, Name = ">5 RUPEES" });
            }
            return _roundingList;
        }
    }

    public class jsonMonthlyinputList
    {
        public jsonMonthlyinputList()
        {
            jsonMonthlyInput = new List<jsonMonthlyInput>();
        }
        public Guid EntityId { get; set; }

        public string EntityModelName { get; set; }

        public List<jsonMonthlyInput> jsonMonthlyInput { get; set; }
    }
    public class jsonMonthlyInput
    {
        public jsonMonthlyInput()
        {
            MonthlyInputAttributes = new List<MonthlyInputAttribute>();
        }

        public Guid EmployeeId { get; set; }

        public int Month { get; set; }

        public int year { get; set; }

        public Guid Id { get; set; }

        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string editStatus { get; set; }

        public List<MonthlyInputAttribute> MonthlyInputAttributes { get; set; }

        public static List<MonthlyInput> ConvertObject(jsonMonthlyInput data, Guid entityModelId)
        {
            List<MonthlyInput> retObject = new List<MonthlyInput>();
            data.MonthlyInputAttributes.ForEach(u =>
            {
                retObject.Add(new MonthlyInput()
                {
                    AttributeModelId = u.AttributeModId,
                    EmployeeId = data.EmployeeId,
                    EntityId = data.Id,
                    Year = data.year,
                    Month = data.Month,
                    Value = u.MIValue,
                    EntityModelId = entityModelId
                });
            });
            return retObject;
        }

        public static jsonMonthlyInput toJson(List<MonthlyInput> data)
        {
            jsonMonthlyInput retObject = new jsonMonthlyInput();
            retObject.EmployeeId = data[0].EmployeeId;
            retObject.EmployeeName = "Tes";
            retObject.EmployeeCode = "Tes";
            retObject.Id = data[0].EntityId;
            retObject.Month = data[0].Month;
            data.ForEach(u =>
            {
                retObject.MonthlyInputAttributes.Add(new MonthlyInputAttribute()
                {
                    AttributeModId = u.AttributeModelId,
                    MIValue = u.Value,
                    Name = "Field2",
                    Code = "Field1"

                });
            });
            return retObject;
        }
    }
    public class MonthlyInputAttribute
    {
        public Guid AttributeModId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string AttributeBehaviorType { get; set; }
        public string MIValue { get; set; }
        public string MIDisplay { get; set; }

        public string status { get; set; }
    }


    //public class previousComponents
    //{
    //    public Guid Id { get; set; }
    //    public string Name { get; set; }
    //    public string MappedColumn { get; set; }
    //    public Guid MappedId { get; set; }

    //    public string radio { get; set; }
    //    public List<newComponents> attr { get; set; }
    //public previousComponents()
    //    {

    //    }
    //}

    //public class newComponents
    //{
    //    public Guid Id { get; set; }
    //    public string Name { get; set; }
    //    public newComponents()
    //    {

    //    }
    //}



    public class MasterInputEarnDedSettings
    {
        public Guid EntitymodelId { get; set; }
        public Guid AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeBehaviorType { get; set; }
        public string AttributeInputType { get; set; }
        public bool IsVisible { get; set; }

        public static MasterInputEarnDedSettings tojson(EntityAttributeModel attr, EntityBehavior beh, bool isVisible = false)
        {
            return new MasterInputEarnDedSettings()
            {
                EntitymodelId = attr.EntityModelId,
                AttributeId = attr.AttributeModelId,
                AttributeName = attr.AttributeModel.Name,
                AttributeBehaviorType = attr.AttributeModel.BehaviorType,
                AttributeInputType = beh != null ? Convert.ToString(beh.ValueType) : "",
                IsVisible = isVisible
            };
        }
    }

    public class jsonPayroll
    {
        public Guid employeeId { get; set; }

        public string employeeCode { get; set; }

        public Guid payrollId { get; set; }

        public string employeeName { get; set; }

        public string status { get; set; }

        public int month { get; set; }

        public int year { get; set; }

        public static jsonPayroll toJson(PayrollHistory data, EmployeeList employeelist, PayrollHistoryList payrollHistory)
        {
            var emp = employeelist.Where(u => u.Id == data.EmployeeId).FirstOrDefault();
            //var pay = payrollHistory.Where(u => u.);
            jsonPayroll retObject = new jsonPayroll();
            retObject.employeeCode = emp != null ? emp.EmployeeCode : "";
            retObject.employeeId = data.EmployeeId;
            retObject.employeeName = emp != null ? emp.FirstName + " " + emp.LastName : "";
            retObject.month = 1;
            retObject.payrollId = data.Id;
            retObject.status = data.Status;
            retObject.year = 1;
            return retObject;
        }

    }
    public class jsonMasterValForEmp
    {
        public Guid id { get; set; }
        //public Guid entityid { get; set; }
        public Guid entityModelId { get; set; }
        public string refEntityModelId { get; set; }
        public Guid employeeId { get; set; }
        public Guid attributeId { get; set; }
        public List<flexiComponent> ComponentKeyVal { get; set; }

        public Guid refEntityId { get; set; }

    }
    public class flexiComponent
    {
        public string componet { get; set; }
        public decimal PerAnnum { get; set; }
        public decimal PerMonth { get; set; }


    }

}
