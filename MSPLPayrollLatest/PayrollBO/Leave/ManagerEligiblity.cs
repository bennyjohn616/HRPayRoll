using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class ManagerEligiblity
    {

        #region property

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public ManagerEligiblity()
        {

        }

        public ManagerEligiblity(int CompanyId, int RoleId, Guid FinancialYear)
        {
            DataTable dtValue = this.GetTableValues( CompanyId,  RoleId,  FinancialYear);
            if (dtValue.Rows.Count > 0)
            {

                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                        this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RoleId"])))
                        this.RoleId = Convert.ToInt32(dtValue.Rows[0]["RoleId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                        this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FinanacialYear"])))
                        this.FinanacialYear = new Guid(Convert.ToString(dtValue.Rows[0]["FinanacialYear"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                        this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                        this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                }
            }
        }

        public ManagerEligiblity(int CompanyId,Guid FinancialYear)
        {
            bool stat=  this.save(CompanyId,FinancialYear);
            
        }


        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>



        #endregion


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        public String FieldName { get; set; }

        /// <summary>
        /// Get or Set the RoleId
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the TanNo
        /// </summary>
        public Guid FinanacialYear { get; set; }        
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
        
        
        
        #endregion

        #region Public methods


        /// <summary>
        /// Save the ManagerELigiblity
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("ManagerEligiblity_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@RoleId", this.RoleId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@FinanacialYear", this.FinanacialYear);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            var Dstatus = outValue;
            if (status)
            {
                this.Id = new Guid(outValue);
            }
            return status;

        }



        #endregion

        #region Public methods

        protected internal DataTable GetTableValues(int CompanyId, int RoleId, Guid FinancialYear)
        {

            SqlCommand sqlCommand = new SqlCommand("ManagerEligiblity_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@RoleId", RoleId);
            sqlCommand.Parameters.AddWithValue("@FinanacialYear", FinancialYear);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        #endregion
        protected internal bool save(int CompanyId, Guid FinancialYear)
        {

            SqlCommand sqlCommand = new SqlCommand("ManagerEligiblity_Update");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@FinanacialYear", FinancialYear);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.save(sqlCommand);
        }

    }
}
