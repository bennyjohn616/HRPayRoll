using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class ESI
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public ESI()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public ESI(int id, int companyId)
        {

        }


        public List<string> GetEsiAmount(Guid employeeId, int companyId, double esiGrossAmt, double eligibilityAmt, int year, int month, int userid)
        {
            List<string> retObj = new List<string>();
            string limit = string.Empty;
            string basedVale = string.Empty;
            Employee employee = new Employee(companyId, employeeId);
            Category cate = new Category(employee.CategoryId, companyId);
            if (string.IsNullOrEmpty(cate.ESIProcess))//check the is empty
            {
                cate.ESIProcess = "GROSS";
            }
            basedVale = cate.ESIProcess;
            // EsiLocation esiLocation = new EsiLocation(employee.ESILocation, companyId);
            // List<string> esiSetting = GetEsiSetting(companyId);
            double esilimt = Convert.ToDouble(cate.ESILimit);// Convert.ToDouble(esiSetting[0]);//15000;
            if (eligibilityAmt > esilimt)//Not eligible
            {
                if (month == 4 || month == 10)//check the eligibility and update
                {
                    //update the employee table
                    employee.UpdateESIEligibility(employee.Id, userid, false);
                }
                limit = "0";//no need to calculate Esi,need to check the month and update the employee table EsiEligibility
            }
            else
            {
                if (!employee.ESIEligibility)//if (month == 4 || month == 10)//check the eligibility and update
                {
                    if (userid > 0)
                    {
                        if (employee.UpdateESIEligibility(employee.Id, userid, true))
                        {
                            employee.ESIEligibility = true;
                        }
                    }
                    else
                    {
                        employee.ESIEligibility = true;//this is for temperary process of salary,should not update the eligibility
                    }
                    //update the employee table
                }
                if (!employee.ESIEligibility)
                {
                    limit="0";
                }
                bool isLimitBased = false;
                if (cate.ESIProcess.ToUpper()== "LIMIT")//esiSetting[1] == "LIMITBASED")
                    isLimitBased = true;
                if (isLimitBased)
                {
                    if (esiGrossAmt >= esilimt)
                    {
                        limit = esilimt.ToString(); //* 1.75 / 100;
                    }
                    else
                    {
                        limit= esiGrossAmt.ToString();// * 1.75 / 100;
                    }
                }
                else
                {
                    limit= esiGrossAmt.ToString(); //* 1.75 / 100;
                }

            }
            retObj.Add(limit);
            retObj.Add(basedVale);
            return retObj;

        }

        public List<string> GetEsiSetting(int companyId)
        {
            List<string> retList = new List<string>();
            SettingList settinglist = new SettingList(companyId);
            Setting setting = settinglist.Where(u => u.Name.ToUpper() == "STATUTORYESI").FirstOrDefault();
            SettingDefinitionList settingDefinitionlist = new SettingDefinitionList(setting.Id, companyId);
            SettingValueList settingvalueList = new SettingValueList(setting.Id);
            double limt = 0;
            string basedValue = string.Empty;
            settingDefinitionlist.ForEach(u =>
            {
                if (u.Name.ToUpper() == "ESILIMIT")
                {
                    var setval = settingvalueList.Where(p => p.SettingDefinitionId == u.Id).FirstOrDefault();
                    if (!object.ReferenceEquals(setval, null))
                    {
                        if (double.TryParse(setval.Value, out limt))
                        {
                            //Vallue assined in limit using try parse
                        }
                    }
                }
                else if (u.Name.ToUpper() == "LIMITBASED" || u.Name.ToUpper() == "NOLIMIT")
                {
                    var setval = settingvalueList.Where(p => p.SettingDefinitionId == u.Id).FirstOrDefault();
                    if (!object.ReferenceEquals(setval, null))
                    {
                        if (setval.Value.ToUpper().Trim() == "ON" || setval.Value.ToUpper().Trim() == "TRUE")
                            basedValue = u.Name.ToUpper(); //setval.Value.ToUpper().Trim();
                    }
                }
            });
            retList.Add(limt.ToString());
            retList.Add(basedValue);
            return retList;
            //return new { Limit = limt, BasedValue = basedValue };
            // return true;

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




        #endregion

        #region private methods





        #endregion

    }

}
