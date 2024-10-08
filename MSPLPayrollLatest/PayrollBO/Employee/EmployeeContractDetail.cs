using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class EmployeeContractDetail
    {
        #region "Constructor"
        public EmployeeContractDetail()
        {

        }
        #endregion
        #region "Properties"
        public Guid Id { get; set; }
        public Guid EmpId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        #endregion
        #region "Method"
        public List<EmployeeContractDetail> GetEmployeeContractDetail(Guid empid)
        {
            List<EmployeeContractDetail> objEmpContrLst = new List<EmployeeContractDetail>();
            EmployeeContractDetail objEmpContr = new EmployeeContractDetail();
            DataTable dtVal = objEmpContr.GetTableValue(empid);
            if (dtVal.Rows.Count > 0)
            {
                for(int i = 0; i < dtVal.Rows.Count; i++)
                {
                    EmployeeContractDetail obj = new EmployeeContractDetail();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtVal.Rows[i][""])))
                        obj.Id = new Guid(dtVal.Rows[i][""].ToString());
                    if (!string.IsNullOrEmpty(Convert.ToString(dtVal.Rows[i][""])))
                        obj.EmpId = new Guid(dtVal.Rows[i][""].ToString());
                    if (!string.IsNullOrEmpty(Convert.ToString(dtVal.Rows[i][""])))
                        obj.StartDate = Convert.ToDateTime(dtVal.Rows[i][""].ToString());
                    if (!string.IsNullOrEmpty(Convert.ToString(dtVal.Rows[i][""])))
                        obj.EndDate = Convert.ToDateTime(dtVal.Rows[i][""].ToString());
                    if (!string.IsNullOrEmpty(Convert.ToString(dtVal.Rows[i][""])))
                        obj.Remarks = dtVal.Rows[i][""].ToString();
                    objEmpContrLst.Add(obj);
                }
            }
            return objEmpContrLst;

        }
        public DataTable GetTableValue(Guid empid)
        {
            SqlCommand objSqlCmd = new SqlCommand("Employee_ContractSelect");
            objSqlCmd.CommandType = CommandType.StoredProcedure;
            objSqlCmd.Parameters.AddWithValue("@Id", Guid.Empty);
            objSqlCmd.Parameters.AddWithValue("@EmpId", empid);
            SQLDBOperation.DBOperation objDbConnec = new SQLDBOperation.DBOperation();
            return objDbConnec.GetTableData(objSqlCmd);


        }
        #endregion
    }
}
