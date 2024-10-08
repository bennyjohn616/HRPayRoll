

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SQLDBOperation;
    using System.Data.SqlClient;
    using System.Data;
   public class Tableleave
    {


        public Tableleave()
        {

        }
        #region property
        public Guid Id { get; set; }
        public string LeaveType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string colour { get; set; }
        public bool IsDeleted { get; set; }

        public int CompanyId { get; set; }
        public int Leaveavailablecnt { get; set; }



        #endregion
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("TableLeave_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            //sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@LeaveType", this.LeaveType);
            sqlCommand.Parameters.AddWithValue("@colour", this.colour);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.save(sqlCommand);
            
            return status;
        }

        public DataTable GetAvailableleavecount(Guid EMPid)
        {

            SqlCommand sqlCommand = new SqlCommand("AssignmanagerupdateordeleteCheck_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EMPId", EMPid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

    }
}
