using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class Emp_CodeSettingList : List<Emp_CodeSetting>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public Emp_CodeSettingList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public Emp_CodeSettingList(int companyId)
        {
            this.CompanyId = companyId;
            Emp_CodeSetting setting = new Emp_CodeSetting();
            setting.CompanyId = companyId;

            DataTable dtValue = setting.GetTableValues();

            DataView view = new DataView(dtValue);
            DataTable distinctValues = view.ToTable(true, "Name", "PreFix", "SNumber", "CompanyId", "CreatedBy", "ModifiedBy");
            distinctValues.Columns.Add("CategoryName");
            foreach (DataRow dr in distinctValues.Rows)
            {
                string Cat = string.Empty;
                foreach (DataRow dc in dtValue.Rows)
                {
                    string caT = dr["Name"].ToString().ToLower();
                    string caT1 = dc["Name"].ToString().ToLower();
                    string caTIID = dc["CategoryId"].ToString().ToLower();
                    if (caT == caT1)
                    {
                        CategoryList catList = new CategoryList(companyId);
                        var Slcat = catList.Where(u => u.Id == new Guid(caTIID)).ToList();
                        //testing
                        if (Slcat.Count != 0 && Cat == "")
                        {
                            Cat = Cat + Slcat[0].Name + "";
                        }
                        else if(Slcat.Count != 0 && Cat!="")
                        {
                            Cat = Cat + "/"+ Slcat[0].Name ;
                        }
                    }
                }
                dr["CategoryName"] = Cat;

            }

            //Distinct Values
            dtValue = distinctValues;
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Emp_CodeSetting settingTemp = new Emp_CodeSetting();
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                    //    settingTemp.Id = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);
                    settingTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    settingTemp.PreFix = Convert.ToString(dtValue.Rows[rowcount]["PreFix"]);
                    settingTemp.SNumber = Convert.ToString(dtValue.Rows[rowcount]["SNumber"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        settingTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CategoryName"])))
                        settingTemp.CategoryName = Convert.ToString(dtValue.Rows[rowcount]["CategoryName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        settingTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                    //    settingTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        settingTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                    //    settingTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(settingTemp);
                }
            }
        }


        public Emp_CodeSettingList(int companyId, string SettingName)
        {
            this.CompanyId = companyId;
            Emp_CodeSetting setting = new Emp_CodeSetting();
            setting.CompanyId = companyId;
            DataTable dtValue = setting.GetTableValues();
            if (!string.IsNullOrEmpty(SettingName))
            {
                DataTable dt = dtValue.Select("Name='" + SettingName + "'").CopyToDataTable();
                dtValue = dt;                
            }
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Emp_CodeSetting settingTemp = new Emp_CodeSetting();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        settingTemp.Id = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);
                    settingTemp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    settingTemp.PreFix = Convert.ToString(dtValue.Rows[rowcount]["PreFix"]);
                    settingTemp.SNumber = Convert.ToString(dtValue.Rows[rowcount]["SNumber"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        settingTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"])))
                        settingTemp.CategoryId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["CategoryId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        settingTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        settingTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        settingTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        settingTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(settingTemp);
                }
            }
        }

        #endregion

        #region property


        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the Category and add to the list
        /// </summary>
        /// <param name="setting"></param>
        public void AddNew(Emp_CodeSetting empCodesetting)
        {
            if (empCodesetting.Save())
            {
                this.Add(empCodesetting);
            }
        }

        /// <summary>
        /// Delete the Category and remove from the list
        /// </summary>
        /// <param name="setting"></param>
        public void DeleteExist(Emp_CodeSetting empCodesetting)
        {
            if (empCodesetting.Delete())
            {
                this.Remove(empCodesetting);
            }
        }

        #endregion

        #region private methods




        #endregion
    }
}
