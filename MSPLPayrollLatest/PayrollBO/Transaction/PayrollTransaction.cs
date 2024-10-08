using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PayrollBO
{
    public class PayrollTransaction
    {
        private Employee _empDetails = new Employee();
        public PayrollTransaction()
        {
            EmployeeDetails = new Employee();
            
        }
        public PayrollTransaction(Guid PayrollHistoryId)
        {

        }

        public PayrollTransaction(Guid EmployeeId, DateTime EffectiveDate)
        {

        }
   

        public Guid PayrollHistoryId { get; set; }
        public Guid EmployeeId { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string Value { get; set; }

        public Employee EmployeeDetails { get; set; }
      

        #region "public Methods"
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("PayrollTransaction_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@PayrollHistoryId", this.PayrollHistoryId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@TableName", this.TableName);
            sqlCommand.Parameters.AddWithValue("@ColumnName", this.ColumnName);
            sqlCommand.Parameters.AddWithValue("@Value", this.Value);

            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue);

            return status;
        }
        public DataTable GetEmpMasterTransaction()
        {

            SqlCommand sqlCommand = new SqlCommand("PayrollTransaction_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@PayrollHistoryId", this.PayrollHistoryId);
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            return dbOperation.GetTableData(sqlCommand);


        }

        public bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("PayrollTransaction_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@PayrollHistoryId", this.PayrollHistoryId);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);

            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.DeleteData(sqlCommand, out outValue);

            return status;
        }



        public static bool InertPayrollTransaction(List<PayrollTransaction> empPayList, int companyId)
        {

            bool straus = true;
            AttributeModelList attributeList = new AttributeModelList(companyId);

            var transMasterAttr = attributeList.Where(a => a.IsTransaction && a.BehaviorType == "Master").ToList();  //****** Yet to add Master Components******//
            FormCommandList frmcmds = new FormCommandList(true);
            List<FormCommand> transAttr = frmcmds.Where(f => f.IsDefaultTransaction).ToList();
            List<PayrollTransaction> paytranslist = new List<PayrollTransaction>();
           
            empPayList.Where(p => p.PayrollHistoryId != Guid.Empty).ToList().ForEach(e =>
                {
                    transAttr.ForEach(t =>
                    {
                        PayrollTransaction payTrans = new PayrollTransaction();
                        payTrans.PayrollHistoryId = e.PayrollHistoryId;
                        payTrans.EmployeeId = e.EmployeeId;
                        payTrans.ColumnName = t.ColumnName;
                        payTrans.TableName = t.TableName == null ? string.Empty : t.TableName;

                        switch (t.TableName.ToLower())
                        {
                            case "employee":
                                payTrans.Value = Convert.ToString(e.EmployeeDetails.GetType().GetProperty(t.ColumnName).GetValue(e.EmployeeDetails, null));
                                break;
                            case "emp_personal":
                                
                                payTrans.Value = Convert.ToString(e.EmployeeDetails.EmployeePersonal.GetType().GetProperty(t.ColumnName).GetValue(e.EmployeeDetails.EmployeePersonal, null));
                                break;
                            case "emp_bank":
                                if (e.EmployeeDetails.EmployeeBankList.Count > 0)
                                    payTrans.Value = Convert.ToString(e.EmployeeDetails.EmployeeBankList[0].GetType().GetProperty(t.ColumnName).GetValue(e.EmployeeDetails.EmployeeBankList[0], null));
                                else
                                    payTrans.Value = "";
                                break;
                            case "emp_address":
                                EmployeeAddress empAddress = e.EmployeeDetails.EmployeeAddressList.FirstOrDefault();
                                if (empAddress != null)
                                {
                                    payTrans.Value = Convert.ToString(empAddress.GetType().GetProperty(t.ColumnName).GetValue(empAddress, null));
                                }
                                break;

                        }
                        payTrans.Save();
                        paytranslist.Add(payTrans);
                    });
                    EntityAdditionalInfoList empAdditionalInfo = new EntityAdditionalInfoList(companyId, Guid.Empty, e.EmployeeId);
                    EntityMappingList entMapList = new EntityMappingList(e.EmployeeId);
                    transMasterAttr.ForEach(m =>
                    {
                        PayrollTransaction payTrans = new PayrollTransaction();
                        payTrans.PayrollHistoryId = e.PayrollHistoryId;
                        payTrans.EmployeeId = e.EmployeeId;
                        payTrans.ColumnName = m.Id.ToString();
                        payTrans.TableName = "AdditionalInfo";

                        if (empAdditionalInfo.Where(a => a.AttributeModelId == m.Id).FirstOrDefault() != null)
                        {
                            payTrans.Value = empAdditionalInfo.Where(a => a.AttributeModelId == m.Id).FirstOrDefault().Value;
                        }

                        payTrans.Save();
                        paytranslist.Add(payTrans);

                    });
                    entMapList.ForEach(M =>
                    {

                        PayrollTransaction payTrans = new PayrollTransaction();
                        payTrans.PayrollHistoryId = e.PayrollHistoryId;
                        payTrans.EmployeeId = e.EmployeeId;
                        payTrans.ColumnName = M.Id.ToString();
                        payTrans.TableName = "EntityMapping";

                        if (entMapList.Where(a => a.RefEntityId == e.EmployeeId.ToString()).FirstOrDefault() != null)
                        {
                            payTrans.Value = entMapList.Where(a => a.RefEntityId == e.EmployeeId.ToString()).FirstOrDefault().RefEntityModelId;

                        }
                        payTrans.Save();
                        paytranslist.Add(payTrans);


                    });

                });


            //StringWriter stringWriter = new StringWriter();
            //XmlDocument xmlDoc = new XmlDocument();

            //XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);

            //Type[] frm = new Type[1];
            //frm[0] = typeof(Employee);

            //XmlSerializer serializer = new XmlSerializer(typeof(List<PayrollTransaction>), frm);

            //serializer.Serialize(xmlWriter, paytranslist);

            //string xmlResult = stringWriter.ToString();



            //SqlCommand sqlCommand = new SqlCommand("sp_XmlSave_PayrollEmpTransaction");
            //sqlCommand.CommandType = CommandType.StoredProcedure;
            //sqlCommand.Parameters.AddWithValue("@xmlstring",xmlResult);
            //DBOperation dbOperation = new DBOperation();
            //string status = string.Empty;
            //return dbOperation.SaveData(sqlCommand, out status);

            return straus;
        }

        public static object GetEmployeeTrasaction(string className, dynamic ClassObject, Guid payrollHistoryId,BankList banklist,Emp_BankList empbanklist,EmployeeAddressList empaddrlist,Emp_PersonalList emppersonallist)
        {


            switch (className)
            {
                case "Employee":
                    Employee emp = (Employee)ClassObject;
                    PayrollTransaction getTrans = new PayrollTransaction();
                    getTrans.EmployeeId = emp.Id;
                    getTrans.PayrollHistoryId = payrollHistoryId;
                    DataTable dtTrans = getTrans.GetEmpMasterTransaction();
                    for (int i = 0; i < dtTrans.Rows.Count; i++)
                    {
                        if (dtTrans.Rows[i]["ColumnName"] != null && Convert.ToString(dtTrans.Rows[i]["ColumnName"]).ToUpper() != "NULL")
                        {
                            switch (Convert.ToString(dtTrans.Rows[i]["TableName"]).ToLower())
                            {
                                case "employee":
                                    switch (emp.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).PropertyType.Name.ToLower())
                                    {
                                        case "guid":
                                            emp.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp, new Guid(Convert.ToString(dtTrans.Rows[i]["Value"])), null);
                                            break;
                                        case "decimal":
                                            emp.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp, Convert.ToDecimal(dtTrans.Rows[i]["Value"]), null);
                                            break;
                                        case "int32":
                                            emp.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp, Convert.ToInt32(dtTrans.Rows[i]["Value"]), null);
                                            break;
                                        case "int16":
                                            emp.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp, Convert.ToInt16(dtTrans.Rows[i]["Value"]), null);
                                            break;
                                        case "string":
                                            emp.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp, Convert.ToString(dtTrans.Rows[i]["Value"]), null);

                                            break;
                                    }
                                    break;
                                case "emp_personal":

                                    switch (emp.EmployeePersonal.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).PropertyType.Name.ToLower())
                                    {
                                        case "guid":
                                            emp.EmployeePersonal.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeePersonal, new Guid(Convert.ToString(dtTrans.Rows[i]["Value"])), null);
                                            break;
                                        case "decimal":
                                            emp.EmployeePersonal.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeePersonal, Convert.ToDecimal(dtTrans.Rows[i]["Value"]), null);
                                            break;
                                        case "int32":
                                            emp.EmployeePersonal.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeePersonal, Convert.ToInt32(dtTrans.Rows[i]["Value"]), null);
                                            break;
                                        case "int16":
                                            emp.EmployeePersonal.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeePersonal, Convert.ToInt16(dtTrans.Rows[i]["Value"]), null);
                                            break;
                                        case "string":
                                            emp.EmployeePersonal.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeePersonal, Convert.ToString(dtTrans.Rows[i]["Value"]), null);

                                            break;
                                    }
                                    break;
                                case "emp_address":
                                    var empAddress = empaddrlist.Where(eal => eal.EmployeeId == emp.Id).FirstOrDefault();
                                   if (empAddress != null)
                                   {


                                        switch (emp.EmployeeAddressList.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).PropertyType.Name.ToLower())
                                        {
                                            case "guid":
                                                emp.EmployeeAddressList.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeeAddressList, new Guid(Convert.ToString(dtTrans.Rows[i]["Value"])), null);
                                                break;
                                            case "decimal":
                                                emp.EmployeeAddressList.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeeAddressList, Convert.ToDecimal(dtTrans.Rows[i]["Value"]), null);
                                                break;
                                            case "int32":
                                                emp.EmployeeAddressList.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeeAddressList, Convert.ToInt32(dtTrans.Rows[i]["Value"]), null);
                                                break;
                                            case "int16":
                                                emp.EmployeeAddressList.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeeAddressList, Convert.ToInt16(dtTrans.Rows[i]["Value"]), null);
                                                break;

                                            case "string":
                                                emp.EmployeeAddressList.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeeAddressList, Convert.ToString(dtTrans.Rows[i]["Value"]), null);

                                                break;
                                        }
                                    }
                                    break;
                                case "emp_bank":
                                    var empbank = empbanklist.Where(ebl => ebl.EmployeeId == emp.Id).FirstOrDefault();
                                    if (ReferenceEquals(emp.EmployeeBankList, null))
                                    {
                                        emp.EmployeeBankList.Add(empbanklist.Where(ebl => ebl.EmployeeId == emp.Id).FirstOrDefault());
                                    }

                                    if (empbank != null)
                                    {
                                        switch (empbank.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).PropertyType.Name.ToLower())
                                        {
                                            case "guid":
                                                emp.EmployeeBankList.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeeBankList, new Guid(Convert.ToString(dtTrans.Rows[i]["Value"])), null);
                                                break;
                                            case "decimal":
                                                emp.EmployeeBankList.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeeBankList, Convert.ToDecimal(dtTrans.Rows[i]["Value"]), null);
                                                break;
                                            case "int32":
                                                emp.EmployeeBankList.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeeBankList, Convert.ToInt32(dtTrans.Rows[i]["Value"]), null);
                                                break;
                                            case "int16":
                                                emp.EmployeeBankList.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeeBankList, Convert.ToInt16(dtTrans.Rows[i]["Value"]), null);
                                                break;
                                            case "string":
                                                emp.EmployeeBankList.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeeBankList, Convert.ToString(dtTrans.Rows[i]["Value"]), null);

                                                break;
                                        }

                                        emp.EmployeePersonal.BankAccountNo = emp.EmployeeBankList.AcctNo;

                                        Bank bank = banklist.Where(bl=>bl.Id == emp.EmployeeBankList.BankId && bl.CompanyId == emp.CompanyId).FirstOrDefault();
                                        if (!object.ReferenceEquals(bank, null))
                                        {
                                            emp.EmployeePersonal.Bank = bank.BankName;
                                        }


                                    }
                                    break;
                            }
                        }
                    }
                    return emp;

            }
            return ClassObject;


        }


        public static object GetEmployeeTrasaction(string className, string transactionDate, dynamic ClassObject, Guid payrollHistoryId)
        {


            switch (className)
            {
                case "Employee":
                    Employee emp = (Employee)ClassObject;
                    PayrollTransaction getTrans = new PayrollTransaction();
                    getTrans.EmployeeId = emp.Id;
                    getTrans.PayrollHistoryId = payrollHistoryId;
                    DataTable dtTrans = getTrans.GetEmpMasterTransaction();
                    for (int i = 0; i < dtTrans.Rows.Count; i++)
                    {
                        if (dtTrans.Rows[i]["ColumnName"] != null && Convert.ToString(dtTrans.Rows[i]["ColumnName"]).ToUpper() != "NULL")
                        {
                            switch (Convert.ToString(dtTrans.Rows[i]["TableName"]).ToLower())
                            {
                                case "employee":
                                    switch (emp.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).PropertyType.Name.ToLower())
                                    {
                                        case "guid":
                                            emp.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp, new Guid(Convert.ToString(dtTrans.Rows[i]["Value"])), null);
                                            break;
                                        case "decimal":
                                            emp.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp, Convert.ToDecimal(dtTrans.Rows[i]["Value"]), null);
                                            break;
                                        case "int32":
                                            emp.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp, Convert.ToInt32(dtTrans.Rows[i]["Value"]), null);
                                            break;
                                        case "int16":
                                            emp.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp, Convert.ToInt16(dtTrans.Rows[i]["Value"]), null);
                                            break;
                                        case "string":
                                            emp.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp, Convert.ToString(dtTrans.Rows[i]["Value"]), null);

                                            break;
                                    }
                                    break;
                                case "emp_personal":

                                    switch (emp.EmployeePersonal.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).PropertyType.Name.ToLower())
                                    {
                                        case "guid":
                                            emp.EmployeePersonal.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeePersonal, new Guid(Convert.ToString(dtTrans.Rows[i]["Value"])), null);
                                            break;
                                        case "decimal":
                                            emp.EmployeePersonal.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeePersonal, Convert.ToDecimal(dtTrans.Rows[i]["Value"]), null);
                                            break;
                                        case "int32":
                                            emp.EmployeePersonal.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeePersonal, Convert.ToInt32(dtTrans.Rows[i]["Value"]), null);
                                            break;
                                        case "int16":
                                            emp.EmployeePersonal.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeePersonal, Convert.ToInt16(dtTrans.Rows[i]["Value"]), null);
                                            break;
                                        case "string":
                                            emp.EmployeePersonal.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(emp.EmployeePersonal, Convert.ToString(dtTrans.Rows[i]["Value"]), null);

                                            break;
                                    }
                                    break;
                                case "emp_address":
                                    EmployeeAddress empAddress = emp.EmployeeAddressList.FirstOrDefault();
                                    if (empAddress != null)
                                    {


                                        switch (empAddress.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).PropertyType.Name.ToLower())
                                        {
                                            case "guid":
                                                empAddress.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(empAddress, new Guid(Convert.ToString(dtTrans.Rows[i]["Value"])), null);
                                                break;
                                            case "decimal":
                                                empAddress.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(empAddress, Convert.ToDecimal(dtTrans.Rows[i]["Value"]), null);
                                                break;
                                            case "int32":
                                                empAddress.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(empAddress, Convert.ToInt32(dtTrans.Rows[i]["Value"]), null);
                                                break;
                                            case "int16":
                                                empAddress.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(empAddress, Convert.ToInt16(dtTrans.Rows[i]["Value"]), null);
                                                break;

                                            case "string":
                                                empAddress.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(empAddress, Convert.ToString(dtTrans.Rows[i]["Value"]), null);

                                                break;
                                        }
                                    }
                                    break;
                                case "emp_bank":
                                    Emp_Bank empbank = emp.EmployeeBankList.FirstOrDefault();
                                    if (empbank != null)
                                    {
                                        switch (empbank.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).PropertyType.Name.ToLower())
                                        {
                                            case "guid":
                                                empbank.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(empbank, new Guid(Convert.ToString(dtTrans.Rows[i]["Value"])), null);
                                                break;
                                            case "decimal":
                                                empbank.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(empbank, Convert.ToDecimal(dtTrans.Rows[i]["Value"]), null);
                                                break;
                                            case "int32":
                                                empbank.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(empbank, Convert.ToInt32(dtTrans.Rows[i]["Value"]), null);
                                                break;
                                            case "int16":
                                                empbank.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(empbank, Convert.ToInt16(dtTrans.Rows[i]["Value"]), null);
                                                break;
                                            case "string":
                                                empbank.GetType().GetProperty(Convert.ToString(dtTrans.Rows[i]["ColumnName"])).SetValue(empbank, Convert.ToString(dtTrans.Rows[i]["Value"]), null);

                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    return emp;
               
            }
            return ClassObject;


        }
        #endregion

    }
}
