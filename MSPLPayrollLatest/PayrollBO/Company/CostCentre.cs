// -----------------------------------------------------------------------
// <copyright file="CostCentre.cs" company="Microsoft">
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
    /// To handle the CostCentre
    /// </summary>
    public class CostCentre
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public CostCentre()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public CostCentre(Guid id,int companyId)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(companyId, this.Id);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                this.CostCentreName = Convert.ToString(dtValue.Rows[0]["CostCentreName"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the CostCentreName
        /// </summary>
        public string CostCentreName { get; set; }

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
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }


        #endregion

        #region Public methods


        /// <summary>
        /// Save the CostCentre
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("CostCentre_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@CostCentreName", this.CostCentreName);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            //sqlCommand.Parameters.Add("@ReturnOut", SqlDbType.Bit).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            //List<string> outValue = new List<string>();
            //List<string> outKey= new List<string>();
            //outKey.Add("@IdOut");
            //outKey.Add("@ReturnOut");
            //bool status = dbOperation.SaveData(sqlCommand, out outValue, outKey);
            if (status)
            {
                //this.Id = new Guid(outValue[0]);
                this.Id = new Guid(outValue);

            }
            return status;
         
         
        }

        /// <summary>
        /// Delete the CostCentre
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("CostCentre_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the CostCentre
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(int companyId, Guid id)
        {

            SqlCommand sqlCommand = new SqlCommand("CostCentre_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

