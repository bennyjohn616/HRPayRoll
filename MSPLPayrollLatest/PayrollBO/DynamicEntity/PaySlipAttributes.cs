using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDBOperation;
using System.Data.SqlClient;
using System.Data;


namespace PayrollBO
{

    /// <summary>
    /// To handle the PaySlipAttributes
    /// </summary>
    public class PaySlipAttributes
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public PaySlipAttributes()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public PaySlipAttributes(Guid id)
        {
            this.CofigurationId = id;
            DataTable dtValue = this.GetTableValues(this.CofigurationId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CofigurationId"])))
                    this.CofigurationId = new Guid(Convert.ToString(dtValue.Rows[0]["CofigurationId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CategoryId"])))
                    this.CategoryId = new Guid(Convert.ToString(dtValue.Rows[0]["CategoryId"]));
                this.TableName = Convert.ToString(dtValue.Rows[0]["TableName"]);
                this.FieldName = Convert.ToString(dtValue.Rows[0]["FieldName"]);
                this.DisplayAs = Convert.ToString(dtValue.Rows[0]["DisplayAs"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["HeaderDisplayOrder"])))
                    this.HeaderDisplayOrder = Convert.ToInt32(dtValue.Rows[0]["HeaderDisplayOrder"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FooterDisplayOrder"])))
                    this.FooterDisplayOrder = Convert.ToInt32(dtValue.Rows[0]["FooterDisplayOrder"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FandFHeaderDisplayOrder"])))
                    this.FandFHeaderDisplayOrder = Convert.ToInt32(dtValue.Rows[0]["FandFHeaderDisplayOrder"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EarningDisplayOrder"])))
                    this.EarningDisplayOrder = Convert.ToInt32(dtValue.Rows[0]["EarningDisplayOrder"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DeductionDisplayOrder"])))
                    this.DeductionDisplayOrder = Convert.ToInt32(dtValue.Rows[0]["DeductionDisplayOrder"]);
                this.Type = Convert.ToString(dtValue.Rows[0]["Type"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsPhysicalTable"])))
                    this.IsPhysicalTable = Convert.ToBoolean(dtValue.Rows[0]["IsPhysicalTable"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get or Set the CofigurationId
        /// </summary>
        public Guid CofigurationId { get; set; }

        public Guid AttributeId { get; set; }

        /// <summary>
        /// Get or Set the CategoryId
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Get or Set the TableName
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Get or Set the FieldName
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Get or Set the HeaderDisplayOrder
        /// </summary>
        public int HeaderDisplayOrder { get; set; }

        /// <summary>
        /// Get or Set the FooterDisplayOrder
        /// </summary>
        public int FooterDisplayOrder { get; set; }

        public int FandFHeaderDisplayOrder { get; set; }
        /// <summary>
        /// Get or Set the EarningDisplayOrder
        /// </summary>
        public int EarningDisplayOrder { get; set; }

        /// <summary>
        /// Get or Set the DeductionDisplayOrder
        /// </summary>
        public int DeductionDisplayOrder { get; set; }

        /// <summary>
        /// Get or Set the Type
        /// </summary>
        public string Type { get; set; }
        public string Data_Type { get; set; }


        /// <summary>
        /// Get or Set the IsPhysicalTable
        /// </summary>
        public bool IsPhysicalTable { get; set; }

        /// <summary>
        /// Get or Set the Type
        /// </summary>
        public string DisplayAs { get; set; }
        public bool IsIncludeGrossPay { get; set; }

        public Guid MatchingId { get; set; }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the PaySlipAttributes
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("PaySlipAttributes_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CofigurationId", this.CofigurationId);
            sqlCommand.Parameters.AddWithValue("@CategoryId", this.CategoryId);
            sqlCommand.Parameters.AddWithValue("@TableName", this.TableName);
            sqlCommand.Parameters.AddWithValue("@FieldName", this.FieldName);
            sqlCommand.Parameters.AddWithValue("@HeaderDisplayOrder", this.HeaderDisplayOrder);
            sqlCommand.Parameters.AddWithValue("@FooterDisplayOrder", this.FooterDisplayOrder);
            sqlCommand.Parameters.AddWithValue("@FandFHeaderDisplayOrder", this.FandFHeaderDisplayOrder);
            sqlCommand.Parameters.AddWithValue("@EarningDisplayOrder", this.EarningDisplayOrder);
            sqlCommand.Parameters.AddWithValue("@DeductionDisplayOrder", this.DeductionDisplayOrder);
            sqlCommand.Parameters.AddWithValue("@Type", this.Type);
            sqlCommand.Parameters.AddWithValue("@IsPhysicalTable", this.IsPhysicalTable);
            sqlCommand.Parameters.AddWithValue("@DisplayAs", this.DisplayAs);
            sqlCommand.Parameters.AddWithValue("@MatchingId", this.MatchingId);
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
        /// Delete the PaySlipAttributes
        /// </summary>
        /// <returns></returns>
        public bool Delete(Guid CofigurationId)
        {

            SqlCommand sqlCommand = new SqlCommand("PaySlipAttributes_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CofigurationId", CofigurationId);

            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the PaySlipAttributes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id)
        {

            SqlCommand sqlCommand = new SqlCommand("PaySlipAttributes_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CofigurationId", id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

