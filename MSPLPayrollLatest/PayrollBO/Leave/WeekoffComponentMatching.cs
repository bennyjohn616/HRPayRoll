using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SQLDBOperation;

namespace PayrollBO.Leave
{
    public class WeekoffComponentMatching
    {
        #region Constructor
        public WeekoffComponentMatching()
        {

        }
        #endregion
        public WeekoffComponentMatching(int CompanyId)
        {

            this.CompanyId = CompanyId;            
            DataTable dtValue = this.Getweekoffcmpmatching();
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LeaveCategoryId"])))
                    this.LeaveCategoryId = new Guid(Convert.ToString(dtValue.Rows[0]["LeaveCategoryId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Leavecomponent"])))
                    this.Leavecomponent = (Convert.ToString(dtValue.Rows[0]["Leavecomponent"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = (Convert.ToInt32(dtValue.Rows[0]["CompanyId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
            }

        }
        #region Properties
        public Guid Id { get; set; }

        public int CompanyId { get; set; }
        public Guid LeaveCategoryId { get; set; }
        public string Leavecomponent { get; set; }       
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        #endregion

        //public List<WeekoffComponentMatching> GetComponentMatching()
        //{           
        //    DataTable dt = GetTableValues();

        //    List<WeekoffComponentMatching> weekoffcomponentmatchinglist = new List<WeekoffComponentMatching>();
        //    weekoffcomponentmatchinglist = (from DataRow dr in dt.Rows
        //                             select new WeekoffComponentMatching()
        //                             {
        //                                 Id = new Guid(Convert.ToString(dr["Id"].ToString())),
        //                                 LeaveCategoryId = new Guid(dr["LeaveCategoryId"].ToString()),
        //                                 Leavecomponent = dr["Leavecomponent"].ToString()
                                        
        //                             }).ToList();
        //    return weekoffcomponentmatchinglist;

        //}

        public bool SaveWeekoffComponentMatching()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_Weekoffcomponentmatching");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);            
            sqlCommand.Parameters.AddWithValue("@LeaveCategoryId", this.LeaveCategoryId);
            sqlCommand.Parameters.AddWithValue("@Leavecomponent", this.Leavecomponent);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@CreatedOn", this.CreatedOn);
            sqlCommand.Parameters.AddWithValue("@ModifiedOn", this.ModifiedOn);
            sqlCommand.Parameters.AddWithValue("@Type", "ADD");
            DBOperation dbOperation = new DBOperation();
            bool status = dbOperation.save(sqlCommand);
            return status;
        }

        //public bool DeleteMonthlyLeaveLimit()
        //{
        //    SqlCommand sqlCommand = new SqlCommand("SP_MonthlyLeaveLimit");
        //    sqlCommand.CommandType = CommandType.StoredProcedure;
        //    sqlCommand.Parameters.AddWithValue("@Id", this.Id);
        //    sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
        //    sqlCommand.Parameters.AddWithValue("@ModifiedOn", this.ModifiedOn);
        //    sqlCommand.Parameters.AddWithValue("@Type", "DELETE");
        //    DBOperation dbOperation = new DBOperation();
        //    bool status = dbOperation.save(sqlCommand);
        //    return status;
        //}
        internal DataTable Getweekoffcmpmatching()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_Weekoffcomponentmatching");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);         
            sqlCommand.Parameters.AddWithValue("@Type", "SELECT");
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

    }
}
