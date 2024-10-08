using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRollReports
{
    public class Paysheet
    {
        public Paysheet()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="Id"></param>
        public Paysheet(int id,int companyId)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id,companyId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = Convert.ToInt32(Convert.ToString(dtValue.Rows[0]["Id"]));
                this.Description = Convert.ToString(dtValue.Rows[0]["Description"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
              
                this.Title = Convert.ToString(dtValue.Rows[0]["Title"]);
                this.Categories = Convert.ToString(dtValue.Rows[0]["Categories"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Status"])))
                    this.Status = Convert.ToBoolean(dtValue.Rows[0]["Status"]);
            }
        }
        #region "Properties"
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Description { get; set; }

        public string Title { get; set; }
        public string Categories { get; set; }
        public int CreatedBy { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public bool Status { get; set; }

        public bool IsDetail { get; set; }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the PaySlipSetting
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("PaySheet_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@Description", this.Description);                   
            sqlCommand.Parameters.AddWithValue("@Title", this.Title);
            sqlCommand.Parameters.AddWithValue("@Categories", this.Categories);          
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@Status", this.Status);
            sqlCommand.Parameters.AddWithValue("@IsDetail", this.IsDetail);
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
        /// Delete the PaySlipSetting
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("PaySheet_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);

            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the PaySlipSetting
        /// </summary>
        /// <param name="configurationid"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(int id, int companyId)
        {

            SqlCommand sqlCommand = new SqlCommand("PaySheet_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues1(int companyId)
        {

            SqlCommand sqlCommand = new SqlCommand("PaySheet_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        public bool Delete(int id)
        {
            SqlCommand sqlCommand = new SqlCommand("PaySheet_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);

            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
        #endregion

    }
}
