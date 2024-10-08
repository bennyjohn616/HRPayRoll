using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{

   public class DefaultLOPid
    {
        #region "Constructor"
        public DefaultLOPid()
        {

        }

        public DefaultLOPid(int CompanyId)
        {

            this.CompanyId = CompanyId;
            
            DataTable dtValue = this.GetLOPid();
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.LOPId = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                
            }

        }

        public DefaultLOPid(int CompanyId,int onduty)
        {

            this.CompanyId = CompanyId;

            DataTable dtValue = this.GetONDUTYid();
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.ONDUTYId = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));

            }

        }

        #endregion
        #region "Property"
        public int CompanyId { get; set; }
        public Guid LOPId { get; set; }
        public Guid ONDUTYId { get; set; }
        #endregion
        #region "Methhods"
        internal DataTable GetLOPid()
        {
            SqlCommand sqlCommand = new SqlCommand("LOSSOFPAY_ID_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@ONDUTYFLAY",0);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        internal DataTable GetONDUTYid()
        {
            SqlCommand sqlCommand = new SqlCommand("LOSSOFPAY_ID_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@ONDUTYFLAY", 1);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        #endregion
    }
}
