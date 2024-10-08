// -----------------------------------------------------------------------
// <copyright file="AttributeModelBehavior.cs" company="Microsoft">
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
    /// To handle the AttributeModelBehavior
    /// </summary>
    public class AttributeModelBehavior
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public AttributeModelBehavior()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="attrubutemodelid"></param>
        public AttributeModelBehavior(Guid attrubutemodelid, Guid categoryId, int companyId)
        {
            this.AttrubuteModelId = attrubutemodelid;
            this.CategoryId = categoryId;
            this.CompanyId = companyId;
            DataTable dtValue = this.GetTableValues(this.AttrubuteModelId, this.CategoryId, this.CompanyId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttrubuteModelId"])))
                    this.AttrubuteModelId = new Guid(Convert.ToString(dtValue.Rows[0]["AttrubuteModelId"]));
                //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CategoryId"])))
                //    this.CategoryId = new Guid(Convert.ToString(dtValue.Rows[0]["CategoryId"]));
                //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                //    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ValueType"])))
                //    this.ValueType = Convert.ToInt32(dtValue.Rows[0]["ValueType"]);
                this.Formula = Convert.ToString(dtValue.Rows[0]["Formula"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Percentage"])))
                    this.Percentage = Convert.ToDecimal(dtValue.Rows[0]["Percentage"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Maximum"])))
                    this.Maximum = Convert.ToDecimal(dtValue.Rows[0]["Maximum"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RoundingId"])))
                    this.RoundingId = Convert.ToInt32(dtValue.Rows[0]["RoundingId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the AttrubuteModelId
        /// </summary>
        public Guid AttrubuteModelId { get; set; }

        /// <summary>
        /// Get or Set the CategoryId
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the ValueType
        /// </summary>
        public int ValueType { get; set; }

        /// <summary>
        /// Get or Set the Formula
        /// </summary>
        public string Formula { get; set; }

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
        /// Get or Set the IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }


        #endregion

        #region Public methods


        /// <summary>
        /// Save the AttributeModelBehavior
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            SqlCommand sqlCommand = new SqlCommand("AttributeModelBehavior_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@AttrubuteModelId", this.AttrubuteModelId);
            sqlCommand.Parameters.AddWithValue("@CategoryId", this.CategoryId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@ValueType", this.ValueType);
            sqlCommand.Parameters.AddWithValue("@Formula", this.Formula);
            sqlCommand.Parameters.AddWithValue("@Percentage", this.Percentage);
            sqlCommand.Parameters.AddWithValue("@Maximum", this.Maximum);
            sqlCommand.Parameters.AddWithValue("@RoundingId", this.RoundingId);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsActive", this.IsActive);
            sqlCommand.Parameters.Add("@AttrubuteModelIdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@AttrubuteModelIdOut");
            if (status)
            {
                this.AttrubuteModelId = new Guid(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the AttributeModelBehavior
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("AttributeModelBehavior_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@AttrubuteModelId", this.AttrubuteModelId);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the AttributeModelBehavior
        /// </summary>
        /// <param name="attrubutemodelid"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid attrubutemodelid, Guid categoryId, int companyId)
        {

            SqlCommand sqlCommand = new SqlCommand("AttributeModelBehavior_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@AttrubuteModelId", attrubutemodelid);
            sqlCommand.Parameters.AddWithValue("@CategoryId", categoryId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

