// -----------------------------------------------------------------------
// <copyright file="EntityBehavior.cs" company="Microsoft">
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
    //using Util;

    /// <summary>
    /// To handle the EntityBehavior
    /// </summary>
    public class EntityBehavior
    {

        #region private variable


        #endregion

        #region construstor


        /// <summary>
        /// initialize the object
        /// </summary>
        public EntityBehavior()
        {

        }

        /// <summary>
        /// initialize the object based on the provided value
        /// </summary>
        /// <param name="attrubuteModelid"></param>
        /// <param name="entityModelId"></param>
        /// <param name="entityId"></param>
        public EntityBehavior(Guid attrubuteModelid, Guid entityModelId, Guid entityId)
        {
            this.EntityId = entityId;
            this.EntityModelId = entityModelId;
            this.AttributeModelId = attrubuteModelid;
            DataTable dtValue = this.GetTableValues(this.AttributeModelId, this.EntityModelId, this.EntityId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EntityId"])))
                    this.EntityId = new Guid(Convert.ToString(dtValue.Rows[0]["EntityId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EntityModelId"])))
                    this.EntityModelId = new Guid(Convert.ToString(dtValue.Rows[0]["EntityModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttributeModelId"])))
                    this.AttributeModelId = new Guid(Convert.ToString(dtValue.Rows[0]["AttributeModelId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ValueType"])))
                    this.ValueType = Convert.ToInt32(dtValue.Rows[0]["ValueType"]);
                this.Formula = Convert.ToString(dtValue.Rows[0]["Formula"]);
               // if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Percentage"])))
                    this.Percentage = Convert.ToString(dtValue.Rows[0]["Percentage"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Maximum"])))
                    this.Maximum = Convert.ToString(dtValue.Rows[0]["Maximum"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RoundingId"])))
                    this.RoundingId = Convert.ToInt32(dtValue.Rows[0]["RoundingId"]);
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
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ArrearAttributeModelId"])))
                    this.ArrearAttributeModelId = new Guid(Convert.ToString(dtValue.Rows[0]["ArrearAttributeModelId"]));
                this.ArrearFormula = Convert.ToString(dtValue.Rows[0]["ArrearFormula"]);
                this.EligibiltyFormula = Convert.ToString(dtValue.Rows[0]["EligibiltyFormula"]);
                this.BaseFormula = Convert.ToString(dtValue.Rows[0]["BaseFormula"]);
                this.BaseValue = Convert.ToString(dtValue.Rows[0]["BaseValue"]);

            }
        }


        #endregion

        #region property


        /// <summary>
        /// Get or Set the AttrubuteModelId
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Get or Set the EntityModelId
        /// </summary>
        public Guid EntityModelId { get; set; }

        /// <summary>
        /// Get or Set the EntityModelId
        /// </summary>
        public Guid AttributeModelId { get; set; }

        /// <summary>
        /// Get or Set the ValueType
        /// </summary>
        public int ValueType { get; set; }

        /// <summary>
        /// Get or Set the Formula
        /// </summary>
        public string Formula { get; set; }

        /// <summary>
        /// Get or Set the Percentage
        /// </summary>
        public string Percentage { get; set; }

        /// <summary>
        /// Get or Set the Maximum
        /// </summary>
        public string Maximum { get; set; }

        /// <summary>
        /// Get or Set the RoundingId
        /// </summary>
        public int RoundingId { get; set; }

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
        /// Get or Set the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// get or set the ArrearAttributeModelId
        /// </summary>
        public Guid ArrearAttributeModelId { get; set; }

        /// <summary>
        /// get or set the ArrearFormula
        /// </summary>
        public string ArrearFormula { get; set; }

        /// <summary>
        /// get or set the elogibilty formula
        /// </summary>
        public string EligibiltyFormula { get; set; }

        public string BaseValue { get; set; }

        public string BaseFormula { get; set; }

        #endregion

        #region Public methods


        /// <summary>
        /// Save the AttributeModelBehavior
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            SqlCommand sqlCommand = new SqlCommand("EntityBehavior_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EntityId", this.EntityId);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", this.EntityModelId);
            sqlCommand.Parameters.AddWithValue("@AttributeModelId", this.AttributeModelId);
            sqlCommand.Parameters.AddWithValue("@ValueType", this.ValueType);
            sqlCommand.Parameters.AddWithValue("@Formula", this.Formula);
            sqlCommand.Parameters.AddWithValue("@Percentage", this.Percentage);
            sqlCommand.Parameters.AddWithValue("@Maximum", this.Maximum);
            sqlCommand.Parameters.AddWithValue("@RoundingId", this.RoundingId);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@IsActive", this.IsActive);
            sqlCommand.Parameters.AddWithValue("@ArrearAttributeModelId", this.ArrearAttributeModelId);
            sqlCommand.Parameters.AddWithValue("@ArrearFormula", this.ArrearFormula);
            sqlCommand.Parameters.AddWithValue("@EligibiltyFormula", this.EligibiltyFormula);

            sqlCommand.Parameters.AddWithValue("@BaseFormula", this.BaseFormula);
            sqlCommand.Parameters.AddWithValue("@BaseValue", this.BaseValue);

            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "");
            return status;
        }

        /// <summary>
        /// Delete the AttributeModelBehavior
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("EntityBehavior_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@AttrubuteModelId", this.EntityId);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
        /*  
        public void Validate(string input, List<EntityBehavior> lstCollection, string lhs, ref string error, ref string values, Guid inputAttributemodelId, EntityMasterValueList entityMasterlist)
          {
              // values = string.Empty;
              if (!string.IsNullOrEmpty(error))
                  return;
              if (!object.ReferenceEquals(input, null))
              {
                  string temp = input;
                  if (temp.IndexOf('{') >= 0)
                  {
                      string id = temp.Substring(temp.IndexOf('{') + 1, 36);
                      var colTemp = lstCollection.Where(u => u.AttributeModelId.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                      if (lhs == id)
                      {
                          error = "Error";
                          return;
                      }
                      string replacevalue = "{" + id + "}";
                      if (!object.ReferenceEquals(colTemp, null))
                      {
                          if (colTemp.Formula.IndexOf('{') >= 0)
                          {
                              input = input.Replace(replacevalue, colTemp.Formula);
                              //  lstCollection.Where(u => u.AttributeModelId == inputAttributemodelId).FirstOrDefault().Formula = input;

                              Validate(input, lstCollection, lhs, ref error, ref values, colTemp.AttributeModelId, entityMasterlist);
                          }
                          else if (input.IndexOf('{') >= 0)
                          {
                              input = input.Replace(replacevalue, colTemp.Formula);
                              // lstCollection.Where(u => u.AttributeModelId == inputAttributemodelId).FirstOrDefault().Formula = input;
                              Validate(input, lstCollection, lhs, ref error, ref values, inputAttributemodelId, entityMasterlist);
                          }
                          else
                          {
                              input = input.Replace(replacevalue, colTemp.Formula);
                              string outval = input;
                              values = input;

                              int result = 0;
                              EvaluateExpression.TestExpression(outval, out result);
                              // lstCollection.Where(u => u.AttributeModelId == inputAttributemodelId).FirstOrDefault().Formula = input;

                          }
                      }
                      else
                      {
                          var entitymaster = entityMasterlist.Where(u => u.AttributeModelId.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                          input = input.Replace(replacevalue, entitymaster == null ? "1" : entitymaster.Value);

                          if (input.IndexOf('{') >= 0)
                          {
                              // lstCollection.Where(u => u.AttributeModelId == inputAttributemodelId).FirstOrDefault().Formula = input;
                              Validate(input, lstCollection, lhs, ref error, ref values, inputAttributemodelId, entityMasterlist);
                          }
                          else
                          {
                              string outval = input;
                              values = input;
                          }
                      }

                  }
                  else
                  {
                      values = input;
                  }
              }


          }

          public void Validate1(List<EntityBehavior> lstCollection, string lhs, ref string error, ref string values, Guid inputAttributemodelId)
          {
              var tr = lstCollection.Where(t => t.AttributeModelId == inputAttributemodelId).FirstOrDefault();
              if (!object.ReferenceEquals(tr, null))
              {
                  string input = tr.Formula;
                  if (!string.IsNullOrEmpty(error))
                      return;
                  string temp = input;
                  if (temp.IndexOf('{') >= 0)
                  {
                      string id = temp.Substring(temp.IndexOf('{') + 1, 36);
                      var colTemp = lstCollection.Where(u => u.AttributeModelId.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                      if (lhs == id)
                      {
                          error = "Error";
                          return;
                      }
                      string replacevalue = "{" + id + "}";
                      if (!object.ReferenceEquals(colTemp, null))
                      {
                          if (colTemp.Formula.IndexOf('{') >= 0)
                          {
                              input = input.Replace(replacevalue, colTemp.Formula);
                              lstCollection.Where(u => u.AttributeModelId == inputAttributemodelId).FirstOrDefault().Formula = input;
                              Validate1(lstCollection, lhs, ref error, ref values, colTemp.AttributeModelId);
                          }
                          else if (input.IndexOf('{') >= 0)
                          {
                              input = input.Replace(replacevalue, colTemp.Formula);
                              lstCollection.Where(u => u.AttributeModelId == inputAttributemodelId).FirstOrDefault().Formula = input;
                              Validate1(lstCollection, lhs, ref error, ref values, inputAttributemodelId);
                          }
                          else
                          {
                              input = input.Replace(replacevalue, colTemp.Formula);
                              string outval = input;
                              values = input;

                              int result = 0;
                              EvaluateExpression.TestExpression(outval, out result);
                              lstCollection.Where(u => u.AttributeModelId == inputAttributemodelId).FirstOrDefault().Formula = input;

                          }
                      }
                      else
                      {
                          input = input.Replace(replacevalue, "1");
                          string outval = input;
                          values = input;
                      }

                  }
                  else
                  {
                      values = input;
                  }

              }




          }

          */

        #endregion

        #region private methods


        /// <summary>
        /// Select the AttributeModelBehavior
        /// </summary>
        /// <param name="attrubuteModelid"></param>
        /// <returns></returns>
        protected internal DataTable GetTableValues(Guid attrubuteModelid, Guid entityModelId, Guid entityId)
        {

            SqlCommand sqlCommand = new SqlCommand("EntityBehavior_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@AttributeModelId", attrubuteModelid);
            sqlCommand.Parameters.AddWithValue("@EntityId", entityId);
            sqlCommand.Parameters.AddWithValue("@EntityModelId", entityModelId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        #endregion

    }
}

