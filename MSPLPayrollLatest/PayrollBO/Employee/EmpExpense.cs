using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SQLDBOperation;

namespace PayrollBO
{
    public class EmpExpense
    {
        #region "Constructor"

        #endregion
        #region "Properties"
        public Guid ID { get; set; }
        public string EmployeeID { get; set; }
        public string CostCenter { get; set; }
        public string CostCenterMgr { get; set; }
        public string PurposeForExpense { get; set; }
        public string CategeroyOfExpense { get; set; }
        public string DescriptExpense { get; set; }
        public DateTime DateOfExpense { get; set; }
        public DateTime SubmitDate { get; set; }
        public string Status { get; set; }
        public string Attachment { get; set; }
        public string CostOfExpense { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBY { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int CompanyId { get; set; }
        #endregion

        #region "Method"

        public List<EmpExpense> GetExpenses(Guid id, string EmpCode)
        {
            List<EmpExpense> empExpenses = new List<EmpExpense>();
            DataTable dt = this.GetExpense(id, EmpCode);
            if (dt.Rows.Count > 0)
            {
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    EmpExpense expense = new EmpExpense();
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["id"])))
                        expense.ID = new Guid(Convert.ToString(dt.Rows[i]["id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["EmployeeID"])))
                        expense.EmployeeID = Convert.ToString(dt.Rows[i]["EmployeeID"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["CostCenter"])))
                        expense.CostCenter = Convert.ToString(dt.Rows[i]["CostCenter"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["CostCenterManager"])))
                        expense.CostCenterMgr = Convert.ToString(dt.Rows[i]["CostCenterManager"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["PurposeForExpense"])))
                        expense.PurposeForExpense = Convert.ToString(dt.Rows[i]["PurposeForExpense"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["CategoryOfExpense"])))
                        expense.CategeroyOfExpense = Convert.ToString(dt.Rows[i]["CategoryOfExpense"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["DescriptionOfExpense"])))
                        expense.DescriptExpense = Convert.ToString(dt.Rows[i]["DescriptionOfExpense"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["DateOfExpense"])))
                        expense.DateOfExpense = Convert.ToDateTime(dt.Rows[i]["DateOfExpense"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["DateSubmitted"])))
                        expense.SubmitDate = Convert.ToDateTime(dt.Rows[i]["DateSubmitted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Status"])))
                        expense.Status = Convert.ToString(dt.Rows[i]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Attachments"])))
                        expense.Attachment = Convert.ToString(dt.Rows[i]["Attachments"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["CostOfExpense"])))
                        expense.CostOfExpense = Convert.ToString(dt.Rows[i]["CostOfExpense"]);
                    empExpenses.Add(expense);

                }
            }
            return empExpenses;
        }



        protected internal DataTable GetExpense(Guid Id, string EmployeeID)
        {
            SqlCommand sqlCommand = new SqlCommand("Emp_Expense_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@ID", Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeID);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public bool Save()
        {
            SqlCommand sqlCommand = new SqlCommand("Emp_Expense_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@ID", this.ID);
            sqlCommand.Parameters.AddWithValue("@EmployeeID", this.EmployeeID);
            sqlCommand.Parameters.AddWithValue("@CostCenterManager", this.CostCenterMgr);
            sqlCommand.Parameters.AddWithValue("@CostCenter", this.CostCenter);
            sqlCommand.Parameters.AddWithValue("@CostOfExpense", this.CostOfExpense);
            sqlCommand.Parameters.AddWithValue("@CategoryOfExpense", this.CategeroyOfExpense);
            sqlCommand.Parameters.AddWithValue("@DescriptionOfExpense", this.DescriptExpense);
            sqlCommand.Parameters.AddWithValue("@DateOfExpense", this.DateOfExpense);
            sqlCommand.Parameters.AddWithValue("@DateSubmitted", this.SubmitDate);
            sqlCommand.Parameters.AddWithValue("@Attachments", this.Attachment);
            sqlCommand.Parameters.AddWithValue("@PurposeForExpense", this.PurposeForExpense);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            
            sqlCommand.Parameters.Add("@Guid", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dB = new DBOperation();
            string OutValue = string.Empty;
            bool Status = dB.SaveData(sqlCommand, out OutValue, "@Guid");
            if(Status)
            {
                this.ID = new Guid(OutValue);
            }
            return Status;
        }
        #endregion
    }
}
