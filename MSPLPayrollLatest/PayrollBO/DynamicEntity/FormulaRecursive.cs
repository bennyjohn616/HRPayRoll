using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class FormulaRecursive
    {
        //public Guid SuppliedID { get; set; }
        //public Decimal TotalPaidDays { get; set; }
        public int ExecuteOrder { get; set; }
        public string Id { get; set; }

        public string ParentId { get; set; }
        public string Assignformulavalue { get; set; }

        public string Assignedvalues { get; set; }

        public string Name { get; set; }

        public string Output { get; set; }

        public int Order { get; set; }

        public int Rounding { get; set; }

        public int type { get; set; }

        public string Percentage { get; set; }

        public string EligibleFormula { get; set; }

        public string EligibleOutPut { get; set; }

        public Guid EmployeeId { get; set; }

        public int CompanyId { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }
        public int UserId { get; set; }

        public string BaseValue { get; set; }

        public bool DoRoundOff { get; set; }
        public int ExemptionType { get; set; }
        public string ActualFormula { get; set; }

        public string status { get; set; }
        public void Validate(string input, List<FormulaRecursive> lstCollection, string lhs, ref string error, out string values,string rerun)
        {
            var orgid = lstCollection.Where(u => u.Id.ToString().ToUpper() == lhs.ToUpper()).FirstOrDefault();
            values = string.Empty;
            if (!string.IsNullOrEmpty(error))
                return;
            if (!object.ReferenceEquals(input, null))
            {
                string temp = input;

                if (temp.IndexOf('{') >= 0)
                {
                    string id = temp.Substring(temp.IndexOf('{') + 1, 36);
                    var colTemp = lstCollection.Where(u => u.Id.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                    //var colTemp = lstCollection.Where(u => u.Id == id).FirstOrDefault();
                    if (lhs == id)
                    {
                        rerun = "Y";
                        return;
                    }
                    string replacevalue = "{" + id + "}";
                    if (!object.ReferenceEquals(colTemp, null))
                    {
                        var colTemp_sec = colTemp;
                        do
                        {
                            string outval = string.Empty;
                            string outval1 = string.Empty;
                            if (colTemp_sec.Assignedvalues.IndexOf("{") >= 0)
                            {
                                Assign_Val(colTemp_sec.Assignedvalues, lstCollection, colTemp_sec, ref error, rerun,out outval1);
                                if (rerun == "Y")
                                {
                                    return;
                                }
                            }
                            else
                            {
                                break;
                            }
                        } while (colTemp_sec.Assignedvalues.IndexOf("{") >= 0);


                        input = input.Replace(replacevalue, colTemp.Assignedvalues);
                        if (input.IndexOf('{') >= 0)
                        {
                            Validate(input, lstCollection, lhs, ref error, out values, rerun);
                        }
                        else
                        {
                            string outval = input;
                            if (input.ToLower().IndexOf("max[") >= 0)
                            {
                                input = GetMax(input);
                            }
                            if (input.ToLower().IndexOf("min[") >= 0)
                            {
                                input = GetMin(input);
                            }
                            values = input;
                        }
                    }
                }
            else
            {
                values = input;
            }

            }

            if (input.ToUpper().IndexOf("IF ") >= 0)
            {
                string outval1 = string.Empty;
                orgid.Assignedvalues = input;
                Assign_Val(input, lstCollection, orgid, ref error, rerun,out outval1);
                string outval = outval1;
                values = outval1;
            }


        }

        public void Assign_Val(string input, List<FormulaRecursive> lstCollection,FormulaRecursive lhs, ref string error,string rerun,out string values)
        {
            string result = string.Empty;
            FormulaRecursive orginput = new FormulaRecursive();
            FormulaRecursive colTemp = new FormulaRecursive();
            string replacevalue = string.Empty;
            orginput = lhs;
        /*    if (!string.IsNullOrEmpty(error))
                return;*/
            string loopend = "Y";
            string id = "";
            do
            {
                if (!object.ReferenceEquals(input, null))
                {
                    string temp = input;
                    if (input.IndexOf("{") >= 0)
                    {
                        id = temp.Substring(temp.IndexOf('{') + 1, 36);
                        colTemp = lstCollection.Where(u => u.Id.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                        replacevalue = "{" + id + "}";
                    }

                    if (lhs.Id == id)
                    {
                        rerun = "Y";
                        values = "";
                        return;
                    }


                    if (temp.IndexOf('{') >= 0)
                    {
                        if (!object.ReferenceEquals(colTemp, null))
                        {
                            if (colTemp.Assignedvalues.IndexOf('{') < 0 &&
                                lstCollection.Where(u => u.Id == colTemp.Id).FirstOrDefault().Assignedvalues.IndexOf("{") < 0)
                            {
                                var val = lstCollection.Where(u => u.Id == colTemp.Id).FirstOrDefault().Assignedvalues;
                                lstCollection.Where(u => u.Id == orginput.Id).FirstOrDefault().Assignedvalues = input.Replace(replacevalue, val);
                                input = input.Replace(replacevalue, val);
                                goto Cont_para;
                            }
                            else
                            {
                                orginput = colTemp;
                                input = colTemp.Assignedvalues;
                                goto Cont_para;
                            }
                        }
                        else
                        {
                            loopend = "";
                            var val = "0";
                            lstCollection.Where(u => u.Id == orginput.Id).FirstOrDefault().Assignedvalues = input.Replace(replacevalue, val);
                            input = input.Replace(replacevalue, val);
                            goto Cont_para;
                        }
                    }
                    else
                    {
                        colTemp = orginput;
                        string tempval = string.Empty;
                        colTemp.Assignedvalues = GetMax(colTemp.Assignedvalues);
                        colTemp.Assignedvalues = GetMin(colTemp.Assignedvalues);
                        if (!object.ReferenceEquals(colTemp, null))
                        {
                            if (colTemp.type == 3)
                            {
                                Eval eval = new Eval();
                                result = double.Parse(eval.Execute(colTemp.Assignedvalues)).ToString();
                                lstCollection.Where(u => u.Id == colTemp.Id).FirstOrDefault().Output = result;
                                lstCollection.Where(u => u.Id == colTemp.Id).FirstOrDefault().Assignedvalues = result;
                                values = result;
                            }
                        }

                        if (!object.ReferenceEquals(colTemp, null))
                        {
                            if (colTemp.type == 4 && colTemp.Assignedvalues.ToUpper().IndexOf("IF ") >= 0)
                            {
                                ifElseStmt obj = new ifElseStmt();
                                List<ifElseStmt> ifElseCollection = obj.GetifElse(colTemp.Assignedvalues);
                                ifElseCollection = obj.GetCorrectExecution(ifElseCollection);
                                var tm = ifElseCollection.Where(f => f.CorrectExecuteionBlock == true).FirstOrDefault();

                                if (!object.ReferenceEquals(tm, null))
                                    tempval = tm.thenVal;
                                Eval eval = new Eval();
                                result = eval.Execute(tempval).ToString();
                                colTemp.Output = result;
                                colTemp.Assignedvalues = result.ToString();
                                colTemp.Assignformulavalue = result.ToString();
                                Eval eval1 = new Eval();
                                result = eval1.Execute(tempval).ToString();
                                if (!string.IsNullOrEmpty(colTemp.EligibleFormula))
                                {
                                    string eliAmt = eval1.Execute(colTemp.EligibleFormula).ToString();
                                    lstCollection.Where(u => u.Id == colTemp.Id).FirstOrDefault().EligibleOutPut = eliAmt;
                                    colTemp.EligibleOutPut = eliAmt;
                                }
                                lstCollection.Where(u => u.Id == colTemp.Id).FirstOrDefault().Output = result;
                                lstCollection.Where(u => u.Id == colTemp.Id).FirstOrDefault().Assignedvalues = result;
                                values = result;
                            }
                        }


                        if (colTemp.type == 5)
                        {
                            if (colTemp.Order == 0)
                            {
                                result = FormulaRecursive.EvalRange(colTemp.Assignedvalues, Convert.ToDecimal(colTemp.Output), colTemp, lstCollection);
                                lstCollection.Where(u => u.Id == colTemp.Id).FirstOrDefault().Assignedvalues = result;
                                values = result;
                            }
                        }
                    }
                        loopend = "";
                    }
            loopend = "";
            Cont_para:;
            } while (loopend == "Y");

            values = result;
        }


        public void PTaxFormulavalues(string input, PayrollHistoryValueList temppayrolllst, string lhs, ref string error, out string values)
        {
          
            values = string.Empty;
            if (!string.IsNullOrEmpty(error))
                return;
            if (!object.ReferenceEquals(input, null))
            {
                string temp = input;
                if (temp.IndexOf('{') >= 0)
                {
                    string id = temp.Substring(temp.IndexOf('{') + 1, 36);
                    var colTemp = temppayrolllst.Where(u => u.AttributeModelId.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                   // var colTemp = lstCollection.Where(u => u.AttributeModelId.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                    //var colTemp = lstCollection.Where(u => u.Id == id).FirstOrDefault();
                    if (lhs == id)
                    {
                        error = "Error";
                        return;
                    }
                    string replacevalue = "{" + id + "}";
                    if (!object.ReferenceEquals(colTemp, null))
                    {
                        if (colTemp.Value.IndexOf('{') >= 0)
                        {
                            input = input.Replace(replacevalue, colTemp.Value);
                            PTaxFormulavalues(input, temppayrolllst, lhs, ref error, out values);
                        }
                        else
                        {
                            input = input.Replace(replacevalue, colTemp.Value);
                            if (input.IndexOf('{') >= 0)
                            {
                                PTaxFormulavalues(input, temppayrolllst, lhs, ref error, out values);
                            }
                            else
                            {
                                string outval = input;
                                values = input;
                            }
                        }
                    }

                }
                else
                {
                    values = input;
                }
                temp = values;
                temp = GetMax(temp);
                temp = GetMin(temp);
                if (temp.Contains("If"))
                {
                    string tempIfvalue = temp.Substring(temp.IndexOf('I'));
                    ifElseStmt obj = new ifElseStmt();
                    List<ifElseStmt> ifElseCollection = obj.GetifElse(tempIfvalue);
                    ifElseCollection = obj.GetCorrectExecution(ifElseCollection);
                    var tm = ifElseCollection.Where(f => f.CorrectExecuteionBlock == true).FirstOrDefault();
                    if (!object.ReferenceEquals(tm, null))
                        temp = temp.Replace(tempIfvalue, tm.thenVal);
                    else
                        temp = temp.Replace(tempIfvalue, "0");
                }
                Eval eval = new Eval();
                string result = eval.Execute(temp).ToString();
                values = result;
            }


        }


        public void ReOrder(TaxComputationInfo taxInfo, Employee employee, List<FormulaRecursive> lstCollection, FormulaRecursive inp, Entity entity,IncrementList increment, bool ffFlag, bool calcAttr = true)
        {
            try
            {
        /*       if (inp.Name == "TOTTAX" || inp.Name == "TOTITAX")
                {
                    Console.WriteLine(inp.Name);
                    var lstname = lstCollection.Where(l => l.Name == "NEWTAXSCHEME").FirstOrDefault();
                }*/
                if (inp.Name == "PTax")
                {
                }
                if (inp.Order == 0)
                {
                    string temp = inp.Assignedvalues.Replace("null", "");
                    if (inp.Name == "EG")
                    {
                        FormulaRecursive gratuity = lstCollection.Where(w => w.Name.ToLower() == "gratuity").FirstOrDefault();
                        if (!object.ReferenceEquals(gratuity, null))
                        {
                            temp = temp + "+" + "{" + gratuity.Id + "}";
                        }
                    }
                    string eligibleTemp = inp.EligibleFormula;
                    if ((!string.IsNullOrEmpty(temp) && temp.IndexOf("{") >= 0) || (!string.IsNullOrEmpty(eligibleTemp) && eligibleTemp.IndexOf('{') >= 0))
                    {
                        if (!string.IsNullOrEmpty(temp) && temp.IndexOf('{') >= 0)
                        {
                            do
                            {
                                try
                                {
                                    string id = temp.Substring(temp.IndexOf("{") + 1, 36);
                                    var colTemp = lstCollection.Where(u => u.Id.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                                    string replacevalue = "{" + id + "}";
                                    if (!object.ReferenceEquals(colTemp, null))
                                    {
                                        if (colTemp.Assignedvalues.IndexOf('{') >= 0)
                                        {
                                            if (colTemp.type == 1 && (colTemp.Name != "MD" && colTemp.Name != "PD" && colTemp.Name != "LD"))  //=------
                                            {
                                                temp = temp.Replace(replacevalue, colTemp.BaseValue.Replace('{', '$').Replace('}', '#'));

                                            }
                                            else
                                            {
                                                temp = temp.Replace(replacevalue, colTemp.Assignedvalues.Replace('{', '$').Replace('}', '#'));
                                                //   temp = temp.Replace(replacevalue, colTemp.Assignedvalues);
                                            }
                                        }
                                        else
                                        {
                                            var newinc = increment.Where(inc => inc.ApplyMonth == inp.Month && inc.ApplyYear == inp.Year).FirstOrDefault();
                                            if (colTemp.type == 1 && newinc != null && (colTemp.Name != "MD" && colTemp.Name != "PD" && colTemp.Name != "LD")) //----------
                                            {
                                                temp = temp.Replace(replacevalue, string.IsNullOrEmpty(colTemp.BaseValue) ? "0" : colTemp.BaseValue);
                                            }
                                            else
                                            {
                                                temp = temp.Replace(replacevalue, string.IsNullOrEmpty(colTemp.Assignedvalues) ? "0" : colTemp.Assignedvalues);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        temp = temp.Replace(replacevalue, "0");//if the attribute is not in the Entity attribute collection
                                        Console.WriteLine(temp);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                            while (temp.IndexOf('{') >= 0);

                        }

                        //Eligibility
                        if (!string.IsNullOrEmpty(eligibleTemp) && eligibleTemp.IndexOf('{') >= 0)
                        {
                            do
                            {
                                string id = eligibleTemp.Substring(eligibleTemp.IndexOf('{') + 1, 36);
                                var colTemp = lstCollection.Where(u => u.Id.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                                string replacevalue = "{" + id + "}";
                                if (!object.ReferenceEquals(colTemp, null))
                                {
                                    if (colTemp.Assignedvalues.IndexOf('{') >= 0)
                                    {
                                        eligibleTemp = eligibleTemp.Replace(replacevalue, colTemp.Assignedvalues.Replace('{', '$').Replace('}', '#'));
                                    }
                                    else
                                    {
                                        eligibleTemp = eligibleTemp.Replace(replacevalue, colTemp.Assignedvalues);
                                    }
                                }
                                else
                                {
                                    eligibleTemp = eligibleTemp.Replace(replacevalue, "0");//if the attribute is not in the Entity attribute collection
                                }
                            } while (eligibleTemp.IndexOf('{') >= 0);

                        }

                        /////////End eligibility
                        if (eligibleTemp == null)
                            eligibleTemp = "";
                        if (temp == null)
                            temp = "";
                        if ((temp.IndexOf('$') < 0 && temp.IndexOf('#') < 0) && (eligibleTemp == "" ? true : (eligibleTemp.IndexOf('$') < 0 && eligibleTemp.IndexOf('#') < 0)))
                        {
                            if (inp.Order == 0)
                            {
                                int maxVal = lstCollection.Max(u => u.Order);
                                maxVal = maxVal + 1;
                                lstCollection.Where(u => u.Id == inp.Id).FirstOrDefault().Order = maxVal;

                                if (inp.type == 4)
                                {
                                    ifElseStmt obj = new ifElseStmt();
                                    List<ifElseStmt> ifElseCollection = obj.GetifElse(temp);
                                    ifElseCollection = obj.GetCorrectExecution(ifElseCollection);
                                    var tm = ifElseCollection.Where(f => f.CorrectExecuteionBlock == true).FirstOrDefault();
                                    if (!object.ReferenceEquals(tm, null))
                                        temp = tm.thenVal;
                                    else
                                        temp = "0";
                                }
                                temp = GetMax(temp);
                                temp = GetMin(temp);
                                if (temp.Contains("If"))
                                {
                                    string tempIfvalue = temp.Substring(temp.IndexOf('I'));
                                    ifElseStmt obj = new ifElseStmt();
                                    List<ifElseStmt> ifElseCollection = obj.GetifElse(tempIfvalue);
                                    ifElseCollection = obj.GetCorrectExecution(ifElseCollection);
                                    var tm = ifElseCollection.Where(f => f.CorrectExecuteionBlock == true).FirstOrDefault();
                                    if (!object.ReferenceEquals(tm, null))
                                        temp = temp.Replace(tempIfvalue, tm.thenVal);
                                    else
                                        temp = temp.Replace(tempIfvalue, "0");
                                }
                                Eval eval = new Eval();
                                string result = eval.Execute(temp).ToString();
                                double baseValue = 0;
                                if (!string.IsNullOrEmpty(eligibleTemp))
                                {
                                    string eliAmt = eval.Execute(eligibleTemp).ToString();
                                    lstCollection.Where(u => u.Id == inp.Id).FirstOrDefault().EligibleOutPut = eliAmt;

                                }
                                double dresult;

                                if (double.TryParse(result, out dresult))
                                {
                                    if (inp.type != 1)
                                    {
                                        baseValue = dresult;
                                    }
                                    else
                                    {
                                        baseValue = Convert.ToDouble(lstCollection.Where(u => u.Id == inp.Id).FirstOrDefault().BaseValue);
                                    }

                                    if (inp.Name == "ESIEMPR")
                                    {
                                        var tempESIEMPR = lstCollection.Where(x => x.Name == "ESI").FirstOrDefault();
                                        if (tempESIEMPR != null && tempESIEMPR.Output == "0")
                                        {
                                            dresult = 0;//ESI Amount zero so Employer contribution also Zero
                                        }
                                    }
                                    if (inp.Name == "PF" && calcAttr)
                                    {
                                        #region calculate PF
                                        PF pf = new PF();
                                        //Get Additional Info model Id Employee based PF Percentage changed if exist otherwise take a default value.
                                        // Command on 03 Auguest 2018 get values from based on Master input settings
                                        //EntityModel entityModel = new EntityModel("AdditionalInfo", inp.CompanyId);
                                        //EntityAdditionalInfoList entyAdditionalInfo = new EntityAdditionalInfoList(inp.CompanyId, entityModel.Id, inp.EmployeeId);
                                        //var tempAttVal = entyAdditionalInfo.Where(X => X.AttributeModelId == new Guid(inp.Id)).FirstOrDefault();
                                        //inp.Percentage = tempAttVal != null ? tempAttVal.Value : inp.Percentage;

                                        pf.CalculatePF(taxInfo,employee,dresult, ref inp, ref lstCollection, inp.Percentage, entity);
                                        dresult = Convert.ToDouble(inp.Output);

                                        /*
                                        pf.Calc(dresult, ref inp, ref lstCollection);
                                        double amt = dresult;//15000
                                        List<string> sett = pf.GetPFSetting(inp.CompanyId, inp.EmployeeId);
                                        double limit = 0;
                                        if (double.TryParse(sett[0], out limit))
                                        {
                                            limit = Convert.ToDouble(sett[0]);//setting value  
                                        }
                                        if (sett[1].ToUpper() == "LIMIT")
                                        {
                                            amt = limit;
                                        }
                                        inp.Output = amt.ToString();
                                        dresult = amt;
                                        if (sett[1].ToUpper() == "BOTH")
                                        {
                                            amt = limit;
                                            inp.Output = amt.ToString();
                                        }
                                        childUpdate(inp, lstCollection);*/
                                        #endregion
                                    }
                                    if (inp.Name == "ESI" && calcAttr)
                                    {
                                        ESI esi = new ESI();
                                        //dresult
                                        double amt = dresult;//15000
                                                             //  var compsetting = esi.GetEsiSetting(inp.CompanyId);
                                        EsiLocationList esilocationlist = new EsiLocationList(inp.CompanyId);
                                    //    Employee empdetails = new Employee(inp.CompanyId, inp.EmployeeId);
                                        var esiApplicable = esilocationlist.Where(X => X.Id == employee.ESILocation).FirstOrDefault();
                                        if (esiApplicable != null)
                                        {
                                            if (esiApplicable.isApplicable == true)
                                            {
                                                List<string> sett1 = esi.GetEsiAmount(inp.EmployeeId, inp.CompanyId, dresult, Convert.ToDouble(inp.EligibleOutPut), inp.Year, inp.Month, inp.UserId);
                                                double limit = 0;
                                                if (double.TryParse(sett1[0], out limit))
                                                {
                                                    limit = Convert.ToDouble(sett1[0]);
                                                }
                                                if (sett1[1].ToUpper() == "LIMIT")
                                                {
                                                    amt = limit;
                                                }
                                                inp.Output = amt.ToString();
                                                dresult = amt;
                                                //Get Additional Info model Id Employee based ESI Percentage changed if exist otherwise take a default value.
                                                // Command on 03 Auguest 2018 get values from based on Master input settings 
                                                //EntityModel entityModel = new EntityModel("AdditionalInfo", inp.CompanyId);
                                                //EntityAdditionalInfoList entyAdditionalInfo = new EntityAdditionalInfoList(inp.CompanyId, entityModel.Id, inp.EmployeeId);
                                                //var tempAttVal = entyAdditionalInfo.Where(X => X.AttributeModelId == new Guid(inp.Id)).FirstOrDefault();
                                                //inp.Percentage = tempAttVal != null ? tempAttVal.Value : inp.Percentage;
                                                if (inp.Percentage == "0")
                                                {
                                                    var tempESIEMPR = lstCollection.Where(x => x.Name == "ESIEMPR").FirstOrDefault();
                                                    if (tempESIEMPR != null)
                                                    {
                                                        lstCollection.Where(u => u.Id == tempESIEMPR.Id).FirstOrDefault().Output = "0";
                                                        lstCollection.Where(u => u.Id == tempESIEMPR.Id).FirstOrDefault().Assignedvalues = "0";
                                                        lstCollection.Where(u => u.Id == inp.Id).FirstOrDefault().BaseValue = "0";
                                                    }

                                                }
                                                childUpdate(inp, lstCollection);
                                            }
                                            else
                                                dresult = 0;
                                        }
                                        else
                                            dresult = 0;
                                    }
                                    if (inp.Name == "PTAX" && calcAttr)
                                    {

                                        //Need to calculate PTAX
                                        PTax ptax = new PTax();
                                        double currentEranedGrossAmt = dresult;
                                        decimal projectedval = 0;
                                        if (dresult > 0 || ffFlag)
                                        {
                                            dresult = ptax.GetPTaxCalculation(taxInfo,employee, inp.CompanyId, dresult, Convert.ToDouble(inp.EligibleOutPut), inp.Year, inp.Month, inp.UserId, out projectedval, ffFlag);
                                            if (!ffFlag)
                                                dresult = currentEranedGrossAmt > dresult ? dresult : currentEranedGrossAmt; // min(Gross,PT)
                                        }
                                        else
                                            dresult = 0;

                                    }
                                    //execute the percentge
                                    if (!string.IsNullOrEmpty(inp.Percentage))
                                    {
                                        double percnt;
                                        if (double.TryParse(inp.Percentage, out percnt))
                                        {
                                            baseValue = dresult;
                                            dresult = dresult * percnt / 100;
                                        }
                                    }
                                    //execute the round off
                                    if (inp.DoRoundOff)
                                    {
                                        double tmpResult = RoundOff(dresult, inp.Rounding);
                                        double tempBaseValue = RoundOff(baseValue, inp.Rounding);
                                        result = tmpResult.ToString();
                                        baseValue = tempBaseValue;
                                    }
                                    else
                                    {
                                        result = dresult.ToString();
                                    }

                                }

                                lstCollection.Where(u => u.Id == inp.Id).FirstOrDefault().Output = result;
                                lstCollection.Where(u => u.Id == inp.Id).FirstOrDefault().Assignedvalues = result;
                                lstCollection.Where(u => u.Id == inp.Id).FirstOrDefault().BaseValue = baseValue.ToString();

                            }
                        }
                    }
                    else
                    {
                        if (inp.Order == 0)
                        {
                            int maxVal = lstCollection.Max(u => u.Order);
                            maxVal = maxVal + 1;
                            lstCollection.Where(u => u.Id == inp.Id).FirstOrDefault().Order = maxVal;

                            if (inp.type == 4)
                            {
                                ifElseStmt obj = new ifElseStmt();
                                List<ifElseStmt> ifElseCollection = obj.GetifElse(temp);
                                ifElseCollection = obj.GetCorrectExecution(ifElseCollection);
                                var tm = ifElseCollection.Where(f => f.CorrectExecuteionBlock == true).FirstOrDefault();
                                if (!object.ReferenceEquals(tm, null))
                                    temp = tm.thenVal;
                                else
                                    temp = "0";
                            }
                            temp = GetMax(temp);
                            temp = GetMin(temp);
                            Eval eval = new Eval();
                            string result = eval.Execute(temp).ToString();
                            if (!string.IsNullOrEmpty(inp.EligibleFormula))
                            {
                                string eliAmt = eval.Execute(inp.EligibleFormula).ToString();
                                lstCollection.Where(u => u.Id == inp.Id).FirstOrDefault().EligibleOutPut = eliAmt;
                                inp.EligibleOutPut = eliAmt;

                            }

                            double dresult;
                            double baseValue = 0;
                            if (double.TryParse(result, out dresult))
                            {
                                if (inp.type != 1)
                                {
                                    baseValue = dresult;
                                }
                                else
                                {
                                    baseValue = Convert.ToDouble(lstCollection.Where(u => u.Id == inp.Id).FirstOrDefault().BaseValue);
                                }

                                if (inp.Name == "PF" && calcAttr)
                                {
                                    #region calculate PF
                                    PF pf = new PF();
                                    pf.CalculatePF(taxInfo,employee,dresult, ref inp, ref lstCollection, inp.Percentage, entity);
                                    dresult = Convert.ToDouble(inp.Output);

                                    /*
                                    double amt = dresult;//15000
                                    List<string> sett = pf.GetPFSetting(inp.CompanyId,inp.EmployeeId);
                                    double limit = 0;
                                    if (double.TryParse(sett[0], out limit))
                                    {
                                        limit = Convert.ToDouble(sett[0]);
                                    }
                                    if (sett[1].ToUpper() == "LIMIT")
                                    {
                                        amt = limit;
                                    }
                                    inp.Output = amt.ToString();
                                    dresult = amt;
                                    if (sett[1].ToUpper() == "BOTH")
                                    {
                                        amt = limit;
                                        inp.Output = amt.ToString();
                                    }
                                    childUpdate(inp, lstCollection);
                                    */
                                    #endregion
                                }
                                if (inp.Name == "ESI" && calcAttr)
                                {
                                    ESI esi = new ESI();
                                    //dresult
                                    double amt = dresult;//15000
                                    EsiLocationList esilocationlist = new EsiLocationList(inp.CompanyId);
                                 //   Employee empdetails = new Employee(inp.CompanyId, inp.EmployeeId);
                                    var esiApplicable = esilocationlist.Where(X => X.Id == employee.ESILocation).FirstOrDefault();
                                    if (esiApplicable != null)
                                    {
                                        if (esiApplicable.isApplicable == true)
                                        {
                                            List<string> sett1 = esi.GetEsiAmount(inp.EmployeeId, inp.CompanyId, dresult, Convert.ToDouble(inp.EligibleOutPut), inp.Year, inp.Month, inp.UserId);
                                            double limit = 0;
                                            if (double.TryParse(sett1[0], out limit))
                                            {
                                                limit = Convert.ToDouble(sett1[0]);
                                            }
                                            if (sett1[1].ToUpper() == "LIMIT")
                                            {
                                                amt = limit;
                                            }
                                            inp.Output = amt.ToString();
                                            dresult = amt;
                                            childUpdate(inp, lstCollection);
                                            //  ESI esi = new ESI();
                                            //  dresult = esi.GetEsiAmount(inp.EmployeeId, inp.CompanyId, dresult, Convert.ToDouble(inp.EligibleOutPut), inp.Year, inp.Month, inp.UserId);
                                        }
                                        else
                                            dresult = 0;
                                    }
                                    else
                                        dresult = 0;
                                }
                                if (inp.Name == "PTAX" && calcAttr)
                                {
                                    //Need to calculate PTAX
                                    PTax ptax = new PTax();
                                    double currentEranedGrossAmt = dresult;
                                    decimal projectedval = 0;
                                    dresult = ptax.GetPTaxCalculation(taxInfo,employee, inp.CompanyId, dresult, Convert.ToDouble(inp.EligibleOutPut), inp.Year, inp.Month, inp.UserId, out projectedval, ffFlag);
                                    if (!ffFlag)
                                        dresult = currentEranedGrossAmt > dresult ? dresult : currentEranedGrossAmt; // min(Gross,PT)                               

                                }
                                //execute the percentge
                                if (!string.IsNullOrEmpty(inp.Percentage))
                                {
                                    double percnt;
                                    if (double.TryParse(inp.Percentage, out percnt))
                                    {
                                        if (inp.type != 1)
                                        {
                                            baseValue = dresult;
                                        }
                                        else
                                        {
                                            baseValue = Convert.ToDouble(lstCollection.Where(u => u.Id == inp.Id).FirstOrDefault().BaseValue);
                                        }


                                        dresult = dresult * percnt / 100;
                                    }
                                }
                                //execute the round off
                                if (inp.Name.ToUpper().Trim() == "PFINSPECTION" || inp.Name.ToUpper().Trim() == "PFEDLI" || inp.Name.ToUpper().Trim() == "PFADM")
                                {
                                    result = dresult.ToString("0.00");
                                }
                                else
                                {
                                    if (inp.DoRoundOff)
                                    {
                                        double tmpResult = RoundOff(dresult, inp.Rounding);
                                        double tmpBaseVal = RoundOff(baseValue, inp.Rounding);
                                        result = tmpResult.ToString();
                                        baseValue = tmpBaseVal;
                                    }
                                    else
                                    {
                                        result = dresult.ToString();
                                    }
                                }

                            }
                            lstCollection.Where(u => u.Id == inp.Id).FirstOrDefault().Output = result;
                            lstCollection.Where(u => u.Id == inp.Id).FirstOrDefault().Assignedvalues = result;
                            lstCollection.Where(u => u.Id == inp.Id).FirstOrDefault().BaseValue = baseValue.ToString();

                        }

                    }

                    }
                if (inp.type == 5)
                {
                    if (inp.Order != 0)
                    {
                        string output = FormulaRecursive.EvalRange(inp.Assignformulavalue, Convert.ToDecimal(inp.Output), inp, lstCollection);
                        // inp.Assignedvalues = output;
                        lstCollection.Where(u => u.Id == inp.Id).FirstOrDefault().Assignedvalues = output;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException("Error in " + inp.Name, ex.Message);
            }
        }

        //-----------------------Modified

        public string GetMin(string temp)
        {
            decimal minValue = 0;
            do
            {
                if (temp.ToLower().IndexOf("min[") >= 0)
                {
                    string minString = temp.Substring(temp.ToLower().IndexOf("min["));
                    minString = minString.Substring(minString.ToLower().IndexOf("min[") + 3, minString.IndexOf(']') - minString.ToLower().IndexOf("min[") - 2);
                    minString = minString.Substring(minString.IndexOf('[') + 1, minString.IndexOf(']') - minString.IndexOf('[') - 1);
                    string[] valueString = minString.Split(',');
                    decimal[] values = new decimal[valueString.Count()];
                    int index = 0;
                    foreach (object item in valueString)
                    {
                        Eval eval = new Eval();
                        string result = eval.Execute(item.ToString()).ToString();
                        values[index] = Convert.ToDecimal(result);

                        index++;
                    }
                    minValue = values.Min();
                    temp = temp.ToLower().Replace("min[" + minString + "]", minValue.ToString());

                }
            } while (temp.ToLower().IndexOf("min[") >= 0);

            return temp;


        }
        public string GetMax(string temp)
        {
            decimal maxValue = 0;
            if (object.ReferenceEquals(temp, null))
            {
                return "";
            }
            do
            {
                if (temp.ToLower().IndexOf("max[") >= 0)
                {
                    string maxString = temp.Substring(temp.ToLower().IndexOf("max["));
                    maxString = maxString.Substring(maxString.ToLower().IndexOf("max[") + 3, maxString.IndexOf(']') - maxString.ToLower().IndexOf("max[") - 2);
                    maxString = maxString.Substring(maxString.IndexOf('[') + 1, maxString.IndexOf(']') - maxString.IndexOf('[') - 1);
                    string[] valueString = maxString.Split(',');
                    decimal[] values = new decimal[valueString.Count()];
                    int index = 0;
                    foreach (object item in valueString)
                    {
                        Eval eval = new Eval();
                        string result = eval.Execute(item.ToString()).ToString();
                        values[index] = Convert.ToDecimal(result);
                        index++;
                    }
                    maxValue = values.Max();
                    temp = temp.ToLower().Replace("max[" + maxString + "]", maxValue.ToString());
                }
            } while (temp.ToLower().IndexOf("max[") >= 0);

            return temp;

        }
        private void childUpdate(FormulaRecursive u, List<FormulaRecursive> lstCollection)
        {


            lstCollection.Where(p => p.ParentId == u.Id).ToList().ForEach(s =>
            {
                double employeerPercntge;
                string employeerPfAmount = u.Output;
                if (!string.IsNullOrEmpty(s.Percentage) && double.TryParse(s.Percentage, out employeerPercntge))
                {
                    employeerPfAmount = (Convert.ToDouble(u.Output) * employeerPercntge / 100).ToString("0.00");

                }
                s.Output = employeerPfAmount;
                s.Assignedvalues = employeerPfAmount;
                s.Percentage = "";
            });
        }

        public double RoundOff(double input, int roundingOption)
        {
            switch (roundingOption)
            {
                case 1:
                    return ((double)Math.Round(input / 1.0, 2)) * 1;  // Round
                case 2:
                    return ((int)Math.Ceiling(input / 1.0)) * 1; // Roundup
                case 3:
                    return ((int)Math.Floor(input / 1.0)) * 1; // Rounddown
                case 4:
                    var temp = ((int)Math.Round(input * 2.0)); // Round to 50 paise
                    return temp / 2;
                case 5:
                    var temp1 = ((int)Math.Ceiling(input * 2.0)); // Roundup to 50 paise
                    return temp1 / 2.0;
                case 6:
                    var temp2 = ((int)Math.Floor(input * 2.0)); // Rounddown to 50 paise
                    return temp2 / 2.0;
                case 11:
                    return ((int)Math.Round(input / 5.0)) * 5;   // Round to 5 rupees
                case 12:
                    return ((int)Math.Ceiling(input / 5.0)) * 5;   // Roundup to 5 rupees
                case 13:
                    return ((int)Math.Floor(input / 5.0)) * 5;   // Rounddown to 5 rupees
                case 14:
                    return ((int)Math.Round(input / 10.0)) * 10;  // Round to 10 rupees
                case 50:
                    return ((int)Math.Round(input / 50.0)) * 50;
                case 100:
                    return ((int)Math.Round(input / 100.0)) * 100;
                case 9:
                    return ((double)Math.Round(Convert.ToDecimal(String.Format("{0:0.00}", input)) + Convert.ToDecimal(0.01)));
                case 15:
                    return ((double)Math.Round((input + 0.01) / 1.0, 0)) * 1;  // 1Rs Rounding +0.01 added for 4.5 consider as 5
                case 99:
                    return ((double)Math.Round(input / 1.0, 2));
                default:
                    return ((double)Math.Round(input / 1.0, 2)) * 1;
            }
        }

        public static string EvalRange(string expr, decimal baseValue, FormulaRecursive curFormula, List<FormulaRecursive> lstCollection)
        {
            string result;
            string output = "0";
            string[] ranges = expr.TrimEnd(':').Split(':');
            decimal currentBaseValue = baseValue;
            decimal tobedeductbase = 0;
            decimal deductedval = 0;
            decimal tovalue = 0;
            for (int range = 0; range < ranges.Count(); range++)
            {
                string fromVal = ranges[range].Substring(0, ranges[range].IndexOf('-'));
                string toVal = ranges[range].Substring(ranges[range].IndexOf('-') + 1, ranges[range].ToUpper().IndexOf("THEN") - ranges[range].IndexOf('-') - 1);
                string thenVal = ranges[range].Substring(ranges[range].IndexOf("THEN") + 4);
                if (baseValue < Convert.ToDecimal(fromVal))
                {
                    tobedeductbase = currentBaseValue;
                    currentBaseValue = 0;
                }
                else if (baseValue > Convert.ToDecimal(fromVal) && baseValue <= Convert.ToDecimal(toVal))
                {
                    tobedeductbase = currentBaseValue;
                    currentBaseValue = 0;
                }
                else if (baseValue > Convert.ToDecimal(toVal))
                {
                    tobedeductbase = Math.Abs(Convert.ToDecimal(toVal) - tovalue);
                    currentBaseValue = currentBaseValue - tobedeductbase;
                    tovalue = Convert.ToDecimal(toVal);
                }
                string outExp = thenVal + "*" + tobedeductbase.ToString();
                deductedval = deductedval + tobedeductbase;
                if (baseValue == 0)
                {
                    outExp = "0";
                }
                if (!string.IsNullOrEmpty(outExp) && outExp.IndexOf('{') >= 0)
                {
                    do
                    {
                        string id = outExp.Substring(outExp.IndexOf('{') + 1, 36);
                        var colTemp = lstCollection.Where(u => u.Id.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                        string replacevalue = "{" + id + "}";
                        if (!object.ReferenceEquals(colTemp, null))
                        {
                            if (colTemp.Assignedvalues.IndexOf('{') >= 0)
                            {
                                outExp = outExp.Replace(replacevalue, colTemp.Assignedvalues.Replace('{', '$').Replace('}', '#'));
                            }
                            else
                            {
                                outExp = outExp.Replace(replacevalue, colTemp.Assignedvalues);
                            }
                        }
                        else
                        {
                            outExp = outExp.Replace(replacevalue, "0");//if the attribute is not in the Entity attribute collection
                        }
                    } while (outExp.IndexOf('{') >= 0);
                }
                Eval eval = new Eval();
                string val = eval.Execute(outExp).ToString();
                output = Convert.ToString(Convert.ToDecimal(output) + Convert.ToDecimal(val));
                // baseValue = currentBaseValue;

            }
            result = output;
            return result;

        }
    }


}
