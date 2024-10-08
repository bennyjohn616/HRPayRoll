// -----------------------------------------------------------------------
// <copyright file="Emp_Personal.cs" company="Microsoft">
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
    /// To handle the Emp_Personal
    /// </summary>
    public class Emp_Personal
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public Emp_Personal()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public Emp_Personal(Guid id)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                this.PersonalMobileNo = Convert.ToString(dtValue.Rows[0]["PersonalMobileNo"]);
                this.OfficeMobileNo = Convert.ToString(dtValue.Rows[0]["OfficeMobileNo"]);
                this.ExtensionNo = Convert.ToString(dtValue.Rows[0]["ExtensionNo"]);
                this.PersonalEmail = Convert.ToString(dtValue.Rows[0]["PersonalEmail"]);
                this.OfficeEmail = Convert.ToString(dtValue.Rows[0]["OfficeEmail"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["BloodGroup"])))
                    this.BloodGroup = Convert.ToInt32(dtValue.Rows[0]["BloodGroup"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PrintCheque"])))
                    this.PrintCheque = Convert.ToBoolean(dtValue.Rows[0]["PrintCheque"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsSeniorCitizen"])))
                    this.IsSeniorCitizen = Convert.ToBoolean(dtValue.Rows[0]["IsSeniorCitizen"]);
                this.PaySlipRemarks = Convert.ToString(dtValue.Rows[0]["PaySlipRemarks"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDisable"])))
                    this.IsDisable = Convert.ToBoolean(dtValue.Rows[0]["IsDisable"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                this.PANNumber = Convert.ToString(dtValue.Rows[0]["PANNumber"]);
                this.PFNumber = Convert.ToString(dtValue.Rows[0]["PFNumber"]);
                this.ESINumber = Convert.ToString(dtValue.Rows[0]["ESINumber"]);
                this.MaritalStatus = Convert.ToString(dtValue.Rows[0]["MaritalStatus"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["NoOfChildren"])))
                    this.NoOfChildren = Convert.ToInt32(dtValue.Rows[0]["NoOfChildren"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PFConfirmationDate"])))
                    this.PFConfirmationDate = Convert.ToDateTime(dtValue.Rows[0]["PFConfirmationDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PensionEligible"])))
                    this.PensionEligible = Convert.ToInt32(dtValue.Rows[0]["PensionEligible"]);
                this.AADHARNumber = Convert.ToString(dtValue.Rows[0]["AADHARNumber"]);
                this.PFUAN = Convert.ToString(dtValue.Rows[0]["PFUAN"]);
                this.FatherName = Convert.ToString(dtValue.Rows[0]["FatherName"]);
                this.SpouseName = Convert.ToString(dtValue.Rows[0]["SpouseName"]);
            }
        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public Emp_Personal(Guid id, string filterExpr)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id, filterExpr);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                this.PersonalMobileNo = Convert.ToString(dtValue.Rows[0]["PersonalMobileNo"]);
                this.OfficeMobileNo = Convert.ToString(dtValue.Rows[0]["OfficeMobileNo"]);
                this.ExtensionNo = Convert.ToString(dtValue.Rows[0]["ExtensionNo"]);
                this.PersonalEmail = Convert.ToString(dtValue.Rows[0]["PersonalEmail"]);
                this.OfficeEmail = Convert.ToString(dtValue.Rows[0]["OfficeEmail"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["BloodGroup"])))
                    this.BloodGroup = Convert.ToInt32(dtValue.Rows[0]["BloodGroup"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PrintCheque"])))
                    this.PrintCheque = Convert.ToBoolean(dtValue.Rows[0]["PrintCheque"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsSeniorCitizen"])))
                    this.IsSeniorCitizen = Convert.ToBoolean(dtValue.Rows[0]["IsSeniorCitizen"]);
                this.PaySlipRemarks = Convert.ToString(dtValue.Rows[0]["PaySlipRemarks"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDisable"])))
                    this.IsDisable = Convert.ToBoolean(dtValue.Rows[0]["IsDisable"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                this.PANNumber = Convert.ToString(dtValue.Rows[0]["PANNumber"]);
                this.PFNumber = Convert.ToString(dtValue.Rows[0]["PFNumber"]);
                this.ESINumber = Convert.ToString(dtValue.Rows[0]["ESINumber"]);
                this.MaritalStatus = Convert.ToString(dtValue.Rows[0]["MaritalStatus"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["NoOfChildren"])))
                    this.NoOfChildren = Convert.ToInt32(dtValue.Rows[0]["NoOfChildren"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PFConfirmationDate"])))
                    this.PFConfirmationDate = Convert.ToDateTime(dtValue.Rows[0]["PFConfirmationDate"]);
                this.AADHARNumber = Convert.ToString(dtValue.Rows[0]["AADHARNumber"]);
                this.PFUAN = Convert.ToString(dtValue.Rows[0]["PFUAN"]);
                this.FatherName = Convert.ToString(dtValue.Rows[0]["FatherName"]);
                this.SpouseName = Convert.ToString(dtValue.Rows[0]["SpouseName"]);
            }
        }

        public Emp_Personal(String Id)
        {
            DataTable dtValue = this.GetTableValues(Id);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                this.PersonalMobileNo = Convert.ToString(dtValue.Rows[0]["PersonalMobileNo"]);
                this.OfficeMobileNo = Convert.ToString(dtValue.Rows[0]["OfficeMobileNo"]);
                this.ExtensionNo = Convert.ToString(dtValue.Rows[0]["ExtensionNo"]);
                this.PersonalEmail = Convert.ToString(dtValue.Rows[0]["PersonalEmail"]);
                this.OfficeEmail = Convert.ToString(dtValue.Rows[0]["OfficeEmail"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["BloodGroup"])))
                    this.BloodGroup = Convert.ToInt32(dtValue.Rows[0]["BloodGroup"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PrintCheque"])))
                    this.PrintCheque = Convert.ToBoolean(dtValue.Rows[0]["PrintCheque"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsSeniorCitizen"])))
                    this.IsSeniorCitizen = Convert.ToBoolean(dtValue.Rows[0]["IsSeniorCitizen"]);
                this.PaySlipRemarks = Convert.ToString(dtValue.Rows[0]["PaySlipRemarks"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDisable"])))
                    this.IsDisable = Convert.ToBoolean(dtValue.Rows[0]["IsDisable"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                this.PANNumber = Convert.ToString(dtValue.Rows[0]["PANNumber"]);
                this.PFNumber = Convert.ToString(dtValue.Rows[0]["PFNumber"]);
                this.ESINumber = Convert.ToString(dtValue.Rows[0]["ESINumber"]);
                this.MaritalStatus = Convert.ToString(dtValue.Rows[0]["MaritalStatus"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["NoOfChildren"])))
                    this.NoOfChildren = Convert.ToInt32(dtValue.Rows[0]["NoOfChildren"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["PFConfirmationDate"])))
                    this.PFConfirmationDate = Convert.ToDateTime(dtValue.Rows[0]["PFConfirmationDate"]);
                this.AADHARNumber = Convert.ToString(dtValue.Rows[0]["AADHARNumber"]);
                this.PFUAN = Convert.ToString(dtValue.Rows[0]["PFUAN"]);
                this.FatherName = Convert.ToString(dtValue.Rows[0]["FatherName"]);
                this.SpouseName = Convert.ToString(dtValue.Rows[0]["SpouseName"]);
            }
        }
        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the EmployeeId
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Get or Set the PersonalMobileNo
        /// </summary>
        public string PersonalMobileNo { get; set; }

        /// <summary>
        /// Get or Set the OfficeMobileNo
        /// </summary>
        public string OfficeMobileNo { get; set; }

        /// <summary>
        /// Get or Set the ExtensionNo
        /// </summary>
        public string ExtensionNo { get; set; }

        /// <summary>
        /// Get or Set the PersonalEmail
        /// </summary>
        public string PersonalEmail { get; set; }

        /// <summary>
        /// Get or Set the OfficeEmail
        /// </summary>
        public string OfficeEmail { get; set; }

        /// <summary>
        /// Get or Set the BloodGroup
        /// </summary>
        public int BloodGroup { get; set; }

        /// <summary>
        /// Get or Set the PrintCheque
        /// </summary>
        public bool PrintCheque { get; set; }

        /// <summary>
        /// Get or Set the IsSeniorCitizen
        /// </summary>
        public bool IsSeniorCitizen { get; set; }

        /// <summary>
        /// Get or Set the PaySlipRemarks
        /// </summary>
        public string PaySlipRemarks { get; set; }

        /// <summary>
        /// Get or Set the IsDisable
        /// </summary>
        public bool IsDisable { get; set; }

        /// <summary>
        /// Get or Set the CreatedBy
        /// </summary>
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

        /// <summary>
        /// Get or Set the PANNumber
        /// </summary>
        public string PANNumber { get; set; }

        /// <summary>
        /// Get or Set the PFNumber
        /// </summary>
        public string PFNumber { get; set; }

        /// <summary>
        /// Get or Set the ESINumber
        /// </summary>
        public string ESINumber { get; set; }

        /// <summary>
        /// Get or Set the MaritalStatus
        /// </summary>
        public string MaritalStatus { get; set; }

        /// <summary>
        /// Get or Set the NoOfChildren
        /// </summary>
        public int NoOfChildren { get; set; }

        /// <summary>
        /// Get or Set the PFConfirmationDate
        /// </summary>
        public DateTime PFConfirmationDate { get; set; }

        /// <summary>
        /// Get or Set the AADHARNumber
        /// </summary>
        public string AADHARNumber { get; set; }
        public int PensionEligible { get; set; }

        /// <summary>
        /// Get or Set the PFUAN
        /// </summary>
        public string PFUAN { get; set; }

        /// <summary>
        /// Get or Set the FatherName
        /// </summary>
        public string FatherName { get; set; }

        /// <summary>
        /// Get or Set the SpouseName
        /// </summary>
        public string SpouseName { get; set; }




        public string Bank { get; set; }
        public string BankAccountNo { get; set; }
        public string IFSC { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ImportOption { get; set; }
        public string Query { get; set; }
        
        #endregion

        #region Public methods


        /// <summary>
        /// Save the Emp_Personal
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Personal_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@PersonalMobileNo", this.PersonalMobileNo);
            sqlCommand.Parameters.AddWithValue("@OfficeMobileNo", this.OfficeMobileNo);
            sqlCommand.Parameters.AddWithValue("@ExtensionNo", this.ExtensionNo);
            sqlCommand.Parameters.AddWithValue("@PersonalEmail", this.PersonalEmail);
            sqlCommand.Parameters.AddWithValue("@OfficeEmail", this.OfficeEmail);
            if (this.BloodGroup != 0)
            {
                sqlCommand.Parameters.AddWithValue("@BloodGroup", this.BloodGroup);
            }

            sqlCommand.Parameters.AddWithValue("@PrintCheque", this.PrintCheque);
            sqlCommand.Parameters.AddWithValue("@IsSeniorCitizen", this.IsSeniorCitizen);
            sqlCommand.Parameters.AddWithValue("@PaySlipRemarks", this.PaySlipRemarks);
            sqlCommand.Parameters.AddWithValue("@IsDisable", this.IsDisable);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            //sqlCommand.Parameters.AddWithValue("@CreatedOn", this.CreatedOn);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            //sqlCommand.Parameters.AddWithValue("@ModifiedOn", this.ModifiedOn);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.AddWithValue("@PANNumber", this.PANNumber);
            sqlCommand.Parameters.AddWithValue("@PFNumber", this.PFNumber);
            sqlCommand.Parameters.AddWithValue("@ESINumber", this.ESINumber);
            sqlCommand.Parameters.AddWithValue("@MaritalStatus", this.MaritalStatus);
            sqlCommand.Parameters.AddWithValue("@NoOfChildren", this.NoOfChildren);
            sqlCommand.Parameters.AddWithValue("@PFConfirmationDate", this.PFConfirmationDate == DateTime.MinValue ? Convert.ToDateTime("01/01/1800 12:00:00") : this.PFConfirmationDate);
            sqlCommand.Parameters.AddWithValue("@AADHARNumber", this.AADHARNumber);
            sqlCommand.Parameters.AddWithValue("@PFUAN", this.PFUAN);
            sqlCommand.Parameters.AddWithValue("@FatherName", this.FatherName);
            sqlCommand.Parameters.AddWithValue("@SpouseName", this.SpouseName);
            sqlCommand.Parameters.AddWithValue("@PensElig", this.PensionEligible == 0 ? 1 : this.PensionEligible);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");

            if (ImportOption == "EmployeeAddress")
            {
                string Query = string.Empty;
                string Query1 = string.Empty;
                Query = "update Emp_Address set ";
                Query = Query + " [EmployeeId] =  '" + this.EmployeeId.ToString() + "'";
                if (!string.IsNullOrEmpty(this.PersonalMobileNo.ToString()))
                    Query = Query + " [PersonalMobileNo] =  '" + this.PersonalMobileNo.ToString() + "'";
                if (!string.IsNullOrEmpty(this.OfficeMobileNo.ToString()))
                    Query = Query + " ,[OfficeMobileNo] =  '" + this.OfficeMobileNo.ToString() + "'";
                if (!string.IsNullOrEmpty(this.ExtensionNo.ToString()))
                    Query = Query + " ,[ExtensionNo] =  '" + this.ExtensionNo.ToString() + "'";
                if (!string.IsNullOrEmpty(this.PersonalEmail.ToString()))
                    Query = Query + " ,[PersonalEmail] =  '" + this.PersonalEmail.ToString() + "'";
                if (!string.IsNullOrEmpty(this.OfficeMobileNo.ToString()))
                    Query = Query + " ,[OfficeEmail] =  '" + this.OfficeMobileNo.ToString() + "'";
                if (!string.IsNullOrEmpty(this.BloodGroup.ToString()))
                    Query = Query + " ,[BloodGroup] =  '" + this.BloodGroup.ToString() + "'";
                if (!string.IsNullOrEmpty(this.PANNumber.ToString()))
                    Query = Query + " ,[PANNumber] =  '" + this.PANNumber.ToString() + "'";
                if (!string.IsNullOrEmpty(this.PFNumber.ToString()))
                    Query = Query + " ,[PFNumber] =  '" + this.PFNumber.ToString() + "'";
                if (!string.IsNullOrEmpty(this.ESINumber.ToString()))
                    Query = Query + " ,[ESINumber] =  '" + this.ESINumber.ToString() + "'";
                if (!string.IsNullOrEmpty(this.MaritalStatus.ToString()))
                    Query = Query + " ,[MaritalStatus] =  '" + this.MaritalStatus.ToString() + "'";
                if (!string.IsNullOrEmpty(this.NoOfChildren.ToString()))
                    Query = Query + " ,[NoOfChildren] =  '" + this.NoOfChildren.ToString() + "'";
                if (!string.IsNullOrEmpty(this.PFConfirmationDate.ToString()))
                    Query = Query + " ,[PFConfirmationDate] =  '" + this.PFConfirmationDate.ToString() + "'";
                if (!string.IsNullOrEmpty(this.AADHARNumber.ToString()))
                    Query = Query + " ,[AADHARNumber] =  '" + this.AADHARNumber.ToString() + "'";
                if (!string.IsNullOrEmpty(this.PFUAN.ToString()))
                    Query = Query + " ,[PFUAN] =  '" + this.PFUAN.ToString() + "'";
                if (!string.IsNullOrEmpty(this.FatherName.ToString()))
                    Query = Query + " ,[FatherName] =  '" + this.FatherName.ToString() + "'";
                if (!string.IsNullOrEmpty(this.SpouseName.ToString()))
                    Query = Query + " ,[SpouseName] =  '" + this.SpouseName.ToString() + "'";
                if (!string.IsNullOrEmpty(this.PensionEligible.ToString()))
                    Query = Query + " ,[PensionEligible] =  '" + this.PensionEligible.ToString() + "'";
                if (!string.IsNullOrEmpty(this.ModifiedBy.ToString()))
                    Query = Query + " ,[ModifiedBy] =  '" + this.ModifiedBy.ToString() + "'";
                Query = Query + " ,[ModifiedOn] = GETDATE()";
                Query = Query + "  Where  Id ='" + this.Id.ToString() + "'";
            }

            if (status)
            {
                this.Id = new Guid(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the Emp_Personal
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Personal_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the Emp_Personal
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id)
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Personal_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues(String Id)
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Personal_SelectAll");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        /// <summary>
        /// Select the Emp_Personal
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, string filterExpr)
        {
            string query = "SELECT [Id] ,[EmployeeId],[PersonalMobileNo],[OfficeMobileNo],[ExtensionNo],[PersonalEmail],[OfficeEmail],"
                         + "[BloodGroup],[PrintCheque],[IsSeniorCitizen],[PaySlipRemarks],[IsDisable],[CreatedBy],[CreatedOn],[ModifiedBy]"
                         + " ,[ModifiedOn],[IsDeleted],[PANNumber] ,[PFNumber],[ESINumber],[MaritalStatus],[NoOfChildren],[PFConfirmationDate],[AADHARNumber] "
                         + " ,[PFUAN],[FatherName],[SpouseName] FROM Emp_Personal WHERE[EmployeeId] ='" + id + "' " + filterExpr;
            SqlCommand sqlCommand = new SqlCommand("USP_EXECQUERY");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@QUERY", query);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        #endregion

    }
}

