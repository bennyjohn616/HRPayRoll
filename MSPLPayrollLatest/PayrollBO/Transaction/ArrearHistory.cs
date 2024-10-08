using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class ArrearHistory
    {
        public int Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid PayHistoryId { get; set; }
        public Guid AttributeModelId { get; set; }
        public int Month { get; set; }

        public int Year { get; set; }
        public decimal Value { get; set; }

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public ArrearHistory()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public ArrearHistory(int id, Guid employeeId)
        {
            this.Id = id;
            this.EmployeeId = employeeId;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PayrollHistoryId"])))
                    this.PayHistoryId = new Guid(Convert.ToString(dtValue.Rows[0]["PayrollHistoryId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttributemodelId"])))
                    this.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[0]["AttributemodelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Month"])))
                    this.Month = (Convert.ToInt16(dtValue.Rows[0]["Month"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Year"])))
                    this.Year = (Convert.ToInt16(dtValue.Rows[0]["Year"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Value"])))
                    this.Value = Convert.ToInt32(dtValue.Rows[0]["Value"]);
               
               
            }
        }


        #endregion





        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("ArrearHistory_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@PayHistoryId", this.PayHistoryId);
            sqlCommand.Parameters.AddWithValue("@ArributemodelId", this.AttributeModelId);
            sqlCommand.Parameters.AddWithValue("@Month", this.Month);
            sqlCommand.Parameters.AddWithValue("@Year", this.Year);
            sqlCommand.Parameters.AddWithValue("@Value", this.Value);

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






        #region private methods


        /// <summary>
        /// Select the ArrearHistory
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues()
        {

            SqlCommand sqlCommand = new SqlCommand("ArrearHistory_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@PayrollhistoryId", this.PayHistoryId);
            sqlCommand.Parameters.AddWithValue("@EmployeeID", this.EmployeeId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        public  DataTable ArrearViewValues(Guid categoryId,int Month ,int year)
        {

            SqlCommand sqlCommand = new SqlCommand("ArrearView_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@categoryId", categoryId);
            sqlCommand.Parameters.AddWithValue("@Month", Month);
            sqlCommand.Parameters.AddWithValue("@Year", year);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion
    }
}
