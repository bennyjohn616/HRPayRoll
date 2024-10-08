using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SQLDBOperation;

namespace PayrollBO
{
    public class Holidays
    {


        public Holidays()
        {

        }
        public Holidays(Guid id,Guid finid)
        {

            this.Id = id;
            this.financeyearId = finid;

            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int i = 0; i < dtValue.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                        this.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FinanceyearId"])))
                        this.financeyearId = new Guid(Convert.ToString(dtValue.Rows[i]["FinanceyearId"]));



                    this.HolidayReason = Convert.ToString(dtValue.Rows[i]["Reason"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Holiday Date"])))
                        this.Holidaydate = Convert.ToDateTime(dtValue.Rows[i]["Holiday Date"]);

                    this.holidayDAY = Convert.ToString(dtValue.Rows[i]["Holiday Day"]);


                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["cretedOn"])))
                        this.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["cretedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                        this.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                        this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                        this.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);

                }
            }

        }
        public Guid Id { get; set; }
        //public int CompanyId { get; set; }
        public DateTime Holidaydate { get; set; }
        public string HolidayReason { get; set; }
        public string CATIDNAME { get; set; }
        public string LEVIDNAME { get; set; }
        public Guid financeyearId { get; set; }
        public Guid catid { get; set; }
        public Guid levid { get; set; }
        public Guid yrlylevopndecID { get; set; }
        public decimal opdays { get; set; }
        public string holidayDAY { get; set; }
        public int CreatedBy { get; set; }
        public int Companyid { get; set; }

        /// <summary>
        /// Get or Set the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Get or Set the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }

        /// <summary>
        /// Get or Set the ModifiedOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }
        public DateTime Revertholidate { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }
        public string Component { get; set; }
        public Guid ComponentValue { get; set; }
        public string Type { get; set; }
        public string ComponentName { get; set; }

        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("HolidaySet_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceyearId", this.financeyearId);
            sqlCommand.Parameters.AddWithValue("@Holi_Date", this.Holidaydate);
            sqlCommand.Parameters.AddWithValue("@Holi_Day", this.holidayDAY);
            sqlCommand.Parameters.AddWithValue("@Holi_Reason", this.HolidayReason);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            // sqlCommand.Parameters.AddWithValue("@CreatedOn", this.CreatedOn);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@Component", this.Component);
            sqlCommand.Parameters.AddWithValue("@ComponentValue", this.ComponentValue);
            sqlCommand.Parameters.AddWithValue("@Type", this.Type);
            // sqlCommand.Parameters.AddWithValue("@ModifiedOn", this.ModifiedOn);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = new Guid(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the TaxBehavior
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            SqlCommand sqlCommand = new SqlCommand("HolidaySet_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
        public bool DeleteYrlyopendec()
        {
            SqlCommand sqlCommand = new SqlCommand("Yrlyopeningsdec_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
        internal DataTable GetTableValues()
        {
            SqlCommand sqlCommand = new SqlCommand("HolidaySet_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceyearId", this.financeyearId);
            //sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        internal DataTable GetTableyearlylevopeningsdecvalue(int compid)
        {
            SqlCommand sqlCommand = new SqlCommand("YearlyLeaveopeningsdec_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Companyid", compid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public DataTable GetleaabsentValues(DateTime date,Guid finid)
        {
            SqlCommand sqlCommand = new SqlCommand("Holidaycheck_select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Date", date);
            sqlCommand.Parameters.AddWithValue("@FinId", finid);

            //sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public bool saveLevopendeclare(Guid ID,int compid, Guid Category,Guid levtypeid,decimal opdays,int userid,Guid finyear)
        {
            SqlCommand sqlCommand = new SqlCommand("YearlyLeaveOpeningsDec_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            sqlCommand.Parameters.AddWithValue("@Catid", Category);
            sqlCommand.Parameters.AddWithValue("@levtpyeid", levtypeid);
            sqlCommand.Parameters.AddWithValue("@opndays", opdays);
            sqlCommand.Parameters.AddWithValue("@Companyid", compid);
            sqlCommand.Parameters.AddWithValue("@Createdby", userid);
            sqlCommand.Parameters.AddWithValue("@Finid", finyear);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.save(sqlCommand);
        }

        public bool RevertholiDelete(Guid finyear,DateTime revertdate)
        {
            SqlCommand sqlCommand = new SqlCommand("HolidayRevertDelete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Finyr", finyear);
            sqlCommand.Parameters.AddWithValue("@Date", revertdate);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }

    }
}
