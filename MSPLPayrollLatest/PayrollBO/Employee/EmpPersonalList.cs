using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDBOperation;
using System.Data.SqlClient;
using System.Data;


namespace PayrollBO
{
   

    /// <summary>
    /// To handle the Emp_Personal
    /// </summary>
    public class Emp_PersonalList : List<Emp_Personal>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public Emp_PersonalList()
        {

        }


        public Emp_PersonalList(String Id)
        {
            DataTable dtValue = this.GetTableValues(Id);
            if (dtValue.Rows.Count > 0)
            {

                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Emp_Personal emppersonalTemp = new Emp_Personal();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        emppersonalTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        emppersonalTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    emppersonalTemp.PersonalMobileNo = Convert.ToString(dtValue.Rows[rowcount]["PersonalMobileNo"]);
                    emppersonalTemp.OfficeMobileNo = Convert.ToString(dtValue.Rows[rowcount]["OfficeMobileNo"]);
                    emppersonalTemp.ExtensionNo = Convert.ToString(dtValue.Rows[rowcount]["ExtensionNo"]);
                    emppersonalTemp.PersonalEmail = Convert.ToString(dtValue.Rows[rowcount]["PersonalEmail"]);
                    emppersonalTemp.OfficeEmail = Convert.ToString(dtValue.Rows[rowcount]["OfficeEmail"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BloodGroup"])))
                        emppersonalTemp.BloodGroup = Convert.ToInt32(dtValue.Rows[rowcount]["BloodGroup"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PrintCheque"])))
                        emppersonalTemp.PrintCheque = Convert.ToBoolean(dtValue.Rows[rowcount]["PrintCheque"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsSeniorCitizen"])))
                        emppersonalTemp.IsSeniorCitizen = Convert.ToBoolean(dtValue.Rows[rowcount]["IsSeniorCitizen"]);
                    emppersonalTemp.PaySlipRemarks = Convert.ToString(dtValue.Rows[rowcount]["PaySlipRemarks"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDisable"])))
                        emppersonalTemp.IsDisable = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDisable"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        emppersonalTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        emppersonalTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        emppersonalTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        emppersonalTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        emppersonalTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    emppersonalTemp.PANNumber = Convert.ToString(dtValue.Rows[rowcount]["PANNumber"]);
                    emppersonalTemp.PFNumber = Convert.ToString(dtValue.Rows[rowcount]["PFNumber"]);
                    emppersonalTemp.ESINumber = Convert.ToString(dtValue.Rows[rowcount]["ESINumber"]);
                    emppersonalTemp.MaritalStatus = Convert.ToString(dtValue.Rows[rowcount]["MaritalStatus"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["NoOfChildren"])))
                        emppersonalTemp.NoOfChildren = Convert.ToInt32(dtValue.Rows[rowcount]["NoOfChildren"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["PFConfirmationDate"])))
                        emppersonalTemp.PFConfirmationDate = Convert.ToDateTime(dtValue.Rows[rowcount]["PFConfirmationDate"]);
                    emppersonalTemp.AADHARNumber = Convert.ToString(dtValue.Rows[rowcount]["AADHARNumber"]);
                    emppersonalTemp.PFUAN = Convert.ToString(dtValue.Rows[rowcount]["PFUAN"]);
                    emppersonalTemp.FatherName = Convert.ToString(dtValue.Rows[rowcount]["FatherName"]);
                    emppersonalTemp.SpouseName = Convert.ToString(dtValue.Rows[rowcount]["SpouseName"]);
                    this.Add(emppersonalTemp);
                }
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



        protected internal DataTable GetTableValues(String Id)
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_Personal_SelectAll");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


    }

}
