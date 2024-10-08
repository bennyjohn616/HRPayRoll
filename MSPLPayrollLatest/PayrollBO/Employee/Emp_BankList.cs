using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class Emp_BankList : List<Emp_Bank>

    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public Emp_BankList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="employeeId"></param>
        public Emp_BankList(Guid employeeId)
        {
            this.EmployeeId = employeeId;
            Emp_Bank employeeBank = new Emp_Bank();
            DataTable dtValue = employeeBank.GetTableValues(this.EmployeeId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Emp_Bank employeeBankTemp = new Emp_Bank();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeBankTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BankId"])))
                        employeeBankTemp.BankId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["BankId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        employeeBankTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    employeeBankTemp.AcctNo = Convert.ToString(dtValue.Rows[rowcount]["AcctNo"]);
                    employeeBankTemp.IFSC = Convert.ToString(dtValue.Rows[rowcount]["IFSC"]);
                    employeeBankTemp.BranchName = Convert.ToString(dtValue.Rows[rowcount]["BranchName"]);
                    employeeBankTemp.Address = Convert.ToString(dtValue.Rows[rowcount]["Address"]);
                    employeeBankTemp.City = Convert.ToString(dtValue.Rows[rowcount]["City"]);
                    employeeBankTemp.State = Convert.ToString(dtValue.Rows[rowcount]["State"]);
                    employeeBankTemp.CreatedBy = Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeBankTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    employeeBankTemp.ModifiedBy = Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeBankTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        employeeBankTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    this.Add(employeeBankTemp);
                }
            }
        }
        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="employeeId"></param>
        public Emp_BankList(Guid employeeId,string filterExpr)
        {
            this.EmployeeId = employeeId;
            Emp_Bank employeeBank = new Emp_Bank();
            DataTable dtValue = employeeBank.GetTableValues(this.EmployeeId, filterExpr);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Emp_Bank employeeBankTemp = new Emp_Bank();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeBankTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BankId"])))
                        employeeBankTemp.BankId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["BankId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        employeeBankTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    employeeBankTemp.AcctNo = Convert.ToString(dtValue.Rows[rowcount]["AcctNo"]);
                    employeeBankTemp.IFSC = Convert.ToString(dtValue.Rows[rowcount]["IFSC"]);
                    employeeBankTemp.BranchName = Convert.ToString(dtValue.Rows[rowcount]["BranchName"]);
                    employeeBankTemp.Address = Convert.ToString(dtValue.Rows[rowcount]["Address"]);
                    employeeBankTemp.City = Convert.ToString(dtValue.Rows[rowcount]["City"]);
                    employeeBankTemp.State = Convert.ToString(dtValue.Rows[rowcount]["State"]);
                    employeeBankTemp.CreatedBy = Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeBankTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    employeeBankTemp.ModifiedBy = Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeBankTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        employeeBankTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    this.Add(employeeBankTemp);
                }
            }
        }


        public Emp_BankList(String Id)
        {
            Emp_Bank employeeBank = new Emp_Bank();
            DataTable dtValue = employeeBank.GetTableValues(Id);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    Emp_Bank employeeBankTemp = new Emp_Bank();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeBankTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["BankId"])))
                        employeeBankTemp.BankId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["BankId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        employeeBankTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    employeeBankTemp.AcctNo = Convert.ToString(dtValue.Rows[rowcount]["AcctNo"]);
                    employeeBankTemp.IFSC = Convert.ToString(dtValue.Rows[rowcount]["IFSC"]);
                    employeeBankTemp.BranchName = Convert.ToString(dtValue.Rows[rowcount]["BranchName"]);
                    employeeBankTemp.Address = Convert.ToString(dtValue.Rows[rowcount]["Address"]);
                    employeeBankTemp.City = Convert.ToString(dtValue.Rows[rowcount]["City"]);
                    employeeBankTemp.State = Convert.ToString(dtValue.Rows[rowcount]["State"]);
                    employeeBankTemp.CreatedBy = Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeBankTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    employeeBankTemp.ModifiedBy = Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeBankTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsActive"])))
                        employeeBankTemp.IsActive = Convert.ToBoolean(dtValue.Rows[rowcount]["IsActive"]);
                    this.Add(employeeBankTemp);
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
        /// Get or Set the BankId
        /// </summary>
        public Guid BankId { get; set; }

        /// <summary>
        /// Get or Set the EmployeeId
        /// </summary>
        public Guid EmployeeId { get; set; }

        ///// <summary>
        ///// Get or Set the Bank Name
        ///// </summary>
        //public string BankName { get; set; }
        /// <summary>
        /// Get or Set the AcctNo
        /// </summary>
        public string AcctNo { get; set; }

        /// <summary>
        /// Get or Set the IFSC
        /// </summary>
        public string IFSC { get; set; }

        /// <summary>
        /// Get or Set the BranchName
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Get or Set the Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Get or Set the City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Get or Set the State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Get or Set the CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Get or Set the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Get or Set the ModifiedBy
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Get or Set the ModifiedOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Get or Set the IsActive
        /// </summary>
        public bool IsActive { get; set; }


        #endregion

        #region Public methods

        /// <summary>
        /// Save the Emp_Bank and add to the list
        /// </summary>
        /// <param name="employeeBank"></param>
        public void AddNew(Emp_Bank employeeBank)
        {
            if (employeeBank.Save())
            {
                this.Add(employeeBank);
            }
        }

        /// <summary>
        /// Delete the Emp_Bank and remove from the list
        /// </summary>
        /// <param name="employeeBank"></param>
        public void DeleteExist(Emp_Bank employeeBank)
        {
            if (employeeBank.Delete())
            {
                this.Remove(employeeBank);
            }
        }

        #endregion

        #region private methods



        #endregion
    }
}
