using PayrollBO.Leave;
using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PayrollBO
{
    public class LeaveRequest : LeaveBase
    {
        #region "Constructor"
        public LeaveRequest()
        {
            this.FinanceYear = this.CurentFinanceYear.Id;

        }







        public LeaveRequest(Guid id)
        {
            this.Id = id;
            this.EmployeeId = Guid.Empty;
            DataTable dtValue = this.GetTableValues();
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FinanceYear"])))
                    this.FinanceYear = new Guid(Convert.ToString(dtValue.Rows[0]["FinanceYear"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LeaveType"])))
                    this.LeaveType = new Guid(Convert.ToString(dtValue.Rows[0]["LeaveType"]));
                this.Reason = (Convert.ToString(dtValue.Rows[0]["Reason"]));
                this.Contact = (Convert.ToString(dtValue.Rows[0]["Contact"]));
                this.NoOfDays = (Convert.ToString(dtValue.Rows[0]["NoOfDays"]));
                this.FirstLvlContact = (Convert.ToString(dtValue.Rows[0]["FirstLvlContact"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FromDay"])))
                    this.FromDay = Convert.ToInt32(dtValue.Rows[0]["FromDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ToDay"])))
                    this.ToDay = Convert.ToInt32(dtValue.Rows[0]["ToDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FromDate"])))
                    this.FromDate = Convert.ToDateTime(dtValue.Rows[0]["FromDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EndDate"])))
                    this.EndDate = (Convert.ToDateTime(dtValue.Rows[0]["EndDate"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Status"])))
                    this.Status = Convert.ToInt32(dtValue.Rows[0]["Status"]);
                //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                //    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                // if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                //   this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                    this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeCode"])))
                    this.Empcode = Convert.ToString(dtValue.Rows[0]["EmployeeCode"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Leave"])))
                    this.LeaveTypeName = Convert.ToString(dtValue.Rows[0]["Leave"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompOff"])))
                    this.CompOff = Convert.ToBoolean(dtValue.Rows[0]["CompOff"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["AttachmentPath"])))
                    this.Imgpath = Convert.ToString(dtValue.Rows[0]["AttachmentPath"]);
            }

        }

        //------------------Geting Debitdays for showing levbalance in request page------------------------------

        public LeaveRequest(Guid Leavetypeid, Guid Employeeid, int companyid, Guid finyrid, int temp)
        {
            this.LeaveType = Leavetypeid;
            this.EmployeeId = Employeeid;
            this.CompanyId = companyid;
            this.FinanceYear = finyrid;
            DataTable dtValue = this.Getavailabledebitdays();
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["NoOfDays"])))
                    this.Debitdays = Convert.ToDouble(dtValue.Rows[0]["NoOfDays"]);
                //this.LeaveBalance = this.LeaveBalance - Debitdays;
            }

        }
        //------------------------------------------------
        //--------------Geting available balance for showing levbalance in request page-------------------------------

        public LeaveRequest(Guid Leavetypeid, Guid Employeeid, int companyid, Guid finyrid)
        {
            this.LeaveType = Leavetypeid;
            this.EmployeeId = Employeeid;
            this.CompanyId = companyid;
            this.FinanceYear = finyrid;
            DataTable dtValue = this.Getleavebalance();
            if (dtValue.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Used Days"])))
                    this.Useddays = Convert.ToDouble(dtValue.Rows[0]["Used Days"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["TotalDays"])))
                    this.Totaldays = Convert.ToDouble(dtValue.Rows[0]["TotalDays"]);

                this.LeaveBalance = Totaldays - Useddays;
            }

        }



















        //---------------------------------------------









        public LeaveRequest(Guid id, Guid employeeId)
        {

            DataTable dtValue = this.GetTableValues(id, employeeId);
            if (dtValue.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Id"])))
                    this.Id = new Guid(Convert.ToString(dtValue.Rows[0]["Id"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FinanceYear"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["FinanceYear"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeId"])))
                    this.EmployeeId = new Guid(Convert.ToString(dtValue.Rows[0]["EmployeeId"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["LeaveType"])))
                    this.LeaveType = new Guid(Convert.ToString(dtValue.Rows[0]["LeaveType"]));
                this.Reason = (Convert.ToString(dtValue.Rows[0]["Reason"]));
                this.Contact = (Convert.ToString(dtValue.Rows[0]["Contact"]));
                this.NoOfDays = (Convert.ToString(dtValue.Rows[0]["NoOfDays"]));
                this.FirstLvlContact = (Convert.ToString(dtValue.Rows[0]["FirstLvlContact"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FromDay"])))
                    this.FromDay = Convert.ToInt32(dtValue.Rows[0]["FromDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ToDay"])))
                    this.ToDay = Convert.ToInt32(dtValue.Rows[0]["ToDay"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["FromDate"])))
                    this.FromDate = Convert.ToDateTime(dtValue.Rows[0]["FromDate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EndDate"])))
                    this.EndDate = (Convert.ToDateTime(dtValue.Rows[0]["EndDate"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["Status"])))
                    this.Status = Convert.ToInt32(dtValue.Rows[0]["Status"]);
                //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedOn"])))
                //    this.CreatedOn = Convert.ToDateTime(dtValue.Rows[0]["CreatedOn"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CreatedBy"])))
                    this.CreatedBy = Convert.ToInt32(dtValue.Rows[0]["CreatedBy"]);
                // if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedOn"])))
                //   this.ModifiedOn = Convert.ToDateTime(dtValue.Rows[0]["ModifiedOn"]);
                //if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["ModifiedBy"])))
                //this.ModifiedBy = Convert.ToInt32(dtValue.Rows[0]["ModifiedBy"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsDeleted"])))
                    this.IsDeleted = Convert.ToBoolean(dtValue.Rows[0]["IsDeleted"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["EmployeeCode"])))
                    this.Empcode = Convert.ToString(dtValue.Rows[0]["EmployeeCode"]);

                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["RejReason"])))
                    this.Rejectreason = Convert.ToString(dtValue.Rows[0]["RejReason"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["CompOff"])))
                    this.CompOff = Convert.ToBoolean(dtValue.Rows[0]["CompOff"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dtValue.Rows[0]["IsappCancelReq"])))
                    this.IsApprvCancel = Convert.ToBoolean(dtValue.Rows[0]["IsappCancelReq"]);
            }

        }

        #endregion
        #region "Properties"
        public new Guid Id { get; set; }
        public Guid FinanceYear { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid DefaultLOPid { get; set; }
        public string NavTabStatus { get; set; }
        public double LeaveBalance { get; set; }
        public int HRentrystatus { get; set; }
        public double Useddays { get; set; }
        public double Totaldays { get; set; }
        public double Debitdays { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid LeaveType { get; set; }
        public Guid AssignmanagerId { get; set; }

        public string IsAttachReq { get; set; }
        public Guid ChildId { get; set; }
        public Guid Childid { get; set; }
        public int FromDay { get; set; }
        public int ToDay { get; set; }
        public int Status { get; set; }
        public int MANAGERStatus { get; set; }
        public string Rejectreason { get; set; }
        public string Reason { get; set; }
        public string Contact { get; set; }
        public string NoOfDays { get; set; }
        public double levcount { get; set; }
        public int CurMonth { get; set; }

        public int prioritynumber { get; set; }
        public int Leaveopening { get; set; }
        public double Leaveopen { get; set; }
        public int Leavecredits { get; set; }
        public double Leavecred { get; set; }

        public string Imgpath { get; set; }
        public int ApprovedBy { get; set; }
        public int RejectedBy { get; set; }
        public int EnteredBy { get; set; }

        public DateTime ApprovedOn { get; set; }
        public DateTime RejectedOn { get; set; }
        public string FirstLvlContact { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveTypeName { get; set; }
        public string LeaveStatus { get; set; }
        public string Empcode { get; set; }
        public string LeavingStation { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public double HFDay { get; set; }
        public int childflag { get; set; }
        public int HRapprovalflag { get; set; }

        public int compid { get; set; }
        public DateTime LevDate { get; internal set; }
        public bool IsApprvCancel { get; set; }
        #endregion





        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        #region"Leave Requesting"


        public string LeaveRequestcommonfieldscheck()
        {
            //CHECKING FINANCIAL YEAR
            if (this.FinanceYear == Guid.Empty)
            {
                return "false,200,Please Set the Financial Year,null";
            }
            AssignManager objAssMgrLst = new AssignManager(this.EmployeeId, this.compid, 0, this.FinanceYear);
            if (objAssMgrLst.Count <= 0)
            {
                return "false, 200,Please set the Assign manager before you Request, null)";
            }
            MailConfig mailConfig = new MailConfig(this.compid);
            if (string.IsNullOrEmpty(mailConfig.IPAddress) || string.IsNullOrEmpty(Convert.ToString(mailConfig.PortNo)) || string.IsNullOrEmpty(mailConfig.FromEmail))
            {
                return "false, 200,Please Set the Mail Configuration,null";
            }
            int Flevmonth = (Convert.ToDateTime(this.FromDate.ToString())).Month;
            int Flevyear = (Convert.ToDateTime(this.FromDate.ToString())).Year;
            DateTime FdtDate = new DateTime(Flevyear, Flevmonth, 1);
            string FMonthFullName = FdtDate.ToString("MMMM");
            int Tlevmonth = (Convert.ToDateTime(this.EndDate.ToString())).Month;
            int Tlevyear = (Convert.ToDateTime(this.EndDate.ToString())).Year;
            DateTime TdtDate = new DateTime(Tlevyear, Tlevmonth, 1);
            string TMonthFullName = TdtDate.ToString("MMMM");
            PayrollHistoryList payrollHistoryList11 = new PayrollHistoryList(this.EmployeeId, this.compid, Flevmonth, Flevyear);
            if (payrollHistoryList11.Count != 0)
            {
                if (this.DefaultLOPid == this.LeavetypeGUid)
                {
                    return "false, 400,Payrole Process has been completed for " + FMonthFullName + ", null";
                }
            }
            PayrollHistoryList payrollHistoryList12 = new PayrollHistoryList(this.EmployeeId, this.compid, Tlevmonth, Tlevyear);
            if (payrollHistoryList12.Count != 0)
            {
                if (this.DefaultLOPid == this.LeavetypeGUid)
                {
                    return "false, 400, Payrole Process has been completed for " + FMonthFullName + ", null";
                }
            }
            LeaveMasterList levmasterlist = new LeaveMasterList(this.compid, this.FinanceYear);
            if (levmasterlist.Count == 0)
            {
                return "false, 200, Please set the Leave master settings, null";
            }
            else
            {
                string weekoffsettingavailable = this.WeekoffAvailablecheck(levmasterlist);
                string[] values = weekoffsettingavailable.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = values[i].Trim();
                }
                if (values[0] == "false")
                {
                    return "false, 200, Your Weekoff Settings is not updated - Please contact your manager!!!, null";
                }
            }
            return "";
        }



        public string WeekoffAvailablecheck(LeaveMasterList levmasterlist)
        {
            Boolean weekoffexixtflag = false;
            string WeekoffParameter = levmasterlist[0].Weekoffparameter;
            string weekofentrytype = levmasterlist[0].Weekoffentryvalid;
            Employee employee = new Employee(this.EmployeeId);
            string parameterid = this.Parametersavailablecheck(WeekoffParameter, employee);

            string[] values = parameterid.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();
            }

            if (values[0] == "false")
            {
                return parameterid;
            }
            else
            {
                Guid temp = string.IsNullOrEmpty(Convert.ToString(values[1])) ? Guid.Empty : new Guid((values[1].ToString()));
                Guid ParameterGuid = temp;// new Guid((values[1].ToString()));
                LeaveSettingsBO Weekoffavailable = new LeaveSettingsBO();
                DataTable weekofftbl = Weekoffavailable.Checkweekoffforrequest(this.compid, this.FinanceYear, WeekoffParameter, ParameterGuid);
                DateTime Levfromdate = DateTime.Now;
                DateTime Levtodate = DateTime.Now;
                DateTime Levfromdate1 = DateTime.Now;
                DateTime Levtodate1 = DateTime.Now;
                if (weekofftbl.Rows.Count != 0)
                {
                    weekoffexixtflag = false;
                    if (weekofentrytype == "C")
                    {









                    }
                    else if (weekofentrytype == "M")
                    {
                        Boolean fromdateexist = false;
                        Boolean todateexist = false;
                        var start = this.StartMonth;
                        var end = this.EndMonth;
                        string[] diff = Enumerable.Range(0, 13).Select(a => start.AddMonths(a))
                                   .TakeWhile(a => a <= end)
                                   .Select(a => String.Concat(a.ToString("MM") + ", " + a.Year)).ToArray();
                        for (int k = 0; k <= diff.Count() - 1; k++)
                        {
                            string[] values1 = diff[k].ToString().Split(',');
                            for (int j = 0; j < values1.Length; j++)
                            {
                                values1[j] = values1[j].Trim();
                            }

                            string FinalMonth = values1[0].ToString();
                            int FinalYear = Convert.ToInt32(values1[1].ToString());

                            string reqmonthfrom = FromDate.ToString("MM");
                            string reqmonthto = EndDate.ToString("MM");
                            if (FinalMonth == reqmonthfrom)
                            {
                                Levfromdate = new DateTime(FinalYear, Convert.ToInt32(FinalMonth), 1);
                                Levtodate = Convert.ToDateTime(Levfromdate.AddMonths(1).AddDays(-1));
                            }
                            if (FinalMonth == reqmonthto)
                            {
                                Levfromdate1 = new DateTime(FinalYear, Convert.ToInt32(FinalMonth), 1);
                                Levtodate1 = Convert.ToDateTime(Levfromdate1.AddMonths(1).AddDays(-1));
                            }
                        }


                        for (int i = 0; i <= weekofftbl.Rows.Count - 1; i++)
                        {

                            if (Convert.ToDateTime(weekofftbl.Rows[i]["Fromdate"]) == Levfromdate && Convert.ToDateTime(weekofftbl.Rows[i]["Todate"]) == Levtodate)
                            {
                                fromdateexist = true;
                            }

                            if (Convert.ToDateTime(weekofftbl.Rows[i]["Fromdate"]) == Levfromdate1 && Convert.ToDateTime(weekofftbl.Rows[i]["Todate"]) == Levtodate1)
                            {
                                todateexist = true;
                            }

                        }

                        if (fromdateexist == true && todateexist == true)
                        {
                            weekoffexixtflag = true;
                        }
                    }
                    else
                    {
                        Levfromdate = this.StartMonth;
                        Levtodate = this.EndMonth;

                        for (int i = 0; i <= weekofftbl.Rows.Count - 1; i++)
                        {

                            if (Convert.ToDateTime(weekofftbl.Rows[i]["Fromdate"]) == Levfromdate && Convert.ToDateTime(weekofftbl.Rows[i]["Todate"]) == Levtodate)
                            {
                                weekoffexixtflag = true;
                            }

                        }

                    }

                    if (weekoffexixtflag == true)
                    {
                        return "true, 100,Weekoff Existing!!!, null";
                    }
                    else
                    {
                        return "false, 200, Your Weekoff settings is not Updated-Please contact your manager!!!, null";
                    }

                }
                else
                {
                    return "false, 200, Your Weekoff settings is not Updated-Please contact your manager!!!, null";
                }

            }
        }




        public string Parametersavailablecheck(string Parameter, Employee employeeDetails)
        {
            int CompanyId = employeeDetails.CompanyId;
            string Parameterid = string.Empty;

            switch (Parameter)
            {
                case "branch":
                    if (employeeDetails.Branch.ToString() != null)
                    {
                        Parameterid = employeeDetails.Branch.ToString();

                    }
                    else
                    {
                        return "false, 200, Please Update your branch before Requesting, null";
                    }

                    break;
                case "category":
                    if (employeeDetails.CategoryId.ToString() != null)
                    {
                        Parameterid = employeeDetails.CategoryId.ToString();

                    }
                    else
                    {
                        return "false, 200, Please Update your branch before Requesting, null";
                    }

                    break;

                case "designation":

                    if (employeeDetails.Designation.ToString() != null)
                    {
                        Parameterid = employeeDetails.Designation.ToString();
                    }
                    else
                    {
                        return "false, 200, Please Update your Designation before Requesting, null";
                    }

                    break;


                case "costCentre":

                    if (employeeDetails.CostCentre.ToString() != null)
                    {
                        Parameterid = employeeDetails.CostCentre.ToString();
                    }
                    else
                    {
                        return "false, 200, Please Update your CostCentre before Requesting, null";
                    }

                    break;

                case "esiLocation":
                    if (employeeDetails.ESILocation.ToString() != null)
                    {
                        Parameterid = employeeDetails.ESILocation.ToString();
                    }
                    else
                    {
                        return "false, 200, Please Update your EsiLocation before Requesting, null";
                    }

                    break;

                case "grade":
                    if (employeeDetails.Grade.ToString() != null)
                    {
                        Parameterid = employeeDetails.Grade.ToString();
                    }
                    else
                    {
                        return "false, 200, Please Update your Grade before Requesting, null";
                    }
                    break;

                case "esiDespensary":
                    if (employeeDetails.ESIDespensary.ToString() != null)
                    {
                        Parameterid = employeeDetails.ESIDespensary.ToString();
                    }
                    else
                    {
                        return "false, 200, Please Update your ESIDespensary before Requesting, null";
                    }

                    break;

                case "department":
                    if (employeeDetails.Department.ToString() != null)
                    {
                        Parameterid = employeeDetails.Department.ToString();
                    }
                    else
                    {
                        return "false, 200, Please Update your Department before Requesting, null";
                    }

                    break;

                case "location":
                    if (employeeDetails.Location.ToString() != null)
                    {
                        Parameterid = employeeDetails.Location.ToString();
                    }
                    else
                    {
                        return "false, 200, Please Update your Location before Requesting, null";
                    }

                    break;

                case "ptlocation":
                    if (employeeDetails.PTLocation.ToString() != null)
                    {
                        Parameterid = employeeDetails.PTLocation.ToString();
                    }
                    else
                    {
                        return "false, 200, Please Update your PTLocation before Requesting, null";
                    }


                    break;
                case "employeewise":
                    if (employeeDetails.Id.ToString() != null)
                    {
                        Parameterid = employeeDetails.Id.ToString();
                    }
                    else
                    {
                        return "false, 200, Please Update your PTLocation before Requesting, null";
                    }
                    break;

                case "bank":
                    //should not allow Bank in Master settings

                    break;

                case "leaveType":
                    //should not allow leaveType in Master settings
                    break;

                case "languagesknown":
                    //should not allow languagesknown in Master settings
                    break;

                case "companywise":
                    //There is no parameterid mapped for company wise setting.
                    break;

                default:
                    //Getting parameter id for dynamically mapped component in component creation and dynamic groups.
                    EntityModel objEntMod = new EntityModel(Parameter,CompanyId);
                    EntityMappingList Maplist = new EntityMappingList(string.Empty, employeeDetails.Id.ToString(), objEntMod.Id);
                    if (Maplist.Count==1)
                    {
                        Maplist.ForEach(p => {
                            Parameterid = p.EntityId.ToString();
                        });
                    }
                    //Console.WriteLine("Default case");
                    break;


            }

            return "true," + Parameterid;
        }









        #endregion





























        public DataTable getmanagertoapprovelist(Guid employeeId, int CompanyId, Guid finnyear)
        {
            SqlCommand sqlCommand = new SqlCommand("AssignMgrML_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", finnyear);
            sqlCommand.Parameters.AddWithValue("@ApprovMust", 1);
            sqlCommand.Parameters.AddWithValue("@Id", Guid.Empty);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public DataTable getalreadyapplieddates(Guid employeeId, int CompanyId, Guid finnyear)
        {
            SqlCommand sqlCommand = new SqlCommand("SP_CompoffRequest");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", finnyear);
            sqlCommand.Parameters.AddWithValue("@Type", "Select");
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public bool saveapprovemanagerrequestID(Guid managerid, Guid employeeid, int mrgprioritylevel, int compid, Guid finnnyear, Guid levreqid, Guid loggedinID)
        {

            SqlCommand sqlCommand = new SqlCommand("LevRequestLeader_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@CompanyId", compid);
            sqlCommand.Parameters.AddWithValue("@LeaveRequestId", levreqid);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeid);
            sqlCommand.Parameters.AddWithValue("@AssignManagerId", managerid);
            sqlCommand.Parameters.AddWithValue("@ManagerPriority", mrgprioritylevel);
            sqlCommand.Parameters.AddWithValue("@LeaveStatus", 0);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", loggedinID);
            sqlCommand.Parameters.AddWithValue("@FinId", finnnyear);

            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }


        public bool apprejsavethroughmail()
        {

            SqlCommand sqlCommand = new SqlCommand("ApproveorRejectThrough_Mail");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", this.FinanceYear);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@FromDate", this.FromDate);
            sqlCommand.Parameters.AddWithValue("@EndDate", this.EndDate);
            sqlCommand.Parameters.AddWithValue("@LeaveType", this.LeaveType);
            sqlCommand.Parameters.AddWithValue("@FromDay", this.FromDay);
            sqlCommand.Parameters.AddWithValue("@ToDay", this.ToDay);
            sqlCommand.Parameters.AddWithValue("@Reason", this.Reason);
            sqlCommand.Parameters.AddWithValue("@Contact", this.Contact);
            sqlCommand.Parameters.AddWithValue("@NoOfDays", this.NoOfDays);
            sqlCommand.Parameters.AddWithValue("@Status", this.Status);
            sqlCommand.Parameters.AddWithValue("@RejReason", this.Rejectreason);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.AddWithValue("@ApprovedBy", this.ApprovedBy);
            sqlCommand.Parameters.AddWithValue("@RejectedBy", this.RejectedBy);
            sqlCommand.Parameters.AddWithValue("@FirstLvlContact", this.FirstLvlContact);
            sqlCommand.Parameters.AddWithValue("@ChildID", this.Childid);
            sqlCommand.Parameters.AddWithValue("@ChildFlag", this.childflag);
            sqlCommand.Parameters.AddWithValue("@Companyid", this.compid);
            sqlCommand.Parameters.AddWithValue("@HRApprove", this.HRapprovalflag);
            sqlCommand.Parameters.AddWithValue("@HRentrystatus", this.HRentrystatus);
            sqlCommand.Parameters.AddWithValue("@EnteredBy", this.EnteredBy);

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
        public bool CompoffSave()
        {

            SqlCommand sqlCommand = new SqlCommand("CompoffRequest_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", this.FinanceYear);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@FromDate", this.FromDate);
            sqlCommand.Parameters.AddWithValue("@EndDate", this.EndDate);
            sqlCommand.Parameters.AddWithValue("@LeaveType", this.LeaveType);
            sqlCommand.Parameters.AddWithValue("@FromDay", this.FromDay);
            sqlCommand.Parameters.AddWithValue("@ToDay", this.ToDay);
            sqlCommand.Parameters.AddWithValue("@Reason", this.Reason);
            sqlCommand.Parameters.AddWithValue("@Contact", this.Contact);
            sqlCommand.Parameters.AddWithValue("@NoOfDays", this.NoOfDays);
            sqlCommand.Parameters.AddWithValue("@Status", this.Status);
            sqlCommand.Parameters.AddWithValue("@RejReason", this.Rejectreason);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.AddWithValue("@ApprovedBy", this.ApprovedBy);
            sqlCommand.Parameters.AddWithValue("@RejectedBy", this.RejectedBy);
            sqlCommand.Parameters.AddWithValue("@FirstLvlContact", this.FirstLvlContact);
            sqlCommand.Parameters.AddWithValue("@ChildID", this.Childid);
            sqlCommand.Parameters.AddWithValue("@ChildFlag", this.childflag);
            sqlCommand.Parameters.AddWithValue("@Companyid", this.compid);
            sqlCommand.Parameters.AddWithValue("@HRApprove", this.HRapprovalflag);
            sqlCommand.Parameters.AddWithValue("@HRentrystatus", this.HRentrystatus);
            sqlCommand.Parameters.AddWithValue("@EnteredBy", this.EnteredBy);

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


        #region Comp-Off Gain Request
        public bool CompOffGainhistorysave()
        {
            SqlCommand sqlCommand = new SqlCommand("Usp_CompOffGainHistroy");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", Guid.NewGuid());
            sqlCommand.Parameters.AddWithValue("@FinYrId", this.FinanceYear);
            sqlCommand.Parameters.AddWithValue("@EmpId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@CompOffDateReq", this.FromDate);
            sqlCommand.Parameters.AddWithValue("@HFday", this.HFDay);
            sqlCommand.Parameters.AddWithValue("@LevReqId", this.Id);
            sqlCommand.Parameters.AddWithValue("@ValidDate", this.EndDate);
            sqlCommand.Parameters.AddWithValue("@AvaliableDays", this.HFDay);
            sqlCommand.Parameters.AddWithValue("@IsApproved", false);
            sqlCommand.Parameters.AddWithValue("@Status", 0);
            sqlCommand.Parameters.AddWithValue("@Type", "INSERT");
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.save(sqlCommand);
            return status;
        }

        public bool CompOffGainRequestUpdate(string type)
        {
            SqlCommand sqlCommand = new SqlCommand("Usp_CompOffGainHistroy");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@LevReqId", this.Id);
            sqlCommand.Parameters.AddWithValue("@Status", this.Status);
            sqlCommand.Parameters.AddWithValue("@Type", type);
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.save(sqlCommand);
            return status;
        }
        public bool CompOffGainApproved()
        {
            SqlCommand sqlCommand = new SqlCommand("Usp_CompOffGainHistroy");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@LevReqId", this.Id);
            sqlCommand.Parameters.AddWithValue("@Type", "APPROVED");
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.save(sqlCommand);
            return status;
        }

        public bool CompOffGainUpdateAvaliableLeave()
        {
            SqlCommand sqlCommand = new SqlCommand("Usp_CompOffGainHistroy");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@AvaliableDays", this.HFDay);
            sqlCommand.Parameters.AddWithValue("@Type", "UPDATEAVALIABLEDAYS");
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.save(sqlCommand);
            return status;
        }

        public DataTable CompOffGainHistroySelect(Guid FinYrId, Guid LevReqId, Guid employeeId,Guid CompoffGainID)
        {
            SqlCommand sqlCommand = new SqlCommand("Usp_CompOffGainHistroy");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmpId", employeeId);
            sqlCommand.Parameters.AddWithValue("@LevReqId", LevReqId);
            sqlCommand.Parameters.AddWithValue("@Finyrid", FinYrId);
            sqlCommand.Parameters.AddWithValue("@Id", CompoffGainID);
            sqlCommand.Parameters.AddWithValue("@Type", "SELECT");
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        public bool CompOffGainHistroyValidDateUpdate(Guid FinYrId, Guid LevReqId, Guid employeeId)
        {
            SqlCommand sqlCommand = new SqlCommand("Usp_CompOffGainHistroy");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmpId", employeeId);
            sqlCommand.Parameters.AddWithValue("@LevReqId", LevReqId);
            sqlCommand.Parameters.AddWithValue("@Finyrid", FinYrId);
            sqlCommand.Parameters.AddWithValue("@Type", "UPDATEVALIDDATE");
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.save(sqlCommand);
            return status;
        }

        public DataTable GetBalanceCompOff(Guid FinYrId, Guid employeeId)
        {
            SqlCommand sqlCommand = new SqlCommand("Usp_CompOffGainHistroy");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmpId", employeeId);
            sqlCommand.Parameters.AddWithValue("@Finyrid", FinYrId);
            sqlCommand.Parameters.AddWithValue("@Type", "GETBALANCECOMPOFF");
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        #endregion
        public new bool Save()
        {

            SqlCommand sqlCommand = new SqlCommand("LeaveRequest_Save");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", this.FinanceYear);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@FromDate", this.FromDate);
            sqlCommand.Parameters.AddWithValue("@EndDate", this.EndDate);
            sqlCommand.Parameters.AddWithValue("@LeaveType", this.LeaveType);
            sqlCommand.Parameters.AddWithValue("@FromDay", this.FromDay);
            sqlCommand.Parameters.AddWithValue("@ToDay", this.ToDay);
            sqlCommand.Parameters.AddWithValue("@Reason", this.Reason);
            sqlCommand.Parameters.AddWithValue("@Contact", this.Contact);
            sqlCommand.Parameters.AddWithValue("@NoOfDays", this.NoOfDays);
            sqlCommand.Parameters.AddWithValue("@Status", this.Status);
            sqlCommand.Parameters.AddWithValue("@RejReason", this.Rejectreason);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.AddWithValue("@ApprovedBy", this.ApprovedBy);
            sqlCommand.Parameters.AddWithValue("@RejectedBy", this.RejectedBy);
            sqlCommand.Parameters.AddWithValue("@FirstLvlContact", this.FirstLvlContact);
            sqlCommand.Parameters.AddWithValue("@ChildID", this.Childid);
            sqlCommand.Parameters.AddWithValue("@ChildFlag", this.childflag);
            sqlCommand.Parameters.AddWithValue("@Companyid", this.compid);
            sqlCommand.Parameters.AddWithValue("@HRApprove", this.HRapprovalflag);
            sqlCommand.Parameters.AddWithValue("@HRentrystatus", this.HRentrystatus);
            sqlCommand.Parameters.AddWithValue("@EnteredBy", this.EnteredBy);
            sqlCommand.Parameters.AddWithValue("@Imgpath", this.Imgpath);
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

        public bool SaveApprovedcancelResponse()
        {
            SqlCommand sqlCommand = new SqlCommand("SP_ApprovedLeaveCancel");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@Status", this.Status);
            sqlCommand.Parameters.AddWithValue("@RejReason", this.Rejectreason);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@Type", "RequestResponse");
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.save(sqlCommand);
            return status;
        }

        public bool SaveApprovedLeaveCancelRequest()
        {

            SqlCommand sqlCommand = new SqlCommand("SP_ApprovedLeaveCancel");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", this.FinanceYear);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", this.EmployeeId);
            sqlCommand.Parameters.AddWithValue("@FromDate", this.FromDate);
            sqlCommand.Parameters.AddWithValue("@EndDate", this.EndDate);
            sqlCommand.Parameters.AddWithValue("@LeaveType", this.LeaveType);
            sqlCommand.Parameters.AddWithValue("@FromDay", this.FromDay);
            sqlCommand.Parameters.AddWithValue("@ToDay", this.ToDay);
            sqlCommand.Parameters.AddWithValue("@Reason", this.Reason);
            sqlCommand.Parameters.AddWithValue("@Contact", this.Contact);
            sqlCommand.Parameters.AddWithValue("@NoOfDays", this.NoOfDays);
            sqlCommand.Parameters.AddWithValue("@Status", this.Status);
            sqlCommand.Parameters.AddWithValue("@RejReason", this.Rejectreason);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            sqlCommand.Parameters.AddWithValue("@CreatedBy", this.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            sqlCommand.Parameters.AddWithValue("@ApprovedBy", this.ApprovedBy);
            sqlCommand.Parameters.AddWithValue("@RejectedBy", this.RejectedBy);
            sqlCommand.Parameters.AddWithValue("@FirstLvlContact", this.FirstLvlContact);
            sqlCommand.Parameters.AddWithValue("@ChildID", this.Childid);
            sqlCommand.Parameters.AddWithValue("@ChildFlag", this.childflag);
            sqlCommand.Parameters.AddWithValue("@Companyid", this.compid);
            sqlCommand.Parameters.AddWithValue("@HRApprove", this.HRapprovalflag);
            sqlCommand.Parameters.AddWithValue("@HRentrystatus", this.HRentrystatus);
            sqlCommand.Parameters.AddWithValue("@EnteredBy", this.EnteredBy);
            sqlCommand.Parameters.AddWithValue("@Imgpath", this.Imgpath);
            sqlCommand.Parameters.AddWithValue("@Type", "SaveRequest");
            DBOperation dbOperation = new DBOperation();
            string status = string.Empty;
            return dbOperation.SaveData(sqlCommand, out status);
        }


        public bool SaveAbsentTable(string xmlResult)
        {
            SqlCommand sqlCommand = new SqlCommand("sp_XmlSave_LeaveAbsent");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@xmlstring", xmlResult);
            DBOperation dbOperation = new DBOperation();
            string status = string.Empty;
            return dbOperation.SaveData(sqlCommand, out status);
        }

        public DataTable GetMRTakenlevCheck(Guid Levtypeid, int CompanyId, Guid finaanceyr, Guid employeeId)
        {
            SqlCommand sqlCommand = new SqlCommand("LevResTakenEmpcheck_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@Levtypeid", Levtypeid);
            sqlCommand.Parameters.AddWithValue("@Finyrid", finaanceyr);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        public new bool Delete()
        {

            SqlCommand sqlCommand = new SqlCommand("LeaveRequest_Delete");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", this.Id);
            sqlCommand.Parameters.AddWithValue("@ModifiedBy", this.ModifiedBy);
            //sqlCommand.Parameters.AddWithValue("@IsDeleted", this.IsDeleted);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.DeleteData(sqlCommand);
        }
        internal DataTable GetTableValuesCompoff(Guid employeeId, int CompanyId, string type)
        {
            SqlCommand sqlCommand = new SqlCommand("LeaveEmpRequest_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@ReqType", type);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        internal DataTable GetTableValuesfh(Guid employeeId, int CompanyId, string type)
        {
            SqlCommand sqlCommand = new SqlCommand("floatingHoliday_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@ReqType", type);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }

        internal DataTable GetSingleLeaverequestvalue(Guid Requestid)
        {
            SqlCommand sqlCommand = new SqlCommand("LeaveReqSingle_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@LeaveRequestID", Requestid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        internal DataTable GetTableValues(Guid employeeId, int CompanyId)
        {
            SqlCommand sqlCommand = new SqlCommand("LeaveEmpRequest_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public DataTable GetAssignManagerViewReport(DateTime from, DateTime to, Guid EmployeeId, int LevStat, Guid FinYear, Guid LoggedUser)
        {
            SqlCommand sqlCommand = new SqlCommand("AssignManagerReport_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FromDate", from);
            sqlCommand.Parameters.AddWithValue("@EndDate", to);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeId);
            sqlCommand.Parameters.AddWithValue("@LeaveStatus", LevStat);
            sqlCommand.Parameters.AddWithValue("@FinYear", FinYear);
            sqlCommand.Parameters.AddWithValue("@AssignManagerId", LoggedUser);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public  DataTable  CheckLeave(Guid EmployeeId ,DateTime FromDate)
        {
            DataTable dt = new DataTable();
            using ( SqlCommand sqlCommand = new SqlCommand("SELECT * FROM LeaveRequest WHERE  EmployeeId= @Empd and FromDate = @fromDate  and Status !=3"))
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@Empd", EmployeeId);
                sqlCommand.Parameters.AddWithValue("@fromDate", FromDate);
                DBOperation dbOperation = new DBOperation();
                dt = dbOperation.GetTableData(sqlCommand);
            }
            return dt;
        }
        public DataTable GetMYREPORTLeaveReport(DateTime from, DateTime to, Guid EmployeeId, int LevStat, int compid)
        {
            SqlCommand sqlCommand = new SqlCommand("employeeleaveREPORT_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FromDate", from);
            sqlCommand.Parameters.AddWithValue("@EndDate", to);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeId);
            sqlCommand.Parameters.AddWithValue("@LeaveStatus", LevStat);
            sqlCommand.Parameters.AddWithValue("@CompanyId", compid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        internal DataTable GetValueforMYREPORT(Guid employeeId, int CompanyId, Guid finyear)
        {
            SqlCommand sqlCommand = new SqlCommand("MYREPORT_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@Finyear", finyear);

            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        internal DataTable GetApprovecancelreportdatas(Guid employeeId, int CompanyId, Guid finyear, string type)
        {
            SqlCommand sqlCommand = new SqlCommand("MYREPORT_AppCanceReport_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@Finyear", finyear);
            sqlCommand.Parameters.AddWithValue("@Type", type);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        internal DataTable Checkingforleavedelete(Guid Leavetypeid)
        {
            SqlCommand sqlCommand = new SqlCommand("LeaveTypeDelete_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@LeaveTypeId", Leavetypeid);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        internal DataTable GetLeaveDeleteData(int CompanyId)
        {
            SqlCommand sqlCommand = new SqlCommand("LeaveDeletecheck_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            //  sqlCommand.Parameters.AddWithValue("@Id", id);
            // sqlCommand.Parameters.AddWithValue("@FinanceYear", this.FinanceYear);
            //sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            //if (this.FromDate != DateTime.MinValue)
            //    sqlCommand.Parameters.AddWithValue("@FromDate", this.FromDate);
            //if (this.EndDate != DateTime.MinValue)
            //    sqlCommand.Parameters.AddWithValue("@EndDate", this.EndDate);
            //sqlCommand.Parameters.AddWithValue("@Status", this.Status);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        internal DataTable GetemployeeRequestedvalue(Guid employeeId, int CompanyId, Guid finnancialyear, string ReqType)
        {
            SqlCommand sqlCommand = new SqlCommand("LeaveAssignEmp_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            //  sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", finnancialyear);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@ReqType", ReqType);
            //if (this.FromDate != DateTime.MinValue)
            //    sqlCommand.Parameters.AddWithValue("@FromDate", this.FromDate);
            //if (this.EndDate != DateTime.MinValue)
            //    sqlCommand.Parameters.AddWithValue("@EndDate", this.EndDate);
            //sqlCommand.Parameters.AddWithValue("@Status", this.Status);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        internal DataTable GetpreviouspriortyRequestedvalue(Guid id, int priority)
        {
            SqlCommand sqlCommand = new SqlCommand("LevLeaderPriority_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            //  sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@REQID", id);
            sqlCommand.Parameters.AddWithValue("@PRIORITYNUM", priority);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public DataTable updatelevreqleadertbl(Guid assignmgrid, Guid REQID, int APPorRejStatus, Guid LoginUserid, int HRflag)
        {
            SqlCommand sqlCommand = new SqlCommand("Leavereqleader_Update");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@REQID", REQID);
            sqlCommand.Parameters.AddWithValue("@Assignmgrid", assignmgrid);
            sqlCommand.Parameters.AddWithValue("@APPorRejStatus", APPorRejStatus);
            sqlCommand.Parameters.AddWithValue("@LoginUserid", LoginUserid);
            sqlCommand.Parameters.AddWithValue("@HRflag", HRflag);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        public DataTable checkfornextproritynum(int prioritynum, Guid REQID)
        {
            SqlCommand sqlCommand = new SqlCommand("LevLeaderNextPriority_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            //  sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@REQID", REQID);
            sqlCommand.Parameters.AddWithValue("@prioritynum", prioritynum);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        internal DataTable GetTableValues(Guid Id, Guid EmployeeId)
        {
            SqlCommand sqlCommand = new SqlCommand("LeaveRequestMail_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", Id);
            // sqlCommand.Parameters.AddWithValue("@FinanceYear", this.FinanceYear);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        internal DataTable GetTableValues()
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand("LeaveRequest_Select");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", this.Id);

                sqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeId);

                DBOperation dbOperation = new DBOperation();
                return dbOperation.GetTableData(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //--------------------------
        internal DataTable Getavailabledebitdays()
        {
            SqlCommand sqlCommand = new SqlCommand("Debitdaysforlevreq_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Leavetypeid", this.LeaveType);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", this.FinanceYear);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
        //--------------------------
        //--------------------------

        internal DataTable Getleavebalance()
        {
            SqlCommand sqlCommand = new SqlCommand("LeaveBalance_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Leavetypeid", this.LeaveType);
            sqlCommand.Parameters.AddWithValue("@FinanceYear", this.FinanceYear);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeId);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }


        //--------------------------
        internal DataTable GetAbsentTableValues(Guid FinanceYear, int CompanyId, int month, int year)
        {
            SqlCommand sqlCommand = new SqlCommand("LeaveAbsent_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@FinanceYear", FinanceYear);
            sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
            sqlCommand.Parameters.AddWithValue("@Month", month);
            sqlCommand.Parameters.AddWithValue("@Year", year);
            //sqlCommand.Parameters.AddWithValue("@Status", this.Status);
            DBOperation dbOperation = new DBOperation();
            return dbOperation.GetTableData(sqlCommand);
        }
    }

}

