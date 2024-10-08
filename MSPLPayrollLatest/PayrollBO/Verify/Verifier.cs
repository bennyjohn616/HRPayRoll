using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDBOperation;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using TraceError;


namespace PayrollBO
{
    /// <summary>
    /// To handle the Company
    /// </summary>
    public class Verifier
    {


        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public Verifier()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public Verifier(Guid finyear, Guid empid)
        {
            DataTable dtValue = this.GetTableValues(finyear, empid);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["VerifierID"])))
                    this.VerifierID = new Guid(Convert.ToString(dtValue.Rows[0]["VerifierID"]));
                this.finyear = new Guid(Convert.ToString(dtValue.Rows[0]["FinanceYear"]));
                this.CompanyName = Convert.ToString(dtValue.Rows[0]["CompanyName"]);
                this.MailID = Convert.ToString(dtValue.Rows[0]["MailID"]);
                this.DBConnectionId = Convert.ToInt32(dtValue.Rows[0]["DBConnectionID"]);
                this.FirstName = Convert.ToString(dtValue.Rows[0]["FirstName"]);
            }
        }

        /// <summary>
        /// Get or Set the AddressLine2
        /// </summary>
        public Guid finyear { get; set; }
        public Guid VerifierID { get; set; }
        public string CompanyName { get; set; }
        public int DBConnectionId { get; set; }
        public string MailID { get; set; }

        public string FirstName { get; set; }

        /// <summary>
        /// Get or Set the State
        /// </summary>
        protected internal DataTable GetTableValues(Guid finyear, Guid empid)
        {

            SqlCommand sqlCommand = new SqlCommand("Verifier_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@finyear", finyear);
            sqlCommand.Parameters.AddWithValue("@empid", empid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues(Guid finyear)
        {

            SqlCommand sqlCommand = new SqlCommand("Verifier_SelectAll");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@finyear", finyear);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion
    }
}
