using SQLDBOperation;
using SQLDBperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class UserCompanymapping
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public UserCompanymapping()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public UserCompanymapping(int userid)
        {
            this.UserId = userid;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["UserId"])))
                    this.UserId = Convert.ToInt32(dtValue.Rows[0]["UserId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);

                this.RightsOnValue = Convert.ToString(dtValue.Rows[0]["RightsOnValue"]);
                this.RightsOn = Convert.ToString(dtValue.Rows[0]["RightsOn"]);
             //   this.IsRights = Convert.ToString(dtValue.Rows[0]["IsRights"]);

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);

            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the Username
        /// </summary>
        public int UserId { get; set; }   

        /// <summary>
        /// Get or Set the Password
        /// </summary>
        public int CompanyId { get; set; }
        public int RoleId { get; set; }
        public Guid FormId { get; set; }
        /// <summary>
        /// Get or Set the RightsOn
        /// </summary>
        public string RightsOn { get; set; }

        /// <summary>
        /// Get or Set the RightsOnValue
        /// </summary>
        public string RightsOnValue { get; set; }

      //  public string IsRights { set; get; }

        /// <summary>
        /// Get or Set the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Get or Set the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Get or Set the ModifiiedOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Get or Set the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }

        public string Displayas { get; set; }

        public string FirstName { get; set; }

        public string EmployeeCode { get; set; }






        #endregion

        #region Public methods


        /// <summary>
        /// Save the User
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("UserCompanyMapping_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@UserId", this.UserId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
          //  sqlCommand.Parameters.AddWithValue("@RoleId", this.RoleId);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@RightsOn", this.RightsOn);
          //  sqlCommand.Parameters.AddWithValue("@FormId", this.FormId);
            sqlCommand.Parameters.AddWithValue("@RightsOnValue", Convert.ToString(this.RightsOnValue));
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

        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("UserCompanyMapping_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@UserId", this.UserId);
          //  sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }



        #endregion

        #region private methods


        /// <summary>
        /// Select the User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues()
        {

            SqlCommand sqlCommand = new SqlCommand("UserCompanyMapping_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@userId", this.UserId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        
        public  DataTable GetPayrolRole()
        {

            SqlCommand sqlCommand = new SqlCommand("PayrolRole_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            //sqlCommand.Parameters.AddWithValue("@userId", userid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        protected internal DataTable GetTableValues(int CompanyId,Guid id)
        {

            SqlCommand sqlCommand = new SqlCommand("UserRole_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@UserId", Guid.Empty);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        protected internal DataTable GetTableValues(Guid id,int CompanyId)
        {
            SqlCommand sqlCommand = new SqlCommand("UserCompanyRole_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        #endregion
    }
}
