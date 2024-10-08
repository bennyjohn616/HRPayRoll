
namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    public class RoleFormCommandList : List<RoleFormCommand>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public RoleFormCommandList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public RoleFormCommandList(int companyId)
        {
            this.CompanyId = companyId;
            RoleFormCommand roleFormCommand = new RoleFormCommand();
            DataTable dtValue = roleFormCommand.GetTableValues(Guid.Empty, this.CompanyId, 0, 0);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    RoleFormCommand roleFormCommandTemp = new RoleFormCommand();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        roleFormCommandTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FormCommandId"])))
                        roleFormCommandTemp.FormCommandId = Convert.ToInt32(dtValue.Rows[rowcount]["FormCommandId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RoleId"])))
                        roleFormCommandTemp.RoleId = Convert.ToInt32(dtValue.Rows[rowcount]["RoleId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        roleFormCommandTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsRead"])))
                        roleFormCommandTemp.IsRead = Convert.ToBoolean(dtValue.Rows[rowcount]["IsRead"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsWrite"])))
                        roleFormCommandTemp.IsWrite = Convert.ToBoolean(dtValue.Rows[rowcount]["IsWrite"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsRequired"])))
                        roleFormCommandTemp.IsRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsPayrollTransaction"])))
                        roleFormCommandTemp.IsPayrollTransaction = Convert.ToBoolean(dtValue.Rows[rowcount]["IsPayrollTransaction"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsApproval"])))
                        roleFormCommandTemp.IsApproval = Convert.ToBoolean(dtValue.Rows[rowcount]["IsApproval"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDelete"])))
                        roleFormCommandTemp.IsDelete = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDelete"]);
                    roleFormCommandTemp.ReadMessage = Convert.ToString(dtValue.Rows[rowcount]["ReadMessage"]);
                    roleFormCommandTemp.WriteMessage = Convert.ToString(dtValue.Rows[rowcount]["WriteMessage"]);
                    roleFormCommandTemp.RequiredMessage = Convert.ToString(dtValue.Rows[rowcount]["RequiredMessage"]);
                    roleFormCommandTemp.TransactionMessage = Convert.ToString(dtValue.Rows[rowcount]["TransactionMessage"]);
                    roleFormCommandTemp.ApprovalMessage = Convert.ToString(dtValue.Rows[rowcount]["ApprovalMessage"]);
                    roleFormCommandTemp.DeleteMessage = Convert.ToString(dtValue.Rows[rowcount]["DeleteMessage"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        roleFormCommandTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        roleFormCommandTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        roleFormCommandTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        roleFormCommandTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(roleFormCommandTemp);
                }
            }
        }

        public RoleFormCommandList(int companyId, int roleId, int formCommandId = 0)
        {
            this.CompanyId = companyId;
            this.RoleId = roleId;
            this.FormCommandId = formCommandId;
            RoleFormCommand roleFormCommand = new RoleFormCommand();
            DataTable dtValue = roleFormCommand.GetTableValues(Guid.Empty, this.CompanyId, this.RoleId, this.FormCommandId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    RoleFormCommand roleFormCommandTemp = new RoleFormCommand();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        roleFormCommandTemp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FormCommandId"])))
                        roleFormCommandTemp.FormCommandId = Convert.ToInt32(dtValue.Rows[rowcount]["FormCommandId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["RoleId"])))
                        roleFormCommandTemp.RoleId = Convert.ToInt32(dtValue.Rows[rowcount]["RoleId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        roleFormCommandTemp.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsRead"])))
                        roleFormCommandTemp.IsRead = Convert.ToBoolean(dtValue.Rows[rowcount]["IsRead"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsWrite"])))
                        roleFormCommandTemp.IsWrite = Convert.ToBoolean(dtValue.Rows[rowcount]["IsWrite"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsRequired"])))
                        roleFormCommandTemp.IsRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsPayrollTransaction"])))
                        roleFormCommandTemp.IsPayrollTransaction = Convert.ToBoolean(dtValue.Rows[rowcount]["IsPayrollTransaction"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsApproval"])))
                        roleFormCommandTemp.IsApproval = Convert.ToBoolean(dtValue.Rows[rowcount]["IsApproval"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDelete"])))
                    //    roleFormCommandTemp.IsDelete = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDelete"]);
                    roleFormCommandTemp.ReadMessage = Convert.ToString(dtValue.Rows[rowcount]["ReadMessage"]);
                    roleFormCommandTemp.WriteMessage = Convert.ToString(dtValue.Rows[rowcount]["WriteMessage"]);
                    roleFormCommandTemp.RequiredMessage = Convert.ToString(dtValue.Rows[rowcount]["RequiredMessage"]);
                    roleFormCommandTemp.TransactionMessage = Convert.ToString(dtValue.Rows[rowcount]["TransactionMessage"]);
                    roleFormCommandTemp.ApprovalMessage = Convert.ToString(dtValue.Rows[rowcount]["ApprovalMessage"]);
                    //roleFormCommandTemp.DeleteMessage = Convert.ToString(dtValue.Rows[rowcount]["DeleteMessage"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedBy"])))
                        roleFormCommandTemp.CreatedBy = Convert.ToInt32(dtValue.Rows[rowcount]["CreatedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CreatedOn"])))
                        roleFormCommandTemp.CreatedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["CreatedOn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedBy"])))
                        roleFormCommandTemp.ModifiedBy = Convert.ToInt32(dtValue.Rows[rowcount]["ModifiedBy"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModifiedOn"])))
                        roleFormCommandTemp.ModifiedOn = Convert.ToDateTime(dtValue.Rows[rowcount]["ModifiedOn"]);
                    this.Add(roleFormCommandTemp);
                }
            }
        }

        #endregion

        #region property


        /// <summary>
        /// Get or Set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// get or set the RoleId
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// get or set the Form CommandId
        /// </summary>
        public int FormCommandId { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Save the roleFormCommand and add to the list
        /// </summary>
        /// <param name="setting"></param>
        public void AddNew(RoleFormCommand roleFormCommand)
        {
            if (roleFormCommand.Save())
            {
                this.Add(roleFormCommand);
            }
        }

        /// <summary>
        /// Delete the roleFormCommand and remove from the list
        /// </summary>
        /// <param name="setting"></param>
        public void DeleteExist(RoleFormCommand roleFormCommand)
        {
            if (roleFormCommand.Delete())
            {
                this.Remove(roleFormCommand);
            }
        }

        #endregion

        #region private methods




        #endregion
    }
}
