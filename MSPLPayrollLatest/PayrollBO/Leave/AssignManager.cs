using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class AssignManager : List<AssignManager>
    {
        public AssignManager()
        {


        }
        //public int Id { set; get; }
        public Guid Id { set; get; }
        public Guid EmployeeId { get; set; }

        public string empCode { get; set; }
        public Guid FinYear { get; set; }
        public string MgrEmpCode { get; set; }
        public string MgrEmpName { get; set; }
        public int CompanyId { get; set; }
        public int Loginid { get; set; }

        public int ApprovMust { get; set; }
        public string ApprovMustString { get; set; }
        public string AppCancelRightString { get; set; }
        public int MgrPriority { get; set; }
        public int AppCancelRights { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Email { get; set; }
        public Guid AssEmpId { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LevStatus { get; set; }
        public int ManagerPriority { get; set; }
        public Guid Existingmgrid { get; set; }
        public Guid Changemgrid { get; set; }

        //public AssignManager(int companyId)
        //{

        //    this.Id = Id;
        //    this.CompanyId = companyId;
        //    DataTable dtValue = this.GetTableValues(companyId);
        //    if (dtValue.Rows.Count > 0)
        //    {
        //        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
        //            this.Id = Convert.ToInt32(dtValue.Rows[0]["Id"]);
        //        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
        //            this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
        //        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Email"])))
        //            this.Email = Convert.ToString(dtValue.Rows[0]["Email"]);

        //        if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompanyId"])))
        //            this.CompanyId = Convert.ToInt32(dtValue.Rows[0]["CompanyId"]);
        //    }
        //}
        public AssignManager(Guid EmployeeId, int CompanyId,int temp,Guid finyear)
        {

            this.Id = Id;
            this.EmployeeId = EmployeeId;
            this.CompanyId = CompanyId;
           
            DataTable dtValue = this.GetTableValues(EmployeeId, CompanyId, finyear);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount1 = 0; rowcount1 < dtValue.Rows.Count; rowcount1++)
                {
                    AssignManager AssMgr1 = new AssignManager();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["EmployeeId"])))
                        AssMgr1.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount1]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["AssignEmployeeId"])))
                        AssMgr1.AssEmpId = new Guid(Convert.ToString(dtValue.Rows[rowcount1]["AssignEmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["Email"])))
                        AssMgr1.Email = Convert.ToString(dtValue.Rows[rowcount1]["Email"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["MgrPriority"])))
                        AssMgr1.MgrPriority = Convert.ToInt32(dtValue.Rows[rowcount1]["MgrPriority"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["CompanyId"])))
                        AssMgr1.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount1]["CompanyId"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["MgrEmpCode"])))
                        AssMgr1.MgrEmpCode = Convert.ToString(dtValue.Rows[rowcount1]["MgrEmpCode"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["MgrEmpName"])))
                        AssMgr1.MgrEmpName = Convert.ToString(dtValue.Rows[rowcount1]["MgrEmpName"]);
                    this.Add(AssMgr1);
                }
            }
        }


        public AssignManager(Guid LevReqId, Guid empid, int temp)
        {


            DataTable dtValue = this.checkingmailalreadyResponced(LevReqId, empid);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount1 = 0; rowcount1 < dtValue.Rows.Count; rowcount1++)
                {
                    AssignManager AssMgrPeLevStat = new AssignManager();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["LeaveStatus"])))
                        AssMgrPeLevStat.LevStatus = Convert.ToString(dtValue.Rows[rowcount1]["LeaveStatus"]);

                    this.Add(AssMgrPeLevStat);
                }
            }
        }

        public AssignManager(Guid LevReqId,int companyid,Guid FinYear)
        {

            
            DataTable dtValue = this.GetPendingLevStat(LevReqId);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount1 = 0; rowcount1 < dtValue.Rows.Count; rowcount1++)
                {
                    AssignManager AssMgrPeLevStat = new AssignManager();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["ManagerCode"])))
                        AssMgrPeLevStat.MgrEmpCode = (Convert.ToString(dtValue.Rows[rowcount1]["ManagerCode"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["ManagerName"])))
                        AssMgrPeLevStat.MgrEmpName = (Convert.ToString(dtValue.Rows[rowcount1]["ManagerName"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["PriorityNumber"])))
                        AssMgrPeLevStat.MgrPriority =Convert.ToInt32(dtValue.Rows[rowcount1]["PriorityNumber"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["LeaveStatus"])))
                        AssMgrPeLevStat.LevStatus = Convert.ToString(dtValue.Rows[rowcount1]["LeaveStatus"]);
                    
                    this.Add(AssMgrPeLevStat);
                }
            }
        }
        public AssignManager(Guid EmployeeId,int companyid)
        {
            DataTable dtValue = this.GetTableValues(EmployeeId,companyid);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount1 = 0; rowcount1 < dtValue.Rows.Count; rowcount1++)
                {
                    AssignManager AssMgr2 = new AssignManager();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount1]["Id"])))
                        AssMgr2.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount1]["Id"]));
                    AssMgr2.FirstName = Convert.ToString(dtValue.Rows[rowcount1]["FirstName"]);
                    AssMgr2.EmployeeCode = Convert.ToString(dtValue.Rows[rowcount1]["EmployeeCode"]);
                    AssMgr2.ManagerPriority = Convert.ToInt32(dtValue.Rows[rowcount1]["ManagerPriority"]);
                    this.Add(AssMgr2);
                }
            }
        }

        public AssignManager(Guid EmployeeId, int CompanyId, Guid FinYear, Guid id, int appmust)
        {
            DataTable dtValue = this.GetTableValues(EmployeeId, CompanyId, FinYear, id, appmust);
            if (dtValue.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < dtValue.Rows.Count; rowcount++)
                {
                    AssignManager AssMgr = new AssignManager();
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["Id"])))
                        AssMgr.Id = new Guid(Convert.ToString(dtValue.Rows[rowcount]["Id"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"])))
                        AssMgr.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["EmployeeId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["FinYear"])))
                        AssMgr.FinYear = new Guid(Convert.ToString(dtValue.Rows[rowcount]["FinYear"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AssignManagerId"])))
                        AssMgr.AssEmpId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AssignManagerId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ManagerCode"])))
                        AssMgr.MgrEmpCode = (Convert.ToString(dtValue.Rows[rowcount]["ManagerCode"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ManagerName"])))
                        AssMgr.MgrEmpName = (Convert.ToString(dtValue.Rows[rowcount]["ManagerName"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApprovMustString"])))
                        AssMgr.ApprovMustString = (Convert.ToString(dtValue.Rows[rowcount]["ApprovMustString"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AppCancelRightsString"])))
                        AssMgr.AppCancelRightString = (Convert.ToString(dtValue.Rows[rowcount]["AppCancelRightsString"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AssignManagerId"])))
                        AssMgr.AssEmpId = new Guid(Convert.ToString(dtValue.Rows[rowcount]["AssignManagerId"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ApprovalMust"])))
                        AssMgr.ApprovMust = (Convert.ToInt32(dtValue.Rows[rowcount]["ApprovalMust"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["AppCancelRights"])))
                        AssMgr.AppCancelRights = (Convert.ToInt32(dtValue.Rows[rowcount]["AppCancelRights"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["ManagerPriority"])))
                        AssMgr.MgrPriority = (Convert.ToInt32(dtValue.Rows[rowcount]["ManagerPriority"]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[rowcount]["CompanyId"])))
                        AssMgr.CompanyId = Convert.ToInt32(dtValue.Rows[rowcount]["CompanyId"]);
                    this.Add(AssMgr);
                }

            }
        }

        public bool SaveAssignMgrData()
        {

            SqlCommand sqlCommand = new SqlCommand("AssignMang_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@FinYear", this.FinYear);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@AssignManagerId", this.AssEmpId);
            sqlCommand.Parameters.AddWithValue("@ApprovalMust", this.ApprovMust);
            sqlCommand.Parameters.AddWithValue("@ManagerPriority", this.MgrPriority);
            sqlCommand.Parameters.AddWithValue("@AppCancelRights", this.AppCancelRights);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.EmployeeId = new Guid(Convert.ToString(outValue));
            }
            return status;
        }
        public bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("LeaveAssignManager_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@Email", this.Email);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@AssignEmployeeId", this.AssEmpId);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.EmployeeId = new Guid(Convert.ToString(outValue));
            }
            return status;
        }

        public bool SaveAssignmanager(Guid managerid, int appstatus, int priority, int appcancelstat)
        {

            SqlCommand sqlCommand = new SqlCommand("LeaveAssignManager_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@Email", this.Email);
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@AssignEmployeeId", this.AssEmpId);
            sqlCommand.Parameters.Add("@IdOut", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.SaveData(sqlCommand, out outValue, "@IdOut");
            if (status)
            {
                this.EmployeeId = new Guid(Convert.ToString(outValue));
            }
            return status;
        }

       
        internal DataTable GetTableValues(Guid EmployeeId, int CompanyId,Guid finyr)
        {
            SqlCommand sqlCommand = new SqlCommand("LeaveAssignManager_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", finyr);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            //sqlCommand.Parameters.AddWithValue("@Email", this.Email);
            //sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        internal DataTable checkingmailalreadyResponced(Guid LevReqId,Guid employeeid)
        {
            SqlCommand sqlCommand = new SqlCommand("AlreadyApprovedbymail_check");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@LevReqId", LevReqId);
            sqlCommand.Parameters.AddWithValue("@employeeid", employeeid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        internal DataTable GetPendingLevStat(Guid LevReqId)
        {
            SqlCommand sqlCommand = new SqlCommand("PendingLeaveStatus_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", LevReqId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        internal DataTable GetTableValues(Guid EmployeeId, int CompanyId, Guid FinYear, Guid id, int appmst)
        {
            SqlCommand sqlCommand = new SqlCommand("AssignMgrML_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", FinYear);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeId);
            sqlCommand.Parameters.AddWithValue("@ApprovMust", appmst);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        public DataTable GetAssignManager(int CompanyId)
        {
            SqlCommand sqlCommand = new SqlCommand("LeaveAssignManager_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            // sqlCommand.Parameters.AddWithValue("@FinanceYear", this.FinanceYearId);
            //sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            //sqlCommand.Parameters.AddWithValue("@Email", this.Email);
            //sqlCommand.Parameters.AddWithValue("@CompanyId", this.CompanyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public DataTable EmployeeUnderAssignManager(Guid ManagerId ,int CompanyId , Guid FinYear)
        {
            SqlCommand sqlCommand = new SqlCommand("EmployeeUnderAssignManager_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@ManagerId", ManagerId);
            sqlCommand.Parameters.AddWithValue("@FinYearId", FinYear);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public DataTable GetTableValues(Guid EmployeeId,int companyid)
        {
            SqlCommand sqlCommand = new SqlCommand("AssignedUser_select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", EmployeeId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", companyid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        public bool Delete()
        {
            SqlCommand sqlCommand = new SqlCommand("AssignMgrML_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }

        public bool SaveChangeManager()
        {
            SqlCommand sqlCommand = new SqlCommand("TblChangemanager_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", Guid.Empty);
            sqlCommand.Parameters.AddWithValue("@Companyid", this.CompanyId);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.Loginid);
            sqlCommand.Parameters.AddWithValue("@Existingmgrid", this.Existingmgrid);
            sqlCommand.Parameters.AddWithValue("@Changemgrid", this.Changemgrid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.save(sqlCommand);
        }

        public bool SaveApprovedcancelleavebyHR(DateTime levdate,Guid Employeeid,Guid levtypeid,int compid,string loginid,Guid levreqid,string cancreson)
        {
            SqlCommand sqlCommand = new SqlCommand("HRApprovedCancel_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@levreqid", levreqid);
            sqlCommand.Parameters.AddWithValue("@Logedonid", loginid);
            sqlCommand.Parameters.AddWithValue("@Companyid", compid);
            sqlCommand.Parameters.AddWithValue("@levtype", levtypeid);
            sqlCommand.Parameters.AddWithValue("@levdate", levdate);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", Employeeid);
            sqlCommand.Parameters.AddWithValue("@Canreson", cancreson);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.save(sqlCommand);
        }
    }
}
//Existingmgrid Changemgrid