// -----------------------------------------------------------------------
// <copyright file="EmployeeAddressList.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EmployeeAddressList : List<EmployeeAddress>
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EmployeeAddressList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="employeeId"></param>
        public EmployeeAddressList(Guid employeeId)
        {
            this.EmployeeId = employeeId;
            EmployeeAddress employeeAddress = new EmployeeAddress();
            DataTable dtValue = employeeAddress.GetTableValues(this.EmployeeId,Guid.Empty);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EmployeeAddress employeeAddressTemp = new EmployeeAddress();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeAddressTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        employeeAddressTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    employeeAddressTemp.AddressLine1 = Convert.ToString(dtValue.Rows[rowcount]["AddressLine1"]);
                    employeeAddressTemp.AddressLine2 = Convert.ToString(dtValue.Rows[rowcount]["AddressLine2"]);
                    employeeAddressTemp.City = Convert.ToString(dtValue.Rows[rowcount]["City"]);
                    employeeAddressTemp.State = Convert.ToString(dtValue.Rows[rowcount]["State"]);
                    employeeAddressTemp.Country = Convert.ToString(dtValue.Rows[rowcount]["Country"]);
                    employeeAddressTemp.PinCode = Convert.ToString(dtValue.Rows[rowcount]["PinCode"]);
                    employeeAddressTemp.Phone = Convert.ToString(dtValue.Rows[rowcount]["Phone"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AddressType"])))
                        employeeAddressTemp.AddressType = Convert.ToInt32(dtValue.Rows[rowcount]["AddressType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeAddressTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeAddressTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeAddressTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeAddressTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeAddressTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(employeeAddressTemp);
                }
            }
        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="employeeId"></param>
        public EmployeeAddressList(Guid employeeId,string filterExpr)
        {
            this.EmployeeId = employeeId;
            EmployeeAddress employeeAddress = new EmployeeAddress();
            DataTable dtValue = employeeAddress.GetTableValues(this.EmployeeId, Guid.Empty,filterExpr);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EmployeeAddress employeeAddressTemp = new EmployeeAddress();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeAddressTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        employeeAddressTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    employeeAddressTemp.AddressLine1 = Convert.ToString(dtValue.Rows[rowcount]["AddressLine1"]);
                    employeeAddressTemp.AddressLine2 = Convert.ToString(dtValue.Rows[rowcount]["AddressLine2"]);
                    employeeAddressTemp.City = Convert.ToString(dtValue.Rows[rowcount]["City"]);
                    employeeAddressTemp.State = Convert.ToString(dtValue.Rows[rowcount]["State"]);
                    employeeAddressTemp.Country = Convert.ToString(dtValue.Rows[rowcount]["Country"]);
                    employeeAddressTemp.PinCode = Convert.ToString(dtValue.Rows[rowcount]["PinCode"]);
                    employeeAddressTemp.Phone = Convert.ToString(dtValue.Rows[rowcount]["Phone"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AddressType"])))
                        employeeAddressTemp.AddressType = Convert.ToInt32(dtValue.Rows[rowcount]["AddressType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeAddressTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeAddressTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeAddressTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeAddressTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeAddressTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(employeeAddressTemp);
                }
            }
        }

        public EmployeeAddressList(String Id)
        {
            EmployeeAddress employeeAddress = new EmployeeAddress();
            DataTable dtValue = employeeAddress.GetTableValues(Id);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EmployeeAddress employeeAddressTemp = new EmployeeAddress();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        employeeAddressTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        employeeAddressTemp.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    employeeAddressTemp.AddressLine1 = Convert.ToString(dtValue.Rows[rowcount]["AddressLine1"]);
                    employeeAddressTemp.AddressLine2 = Convert.ToString(dtValue.Rows[rowcount]["AddressLine2"]);
                    employeeAddressTemp.City = Convert.ToString(dtValue.Rows[rowcount]["City"]);
                    employeeAddressTemp.State = Convert.ToString(dtValue.Rows[rowcount]["State"]);
                    employeeAddressTemp.Country = Convert.ToString(dtValue.Rows[rowcount]["Country"]);
                    employeeAddressTemp.PinCode = Convert.ToString(dtValue.Rows[rowcount]["PinCode"]);
                    employeeAddressTemp.Phone = Convert.ToString(dtValue.Rows[rowcount]["Phone"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AddressType"])))
                        employeeAddressTemp.AddressType = Convert.ToInt32(dtValue.Rows[rowcount]["AddressType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        employeeAddressTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        employeeAddressTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        employeeAddressTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        employeeAddressTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        employeeAddressTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    this.Add(employeeAddressTemp);
                }
            }
        }

        #endregion

        #region property


        /// <summary>
        /// Get or Set the EmployeeId
        /// </summary>
        public Guid EmployeeId { get; set; }



        #endregion

        #region Public methods

        /// <summary>
        /// Save the EmployeeAddress and add to the list
        /// </summary>
        /// <param name="employeeAddress"></param>
        public void AddNew(EmployeeAddress employeeAddress)
        {
            if (employeeAddress.Save())
            {
                this.Add(employeeAddress);
            }
        }

        /// <summary>
        /// Delete the EmployeeAddress and remove from the list
        /// </summary>
        /// <param name="employeeAddress"></param>
        public void DeleteExist(EmployeeAddress employeeAddress)
        {
            if (employeeAddress.Delete())
            {
                this.Remove(employeeAddress);
            }
        }

        #endregion

        #region private methods



        #endregion
    }
}
