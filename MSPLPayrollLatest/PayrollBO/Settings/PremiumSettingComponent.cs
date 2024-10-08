using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDBOperation;
using System.Data.SqlClient;
using System.Data;

namespace PayrollBO
{
   public class PremiumSettingComponent
    {
        public PremiumSettingComponent()
            {
            }


        public PremiumSettingComponent(int companyId,string Type,Guid CategoryId)
        {
            this.CompanyId = companyId;

            DataTable dtValue = this.GetTableValues(this.CompanyId, this.Type, this.CategoryId);
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Component"])))
                    this.AttrId = new Guid(Convert.ToString(dtValue.Rows[0]["Component"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CategoryId"])))
                    this.CategoryId = new Guid(Convert.ToString(dtValue.Rows[0]["CategoryId"]));
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

        public Guid AttrId { get; set; }

        public Guid CategoryId { get; set; }

        public string Type { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsActive { get; set; }
        public string AttributeName { get; set; }

        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("PremiumSettingComponent_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@Component", this.AttrId);
            sqlCommand.Parameters.AddWithValue("@Type", this.Type);
            sqlCommand.Parameters.AddWithValue("@CategoryId", this.CategoryId);
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

        /// <summary>
        /// Delete the PremiumSettingComponent
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            SqlCommand sqlCommand = new SqlCommand("PremiumSettingComponent_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Type", this.Type);
            sqlCommand.Parameters.AddWithValue("@CategoryId", this.CategoryId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        protected internal DataTable GetTableValues(int companyId,string Type,Guid CategoryId)
        {

            SqlCommand sqlCommand = new SqlCommand("PremiumSettingComponent_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;           
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@CategoryId", CategoryId);
            sqlCommand.Parameters.AddWithValue("@Type", Type);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }        
    }
}
