using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class LeaveType
    {
        public LeaveType()
        {

        }
        public LeaveType(Guid id,int companyId,Guid Finyear)
        {

            this.Id = id;
            this.CompanyId = companyId;
            this.FinyrId = Finyear;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int i = 0; i < dtValue.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                        this.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));

                    this.LeaveTypeName = Convert.ToString(dtValue.Rows[i]["LeaveType"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CompanyId"])))
                        this.CompanyId = Convert.ToInt32(dtValue.Rows[i]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                        this.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                        this.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                        this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                        this.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDeleted"])))
                        this.IsDeleted = Convert.ToBoolean(dtValue.Rows[i]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Colour"])))
                      this.colour = Convert.ToString(dtValue.Rows[i]["Colour"]);
                }
            }

        }
        public Guid Id { get; set; }
        public Guid FinyrId { get; set; }
        public int CompanyId { get; set; }
        public string LeaveTypeName { get; set; }
        public int CreatedBy { get; set; }
        public string colour { get; set; }
        public string LeaveDescription { get; set; }
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
       
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("LeaveType_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@LeaveType", this.LeaveTypeName);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@CreatedOn", this.CreatedOn);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedOn", this.ModifiedOn);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@colour", this.colour);

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
        /// Delete the TaxBehavior
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            SqlCommand sqlCommand = new SqlCommand("LeaveType_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);            
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
        internal DataTable GetTableValues()
        {
            SqlCommand sqlCommand = new SqlCommand("LeaveType_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@FinyrId", this.FinyrId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        internal DataTable GetMasterTableValues()
        {
            SqlCommand sqlCommand = new SqlCommand("MasterLeaveType_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
    }
}
