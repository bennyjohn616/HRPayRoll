// -----------------------------------------------------------------------
// <copyright file="SettingDefinition.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SQLDBOperation;
    using System.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// To handle the SettingDefinition
    /// </summary>
    public class SettingDefinition
    {

        #region private variable
        private SettingValue _settingValue;

        private List<settingDropDown> _settingDropDown;

        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public SettingDefinition()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public SettingDefinition(int id, int settingId)
        {
            this.Id = id;
            this.SettingId = settingId;
            DataTable dtValue = this.GetTableValues(this.Id, this.SettingId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ParentId"])))
                    this.ParentId = Convert.ToInt32(dtValue.Rows[0]["ParentId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsUniqueConstraint"])))
                    this.IsUniqueConstraint = Convert.ToBoolean(dtValue.Rows[0]["IsUniqueConstraint"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SettingId"])))
                    this.SettingId = Convert.ToInt32(dtValue.Rows[0]["SettingId"]);
                this.Name = Convert.ToString(dtValue.Rows[0]["Name"]);
                this.DisplayAs = Convert.ToString(dtValue.Rows[0]["DisplayAs"]);
                this.ControlType = Convert.ToString(dtValue.Rows[0]["ControlType"]);
                this.Value = Convert.ToString(dtValue.Rows[0]["Value"]);
                this.RefEntityModelId = Convert.ToString(dtValue.Rows[0]["RefEntityModelId"]);
                this.RadioGroupName = Convert.ToString(dtValue.Rows[0]["RadioGroupName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get or set the parent id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Get or Set the SettingId
        /// </summary>
        public int SettingId { get; set; }

        /// <summary>
        /// Get or Set the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or Set the DisplayAs
        /// </summary>
        public string DisplayAs { get; set; }

        /// <summary>
        /// Get or Set the ControlType
        /// </summary>
        public string ControlType { get; set; }

        /// <summary>
        /// Get or Set the Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Get or Set the RefEntityModelId
        /// </summary>
        public string RefEntityModelId { get; set; }

        /// <summary>
        /// Get or Set the RadioGroupName
        /// </summary>
        public string RadioGroupName { get; set; }

        /// <summary>
        /// Get or Set the IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

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

        /// <summary>
        /// Get or Set the ModifiedOn
        /// </summary>
        public int CompanyId { get; set; }

        public bool IsUniqueConstraint { get; set; }

        public string UniqueConstaraintName { get; set; }



        public SettingValue SettingValue
        {
            get
            {
                if (object.ReferenceEquals(_settingValue, null))
                {
                    if (this.Id > 0)
                    {
                        _settingValue = new SettingValue(this.SettingId, this.Id);

                    }
                    else
                    {
                        _settingValue = new SettingValue();
                    }
                }
                return _settingValue;
            }
            set
            {
                _settingValue = value;
            }
        }


        public List<settingDropDown> SettingDropDown
        {
            get
            {
                if (object.ReferenceEquals(_settingDropDown, null))
                {
                    if (this.ControlType == "dropdown")
                    {
                        _settingDropDown = new List<settingDropDown>();
                        if (!string.IsNullOrEmpty(this.RefEntityModelId))
                        {
                            string refEnt = this.RefEntityModelId;
                            if (!string.IsNullOrEmpty(refEnt))
                            {
                                refEnt = refEnt.ToUpper();
                            }
                            switch (refEnt)
                            {
                                case "CATEGORY":
                                    CategoryList category = new CategoryList(this.CompanyId);
                                    category.ForEach(u => { _settingDropDown.Add(new settingDropDown() { id = u.Id.ToString(), name = u.Name }); });
                                    break;
                                case "PARYOLLPROCESS":
                                    _settingDropDown.Add(new settingDropDown() { id = "Id", name = "Employee" });
                                    _settingDropDown.Add(new settingDropDown() { id = "CategoryId", name = "Category" });
                                    _settingDropDown.Add(new settingDropDown() { id = "Designation", name = "Designation" });
                                    _settingDropDown.Add(new settingDropDown() { id = "Department", name = "Department" });
                                    _settingDropDown.Add(new settingDropDown() { id = "Branch", name = "Branch" });
                                    _settingDropDown.Add(new settingDropDown() { id = "Location", name = "Location" });
                                    _settingDropDown.Add(new settingDropDown() { id = "CostCentre", name = "CostCentre" });
                                    _settingDropDown.Add(new settingDropDown() { id = "ESILocation", name = "ESILocation" });
                                    _settingDropDown.Add(new settingDropDown() { id = "PTLocation", name = "PTLocation" });
                                    break;
                            }
                        }
                        else if (!string.IsNullOrEmpty(this.Value))
                        {

                            List<string> data = new List<string>();
                            data = this.Value.Split(',').ToList();
                            data.ForEach(u => { _settingDropDown.Add(new settingDropDown() { id = u, name = u }); });
                        }
                        else
                            _settingDropDown = new List<settingDropDown>();

                    }
                    else
                    {
                        _settingDropDown = new List<settingDropDown>();
                    }
                }
                return _settingDropDown;
            }
            set
            {
                _settingDropDown = value;
            }
        }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the SettingDefinition
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("SettingDefinition_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ParentId", this.ParentId);
            sqlCommand.Parameters.AddWithValue("@SettingId", this.SettingId);
            sqlCommand.Parameters.AddWithValue("@Name", this.Name);
            sqlCommand.Parameters.AddWithValue("@DisplayAs", this.DisplayAs);
            sqlCommand.Parameters.AddWithValue("@ControlType", this.ControlType);
            sqlCommand.Parameters.AddWithValue("@Value", this.Value);
            sqlCommand.Parameters.AddWithValue("@RefEntityModelId", this.RefEntityModelId);
            sqlCommand.Parameters.AddWithValue("@RadioGroupName", this.RadioGroupName);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsUniqueConstraint", this.IsUniqueConstraint);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = Convert.ToInt32(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the SettingDefinition
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("SettingDefinition_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the SettingDefinition
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(int id, int settingId)
        {

            SqlCommand sqlCommand = new SqlCommand("SettingDefinition_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@SettingId", settingId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        ///<summary>
        ///DeclarationCarryForward
        ///</summary>
        public bool CarryForward(int companyId, DateTime SMonth, DateTime EMonth)
        {
            SqlCommand sqlCommand = new SqlCommand("usp_DeclarationCarryForward_Create");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@StartDate", SMonth);
            sqlCommand.Parameters.AddWithValue("@EndDate", EMonth);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                // this.Id = Convert.ToInt32(outValue); 
            }
            return status;
        }
        public DataTable MonthlyCarryForward(int companyId, DateTime SMonth, DateTime EMonth)
        {
            SqlCommand sqlCommand = new SqlCommand("USP_MonthlyCarryForwardSelect");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@StartDate", SMonth);
            sqlCommand.Parameters.AddWithValue("@EndDate", EMonth);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        #endregion

    }

    public class settingDropDown
    {
        public string id { get; set; }

        public string name { get; set; }
    }
}

