using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class PFChallan
    {

        #region private variable
        private AttributeModel _attributeModel;

        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public PFChallan()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public PFChallan(int id)
        {
            this.CompanyId = id;
            DataTable dtValue = this.GetTableValues(this.CompanyId,0);
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Company"])))
                    this.CompanyId = Convert.ToInt32(Convert.ToString(dtValue.Rows[0]["Company"]));
                this.TableName = Convert.ToString(dtValue.Rows[0]["TableName"]);
                this.ColumnName = Convert.ToString(dtValue.Rows[0]["FieldName"]);
                this.DisplayAs = Convert.ToString(dtValue.Rows[0]["DisplayAs"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                this.DisplayOrder = Convert.ToInt32(dtValue.Rows[0]["DisplayOrder"]);
            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get or Set the CofigurationId
        /// </summary>
        public int CompanyId { get; set; }



        /// <summary>
        /// Get or Set the TableName
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Get or Set the FieldName
        /// </summary>
        public string ColumnName { get; set; }


        /// <summary>
        /// Get or Set the Type
        /// </summary>
        public string DisplayAs { get; set; }
        public int DisplayOrder { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsActive { get; set; }

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
        /// Get or Set the ModifeidOn
        /// </summary>
        public DateTime ModifiedOn { get; set; }
        public string EmployeeId { get; set; }

        //---
        public string BaseValue { get; set; }
        public AttributeModel AttributeModel
        {
            // Modified By Keerthika on 21/04/2016
            get
            {
                  if (this.TableName == ComValue.SalaryTable|| this.TableName== "SalaryBase")

               // if (this.TableName == ComValue.SalaryTable)
                   {
                    if (object.ReferenceEquals(_attributeModel, null))
                    {
                        if (!string.IsNullOrEmpty(this.ColumnName))
                        {
                            _attributeModel = new AttributeModel(new Guid(this.ColumnName), this.CompanyId);

                        }
                        else
                        {
                            _attributeModel = new AttributeModel();
                        }
                    }
                }

                else


                {
                    _attributeModel = new AttributeModel { DisplayAs = this.ColumnName };
                }
                return _attributeModel;

            }


            set
            {
                _attributeModel = value;
            }
        }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the PaySlipAttributes
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("PFChallanTemplate_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@Company", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@TableName", this.TableName);
            sqlCommand.Parameters.AddWithValue("@ColumnName", this.ColumnName);
            sqlCommand.Parameters.AddWithValue("@DisplayAs", this.DisplayAs);
            sqlCommand.Parameters.AddWithValue("@DisplayOrder", this.DisplayOrder);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);

            sqlCommand.Parameters.Add("@IdOut", SqlDbType.Int).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = Convert.ToInt32(outValue);
            }
            return status;
        }

        /// <summary>
        /// Delete the PaySlipAttributes
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("PFChallanTemplate_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);

            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the PaySlipAttributes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(int companyId,int id)
        {

            SqlCommand sqlCommand = new SqlCommand("PFChallanTemplate_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}
