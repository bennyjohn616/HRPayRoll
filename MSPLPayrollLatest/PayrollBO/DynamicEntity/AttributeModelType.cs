// -----------------------------------------------------------------------
// <copyright file="AttributeModelType.cs" company="Microsoft">
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
    /// To handle the AttributeModelType
    /// </summary>
    public class AttributeModelType
    {

        #region private variable

        private AttributeModelList _attributeModelList;

        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public AttributeModelType()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public AttributeModelType(Guid id, int companyId)
        {
            this.Id = id;
            this.CompanyId = companyId;
            DataTable dtValue = this.GetTableValues(this.Id, companyId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                this.Name = Convert.ToString(dtValue.Rows[0]["Name"]);
                this.DisplayAs = Convert.ToString(dtValue.Rows[0]["DisplayAs"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                    this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsActive"])))
                    this.IsActive = Convert.ToBoolean(dtValue.Rows[0]["IsActive"]);
            }
        }
        public List<AttributeModelType> Flexipay(int companyId,Guid id  )
        {
            this.Id = id;
            this.CompanyId = companyId;
            List<AttributeModelType> modelTypes = new List<AttributeModelType>();
            DataTable dtValue = this.GetFlexiValues(this.Id, companyId  );
            if (dtValue.Rows.Count > 0)
            {
                foreach (DataRow row in dtValue.Rows)
                {
                    AttributeModelType attributeModel = new AttributeModelType();

                    if (!string.IsNullOrEmpty(row["Id"].ToString()))
                        attributeModel.Id = new Guid(row["Id"].ToString());

                    attributeModel.Name = row["Name"].ToString();

                    attributeModel.FixedAmount = row["FixedAmount"].ToString();

                    if (!string.IsNullOrEmpty(row["IsReadOnly"].ToString()))
                        attributeModel.IsReadOnly = Convert.ToBoolean(row["IsReadOnly"] );

                    if (!string.IsNullOrEmpty(row["MasterComponentId"].ToString()))
                        attributeModel.MasterCompentId = new Guid(row["MasterComponentId"].ToString());

                    if (!string.IsNullOrEmpty(row["DisplayOrder"].ToString()))
                    attributeModel.DisplayOrder = Convert.ToInt32(row["DisplayOrder"]);

                    if (!string.IsNullOrEmpty(row["CreatedBy"].ToString()))
                        attributeModel.CreatedBy = Convert.ToInt32(row["CreatedBy"]);

                    if (!string.IsNullOrEmpty(row["CreatedOn"].ToString()))
                        attributeModel.CreatedOn = Convert.ToDateTime(row["CreatedOn"]);

                    if (!string.IsNullOrEmpty(row["ModifiedBy"].ToString()))
                        attributeModel.ModifiedBy = Convert.ToInt32(row["ModifiedBy"]);

                    if (!string.IsNullOrEmpty(row["ModifiedOn"].ToString()))
                        attributeModel.ModifiedOn = Convert.ToDateTime(row["ModifiedOn"]);

                    if (!string.IsNullOrEmpty(row["IsActive"].ToString()))
                        attributeModel.IsActive = Convert.ToBoolean(row["IsActive"]);

                    if (!string.IsNullOrEmpty(row["IsFlexiPay"].ToString()))
                        attributeModel.IsFlexiPay = Convert.ToBoolean(row["IsFlexiPay"]);

                    if (!string.IsNullOrEmpty(row["IsBasicPay"].ToString()))
                        attributeModel.IsBasicPay = Convert.ToBoolean(row["IsBasicPay"]);

                    modelTypes.Add(attributeModel);
                }

            }
            return modelTypes;
        }
         
        #endregion

        #region property


        /// <summary>
        /// Get or Set the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Get or Set the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or Set the DisplayAs
        /// </summary>
        public string DisplayAs { get; set; }

        public string FixedAmount { get; set; }

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
        /// Get or Set the IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// get or set the CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// get or set the attribute model list
        /// </summary>
        /// 
        public int? DisplayOrder { get; set; }

        public bool IsReadOnly { get; set; }

        public bool IsBasicPay { get; set; }

        public bool IsFlexiPay { get; set; }

        public Guid MasterCompentId { get; set; }

        public AttributeModelList AttributeModelList
        {
            get
            {
                if (object.ReferenceEquals(_attributeModelList, null))
                {
                    if (this.Id != Guid.Empty)
                    {
                        _attributeModelList = new AttributeModelList(this.CompanyId, this.Id);

                    }
                    else
                    {
                        _attributeModelList = new AttributeModelList();
                    }
                }
                return _attributeModelList;
            }
            set
            {
                _attributeModelList = value;
            }
        }
        #endregion

        #region Public methods


        /// <summary>
        /// Save the AttributeModelType
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("AttributeModelType_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@Name", this.Name);
            sqlCommand.Parameters.AddWithValue("@DisplayAs", this.DisplayAs);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsActive", this.IsActive);
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
        public bool SaveFlexiComponent()
        {

            SqlCommand sqlCommand = new SqlCommand("FlexiPayComponent_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@Name", this.Name);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@MasterComponentId", this.MasterCompentId);
            sqlCommand.Parameters.AddWithValue("@FixedAmount", this.FixedAmount);
            sqlCommand.Parameters.AddWithValue("@IsActive", this.IsActive);
            sqlCommand.Parameters.AddWithValue("@IsReadOnly", this.IsReadOnly);
            sqlCommand.Parameters.AddWithValue("@IsFlexiPay", this.IsFlexiPay);
            sqlCommand.Parameters.AddWithValue("@IsBasicPay", this.IsBasicPay);
            sqlCommand.Parameters.AddWithValue("@DisplayOrder", this.DisplayOrder);
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
        /// <summary>
        /// Delete the AttributeModelType
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("AttributeModelType_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        #endregion

        #region private methods


        /// <summary>
        /// Select the AttributeModelType
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, int companyId)
        {

            SqlCommand sqlCommand = new SqlCommand("AttributeModelType_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

         
        protected internal DataTable GetFlexiValues(Guid id, int companyId)
        {

            SqlCommand sqlCommand = new SqlCommand("FlexiPayComponent_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        #endregion

    }
}

