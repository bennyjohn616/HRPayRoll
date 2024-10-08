using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollBO;
using SQLDBOperation;

namespace PayRollReports
{
    public class PaySlip
    {
        #region private variable


        #endregion

        #region construstor

        /// <summary>
        /// initialize the object
        /// </summary>
        public PaySlip()
        {

        }
        public PaySlip(string TableName)
        {
            DataTable dtValue = PaySlip.GetTableValues(TableName);
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Table_Name"])))
                    this.Table_Name = Convert.ToString(dtValue.Rows[0]["Table_Name"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Column_Name"])))
                    this.Column_Name = Convert.ToString(dtValue.Rows[0]["Column_Name"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Data_Type"])))
                    this.Data_Type = Convert.ToString(dtValue.Rows[0]["Data_Type"]);
            }
        }
        #endregion

        #region property
        /// <summary>
        /// Get or Set the Table_Name
        /// </summary>
        public int Id { get; set; }

        public Guid CompanyId { get; set; }

        public Guid CategoryId { get; set; }

        public string Logo { get; set; }

        public string Title { get; set; }

        public string string1 { get; set; }
        public string string2 { get; set; }

        public string string3 { get; set; }

        public bool IsCumulative { get; set; }

        public string CumulativeMonth { get; set; }
        public string Table_Name { get; set; }

        public string Column_Name { get; set; }

        public string Data_Type { get; set; }

        #endregion
        #region Public methods
        public bool Save()
        {
            return true;
        }
        public bool Delete()
        {
            return true;
        }

        #endregion
        #region private methods


        /// <summary>
        /// Select the FullFinalSettlementDetail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        protected static internal DataTable GetTableValues(string tblNames)
        {
            string query = "SELECT case when Table_Name='Bank' then 'Emp_Bank' else TABLE_NAME end as Table_Name,Column_Name,Data_Type FROM INFORMATION_SCHEMA.COLUMNS WHERE "
            + " Lower(Column_Name) NOT IN('createdon', 'createdby', 'modifiedon', 'modifiedby', 'isactive', 'isdeleted', 'id') "
            + " AND Lower(Column_NAME)NOT LIKE '%id'"
            + " AND Lower(Table_Name) IN(" + tblNames + ")"
            + " union select 'Category' as Table_Name,'Category' as Column_Name, 'Guid'Data_Type "
            + " union select 'Emp_Bank' as Table_Name, 'BankId' as Column_Name, 'Guid'Data_Type ";
            SqlCommand sqlCommand = new SqlCommand("USP_EXECQUERY");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@QUERY", query);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public static List<PaySlipAttributes> GetMasterFields(String TableName, int companyId)
        {

            Payattr paySlip = new Payattr();
            List<PaySlipAttributes> result = new List<PaySlipAttributes>();
            DataTable dtValue = GetTableValues(TableName);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    PaySlipAttributes paySlipTemp = new PaySlipAttributes();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Table_Name"])))
                        paySlipTemp.TableName = Convert.ToString(dtValue.Rows[rowcount]["Table_Name"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Column_Name"])))
                        paySlipTemp.FieldName = Convert.ToString(dtValue.Rows[rowcount]["Column_Name"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Data_Type"])))
                        paySlipTemp.Data_Type = Convert.ToString(dtValue.Rows[rowcount]["Data_Type"]);
                    result.Add(paySlipTemp);
                }

            }
            EntityModel entityModel = new EntityModel("AdditionalInfo", companyId);
            EntityAttributeModelList attributemodalList = new EntityAttributeModelList(entityModel.Id);
            attributemodalList.ForEach(attr =>
            {
                result.Add(new PaySlipAttributes
                {
                    TableName = "AdditionalInfo",
                    FieldName = attr.AttributeModel.Name,
                    DisplayAs = attr.AttributeModel.Name,
                    AttributeId= attr.AttributeModelId
                });
            });
            return result;
        }

        public DataTable GetSetting(string Categories)
        {
            string query = "SELECT  DISTINCT CofigurationId, FieldName  FROM PaySlipAttributes WHERE TableName='Category' AND "
            + " FieldName in (" + Categories + ")";
            SqlCommand sqlCommand = new SqlCommand("USP_EXECQUERY");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@QUERY", query);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }



        public Guid ConfigurationId { get; set; }



        #endregion
    }
    public class Payattr
    {
        public Guid ConfigurationId { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string DisplayAs { get; set; }

        public string Value1 { get; set; }
        public string Value2 { get; set; }

        public string Section { get; set; }
        public int DisplayOrder { get; set; }
        public Guid MatchingId { get; set; }

    }
}
