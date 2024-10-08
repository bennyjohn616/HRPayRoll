using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class PF
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public PF()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public PF(int id, int companyId)
        {

        }

        #endregion

        #region property

        /// <summary>
        /// get or set the Esi Gross amount
        /// </summary>
        public double EsiGrossAmt { get; set; }

        /// <summary>
        /// get or set the eligibility amount
        /// </summary>
        public double EligibilityAmt { get; set; }

        /// <summary>
        /// Get or Set the Name
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Get or Set the DisplayAs
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Get or Set the ParentId
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Get or Set the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Get or Set the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }

        /// <summary>
        /// Get or Set the ModifiedOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        #endregion

        #region Public methods

        public void CalculatePF(TaxComputationInfo taxInfo,Employee emp, double dresult, ref FormulaRecursive inp, ref List<FormulaRecursive> lstCollection, string PFPercent, Entity entity)
        {
            PF pf = new PF();
     //       Employee emp = new Employee(inp.CompanyId, inp.EmployeeId);
            Category cate = new Category(emp.CategoryId, inp.CompanyId);
            Emp_Personal Emppers = new Emp_Personal(inp.EmployeeId);
            if (string.IsNullOrEmpty(cate.PFProcess))//check the is empty
            {
                cate.PFProcess = "GROSS";
            }
            double grossAmt = dresult;
            double limitAmt = 0;
            string settingBase = string.Empty;
            settingBase = cate.PFProcess.ToUpper();
            limitAmt = Convert.ToDouble(cate.PFLimit);



            //For EPF Calculation
            string FPFValue = string.Empty;
            string PFPercentage = PFPercent;

            //For SENIOR CITIZEN FPF & EPF Calculation
            string SENIOR_FPF = string.Empty;
            string SENIOR_EPF = string.Empty;
            bool isSeniorCitizen = false;

            string SeniorCitizen_Id = string.Empty;
            string SENIOR_FPF_Id = string.Empty;
            string SENIOR_EPF_Id = string.Empty;
            int rounding = inp.Rounding;
            ////update the child elements
            string id = inp.Id;
            lstCollection.Where(p => p.ParentId == id).ToList().ForEach(s =>
            {
                if (s.Name.ToUpper().Trim() == "PFSNRCITZAGE")
                {
                    if (emp.DateOfBirth.AddYears(Convert.ToInt32(s.Percentage)) <= taxInfo.FinanceYear.EndingDate)
                    {
                        isSeniorCitizen = true;

                    }
                    else
                    {
                        isSeniorCitizen = false;
                    }
                    SeniorCitizen_Id = s.Id;
                    s.Assignedvalues = PFPercentage == "0" ? "0" : s.Percentage;
                    s.Percentage = "100";

                }
                else if (s.Name.ToUpper().Trim() == "PFSNRFPF")
                {
                    SENIOR_FPF = PFPercentage == "0" ? "0" : s.Percentage;
                    SENIOR_FPF_Id = s.Id;
                }
                else if (s.Name.ToUpper().Trim() == "PFSNREPF")
                {
                    SENIOR_EPF = PFPercentage == "0" ? "0" : s.Percentage;
                    SENIOR_EPF_Id = s.Id;
                }
            });

            lstCollection.Where(p => p.ParentId == id).ToList().ForEach(s =>
            {
                double employeerPercntge;
                string strUptPut = string.Empty;
                if (settingBase.ToUpper().Trim() == "LIMIT" || settingBase.ToUpper().Trim() == "BOTH")
                {
                    strUptPut = limitAmt.ToString();
                    if (dresult <= limitAmt)//if gross amount is less than limit,Taking a min value for employer contribution
                    {
                        strUptPut = dresult.ToString();
                    }
                }
                else
                {
                    strUptPut = grossAmt.ToString();
                }
                if (s.Name.ToUpper().Trim() == "PFADM")
                {
                    if (!object.ReferenceEquals(cate.PFAdminChargeProcess, null) && cate.PFAdminChargeProcess.ToUpper().Trim() == "GROSS")
                    {
                        strUptPut = grossAmt.ToString();
                    }
                    else
                    {
                        strUptPut = Math.Min(limitAmt, grossAmt).ToString();
                    }
                }
                else if (s.Name.ToUpper().Trim() == "PFEDLI")
                {
                    if (!object.ReferenceEquals(cate.PFEdliChargeProcess, null) && cate.PFEdliChargeProcess.ToUpper().Trim() == "GROSS")
                    {
                        strUptPut = grossAmt.ToString();
                    }
                    else
                    {
                        strUptPut = Math.Min(limitAmt, grossAmt).ToString();
                    }

                }
                else if (s.Name.ToUpper().Trim() == "PFINSPECTION")
                {
                    if (!object.ReferenceEquals(cate.PFInspectionChargeProcess, null) && cate.PFInspectionChargeProcess.ToUpper().Trim() == "GROSS")
                    {
                        strUptPut = grossAmt.ToString();
                    }
                    else
                    {
                        strUptPut = Math.Min(limitAmt, grossAmt).ToString();
                    }
                }
                else if (s.Name.ToUpper().Trim() == "FPF")
                {
                    if (!object.ReferenceEquals(cate.PFFPFProcess, null) && cate.PFFPFProcess.ToUpper().Trim() == "GROSS")
                    {
                        strUptPut = grossAmt.ToString();

                    }
                    else
                    {
                        strUptPut = Math.Min(limitAmt, grossAmt).ToString();
                    }

                }
                else if (s.Name.ToUpper().Trim() == "EPF")
                {
                    if (!object.ReferenceEquals(cate.PFProcess, null) && cate.PFProcess.ToUpper().Trim() == "GROSS")
                    {
                        strUptPut = grossAmt.ToString();

                    }
                    else
                    {
                        strUptPut = Math.Min(limitAmt, grossAmt).ToString();
                    }

                }

                if (s.Name.ToUpper().Trim() == "FPF")
                {
                    string employeerPfAmount = strUptPut;
                    if (Emppers.PensionEligible == 1)
                    {
                        if (!string.IsNullOrEmpty(isSeniorCitizen == false ? s.Percentage : SENIOR_FPF) && double.TryParse(isSeniorCitizen == false ? s.Percentage : SENIOR_FPF, out employeerPercntge))
                        {
                            employeerPfAmount = (Convert.ToDouble(strUptPut) * employeerPercntge / 100).ToString("0.00");

                        }
                    }
                    else
                    {
                        employeerPfAmount = "0";
                    }
                    if (PFPercent == "0")
                    {
                        employeerPfAmount = "0.00";
                    }
                    s.Output = employeerPfAmount;
                    s.Assignedvalues = employeerPfAmount;
                    s.Percentage = "";
                    s.BaseValue = strUptPut;

                }
                else
                {
                    string employeerPfAmount = strUptPut;
                    if (!string.IsNullOrEmpty(s.Percentage) && double.TryParse(s.Percentage, out employeerPercntge))
                    {
                        employeerPfAmount = (Convert.ToDouble(strUptPut) * employeerPercntge / 100).ToString("0.00");

                    }
                    if (s.Name.ToUpper().Trim() != "PFSNRCITZAGE")
                    {
                        if (PFPercent == "0")
                        {
                            employeerPfAmount = "0.00";
                        }
                    }
                    s.Output = employeerPfAmount;
                    s.Assignedvalues = employeerPfAmount;
                    s.Percentage = "";
                    s.BaseValue = strUptPut;
                }
                s.Output = Convert.ToString(new FormulaRecursive().RoundOff(Convert.ToDouble(s.Output), rounding));
                if (s.Name.ToUpper().Trim() == "FPF")
                {
                    FPFValue = s.Output;
                }
            });

            lstCollection.Where(p => p.ParentId == id).ToList().ForEach(s =>
            {
                double employeerPercntge;
                string strUptPut = string.Empty;
                if (s.Name.ToUpper().Trim() == "EPF")
                {
                    if (cate.PFProcess.ToUpper().Trim() == "GROSS")
                    {
                        strUptPut = grossAmt.ToString();

                    }
                    else
                    {
                        strUptPut = Math.Min(limitAmt, grossAmt).ToString();
                    }

                    string employeerPfAmount = strUptPut;

                    if (!string.IsNullOrEmpty(isSeniorCitizen == false ? PFPercentage : SENIOR_EPF) && double.TryParse(isSeniorCitizen == false ? PFPercentage : SENIOR_EPF, out employeerPercntge))
                    {
                        employeerPfAmount = ((Convert.ToDouble(strUptPut) * employeerPercntge / 100) - Convert.ToDouble(FPFValue)).ToString("0.00");
                    }
                    if (PFPercent == "0")
                    {
                        employeerPfAmount = "0.00";
                    }
                    s.Output = employeerPfAmount;
                    s.Assignedvalues = employeerPfAmount;
                    s.Percentage = "";

                }
                else if (s.Name.ToUpper().Trim() == "EDLIW")
                {
                    double LimitAmount = 15000;
                    if (grossAmt > LimitAmount)
                    {
                        strUptPut = LimitAmount.ToString();
                    }
                    else
                    {
                        strUptPut = grossAmt.ToString();
                    }
                    if (PFPercent == "0")
                    {
                        strUptPut = "0.00";
                    }
                    s.Output = strUptPut;
                    s.Assignedvalues = strUptPut;
                    s.Percentage = "";
                }
                else if (s.Name.ToUpper().Trim() == "EPSW")
                {
                    double amtempty = 0;
                    if (Emppers.PensionEligible == 1)
                    {
                        double LimitAmount = 15000;
                        if (grossAmt > LimitAmount)
                        {
                            strUptPut = LimitAmount.ToString();
                        }
                        else
                        {
                            strUptPut = grossAmt.ToString();
                        }
                    }
                    else
                    {
                        // strUptPut = Math.Min(limitAmt, grossAmt).ToString();
                        strUptPut = amtempty.ToString();
                    }
                    if (PFPercent == "0")
                    {
                        strUptPut = "0.00";
                    }
                    s.Output = strUptPut;
                    s.Assignedvalues = strUptPut;
                    s.Percentage = "";
                }

            });

            //update the PF 
            if (settingBase.ToUpper().Trim() == "LIMIT")
            {
                grossAmt = Math.Min(limitAmt, grossAmt);
            }

            inp.Output = grossAmt.ToString();
            dresult = grossAmt;
            if (settingBase.ToUpper().Trim() == "BOTH")
            {
                //             grossAmt = Math.Min(limitAmt, grossAmt);
                inp.Output = grossAmt.ToString();
            }

            //Remove Unwanted Component
            RemoveEntity(entity, SeniorCitizen_Id); //Senior Citizen Age
            RemoveEntity(entity, SENIOR_FPF_Id); //Senior Citizen FPF
            RemoveEntity(entity, SENIOR_EPF_Id); //Senior Citizen EPF

        }

        public void CalculateArrearPF(double dresult, ref FormulaRecursive inp, ref List<FormulaRecursive> lstCollection, string PFPercent, Entity entity)
        {
            if (Convert.ToDecimal(lstCollection.Where(l => l.Name == "AB").FirstOrDefault().Assignedvalues) > 0)
            {


                PF pf = new PF();
                Employee emp = new Employee(inp.CompanyId, inp.EmployeeId);
                Category cate = new Category(emp.CategoryId, inp.CompanyId);
                Emp_Personal Emppers = new Emp_Personal(inp.EmployeeId);
                if (string.IsNullOrEmpty(cate.PFProcess))//check the is empty
                {
                    cate.PFProcess = "GROSS";
                }
                double grossAmt = dresult;
                double limitAmt = 0;
                string settingBase = string.Empty;
                settingBase = cate.PFProcess.ToUpper();
                limitAmt = Convert.ToDouble(cate.PFLimit);



                //For EPF Calculation
                string FPFValue = string.Empty;
                string PFPercentage = PFPercent;

                //For SENIOR CITIZEN FPF & EPF Calculation
                string SENIOR_FPF = string.Empty;
                string SENIOR_EPF = string.Empty;
                bool isSeniorCitizen = false;

                string SeniorCitizen_Id = string.Empty;
                string SENIOR_FPF_Id = string.Empty;
                string SENIOR_EPF_Id = string.Empty;

                ////update the child elements
                string id = inp.Id;
                lstCollection.Where(p => p.ParentId == id).ToList().ForEach(s =>
                {
                    if (s.Name.ToUpper().Trim() == "PFSNRCITZAGE")
                    {
                        if (emp.DateOfBirth.AddYears(Convert.ToInt32(s.Percentage)) <= DateTime.Now)
                        {
                            isSeniorCitizen = true;

                        }
                        else
                        {
                            isSeniorCitizen = false;
                        }
                        SeniorCitizen_Id = s.Id;
                    }
                    else if (s.Name.ToUpper().Trim() == "PFSNRFPF")
                    {
                        SENIOR_FPF = PFPercentage == "0" ? "0" : s.Percentage;
                        SENIOR_FPF_Id = s.Id;
                    }
                    else if (s.Name.ToUpper().Trim() == "PFSNREPF")
                    {
                        SENIOR_EPF = PFPercentage == "0" ? "0" : s.Percentage;
                        SENIOR_EPF_Id = s.Id;
                    }
                });

                lstCollection.Where(p => p.ParentId == id).ToList().ForEach(s =>
                {

                    double employeerPercntge;
                    string strUptPut = string.Empty;
                    if (settingBase.ToUpper().Trim() == "LIMIT" || settingBase.ToUpper().Trim() == "BOTH")
                    {
                        strUptPut = limitAmt.ToString();
                        if (dresult <= limitAmt)//if gross amount is less than limit,Taking a min value for employer contribution
                        {
                            strUptPut = dresult.ToString();
                        }
                    }
                    else
                    {
                        strUptPut = grossAmt.ToString();
                    }
                    if (s.Name.ToUpper().Trim() == "APFADM")
                    {
                        if (!object.ReferenceEquals(cate.PFAdminChargeProcess, null) && cate.PFAdminChargeProcess.ToUpper().Trim() == "GROSS")
                        {
                            strUptPut = grossAmt.ToString();
                        }
                        else
                        {
                            strUptPut = Math.Min(limitAmt, grossAmt).ToString();
                        }
                    }
                    else if (s.Name.ToUpper().Trim() == "APFEDLI")
                    {
                        if (!object.ReferenceEquals(cate.PFEdliChargeProcess, null) && cate.PFEdliChargeProcess.ToUpper().Trim() == "GROSS")
                        {
                            strUptPut = grossAmt.ToString();
                        }
                        else
                        {
                            strUptPut = Math.Min(limitAmt, grossAmt).ToString();
                        }

                    }
                    else if (s.Name.ToUpper().Trim() == "APFINSPECTION")
                    {
                        if (!object.ReferenceEquals(cate.PFInspectionChargeProcess, null) && cate.PFInspectionChargeProcess.ToUpper().Trim() == "GROSS")
                        {
                            strUptPut = grossAmt.ToString();
                        }
                        else
                        {
                            strUptPut = Math.Min(limitAmt, grossAmt).ToString();
                        }
                    }
                    else if (s.Name.ToUpper().Trim() == "AFPF")
                    {
                        if (!object.ReferenceEquals(cate.PFFPFProcess, null) && cate.PFFPFProcess.ToUpper().Trim() == "GROSS")
                        {
                            strUptPut = grossAmt.ToString();

                        }
                        else
                        {
                            strUptPut = Math.Min(limitAmt, grossAmt).ToString();
                        }

                    }
                    else if (s.Name.ToUpper().Trim() == "AEPF")
                    {
                        if (!object.ReferenceEquals(cate.PFProcess, null) && cate.PFProcess.ToUpper().Trim() == "GROSS")
                        {
                            strUptPut = grossAmt.ToString();

                        }
                        else
                        {
                            strUptPut = Math.Min(limitAmt, grossAmt).ToString();
                        }

                    }

                    if (s.Name.ToUpper().Trim() == "AFPF")
                    {
                        string employeerPfAmount = strUptPut;
                        if (Emppers.PensionEligible == 1)
                        {
                            if (!string.IsNullOrEmpty(isSeniorCitizen == false ? s.Percentage : SENIOR_FPF) && double.TryParse(isSeniorCitizen == false ? s.Percentage : SENIOR_FPF, out employeerPercntge))
                            {
                                employeerPfAmount = (Convert.ToDouble(strUptPut) * employeerPercntge / 100).ToString("0.00");

                            }
                        }
                        else
                        {
                            employeerPfAmount = "0";
                        }
                        if (PFPercent == "0")
                        {
                            employeerPfAmount = "0.00";
                        }
                        s.Output = employeerPfAmount;
                        s.Assignedvalues = employeerPfAmount;
                        s.Percentage = "";
                        s.BaseValue = strUptPut;
                        FPFValue = s.Output;
                    }
                    else
                    {
                        string employeerPfAmount = strUptPut;
                        if (!string.IsNullOrEmpty(s.Percentage) && double.TryParse(s.Percentage, out employeerPercntge))
                        {
                            employeerPfAmount = (Convert.ToDouble(strUptPut) * employeerPercntge / 100).ToString("0.00");

                        }
                        s.Output = employeerPfAmount;
                        s.Assignedvalues = employeerPfAmount;
                        s.Percentage = "";
                        s.BaseValue = strUptPut;
                    }
                });

                lstCollection.Where(p => p.ParentId == id).ToList().ForEach(s =>
                {
                    double employeerPercntge;
                    string strUptPut = string.Empty;
                    if (s.Name.ToUpper().Trim() == "AEPF")
                    {
                        if (cate.PFProcess.ToUpper().Trim() == "GROSS")
                        {
                            strUptPut = grossAmt.ToString();

                        }
                        else
                        {
                            strUptPut = Math.Min(limitAmt, grossAmt).ToString();
                        }

                        string employeerPfAmount = strUptPut;

                        if (!string.IsNullOrEmpty(isSeniorCitizen == false ? PFPercentage : SENIOR_EPF) && double.TryParse(isSeniorCitizen == false ? PFPercentage : SENIOR_EPF, out employeerPercntge))
                        {
                            employeerPfAmount = ((Convert.ToDouble(strUptPut) * employeerPercntge / 100) - Convert.ToDouble(FPFValue)).ToString("0.00");
                        }
                        if (PFPercent == "0")
                        {
                            employeerPfAmount = "0.00";
                        }
                        s.Output = employeerPfAmount;
                        s.Assignedvalues = employeerPfAmount;
                        s.Percentage = "";

                    }
                    else if (s.Name.ToUpper().Trim() == "AEDLIW")
                    {
                        double LimitAmount = 15000;
                        if (grossAmt > LimitAmount)
                        {
                            strUptPut = LimitAmount.ToString();
                        }
                        else
                        {
                            strUptPut = grossAmt.ToString();
                        }
                        s.Output = strUptPut;
                        s.Assignedvalues = strUptPut;
                        s.Percentage = "";
                    }
                    else if (s.Name.ToUpper().Trim() == "AEPSW")
                    {
                        double amtempty = 0;
                        if (Emppers.PensionEligible == 1)
                        {
                            double LimitAmount = 15000;
                            if (grossAmt > LimitAmount)
                            {
                                strUptPut = LimitAmount.ToString();
                            }
                            else
                            {
                                strUptPut = grossAmt.ToString();
                            }
                        }
                        else
                        {
                            // strUptPut = Math.Min(limitAmt, grossAmt).ToString();
                            strUptPut = amtempty.ToString();
                        }
                        s.Output = strUptPut;
                        s.Assignedvalues = strUptPut;
                        s.Percentage = "";
                    }

                });
                //lstCollection.Where(p => p.ParentId == id).ToList().ForEach(s =>
                //{
                //    double employeerPercntge;
                //    string strUptPut = string.Empty;
                //    if (s.Name.ToUpper().Trim() == "EPF")
                //    {
                //        if (cate.PFProcess.ToUpper().Trim() == "GROSS")
                //        {
                //            strUptPut = grossAmt.ToString();

                //        }
                //        else
                //        {
                //            strUptPut = Math.Min(limitAmt, grossAmt).ToString();
                //        }

                //        string employeerPfAmount = strUptPut;

                //        if (!string.IsNullOrEmpty(isSeniorCitizen == false ? PFPercentage : SENIOR_EPF) && double.TryParse(isSeniorCitizen == false ? PFPercentage : SENIOR_EPF, out employeerPercntge))
                //        {
                //            employeerPfAmount = ((Convert.ToDouble(strUptPut) * employeerPercntge / 100) - Convert.ToDouble(FPFValue)).ToString("0.00");
                //        }
                //        s.Output = employeerPfAmount;
                //        s.Assignedvalues = employeerPfAmount;
                //        s.Percentage = "";

                //    }

                //});

                //update the PF 
                if (settingBase.ToUpper().Trim() == "LIMIT")
                {
                    grossAmt = Math.Min(limitAmt, grossAmt);
                }

                inp.Output = grossAmt.ToString();
                dresult = grossAmt;
                if (settingBase.ToUpper().Trim() == "BOTH")
                {
                    //             grossAmt = Math.Min(limitAmt, grossAmt);
                    inp.Output = grossAmt.ToString();
                }

                //Remove Unwanted Component
                RemoveEntity(entity, SeniorCitizen_Id); //Senior Citizen Age
                RemoveEntity(entity, SENIOR_FPF_Id); //Senior Citizen FPF
                RemoveEntity(entity, SENIOR_EPF_Id); //Senior Citizen EPF
            }
        }

        public void RemoveEntity(Entity entity, string remove_Id)
        {
            if (remove_Id != "")
            {
                var Removeitem = entity.EntityAttributeModelList.FirstOrDefault(u => u.AttributeModelId == new Guid(Convert.ToString(remove_Id)));
                entity.EntityAttributeModelList.Remove(Removeitem);
            }
        }

        public string GetProjection(AttributeModel pfAttr, Employee employee, TaxComputationInfo taxInfo)
        {
            string outExpr = string.Empty;
            var baseAttr = employee.EntityBehaviorList.Where(b => b.AttributeModelId == pfAttr.Id).FirstOrDefault();
            EntityMasterValueList entityMasterValus = new EntityMasterValueList(employee.Id, ComValue.EmployeeTable);
            var tempAttVal = entityMasterValus.Where(x => x.AttributeModelId == pfAttr.Id).FirstOrDefault();
            baseAttr.Percentage = tempAttVal != null ? tempAttVal.Value : baseAttr.Percentage;
            string[] baseid = baseAttr.Formula.Replace('+', '*').Replace('/', '*').Replace('(', '*').Replace(')', '*').Replace('{', '*').Replace('}', '*').Split('*');
            foreach (string id in baseid)
            {
                Guid gu;
                if (Guid.TryParse(id, out gu))
                {
                    // TXFinanceYear txfinance = new TXFinanceYear(Guid.Empty, pfAttr.CompanyId, true);
                    TXFinanceYear txfinance = taxInfo.FinanceYear;
                    IncomeMatching im = new IncomeMatching(txfinance.Id, new Guid(id));

                    if (!ReferenceEquals(im, null))
                    {
                        //matching component value is empty take attr id  by Muthu 
                        Guid matchingId = im.MatchingComponent == Guid.Empty ? im.AttributemodelId : im.MatchingComponent;
                        var payrollhistryvalue = taxInfo.entity.EntityAttributeModelList.Where(h => h.AttributeModelId == matchingId).FirstOrDefault();
                        string val = payrollhistryvalue == null ? "0" : payrollhistryvalue.EntityAttributeValue.Value;

                        outExpr = outExpr + val + "+";
                    }
                }
            }

            outExpr = outExpr.TrimEnd('+');
            Eval eval = new Eval();
            outExpr = eval.Execute(outExpr);


            Category cate = new Category(employee.CategoryId, employee.CompanyId);
            string settingBase = string.Empty;
            settingBase = cate.PFProcess.ToUpper();
            string prjctPF = outExpr;
            decimal limitAmt = Convert.ToDecimal(cate.PFLimit);
            if (settingBase.ToUpper().Trim() == "LIMIT" || settingBase.ToUpper().Trim() == "BOTH")
            {
                prjctPF = limitAmt.ToString();
                if (Convert.ToDecimal(outExpr) <= limitAmt)//if gross amount is less than limit,Taking a min value for employer contribution
                {
                    prjctPF = outExpr;
                }
            }


            if (baseAttr.Percentage != string.Empty)
            {
                outExpr = prjctPF + "*" + baseAttr.Percentage + "/100";

            }

            return new FormulaRecursive().RoundOff(Convert.ToDouble(eval.Execute(outExpr)), baseAttr.RoundingId).ToString();
        }

        #endregion

        #region private methods





        #endregion
    }
}
