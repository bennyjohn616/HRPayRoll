// -----------------------------------------------------------------------
// <copyright file="Role.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SQLDBOperation;
    using System.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// To handle the Role
    /// </summary>
    public class Role
    {
      

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public Role()
        {

        }
       
        public Role(int Id,int CompanyId)
        {
           //this.Id = Id;
            this.Id = Id;
            this.Name = Name;
            DataTable dtValue = this.GetTableValues(Id,CompanyId);
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Name"])))
                    this.Name = Convert.ToString(dtValue.Rows[0]["Name"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DisplayAs"])))
                    this.DisplayAs = Convert.ToString(dtValue.Rows[0]["DisplayAs"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Description"])))
                    this.Description = Convert.ToString(dtValue.Rows[0]["Description"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);

            }
        }




        #endregion

        #region property
        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or Set the RoleName
        /// </summary>
        public string DisplayAs { get; set; }

        /// <summary>
        /// Get or Set the DisplayAs
        /// </summary>
        public string Description { get; set; }

        public int delstatus { get; set; }


        /// <summary>
        /// Get or Set the Description
        /// </summary>

    


        #endregion

        #region Public methods


        /// <summary>
        /// Save the Role
        /// </summary>
        /// <returns></returns>
        /// 
        //Modified By Keerthika  on 17/05/2017
        public bool Save()
    {

        SqlCommand sqlCommand = new SqlCommand("Role_Save");
        sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@Name", this.Name);
        sqlCommand.Parameters.AddWithValue("@DisplayAs", this.DisplayAs);
        sqlCommand.Parameters.AddWithValue("@Description", this.Description);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Name = Convert.ToString(outValue);
            }
            return status;
        }

        // Modified by keerthika on 17/05/2017

        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("Role_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            //  sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            //return dbOperation.DeleteData(sqlCommand);
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");          
            return status;
        }

        /// <summary>
        /// Delete the Role
        /// </summary>
        /// <returns></returns>



        #endregion

        #region private methods


        /// <summary>
        /// Select the Role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        // Modified by keerthika on 17/05/2017
        protected internal DataTable GetTableValues(int Id,int CompanyId)
        {

            SqlCommand sqlCommand = new SqlCommand("Role_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
           sqlCommand.Parameters.AddWithValue("@Id", Id);
          sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }

   
}

