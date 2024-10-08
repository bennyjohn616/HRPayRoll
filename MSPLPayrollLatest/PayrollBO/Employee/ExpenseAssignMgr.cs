using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollBO
{
    public class ExpenseAssignMgr : List<ExpenseAssignMgr>
    {
        public ExpenseAssignMgr()
        {

        }
        public Guid Id { set; get; }
        public Guid EmployeeId { get; set; }
        public string empCode { get; set; }
        public Guid FinYear { get; set; }
        public string MgrEmpCode { get; set; }
        public string MgrEmpName { get; set; }
        public int CompanyId { get; set; }
        public int Loginid { get; set; }
        public int ApprovMust { get; set; }
        public string ApprovMustString { get; set; }
        public string AppCancelRightString { get; set; }
        public int MgrPriority { get; set; }
        public int AppCancelRights { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Email { get; set; }
        public Guid AssEmpId { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string ExpenseStatus { get; set; }
        public int ManagerPriority { get; set; }
        public Guid Existingmgrid { get; set; }
        public Guid Changemgrid { get; set; }
        public ExpenseAssignMgr(Guid EmployeeId, int CompanyId, int temp, Guid finyear)
        {

            this.Id = Id;
            this.EmployeeId = EmployeeId;
            this.CompanyId = CompanyId;

            DataTable dtValue = this.GetTableValues(EmployeeId, CompanyId, finyear);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount1 = 0; rowcount1 < dtValue.Rows.Count; rowcount1++)
                {
                    ExpenseAssignMgr AssMgr1 = new ExpenseAssignMgr();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["EmployeeId"])))
                        AssMgr1.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount1]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["AssignEmployeeId"])))
                        AssMgr1.AssEmpId = new Guid(Convert.ToString(dtValue.Rows[rowcount1]["AssignEmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["Email"])))
                        AssMgr1.Email = Convert.ToString(dtValue.Rows[rowcount1]["Email"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["MgrPriority"])))
                        AssMgr1.MgrPriority = Convert.ToInt32(dtValue.Rows[rowcount1]["MgrPriority"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["CompanyId"])))
                        AssMgr1.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount1]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["MgrEmpCode"])))
                        AssMgr1.MgrEmpCode = Convert.ToString(dtValue.Rows[rowcount1]["MgrEmpCode"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["MgrEmpName"])))
                        AssMgr1.MgrEmpName = Convert.ToString(dtValue.Rows[rowcount1]["MgrEmpName"]);
                    this.Add(AssMgr1);
                }
            }
        }
        public ExpenseAssignMgr(Guid EmployeeId, int CompanyId, Guid FinYear, Guid id, int appmust)
        {
            DataTable dtValue = this.GetTableValues(EmployeeId, CompanyId, FinYear, id, appmust);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    ExpenseAssignMgr AssMgr = new ExpenseAssignMgr();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        AssMgr.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        AssMgr.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinYear"])))
                        AssMgr.FinYear = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinYear"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AssignManagerId"])))
                        AssMgr.AssEmpId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AssignManagerId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ManagerCode"])))
                        AssMgr.MgrEmpCode = (Convert.ToString(dtValue.Rows[rowcount]["ManagerCode"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ManagerName"])))
                        AssMgr.MgrEmpName = (Convert.ToString(dtValue.Rows[rowcount]["ManagerName"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApprovMustString"])))
                        AssMgr.ApprovMustString = (Convert.ToString(dtValue.Rows[rowcount]["ApprovMustString"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AppCancelRightsString"])))
                        AssMgr.AppCancelRightString = (Convert.ToString(dtValue.Rows[rowcount]["AppCancelRightsString"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AssignManagerId"])))
                        AssMgr.AssEmpId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AssignManagerId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApprovalMust"])))
                        AssMgr.ApprovMust = (Convert.ToInt32(dtValue.Rows[rowcount]["ApprovalMust"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AppCancelRights"])))
                        AssMgr.AppCancelRights = (Convert.ToInt32(dtValue.Rows[rowcount]["AppCancelRights"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ManagerPriority"])))
                        AssMgr.MgrPriority = (Convert.ToInt32(dtValue.Rows[rowcount]["ManagerPriority"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        AssMgr.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    this.Add(AssMgr);
                }

            }
        }
        public bool SaveAssignMgrData()
        {

            SqlCommand sqlCommand = new SqlCommand("ExpenseAssignMang_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@FinYear", this.FinYear);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@AssignManagerId", this.AssEmpId);
            sqlCommand.Parameters.AddWithValue("@ApprovalMust", this.ApprovMust);
            sqlCommand.Parameters.AddWithValue("@ManagerPriority", this.MgrPriority);
            sqlCommand.Parameters.AddWithValue("@AppCancelRights", this.AppCancelRights);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.EmployeeId = new Guid(Convert.ToString(outValue));
            }
            return status;
        }
        internal DataTable GetTableValues(Guid EmployeeId, int CompanyId, Guid FinYear, Guid id, int appmst)
        {
            SqlCommand sqlCommand = new SqlCommand("ExpenseAssignMgr_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", FinYear);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeId);
            sqlCommand.Parameters.AddWithValue("@ApprovMust", appmst);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);

        }
        internal DataTable GetTableValues(Guid EmployeeId, int CompanyId, Guid finyr)
        {
            SqlCommand sqlCommand = new SqlCommand("ExpenseAssignManager_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", finyr);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            //sqlCommand.Parameters.AddWithValue("@Email", this.Email);
            //sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public bool Delete()
        {
            SqlCommand sqlCommand = new SqlCommand("ExpenseAssignMgr_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
    }
}
