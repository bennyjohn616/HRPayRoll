using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PayrollBO
{
    public class EmployeeRole
    {
        #region "Constructor"
        public EmployeeRole()
        {

        }
        #endregion
        #region "Properties"
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string DisplayAs { get; set; }
        public int RoleId { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public int EmpStatus { get; set; }

        #endregion

        #region "Methods"

        public List<EmployeeRole> role(int companyid)
        {
            var usercomplist=new List<User>();
            var finalusercomplist = new List<User>();
            EmployeeRole ObjEmpRols = new EmployeeRole();
            List<EmployeeRole> ERLst = new List<EmployeeRole>();
            DataTable dtRolSel = new DataTable();
            UserCompanymapping ObjCompMap = new UserCompanymapping();
            dtRolSel = ObjCompMap.GetPayrolRole();
            //getting the user company mapping list 
            UserCompanymappingList CompList = new UserCompanymappingList(0);
            //adding the where condition in the list and getting only the users belongs to this company 
            var usercomp = CompList.Where(d => d.CompanyId == companyid).ToList();
            //Getting the total user list from the login DB
            UserList user = new UserList(0);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyid);

            //geting the manager roles assigned from the manager eligibility table 
            ManagerEligiblityList ObjMGRList = new ManagerEligiblityList(companyid, DefaultFinancialYr.Id);


            var usermappingid = CompList.Where(d => d.CompanyId == companyid).Select(e => e.UserId);
            usercomplist.AddRange(user.Where(r => usermappingid.Contains(r.Id)).ToList());


            var getingmanagers = ObjMGRList.Select(e => e.RoleId);
            finalusercomplist.AddRange(usercomplist.Where(r => getingmanagers.Contains(r.UserRole)).ToList());



            finalusercomplist.ForEach(r =>
            {
                EmployeeRole ObjEmpRol = new EmployeeRole();
                if (!string.IsNullOrEmpty(r.EmployeeId.ToString()))
                {
                    Employee Objemp = new Employee(r.EmployeeId);
                    ObjEmpRol.Id = r.EmployeeId;
                    ObjEmpRol.FirstName = Objemp.FirstName;
                    ObjEmpRol.LastName = Objemp.LastName;
                    ObjEmpRol.FullName = Objemp.FirstName + " " + Objemp.LastName;
                    ObjEmpRol.EmployeeCode = Objemp.EmployeeCode;
                    ObjEmpRol.EmpStatus = Objemp.Status;
                }
                if (!string.IsNullOrEmpty(r.UserRole.ToString()))
                {
                   
                            for (int i = 0; i < dtRolSel.Rows.Count; i++)
                            {
                                if (r.UserRole == Convert.ToInt32(dtRolSel.Rows[i]["Id"]))
                                {
                            ObjEmpRol.DisplayAs = dtRolSel.Rows[i]["DisplayAs"].ToString();
                            ObjEmpRol.RoleId = Convert.ToInt32(dtRolSel.Rows[i]["Id"]);
                                }
                            }
                       
                    
                }
                ObjEmpRol.UserId = r.Id;
                ERLst.Add(ObjEmpRol);
            });
            return ERLst;
        }


        public List<EmployeeRole> role(int companyid,int temp)
        {
            var usercomplist = new List<User>();
            var userrolelist = new List<User>();
            EmployeeRole ObjEmpRols = new EmployeeRole();
            List<EmployeeRole> ERLst = new List<EmployeeRole>();
            DataTable dtRolSel = new DataTable();
            UserCompanymapping ObjCompMap = new UserCompanymapping();
            dtRolSel = ObjCompMap.GetPayrolRole();
            //getting the user company mapping list 
            UserCompanymappingList CompList = new UserCompanymappingList(0);
            //adding the where condition in the list and getting only the users belongs to this company 
            var usercomp = CompList.Where(d => d.CompanyId == companyid).ToList();
            //Getting the total user list from the login DB
            UserList user = new UserList(0);
            LeaveFinanceYear DefaultFinancialYr = new LeaveFinanceYear(companyid);

            //geting the manager roles assigned from the manager eligibility table 
            ManagerEligiblityList ObjMGRList = new ManagerEligiblityList(companyid, DefaultFinancialYr.Id);


            var usermappingid = CompList.Where(d => d.CompanyId == companyid).Select(e => e.UserId);
            usercomplist.AddRange(user.Where(r => usermappingid.Contains(r.Id)).ToList());
            var roletypes = usercomplist.Select(h => h.UserRole).Distinct().ToList();
           
            foreach (int value in roletypes)
            {
                for (int i=0;i< dtRolSel.Rows.Count;i++)
                {
                    EmployeeRole ObjEmpRol = new EmployeeRole();
                    if (value == Convert.ToInt32(dtRolSel.Rows[i]["Id"]))
                    {
                        ObjEmpRol.DisplayAs = dtRolSel.Rows[i]["DisplayAs"].ToString();
                        ObjEmpRol.RoleId = Convert.ToInt32(dtRolSel.Rows[i]["Id"]);
                        ERLst.Add(ObjEmpRol);
                    }
                }
               
            }
            return ERLst;
        }

        #endregion
    }
}
