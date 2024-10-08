
namespace PayrollBO.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SQLDBOperation;
    using System.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// To handle the ApprovelTransaction
    /// </summary>
    public class ApprovelTransactionList : List<ApprovelTransaction>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public ApprovelTransactionList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public ApprovelTransactionList(int companyId,string tablePrimaryId,string tableName,string columnName)
        {
            //pass Empty value if not required
            ApprovelTransaction formCommand = new ApprovelTransaction();
            DataTable dtValue = formCommand.GetTableValues(Guid.Empty,companyId,tablePrimaryId, tableName, columnName);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    ApprovelTransaction ApprovelTransactionTemp = new ApprovelTransaction();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        ApprovelTransactionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        ApprovelTransactionTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    ApprovelTransactionTemp.TablePrimaryId = Convert.ToString(dtValue.Rows[rowcount]["TablePrimaryId"]);
                    ApprovelTransactionTemp.TableSubPrimaryId = Convert.ToString(dtValue.Rows[rowcount]["TableSubPrimaryId"]);
                    ApprovelTransactionTemp.TableName = Convert.ToString(dtValue.Rows[rowcount]["TableName"]);
                    ApprovelTransactionTemp.ColumnName = Convert.ToString(dtValue.Rows[rowcount]["ColumnName"]);
                    ApprovelTransactionTemp.OldValue = Convert.ToString(dtValue.Rows[rowcount]["OldValue"]);
                    ApprovelTransactionTemp.NewValue = Convert.ToString(dtValue.Rows[rowcount]["NewValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsApproved"])))
                        ApprovelTransactionTemp.IsApproved = Convert.ToBoolean(dtValue.Rows[rowcount]["IsApproved"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApproverId"])))
                        ApprovelTransactionTemp.ApproverId = Convert.ToInt32(dtValue.Rows[rowcount]["ApproverId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        ApprovelTransactionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        ApprovelTransactionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        ApprovelTransactionTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        ApprovelTransactionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        ApprovelTransactionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    this.Add(ApprovelTransactionTemp);
                }
            }
        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public ApprovelTransactionList(int companyId, string tableName)
        {
            ApprovelTransaction formCommand = new ApprovelTransaction();
            DataTable dtValue = formCommand.GetTableValues(Guid.Empty, companyId, "", tableName, "");
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    ApprovelTransaction ApprovelTransactionTemp = new ApprovelTransaction();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        ApprovelTransactionTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        ApprovelTransactionTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    ApprovelTransactionTemp.TablePrimaryId = Convert.ToString(dtValue.Rows[rowcount]["TablePrimaryId"]);
                    ApprovelTransactionTemp.TableSubPrimaryId = Convert.ToString(dtValue.Rows[rowcount]["TableSubPrimaryId"]);
                    ApprovelTransactionTemp.TableName = Convert.ToString(dtValue.Rows[rowcount]["TableName"]);
                    ApprovelTransactionTemp.ColumnName = Convert.ToString(dtValue.Rows[rowcount]["ColumnName"]);
                    ApprovelTransactionTemp.OldValue = Convert.ToString(dtValue.Rows[rowcount]["OldValue"]);
                    ApprovelTransactionTemp.NewValue = Convert.ToString(dtValue.Rows[rowcount]["NewValue"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsApproved"])))
                        ApprovelTransactionTemp.IsApproved = Convert.ToBoolean(dtValue.Rows[rowcount]["IsApproved"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApproverId"])))
                        ApprovelTransactionTemp.ApproverId = Convert.ToInt32(dtValue.Rows[rowcount]["ApproverId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDeleted"])))
                        ApprovelTransactionTemp.IsDeleted = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDeleted"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        ApprovelTransactionTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        ApprovelTransactionTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        ApprovelTransactionTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        ApprovelTransactionTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    this.Add(ApprovelTransactionTemp);
                }
            }
        }
        #endregion

        #region property


        #endregion

        #region Public methods

        /// <summary>
        ///  Save the approvelTransaction and add to the list
        /// </summary>
        /// <param name="approvelTransaction"></param>
        public void AddNew(ApprovelTransaction approvelTransaction)
        {
            if (approvelTransaction.Save())
            {
                this.Add(approvelTransaction);
            }
        }

        /// <summary>
        /// Delete the approvelTransaction and remove from the list
        /// </summary>
        /// <param name="approvelTransaction"></param>
        public void DeleteExist(ApprovelTransaction approvelTransaction)
        {
            if (approvelTransaction.Delete())
            {
                this.Remove(approvelTransaction);
            }
        }

        #endregion

        #region private methods




        #endregion
    }
}
