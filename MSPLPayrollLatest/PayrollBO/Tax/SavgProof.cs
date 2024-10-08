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
    public class SavgProof
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public SavgProof()
        {
        }
        public SavgProof(Guid financeyearId, Guid employeeId, int companyId)
        {
            this.EmployeeId = employeeId;
            this.financeyearId = financeyearId;
            this.companyId = companyId;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["companyId"])))
                        this.companyId = Convert.ToInt32(dtValue.Rows[rowcount]["companyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["financeyearId"])))
                        this.financeyearId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["financeyearId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmpId"])))
                        this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmpId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["SerialNo"])))
                        this.SerialNo = Convert.ToInt32(dtValue.Rows[rowcount]["SerialNo"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Description"])))
                        this.Description = Convert.ToString(dtValue.Rows[rowcount]["Description"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Uploaddate"])))
                        this.Uploaddate = Convert.ToDateTime(dtValue.Rows[rowcount]["Uploaddate"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Status"])))
                        this.Status = Convert.ToString(dtValue.Rows[rowcount]["Status"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Remarks"])))
                        this.Remarks = Convert.ToString(dtValue.Rows[rowcount]["Remarks"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["filename"])))
                        this.Filename = Convert.ToString(dtValue.Rows[rowcount]["filename"]);
                }
            }
        }

        public bool Delete()
        {
            SqlCommand sqlCommand = new SqlCommand("SavgProof_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@companyId", this.companyId);
            sqlCommand.Parameters.AddWithValue("@financeyearId", this.financeyearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@SerialNo", this.SerialNo);
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "");
            var Dstatus = outValue;
            return status;
        }


        #endregion

        #region property

        public Guid Id { get; set; }
        /// <summary>
        /// Get or Set the Id
        /// </summary>
        /// 
        public int companyId { get; set; }

        public Guid financeyearId { get; set; }
        public Guid EmployeeId { get; set; }
        public int SerialNo { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Filename { get; set; }

        public DateTime Uploaddate { get; set; }

        #endregion
        #region Public methods


        /// <summary>
        /// Save the TXProjIncome
        /// </summary>
        /// <returns></returns>
        public bool Save(SavgProof savg)
        {

            SqlCommand sqlCommand = new SqlCommand("SavgProof_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@companyId", savg.companyId);
            sqlCommand.Parameters.AddWithValue("@financeyearId", savg.financeyearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", savg.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@SerialNo", savg.SerialNo);
            sqlCommand.Parameters.AddWithValue("@Description", savg.Description);
            sqlCommand.Parameters.AddWithValue("@filename", savg.Filename);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            var Dstatus = outValue;
            return status;
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the TXEmployeeSection
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues()
        {

            SqlCommand sqlCommand = new SqlCommand("SavgProof_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@companyId", this.companyId);
            sqlCommand.Parameters.AddWithValue("@financeyearId", this.financeyearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        #endregion

    }

}
