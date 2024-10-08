// -----------------------------------------------------------------------
// <copyright file="Category.cs" company="Microsoft">
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
    /// To handle the Category
    /// </summary>
    public class Category
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public Category()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public Category(Guid id, int companyId)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(companyId, this.Id);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                this.Name = Convert.ToString(dtValue.Rows[0]["Name"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DisOrder"])))
                    this.DisOrder = Convert.ToInt32(dtValue.Rows[0]["DisOrder"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreaateBy"])))
                    this.CreaateBy = Convert.ToInt32(dtValue.Rows[0]["CreaateBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CretedOn"])))
                    this.CretedOn = Convert.ToDateTime(dtValue.Rows[0]["CretedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PFLimit"])))
                    this.PFLimit = Convert.ToDecimal(dtValue.Rows[0]["PFLimit"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PFProcess"])))
                    this.PFProcess = Convert.ToString(dtValue.Rows[0]["PFProcess"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PFInspectionChargeProcess"])))
                    this.PFInspectionChargeProcess = Convert.ToString(dtValue.Rows[0]["PFInspectionChargeProcess"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PFInspectionLimit"])))
                    this.PFInspectionLimit = Convert.ToDecimal(dtValue.Rows[0]["PFInspectionLimit"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PFFPFProcess"])))
                    this.PFFPFProcess = Convert.ToString(dtValue.Rows[0]["PFFPFProcess"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PFAdminChargeProcess"])))
                    this.PFAdminChargeProcess = Convert.ToString(dtValue.Rows[0]["PFAdminChargeProcess"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PFAdminLimit"])))
                    this.PFAdminLimit = Convert.ToDecimal(dtValue.Rows[0]["PFAdminLimit"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PFEdliChargeProcess"])))
                    this.PFEdliChargeProcess = Convert.ToString(dtValue.Rows[0]["PFEdliChargeProcess"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PFEdliLimit"])))
                    this.PFEdliLimit = Convert.ToDecimal(dtValue.Rows[0]["PFEdliLimit"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PFRounding"])))
                    this.PFRounding = Convert.ToString(dtValue.Rows[0]["PFRounding"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ESILimit"])))
                    this.ESILimit = Convert.ToDecimal(dtValue.Rows[0]["ESILimit"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ESIProcess"])))
                    this.ESIProcess = Convert.ToString(dtValue.Rows[0]["ESIProcess"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ESIRounding"])))
                    this.ESIRounding = Convert.ToString(dtValue.Rows[0]["ESIRounding"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ESIInspectionChargeProcess"])))
                    this.ESIInspectionChargeProcess = Convert.ToString(dtValue.Rows[0]["ESIInspectionChargeProcess"]);            
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ESIEdliChargeProcess"])))
                    this.ESIEdliChargeProcess = Convert.ToString(dtValue.Rows[0]["ESIEdliChargeProcess"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["MonthDayProcess"])))
                    this.MonthDayProcess = Convert.ToString(dtValue.Rows[0]["MonthDayProcess"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["MonthDayOrStartDay"])))
                    this.MonthDayOrStartDay = Convert.ToInt32(dtValue.Rows[0]["MonthDayOrStartDay"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or Set the DisplayOrder
        /// </summary>
        public int DisOrder { get; set; }

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Get or Set the CreaateBy
        /// </summary>
        public int CreaateBy { get; set; }

        /// <summary>
        /// Get or Set the CretedOn
        /// </summary>
        public DateTime CretedOn { get; set; }

        /// <summary>
        /// Get or Set the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }

        /// <summary>
        /// Get or Set the ModifiedOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        ///Get or set the PFLimit
        public Decimal PFLimit { get; set; }

        ///Get or set the PFProcess
        public string PFProcess { get; set; }

        ///Get or set the PFInspectionChargeProcess
        public string PFInspectionChargeProcess { get; set; }
        ///Get or set the PFLimit
        public Decimal PFInspectionLimit { get; set; }

        ///Get or set the PFFPFProcess 
        public string PFFPFProcess { get; set; }

        ///Get or set the PFAdminChargeProcess 
        public string PFAdminChargeProcess { get; set; }
        ///Get or set the PFLimit
        public Decimal PFAdminLimit { get; set; }

        ///Get or set the PFEdliChargeProcess
        public string PFEdliChargeProcess { get; set; }
        ///Get or set the PFLimit
        public Decimal PFEdliLimit { get; set; }

        ///Get or set the PFRounding
        public string PFRounding { get; set; }

        ///Get or set the ESILimit
        public decimal ESILimit { get; set; }

        ///Get or set the ESIProcess
        public string ESIProcess { get; set; }

        ///Get or set the ESIRounding
        public string ESIRounding { get; set; }

        ///Get or set the ESIInspectionChargeProcess
        public string ESIInspectionChargeProcess { get; set; }

        ///Get or set the ESIAdminChargeProcess
        public string ESIAdminChargeProcess { get; set; }

        ///Get or set the ESIEdliChargeProcess
        public string ESIEdliChargeProcess { get; set; }

        ///Get or set the MonthDayProcess
        public string MonthDayProcess { get; set; }

        ///Get or set the MonthDayOrStartDay
        public int MonthDayOrStartDay { get; set; }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the Category
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("Category_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@Name", this.Name);
            sqlCommand.Parameters.AddWithValue("@DisOrder", this.DisOrder);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@IsActive", this.IsActive);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.AddWithValue("@CreaateBy", this.CreaateBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = new Guid(Convert.ToString(outValue));
            }
            return status;
        }

        ///Save the category setting

        public bool Savesetting()
        {

            SqlCommand sqlCommand = new SqlCommand("Category_SaveSetting");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@PFLimit", this.PFLimit);
            sqlCommand.Parameters.AddWithValue("@PFProcess", this.PFProcess);
            sqlCommand.Parameters.AddWithValue("@PFInspectionChargeProcess", this.PFInspectionChargeProcess);
            sqlCommand.Parameters.AddWithValue("@PFInspectionLimit", this.PFInspectionLimit);
            sqlCommand.Parameters.AddWithValue("@PFFPFProcess", this.PFFPFProcess);
            sqlCommand.Parameters.AddWithValue("@PFAdminChargeProcess", this.PFAdminChargeProcess);
            sqlCommand.Parameters.AddWithValue("@PFAdminLimit", this.PFAdminLimit);
            sqlCommand.Parameters.AddWithValue("@PFEdliChargeProcess", this.PFEdliChargeProcess);
            sqlCommand.Parameters.AddWithValue("@PFEdliLimit", this.PFEdliLimit);
            sqlCommand.Parameters.AddWithValue("@PFRounding", this.PFRounding);
            sqlCommand.Parameters.AddWithValue("@ESILimit", this.ESILimit);
            sqlCommand.Parameters.AddWithValue("@ESIProcess", this.ESIProcess);
            sqlCommand.Parameters.AddWithValue("@ESIRounding", this.ESIRounding);
            sqlCommand.Parameters.AddWithValue("@ESIInspectionChargeProcess", this.ESIInspectionChargeProcess);
            sqlCommand.Parameters.AddWithValue("@ESIAdminChargeProcess", this.ESIAdminChargeProcess);
            sqlCommand.Parameters.AddWithValue("@ESIEdliChargeProcess", this.ESIEdliChargeProcess);
            sqlCommand.Parameters.AddWithValue("@MonthDayProcess", this.MonthDayProcess);
            sqlCommand.Parameters.AddWithValue("@MonthDayOrStartDay", this.MonthDayOrStartDay);
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            return dbOperation.SaveData(sqlCommand, out outValue, "");
          
        }
        /// <summary>
        /// Delete the Category
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("Category_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(int companyId, Guid id)
        {
            SqlCommand sqlCommand = new SqlCommand("Category_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

