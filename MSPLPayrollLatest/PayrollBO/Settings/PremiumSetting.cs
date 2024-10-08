using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDBOperation;
using System.Data.SqlClient;
using System.Data;

namespace PayrollBO
{
  public class PremiumSetting
    {

        public PremiumSetting()
        {

        }
        public PremiumSetting(int companyId,string Type)
        {           
            this.CompanyId = companyId;
            this.Type = Type;
            DataTable dtValue = this.GetTableValues(this.CompanyId,this.Type);
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);              
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Component"])))
                    this.Component = new Guid(Convert.ToString(dtValue.Rows[0]["Component"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["BackMonth"])))
                    this.BackMonth = Convert.ToInt32(dtValue.Rows[0]["BackMonth"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Type"])))
                    this.Type = Convert.ToString(dtValue.Rows[0]["Type"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
            }
        }

        public int Id { get; set; }

        public int CompanyId { get; set; }

        public Guid Component { get; set; }

        public int BackMonth { get; set; }

        public string Type { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsActive { get; set; }

        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("PremiumSetting_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@Component", this.Component);
            sqlCommand.Parameters.AddWithValue("@Type", this.Type);
            sqlCommand.Parameters.AddWithValue("@BackMonth", this.BackMonth);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
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

        protected internal DataTable GetTableValues(int companyId,string Type)
        {

            SqlCommand sqlCommand = new SqlCommand("PremiumSetting_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", 0);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@Type", Type);
           // sqlCommand.Parameters.AddWithValue("@Component", Component);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

    }
}
