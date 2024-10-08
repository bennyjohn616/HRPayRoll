// -----------------------------------------------------------------------
// <copyright file="EntityMasterValue.cs" company="Microsoft">
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
    /// To handle the EntityMasterValue
    /// </summary>
    public class EntityMasterValue
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EntityMasterValue()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="id"></param>
        public EntityMasterValue(Guid id)
        {
            this.Id = id;
            DataTable dtValue = this.GetTableValues(this.Id, Guid.Empty, string.Empty);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EntityId"])))
                    this.EntityId = new Guid(Convert.ToString(dtValue.Rows[0]["EntityId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttributeModelId"])))
                    this.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[0]["AttributeModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EntityModelId"])))
                    this.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[0]["EntityModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RefEntityModelId"])))
                    this.RefEntityModelId = Convert.ToString(dtValue.Rows[0]["RefEntityModelId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RefEntityId"])))
                    this.RefEntityId = new Guid(Convert.ToString(dtValue.Rows[0]["RefEntityId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Value"])))
                    this.Value = Convert.ToString(dtValue.Rows[0]["Value"]);
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
        /// Get or Set the EntityId
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Get or Set the AttributeModelId
        /// </summary>
        public Guid AttributeModelId { get; set; }

        /// <summary>
        /// Get or Set the EntityModelId
        /// </summary>
        public Guid EntityModelId { get; set; }

        /// <summary>
        /// Get or Set the RefEntityModelId
        /// </summary>
        public string RefEntityModelId { get; set; }

        /// <summary>
        /// Get or Set the RefEntityId
        /// </summary>
        public Guid RefEntityId { get; set; }

        /// <summary>
        /// Get or Set the Value
        /// </summary>
        public string Value { get; set; }

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

        public DateTime IncrementDate { get; set; }

        /// <summary>
        /// Get or Set the IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

        public string Query { get; set; }
        public string ImportOption { get; set; }
        
        public string Component { get; set; }
        public decimal PerAnnum { get; set; }
        public  decimal PerMonth { get; set; }


        public string EmpName { get; set; }

        public  string AttName { get; set; }

        public string EmpConde { get; set; }




        #endregion

        #region Public methods


        /// <summary>
        /// Save the EntityMasterValue
        /// </summary>
        /// <returns></returns>
        /// 


        public  List<EntityMasterValue> FlexiReport(Guid empid)
        {
            List<EntityMasterValue> entityMasters = new List<EntityMasterValue>();
            DataTable dt = this.GetFlexiPayRPT(empid);
            if(dt.Rows.Count > 0)
            {
                for(int i =0; i < dt.Rows.Count; i++)
                {
                    EntityMasterValue masterValue = new EntityMasterValue();
                    if(!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["EmployeeCode"])))
                    masterValue.EmpConde = Convert.ToString( dt.Rows[i]["EmployeeCode"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["EmployeeName"])))
                        masterValue.EmpName = Convert.ToString(dt.Rows[i]["EmployeeName"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["DisplayAs"])))
                        masterValue.AttName = Convert.ToString(dt.Rows[i]["DisplayAs"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Value"])))
                        masterValue.Value = Convert.ToString(dt.Rows[i]["Value"]);
                    entityMasters.Add(masterValue);
                }
               
            }
            return entityMasters;
        }
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("EntityMasterValue_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EntityId", this.EntityId);
            sqlCommand.Parameters.AddWithValue("@AttributeModelId", this.AttributeModelId);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", this.EntityModelId);
            sqlCommand.Parameters.AddWithValue("@RefEntityModelId", this.RefEntityModelId);
            sqlCommand.Parameters.AddWithValue("@RefEntityId", this.RefEntityId);
            sqlCommand.Parameters.AddWithValue("@Value", this.Value);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
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
        public bool SaveFlaxe()
        {

            SqlCommand sqlCommand = new SqlCommand("EntityFlaxieValue_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EntityId", this.EntityId);
            sqlCommand.Parameters.AddWithValue("@AttributeModelId", this.AttributeModelId);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", this.EntityModelId);
            sqlCommand.Parameters.AddWithValue("@RefEntityModelId", this.RefEntityModelId);
            sqlCommand.Parameters.AddWithValue("@RefEntityId", this.RefEntityId);
            sqlCommand.Parameters.AddWithValue("@Value", this.Value);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
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
        public bool SaveIncrementDate()
        {

            SqlCommand sqlCommand = new SqlCommand("IncrementDate_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.RefEntityId);
            sqlCommand.Parameters.AddWithValue("@EffectiveDate", this.IncrementDate);
             sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
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


        public bool EmployeeSave()
        {

            SqlCommand sqlCommand = new SqlCommand("FlexiPayValue_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EntityId", this.EntityId);
            sqlCommand.Parameters.AddWithValue("@AttributeModelId", this.AttributeModelId);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", this.EntityModelId);
            sqlCommand.Parameters.AddWithValue("@RefEntityModelId", this.RefEntityModelId);
            sqlCommand.Parameters.AddWithValue("@RefEntityId", this.RefEntityId);
            sqlCommand.Parameters.AddWithValue("@Value", this.Value);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
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
        /// Delete the EntityMasterValue
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("EntityMasterValue_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }

        public bool SaveMasterInputSettings(DataTable dt,Guid entityId)
        {
            SqlCommand sqlCommand = new SqlCommand("Usp_MasterInputsettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EntityId", entityId);
            sqlCommand.Parameters.AddWithValue("@Type", "DELETE");
            DBOperation dbOperation = new DBOperation();
            dbOperation.DeleteData(sqlCommand);
            
            return dbOperation.bulksave(dt);
        }
        public DataTable MasterFlexipay(Guid AttributeModelId, Guid empID)
        {
            SqlCommand sqlCommand = new SqlCommand("select * from EntityMasterValue where AttributeModelId=@AttributeModelId and RefEntityId=@EmpId  "); /*and not EntityId =CAST(0x0 AS UNIQUEIDENTIFIER)  and not EntityModelId =CAST(0x0 AS UNIQUEIDENTIFIER)*/
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Parameters.AddWithValue("@AttributeModelId", AttributeModelId);
            sqlCommand.Parameters.AddWithValue("@EmpId", empID);
            DBOperation dBOperation = new DBOperation();
            return dBOperation.GetTableData(sqlCommand);
        }
        public DataTable MasterBasicpay(Guid AttributeModelId, Guid empID)
        {
            SqlCommand sqlCommand = new SqlCommand("select * from EntityMasterValue where AttributeModelId=@AttributeModelId and RefEntityId=@EmpId ");
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Parameters.AddWithValue("@AttributeModelId", AttributeModelId);
            sqlCommand.Parameters.AddWithValue("@EmpId", empID);
            DBOperation dBOperation = new DBOperation();
            return dBOperation.GetTableData(sqlCommand);
        }
        public DataTable IncrementCheck(  Guid empID)
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT MAX(EFFECTIVEDATE) FROM INCREMENT WHERE EMPLOYEEID=@EmpId  AND ISDELETED=0");
            sqlCommand.CommandType = CommandType.Text;
             sqlCommand.Parameters.AddWithValue("@EmpId", empID);
            DBOperation dBOperation = new DBOperation();
            return dBOperation.GetTableData(sqlCommand);
        }
        public DataTable PayrollHistory(Guid AttributeModelId)
        {
            SqlCommand sqlCommand = new SqlCommand("select * from PayrollHistory where EmployeeId = @AttributeModelId");
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Parameters.AddWithValue("@AttributeModelId", AttributeModelId);
            DBOperation dBOperation = new DBOperation();
            return dBOperation.GetTableData(sqlCommand);
        }
        #endregion

        #region private methods


        /// <summary>
        /// Select the EntityMasterValue
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid id, Guid refEntityId, string refEntityModelId)
        {

            SqlCommand sqlCommand = new SqlCommand("EntityMasterValue_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@RefEntityModelId", refEntityModelId);
            sqlCommand.Parameters.AddWithValue("@RefEntityId", refEntityId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        protected internal DataTable GetEntityValues(Guid EntityId, Guid refEntityId, string refEntityModelId)
        {

            SqlCommand sqlCommand = new SqlCommand("EntityMasterEntity_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EntityId", EntityId);
            sqlCommand.Parameters.AddWithValue("@RefEntityModelId", refEntityModelId);
            sqlCommand.Parameters.AddWithValue("@RefEntityId", refEntityId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        //maddy
        public   DataTable GetFlexiPayRPT(Guid id)
        {
            SqlCommand sqlcommand = new SqlCommand("FlexiPayRPT_Select");
            sqlcommand.CommandType = CommandType.StoredProcedure;
            sqlcommand.Parameters.AddWithValue("@Id", id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlcommand);
        }

        #endregion

    }


    public class EntityMasterSettings
    {
        public Guid EntitymodelId { get; set; }
        public Guid AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeBehaviorType { get; set; }
        public string AttributeInputType { get; set; }
        public bool IsVisible { get; set; }

        public List<EntityMasterSettings> entityMastersettingList(Guid entityModelId)
        {
            List<EntityMasterSettings> masterSettingList = new List<PayrollBO.EntityMasterSettings>();
                DataTable dtValue = this.GetTableValues(entityModelId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    EntityMasterSettings mastersettings = new EntityMasterSettings();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Entityid"])))
                        mastersettings.EntitymodelId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Entityid"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AttributeId"])))
                        mastersettings.AttributeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AttributeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["IsVisible"])))
                        mastersettings.IsVisible = Convert.ToBoolean(dtValue.Rows[rowcount]["IsVisible"]);
                    masterSettingList.Add(mastersettings);
                }
            }

            return masterSettingList;
        }


        protected internal DataTable GetTableValues(Guid EntityId)
        {

            SqlCommand sqlCommand = new SqlCommand("Usp_MasterInputsettings");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EntityId", EntityId);
            sqlCommand.Parameters.AddWithValue("@Type", "SELECT");
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
    }
}

