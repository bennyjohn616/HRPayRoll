// -----------------------------------------------------------------------
// <copyright file="MonthlyInput.cs" company="Microsoft">
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
    /// To handle the MonthlyInput
    /// </summary>
    public class MonthlyInput
    {

        #region private variable
        private AttributeModel _attributeModel;
        // private Guid employeeId;
        // private object month;
        // private object year;

        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public MonthlyInput()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public MonthlyInput(Guid id)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EntityId"])))
                    this.EntityId = new Guid(Convert.ToString(dtValue.Rows[0]["EntityId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EntityModelId"])))
                    this.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[0]["EntityModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Month"])))
                    this.Month = Convert.ToInt32(dtValue.Rows[0]["Month"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Year"])))
                    this.Year = Convert.ToInt32(dtValue.Rows[0]["Year"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttributeModelId"])))
                    this.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[0]["AttributeModelId"]));
                this.Value = Convert.ToString(dtValue.Rows[0]["Value"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
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
        /// Get or Set the EntityId
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Get or Set the EntityModelId
        /// </summary>
        public Guid EntityModelId { get; set; }

        /// <summary>
        /// Get or Set the Month
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// get or set the year
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Get or Set the AttributeModelId
        /// </summary>
        public Guid AttributeModelId { get; set; }

        /// <summary>
        /// Get or Set the Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }
        public string Query { get; set; }
        public string ImportOption { get; set; }
        public AttributeModel Attributemodel
        {
            get
            {
                if (object.ReferenceEquals(_attributeModel, null))
                {
                    if (this.AttributeModelId != Guid.Empty)
                    {
                        _attributeModel = new AttributeModel(this.AttributeModelId,0);
                    }
                    else
                        _attributeModel = new AttributeModel();

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

        public bool SaveSetting(previousComponents pre, Guid ent,Guid cat ,Guid entmod)
        {

            SqlCommand sqlCommand = new SqlCommand("MonthlyInput_SaveSetting");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@categoryId", cat);
            sqlCommand.Parameters.AddWithValue("@EntityId", ent);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", entmod);
            sqlCommand.Parameters.AddWithValue("@Name", pre.Name);
            sqlCommand.Parameters.AddWithValue("@MappedColumn", pre.MappedColumn);
            sqlCommand.Parameters.AddWithValue("@NameId", pre.Id);
            sqlCommand.Parameters.AddWithValue("@MappedColumnId", pre.MappedId);
            sqlCommand.Parameters.AddWithValue("@Radio", pre.radio);

            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
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
        /// Save the MonthlyInput
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("MonthlyInput_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@EntityId", this.EntityId);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", this.EntityModelId);
            sqlCommand.Parameters.AddWithValue("@Month", this.Month);
            sqlCommand.Parameters.AddWithValue("@Year", this.Year);
            sqlCommand.Parameters.AddWithValue("@AttributeModelId", this.AttributeModelId);
            sqlCommand.Parameters.AddWithValue("@Value", this.Value);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
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

        public bool BulkSave(DataTable dt1)
        {

            SqlCommand sqlCommand = new SqlCommand("MonthlyInput_SaveBulk");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@MonData", dt1);
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
        /// Delete the MonthlyInput
        /// </summary>
        /// <returns></returns> 
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("MonthlyInput_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }

        public bool BulkDelete(DataTable dt1)
        {

            SqlCommand sqlCommand = new SqlCommand("MonthlyInput_DeleteBulk");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@MonData", dt1);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.DeleteData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.Id = new Guid(outValue);
            }
            return status;
        }



        #endregion

        #region private methods


        /// <summary>
        /// Select the MonthlyInput
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id)
        {

            SqlCommand sqlCommand = new SqlCommand("MonthlyInput_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", Guid.Empty);
            sqlCommand.Parameters.AddWithValue("@EntityId", Guid.Empty);
            sqlCommand.Parameters.AddWithValue("@Month", 0);
            sqlCommand.Parameters.AddWithValue("@Year", 0);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public DataTable GetTableValueSettings(Guid category,Guid entity,Guid entitymodel)
        {

            SqlCommand sqlCommand = new SqlCommand("SelectMonthlyInput_Setting");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@categoryId", category);
            sqlCommand.Parameters.AddWithValue("@EntityId", entity);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", entitymodel);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetTableValues(Guid entityId, Guid employeeId, int month, int year)
        {

            SqlCommand sqlCommand = new SqlCommand("MonthlyInput_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", Guid.Empty);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@EntityId", entityId);
            sqlCommand.Parameters.AddWithValue("@Month", month);
            sqlCommand.Parameters.AddWithValue("@Year", year);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        protected internal DataTable GetSettingTableValues(Guid entityId, Guid CategoryId)
        {
            SqlCommand sqlCommand = new SqlCommand("MonthlyInput_SelectSetting");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CategoryId", CategoryId);
            sqlCommand.Parameters.AddWithValue("@EntityId", entityId);
         
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        //---------
        protected internal DataTable GetTableValues(Guid entityId, Guid entitymodelId, Guid employeeId)
        {

            SqlCommand sqlCommand = new SqlCommand("MonthlyInputEntity_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
       sqlCommand.Parameters.AddWithValue("@EntityModelId", entitymodelId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@EntityId", entityId);
         //   sqlCommand.Parameters.AddWithValue("@Month", month);
        //   sqlCommand.Parameters.AddWithValue("@Year", year);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        ////testing

        protected internal DataTable GetTableValues(Guid AttributeModelId, Guid emploeeId)
        {

            SqlCommand sqlCommand = new SqlCommand("MonthlyInputLOP_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", emploeeId);
            sqlCommand.Parameters.AddWithValue("@AttributeModelId", AttributeModelId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetEmpLastEntryMonthlyInputValues(Guid EntityId, Guid EntityModelId, Guid employeeId)
        {

            SqlCommand sqlCommand = new SqlCommand("Emp_LastMonthlyInput");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EntityId", EntityId);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", EntityModelId);
            sqlCommand.Parameters.AddWithValue("@EmpId", employeeId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        #endregion

    }
    public class previousComponents
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MappedColumn { get; set; }
        public Guid MappedId { get; set; }

        public string radio { get; set; }
        public List<newComponents> attr { get; set; }
        public previousComponents()
        {

        }
    }
    public class previousComponentslist:List<previousComponents>
    {
       
        public previousComponentslist()
        {

        }

        public previousComponentslist(Guid cat, Guid ent,Guid entmod)
        {

            DataTable dtValue = new MonthlyInput().GetTableValueSettings(cat, ent,entmod);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    previousComponents emp = new previousComponents();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["NameId"])))
                        emp.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["NameId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Name"])))
                        emp.Name = Convert.ToString(dtValue.Rows[rowcount]["Name"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["MappedColumn"])))
                        emp.MappedColumn = Convert.ToString(dtValue.Rows[rowcount]["MappedColumn"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["MappedColumnId"])))
                        emp.MappedId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["MappedColumnId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Radio"])))
                        emp.radio = Convert.ToString(dtValue.Rows[rowcount]["Radio"]);
                    
                    this.Add(emp);
                }
            }
        }

    }

    public class newComponents
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public newComponents()
        {

        }
    }
}


