// -----------------------------------------------------------------------
// <copyright file="PaySlipSetting.cs" company="Microsoft">
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
    /// To handle the PaySlipSetting
    /// </summary>
    public class PaySlipSetting
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public PaySlipSetting()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name = "configurationid" ></ param >
        public PaySlipSetting(Guid configurationid)
        {
            this.ConfigurationId = configurationid;
            DataTable dtValue = this.GetTableValues(this.ConfigurationId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ConfigurationId"])))
                    this.ConfigurationId = new Guid(Convert.ToString(dtValue.Rows[0]["ConfigurationId"]));
                this.Description = Convert.ToString(dtValue.Rows[0]["Description"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
                    this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
                this.Logo = Convert.ToString(dtValue.Rows[0]["Logo"]);
                this.Title = Convert.ToString(dtValue.Rows[0]["Title"]);
                this.String1 = Convert.ToString(dtValue.Rows[0]["String1"]);
                this.String2 = Convert.ToString(dtValue.Rows[0]["String2"]);
                this.FooterString1 = Convert.ToString(dtValue.Rows[0]["FooterString1"]);
                this.FooterString2 = Convert.ToString(dtValue.Rows[0]["FooterString2"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["DisplayCumulative"])))
                    this.DisplayCumulative = Convert.ToBoolean(dtValue.Rows[0]["DisplayCumulative"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CumulativeMonth"])))
                    this.CumulativeMonth = Convert.ToInt32(dtValue.Rows[0]["CumulativeMonth"]);
                this.Header = Convert.ToString(dtValue.Rows[0]["Header"]);
                this.Footer = Convert.ToString(dtValue.Rows[0]["Footer"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Matchingtype"])))
                    this.Matchingtype = Convert.ToString(dtValue.Rows[0]["Matchingtype"]);
            }
        }

        //public PaySlipSetting(int CompanyId)
        //{
        //    this.CompanyId = CompanyId;
        //    DataTable dtValue = this.GetTableValues1(this.CompanyId);
        //    if (dtValue.Rows.Count > 0)
        //    {
        //        for (var rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
        //        {
        //            PaySlipSetting PaySlipsettinTemp = new PaySlipSetting();

        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ConfigurationId"])))
        //                PaySlipsettinTemp.ConfigurationId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["ConfigurationId"]));
        //            this.Description = Convert.ToString(dtValue.Rows[rowcount]["Description"]);
        //            if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
        //                PaySlipsettinTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
        //            PaySlipsettinTemp.Logo = Convert.ToString(dtValue.Rows[rowcount]["Logo"]);
        //            PaySlipsettinTemp.Title = Convert.ToString(dtValue.Rows[rowcount]["Title"]);
        //            this.Add(PaySlipsettinTemp);

        //        }
        //    }
        //}


        #endregion

        #region property


        /// <summary>
        /// Get or Set the ConfigurationId
        /// </summary>
        public Guid ConfigurationId { get; set; }

        /// <summary>
        /// Get or Set the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Get or Set the Logo
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// Get or Set the Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get or Set the String1
        /// </summary>
        public string String1 { get; set; }

        /// <summary>
        /// Get or Set the String2
        /// </summary>
        public string String2 { get; set; }

        /// <summary>
        /// Get or Set the FooterString1
        /// </summary>
        public string FooterString1 { get; set; }

        /// <summary>
        /// Get or Set the FooterString2
        /// </summary>
        public string FooterString2 { get; set; }

        /// <summary>
        /// Get or Set the DisplayCumulative
        /// </summary>
        public bool DisplayCumulative { get; set; }

        /// <summary>
        /// Get or Set the CumulativeMonth
        /// </summary>
        public int CumulativeMonth { get; set; }

        public string TableName { get; set; }

        public string FieldName { get; set; }


        public string Type { get; set; }

        public Guid Id { get; set; }


        public string Header { get; set; }
        public string Footer { get; set; }
        public string Matchingtype { get; set; }


        #endregion

        #region Public methods


        /// <summary>
        /// Save the PaySlipSetting
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("PaySlipSetting_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@ConfigurationId", this.ConfigurationId);
            sqlCommand.Parameters.AddWithValue("@Description", this.Description);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@Logo", this.Logo);
            sqlCommand.Parameters.AddWithValue("@Title", this.Title);
            sqlCommand.Parameters.AddWithValue("@String1", this.String1);
            sqlCommand.Parameters.AddWithValue("@String2", this.String2);
            sqlCommand.Parameters.AddWithValue("@FooterString1", this.FooterString1);
            sqlCommand.Parameters.AddWithValue("@FooterString2", this.FooterString2);
            sqlCommand.Parameters.AddWithValue("@DisplayCumulative", this.DisplayCumulative);
            sqlCommand.Parameters.AddWithValue("@CumulativeMonth", this.CumulativeMonth);
            sqlCommand.Parameters.Add("@ConfigurationIdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            sqlCommand.Parameters.AddWithValue("@Header", this.Header);
            sqlCommand.Parameters.AddWithValue("@Footer", this.Footer);
            sqlCommand.Parameters.AddWithValue("@Matchingtype", this.Matchingtype);
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@ConfigurationIdOut");
            if (status)
            {
                this.ConfigurationId = new Guid(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the PaySlipSetting
        /// </summary>
        /// <returns></returns>
        public bool Delete(Guid ConfigurationId)
        {

            SqlCommand sqlCommand = new SqlCommand("PaySlipSetting_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CofigurationId", ConfigurationId);
           
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
        protected internal DataTable GetTableValues(Guid configurationid)
        {

            SqlCommand sqlCommand = new SqlCommand("PaySlipSetting_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@ConfigurationId", configurationid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues1(int CompanyId)
        {

            SqlCommand sqlCommand = new SqlCommand("PaySlipSettingcompany_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        protected internal DataTable GetTableValues(Guid configurationid, int CompanyId)
        {

            SqlCommand sqlCommand = new SqlCommand("PaySlipSettingcatgory_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CofigurationId", configurationid);
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

