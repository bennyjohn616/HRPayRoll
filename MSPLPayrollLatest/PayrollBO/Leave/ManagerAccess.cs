using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class ManagerAccess
    {
        public ManagerAccess()
        {


        }
        public ManagerAccess(Guid id, int companyId)
        {

            this.Id = id;
            this.CompanyId = companyId;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ManagerId"])))
                    this.ManagerId = new Guid(Convert.ToString(dtValue.Rows[0]["ManagerId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Prioritylevel"])))
                    this.Prioritylevel = (Convert.ToInt32(dtValue.Rows[0]["Prioritylevel"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ApprovalMust"])))
                    this.ApprovalMust = (Convert.ToBoolean(dtValue.Rows[0]["ApprovalMust"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CancelRights"])))
                    this.CancelRights = (Convert.ToBoolean(dtValue.Rows[0]["CancelRights"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }

        }
        public ManagerAccess(Guid employeeId)
        {

            this.EmployeeId = employeeId;
           
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ManagerId"])))
                    this.ManagerId = new Guid(Convert.ToString(dtValue.Rows[0]["ManagerId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Prioritylevel"])))
                    this.Prioritylevel = (Convert.ToInt32(dtValue.Rows[0]["Prioritylevel"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ApprovalMust"])))
                    this.ApprovalMust = (Convert.ToBoolean(dtValue.Rows[0]["ApprovalMust"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CancelRights"])))
                    this.CancelRights = (Convert.ToBoolean(dtValue.Rows[0]["CancelRights"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }

        }
        #region "Properties"

        public Guid Id { get; set; }
        public Guid FinanceYearId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ManagerId { get; set; }
        public int Prioritylevel { get; set; }

        public bool ApprovalMust { get; set; }

        public bool CancelRights { get; set; }

        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }
        #endregion

        #region "Public Methods"
        /// <summary>
        /// Save the TaxBehavior
        /// </summary>
        /// <returns></returns>
        public  bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("LeaveManagerDetails_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@ManagerId", this.ManagerId);
            sqlCommand.Parameters.AddWithValue("@Prioritylevel", this.Prioritylevel);
            sqlCommand.Parameters.AddWithValue("@ApprovalMust", this.ApprovalMust);
            sqlCommand.Parameters.AddWithValue("@ApprovalMust", this.CancelRights);
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
        /// Delete the TaxBehavior
        /// </summary>
        /// <returns></returns>
        public  bool Delete()
        {
            SqlCommand sqlCommand = new SqlCommand("LeaveManagerDetails_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
        internal DataTable GetTableValues()
        {
            SqlCommand sqlCommand = new SqlCommand("LeaveManagerDetails_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", this.FinanceYearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@ManagerId", this.ManagerId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        #endregion
    }
}
