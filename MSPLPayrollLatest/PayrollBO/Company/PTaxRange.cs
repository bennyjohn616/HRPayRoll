// -----------------------------------------------------------------------
// <copyright file="PTaxRange.cs" company="Microsoft">
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
    /// To handle the PTaxRange
    /// </summary>
    public class PTaxRange
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public PTaxRange()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public PTaxRange(Guid id, Guid PTaxId)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id, PTaxId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PTaxId"])))
                    this.PTaxId = new Guid(Convert.ToString(dtValue.Rows[0]["PTaxId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RangeFrom"])))
                    this.RangeFrom = Convert.ToDecimal(dtValue.Rows[0]["RangeFrom"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RangeTo"])))
                    this.RangeTo = Convert.ToDecimal(dtValue.Rows[0]["RangeTo"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Amt"])))
                    this.Amt = Convert.ToDecimal(dtValue.Rows[0]["Amt"]);
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


        public PTaxRange(Guid ptaxId, double EarningsAmt)
        {
            this.Id = ptaxId;
            DataTable dtValue = this.GetTableValues(Id, EarningsAmt);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PTaxId"])))
                    this.PTaxId = new Guid(Convert.ToString(dtValue.Rows[0]["PTaxId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RangeFrom"])))
                    this.RangeFrom = Convert.ToDecimal(dtValue.Rows[0]["RangeFrom"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RangeTo"])))
                    this.RangeTo = Convert.ToDecimal(dtValue.Rows[0]["RangeTo"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Amt"])))
                    this.Amt = Convert.ToDecimal(dtValue.Rows[0]["Amt"]);
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
        /// Get or Set the PTaxId
        /// </summary>
        public Guid PTaxId { get; set; }

        /// <summary>
        /// Get or Set the RangeFrom
        /// </summary>
        public Decimal RangeFrom { get; set; }

        /// <summary>
        /// Get or Set the RangeTo
        /// </summary>
        public Decimal RangeTo { get; set; }

        /// <summary>
        /// Get or Set the Amt
        /// </summary>
        public Decimal Amt { get; set; }

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
        /// Save the PTaxRange
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("PTaxRange_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@PTaxId", this.PTaxId);
            sqlCommand.Parameters.AddWithValue("@RangeFrom", this.RangeFrom);
            sqlCommand.Parameters.AddWithValue("@RangeTo", this.RangeTo);
            sqlCommand.Parameters.AddWithValue("@Amt", this.Amt);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = new Guid(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the PTaxRange
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("PTaxRange_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the PTaxRange
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, Guid PTaxId)
        {

            SqlCommand sqlCommand = new SqlCommand("PTaxRange_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@PTaxId", PTaxId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        protected internal DataTable GetTableValues(Guid id, double EarningsAmt)
        {
            SqlCommand sqlCommand = new SqlCommand("select * From PTaxRange where PTaxId ='" + id + "'  and " + EarningsAmt + " between RangeFrom and RangeTo");
            sqlCommand.CommandType = CommandType.Text;
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        #endregion

    }
}

