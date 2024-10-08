using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRollReports
{
    public class Paysheetatrr
    {
        #region "Properties"
        public int Id { get; set; }
        public int PaySheetId { get; set; }
        public string TableName { get; set; }

        public string FieldName { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
       
        public string DisplayAs { get; set; }

        public string Value { get; set; }
        public string Type { get; set; }

        public int OrderBy { get; set; }

        public bool Visible { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the PaySlipSetting
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("PaySheetAttributes_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@PaySheetId", this.PaySheetId);
            sqlCommand.Parameters.AddWithValue("@TableName", this.TableName);
            sqlCommand.Parameters.AddWithValue("@FieldName", this.FieldName);
            sqlCommand.Parameters.AddWithValue("@DisplayAs", this.DisplayAs);
            sqlCommand.Parameters.AddWithValue("@Type", this.Type);
            sqlCommand.Parameters.AddWithValue("@OrderBy", this.OrderBy);       
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = Convert.ToInt32(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the PaySlipSetting
        /// </summary>
        /// <returns></returns>
        public bool Delete(int id)
        {

            SqlCommand sqlCommand = new SqlCommand("PaySheetAttributes_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);

            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the PaySlipSetting
        /// </summary>
        /// <param name="configurationid"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(int id)
        {

            SqlCommand sqlCommand = new SqlCommand("PaySheetAttributes_SelectAll");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}
