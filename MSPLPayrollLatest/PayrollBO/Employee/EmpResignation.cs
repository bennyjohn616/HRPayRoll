using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class EmpResignation
    {
        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public Guid EmpId { get; set; }

        /// <summary>
        /// Get or Set the RoleName
        /// </summary>
        public DateTime ResignationDate { get; set; }

        public DateTime LastWorkingDate { get; set; }

        /// <summary>
        /// Get or Set the DisplayAs
        /// </summary>
        public string Reason { get; set; }


        /// <summary>
        /// Get or Set the Description
        /// </summary>

        public bool Isdeleted { get; set; }
        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }


        public DateTime ModifiedOn { get; set; }

        public int ModifiedBy { get; set; }
        public int CompanyId { get; set; }

        public Guid CategoryId { get; set; }
        public string Category { get; set; }
        public string EmpCode { get; set; }
        #endregion

        public List<EmpResignation> EmpResignationList()
        {
            List<EmpResignation> empList = new List<PayrollBO.EmpResignation>();
            DataTable dt = GetEmployess();
            if (dt.Rows.Count>0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    EmpResignation empResg = new EmpResignation();
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Id"])))
                        empResg.Id = new Guid(Convert.ToString(dt.Rows[i]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["EmpId"])))
                        empResg.EmpId = new Guid(Convert.ToString(dt.Rows[i]["EmpId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["ResignationDate"])))
                        empResg.ResignationDate = Convert.ToDateTime(dt.Rows[i]["ResignationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["ResignationDate"])))
                        empResg.ResignationDate = Convert.ToDateTime(dt.Rows[i]["ResignationDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LastWorkingDate"])))
                        empResg.LastWorkingDate = Convert.ToDateTime(dt.Rows[i]["LastWorkingDate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Reason"])))
                        empResg.Reason = Convert.ToString(dt.Rows[i]["Reason"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["CategoryId"])))
                        empResg.CategoryId = new Guid(Convert.ToString(dt.Rows[i]["CategoryId"]));
                    empResg.Category = Convert.ToString(dt.Rows[i]["Category"]);
                    empResg.EmpCode= Convert.ToString(dt.Rows[i]["EmpCode"]);
                    empResg.Isdeleted = Convert.ToBoolean(dt.Rows[i]["Isdeleted"]);
                    empList.Add(empResg);
                }
            }


            return empList;
        }


        public DataTable GetEmployess()
        {

            //SqlCommand sqlCommand = new SqlCommand("Employee_SelectALL");
            SqlCommand sqlCommand = new SqlCommand("USP_Resignation");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@EmpId", this.EmpId);
            sqlCommand.Parameters.AddWithValue("@Type", "SELECT");
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public bool Save()
        {
            SqlCommand sqlCommand = new SqlCommand("USP_Resignation");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@EmpId", this.EmpId);
            sqlCommand.Parameters.AddWithValue("@ResignationDate", this.ResignationDate);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
           // sqlCommand.Parameters.AddWithValue("@CreatedOn", this.CreatedOn);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
        //    sqlCommand.Parameters.AddWithValue("@ModigiedOn", this.ModifiedOn);
            sqlCommand.Parameters.AddWithValue("@Reason", this.Reason);
            sqlCommand.Parameters.AddWithValue("@Type", "SAVE");
          //  sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.save(sqlCommand);
            return status;
        }

        public bool Delete()
        {
            SqlCommand sqlCommand = new SqlCommand("USP_Resignation");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);           
            sqlCommand.Parameters.AddWithValue("@Type", "DELETE");
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.save(sqlCommand);
            return status;
        }
    }
}
