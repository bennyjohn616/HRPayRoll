using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    class Release
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public Release()
        {

        }

        public Release(string EmployeeCode)
        {
            this.Id = EmployeeCode;
            DataTable dtValue = this.GetTableValues(EmployeeCode);
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeCode"])))
                    this.EmployeeCode = Convert.ToString(dtValue.Rows[0]["EmployeeCode"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TypeofSeparation"])))
                    this.TypeofSeparation = Convert.ToString(dtValue.Rows[0]["TypeofSeparation"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ReasonforSeparation"])))
                    this.ReasonforSeparation = Convert.ToString(dtValue.Rows[0]["ReasonforSeparation"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["SeparationDate"])))
                    this.SeparationDate = Convert.ToString(dtValue.Rows[0]["SeparationDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ReleaseDate"])))
                    this.ReleaseDate = Convert.ToString(dtValue.Rows[0]["ReleaseDate"]);

            }
        }




        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Get or Set the RoleName
        /// </summary>
        public string TypeofSeparation { get; set; }

        /// <summary>
        /// Get or Set the DisplayAs
        /// </summary>
        public string ReasonforSeparation { get; set; }


        /// <summary>
        /// Get or Set the Description
        /// </summary>

        public string SeparationDate { get; set; }
        public string ReleaseDate { get; set; }

        #endregion
        
        #region Public methods


        /// <summary>
        /// Save the Role
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("Employee_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeCode", this.EmployeeCode);
            sqlCommand.Parameters.AddWithValue("@TypeofSeparation", this.TypeofSeparation);
            sqlCommand.Parameters.AddWithValue("@ReasonforSeparation", this.ReasonforSeparation);
            sqlCommand.Parameters.AddWithValue("@SeparationDate", this.SeparationDate);
            sqlCommand.Parameters.AddWithValue("@ReleaseDate", this.ReleaseDate);

            // sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@EmployeeCode");
            if (status)
            {
                this.EmployeeCode = Convert.ToString(outValue);
            }
            return status;
        }

        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("Employee_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeCode", this.EmployeeCode);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
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
        protected internal DataTable GetTableValues(string EmployeeCode)
        {

            SqlCommand sqlCommand = new SqlCommand("Employee_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}



// -----------------------------------------------------------------------
// <copyright file="Role.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------



