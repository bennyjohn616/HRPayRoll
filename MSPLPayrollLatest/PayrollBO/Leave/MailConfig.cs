using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
 public   class MailConfig
    {
        public int CompanyId;

        public MailConfig()
        {

        }
        public MailConfig(int CompanyId)
        {
           
            this.CompanyId = CompanyId;

            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {
                for (int i = 0; i < dtValue.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["Id"])))
                        this.Id = new Guid(Convert.ToString(dtValue.Rows[i]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["smtpIP"])))
                        this.IPAddress = Convert.ToString(dtValue.Rows[i]["smtpIP"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["smtpPORT"])))
                        this.PortNo = Convert.ToInt32(dtValue.Rows[i]["smtpPORT"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["FromEmail"])))
                        this.FromEmail = Convert.ToString(dtValue.Rows[i]["FromEmail"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["AutEmail"])))
                        this.AuthenEmail = Convert.ToBoolean(dtValue.Rows[i]["AutEmail"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["MailPassword"])))
                        this.MailPassword = Convert.ToString(dtValue.Rows[i]["MailPassword"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CCMail"])))
                        this.CCMail = Convert.ToString(dtValue.Rows[i]["CCMail"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["mailapproval"])))
                        this.mailapproval = Convert.ToString(dtValue.Rows[i]["mailapproval"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["AutSmtpUser"])))
                        this.AuthenSMTPUser= Convert.ToString(dtValue.Rows[i]["AutSmtpUser"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["AutSmtpPassword"])))
                        this.AuthenSMTPPwd = Convert.ToString(dtValue.Rows[i]["AutSmtpPassword"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CompanyId"])))
                        this.CompanyId= Convert.ToInt32(dtValue.Rows[i]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["SSL_Enable"])))
                        this.EnableSSL= Convert.ToBoolean(dtValue.Rows[i]["SSL_Enable"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedOn"])))
                        this.CreatedOn = Convert.ToDateTime(dtValue.Rows[i]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["CreatedBy"])))
                        this.CreatedBy = Convert.ToInt32(dtValue.Rows[i]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedOn"])))
                        this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[i]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["ModifiedBy"])))
                        this.ModifiedBy = Convert.ToInt32(dtValue.Rows[i]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[i]["IsDelete"])))
                        this.IsDeleted = Convert.ToBoolean(dtValue.Rows[i]["IsDelete"]);

                }
            }
        }
        public Guid Id { get; set; }
        //public int CompanyId { get; set; }
        public string IPAddress { get; set; }
        public int PortNo { get; set; }
        public string FromEmail { get; set; }
        public bool EnableSSL { get; set; }

        public bool AuthenEmail { get; set; }
        public string AuthenSMTPUser { get; set; }
        public string AuthenSMTPPwd { get; set; }

        public string MailPassword { get; set; }

        public string CCMail { get; set; }

        public string mailapproval { get; set; }
        public int CreatedBy { get; set; }

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

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }


        public bool Savemailhistory(int CompanyID, Guid LoginID, string FromMailID, string ToMailID, string CCMail, string BCCMail, string MailMessages, string MailObject, string MailStatus)

        {
            SqlCommand sqlCommand = new SqlCommand("Mailhistory_save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyID);
            sqlCommand.Parameters.AddWithValue("@LoginID", LoginID);
            sqlCommand.Parameters.AddWithValue("@FromMailID", FromMailID);
            sqlCommand.Parameters.AddWithValue("@ToMailID", ToMailID);
            sqlCommand.Parameters.AddWithValue("@CCMail",CCMail);
            sqlCommand.Parameters.AddWithValue("@BCCMail", BCCMail);
            sqlCommand.Parameters.AddWithValue("@MailMessages", MailMessages);
            sqlCommand.Parameters.AddWithValue("@MailObject", MailObject);
            sqlCommand.Parameters.AddWithValue("@MailStatus", MailStatus);
            DBOperation dbOperation = new DBOperation();
            bool status = dbOperation.DeleteData(sqlCommand);
            return true;
        }


        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("MailConfig_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@smtpIP", this.IPAddress);
            sqlCommand.Parameters.AddWithValue("@smtpPORT", this.PortNo);
            sqlCommand.Parameters.AddWithValue("@FromEmail", this.FromEmail);
            sqlCommand.Parameters.AddWithValue("@MailPassword", this.MailPassword);
            sqlCommand.Parameters.AddWithValue("@CCMail", this.CCMail);
            sqlCommand.Parameters.AddWithValue("@mailapproval", this.mailapproval);
            sqlCommand.Parameters.AddWithValue("@SSL_Enable", this.EnableSSL);
            sqlCommand.Parameters.AddWithValue("@AutEmail", this.AuthenEmail);
            sqlCommand.Parameters.AddWithValue("@AutSmtpUser", this.AuthenSMTPUser);
            sqlCommand.Parameters.AddWithValue("@AutSmtpPassword", this.AuthenSMTPPwd);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@CreatedOn", this.CreatedOn);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
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
        internal DataTable GetTableValues()
        {
            SqlCommand sqlCommand = new SqlCommand("MailConfig_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

    }
}
