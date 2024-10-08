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
    public class VerifyProof
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public VerifyProof()
        {
        }
        public VerifyProof(Guid financeyearId, Guid employeeId)
        {
            this.EmployeeId = employeeId;
            this.financeyearId = financeyearId;
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
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Mailsent"])))
                        this.Mailsent = Convert.ToString(dtValue.Rows[rowcount]["Mailsent"]);
                }
            }
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
        public Guid VerifiedBy { get; set; }

        public string Mailsent { get; set; }

        #endregion
        #region Public methods


        /// <summary>
        /// Save the TXProjIncome
        /// </summary>
        /// <returns></returns>
        public bool Save(VerifyProof Verify)
        {

            SqlCommand sqlCommand = new SqlCommand("VerifyProof_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@financeyearId", Verify.financeyearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", Verify.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@SerialNo", Verify.SerialNo);
            sqlCommand.Parameters.AddWithValue("@Remarks", Verify.Remarks);
            sqlCommand.Parameters.AddWithValue("@Status", Verify.Status);
            sqlCommand.Parameters.AddWithValue("@VerifiedBy", Verify.VerifiedBy);
            VerifyDBOpeartion vdbOperation = new VerifyDBOpeartion();
            string outValue = string.Empty;
            bool status = vdbOperation.SaveData(sqlCommand, out outValue, "");
            return status;
        }


        public bool SaveMailSent(VerifyProof Verify)
        {

            SqlCommand sqlCommand = new SqlCommand("VerifyProof_SaveMail");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@financeyearId", Verify.financeyearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", Verify.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@SerialNo", Verify.SerialNo);
            sqlCommand.Parameters.AddWithValue("@Mailsent", Verify.Mailsent);
            VerifyDBOpeartion vdbOperation = new VerifyDBOpeartion();
            string outValue = string.Empty;
            bool status = vdbOperation.SaveData(sqlCommand, out outValue, "");
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

            SqlCommand sqlCommand = new SqlCommand("VerifyProof_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@financeyearId", this.financeyearId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            VerifyDBOpeartion vdbOperation = new VerifyDBOpeartion();
            return vdbOperation.GetTableData(sqlCommand);
        }

        #endregion

    }

}
