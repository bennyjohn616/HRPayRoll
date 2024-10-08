

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    public class FormCommandList : List<FormCommand>
    {
        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public FormCommandList()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public FormCommandList(bool getAllCommand)
        {
            if (!getAllCommand)
                return;
            FormCommand formCommand = new FormCommand();
            DataTable dtValue = formCommand.GetTableValues(0);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    FormCommand formCommandTemp = new FormCommand();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        formCommandTemp.Id = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);
                    formCommandTemp.CommandName = Convert.ToString(dtValue.Rows[rowcount]["CommandName"]);
                    formCommandTemp.Description = Convert.ToString(dtValue.Rows[rowcount]["Description"]);
                    formCommandTemp.CommandTypes = Convert.ToString(dtValue.Rows[rowcount]["CommandTypes"]);
                    formCommandTemp.TableName = Convert.ToString(dtValue.Rows[rowcount]["TableName"]);
                    formCommandTemp.ColumnName = Convert.ToString(dtValue.Rows[rowcount]["ColumnName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDefaultRead"])))
                        formCommandTemp.IsDefaultRead = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDefaultRead"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDefaultWrite"])))
                        formCommandTemp.IsDefaultWrite = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDefaultWrite"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDefaultRequired"])))
                        formCommandTemp.IsDefaultRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDefaultRequired"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDefaultApprovel"])))
                        formCommandTemp.IsDefaultApprovel = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDefaultApprovel"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDefaultTransaction"])))
                        formCommandTemp.IsDefaultTransaction = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDefaultTransaction"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ModuleType"])))
                        formCommandTemp.ModuleType = Convert.ToString(dtValue.Rows[rowcount]["ModuleType"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ParentMenu"])))
                        formCommandTemp.ParentMenu = Convert.ToString(dtValue.Rows[rowcount]["ParentMenu"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DisplayOrder"])))
                        formCommandTemp.DisplayOrder = Convert.ToInt32(dtValue.Rows[rowcount]["DisplayOrder"]);
                    //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ParentId"]))) -- Need to check
                    //   formCommandTemp.ParentId = Convert.ToInt32(dtValue.Rows[rowcount]["ParentId"]);
                    //  if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DependentId"])))
                    //     formCommandTemp.DependentId = Convert.ToInt32(dtValue.Rows[rowcount]["DependentId"]);
                    this.Add(formCommandTemp);
                }
            }
        }
        public FormCommandList(string type)
        {
            
            FormCommand formCommand = new FormCommand();
            DataTable dtValue = formCommand.GetTableValues(0, type);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    if (Convert.ToString(dtValue.Rows[rowcount]["TableName"]) !="NULL")
                    {
                        FormCommand formCommandTemp = new FormCommand();
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                            formCommandTemp.Id = Convert.ToInt32(dtValue.Rows[rowcount]["Id"]);
                        formCommandTemp.CommandName = Convert.ToString(dtValue.Rows[rowcount]["CommandName"]);
                        formCommandTemp.Description = Convert.ToString(dtValue.Rows[rowcount]["Description"]);
                        formCommandTemp.CommandTypes = Convert.ToString(dtValue.Rows[rowcount]["CommandTypes"]);
                        formCommandTemp.TableName = Convert.ToString(dtValue.Rows[rowcount]["TableName"]);
                        formCommandTemp.ColumnName = Convert.ToString(dtValue.Rows[rowcount]["ColumnName"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDefaultRead"])))
                            formCommandTemp.IsDefaultRead = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDefaultRead"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDefaultWrite"])))
                            formCommandTemp.IsDefaultWrite = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDefaultWrite"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDefaultRequired"])))
                            formCommandTemp.IsDefaultRequired = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDefaultRequired"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDefaultApprovel"])))
                            formCommandTemp.IsDefaultApprovel = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDefaultApprovel"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsDefaultTransaction"])))
                            formCommandTemp.IsDefaultTransaction = Convert.ToBoolean(dtValue.Rows[rowcount]["IsDefaultTransaction"]);
                        //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ParentId"]))) -- Need to check
                        //   formCommandTemp.ParentId = Convert.ToInt32(dtValue.Rows[rowcount]["ParentId"]);
                        //  if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["DependentId"])))
                        //     formCommandTemp.DependentId = Convert.ToInt32(dtValue.Rows[rowcount]["DependentId"]);
                        this.Add(formCommandTemp);
                    }
                }
            }
        }

        #endregion

        #region property


        #endregion

        #region Public methods

        /// <summary>
        /// Save the formCommand and add to the list
        /// </summary>
        /// <param name="setting"></param>
        public void AddNew(FormCommand formCommand)
        {
            if (formCommand.Save())
            {
                this.Add(formCommand);
            }
        }

        /// <summary>
        /// Delete the formCommand and remove from the list
        /// </summary>
        /// <param name="setting"></param>
        public void DeleteExist(FormCommand formCommand)
        {
            if (formCommand.Delete())
            {
                this.Remove(formCommand);
            }
        }

        #endregion

        #region private methods




        #endregion
    }
}
